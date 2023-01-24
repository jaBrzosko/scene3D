using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scene3D.Objects;
using Scene3D.Lights;
using System.Numerics;


namespace Scene3D.Helper
{
    internal class Drawer
    {
        private int width;
        private int height;
        private int halfHeight;
        private int halfWidth;
        private float[,] zBuffor;
        private object[,] zMutex;
        private float kd = 1;
        private float ks = 1;
        private float m = 50;
        private float fogMinZ = 0.97f;
        private float fogMaxZ = 0.99f;
        public Drawer(int width, int height)
        {
            this.width = width;
            this.height = height;
            halfHeight = height / 2;
            halfWidth = width / 2;
            zBuffor = new float[width, height];
            zMutex = new object[width, height];
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    zMutex[i, j] = new object();
                }
            }
        }

        public void Reset()
        {
            float max = float.MinValue;
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    if(zBuffor[i, j] > max)
                    {
                        max = zBuffor[i, j];
                    }
                    zBuffor[i, j] = float.MaxValue;
                }
            }
        }

        public void Draw(FastBitmap fastBitmap, Triangle triangle, Matrix4x4 modelMatrix, Vector3 vectorColor, Vector3 cameraPos, bool interpolateColor, bool showFog)
        {

            // triangle in local
            var localTriangle = triangle.SimpleRotate(modelMatrix);

            // Back face culling
            int possible = 3;
            int possibleClip = 3;
            for (int i = 0; i < 3; i++)
            {
                if (Math.Abs(triangle.rotatedPositions[i].X) > 1 
                    || Math.Abs(triangle.rotatedPositions[i].Y) > 1)
                {
                    possibleClip--;
                }
                //Vector3 v0 = triangle.rotatedPositions[i];
                Vector3 v0 = localTriangle[i].position;
                //v0.X *= fastBitmap.Width;
                //v0.Y *= fastBitmap.Height;
                if (Vector3.Dot(v0 - cameraPos, Vector3.Normalize(localTriangle[i].normal)) > 0)
                //if (Vector3.Dot(v0 - cameraPos, Vector3.Normalize(triangle.rotatedNormals[i])) > 0)
                {
                    possible--;
                }
            }
            if (possible == 0 || possibleClip == 0)
                return;

            int[] ind = new int[3];
            int[] yval = new int[3];
            for (int i = 0; i < 3; i++)
            {
                ind[i] = i;
                yval[i] = (int)(triangle.rotatedPositions[i].Y * halfWidth + halfWidth);
            }

            for (int i = 0; i < 3; i++)
            {
                int indexMin = i;
                for (int j = i + 1; j < 3; j++)
                {
                    if (yval[j] < yval[indexMin])
                    {
                        indexMin = j;
                    }
                }
                if (indexMin != i)
                {
                    (ind[i], ind[indexMin]) = (ind[indexMin], ind[i]);
                    (yval[i], yval[indexMin]) = (yval[indexMin], yval[i]);
                }
            }

            int y = yval[0];
            int yMax = yval[2];

            float x0 = (1 + triangle.rotatedPositions[ind[0]].X) * halfWidth;
            float x1 = (1 + triangle.rotatedPositions[ind[1]].X) * halfWidth;
            float x2 = (1 + triangle.rotatedPositions[ind[2]].X) * halfWidth;

            float y0 = (1 + triangle.rotatedPositions[ind[0]].Y) * halfHeight;
            float y1 = (1 + triangle.rotatedPositions[ind[1]].Y) * halfHeight;
            float y2 = (1 + triangle.rotatedPositions[ind[2]].Y) * halfHeight;

            var aetp0 = new AETP(x0, y0, x1, y1);
            var aetp1 = new AETP(x0, y0, x2, y2);

            var lights = LightSingleton.GetInstance();
            Vector3[] cornerColors = new Vector3[3];
            if(interpolateColor)
            {
                for(int i = 0; i < 3; i++)
                {
                    cornerColors[i] = GetVectorColor(localTriangle[i].position,
                                            localTriangle[i].normal, vectorColor, cameraPos, lights);
                }
            }

            for(y+=0; y < Math.Min(yMax, fastBitmap.Height); y++)
            {
                if(y == aetp0.YMax)
                {
                    aetp0 = new AETP(x1, y1, x2, y2);
                }
                if(y == aetp1.YMax)
                {
                    aetp1 = new AETP(x1, y1, x2, y2);
                }
                if(y >= 0)
                {
                    int xp1 = aetp0.X;
                    int xp0 = aetp1.X;
                    if (xp0 > xp1)
                        (xp0, xp1) = (xp1, xp0);
                    FillAETP(xp0, xp1, y, fastBitmap, vectorColor, triangle, lights, cornerColors, interpolateColor, cameraPos, localTriangle, showFog);
                }

                aetp0.Next();
                aetp1.Next();
            }
        }

        private void FillAETP(int x0, int x1, int y, FastBitmap fastBitmap, Vector3 vectorColor, Triangle triangle, 
            List<Light> lights, Vector3[] cornerColors, bool interpolateColor, Vector3 cameraPos, (Vector3 pos, Vector3 n)[] localTriangle, bool showFog)
        {
            for (int x = Math.Max(x0, 0); x <= Math.Min(x1, fastBitmap.Width - 1); x++)
            {
                float z = triangle.InterpolateZ(x, y, width, height);
                lock (zMutex[x, y])
                {

                    if (z >= zBuffor[x, y])
                    {
                        continue;
                    }
                    zBuffor[x, y] = z;
                    if (interpolateColor)
                    {
                        var color = GetColorCornerInterpolated(triangle.GetBarycentricCoordinates(x, y, width, height), cornerColors);
                        if (showFog)
                            color = GetFoggedColor(color, z);
                        fastBitmap.SetPixel(x, y, color);
                    }
                    else
                    {
                        var bar = triangle.GetBarycentricCoordinates(x, y, width, height);
                        //var normal = triangle.rotatedNormals[0] * bar.X + triangle.rotatedNormals[1] * bar.Y + triangle.rotatedNormals[2] * bar.Z;
                        var normal = localTriangle[0].n * bar.X + localTriangle[1].n * bar.Y + localTriangle[2].n * bar.Z;
                        var position = localTriangle[0].pos * bar.X + localTriangle[1].pos * bar.Y + localTriangle[2].pos * bar.Z;
                        var color = ColorFromVector(GetVectorColor(position, normal, vectorColor, cameraPos, lights));
                        if (showFog)
                            color = GetFoggedColor(color, z);
                        fastBitmap.SetPixel(x, y, color);
                    }
                }
            }
        }

        private Color GetFoggedColor(Color color, float z)
        {
            if (z <= fogMinZ)
                return color;
            if (z >= fogMaxZ)
                return Color.White;
            float coef = MathF.Cos((z - fogMinZ) * (MathF.PI / 2) / (fogMaxZ - fogMinZ));
            //float coef = Math.Clamp((z - fogMinZ) / (fogMaxZ - fogMinZ), 0, 1);
            coef *= MathF.PI / 2;
            coef = MathF.Pow(MathF.Cos(coef), 2);
            int R = Math.Clamp((int)(color.R * (1 - coef) + 255 * coef), 0, 255);
            int G = Math.Clamp((int)(color.G * (1 - coef) + 255 * coef), 0, 255);
            int B = Math.Clamp((int)(color.B * (1 - coef) + 255 * coef), 0, 255);
            return Color.FromArgb(R, G, B);
        }

        private Color ColorFromVector(Vector3 color)
        {
            return Color.FromArgb(Math.Min(Math.Max((int)(color.X * 255), 0), 255),
                                  Math.Min(Math.Max((int)(color.Y * 255), 0), 255),
                                  Math.Min(Math.Max((int)(color.Z * 255), 0), 255));
        }

        private Color GetColorCornerInterpolated(Vector3 barycentric, Vector3[] cornerColors)
        {
            Vector3 color = Vector3.Zero;

            color += barycentric.X * cornerColors[0];
            color += barycentric.Y * cornerColors[1];
            color += barycentric.Z * cornerColors[2];

            return ColorFromVector(color);
        }

        private Vector3 GetVectorColor(Vector3 position, Vector3 normal, Vector3 objectColor, Vector3 cameraPos, List<Light> lights)
        {
            Vector3 outColor = Vector3.Zero;
            normal = Vector3.Normalize(normal);
            foreach(var light in lights)
            {
                Vector3 V = cameraPos - position;
                V = Vector3.Normalize(V);
                Vector3 L = light.GetWorldPosition() - position;
                L = Vector3.Normalize(L);
                float dotLN = Vector3.Dot(L, normal);
                Vector3 R = 2 * dotLN * normal - L;
                R = Vector3.Normalize(R);
                float dotVR = Vector3.Dot(R, V);

                var thisColor = objectColor * light.GetLightColor(L, m);

                outColor += thisColor * (Math.Max(dotLN, 0) * kd +  MathF.Pow(Math.Max(dotVR, 0), m) * ks);
            }

            return outColor;
        }
    }
}