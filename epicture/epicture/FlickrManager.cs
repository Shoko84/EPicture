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
        private OAuthRequestToken requestToken;
        private static FlickrManager instance;
        public static FlickrManager Instance
        {
            get {
                if (instance == null)
                    instance = new FlickrManager();
                return (instance);
            }
        }

        private FlickrManager()
        {

        }

        public void Connect(string publicKey, string secretKey)
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
            if (Client.IsAuthenticated)
            {
                PhotoCollection photos = Client.FavoritesGetList();

                return (photos);
            }
            else
                throw new UserNotAuthenticatedException("The user is not authenticated.");
        }

        public void AddFavoritePicture(string photoId)
        {
            if (Client.IsAuthenticated)
                Client.FavoritesAdd(photoId);
            else
                throw new UserNotAuthenticatedException("The user is not authenticated.");
        }

        public void RemoveFavoritePicture(string photoId)
        {
            if (Client.IsAuthenticated)
                Client.FavoritesRemove(photoId);
            else
                throw new UserNotAuthenticatedException("The user is not authenticated.");
        }

        public void UserAuthenticationRequest()
        {
            FlickrManager f = FlickrManager.Instance;
            requestToken = f.Client.OAuthGetRequestToken("oob");

            string url = f.Client.OAuthCalculateAuthorizationUrl(requestToken.Token, AuthLevel.Write);

            System.Diagnostics.Process.Start(url);
        }

        public void UserAuthenticationAnswer()
        {

        }
    }
}
