using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace epicture
{
    /// <summary>
    /// Container of static functions for graphics drawing
    /// </summary>
    public class GraphicUtils
    {
        /// <summary>
        /// Load an image by an url
        /// </summary>
        /// <param name="imageUrl">The image URL</param>
        /// <returns>An <see cref="Image"/></returns>
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

        /// <summary>
        /// Load an image by an url
        /// </summary>
        /// <param name="imageUrl">The image URL</param>
        /// <param name="width">The width of the <see cref="Image"/></param>
        /// <param name="width">The height of the <see cref="Image"/></param>
        /// <returns>An <see cref="Image"/></returns>
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
