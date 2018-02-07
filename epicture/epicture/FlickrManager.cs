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

        private string LocalTags;
        private uint LocalPerPage;
        private uint LocalSearchCurrentPage;
        private uint LocalFavoriteCurrentPage;
        public string LocalUserId { get; set; }

        public enum SearchType : uint
        {
            USERID = 0,
            USERNAME = 1,
            EMAIL = 2
        }

        public enum PublicSearchType : uint
        {
            USERNAME = 1,
            EMAIL = 2
        }

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
            LocalTags = "";
            LocalPerPage = 20;
            LocalSearchCurrentPage = 1;
            LocalFavoriteCurrentPage = 1;
            LocalUserId = "";
        }

        public void Connect(string publicKey, string secretKey)
        {
            Client = new Flickr(publicKey, secretKey);
        }

        public PhotoCollection SearchPhotos()
        {
            if (LocalTags != "")
            {
                var options = new PhotoSearchOptions { Tags = LocalTags, PerPage = Convert.ToInt32(LocalPerPage), Page = Convert.ToInt32(LocalSearchCurrentPage) };
                PhotoCollection photos = Client.PhotosSearch(options);

                if (IsUserAuthenticated())
                {
                    PhotoCollection favorites = SearchFavorites(accessToken.UserId, SearchType.USERID);

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
            LocalSearchCurrentPage = page;
            return (SearchPhotos());
        }

        public PhotoCollection SearchPhotos(uint perPage, uint page)
        {
            this.LocalPerPage = perPage;
            LocalSearchCurrentPage = page;
            return (SearchPhotos());
        }

        public PhotoCollection SearchPhotos(string tags, uint perPage, uint page)
        {
            LocalTags = tags;
            this.LocalPerPage = perPage;
            LocalSearchCurrentPage = page;
            return (SearchPhotos());
        }

        public UserInfos GetUserInfos(string user, PublicSearchType type)
        {
            FoundUser foundUser = null;

            switch (type)
            {
                case PublicSearchType.USERNAME:
                    foundUser = Client.PeopleFindByUserName(user);
                    break;
                case PublicSearchType.EMAIL:
                    foundUser = Client.PeopleFindByEmail(user);
                    break;
                default:
                    break;
            }
            return (new UserInfos(foundUser.FullName, foundUser.UserName, foundUser.UserId));
        }

        public PhotoCollection SearchFavorites(string user, uint page, SearchType type)
        {
            LocalFavoriteCurrentPage = page;
            if (type == SearchType.USERID)
                LocalUserId = user;
            else
            {
                UserInfos userInfos = GetUserInfos(user, (PublicSearchType)type);
                LocalUserId = userInfos.UserId;
            }

            return (SearchFavorites());
        }

        public PhotoCollection SearchFavorites(string user, SearchType type)
        {
            if (type == SearchType.USERID)
                LocalUserId = user;
            else
            {
                UserInfos userInfos = GetUserInfos(user, (PublicSearchType)type);
                LocalUserId = userInfos.UserId;
            }

            return (SearchFavorites());
        }

        public PhotoCollection SearchFavorites(uint page)
        {
            LocalFavoriteCurrentPage = page;
            return (SearchFavorites());
        }

        public PhotoCollection SearchFavorites()
        {
            PhotoCollection photos = Client.FavoritesGetPublicList(LocalUserId, DateTime.MinValue, DateTime.MaxValue, PhotoSearchExtras.All, Convert.ToInt32(LocalFavoriteCurrentPage), 20);
            return (photos);
        }

        public void AddFavoritePicture(string photoId)
        {
            if (IsUserAuthenticated())
                Client.FavoritesAdd(photoId);
            else
                throw new UserAuthenticationException("The user is not authenticated.");
        }

        public void RemoveFavoritePicture(string photoId)
        {
            if (IsUserAuthenticated())
                Client.FavoritesRemove(photoId);
            else
                throw new UserAuthenticationException("The user is not authenticated.");
        }

        public void UserAuthenticationRequest()
        {
            accessToken = null;
            Client.OAuthAccessToken = null;
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
        public bool IsUserAuthenticated()
        {
            return (Client.OAuthAccessToken != null);
        }

        public UserInfos UserInfos()
        {
            if (accessToken != null)
                return (new UserInfos(accessToken.FullName, accessToken.Username, accessToken.UserId));
            throw new UserAuthenticationException("The user is not authenticated.");
        }
    }
}
