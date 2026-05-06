using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace The_CH33S3.Models
{
    public static class BoardThemes
    {
        public static string DefaultDarkSquareColor = "#FF231E38";
        public static string DefaultLightSquareColor = "#FF120C29";
    }

    class ChessBoard
    {
        private Square[,]? _squares = null;

        public Square[,]? Squares
        {
            get => _squares;
            set => _squares = value;
        }

        public ChessBoard()
        {

        }

        public async Task LoadBoardAsync()
        {
            _squares = await Task.Run(() => InitializeBoard());
        }

        private Square[,] InitializeBoard()
        {
            Square[,] board = new Square[8, 8];

            string darkSquare = BoardThemes.DefaultDarkSquareColor;
            string lightSquare = BoardThemes.DefaultLightSquareColor;

            string pawn = "pack://application:,,,/Images/blackpawn.png";
            string rook = "pack://application:,,,/Images/blackrook.png";
            string knight = "pack://application:,,,/Images/blackknight.png";
            string bishop = "pack://application:,,,/Images/blackbishop.png";
            string queen = "pack://application:,,,/Images/blackqueen.png";
            string king = "pack://application:,,,/Images/blackking.png";

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    string squareColor;
                    ChessPiece chessPiece;
                    string id = "";
                    string name = "";
                    string imagePath = "";
                    int side = 0;
                    int[] position = new int[2] { i, j };

                    bool isDark = (i + j) % 2 == 0;

                    if (isDark)
                        squareColor = darkSquare;
                    else
                        squareColor = lightSquare;

                    if (i <= 1)
                        side = 1;
                    else if (i >= 6)
                        side = 0;

                    switch (i)
                    {
                        case 0:
                        case 7:
                            switch (j)
                            {
                                case 0:
                                case 7:
                                    id = $"R";
                                    name = "Rook";
                                    imagePath = rook;
                                    break;
                                case 1:
                                case 6:
                                    id = $"N";
                                    name = "Knight";
                                    imagePath = knight;
                                    break;
                                case 2:
                                case 5:
                                    id = $"B";
                                    name = "Bishop";
                                    imagePath = bishop;
                                    break;
                                case 3:
                                    id = $"Q";
                                    name = "Queen";
                                    imagePath = queen;
                                    break;
                                case 4:
                                    id = $"K";
                                    name = "King";
                                    imagePath = king;
                                    break;
                            }
                            break;
                        case 1:
                        case 6:
                            id = $"P";
                            name = "Pawn";
                            imagePath = pawn;
                            break;
                        default:

                            break;
                    }

                    Square square;

                    if (i <= 1 || i >= 6)
                    {
                        chessPiece = new ChessPiece(id, name, imagePath, side);
                        square = new Square(squareColor, position, chessPiece);
                    }
                    else
                        square = new Square(squareColor, position);

                    board[i, j] = square;
                }
            }

            return board;
        }
    }
}
