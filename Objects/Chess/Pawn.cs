using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Objects.Chess
{
    internal class Pawn : Piece
    {
        private bool isWhite;
        public Pawn(Model model, int row, int col, float offset, float step, int numberOfSteps, bool isWhite) : base(model, row, col, offset, step, numberOfSteps)
        {
            Moved = false;
            this.isWhite = isWhite;
        }

        public override bool CanMove(int x, int y, Piece[,] pieces)
        {
            if(!base.CanMove(x, y, pieces))
                return false;
            if(y != 0)
                return false;
            if((isWhite && x == 1) || (!isWhite && x == -1))
            {
                return pieces[row + x, col] != null;
            }
            if (!Moved && ((isWhite && x == 2) || (!isWhite && x == -2)))
            {
                return pieces[row + x, col] != null && pieces[row + x, col] != null;
            }
            return false;
        }

        public override bool CanCapture(int x, int y, Piece[,] pieces)
        {
            if (!base.CanCapture(x, y, pieces))
                return false;

            if (pieces[row + x, row + y] == null) //ignore en pessant
                return false;

            return ((isWhite && x == 1) || (!isWhite && x == 1)) && Math.Abs(y) == 1;

        }
    }
}
