using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace epicture
{
    public class PictureInfoArgs : RoutedEventArgs
    {
        private readonly string photoId;

        public string PhotoId
        {
            get { return photoId; }
        }

        public PictureInfoArgs(RoutedEvent routedEvent, string photoId) : base(routedEvent)
        {
            this.photoId = photoId;
        }
    }
}
