using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Objects.Chess
{
    internal class Rook : Piece
    {
        public Rook(Piece piece, Model newModel) : base(piece, newModel)
        { }
        public Rook(Model model, int row, int col, float offset, float step, int numberOfSteps, bool isWhite) : 
            base(model, row, col, offset, step, numberOfSteps, isWhite)
        { }

        public override bool CanMove(int x, int y, Piece[,] pieces)
        {
            if (!base.CanMove(x, y, pieces))
                return false;
            if (x != 0 && y != 0)
                return false;
            if(x > 0)
            {
                for (int i = 1; i <= x; i++)
                {
                    if (pieces[col + i, row] != null)
                        return false;
                }
            }
            else if (x < 0)
            {
                for (int i = x; i < 0; i++)
                {
                    if (pieces[col + i, row] != null)
                        return false;
                }
            }
            else if (y > 0)
            {
                for (int i = 1; i <= y; i++)
                {
                    if (pieces[col, row + i] != null)
                        return false;
                }
            }
            else if (y < 0)
            {
                for (int i = y; i < 0; i++)
                {
                    if (pieces[col, row + i] != null)
                        return false;
                }
            }
            return true;
        }

        public override bool CanCapture(int x, int y, Piece[,] pieces)
        {
            if (!base.CanCapture(x, y, pieces))
                return false;
            if (x != 0 && y != 0)
                return false;
            if (x > 0)
            {
                for (int i = 1; i < x; i++)
                {
                    if (pieces[col + i, row] != null)
                        return false;
                }
                return pieces[col + x, row] != null;
            }
            else if (x < 0)
            {
                for (int i = x + 1; i < 0; i++)
                {
                    if (pieces[col + i, row] != null)
                        return false;
                }
                return pieces[col + x, row] != null;
            }
            else if (y > 0)
            {
                for (int i = 1; i < y; i++)
                {
                    if (pieces[col, row + i] != null)
                        return false;
                }
                return pieces[col, row + y] != null;
            }
            else if (y < 0)
            {
                for (int i = y + 1; i < 0; i++)
                {
                    if (pieces[col, row + i] != null)
                        return false;
                }
                return pieces[col, row + y] != null;
            }
            return false;
        }
    }
}
