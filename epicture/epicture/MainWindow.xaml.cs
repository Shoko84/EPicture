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
    public partial class MainWindow : Window
    {
        FlickrManager                   flickrManager;
        TokenAuthentificationControl    tokenAuthControl;
        ExploreControl                  exploreControl;
        UploadControl                   uploadControl;

        public MainWindow()
        {
            InitializeComponent();
            TopNavBar.Visibility = Visibility.Hidden;
            tokenAuthControl = new TokenAuthentificationControl();
            exploreControl = new ExploreControl();
            uploadControl = new UploadControl();
            ContentControl.Content = tokenAuthControl;

            AddHandler(TokenAuthentificationControl.ConfirmPublicKeyAPI,
                       new RoutedEventHandler(ConfirmPublicKeyAPIHandler));
        }

        private void ConfirmPublicKeyAPIHandler(object sender, RoutedEventArgs e)
        {
            TokenInfoArgs args = e as TokenInfoArgs;

            flickrManager = new FlickrManager(args.PublicKey);
            ContentControl.Content = exploreControl;
            TopNavBar.Visibility = Visibility.Visible;
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
            }
            else
            {
                TopNavBarButtonHandler.Content = "^";
                WindowGrid.RowDefinitions[0].Height = new GridLength(70);
                TopNavBarButtonHandler.VerticalContentAlignment = VerticalAlignment.Top;
                ExploreLabel.Visibility = Visibility.Visible;
                UploadLabel.Visibility = Visibility.Visible;
            }
        }
    }
}
