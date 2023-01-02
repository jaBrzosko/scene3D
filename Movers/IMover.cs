using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Scene3D.Movers
{
    internal interface IMover
    {
        public Vector3 GetNewPosition();
    }
}
