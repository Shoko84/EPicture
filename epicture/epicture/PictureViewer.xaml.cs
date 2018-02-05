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

        public static readonly RoutedEvent PrevPageAskedEvent =
            EventManager.RegisterRoutedEvent("PrevPageAskedEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(PictureViewer));

        public static readonly RoutedEvent NextPageAskedEvent =
            EventManager.RegisterRoutedEvent("NextPageAskedEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(PictureViewer));

        public static readonly RoutedEvent SetFavoriteFromPictureViewerEvent =
            EventManager.RegisterRoutedEvent("SetFavoriteFromPictureViewerEvent", RoutingStrategy.Bubble,
            typeof(PictureInfoArgs), typeof(PictureViewer));

        public static readonly RoutedEvent RemoveFavoriteFromPictureViewerEvent =
            EventManager.RegisterRoutedEvent("RemoveFavoriteFromPictureViewerEvent", RoutingStrategy.Bubble,
            typeof(PictureInfoArgs), typeof(PictureViewer));

        public PictureViewer()
        {
            InitializeComponent();
            CurrentPage = 1;
            Pictures = new PhotoCollection();

            AddHandler(Picture.SetFavoriteFromPictureEvent,
                       new RoutedEventHandler(SetFavoriteFromPictureHandler));

            AddHandler(Picture.RemoveFavoriteFromPictureEvent,
                       new RoutedEventHandler(RemoveFavoriteFromPictureHandler));
        }

        private void SetFavoriteFromPictureHandler(object sender, RoutedEventArgs e)
        {
            PictureInfoArgs args = e as PictureInfoArgs;

            RaiseEvent(new PictureInfoArgs(PictureViewer.SetFavoriteFromPictureViewerEvent, args.PhotoId));
        }

        private void RemoveFavoriteFromPictureHandler(object sender, RoutedEventArgs e)
        {
            PictureInfoArgs args = e as PictureInfoArgs;

            RaiseEvent(new PictureInfoArgs(PictureViewer.RemoveFavoriteFromPictureViewerEvent, args.PhotoId));
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
            RaiseEvent(new RoutedEventArgs(PictureViewer.PrevPageAskedEvent));
            ScrollerViewer.ScrollToTop();
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage += 1;
            PrevPageButton.IsEnabled = true;
            RaiseEvent(new RoutedEventArgs(PictureViewer.NextPageAskedEvent));
            ScrollerViewer.ScrollToTop();
        }
    }
}
