using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Objects.Chess
{
    internal class King: Piece
    {
        public King(Model model, int row, int col, float offset, float step, int numberOfSteps, bool isWhite) : 
            base(model, row, col, offset, step, numberOfSteps, isWhite)
        { }

        public override bool CanMove(int x, int y, Piece[,] pieces)
        {
            if (!base.CanMove(x, y, pieces))
                return false;
            if (Math.Abs(x) <= 1 && Math.Abs(y) <= 1 && pieces[row + x, row + y] == null)
                return true;
            if(x == 0 && !Moved)
            {
                if (y == 2 && pieces[row, col + 2] is Rook && !pieces[row, col + 2].Moved)
                    return true;
                if (y == -2 && pieces[row, col - 2] is Rook && !pieces[row, col + 2].Moved)
                    return true;
            }

            return false;
        }
        public override bool CanCapture(int x, int y, Piece[,] pieces)
        {
            return base.CanCapture(x, y, pieces);
        }
    }
}
