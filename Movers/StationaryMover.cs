using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Movers
{
    internal class StationaryMover : IMover
    {
        public StationaryMover(Vector3 position)
        {
        }
        public Vector3 GetNewPosition(Vector3 offset)
        {
            return offset;
        }
    }
}
