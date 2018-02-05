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

        public FlickrManager(string publicKey, string secretKey)
        {
            Client = new Flickr(publicKey, secretKey);
        }

        public PhotoCollection SearchPhotos(string tags, uint perPage, uint page)
        {
            var options = new PhotoSearchOptions { Tags = tags, PerPage = Convert.ToInt32(perPage), Page = Convert.ToInt32(page) };
            PhotoCollection photos = Client.PhotosSearch(options);

            return (photos);
        }

        public PhotoCollection SearchFavorites()
        {
            PhotoCollection photos = Client.FavoritesGetList();

            return (photos);
        }

        public void AddFavoritePicture(string photoId)
        {
            if (Client.IsAuthenticated)
                Client.FavoritesAdd(photoId);
        }

        public void RemoveFavoritePicture(string photoId)
        {
            if (Client.IsAuthenticated)
                Client.FavoritesRemove(photoId);
        }
    }
}
