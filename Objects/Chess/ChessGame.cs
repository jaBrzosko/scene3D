using Scene3D.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Objects.Chess
{
    internal class ChessGame: ModelCollection
    {
        private static (int x, int y)[] knightMoves = { (-1, 2), (-1, -2), (1, 2), (1, -2), (2, -1), (2, 1), (-2, -1), (-2, 1) };
        private static (int x, int y)[] bishopMultipliers = { (-1, -1), (-1, 1), (1, -1), (1, 1) };
        private static (int x, int y)[] kingMoves = { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
        private Piece?[,] chessBoard;
        private Queue<string> pendingMoves;
        private bool isWhiteMove;
        public Piece? Moving { get; set; }
        private Piece? capturable;
        private (int x, int y)? promotableCords;
        private char? promoCode;
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
            if(Moving != null)
            {
                if (Moving.IsMoving)
                    return;
                Moving = null;
                if (capturable is not null)
                {
                    RemoveModel(capturable);
                    capturable = null;
                }
                if(promotableCords is not null && promoCode is not null)
                {
                    int tx = promotableCords.Value.x;
                    int ty = promotableCords.Value.y;
                    var toBePromoted = chessBoard[tx, ty];
                    Piece? newPiece = null;
                    switch(promoCode)
                    {
                        case 'Q':
                            {
                                var objName = "Queen.obj";
                                var pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
                                var newModel = FileReader.ReadObj(pathModel);
                                newPiece = new Queen(toBePromoted,  newModel);
                                break;
                            }
                        case 'R':
                            {
                                var objName = "Rook.obj";
                                var pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
                                var newModel = FileReader.ReadObj(pathModel);
                                newPiece = new Rook(toBePromoted, newModel);
                                break;
                            }
                        case 'B':
                            {
                                var objName = "Knight.obj";
                                var pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
                                var newModel = FileReader.ReadObj(pathModel);
                                newPiece = new Bishop(toBePromoted, newModel);
                                break;
                            }
                        case 'N':
                            {
                                var objName = "Knight.obj";
                                var pathModel = Path.Combine(Environment.CurrentDirectory, "data\\", objName);
                                var newModel = FileReader.ReadObj(pathModel);
                                newPiece = new Knight(toBePromoted, newModel);
                                break;
                            }
                    }
                    if (newPiece != null)
                    {
                        chessBoard[tx, ty] = newPiece;
                        AddModel(newPiece);
                    }

                    RemoveModel(toBePromoted);
                    promotableCords = null;
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
            Moving = temp;
            pendingMoves.Dequeue();
        }

        private Piece? ParseMove(string move)
        {
            var key = move[0];
            if(Char.IsLower(key))
            {
                if (move.Contains('='))
                    return ParsePromotion(move);
                return ParsePawnMove(move);
            }
            switch (key)
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
                case 'O':
                    {
                        return ParseCastle(move);
                    }
            }
            return null;
        }

        private Piece? ParsePromotion(string move)
        {
            promoCode = move[move.Length - 1];
            move = move.Substring(0, move.Length - 2);

            if (move.Contains('x'))
            {
                // capture //exd5
                var colSrc = 7 - (move[0] - 'a');
                var colDest = 7 - (move[2] - 'a');
                if (!int.TryParse(move.Substring(3, 1), out var row))
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
                        promotableCords = (colDest, row);
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
                        promotableCords = (colDest, row);
                        return temp;
                    }
                }
            }
            else
            {
                var col = 7 - (move[0] - 'a');
                if (!int.TryParse(move.Substring(1, 1), out var row))
                    return null;
                row--;
                if (isWhiteMove)
                {
                    if (chessBoard[col, row - 1] is Pawn)
                    {
                        chessBoard[col, row - 1].Move(0, 1);
                        Piece temp = chessBoard[col, row - 1];
                        chessBoard[col, row - 1] = null;
                        chessBoard[col, row] = temp;
                        promotableCords = (col, row);
                        return temp;
                    }
                    if (row == 3 && chessBoard[col, row - 2] is Pawn)
                    {
                        chessBoard[col, row - 2].Move(0, 2);
                        Piece temp = chessBoard[col, row - 2];
                        chessBoard[col, row - 2] = null;
                        chessBoard[col, row] = temp;
                        promotableCords = (col, row);
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
                        promotableCords = (col, row);
                        return temp;
                    }
                    if (row == 4 && chessBoard[col, row + 2] is Pawn)
                    {
                        chessBoard[col, row + 2].Move(0, -2);
                        Piece temp = chessBoard[col, row + 2];
                        chessBoard[col, row + 2] = null;
                        chessBoard[col, row] = temp;
                        promotableCords = (col, row);
                        return temp;
                    }
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
                if (!int.TryParse(move.Substring(3, 1), out var row))
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
                if (!int.TryParse(move.Substring(1, 1), out var row))
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
            move = move.Substring(1);
            var (specificColumn, specificRow, mv) = ParseSpecific(move);
            move = mv;
            if (move.Contains('x'))
            {
                // capture
                var col = 7 - (move[1] - 'a');
                if (!int.TryParse(move.Substring(2, 1), out var row))
                    return null;
                row--;
                for (int i = -7; i <= 7; i++)
                {
                    int nx = col + i;
                    int ny = row;
                    if ((specificColumn is not null && nx != specificColumn) || (specificRow is not null && ny != specificRow))
                        continue;
                    if (nx < 0 || nx > 7)
                        continue;
                    if (chessBoard[nx, ny] is Rook && (chessBoard[nx, ny].IsWhite == isWhiteMove))
                    {
                        Piece temp = chessBoard[nx, ny];
                        if (temp is null || !temp.CanCapture(-i, 0, chessBoard))
                            continue;
                        temp.Move(-i, 0);
                        chessBoard[nx, ny] = null;
                        capturable = chessBoard[col, row];
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                }
                for (int i = -7; i <= 7; i++)
                {
                    int nx = col;
                    int ny = row + i;
                    if ((specificColumn is not null && nx != specificColumn) || (specificRow is not null && ny != specificRow))
                        continue;
                    if (ny < 0 || ny > 7)
                        continue;
                    if (chessBoard[nx, ny] is Rook && (chessBoard[nx, ny].IsWhite == isWhiteMove))
                    {
                        Piece temp = chessBoard[nx, ny];
                        if (temp is null || !temp.CanCapture(0, -i, chessBoard))
                            continue;
                        temp.Move(0, -i);
                        chessBoard[nx, ny] = null;
                        capturable = chessBoard[col, row];
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                }
            }
            else
            {
                var col = 7 - (move[0] - 'a');
                if (!int.TryParse(move.Substring(1, 1), out var row))
                    return null;
                row--;
                for (int i = -7; i <= 7; i++)
                {
                    int nx = col + i;
                    int ny = row;
                    if ((specificColumn is not null && nx != specificColumn) || (specificRow is not null && ny != specificRow))
                        continue;
                    if (nx < 0 || nx > 7)
                        continue;
                    if (chessBoard[nx, ny] is Rook && (chessBoard[nx, ny].IsWhite == isWhiteMove))
                    {
                        Piece temp = chessBoard[nx, ny];
                        if (temp is null || !temp.CanMove(-i, 0, chessBoard))
                            continue;
                        temp.Move(-i, 0);
                        chessBoard[nx, ny] = null;
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                }
                for (int i = -7; i <= 7; i++)
                {
                    int nx = col;
                    int ny = row + i;
                    if ((specificColumn is not null && nx != specificColumn) || (specificRow is not null && ny != specificRow))
                        continue;
                    if (ny < 0 || ny > 7)
                        continue;
                    if (chessBoard[nx, ny] is Rook && (chessBoard[nx, ny].IsWhite == isWhiteMove))
                    {
                        Piece temp = chessBoard[nx, ny];
                        if (temp is null || !temp.CanMove(0, -i, chessBoard))
                            continue;
                        temp.Move(0, -i);
                        chessBoard[nx, ny] = null;
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                }
            }
            return null;
        }
        private Piece? ParseBishopMove(string move)
        {
            move = move.Substring(1);
            var (specificColumn, specificRow, mv) = ParseSpecific(move);
            move = mv;
            if (move.Contains('x'))
            {
                // capture
                var col = 7 - (move[1] - 'a');
                if (!int.TryParse(move.Substring(2, 1), out var row))
                    return null;
                row--;
                foreach (var bishopMult in bishopMultipliers)
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        int nx = col + bishopMult.x * i;
                        int ny = row + bishopMult.y * i;
                        if ((specificColumn is not null && nx != specificColumn) || (specificRow is not null && ny != specificRow))
                            continue;
                        if (nx < 0 || nx > 7 || ny < 0 || ny > 7)
                            continue;
                        if (chessBoard[nx, ny] is Bishop && (chessBoard[nx, ny].IsWhite == isWhiteMove))
                        {
                            chessBoard[nx, ny].Move(-bishopMult.x * i, -bishopMult.y * i);
                            Piece temp = chessBoard[nx, ny];
                            chessBoard[nx, ny] = null;
                            capturable = chessBoard[col, row];
                            chessBoard[col, row] = temp;
                            return temp;
                        }
                    }
                }
            }
            else
            {
                var col = 7 - (move[0] - 'a');
                if (!int.TryParse(move.Substring(1, 1), out var row))
                    return null;
                row--;
                foreach(var bishopMult in bishopMultipliers)
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        int nx = col + bishopMult.x * i;
                        int ny = row + bishopMult.y * i;
                        if ((specificColumn is not null && nx != specificColumn) || (specificRow is not null && ny != specificRow))
                            continue;
                        if (nx < 0 || nx > 7 || ny < 0 || ny > 7)
                            continue;
                        if (chessBoard[nx, ny] is Bishop && (chessBoard[nx, ny].IsWhite == isWhiteMove))
                        {
                            chessBoard[nx, ny].Move(-bishopMult.x * i, -bishopMult.y * i);
                            Piece temp = chessBoard[nx, ny];
                            chessBoard[nx, ny] = null;
                            chessBoard[col, row] = temp;
                            return temp;
                        }
                    }
                }
            }
            return null;
        }
        private Piece? ParseKnightMove(string move)
        {
            move = move.Substring(1); // Delete N from move

            var (specificColumn, specificRow, mv) = ParseSpecific(move);
            move = mv;
            if (move.Contains('x'))
            {
                // capture
                var col = 7 - (move[1] - 'a');
                if (!int.TryParse(move.Substring(2, 1), out var row))
                    return null;
                row--;
                foreach (var knightMove in knightMoves)
                {
                    int nx = col + knightMove.x;
                    int ny = row + knightMove.y;
                    if ((specificColumn is not null && nx != specificColumn) || (specificRow is not null && ny != specificRow))
                        continue;
                    if (nx < 0 || nx > 7 || ny < 0 || ny > 7)
                        continue;
                    if (chessBoard[nx, ny] is Knight)
                    {
                        chessBoard[nx, ny].Move(-knightMove.x, -knightMove.y);
                        Piece temp = chessBoard[nx, ny];
                        chessBoard[nx, ny] = null;
                        capturable = chessBoard[col, row];
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                }
            }
            else
            {
                var col = 7 - (move[0] - 'a');
                if (!int.TryParse(move.Substring(1, 1), out var row))
                    return null;
                row--;
                foreach(var knightMove in knightMoves)
                {
                    int nx = col + knightMove.x;
                    int ny = row + knightMove.y;
                    if ((specificColumn is not null && nx != specificColumn) || (specificRow is not null && ny != specificRow))
                        continue;
                    if (nx < 0 || nx > 7 || ny < 0 || ny > 7)
                        continue;
                    if(chessBoard[nx, ny] is Knight && (chessBoard[nx, ny].IsWhite == isWhiteMove))
                    {
                        chessBoard[nx, ny].Move(-knightMove.x, -knightMove.y);
                        Piece temp = chessBoard[nx, ny];
                        chessBoard[nx, ny] = null;
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                }
            }
            return null;
        }
        private Piece? ParseQueenMove(string move)
        {
            move = move.Substring(1);
            var (specificColumn, specificRow, mv) = ParseSpecific(move);
            move = mv;
            if (move.Contains('x'))
            {
                // capture
                var col = 7 - (move[1] - 'a');
                if (!int.TryParse(move.Substring(2, 1), out var row))
                    return null;
                row--;
                for (int i = -7; i <= 7; i++)
                {
                    int nx = col + i;
                    int ny = row;
                    if ((specificColumn is not null && nx != specificColumn) || (specificRow is not null && ny != specificRow))
                        continue;
                    if (nx < 0 || nx > 7)
                        continue;
                    if (chessBoard[nx, ny] is Queen && (chessBoard[nx, ny].IsWhite == isWhiteMove))
                    {
                        chessBoard[nx, ny].Move(-i, 0);
                        Piece temp = chessBoard[nx, ny];
                        chessBoard[nx, ny] = null;
                        capturable = chessBoard[col, row];
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                }
                for (int i = -7; i <= 7; i++)
                {
                    int nx = col;
                    int ny = row + i;
                    if ((specificColumn is not null && nx != specificColumn) || (specificRow is not null && ny != specificRow))
                        continue;
                    if (ny < 0 || ny > 7)
                        continue;
                    if (chessBoard[nx, ny] is Queen && (chessBoard[nx, ny].IsWhite == isWhiteMove))
                    {
                        chessBoard[nx, ny].Move(0, -i);
                        Piece temp = chessBoard[nx, ny];
                        chessBoard[nx, ny] = null;
                        capturable = chessBoard[col, row];
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                }
                foreach (var bishopMult in bishopMultipliers)
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        int nx = col + bishopMult.x * i;
                        int ny = row + bishopMult.y * i;
                        if ((specificColumn is not null && nx != specificColumn) || (specificRow is not null && ny != specificRow))
                            continue;
                        if (nx < 0 || nx > 7 || ny < 0 || ny > 7)
                            continue;
                        if (chessBoard[nx, ny] is Queen && (chessBoard[nx, ny].IsWhite == isWhiteMove))
                        {
                            chessBoard[nx, ny].Move(-bishopMult.x * i, -bishopMult.y * i);
                            Piece temp = chessBoard[nx, ny];
                            chessBoard[nx, ny] = null;
                            capturable = chessBoard[col, row];
                            chessBoard[col, row] = temp;
                            return temp;
                        }
                    }
                }
            }
            else
            {
                var col = 7 - (move[0] - 'a');
                if (!int.TryParse(move.Substring(1, 1), out var row))
                    return null;
                row--;
                for (int i = -7; i <= 7; i++)
                {
                    int nx = col + i;
                    int ny = row;
                    if ((specificColumn is not null && nx != specificColumn) || (specificRow is not null && ny != specificRow))
                        continue;
                    if (nx < 0 || nx > 7)
                        continue;
                    if (chessBoard[nx, ny] is Queen && (chessBoard[nx, ny].IsWhite == isWhiteMove))
                    {
                        chessBoard[nx, ny].Move(-i, 0);
                        Piece temp = chessBoard[nx, ny];
                        chessBoard[nx, ny] = null;
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                }
                for (int i = -7; i <= 7; i++)
                {
                    int nx = col;
                    int ny = row + i;
                    if ((specificColumn is not null && nx != specificColumn) || (specificRow is not null && ny != specificRow))
                        continue;
                    if (ny < 0 || ny > 7)
                        continue;
                    if (chessBoard[nx, ny] is Queen && (chessBoard[nx, ny].IsWhite == isWhiteMove))
                    {
                        chessBoard[nx, ny].Move(0, -i);
                        Piece temp = chessBoard[nx, ny];
                        chessBoard[nx, ny] = null;
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                }
                foreach (var bishopMult in bishopMultipliers)
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        int nx = col + bishopMult.x * i;
                        int ny = row + bishopMult.y * i;
                        if ((specificColumn is not null && nx != specificColumn) || (specificRow is not null && ny != specificRow))
                            continue;
                        if (nx < 0 || nx > 7 || ny < 0 || ny > 7)
                            continue;
                        if (chessBoard[nx, ny] is Queen && (chessBoard[nx, ny].IsWhite == isWhiteMove))
                        {
                            chessBoard[nx, ny].Move(-bishopMult.x * i, -bishopMult.y * i);
                            Piece temp = chessBoard[nx, ny];
                            chessBoard[nx, ny] = null;
                            chessBoard[col, row] = temp;
                            return temp;
                        }
                    }
                }
            }
            return null;
        }
        private Piece? ParseKingMove(string move)
        {
            move = move.Substring(1);
            if (move.Contains('x'))
            {
                // capture
                var col = 7 - (move[1] - 'a');
                if (!int.TryParse(move.Substring(2, 1), out var row))
                    return null;
                row--;
                foreach (var kingMove in kingMoves)
                {
                    int nx = col + kingMove.x;
                    int ny = row + kingMove.y;
                    if (nx < 0 || nx > 7 || ny < 0 || ny > 7)
                        continue;
                    if (chessBoard[nx, ny] is King && (chessBoard[nx, ny].IsWhite == isWhiteMove))
                    {
                        chessBoard[nx, ny].Move(-kingMove.x, -kingMove.y);
                        Piece temp = chessBoard[nx, ny];
                        chessBoard[nx, ny] = null;
                        capturable = chessBoard[col, row];
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                }
            }
            else
            {
                var col = 7 - (move[0] - 'a');
                if (!int.TryParse(move.Substring(1, 1), out var row))
                    return null;
                row--;
                foreach(var kingMove in kingMoves)
                {
                    int nx = col + kingMove.x;
                    int ny = row + kingMove.y;
                    if (nx < 0 || nx > 7 || ny < 0 || ny > 7)
                        continue;
                    if (chessBoard[nx, ny] is King && (chessBoard[nx, ny].IsWhite == isWhiteMove))
                    {
                        chessBoard[nx, ny].Move(-kingMove.x, -kingMove.y);
                        Piece temp = chessBoard[nx, ny];
                        chessBoard[nx, ny] = null;
                        chessBoard[col, row] = temp;
                        return temp;
                    }
                }
            }
            return null;
        }

        private Piece? ParseCastle(string move)
        {
            int row = isWhiteMove ? 0 : 7;
            if (move == "O-O")
            {
                if(chessBoard[3, row] is King && !chessBoard[3, row].Moved && chessBoard[0, row] is Rook && !chessBoard[0, row].Moved)
                {
                    if (chessBoard[2, row] is not null || chessBoard[1, row] is not null)
                        return null;
                    Piece king = chessBoard[3, row];
                    Piece rook = chessBoard[0, row];
                    chessBoard[0, row] = null;
                    chessBoard[1, row] = king;
                    chessBoard[2, row] = rook;
                    chessBoard[3, row] = null;

                    king.Move(-2, 0);
                    rook.Move(2, 0);
                    return rook;
                }
            }
            else if(move == "O-O-O")
            {
                if (chessBoard[3, row] is King && !chessBoard[3, row].Moved && chessBoard[7, row] is Rook && !chessBoard[7, row].Moved)
                {
                    if (chessBoard[6, row] is not null || chessBoard[5, row] is not null || chessBoard[4, row] is not null)
                        return null;
                    Piece king = chessBoard[3, row];
                    Piece rook = chessBoard[7, row];
                    chessBoard[3, row] = null;
                    chessBoard[4, row] = rook;
                    chessBoard[5, row] = king;
                    chessBoard[7, row] = null;

                    king.Move(2, 0);
                    rook.Move(-3, 0);
                    return rook;
                }
            }
            return null;
        }

        private (int? col, int? row, string mv) ParseSpecific(string move)
        {
            int length = move.Length;
            int? specificColumn = null, specificRow = null;
            if (move.Contains('x'))
                length--;
            if (length == 3) // specify row or column
            {
                var temp = move[0];
                if (char.IsLetter(temp))
                {
                    specificColumn = 7 - (move[0] - 'a');
                }
                else
                {
                    if (!int.TryParse(move.Substring(0, 1), out var row))
                        return (null, null, move);
                }
                move = move.Substring(1);
            }
            else if (length == 4) // specify row and column
            {
                specificColumn = 7 - (move[0] - 'a');
                if (!int.TryParse(move.Substring(1, 1), out var row))
                    return (null, null, move);

                specificRow = row;

                move = move.Substring(2);
            }

            return (specificColumn, specificRow, move);
        }
    }
}
