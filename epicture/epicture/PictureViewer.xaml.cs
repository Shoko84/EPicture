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
    /// Interaction logic for PictureViewer.xaml
    /// </summary>
    public partial class PictureViewer : UserControl
    {
        PhotoCollection Pictures;
        uint            CurrentPage;

        public static readonly RoutedEvent UserAuthenticatedRequestFromPictureViewerEvent =
            EventManager.RegisterRoutedEvent("UserAuthenticatedRequestFromPictureViewerEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(PictureViewer));

        public PictureViewer()
        {
            InitializeComponent();
            CurrentPage = 1;
            Pictures = new PhotoCollection();

            AddHandler(Picture.UserAuthenticatedRequestFromPictureEvent,
                       new RoutedEventHandler(UserAuthenticatedRequestFromPictureHandler));

            AddHandler(Picture.ChangeFavoriteEvent,
                       new RoutedEventHandler(ChangeFavoriteHandler));
        }

        private void UserAuthenticatedRequestFromPictureHandler(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PictureViewer.UserAuthenticatedRequestFromPictureViewerEvent));
        }
        private void ChangeFavoriteHandler(object sender, RoutedEventArgs e)
        {
            //PictureInfoArgs args = e as PictureInfoArgs;

            SetPictures(FlickrManager.Instance.SearchPhotos());
        }

        public void SetPictures(PhotoCollection pictures)
        {
            Pictures = pictures;
            PrevPageButton.IsEnabled = (CurrentPage == 1) ? (false) : (true);
            RefreshPicturesOnScreen();
        }
        public void SetPictures(PhotoCollection pictures, uint currentPage)
        {
            Pictures = pictures;
            CurrentPage = currentPage;
            PrevPageButton.IsEnabled = (CurrentPage == 1) ? (false) : (true);
            RefreshPicturesOnScreen();
        }

        private void RefreshPicturesOnScreen()
        {
            ViewerContainer.Children.Clear();
            for (var i = 0; i < Pictures.Count; ++i)
            {
                var p = Pictures[i];
                Picture picture = new Picture(Pictures[i])
                {
                    Margin = new Thickness(15)
                };
                ViewerContainer.Children.Add(picture);
            }
        }

        private void PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage -= 1;
            if (CurrentPage == 1)
                PrevPageButton.IsEnabled = false;
            SetPictures(FlickrManager.Instance.SearchPhotos(20, CurrentPage), CurrentPage);
            ScrollerViewer.ScrollToTop();
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage += 1;
            PrevPageButton.IsEnabled = true;
            SetPictures(FlickrManager.Instance.SearchPhotos(20, CurrentPage), CurrentPage);
            ScrollerViewer.ScrollToTop();
        }
    }
}
