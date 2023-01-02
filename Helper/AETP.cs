using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Scene3D.Helper
{
    internal class AETP
    {
        public int YMax { get; private set; }
        private float x;
        public int X => (int)x;
        private readonly float coef;
        public AETP(float ux, float uy, float vx, float vy)
        {
            YMax = Math.Max((int)uy, (int)vy);
            x = (int)uy == YMax ? vx : ux;
            coef = uy == vy ? 0 : (ux - vx) / (uy - vy);
        }

        public void Next()
        {
            x += coef;
        }
    }
}
