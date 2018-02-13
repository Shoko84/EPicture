using System.Windows;
using System.Windows.Controls;

namespace epicture
{
    /// <summary>
    /// Interaction logic for TokenAuthentificationControl.xaml
    /// </summary>
    public partial class TokenAuthentificationControl : UserControl
    {
        /// <summary>
        /// Event raised if the user is asking an action where he should be authentified from <see cref="UploadControl"/>
        /// </summary>
        public static readonly RoutedEvent ConfirmUserTokenEvent =
            EventManager.RegisterRoutedEvent("ConfirmUserTokenEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(TokenAuthentificationControl));

        /// <summary>
        /// Constructor of the class <see cref="TokenAuthentificationControl"/>
        /// </summary>
        public TokenAuthentificationControl()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FlickrManager.Instance.UserAuthenticationAnswer(PublicKeyTextBox.Text);
                RaiseEvent(new RoutedEventArgs(TokenAuthentificationControl.ConfirmUserTokenEvent));
            }
            catch (UserAuthenticationException)
            {
                MessageBox.Show("The token was incorrect, please try again", "Incorrect token", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
