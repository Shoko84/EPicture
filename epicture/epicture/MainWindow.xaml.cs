using System;
using System.Windows;
using FlickrNet;
using System.Windows.Controls;
using System.Windows.Input;

namespace epicture
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private ExploreControl                  exploreControl;
        private FavoritesControl                favoritesControl;
        private UploadControl                   uploadControl;
        private TokenAuthentificationControl    tokenAuthentificationControl;
        private UserControl                     processingControl;

        /// <summary>
        /// Constructor of the class <see cref="MainWindow"/>
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            FlickrManager.Instance.Connect("b0cfac361f6ef2f56451b914bbb1faf9", "669e471cad095d80");
            exploreControl = new ExploreControl();
            favoritesControl = new FavoritesControl();
            uploadControl = new UploadControl();
            processingControl = null;
            ContentControl.Content = exploreControl;

            AddHandler(ExploreControl.UserAuthenticatedRequestFromExploreControlEvent,
                       new RoutedEventHandler(UserAuthenticatedRequestFromExploreControlHandler));

            AddHandler(FavoritesControl.UserAuthenticatedRequestFromFavoritesControlEvent,
                       new RoutedEventHandler(UserAuthenticatedRequestFromFavoritesControlHandler));

            AddHandler(UploadControl.UserAuthenticatedRequestFromUploadControlEvent,
                       new RoutedEventHandler(UserAuthenticatedRequestFromUploadControlHandler));

            AddHandler(TokenAuthentificationControl.ConfirmUserTokenEvent,
                       new RoutedEventHandler(ConfirmUserTokenHandler));
        }

        private void UserAuthenticatedRequestFromExploreControlHandler(object sender, RoutedEventArgs e)
        {
            processingControl = exploreControl;
            tokenAuthentificationControl = new TokenAuthentificationControl();
            ContentControl.Content = tokenAuthentificationControl;
        }

        private void UserAuthenticatedRequestFromFavoritesControlHandler(object sender, RoutedEventArgs e)
        {
            processingControl = favoritesControl;
            tokenAuthentificationControl = new TokenAuthentificationControl();
            ContentControl.Content = tokenAuthentificationControl;
        }

        private void UserAuthenticatedRequestFromUploadControlHandler(object sender, RoutedEventArgs e)
        {
            processingControl = uploadControl;
            tokenAuthentificationControl = new TokenAuthentificationControl();
            ContentControl.Content = tokenAuthentificationControl;
        }

        private void ConfirmUserTokenHandler(object sender, RoutedEventArgs e)
        {
            UsernameLabel.Visibility = Visibility.Visible;
            UsernameValueLabel.Text = FlickrManager.Instance.UserInfos().Username;
            UsernameValueLabel.Visibility = Visibility.Visible;
            LoginButton.Content = "Logout";

            ExploreControl _exploreControl = processingControl as ExploreControl;
            FavoritesControl _favoritesControl = processingControl as FavoritesControl;
            UploadControl _uploadControl = processingControl as UploadControl;

            if (_exploreControl != null)
                FlickrManager.Instance.SearchPhotosAsync(delegate (PhotoCollection photos)
                {
                    _exploreControl.PictureViewer.SetPictures(photos);
                });
            else if (_favoritesControl != null && FlickrManager.Instance.LocalFavoriteUserId == FlickrManager.Instance.UserInfos().UserId)
                FlickrManager.Instance.SearchFavoritesAsync(delegate (FlickrResult<PhotoCollection> photos)
                {
                    _favoritesControl.PictureViewer.SetPictures(photos.Result);
                });
            else if (_uploadControl != null)
            {
                FlickrManager.Instance.SearchUploadedPicturesAsync(delegate (FlickrResult<PhotoCollection> photos)
                {
                    _uploadControl.PictureViewer.SetPictures(photos.Result);
                });
                _uploadControl.UploadedPicturesLabel.Visibility = Visibility.Visible;
                _uploadControl.PictureViewerControl.Visibility = Visibility.Visible;
            }

            ContentControl.Content = processingControl;
            processingControl = null;
            if (favoritesControl != null)
            {
                if (FlickrManager.Instance.LocalFavoriteUserId == FlickrManager.Instance.UserInfos().UserId)
                {
                    favoritesControl.PictureViewer.HideFavoriteButton = false;
                    FlickrManager.Instance.SearchFavoritesAsync(FlickrManager.Instance.LocalFavoriteUserId, FlickrManager.SearchType.USERID, delegate(FlickrResult<PhotoCollection> photos)
                    {
                        favoritesControl.PictureViewer.SetPictures(photos.Result, 1);
                    });
                }
            }
        }

        private void TopNavBarButtonHandler_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(TopNavBarButtonHandler.Content) == "^")
            {
                TopNavBarButtonHandler.Content = "v";
                WindowGrid.RowDefinitions[0].Height = new GridLength(25);
                TopNavBarButtonHandler.VerticalContentAlignment = VerticalAlignment.Center;
                ExploreLabel.Visibility = Visibility.Hidden;
                UploadLabel.Visibility = Visibility.Hidden;
                FavoritesLabel.Visibility = Visibility.Hidden;
            }
            else
            {
                TopNavBarButtonHandler.Content = "^";
                WindowGrid.RowDefinitions[0].Height = new GridLength(70);
                TopNavBarButtonHandler.VerticalContentAlignment = VerticalAlignment.Top;
                ExploreLabel.Visibility = Visibility.Visible;
                UploadLabel.Visibility = Visibility.Visible;
                FavoritesLabel.Visibility = Visibility.Visible;
            }
        }

        private void ExploreLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentControl.Content = exploreControl;
        }

        private void UploadLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentControl.Content = uploadControl;
        }

        private void FavoritesLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentControl.Content = favoritesControl;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(LoginButton.Content) == "Login")
            {
                processingControl = ContentControl.Content as UserControl;
                FlickrManager.Instance.UserAuthenticationRequest();
                tokenAuthentificationControl = new TokenAuthentificationControl();
                ContentControl.Content = tokenAuthentificationControl;
            }
            else
            {
                FlickrManager.Instance.Disconnect();
                uploadControl.ResetUploadControlTab(true);
                uploadControl.PictureViewer.SetPictures(new PhotoCollection());
                uploadControl.PictureViewerControl.Visibility = Visibility.Hidden;
                uploadControl.UploadedPicturesLabel.Visibility = Visibility.Hidden;

                UsernameLabel.Visibility = Visibility.Hidden;
                UsernameValueLabel.Text = "";
                UsernameValueLabel.Visibility = Visibility.Hidden;
                favoritesControl.PictureViewer.HideFavoriteButton = true;
                if (FlickrManager.Instance.LocalFavoriteUserId != "")
                    FlickrManager.Instance.SearchFavoritesAsync(FlickrManager.Instance.LocalFavoriteUserId, 1, FlickrManager.SearchType.USERID, delegate (FlickrResult<PhotoCollection> photos)
                    {
                        favoritesControl.PictureViewer.SetPictures(photos.Result, 1);
                    });
                LoginButton.Content = "Login";
            }
        }
    }
}
