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
        public PictureViewer   PictureViewer;

        public static readonly RoutedEvent UserAuthenticatedRequestFromExploreControlEvent =
            EventManager.RegisterRoutedEvent("UserAuthenticatedRequestFromExploreControlEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(ExploreControl));

        public ExploreControl()
        {
            InitializeComponent();
            PictureViewer = new PictureViewer();
            PictureViewer.NextPageButton.IsEnabled = false;
            PictureViewerControl.Content = PictureViewer;

            AddHandler(PictureViewer.UserAuthenticatedRequestFromPictureViewerEvent,
                       new RoutedEventHandler(UserAuthenticatedRequestFromPictureViewerHandler));

            AddHandler(PictureViewer.ChangeFavoriteFromPictureViewerEvent,
                       new RoutedEventHandler(ChangeFavoriteFromPictureViewerHandler));

            AddHandler(PictureViewer.PreviousPageClickedEvent,
                       new RoutedEventHandler(PageChangedClickedHandler));

            AddHandler(PictureViewer.NextPageClickedEvent,
                       new RoutedEventHandler(PageChangedClickedHandler));
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text != "")
            {
                PictureViewer.SetCurrentPage(1);
                PictureViewer.NextPageButton.IsEnabled = true;
                PictureViewer.SetPictures(FlickrManager.Instance.SearchPhotos(SearchTextBox.Text, 20, 1), 1);
                PictureViewer.ScrollerViewer.ScrollToTop();
            }
        }

        private void UserAuthenticatedRequestFromPictureViewerHandler(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ExploreControl.UserAuthenticatedRequestFromExploreControlEvent));
        }

        private void ChangeFavoriteFromPictureViewerHandler(object sender, RoutedEventArgs e)
        {
            PictureViewer.SetPictures(FlickrManager.Instance.SearchPhotos());
        }
 
        private void PageChangedClickedHandler(object sender, RoutedEventArgs e)
        {
            PictureViewer.SetPictures(FlickrManager.Instance.SearchPhotos(20, PictureViewer.CurrentPage));
            PictureViewer.ScrollerViewer.ScrollToTop();
        }
    }
}
