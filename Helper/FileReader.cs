using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scene3D.Objects;
using System.Numerics;

namespace Scene3D.Helper
{
    internal class FileReader
    {
        public static Model ReadObj(string path)
        {
            List<Vector3> positions = new List<Vector3>();
            List<Vector3> normalVectors = new List<Vector3>();
            List<List<Vector3>> faces = new List<List<Vector3>>();

            StreamReader sr = new StreamReader(path);

            string? ln;

            while ((ln = sr.ReadLine()) != null)
            {
                if (ln.StartsWith("v ")) // position
                {
                    Vector3? temp = ParseVector3(ln);
                    if (temp != null)
                    {
                        positions.Add((Vector3)temp);
                    }
                }
                else if (ln.StartsWith("vn ")) // normal vector
                {
                    Vector3? temp = ParseVector3(ln);
                    if (temp != null)
                    {
                        normalVectors.Add((Vector3)temp);
                    }
                }
                else if (ln.StartsWith("f ")) //faces
                {
                    List<Vector3>? temp = ParseFaces(ln);
                    if (temp != null && temp.Count > 2)
                    {
                        faces.Add(temp);
                    }
                }
            }
            sr.Close();

            return ConsolidateModel(positions, normalVectors, faces);
        }

        private static Vector3? ParseVector3(string line)
        {
            string[] comps = line.Split(" ");
            if (comps.Length != 4)
            {
                return null;
            }

            float[] temp = new float[3];
            for (int i = 1; i < 4; i++)
            {
                if (!float.TryParse(comps[i], out float x))
                {
                    return null;
                }

                temp[i - 1] = x;
            }
            return new Vector3(temp[0], temp[1], temp[2]);
        }

        private static List<Vector3>? ParseFaces(string line)
        {
            List<Vector3> faces = new List<Vector3>();
            string[] comps = line.Split(" ");

            foreach (string? comp in comps.Skip(1))
            {
                string[] face = comp.Split("/");
                if (face.Length != 3)
                {
                    return null;
                }

                int[] temp = new int[3];
                for (int i = 0; i < 3; i++)
                {
                    if (!int.TryParse(face[i], out int x))
                    {
                        if (i == 1)
                            continue;
                        return null;
                    }

                    temp[i] = x;
                }
                faces.Add(new Vector3(temp[0], temp[1], temp[2]));
            }

            return faces;
        }

        private static Model ConsolidateModel(List<Vector3> v, List<Vector3> vn, List<List<Vector3>> f)
        {
            List<Vector3> positions = new();
            List<Vector3> normals = new();
            List<Triangle> triangles = new();
            foreach (List<Vector3>? face in f)
            {
                foreach (Vector3 comp in face)
                {
                    positions.Add(v[(int)comp.X - 1]);
                    normals.Add(vn[(int)comp.Z - 1]);
                }
                Triangle temp = new Triangle(positions.ToArray(), normals.ToArray());
                triangles.Add(temp);
                positions.Clear();
                normals.Clear();
            }
            //var minX = model.polygons.Min(x => x.vertices.Min(y => y.vectorPosition.X));
            //var minY = model.polygons.Min(x => x.vertices.Min(y => y.vectorPosition.Y));
            //var maxX = model.polygons.Max(x => x.vertices.Max(y => y.vectorPosition.X));
            //var maxY = model.polygons.Max(x => x.vertices.Max(y => y.vectorPosition.Y));

            //var width = (maxX - minX) / 2;
            //var height = (maxY - minY) / 2;

            //foreach (var polygon in model.polygons)
            //{
            //    foreach (var vertex in polygon.vertices)
            //    {
            //        vertex.vectorPosition.X = vertex.vectorPosition.X / width;
            //        vertex.vectorPosition.Y = vertex.vectorPosition.Y / height;
            //    }
            //}

            return new Model(triangles.ToArray(), Vector3.Zero, Vector3.Zero, Vector3.Zero, new Scene3D.Movers.StationaryMover(Vector3.Zero), 1);
        }
    }
}

