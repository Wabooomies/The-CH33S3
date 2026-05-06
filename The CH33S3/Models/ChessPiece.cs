using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_CH33S3.Models
{
    public class ChessPiece
    {
        private string? _id;
        private string? _name;
        private string? _imagePath;
        private string? _side;

        public string? Id
        {
            get => _id;
            set => _id = value;
        }

        public string? Name
        {
            get => _name;
            set => _name = value;
        }

        public string? ImagePath
        {
            get => _imagePath;
            set => _imagePath = value;
        }

        public string? Side
        {
            get => _side;
            set => _side = value;
        }

        public ChessPiece(string id, string name, string path, int side)
        {
            switch (side)
            {
                case 0:
                    _side = "White";
                    break;
                case 1:
                    _side = "Black";
                    break;
                default:
                    throw new ArgumentException("Invalid side value. Must be 0 (White) or 1 (Black).");
            }

            Id = id;
            Name = name;
            ImagePath = path;
        }

        public ChessPiece()
        {

        }
    }
}
