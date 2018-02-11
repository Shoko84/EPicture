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
using Microsoft.Win32;

namespace epicture
{
    /// <summary>
    /// Interaction logic for UploadControl.xaml
    /// </summary>
    public partial class UploadControl : UserControl
    {
        public PictureViewer PictureViewer;
        private string filePathPicture;
        private OpenFileDialog fileSelectWindow;
        private UploadPictureInfosControl uploadPictureStep2Control;

        public static readonly RoutedEvent UserAuthenticatedRequestFromUploadControlEvent =
            EventManager.RegisterRoutedEvent("UserAuthenticatedRequestFromUploadControlEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(UploadControl));

        public UploadControl()
        {
            InitializeComponent();
            filePathPicture = null;
            fileSelectWindow = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
            };
            PictureViewer = new PictureViewer();
            PictureViewer.HideFavoriteButton = true;
            PictureViewer.PrevPageButton.Visibility = Visibility.Hidden;
            PictureViewer.NextPageButton.Visibility = Visibility.Hidden;
            if (FlickrManager.Instance.IsUserAuthenticated())
            {
                UploadedPicturesLabel.Visibility = Visibility.Visible;
                PictureViewer.Visibility = Visibility.Visible;
            }
            PictureViewerControl.Content = PictureViewer;

            AddHandler(UploadPictureInfosControl.CancelUploadPictureInfosEvent,
                       new RoutedEventHandler(CancelUploadPictureInfosHandler));

            AddHandler(UploadPictureInfosControl.ConfirmUploadPictureInfosEvent,
                       new RoutedEventHandler(ConfirmUploadPictureInfosHandler));
        }

        private void CancelUploadPictureInfosHandler(object sender, RoutedEventArgs e)
        {
            ResetUploadControlTab();
        }

        private void ConfirmUploadPictureInfosHandler(object sender, RoutedEventArgs e)
        {
            UploadPictureInfosControl control = UploadControlStep2.Content as UploadPictureInfosControl;

            FlickrManager.Instance.UploadPicture(filePathPicture, control.DescriptionInput.Text, control.TagsInput.Text, control.TagsInput.Text,
                                                 control.PublicCheckbox.IsChecked ?? false, control.FamilyCheckbox.IsChecked ?? false, control.FriendsCheckbox.IsChecked ?? false);
            PictureViewer.SetPictures(FlickrManager.Instance.SearchUploadedPictures());
            FileSelectedLabel.Text = "No file selected";
            filePathPicture = null;
            ResetUploadControlTab();
        }

        public void ResetUploadControlTab(bool resetFilePath = false)
        {
            if (resetFilePath)
            {
                FileSelectedLabel.Text = "No file selected";
                filePathPicture = null;
            }
            TopLabelInfo.Visibility = Visibility.Visible;
            FileSelectionContainer.Visibility = Visibility.Visible;
            UploadedPicturesLabel.Visibility = Visibility.Visible;
            PictureViewerControl.Visibility = Visibility.Visible;
            UploadControlStep2.Content = null;
            UploadControlStep2.Visibility = Visibility.Hidden;
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (fileSelectWindow.ShowDialog() == true)
            {
                filePathPicture = fileSelectWindow.FileName;
                FileSelectedLabel.Text = filePathPicture;
            }
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            if (filePathPicture == null)
                MessageBox.Show("Please select a valid picture to upload", "Invalid picture", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                if (FlickrManager.Instance.IsUserAuthenticated())
                {
                    uploadPictureStep2Control = new UploadPictureInfosControl();
                    TopLabelInfo.Visibility = Visibility.Hidden;
                    FileSelectionContainer.Visibility = Visibility.Hidden;
                    UploadedPicturesLabel.Visibility = Visibility.Hidden;
                    PictureViewerControl.Visibility = Visibility.Hidden;
                    UploadControlStep2.Content = uploadPictureStep2Control;
                    UploadControlStep2.Visibility = Visibility.Visible;
                }
                else
                {
                    FlickrManager.Instance.UserAuthenticationRequest();
                    RaiseEvent(new RoutedEventArgs(UploadControl.UserAuthenticatedRequestFromUploadControlEvent));
                }
            }
        }
    }
}
