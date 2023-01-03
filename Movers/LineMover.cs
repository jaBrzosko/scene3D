using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Movers
{
    internal class LineMover : IMover
    {
        private Vector3 position;
        private Vector3 speed;
        private float radius;

        public LineMover(float radius, Vector3 speed, Vector3 initialPosition)
        {
            this.speed = speed;
            this.radius = radius;
            position = initialPosition;
        }
        public Vector3 GetNewPosition()
        {
            if (position.Length() > radius)
                speed *= -1;
            position += speed;
            
            return position;
        }
    }
}
