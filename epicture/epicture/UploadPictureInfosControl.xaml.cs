using System.Windows;
using System.Windows.Controls;

namespace epicture
{
    /// <summary>
    /// Interaction logic for UploadPictureInfosControl.xaml
    /// </summary>
    public partial class UploadPictureInfosControl : UserControl
    {
        /// <summary>
        /// Event raised when the user is clicking on the "Cancel" button on the <see cref="UploadPictureInfosControl"/>
        /// </summary>
        public static readonly RoutedEvent CancelUploadPictureInfosEvent =
            EventManager.RegisterRoutedEvent("CancelUploadPictureInfosEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(UploadPictureInfosControl));

        /// <summary>
        /// Event raised when the user is clicking on the "Confirm" button on the <see cref="UploadPictureInfosControl"/>
        /// </summary>
        public static readonly RoutedEvent ConfirmUploadPictureInfosEvent =
            EventManager.RegisterRoutedEvent("ConfirmUploadPictureInfosEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(UploadPictureInfosControl));

        /// <summary>
        /// Constructor of the class <see cref="UploadPictureInfosControl"/>
        /// </summary>
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
            {
                CancelButton.IsEnabled = false;
                ConfirmButton.IsEnabled = false;
                RaiseEvent(new RoutedEventArgs(UploadPictureInfosControl.ConfirmUploadPictureInfosEvent));
            }
        }
    }
}
