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
        private readonly string publicKey;

        public string PublicKey
        {
            get { return publicKey; }
        }

        public TokenInfoArgs(RoutedEvent routedEvent, string publicKey) : base(routedEvent)
        {
            this.publicKey = publicKey;
        }
    }
}
