using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace The_CH33S3.Models
{
    internal class Convert
    {
        public byte[] ImageToByteArray(BitmapImage image)
        {
            byte[] data;

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var ms = new System.IO.MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }

    }
}
