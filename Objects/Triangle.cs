using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Scene3D.Objects
{
    internal class Triangle
    {
        public Vector3[] positions;
        public Vector3[] normals;

        public Vector3[] rotatedPositions;
        public Vector3[] rotatedNormals;

        public Triangle(Vector3[] positions, Vector3[] normals)
        {
            this.positions = positions;
            this.normals = normals;

            rotatedPositions = new Vector3[positions.Length];
            positions.CopyTo(rotatedPositions, 0);
            rotatedNormals = new Vector3[normals.Length];
            normals.CopyTo(rotatedNormals, 0);
        }

        public Triangle(Triangle triangle)
        {
            positions = new Vector3[triangle.positions.Length];
            normals = new Vector3[triangle.normals.Length];
            rotatedPositions = new Vector3[triangle.rotatedPositions.Length];
            rotatedNormals = new Vector3[triangle.rotatedNormals.Length];

            triangle.positions.CopyTo(positions, 0);
            triangle.normals.CopyTo(normals, 0);
            triangle.rotatedPositions.CopyTo(rotatedPositions, 0);
            triangle.rotatedNormals.CopyTo(rotatedNormals, 0);
        }

        public void Rotate(Matrix4x4 PVM, Matrix4x4 modelMatrix)
        {
            for(int i = 0; i < positions.Length; i++)
            {
                rotatedNormals[i] = Vector3.TransformNormal(normals[i], modelMatrix);
                var temp = Vector4.Transform(positions[i], PVM);
                temp /= temp.W;
                rotatedPositions[i].X = temp.X;
                rotatedPositions[i].Y = temp.Y;
                rotatedPositions[i].Z = temp.Z;
            }
        }

        public float InterpolateZ(int x, int y, int width, int height)
        {
            var cords = GetBarycentricCoordinates(x, y, width, height);
            var retZ = rotatedPositions[0].Z * cords.X + rotatedPositions[1].Z * cords.Y + rotatedPositions[2].Z * cords.Z;
            if (retZ > rotatedPositions[0].Z && retZ > rotatedPositions[1].Z && retZ > rotatedPositions[2].Z)
                return MathF.Max(MathF.Max(rotatedPositions[0].Z, rotatedPositions[1].Z), rotatedPositions[2].Z);
            if (retZ < rotatedPositions[0].Z && retZ < rotatedPositions[1].Z && retZ < rotatedPositions[2].Z)
                return MathF.Min(MathF.Min(rotatedPositions[0].Z, rotatedPositions[1].Z), rotatedPositions[2].Z);
            return retZ;
        }

        public Vector3 GetBarycentricCoordinates(float x, float y, int width, int height)
        {
            float x1 = (1 + rotatedPositions[0].X) * width / 2;
            float y1 = (1 + rotatedPositions[0].Y) * height / 2;
            float x2 = (1 + rotatedPositions[1].X) * width / 2;
            float y2 = (1 + rotatedPositions[1].Y) * height / 2;
            float x3 = (1 + rotatedPositions[2].X) * width / 2;
            float y3 = (1 + rotatedPositions[2].Y) * height / 2;

            float det = (y2 - y3) * (x1 - x3) + (x3 - x2) * (y1 - y3);

            float alpha = ((y2 - y3) * (x - x3) + (x3 - x2) * (y - y3)) / det;
            float beta = ((y3 - y1) * (x - x3) + (x1 - x3) * (y - y3)) / det;
            float gamma = 1 - alpha - beta;

            return new Vector3(alpha, beta, gamma);
        }
    }
}
