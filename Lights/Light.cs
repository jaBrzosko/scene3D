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
        public Vector3 Position { get; set; }
        private Vector3 WorldPosition { get; set; }
        private Vector3 RotatedWorldPosition { get; set; }
        private int width;
        private int height;
        protected bool isTurnedOn;

        public Light(Vector3 lightColor, Vector3 position, int width, int height)
        {
            this.width = width;
            this.height = height;
            LightColor = lightColor;
            Position = position;
            isTurnedOn = true;
        }

        public abstract Vector3 GetLightColor(Vector3 L, float m);
        public Vector3 GetWorldPosition()
        {
            return Position;

        }
        public void Rotate(Matrix4x4 lookAt, Matrix4x4 perspective)
        {
            var temp = Vector3.Transform(Position, lookAt * perspective);
            RotatedWorldPosition = new Vector3(temp.X * width, temp.Y * height, temp.Z);
        }
        public void ChangeOnOff()
        {
            isTurnedOn = !isTurnedOn;
        }
    }
}
