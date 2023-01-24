using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Scene3D.Lights
{
    internal class PointLight: Light
    {

        private float lightCoef;
        public float LightCoef
        {
            get { return lightCoef; }
            set { lightCoef = Math.Clamp(value, 0, 1); }
        }

        public PointLight(Vector3 lightColor, Vector3 position, int width, int height): base(lightColor, position, width, height)
        {
            LightCoef = 1;
        }

        public override Vector3 GetLightColor(Vector3 L, float m)
        {
            return isTurnedOn ? LightCoef * LightColor : Vector3.Zero;
        }
    }
}
