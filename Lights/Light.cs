using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Scene3D.Lights
{
    internal struct Light
    {
        public Vector3 LightColor { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 WorldPosition { get; set; }

        public Light(Vector3 lightColor, Vector3 position, int width, int height)
        {
            LightColor = lightColor;
            Position = position;
            WorldPosition = new Vector3(width * position.X, height * position.Y, position.Z);
        }
    }
}
