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
        public PointLight(Vector3 lightColor, Vector3 position, int width, int height): base(lightColor, position, width, height)
        {}

        public override Vector3 GetLightColor(Vector3 L, float m)
        {
            return LightColor;
        }
    }
}
