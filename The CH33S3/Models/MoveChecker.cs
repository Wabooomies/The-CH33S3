using System;
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

            // Safety Check 1: Is there actually a piece to move?
            if (source.Piece == null) return false;

            // Safety Check 2: You can't capture your own team's piece!
            if (target.Piece != null && target.Piece.Side == source.Piece.Side) return false;

            // Grab the i (row) and j (column) coordinates from your array
            int startRow = source.Position[0];
            int startCol = source.Position[1];
            int endRow = target.Position[0];
            int endCol = target.Position[1];

            // Calculate the absolute distance moved in rows and columns (The Delta)
            int deltaRow = Math.Abs(endRow - startRow);
            int deltaCol = Math.Abs(endCol - startCol);

            switch (source.Piece.Name)
            {
                case "Knight":
                    // Knights ignore the path check entirely!
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
                    // Kings only move 1 step, so there is no path to be blocked.
                    return deltaRow <= 1 && deltaCol <= 1;

                case "Pawn":
                    int forwardDir = source.Piece.Side == "Black" ? 1 : -1;
                    int startingRow = source.Piece.Side == "Black" ? 1 : 6;

                    // Moving straight forward
                    if (deltaCol == 0 && target.Piece == null)
                    {
                        // 1 step forward
                        if (endRow - startRow == forwardDir) return true;

                        // 2 steps forward (only from starting line AND path must be clear)
                        if (startRow == startingRow && endRow - startRow == 2 * forwardDir)
                        {
                            return IsPathClear(chessInstance, startRow, startCol, endRow, endCol);
                        }
                    }
                    // Capturing diagonally
                    else if (deltaCol == 1 && endRow - startRow == forwardDir && target.Piece != null)
                    {
                        return true;
                    }

                    return false;

                default:
                    return false;
            }
        }
        private static bool IsPathClear(ChessGameViewModel chessInstance, int startRow, int startCol, int endRow, int endCol)
        {
            // Math.Sign returns 1, -1, or 0. This gives us the exact "direction" to step.
            int rowStep = Math.Sign(endRow - startRow);
            int colStep = Math.Sign(endCol - startCol);

            // Start checking one square ahead of the current piece
            int currentRow = startRow + rowStep;
            int currentCol = startCol + colStep;

            // Loop until we reach the target square
            while (currentRow != endRow || currentCol != endCol)
            {
                // Find the square on the board that matches these coordinates
                var squareToCheck = chessInstance.Board.FirstOrDefault(s => s.Position[0] == currentRow && s.Position[1] == currentCol);

                // If the square exists and has a piece on it, the path is blocked!
                if (squareToCheck != null && squareToCheck.Piece != null)
                {
                    return false;
                }

                // Move to the next square in the path
                currentRow += rowStep;
                currentCol += colStep;
            }

            // If we made it through the whole loop without hitting a piece, the path is clear!
            return true;
        }
    }
}