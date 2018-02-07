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
        private uint    currentPage;
        public uint     CurrentPage { get { return currentPage; } }
        public bool     HideFavoriteButton { get; set; }

        public static readonly RoutedEvent UserAuthenticatedRequestFromPictureViewerEvent =
            EventManager.RegisterRoutedEvent("UserAuthenticatedRequestFromPictureViewerEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(PictureViewer));

        public static readonly RoutedEvent ChangeFavoriteFromPictureViewerEvent =
            EventManager.RegisterRoutedEvent("ChangeFavoriteFromPictureViewerEvent", RoutingStrategy.Bubble,
            typeof(PictureInfoArgs), typeof(PictureViewer));

        public static readonly RoutedEvent PreviousPageClickedEvent =
            EventManager.RegisterRoutedEvent("PreviousPageClickedEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(PictureViewer));

        public static readonly RoutedEvent NextPageClickedEvent =
            EventManager.RegisterRoutedEvent("NextPageClickedEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(PictureViewer));

        public PictureViewer()
        {
            InitializeComponent();
            currentPage = 1;
            HideFavoriteButton = false;
            Pictures = new PhotoCollection();

            AddHandler(Picture.UserAuthenticatedRequestFromPictureEvent,
                       new RoutedEventHandler(UserAuthenticatedRequestFromPictureHandler));

            AddHandler(Picture.ChangeFavoriteFromPictureEvent,
                       new RoutedEventHandler(ChangeFavoriteHandler));
        }

        public void SetCurrentPage(uint page)
        {
            currentPage = (page == 0) ? (1) : (page);
            if (page == 1)
                PrevPageButton.IsEnabled = false;
        }

        private void UserAuthenticatedRequestFromPictureHandler(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PictureViewer.UserAuthenticatedRequestFromPictureViewerEvent));
        }
        private void ChangeFavoriteHandler(object sender, RoutedEventArgs e)
        {
            PictureInfoArgs args = e as PictureInfoArgs;

            RaiseEvent(new PictureInfoArgs(PictureViewer.ChangeFavoriteFromPictureViewerEvent, args.Photo));
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
            this.currentPage = currentPage;
            PrevPageButton.IsEnabled = (CurrentPage == 1) ? (false) : (true);
            RefreshPicturesOnScreen();
        }

        private void RefreshPicturesOnScreen()
        {
            ViewerContainer.Children.Clear();
            var p = Pictures.Count;
            for (var i = 0; i < Pictures.Count; ++i)
            {
                Picture picture = new Picture(Pictures[i]);
                picture.Margin = new Thickness(15);
                picture.PictureFavoriteButton.Visibility = (HideFavoriteButton) ? (Visibility.Hidden) : (Visibility.Visible);
                ViewerContainer.Children.Add(picture);
            }
        }

        private void PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
            currentPage -= 1;
            if (CurrentPage == 1)
                PrevPageButton.IsEnabled = false;
            RaiseEvent(new RoutedEventArgs(PictureViewer.PreviousPageClickedEvent));
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            currentPage += 1;
            PrevPageButton.IsEnabled = true;
            RaiseEvent(new RoutedEventArgs(PictureViewer.NextPageClickedEvent));
        }
    }
}
