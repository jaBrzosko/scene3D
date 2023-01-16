using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Scene3D.Movers;

namespace Scene3D.Objects.Chess
{
    internal class Piece: Model
    {
        protected int row;
        protected int col;
        protected float offset;
        protected float step;
        private PointToPointMover PieceMover { get; set; }
        public Piece(Model model, int row, int col, float offset, float step, int numberOfSteps): base(model)
        {
            this.row = row;
            this.col = col;
            this.offset = offset;
            this.step = step;
            base.InitialPosition = new Vector3(offset + row * step, offset + col * step, 0);
            PieceMover = new PointToPointMover(numberOfSteps, InitialPosition);
        }

        public override void MakeMovementStep()
        {
            Movement = PieceMover.GetNewPosition(Vector3.Zero);
        }

        public void Move(int x, int y)
        {
            int tempX = row + x;
            int tempY = col + y;
            if (tempX >= 0 && tempX < 8)
                row = tempX;
            if(tempY >= 0 && tempY < 8)
                col = tempY;
            
            var dest = new Vector3(offset + row * step, offset + col * step, 0);
            PieceMover.ResetMover(InitialPosition, dest);
            InitialPosition = dest;
        }
    }
}
