using FlickrNet;
using System;
using System.IO;

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
        /// <summary>
        /// The UserId for the last favorite search
        /// </summary>
        public string LocalFavoriteUserId { get; set; }

        /// <summary>
        /// Used for searching by UserID, Username or by Email
        /// </summary>
        public enum SearchType : uint
        {
            USERID = 0,
            USERNAME = 1,
            EMAIL = 2
        }

        /// <summary>
        /// Used for searching by Username or by Email
        /// </summary>
        public enum PublicSearchType : uint
        {
            USERNAME = 1,
            EMAIL = 2
        }

        private static FlickrManager instance;
        /// <summary>
        /// Getter to the <see cref="FlickrManager"/>'s instance
        /// </summary>
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

        /// <summary>
        /// Synchronous photo searching
        /// </summary>
        /// <returns>A list (<see cref="PhotoCollection"/>) of photos informations</returns>
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

        /// <summary>
        /// Synchronous photo searching
        /// </summary>
        /// <param name="page">The page searching index</param>
        /// <returns>A list (<see cref="PhotoCollection"/>) of photos informations</returns>
        public PhotoCollection SearchPhotos(uint page)
        {
            LocalSearchCurrentPage = page;
            return (SearchPhotos());
        }

        /// <summary>
        /// Synchronous photo searching
        /// </summary>
        /// <param name="perPage">The number of photos per page</param>
        /// <param name="page">The page searching index</param>
        /// <returns>A list (<see cref="PhotoCollection"/>) of photos informations</returns>
        public PhotoCollection SearchPhotos(uint perPage, uint page)
        {
            this.LocalPerPage = perPage;
            LocalSearchCurrentPage = page;
            return (SearchPhotos());
        }

        /// <summary>
        /// Synchronous photo searching
        /// </summary>
        /// <param name="tags">The tags for the search</param>
        /// <param name="perPage">The number of photos per page</param>
        /// <param name="page">The page searching index</param>
        /// <returns>A list (<see cref="PhotoCollection"/>) of photos informations</returns>
        public PhotoCollection SearchPhotos(string tags, uint perPage, uint page)
        {
            LocalTags = tags;
            this.LocalPerPage = perPage;
            LocalSearchCurrentPage = page;
            return (SearchPhotos());
        }

        /// <summary>
        /// Asynchronous photo searching
        /// </summary>
        /// <param name="callback">The callback called at the end of the search</param>
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

        /// <summary>
        /// Asynchronous photo searching
        /// </summary>
        /// <param name="page">The page searching index</param>
        /// <param name="callback">The callback called at the end of the search</param>
        public void SearchPhotosAsync(uint page, Action<PhotoCollection> callback)
        {
            LocalSearchCurrentPage = page;
            SearchPhotosAsync(callback);
        }

        /// <summary>
        /// Asynchronous photo searching
        /// </summary>
        /// <param name="perPage">The number of photos per page</param>
        /// <param name="page">The page searching index</param>
        /// <param name="callback">The callback called at the end of the search</param>
        public void SearchPhotosAsync(uint perPage, uint page, Action<PhotoCollection> callback)
        {
            this.LocalPerPage = perPage;
            LocalSearchCurrentPage = page;
            SearchPhotosAsync(callback);
        }

        /// <summary>
        /// Asynchronous photo searching
        /// </summary>
        /// <param name="tags">The tags for the search</param>
        /// <param name="perPage">The number of photos per page</param>
        /// <param name="page">The page searching index</param>
        /// <param name="callback">The callback called at the end of the search</param>
        public void SearchPhotosAsync(string tags, uint perPage, uint page, Action<PhotoCollection> callback)
        {
            LocalTags = tags;
            this.LocalPerPage = perPage;
            LocalSearchCurrentPage = page;
            SearchPhotosAsync(callback);
        }

        /// <summary>
        /// Synchronous favorites photo searching
        /// </summary>
        /// <param name="user">An user</param>
        /// <param name="page">The page searching index</param>
        /// <param name="type">The string type related to the user parameter</param>
        /// <returns>A list (<see cref="PhotoCollection"/>) of photos informations</returns>
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

        /// <summary>
        /// Synchronous favorites photo searching
        /// </summary>
        /// <param name="user">An user</param>
        /// <param name="type">The string type related to the user parameter</param>
        /// <returns>A list (<see cref="PhotoCollection"/>) of photos informations</returns>
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

        /// <summary>
        /// Synchronous favorites photo searching
        /// </summary>
        /// <param name="page">The page searching index</param>
        /// <returns>A list (<see cref="PhotoCollection"/>) of photos informations</returns>
        public PhotoCollection SearchFavorites(uint page)
        {
            LocalFavoriteCurrentPage = page;
            return (SearchFavorites());
        }

        /// <summary>
        /// Synchronous favorites photo searching
        /// </summary>
        /// <returns>A list (<see cref="PhotoCollection"/>) of photos informations</returns>
        public PhotoCollection SearchFavorites()
        {
            PhotoCollection photos = Client.FavoritesGetPublicList(LocalFavoriteUserId, DateTime.MinValue, DateTime.MaxValue, PhotoSearchExtras.All, Convert.ToInt32(LocalFavoriteCurrentPage), 50);
            return (photos);
        }

        /// <summary>
        /// Asynchronous favorites photo searching
        /// </summary>
        /// <param name="user">An user</param>
        /// <param name="page">The page searching index</param>
        /// <param name="type">The string type related to the user parameter</param>
        /// <param name="callback">The callback called at the end of the search</param>
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

        /// <summary>
        /// Asynchronous favorites photo searching
        /// </summary>
        /// <param name="user">An user</param>
        /// <param name="type">The string type related to the user parameter</param>
        /// <param name="callback">The callback called at the end of the search</param>
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

        /// <summary>
        /// Asynchronous favorites photo searching
        /// </summary>
        /// <param name="page">The page searching index</param>
        /// <param name="callback">The callback called at the end of the search</param>
        public void SearchFavoritesAsync(uint page, Action<FlickrResult<PhotoCollection>> callback)
        {
            LocalFavoriteCurrentPage = page;
            SearchFavoritesAsync(callback);
        }

        /// <summary>
        /// Asynchronous favorites photo searching
        /// </summary>
        /// <param name="callback">The callback called at the end of the search</param>
        public void SearchFavoritesAsync(Action<FlickrResult<PhotoCollection>> callback)
        {
            Client.FavoritesGetPublicListAsync(LocalFavoriteUserId, DateTime.MinValue, DateTime.MaxValue, PhotoSearchExtras.All, Convert.ToInt32(LocalFavoriteCurrentPage), 50, callback);
        }

        /// <summary>
        /// Synchronous uploaded picture searching
        /// </summary>
        /// <returns>A list (<see cref="PhotoCollection"/>) of photos informations</returns>
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

        /// <summary>
        /// Asynchronous uploaded picture searching
        /// </summary>
        /// <param name="callback">The callback called at the end of the search</param>
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

        /// <summary>
        /// Synchronous favorite picture addition
        /// </summary>
        /// <param name="photoId">The photo's photoId to add from favorites</param>
        public void AddFavoritePicture(string photoId)
        {
            if (IsUserAuthenticated())
                Client.FavoritesAdd(photoId);
            else
                throw new UserAuthenticationException("The user is not authenticated.");
        }

        /// <summary>
        /// Asynchronous favorite picture addition
        /// </summary>
        /// <param name="photoId">The photo's photoId to add from favorites</param>
        /// <param name="callback">The callback called at the end of the addition</param>
        public void AddFavoritePictureAsync(string photoId, Action<FlickrResult<NoResponse>> callback)
        {
            if (IsUserAuthenticated())
                Client.FavoritesAddAsync(photoId, callback);
            else
                throw new UserAuthenticationException("The user is not authenticated.");
        }

        /// <summary>
        /// Synchronous favorite picture removal
        /// </summary>
        /// <param name="photoId">The photo's photoId to remove from favorites</param>
        public void RemoveFavoritePicture(string photoId)
        {
            if (IsUserAuthenticated())
                Client.FavoritesRemove(photoId);
            else
                throw new UserAuthenticationException("The user is not authenticated.");
        }

        /// <summary>
        /// Asynchronous favorite picture removal
        /// </summary>
        /// <param name="photoId">The photo's photoId to remove from favorites</param>
        /// <param name="callback">The callback called at the end of the removal</param>
        public void RemoveFavoritePictureAsync(string photoId, Action<FlickrResult<NoResponse>> callback)
        {
            if (IsUserAuthenticated())
                Client.FavoritesRemoveAsync(photoId, callback);
            else
                throw new UserAuthenticationException("The user is not authenticated.");
        }

        /// <summary>
        /// Synchronous picture uploading
        /// </summary>
        /// <param name="fileName">The path to the picture</param>
        /// <param name="title">The title for the picture</param>
        /// <param name="description">The description for the picture</param>
        /// <param name="tags">Tags associed to the picture</param>
        /// <param name="isPublic">Is the picture visible to everyone?</param>
        /// <param name="isFamily">Is the picture visible to family contacts ?</param>
        /// <param name="isFriend">Is the picture visible for friends ?</param>
        public void UploadPicture(string fileName, string title, string description, string tags, bool isPublic, bool isFamily, bool isFriend)
        {
            Client.UploadPicture(fileName, title, description, tags, isPublic, isFamily, isFriend);
        }

        /// <summary>
        /// Asynchronous picture uploading
        /// </summary>
        /// <param name="fileName">The path to the picture</param>
        /// <param name="title">The title for the picture</param>
        /// <param name="description">The description for the picture</param>
        /// <param name="tags">Tags associed to the picture</param>
        /// <param name="isPublic">Is the picture visible to everyone?</param>
        /// <param name="isFamily">Is the picture visible to family contacts ?</param>
        /// <param name="isFriend">Is the picture visible for friends ?</param>
        /// <param name="callback">The callback called at the end of the upload</param>
        public void UploadPictureAsync(string fileName, string title, string description, string tags, bool isPublic, bool isFamily, bool isFriend, Action<FlickrResult<string>> callback)
        {
            Client.UploadPictureAsync(File.Open(fileName, FileMode.Open), fileName, title, description, tags, isPublic, isFamily, isFriend, ContentType.Photo, SafetyLevel.Safe, HiddenFromSearch.Visible, callback);
        }

        /// <summary>
        /// Request an OAuth authentification for a token to the user
        /// </summary>
        public void UserAuthenticationRequest()
        {
            accessToken = null;
            Client.OAuthAccessToken = null;
            requestToken = Client.OAuthGetRequestToken("oob");

            string url = Client.OAuthCalculateAuthorizationUrl(requestToken.Token, AuthLevel.Read | AuthLevel.Write);

            System.Diagnostics.Process.Start(url);
        }

        /// <summary>
        /// Submit the token to the API
        /// </summary>
        /// <param name="token">The user's token</param>
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

        /// <summary>
        /// Returning if the user is connected to the API
        /// </summary>
        /// <returns>True if the user is connected</returns>
        public bool IsUserAuthenticated()
        {
            return (Client.OAuthAccessToken != null);
        }

        /// <summary>
        /// Getting the current logged-in user informations 
        /// </summary>
        /// <returns>Class containing the fullname, username and the userid of the user</returns>
        public UserInfos UserInfos()
        {
            if (accessToken != null)
                return (new UserInfos(accessToken.FullName, accessToken.Username, accessToken.UserId));
            return (null);
        }

        /// <summary>
        /// Getting informations from an user
        /// </summary>
        /// <param name="user">An user</param>
        /// <param name="type">The string type related to the user parameter</param>
        /// <returns>Class containing the fullname, username and the userid of the user</returns>
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

        /// <summary>
        /// Initializing the Flickr client
        /// </summary>
        /// <param name="publicKey">The public key</param>
        /// <param name="secretKey">The private key</param>
        public void Connect(string publicKey, string secretKey)
        {
            Client = new Flickr(publicKey, secretKey);
        }

        /// <summary>
        /// Disconnecting the user
        /// </summary>
        public void Disconnect()
        {
            Client.OAuthAccessToken = null;
            accessToken = null;
            requestToken = null;
        }
    }
}
