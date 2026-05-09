using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using The_CH33S3.Models;

namespace The_CH33S3.ViewModels
{
    public class ChessGameViewModel : BaseViewModel
    {
        private ObservableCollection<Square> _board = new ObservableCollection<Square>();

        public ObservableCollection<Square> Board
        {
            get => _board;
            set => SetProperty(ref _board, value);
        }

        // ---> CHANGED: Now we just save the exact coordinates [Row, Col]
        public int[] EnPassantTargetCoords { get; set; }

        public ChessGameViewModel()
        {
            InitializeGame();
        }

        private async void InitializeGame()
        {
            ChessBoard chessBoard = new ChessBoard();

            await chessBoard.LoadBoardAsync();

            if (chessBoard.Squares != null)
            {
                var tempBoard = new ObservableCollection<Square>();

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        tempBoard.Add(chessBoard.Squares[i, j]);
                    }
                }

                Board = tempBoard;
            }
        }
    }
}