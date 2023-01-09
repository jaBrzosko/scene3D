using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Scene3D.Lights
{
    internal class PointLight: ILight
    {
        public Vector3 LightColor { get; set; }

        private Vector3 position;
        public Vector3 Position { 
            get { return position; } 
            set { 
                position = value;
                WorldPosition = new Vector3(width * position.X, height * position.Y, position.Z);
            } }
        public Vector3 WorldPosition { get; set; }
        private int width;
        private int height;

        public PointLight(Vector3 lightColor, Vector3 position, int width, int height)
        {
            this.width = width;
            this.height = height;
            LightColor = lightColor;
            Position = position;
        }

        public Vector3 GetLightColor(Vector3 L, float m)
        {
            return LightColor;
        }
        public Vector3 GetWorldPosition()
        {
            return WorldPosition;
        }
    }
}
