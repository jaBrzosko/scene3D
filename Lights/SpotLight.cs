using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Lights
{
    internal class SpotLight : ILight
    {
        public float LightAngle { get; set; }
        public Vector3 LightDirection { get; set; }

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
        public Vector3 WorldPosition { get; set; }
        private int width;
        private int height;

        public SpotLight(Vector3 lightColor, Vector3 position, Vector3 lightDirection, int width, int height, float angle)
        {
            this.width = width;
            this.height = height;
            LightColor = lightColor;
            Position = position;
            LightDirection = lightDirection;
            LightAngle = angle;
        }
        public Vector3 GetLightColor(Vector3 L, float m)
        {
            var cos = Vector3.Dot(Vector3.Normalize(-LightDirection), L);
            //if ( cos >   MathF.Cos(LightAngle))
            //    return LightColor;
            //return Vector3.Zero;
            cos = Math.Clamp((cos - MathF.Cos(LightAngle)) / 0.02f, 0f, 1f);
            return LightColor * cos;
        }

        public Vector3 GetWorldPosition()
        {
            return WorldPosition;
        }
    }
}
