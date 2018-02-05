using FlickrNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epicture
{
    public class FlickrManager
    {
        private Flickr Client;

        public FlickrManager(string publicKey)
        {
            //3615954777bd6d01b49492bfd402debf
            Client = new Flickr(publicKey);
        }

        public PhotoCollection SearchPhotos(string tags, uint perPage, uint page)
        {
            var options = new PhotoSearchOptions { Tags = tags, PerPage = Convert.ToInt32(perPage), Page = Convert.ToInt32(page) };
            PhotoCollection photos = Client.PhotosSearch(options);

            return (photos);
        }
    }
}
