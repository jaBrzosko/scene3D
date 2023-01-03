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
        private float kd = 1;
        private float ks = 1;
        private float m = 50;
        public Drawer(int width, int height)
        {
            this.width = width;
            this.height = height;
            halfHeight = height / 2;
            halfWidth = width / 2;
            zBuffor = new float[height, width];
        }

        public void Reset()
        {
            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    zBuffor[i, j] = float.MaxValue;
                }
            }
        }

        public void Draw(FastBitmap fastBitmap, Triangle triangle, Vector3 vectorColor, Vector3 cameraPos)
        {
            // Back face culling
            bool acceptable = false;
            for(int i = 0; i < 3; i++)
            {
                Vector3 v0 = triangle.rotatedPositions[i];
                v0.X *= fastBitmap.Width;
                v0.Y *= fastBitmap.Height;
                if (Vector3.Dot(v0 - cameraPos, Vector3.Normalize(triangle.rotatedNormals[i])) < 0)
                {
                    acceptable = true;
                    break;
                }
            }
            //if (!acceptable)
            //    return;

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
            for(int i = 0; i < 3; i++)
            {
                cornerColors[i] = GetVectorColor(new Vector3(triangle.rotatedPositions[ind[i]].X * halfWidth + halfWidth,
                                                             triangle.rotatedPositions[ind[i]].Y * halfHeight + halfHeight,
                                                             triangle.rotatedPositions[ind[i]].Z),
                                        triangle.rotatedNormals[i], vectorColor, cameraPos, lights);
            }

            for(y+=0; y < yMax; y++)
            {
                if(y == aetp0.YMax)
                {
                    aetp0 = new AETP(x1, y1, x2, y2);
                }
                if(y == aetp1.YMax)
                {
                    aetp1 = new AETP(x1, y1, x2, y2);
                }

                int xp1 = aetp0.X;
                int xp0 = aetp1.X;
                if (xp0 > xp1)
                    (xp0, xp1) = (xp1, xp0);
                FillAETP(xp0, xp1, y, fastBitmap, vectorColor, triangle, lights, cornerColors);

                aetp0.Next();
                aetp1.Next();
            }
        }

        private void FillAETP(int x0, int x1, int y, FastBitmap fastBitmap, Vector3 vectorColor, Triangle triangle, List<Light> lights, Vector3[] cornerColors)
        {
            for(int x = x0; x <= x1; x++)
            {
                if(x < 0 || x >= fastBitmap.Width || y < 0 || y >= fastBitmap.Height)
                    continue;
                float z = triangle.InterpolateZ(x, y, width, height);
                if (z >= zBuffor[x, y])
                    continue;
                zBuffor[x, y] = z;
                fastBitmap.SetPixel(x, y, GetColorCornerInterpolated(triangle.GetBarycentricCoordinates(x, y, width, height), cornerColors));
            }
        }

        private Color GetColorCornerInterpolated(Vector3 barycentric, Vector3[] cornerColors)
        {
            Vector3 color = Vector3.Zero;

            color += barycentric.X * cornerColors[0];
            color += barycentric.Y * cornerColors[1];
            color += barycentric.Z * cornerColors[2];

            return Color.FromArgb(Math.Min(Math.Max((int)(color.X * 255), 0), 255), 
                                  Math.Min(Math.Max((int)(color.Y * 255), 0), 255), 
                                  Math.Min(Math.Max((int)(color.Z * 255), 0), 255));
        }

        private Vector3 GetVectorColor(Vector3 position, Vector3 normal, Vector3 objectColor, Vector3 cameraPos, List<Light> lights)
        {
            Vector3 outColor = Vector3.Zero;
            normal = Vector3.Normalize(normal);
            foreach(var light in lights)
            {
                var thisColor = objectColor * light.LightColor;
                Vector3 V = cameraPos - position;
                V = Vector3.Normalize(V);
                Vector3 L = light.WorldPosition - position;
                L = Vector3.Normalize(L);
                float dotLN = Vector3.Dot(L, normal);
                Vector3 R = 2 * dotLN * normal - L;
                R = Vector3.Normalize(R);
                float dotVR = Vector3.Dot(R, V);

                outColor += thisColor * (dotLN * kd +  MathF.Pow(dotVR, m) * ks);
            }

            return outColor;
        }
    }
}