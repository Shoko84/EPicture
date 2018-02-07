using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FlickrNet;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        ExploreControl                  exploreControl;
        FavoritesControl                favoritesControl;
        UploadControl                   uploadControl;
        TokenAuthentificationControl    tokenAuthentificationControl;
        UserControl                     processingControl;

        public MainWindow()
        {
            InitializeComponent();
            FlickrManager.Instance.Connect("b0cfac361f6ef2f56451b914bbb1faf9", "669e471cad095d80");
            exploreControl = new ExploreControl();
            favoritesControl = new FavoritesControl();
            uploadControl = new UploadControl();
            ContentControl.Content = exploreControl;

            AddHandler(ExploreControl.UserAuthenticatedRequestFromExploreControlEvent,
                       new RoutedEventHandler(UserAuthenticatedRequestFromExploreControlHandler));

            AddHandler(FavoritesControl.UserAuthenticatedRequestFromFavoritesControlEvent,
                       new RoutedEventHandler(UserAuthenticatedRequestFromFavoritesControlHandler));

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

        private void ConfirmUserTokenHandler(object sender, RoutedEventArgs e)
        {
            ExploreControl _exploreControl = processingControl as ExploreControl;
            FavoritesControl _favoritesControl = processingControl as FavoritesControl;
            UploadControl _uploadControl = processingControl as UploadControl;

            if (_exploreControl != null)
                _exploreControl.PictureViewer.SetPictures(FlickrManager.Instance.SearchPhotos());
            else if (_favoritesControl != null)
                _favoritesControl.PictureViewer.SetPictures(FlickrManager.Instance.SearchFavorites());
            else if (_uploadControl != null)
            {

            }

            ContentControl.Content = processingControl;
            processingControl = null;
            if (favoritesControl != null)
            {
                favoritesControl.UserInfos = FlickrManager.Instance.UserInfos();
                if (FlickrManager.Instance.LocalUserId == favoritesControl.UserInfos.UserId)
                {
                    favoritesControl.PictureViewer.HideFavoriteButton = false;
                    favoritesControl.PictureViewer.SetPictures(FlickrManager.Instance.SearchFavorites(FlickrManager.Instance.LocalUserId, FlickrManager.SearchType.USERID), 1);
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
            //if (uploadControl == null)
            //    uploadControl = new UploadControl();
            //ContentControl.Content = uploadControl;
        }

        private void FavoritesLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentControl.Content = favoritesControl;
        }
    }
}
