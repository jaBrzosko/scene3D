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
        public bool Moved { get; set; }
        private PointToPointMover PieceMover { get; set; }
        public Piece(Model model, int row, int col, float offset, float step, int numberOfSteps): base(model)
        {
            this.row = row;
            this.col = col;
            this.offset = offset;
            this.step = step;
            base.InitialPosition = new Vector3(offset + row * step, offset + col * step, 0);
            PieceMover = new PointToPointMover(numberOfSteps, InitialPosition);
            Moved = false;
        }

        public bool IsMoving { get { return InitialPosition != Movement; } }

        public override void MakeMovementStep()
        {
            Movement = PieceMover.GetNewPosition(Vector3.Zero);
        }

        public virtual void Move(int x, int y)
        {
            int tempX = col + x;
            int tempY = row + y;
            if (tempX >= 0 && tempX < 8)
                col = tempX;
            if(tempY >= 0 && tempY < 8)
                row = tempY;
            if (row != tempY && col != tempX)
                return;
            Moved = true;
            var dest = new Vector3(offset + row * step, offset + col * step, 0);
            PieceMover.ResetMover(InitialPosition, dest);
            InitialPosition = dest;
        }

        public virtual bool CanMove(int x, int y, Piece[,] pieces)
        {
            int nx = row + x;
            int ny = col + y;
            return nx >= 0 && nx < 8 && ny >= 0 && ny < 8;
        }

        public virtual bool CanCapture(int x, int y, Piece[,] pieces)
        {
            int nx = row + x;
            int ny = col + y;
            return nx >= 0 && nx < 8 && ny >= 0 && ny < 8;
        }
    }
}
