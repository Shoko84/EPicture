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
        uint            CurrentPage;

        public ExploreControl()
        {
            InitializeComponent();
            CurrentPage = 1;
            pictureViewer = new PictureViewer();
            PictureViewerControl.Content = pictureViewer;

            AddHandler(PictureViewer.PrevPageAskedEvent,
                       new RoutedEventHandler(PrevPageAskedHandler));

            AddHandler(PictureViewer.NextPageAskedEvent,
                       new RoutedEventHandler(NextPageAskedHandler));
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            pictureViewer.SetPictures(FlickrManager.Instance.SearchPhotos(SearchTextBox.Text, 20, CurrentPage), CurrentPage);
        }

        private void PrevPageAskedHandler(object sender, RoutedEventArgs e)
        {
            CurrentPage -= 1;
            pictureViewer.SetPictures(FlickrManager.Instance.SearchPhotos(SearchTextBox.Text, 20, CurrentPage), CurrentPage);
        }

        private void NextPageAskedHandler(object sender, RoutedEventArgs e)
        {
            CurrentPage += 1;
            pictureViewer.SetPictures(FlickrManager.Instance.SearchPhotos(SearchTextBox.Text, 20, CurrentPage), CurrentPage);
        }
    }
}
