using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Objects.Chess
{
    internal class Queen : Piece
    {
        public Queen(Piece piece, Model newModel): base(piece, newModel)
        {}
        public Queen(Model model, int row, int col, float offset, float step, int numberOfSteps, bool isWhite) : 
            base(model, row, col, offset, step, numberOfSteps, isWhite)
        { }

        public override bool CanMove(int x, int y, Piece[,] pieces)
        {
            if (!base.CanMove(x, y, pieces))
                return false;
            return true;
        }
    }
}
