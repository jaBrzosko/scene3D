using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Objects.Chess
{
    internal class Bishop : Piece
    {
        public Bishop(Piece piece, Model newModel) : base(piece, newModel)
        { }
        public Bishop(Model model, int row, int col, float offset, float step, int numberOfSteps, bool isWhite) : 
            base(model, row, col, offset, step, numberOfSteps, isWhite)
        { }

        public override bool CanMove(int x, int y, Piece[,] pieces)
        {
            if (!base.CanMove(x, y, pieces))
                return false;
            if (Math.Abs(x) != Math.Abs(y))
                return false;
            int xModif = x < 0 ? -1 : 1;
            int yModif = y < 0 ? -1 : 1;

            for(int i = Math.Abs(x); i > 0; i--)
            {
                if (pieces[col + i * xModif, row + i * yModif] is not null)
                    return false;
            }
            return true;
        }
        public override bool CanCapture(int x, int y, Piece[,] pieces)
        {
            return CanMove(x, y, pieces);
        }
    }
}
