using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace epicture
{
    public class GraphicUtils
    {
        public static Image LoadImage(string imageUrl)
        {
            Image image = new Image();
            var fullFilePath = imageUrl;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();

            image.Source = bitmap;

            return (image);
        }

        public static Image LoadImage(string imageUrl, uint width, uint height)
        {
            Image image = new Image();
            var fullFilePath = @imageUrl;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();

            image.Source = bitmap;
            image.Width = width;
            image.Height = height;

            return (image);
        }
    }
}
