using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Scene3D.Lights
{
    internal abstract class Light
    {
        public Vector3 LightColor { get; set; }

        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                WorldPosition = new Vector3(width * position.X, height * position.Y, position.Z);
            }
        }
        private Vector3 WorldPosition { get; set; }
        private Vector3 RotatedWorldPosition { get; set; }
        private int width;
        private int height;

        public Light(Vector3 lightColor, Vector3 position, int width, int height)
        {
            this.width = width;
            this.height = height;
            LightColor = lightColor;
            Position = position;
        }

        public abstract Vector3 GetLightColor(Vector3 L, float m);
        public Vector3 GetWorldPosition()
        {
            return RotatedWorldPosition;

        }
        public void Rotate(Matrix4x4 lookAt, Matrix4x4 perspective)
        {
            var temp = Vector3.Transform(Position, lookAt * perspective);
            RotatedWorldPosition = new Vector3(temp.X * width, temp.Y * height, temp.Z);
        }
    }
}
