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

namespace The_CH33S3.Controls
{
    /// <summary>
    /// Interaction logic for ChessGame.xaml
    /// </summary>
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
                // Only start dragging if there's actually a piece on this square
                if (sourceSquare.Piece is not null)
                {
                    // Package the Square object into the drag data
                    DataObject dragData = new DataObject("ChessPiece", sourceSquare);

                    // Start the drag operation
                    DragDrop.DoDragDrop(border, dragData, DragDropEffects.Move);
                }
            }
        }

        private void Square_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("ChessPiece"))
            {
                // Get the Square we dragged FROM
                var sourceSquare = e.Data.GetData("ChessPiece") as Square;

                // Get the Square we dropped ONTO (the Border's DataContext)
                var targetSquare = (sender as Border)?.DataContext as Square;

                if (sourceSquare is not null && targetSquare is not null && sourceSquare != targetSquare)
                {
                    // Logic: Move the piece data
                    // This triggers PropertyChanged, so the UI updates instantly
                    targetSquare.Piece = sourceSquare.Piece;
                    sourceSquare.Piece = null;
                }
            }
        }
    }
}
