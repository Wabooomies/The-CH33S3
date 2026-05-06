using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace The_CH33S3.Models
{
    public class Square : INotifyPropertyChanged
    {
        private ChessPiece? _piece = null;
        private string? _color;
        private int[] _position = new int[2];

        public ChessPiece? Piece
        {
            get => _piece;
            set
            {
                if (_piece != value)
                {
                    _piece = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Piece)));
                }
            }
        }

        public string? Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Color)));
                }
            }
        }

        public int[] Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Position)));
                }
            }
        }

        public Square(string color, int[] position, ChessPiece? piece = null)
        {
            Color = color;
            Position = position;
            Piece = piece;
        }

        public Square()
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
