using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using The_CH33S3.Models;
using The_CH33S3.ViewModels;

namespace The_CH33S3.Controls
{
    public partial class ChessGame : UserControl
    {
        public ChessGame()
        {
            InitializeComponent();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Square sourceSquare)
            {
                if (sourceSquare.Piece is not null)
                {
                    DataObject dragData = new DataObject("ChessPiece", sourceSquare);
                    DragDrop.DoDragDrop(border, dragData, DragDropEffects.Move);
                }
            }
        }

        private void Square_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("ChessPiece"))
            {
                var sourceSquare = e.Data.GetData("ChessPiece") as Square;
                var targetSquare = (sender as Border)?.DataContext as Square;
                ChessGameViewModel viewModel = this.DataContext as ChessGameViewModel;

                if (viewModel is null)
                {
                    MessageBox.Show("ViewModel is not set. Cannot perform move.");
                    return;
                }

                if (sourceSquare is not null && targetSquare is not null && sourceSquare != targetSquare)
                {
                    if (MoveChecker.IslegalMove(viewModel, sourceSquare, targetSquare))
                    {
                        // ---> EN PASSANT GHOST CAPTURE LOGIC <---
                        if (sourceSquare.Piece.Name == "Pawn" &&
                            Math.Abs(targetSquare.Position[1] - sourceSquare.Position[1]) == 1 &&
                            targetSquare.Piece == null)
                        {
                            // Find the enemy pawn we are jumping behind and delete it
                            var capturedPawnSquare = viewModel.Board.FirstOrDefault(s =>
                                s.Position[0] == sourceSquare.Position[0] &&
                                s.Position[1] == targetSquare.Position[1]);

                            if (capturedPawnSquare != null)
                            {
                                capturedPawnSquare.Piece = null;
                            }
                        }

                        // Normal move logic
                        targetSquare.Piece = sourceSquare.Piece;
                        sourceSquare.Piece = null;

                        // ---> CHANGED: UPDATE EN PASSANT MEMORY FOR NEXT TURN <---
                        if (targetSquare.Piece.Name == "Pawn" && Math.Abs(targetSquare.Position[0] - sourceSquare.Position[0]) == 2)
                        {
                            // Calculate the row that was skipped over and save just the numbers
                            int skippedRow = (sourceSquare.Position[0] + targetSquare.Position[0]) / 2;
                            viewModel.EnPassantTargetCoords = new int[] { skippedRow, targetSquare.Position[1] };
                        }
                        else
                        {
                            // If ANY other move is made, the En Passant window closes!
                            viewModel.EnPassantTargetCoords = null;
                        }
                    }
                }
            }
        }
    }
}