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
        TokenAuthentificationControl tokenAuthentificationControl;

        public ExploreControl()
        {
            InitializeComponent();
            pictureViewer = new PictureViewer();
            PictureViewerControl.Content = pictureViewer;

            AddHandler(PictureViewer.UserAuthenticatedRequestFromPictureViewerEvent,
                       new RoutedEventHandler(UserAuthenticatedRequestFromPictureViewerHandler));

            AddHandler(TokenAuthentificationControl.ConfirmUserTokenEvent,
                       new RoutedEventHandler(ConfirmUserTokenHandler));
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            pictureViewer.SetPictures(FlickrManager.Instance.SearchPhotos(SearchTextBox.Text, 20, 1), 1);
        }

        private void UserAuthenticatedRequestFromPictureViewerHandler(object sender, RoutedEventArgs e)
        {
            tokenAuthentificationControl = new TokenAuthentificationControl();
            PictureViewerControl.Content = tokenAuthentificationControl;
        }

        private void ConfirmUserTokenHandler(object sender, RoutedEventArgs e)
        {
            pictureViewer.SetPictures(FlickrManager.Instance.SearchPhotos());
            PictureViewerControl.Content = pictureViewer;
        }
    }
}
