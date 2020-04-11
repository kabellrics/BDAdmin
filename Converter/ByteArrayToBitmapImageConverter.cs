using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BDAdmin.Converter
{
    public class ByteArrayToBitmapImageConverter : IValueConverter
    {
        public BitmapImage ConvertByteArrayToBitMapImage(byte[] imageByteArray)
        {
            BitmapImage img = new BitmapImage();
            using (MemoryStream memStream = new MemoryStream(imageByteArray))
            {
                img.StreamSource = memStream;
            }
            return img;
        }



        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage img = new BitmapImage();
            if (value != null)
            {
                img = this.ConvertByteArrayToBitMapImage(value as byte[]);
            }
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
