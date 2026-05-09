using System;
using System.Linq;
using System.Windows;
using The_CH33S3.Models;

namespace The_CH33S3.ViewModels
{
    public static class MoveChecker
    {
        public static bool IslegalMove(ChessGameViewModel chessInstance, Square source, Square target)
        {
            if (chessInstance is null)
                throw new ArgumentNullException(nameof(chessInstance), "ChessGameViewModel instance cannot be null.");

            if (chessInstance.Board is null)
                throw new InvalidOperationException("ChessGameViewModel.Board cannot be null.");

            if (source.Piece == null) return false;
            if (target.Piece != null && target.Piece.Side == source.Piece.Side) return false;

            int startRow = source.Position[0];
            int startCol = source.Position[1];
            int endRow = target.Position[0];
            int endCol = target.Position[1];

            int deltaRow = Math.Abs(endRow - startRow);
            int deltaCol = Math.Abs(endCol - startCol);

            switch (source.Piece.Name)
            {
                case "Knight":
                    return (deltaRow == 2 && deltaCol == 1) || (deltaRow == 1 && deltaCol == 2);

                case "Rook":
                    if (!(deltaRow == 0 || deltaCol == 0)) return false;
                    return IsPathClear(chessInstance, startRow, startCol, endRow, endCol);

                case "Bishop":
                    if (deltaRow != deltaCol) return false;
                    return IsPathClear(chessInstance, startRow, startCol, endRow, endCol);

                case "Queen":
                    if (!((deltaRow == 0 || deltaCol == 0) || (deltaRow == deltaCol))) return false;
                    return IsPathClear(chessInstance, startRow, startCol, endRow, endCol);

                case "King":
                    return deltaRow <= 1 && deltaCol <= 1;

                case "Pawn":
                    int forwardDir = source.Piece.Side.ToString() == "1" || source.Piece.Side.ToString() == "Black" ? 1 : -1;
                    int startingRow = source.Piece.Side.ToString() == "1" || source.Piece.Side.ToString() == "Black" ? 1 : 6;

                    // Moving straight forward
                    if (deltaCol == 0 && target.Piece == null)
                    {
                        if (endRow - startRow == forwardDir) return true;

                        if (startRow == startingRow && endRow - startRow == 2 * forwardDir)
                        {
                            return IsPathClear(chessInstance, startRow, startCol, endRow, endCol);
                        }
                    }
                    // Normal Capture (diagonally)
                    else if (deltaCol == 1 && endRow - startRow == forwardDir && target.Piece != null)
                    {
                        return true;
                    }
                    // ---> CHANGED: En Passant Capture (diagonally into an empty square) <---
                    else if (deltaCol == 1 && endRow - startRow == forwardDir && target.Piece == null)
                    {
                        // Compare the actual X/Y coordinates instead of WPF objects
                        if (chessInstance.EnPassantTargetCoords != null &&
                            target.Position[0] == chessInstance.EnPassantTargetCoords[0] &&
                            target.Position[1] == chessInstance.EnPassantTargetCoords[1])
                        {
                            return true;
                        }
                    }

                    return false;

                default:
                    return false;
            }
        }

        private static bool IsPathClear(ChessGameViewModel chessInstance, int startRow, int startCol, int endRow, int endCol)
        {
            int rowStep = Math.Sign(endRow - startRow);
            int colStep = Math.Sign(endCol - startCol);

            int currentRow = startRow + rowStep;
            int currentCol = startCol + colStep;

            while (currentRow != endRow || currentCol != endCol)
            {
                var squareToCheck = chessInstance.Board.FirstOrDefault(s => s.Position[0] == currentRow && s.Position[1] == currentCol);

                if (squareToCheck != null && squareToCheck.Piece != null)
                {
                    return false;
                }

                currentRow += rowStep;
                currentCol += colStep;
            }
            return true;
        }
    }
}