using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Movers
{
    internal class CircleMover : IMover
    {
        private float r;
        private float theta;
        private float thetaStep;
        private float z;
        public CircleMover(float r, float theta, float z)
        {
            this.r = r;
            thetaStep = theta;
            this.z = z;
        }
        public Vector3 GetNewPosition(Vector3 offset)
        {
            theta += thetaStep;
            return offset + new Vector3(r * MathF.Cos(theta), r * MathF.Sin(theta), z);
        }
    }
}
