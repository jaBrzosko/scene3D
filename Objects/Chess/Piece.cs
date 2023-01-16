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
        public bool IsWhite { get; set; }
        public bool Moved { get; set; }
        private PointToPointMover PieceMover { get; set; }
        public Piece(Piece piece, Model newModel): base(newModel)
        {
            row = piece.row;
            col = piece.col;
            offset = piece.offset;
            step = piece.step;
            InitialPosition = new Vector3(offset + row * step, offset + col * step, 0);
            Movement = piece.Movement;
            PieceMover = piece.PieceMover;
            Moved = piece.Moved;
            IsWhite = piece.IsWhite;
            ObjectColor = piece.ObjectColor;
            Scale = piece.Scale;
        }
        public Piece(Model model, int row, int col, float offset, float step, int numberOfSteps, bool isWhite): base(model)
        {
            this.row = row;
            this.col = col;
            this.offset = offset;
            this.step = step;
            base.InitialPosition = new Vector3(offset + row * step, offset + col * step, 0);
            PieceMover = new PointToPointMover(numberOfSteps, InitialPosition);
            Moved = false;
            IsWhite = isWhite;
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
            int nx = col + x;
            int ny = row + y;
            return nx >= 0 && nx < 8 && ny >= 0 && ny < 8;
        }

        public virtual bool CanCapture(int x, int y, Piece[,] pieces)
        {
            int nx = col + x;
            int ny = row + y;
            return nx >= 0 && nx < 8 && ny >= 0 && ny < 8;
        }
    }
}
