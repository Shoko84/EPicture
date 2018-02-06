using FlickrNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace epicture
{
    /// <summary>
    /// Interaction logic for Picture.xaml
    /// </summary>
    public partial class Picture : UserControl
    {
        Photo PhotoInfo;

        public static readonly RoutedEvent UserAuthenticatedRequestFromPictureEvent =
            EventManager.RegisterRoutedEvent("UserAuthenticatedRequestFromPictureEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(PictureViewer));

        public Picture(Photo photoInfo)
        {
            InitializeComponent();
            PhotoInfo = photoInfo;
            PictureTitle.Text = PhotoInfo.Title;
            Image img = GraphicUtils.LoadImage(PhotoInfo.Small320Url, 250, 250);
            Grid.SetRow(img, 1);
            PictureGrid.Children.Add(img);
        }

        private void PictureFavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (PhotoInfo.DateFavorited == null)
            {
                try
                {
                    FlickrManager.Instance.AddFavoritePicture(PhotoInfo.PhotoId);
                    PictureFavoriteButton.Content = "Unfavorite";
                }
                catch (UserNotAuthenticatedException ex)
                {
                    RaiseEvent(new RoutedEventArgs(Picture.UserAuthenticatedRequestFromPictureEvent));
                }
            }
            else
            {
                try
                {
                    FlickrManager.Instance.RemoveFavoritePicture(PhotoInfo.PhotoId);
                    PictureFavoriteButton.Content = "Unfavorite";
                }
                catch (UserNotAuthenticatedException ex)
                {

                }
            }
        }
    }
}
