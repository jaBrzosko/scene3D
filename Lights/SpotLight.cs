using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Lights
{
    internal class SpotLight : Light
    {
        public float Narrow { get; set; }
        private Vector3 lightDirection;
        private Vector3 directionIncrement;
        public Vector3 LightDirection
        {
            get { return lightDirection; }
            set { lightDirection = Vector3.Normalize(value); }
        }
        private int height;

        public SpotLight(Vector3 lightColor, Vector3 position, Vector3 lightDirection, Vector3 directionIncrement, int width, int height, float narrow)
            :base(lightColor, position, width, height)
        {
            LightDirection = lightDirection;
            this.directionIncrement = directionIncrement;
            Narrow = narrow;
        }
        public override Vector3 GetLightColor(Vector3 L, float m)
        {
            if (!isTurnedOn)
                return Vector3.Zero;
            var cos = Vector3.Dot(-LightDirection, L);
            //return LightColor * MathF.Pow(cos, 15);
            //if (cos > MathF.Cos(LightAngle))
            //return LightColor;
            //return Vector3.Zero;
            //cos = Math.Clamp((cos - MathF.Cos(LightAngle)) / 0.05f, 0f, 1f);
            return LightColor * MathF.Pow(cos, Narrow);
        }

        public void ChangeDirection(bool plus)
        {
            var mult = plus ? 1 : -1;
            LightDirection += mult * directionIncrement;
        }
    }
}
