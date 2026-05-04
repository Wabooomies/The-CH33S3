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
        private Brush? _color;

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

        public Brush? Color
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

        public Square(Brush color, ChessPiece? piece = null)
        {
            Color = color;
            Piece = piece;
        }

        public Square()
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
