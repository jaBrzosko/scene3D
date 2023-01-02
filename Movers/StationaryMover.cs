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
        private Vector3 position;
        public StationaryMover(Vector3 position)
        {
            this.position = position;
        }
        public Vector3 GetNewPosition()
        {
            return position;
        }
    }
}
