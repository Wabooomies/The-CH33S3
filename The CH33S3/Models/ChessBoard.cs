using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace The_CH33S3.Models
{
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
            _squares = InitializeBoard();
        }

        private Square[,] InitializeBoard()
        {
            Square[,] board = new Square[8, 8];

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    SolidColorBrush squareColor;
                    ChessPiece chessPiece;
                    string id = "";
                    string name = "";
                    string imagePath = "";
                    int side = 0;

                    bool isDark = (i + j) % 2 == 0;

                    if (isDark)
                        squareColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF231E38"));
                    else
                        squareColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF120C29"));

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
                                    id = $"R{j}";
                                    name = "Rook";
                                    imagePath = "";
                                    break;
                                case 1:
                                case 6:
                                    id = $"N{j}";
                                    name = "Knight";
                                    imagePath = "";
                                    break;
                                case 2:
                                case 5:
                                    id = $"B{j}";
                                    name = "Bishop";
                                    imagePath = "";
                                    break;
                                case 3:
                                    id = $"Q{j}";
                                    name = "Queen";
                                    imagePath = "";
                                    break;
                                case 4:
                                    id = $"K{j}";
                                    name = "King";
                                    imagePath = "";
                                    break;
                            }
                            break;
                        case 1:
                        case 6:
                            id = $"P{j}";
                            name = "Pawn";
                            imagePath = "";
                            break;
                        default:

                            break;
                    }

                    Square square;

                    if (i <= 1 || i >= 6)
                    {
                        chessPiece = new ChessPiece(id, name, imagePath, side);
                        square = new Square(squareColor, chessPiece);
                    }
                    else
                        square = new Square(squareColor);

                    board[i, j] = square;
                }
            }

            return board;
        }
    }
}
