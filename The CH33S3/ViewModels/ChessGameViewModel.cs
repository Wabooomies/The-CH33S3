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
        private Square[,] ChessBoard;

        public ObservableCollection<ObservableCollection<Square>> Board { get; set; }

        public ChessGameViewModel()
        {
            ChessBoard chessBoard = new ChessBoard();
            ChessBoard = chessBoard.Squares ?? new Square[8, 8];
            BuildBoardView();
        }

        public void BuildBoardView()
        {
            Board = new ObservableCollection<ObservableCollection<Square>>();

            for (int i = 0; i < 8; i++)
            {
                var row = new ObservableCollection<Square>();

                for (int j = 0; j < 8; j++)
                {
                    row.Add(ChessBoard[i,j]);
                }

                Board.Add(row);
            }
        }
    }
}
