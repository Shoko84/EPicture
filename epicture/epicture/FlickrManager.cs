using FlickrNet;
using System;
using System.Collections.Generic;
using System.IO;
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
        public string LocalFavoriteUserId { get; set; }

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
            LocalPerPage = 50;
            LocalSearchCurrentPage = 1;
            LocalFavoriteCurrentPage = 1;
            LocalFavoriteUserId = "";
        }

        // SEARCH PHOTOS
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
                return (new PhotoCollection());
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

        public void SearchPhotosAsync(Action<PhotoCollection> callback)
        {
            if (LocalTags != "")
            {
                var options = new PhotoSearchOptions { Tags = LocalTags, PerPage = Convert.ToInt32(LocalPerPage), Page = Convert.ToInt32(LocalSearchCurrentPage) };

                if (IsUserAuthenticated())
                {
                    SearchFavoritesAsync(delegate (FlickrResult<PhotoCollection> photosFavorites)
                    {
                        PhotoCollection favorites = photosFavorites.Result;
                        Client.PhotosSearchAsync(options, delegate (FlickrResult<PhotoCollection> photosSearch)
                        {
                            PhotoCollection photos = photosSearch.Result;
                            for (var i = 0; i < photos.Count; ++i)
                            {
                                for (var j = 0; j < favorites.Count; ++j)
                                {
                                    if (photos[i].PhotoId == favorites[j].PhotoId)
                                        photos[i] = favorites[j];
                                }
                            }
                            callback(photos);
                        });
                    });
                }
                else
                {
                    Client.PhotosSearchAsync(options, delegate (FlickrResult<PhotoCollection> photosSearch)
                    {
                        PhotoCollection photos = photosSearch.Result;
                        callback(photos);
                    });
                }
            }
        }

        public void SearchPhotosAsync(uint page, Action<PhotoCollection> callback)
        {
            LocalSearchCurrentPage = page;
            SearchPhotosAsync(callback);
        }

        public void SearchPhotosAsync(uint perPage, uint page, Action<PhotoCollection> callback)
        {
            this.LocalPerPage = perPage;
            LocalSearchCurrentPage = page;
            SearchPhotosAsync(callback);
        }

        public void SearchPhotosAsync(string tags, uint perPage, uint page, Action<PhotoCollection> callback)
        {
            LocalTags = tags;
            this.LocalPerPage = perPage;
            LocalSearchCurrentPage = page;
            SearchPhotosAsync(callback);
        }

        // FAVORITES
        public PhotoCollection SearchFavorites(string user, uint page, SearchType type)
        {
            LocalFavoriteCurrentPage = page;
            if (type == SearchType.USERID)
                LocalFavoriteUserId = user;
            else
            {
                UserInfos userInfos = GetUserInfos(user, (PublicSearchType)type);
                LocalFavoriteUserId = userInfos.UserId;
            }

            return (SearchFavorites());
        }

        public PhotoCollection SearchFavorites(string user, SearchType type)
        {
            if (type == SearchType.USERID)
                LocalFavoriteUserId = user;
            else
            {
                UserInfos userInfos = GetUserInfos(user, (PublicSearchType)type);
                LocalFavoriteUserId = userInfos.UserId;
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
            PhotoCollection photos = Client.FavoritesGetPublicList(LocalFavoriteUserId, DateTime.MinValue, DateTime.MaxValue, PhotoSearchExtras.All, Convert.ToInt32(LocalFavoriteCurrentPage), 50);
            return (photos);
        }

        public void SearchFavoritesAsync(string user, uint page, SearchType type, Action<FlickrResult<PhotoCollection>> callback)
        {
            LocalFavoriteCurrentPage = page;
            if (type == SearchType.USERID)
                LocalFavoriteUserId = user;
            else
            {
                UserInfos userInfos = GetUserInfos(user, (PublicSearchType)type);
                LocalFavoriteUserId = userInfos.UserId;
            }

            SearchFavoritesAsync(callback);
        }

        public void SearchFavoritesAsync(string user, SearchType type, Action<FlickrResult<PhotoCollection>> callback)
        {
            if (type == SearchType.USERID)
                LocalFavoriteUserId = user;
            else
            {
                UserInfos userInfos = GetUserInfos(user, (PublicSearchType)type);
                LocalFavoriteUserId = userInfos.UserId;
            }

            SearchFavoritesAsync(callback);
        }

        public void SearchFavoritesAsync(uint page, Action<FlickrResult<PhotoCollection>> callback)
        {
            LocalFavoriteCurrentPage = page;
            SearchFavoritesAsync(callback);
        }

        public void SearchFavoritesAsync(Action<FlickrResult<PhotoCollection>> callback)
        {
            Client.FavoritesGetPublicListAsync(LocalFavoriteUserId, DateTime.MinValue, DateTime.MaxValue, PhotoSearchExtras.All, Convert.ToInt32(LocalFavoriteCurrentPage), 50, callback);
        }


        // UPLOADED PICTURES
        public PhotoCollection SearchUploadedPictures()
        {
            if (IsUserAuthenticated())
            {
                var options = new PartialSearchOptions { Extras = PhotoSearchExtras.Description | PhotoSearchExtras.Usage };
                PhotoCollection photos = Client.PhotosGetNotInSet(options);
                return (photos);
            }
            else
                throw new UserAuthenticationException("The user is not authenticated.");
        }

        public void SearchUploadedPicturesAsync(Action<FlickrResult<PhotoCollection>> callback)
        {
            if (IsUserAuthenticated())
            {
                var options = new PartialSearchOptions { Extras = PhotoSearchExtras.Description | PhotoSearchExtras.Usage };
                Client.PhotosGetNotInSetAsync(options, callback);
            }
            else
                throw new UserAuthenticationException("The user is not authenticated.");
        }


        // FAVORITES HANDLERS
        public void AddFavoritePicture(string photoId)
        {
            if (IsUserAuthenticated())
                Client.FavoritesAdd(photoId);
            else
                throw new UserAuthenticationException("The user is not authenticated.");
        }

        public void AddFavoritePictureAsync(string photoId, Action<FlickrResult<NoResponse>> callback)
        {
            if (IsUserAuthenticated())
                Client.FavoritesAddAsync(photoId, callback);
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

        public void RemoveFavoritePictureAsync(string photoId, Action<FlickrResult<NoResponse>> callback)
        {
            if (IsUserAuthenticated())
                Client.FavoritesRemoveAsync(photoId, callback);
            else
                throw new UserAuthenticationException("The user is not authenticated.");
        }


        //UPLOAD PICTURES
        public void UploadPicture(string fileName, string title, string description, string tags, bool isPublic, bool isFamily, bool isFriend)
        {
            Client.UploadPicture(fileName, title, description, tags, isPublic, isFamily, isFriend);
        }

        public void UploadPictureAsync(string fileName, string title, string description, string tags, bool isPublic, bool isFamily, bool isFriend, Action<FlickrResult<string>> callback)
        {
            Client.UploadPictureAsync(File.Open(fileName, FileMode.Open), fileName, title, description, tags, isPublic, isFamily, isFriend, ContentType.Photo, SafetyLevel.Safe, HiddenFromSearch.Visible, callback);
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
            return (null);
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
        public void Connect(string publicKey, string secretKey)
        {
            Client = new Flickr(publicKey, secretKey);
        }

        public void Disconnect()
        {
            Client.OAuthAccessToken = null;
            accessToken = null;
            requestToken = null;
        }
    }
}
