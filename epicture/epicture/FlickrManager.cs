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
        private OAuthAccessToken accessToken;

        private string localTags;
        private uint perPage;
        private uint localCurrentPage;

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
            localTags = "";
            perPage = 20;
            localCurrentPage = 1;
        }

        public void Connect(string publicKey, string secretKey)
        {
            Client = new Flickr(publicKey, secretKey);
        }

        public PhotoCollection SearchPhotos()
        {
            if (localTags != "")
            {
                var options = new PhotoSearchOptions { Tags = localTags, PerPage = Convert.ToInt32(perPage), Page = Convert.ToInt32(localCurrentPage) };
                PhotoCollection photos = Client.PhotosSearch(options);

                if (Client.OAuthAccessToken != null)
                {
                    PhotoCollection favorites = SearchFavorites();

                    for (var i = 0; i < photos.Count; ++i)
                    {
                        for (var j = 0; j < favorites.Count; ++j)
                        {
                            if (photos[i].PhotoId == favorites[j].PhotoId)
                                photos[i] = favorites[j];
                        }
                    }
                }

                return (photos);
            }
            else
                return (new PhotoCollection()); //TODO exception
        }

        public PhotoCollection SearchPhotos(uint page)
        {
            localCurrentPage = page;
            return (SearchPhotos());
        }

        public PhotoCollection SearchPhotos(uint perPage, uint page)
        {
            this.perPage = perPage;
            localCurrentPage = page;
            return (SearchPhotos());
        }

        public PhotoCollection SearchPhotos(string tags, uint perPage, uint page)
        {
            localTags = tags;
            this.perPage = perPage;
            localCurrentPage = page;
            return (SearchPhotos());
        }

        public PhotoCollection SearchFavorites()
        {
            if (Client.OAuthAccessToken != null)
            {
                PhotoCollection photos = Client.FavoritesGetList();

                return (photos);
            }
            else
                throw new UserAuthenticationException("The user is not authenticated.");
        }

        public void AddFavoritePicture(string photoId)
        {
            if (Client.OAuthAccessToken != null)
                Client.FavoritesAdd(photoId);
            else
                throw new UserAuthenticationException("The user is not authenticated.");
        }

        public void RemoveFavoritePicture(string photoId)
        {
            if (Client.OAuthAccessToken != null)
                Client.FavoritesRemove(photoId);
            else
                throw new UserAuthenticationException("The user is not authenticated.");
        }

        public void UserAuthenticationRequest()
        {
            requestToken = Client.OAuthGetRequestToken("oob");

            string url = Client.OAuthCalculateAuthorizationUrl(requestToken.Token, AuthLevel.Read | AuthLevel.Write);

            System.Diagnostics.Process.Start(url);
        }

        public void UserAuthenticationAnswer(string token)
        {
            try
            {
                accessToken = Client.OAuthGetAccessToken(requestToken, token);
            }
            catch (Exception ex)
            {
                throw new UserAuthenticationException("Failed to get access token: " + ex.Message);
            }
        }
    }
}
