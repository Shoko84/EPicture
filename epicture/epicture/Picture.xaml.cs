using FlickrNet;
using System.Windows;
using System.Windows.Controls;

namespace epicture
{
    /// <summary>
    /// Interaction logic for Picture.xaml
    /// </summary>
    public partial class Picture : UserControl
    {
        private Photo PhotoInfo;

        /// <summary>
        /// Event raised if the user is asking an action where he should be authentified from a <see cref="Picture"/>
        /// </summary>
        public static readonly RoutedEvent UserAuthenticatedRequestFromPictureEvent =
            EventManager.RegisterRoutedEvent("UserAuthenticatedRequestFromPictureEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(Picture));

        /// <summary>
        /// Event raised when the user is asking to change the favorite state of a <see cref="Picture"/>
        /// </summary>
        public static readonly RoutedEvent ChangeFavoriteFromPictureEvent =
            EventManager.RegisterRoutedEvent("ChangeFavoriteFromPictureEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(Picture));

        /// <summary>
        /// Constructor of the class <see cref="Picture"/>
        /// </summary>
        /// <param name="photoInfo">The API information of a picture</param>
        public Picture(Photo photoInfo)
        {
            InitializeComponent();
            PhotoInfo = photoInfo;
            PictureFavoriteButton.Content = (PhotoInfo.DateFavorited == null) ? ("Favorite") : ("Unfavorite");
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
                    FlickrManager.Instance.AddFavoritePictureAsync(PhotoInfo.PhotoId, delegate(FlickrResult<NoResponse> arg)
                    {
                        PictureFavoriteButton.Content = "Unfavorite";
                        RaiseEvent(new RoutedEventArgs(Picture.ChangeFavoriteFromPictureEvent));
                    });
                }
                catch (UserAuthenticationException)
                {
                    FlickrManager.Instance.UserAuthenticationRequest();
                    RaiseEvent(new RoutedEventArgs(Picture.UserAuthenticatedRequestFromPictureEvent));
                }
            }
            else
            {
                try
                {
                    FlickrManager.Instance.RemoveFavoritePictureAsync(PhotoInfo.PhotoId, delegate (FlickrResult<NoResponse> arg)
                    {
                        PictureFavoriteButton.Content = "Favorite";
                        RaiseEvent(new RoutedEventArgs(Picture.ChangeFavoriteFromPictureEvent));
                    });
                }
                catch (UserAuthenticationException)
                {
                    FlickrManager.Instance.UserAuthenticationRequest();
                    RaiseEvent(new RoutedEventArgs(Picture.UserAuthenticatedRequestFromPictureEvent));
                }
            }
        }
    }
}
