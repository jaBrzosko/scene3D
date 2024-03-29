﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Movers
{
    internal class SphereMover : IMover
    {
        private float r;
        private float theta;
        private float phi;
        private float thetaStep;
        private float phiStep;
        public SphereMover(float r, float theta, float phi)
        {
            this.r = r;
            thetaStep = theta;
            phiStep = phi;
            this.theta = 0;
            this.phi = 0;
        }
        public Vector3 GetNewPosition(Vector3 offset)
        {
            theta += thetaStep;
            phi += phiStep;
            return offset + new Vector3(r * MathF.Sin(theta) * MathF.Cos(phi),
                               r * MathF.Sin(theta) * MathF.Sin(phi),
                               r * MathF.Cos(theta));
        }
    }
}
