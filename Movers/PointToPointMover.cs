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
        private int currentSteps;
        private Vector3 current;
        private Vector3 destination;
        private Vector3 movement;
        public PointToPointMover(int steps, Vector3 initPos)
        {
            this.steps = steps;
            currentSteps = 0;
            current = initPos;
            destination = initPos;
            movement = Vector3.Zero;
        }

        public void ResetMover(Vector3 current, Vector3 destination)
        {
            this.current = current;
            this.destination = destination;
            movement = (destination - current) / steps;
            currentSteps = steps;
        }

        public Vector3 GetNewPosition(Vector3 offset)
        {
            if (currentSteps == 0)
                return offset + destination;
         
            currentSteps--;
            current += movement;
            return offset + current;
        }
    }
}
