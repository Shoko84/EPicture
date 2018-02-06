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

        public MainWindow()
        {
            InitializeComponent();
            FlickrManager.Instance.Connect("b0cfac361f6ef2f56451b914bbb1faf9", "669e471cad095d80");
            exploreControl = new ExploreControl();
            uploadControl = new UploadControl();
            favoritesControl = new FavoritesControl();
            ContentControl.Content = exploreControl;
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
    }
}
