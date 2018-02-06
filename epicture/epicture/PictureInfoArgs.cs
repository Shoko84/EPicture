using FlickrNet;
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
        private readonly Photo photo;

        public Photo Photo
        {
            get { return photo; }
        }

        public PictureInfoArgs(RoutedEvent routedEvent, Photo photo) : base(routedEvent)
        {
            this.photo = photo;
        }
    }
}
