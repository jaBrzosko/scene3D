using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Objects.Chess
{
    internal class ChessGame: ModelCollection
    {
        private Piece?[,] chessBoard;
        private Queue<string> pendingMoves;
        private bool isWhiteMove;
        private Piece? moving;
        private Piece? capturable;
        public ChessGame(float aspectRatio):base(aspectRatio)
        {
            chessBoard = new Piece[8, 8];
            pendingMoves = new Queue<string>();
            isWhiteMove = true;
        }

        public void AddMove(string move)
        {
            pendingMoves.Enqueue(move);
        }

        public void AddPiece(Piece piece, int x, int y)
        {
            chessBoard[x, y] = piece;
            AddModel(piece);
        }

        public void MakeChessMove()
        {
            if(moving != null)
            {
                if (moving.IsMoving)
                    return;
                moving = null;
                if (capturable is not null)
                {
                    RemoveModel(capturable);
                    capturable = null;
                }
            }
            Piece? temp;
            try
            {
                temp = ParseMove(pendingMoves.Peek());
            }
            catch
            {
                return;
            }
            if (temp == null)
                return;
            isWhiteMove = !isWhiteMove;
            moving = temp;
            pendingMoves.Dequeue();
        }

        private Piece? ParseMove(string move)
        {
            var key = move[0];
            if(Char.IsLower(key))
            {
                return ParsePawnMove(move);
            }
            switch(key)
            {
                case 'R':
                    {
                        return ParseRookMove(move);
                    }
                case 'B':
                    {
                        return ParseBishopMove(move);
                    }
                case 'N':
                    {
                        return ParseKnightMove(move);
                    }
                case 'Q':
                    {
                        return ParseQueenMove(move);
                    }
                case 'K':
                    {
                        return ParseKingMove(move);
                    }
            }
            return null;
        }

        private Piece? ParsePawnMove(string move)
        {
            if(move.Contains('x'))
            {
                // capture //exd5
                var colSrc = 7 - (move[0] - 'a');
                var colDest = 7 - (move[2] - 'a');
                if (!int.TryParse(move.Substring(3), out var row))
                    return null;
                row--;
                if (chessBoard[colDest, row] is null)
                    return null;
                if (isWhiteMove)
                {
                    if (chessBoard[colSrc, row - 1] is Pawn)
                    {
                        chessBoard[colSrc, row - 1].Move(colDest - colSrc, 1);
                        Piece temp = chessBoard[colSrc, row - 1];
                        chessBoard[colSrc, row - 1] = null;
                        capturable = chessBoard[colDest, row];
                        chessBoard[colDest, row] = temp;
                        return temp;
                    }
                }
                else
                {
                    if (chessBoard[colSrc, row + 1] is Pawn)
                    {
                        chessBoard[colSrc, row + 1].Move(colDest - colSrc, -1);
                        Piece temp = chessBoard[colSrc, row + 1];
                        chessBoard[colSrc, row + 1] = null;
                        capturable = chessBoard[colDest, row];
                        chessBoard[colDest, row] = temp;
                        return temp;
                    }
                }
            }
            else
            {
                var col = 7 - (move[0] -'a');
                if (!int.TryParse(move.Substring(1), out var row))
                    return null;
                row--;
                if(isWhiteMove)
                {
                    if(chessBoard[col, row - 1] is Pawn)
                    {
                        chessBoard[col, row - 1].Move(0, 1);
                        Piece temp = chessBoard[col, row - 1];
                        chessBoard[col, row - 1] = null;
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                    if (row == 3 && chessBoard[col, row - 2] is Pawn)
                    {
                        chessBoard[col, row - 2].Move(0, 2);
                        Piece temp = chessBoard[col, row - 2];
                        chessBoard[col, row - 2] = null;
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                }
                else
                {
                    if (chessBoard[col, row + 1] is Pawn)
                    {
                        chessBoard[col, row + 1].Move(0, -1);
                        Piece temp = chessBoard[col, row + 1];
                        chessBoard[col, row + 1] = null;
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                    if (row == 4 && chessBoard[col, row + 2] is Pawn)
                    {
                        chessBoard[col, row + 2].Move(0, -2);
                        Piece temp = chessBoard[col, row + 2];
                        chessBoard[col, row + 2] = null;
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                }
            }
            return null;
        }
        private Piece? ParseRookMove(string move)
        {
            if (move.Contains('x'))
            {
                // capture
            }
            else
            {

            }
            return null;
        }
        private Piece? ParseBishopMove(string move)
        {
            if (move.Contains('x'))
            {
                // capture
            }
            else
            {

            }
            return null;
        }
        private Piece? ParseKnightMove(string move)
        {
            if (move.Contains('x'))
            {
                // capture
            }
            else
            {

            }
            return null;
        }
        private Piece? ParseQueenMove(string move)
        {
            if (move.Contains('x'))
            {
                // capture
            }
            else
            {

            }
            return null;
        }
        private Piece? ParseKingMove(string move)
        {
            if (move.Contains('x'))
            {
                // capture
            }
            else
            {

            }
            return null;
        }
    }
}
