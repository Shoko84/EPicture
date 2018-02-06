using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace epicture
{
    public class TokenInfoArgs : RoutedEventArgs
    {
        private readonly string token;

        public string Token
        {
            get { return token; }
        }

        public TokenInfoArgs(RoutedEvent routedEvent, string token) : base(routedEvent)
        {
            this.token = token;
        }
    }
}
