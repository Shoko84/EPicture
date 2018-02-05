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
    /// Interaction logic for TokenAuthentificationControl.xaml
    /// </summary>
    public partial class TokenAuthentificationControl : UserControl
    {
        public static readonly RoutedEvent ConfirmPublicKeyAPI =
            EventManager.RegisterRoutedEvent("ConfirmPublicKeyAPI", RoutingStrategy.Bubble,
            typeof(TokenInfoArgs), typeof(TokenAuthentificationControl));

        public TokenAuthentificationControl()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new TokenInfoArgs(TokenAuthentificationControl.ConfirmPublicKeyAPI, PublicKeyTextBox.Text));
        }
    }
}
