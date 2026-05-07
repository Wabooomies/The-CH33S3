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
            {
                throw new ArgumentNullException(nameof(chessInstance), "ChessGameViewModel instance cannot be null.");
            }

            if (chessInstance.Board is null)
            {
                throw new InvalidOperationException("ChessGameViewModel.Board cannot be null.");
            }

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
                    // This is your leader's exact MS Paint math, simplified!
                    // It checks if it moved 2 spaces one way, and 1 space the other.
                    return (deltaRow == 2 && deltaCol == 1) || (deltaRow == 1 && deltaCol == 2);

                case "Rook":


                    return deltaRow == 0 || deltaCol == 0;

                case "Bishop":


                    return deltaRow == deltaCol;

                case "Queen":
                    // A Queen is literally just a Rook and a Bishop combined
                    return (deltaRow == 0 || deltaCol == 0) || (deltaRow == deltaCol);

                case "King":
                    // Kings move 1 step in any direction
                    return deltaRow <= 1 && deltaCol <= 1;

                case "Pawn":

                    int forwardDir = source.Piece.Side == "Black" ? 1 : -1;
                    int startingRow = source.Piece.Side == "Black" ? 1 : 6;

                    // Moving straight forward (can't capture straight forward)
                    if (deltaCol == 0 && target.Piece == null)
                    {
                        if (endRow - startRow == forwardDir) return true;

                        // 2 steps forward (only from starting line)
                        if (startRow == startingRow && endRow - startRow == 2 * forwardDir) return true;
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
    }
}