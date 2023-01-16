using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Objects.Chess
{
    internal class Rook : Piece
    {
        public Rook(Model model, int row, int col, float offset, float step, int numberOfSteps) : base(model, row, col, offset, step, numberOfSteps)
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
                    if (pieces[row + i, col] != null)
                        return false;
                }
            }
            else if (x < 0)
            {
                for (int i = x; i < 0; i++)
                {
                    if (pieces[row + i, col] != null)
                        return false;
                }
            }
            else if (y > 0)
            {
                for (int i = 1; i <= y; i++)
                {
                    if (pieces[row, col + i] != null)
                        return false;
                }
            }
            else if (y < 0)
            {
                for (int i = y; i < 0; i++)
                {
                    if (pieces[row, col + i] != null)
                        return false;
                }
            }
            return false;
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
                    if (pieces[row + i, col] != null)
                        return false;
                }
                return pieces[row + x, col] != null;
            }
            else if (x < 0)
            {
                for (int i = x; i < 0; i++)
                {
                    if (pieces[row + i, col] != null)
                        return false;
                }
                return pieces[row + x, col] != null;
            }
            else if (y > 0)
            {
                for (int i = 1; i < y; i++)
                {
                    if (pieces[row, col + i] != null)
                        return false;
                }
                return pieces[row, col + y] != null;
            }
            else if (y < 0)
            {
                for (int i = y; i < 0; i++)
                {
                    if (pieces[row, col + i] != null)
                        return false;
                }
                return pieces[row, col + y] != null;
            }
            return false;
        }
    }
}
