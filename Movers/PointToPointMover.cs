using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Movers
{
    internal class PointToPointMover: IMover
    {
        private int steps;
        public int CurrentSteps { get; set; }
        private Vector3 current;
        private Vector3 destination;
        private Vector3 movement;
        public PointToPointMover(int steps, Vector3 initPos)
        {
            this.steps = steps;
            CurrentSteps = 0;
            current = initPos;
            destination = initPos;
            movement = Vector3.Zero;
        }

        public void ResetMover(Vector3 current, Vector3 destination)
        {
            this.current = current;
            this.destination = destination;
            int tempSteps = (int)(steps * (destination - current).Length());
            movement = (destination - current) / tempSteps;
            CurrentSteps = tempSteps;
        }

        public Vector3 GetNewPosition(Vector3 offset)
        {
            if (CurrentSteps == 0)
                return offset + destination;
         
            CurrentSteps--;
            current += movement;
            return offset + current;
        }
    }
}
