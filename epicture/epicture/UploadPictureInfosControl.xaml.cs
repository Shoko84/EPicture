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
    /// Interaction logic for UploadPictureInfosControl.xaml
    /// </summary>
    public partial class UploadPictureInfosControl : UserControl
    {
        public static readonly RoutedEvent CancelUploadPictureInfosEvent =
            EventManager.RegisterRoutedEvent("CancelUploadPictureInfosEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(UploadPictureInfosControl));

        public static readonly RoutedEvent ConfirmUploadPictureInfosEvent =
            EventManager.RegisterRoutedEvent("ConfirmUploadPictureInfosEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(UploadPictureInfosControl));

        public UploadPictureInfosControl()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(UploadPictureInfosControl.CancelUploadPictureInfosEvent));
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (TitleInput.Text == "")
                MessageBox.Show("Enter a non-empty title", "Empty title", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
                RaiseEvent(new RoutedEventArgs(UploadPictureInfosControl.ConfirmUploadPictureInfosEvent));
        }
    }
}
