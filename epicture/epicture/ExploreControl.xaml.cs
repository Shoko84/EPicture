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
    /// Interaction logic for ExploreControl.xaml
    /// </summary>
    public partial class ExploreControl : UserControl
    {
        PictureViewer   pictureViewer;
        FlickrManager   FlickrManager;
        uint            CurrentPage;

        public ExploreControl(FlickrManager flickrManager)
        {
            InitializeComponent();
            CurrentPage = 1;
            FlickrManager = flickrManager;
            pictureViewer = new PictureViewer();
            PictureViewerControl.Content = pictureViewer;

            AddHandler(PictureViewer.PrevPageAskedEvent,
                       new RoutedEventHandler(PrevPageAskedHandler));

            AddHandler(PictureViewer.NextPageAskedEvent,
                       new RoutedEventHandler(NextPageAskedHandler));

            AddHandler(PictureViewer.SetFavoriteFromPictureViewerEvent,
                       new RoutedEventHandler(SetFavoriteFromPictureViewerHandler));

            AddHandler(PictureViewer.RemoveFavoriteFromPictureViewerEvent,
                       new RoutedEventHandler(RemoveFavoriteFromPictureViewerHandler));
        }

        private void SetFavoriteFromPictureViewerHandler(object sender, RoutedEventArgs e)
        {
            PictureInfoArgs args = e as PictureInfoArgs;

            FlickrManager.AddFavoritePicture(args.PhotoId);
        }

        private void RemoveFavoriteFromPictureViewerHandler(object sender, RoutedEventArgs e)
        {
            PictureInfoArgs args = e as PictureInfoArgs;

            FlickrManager.RemoveFavoritePicture(args.PhotoId);
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            pictureViewer.SetPictures(FlickrManager.SearchPhotos(SearchTextBox.Text, 20, CurrentPage), CurrentPage);
        }

        private void PrevPageAskedHandler(object sender, RoutedEventArgs e)
        {
            CurrentPage -= 1;
            pictureViewer.SetPictures(FlickrManager.SearchPhotos(SearchTextBox.Text, 20, CurrentPage), CurrentPage);
        }

        private void NextPageAskedHandler(object sender, RoutedEventArgs e)
        {
            CurrentPage += 1;
            pictureViewer.SetPictures(FlickrManager.SearchPhotos(SearchTextBox.Text, 20, CurrentPage), CurrentPage);
        }
    }
}
