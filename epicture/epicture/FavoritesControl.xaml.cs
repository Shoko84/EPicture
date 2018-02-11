﻿using FlickrNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace epicture
{
    /// <summary>
    /// Interaction logic for FavoritesControl.xaml
    /// </summary>
    public partial class FavoritesControl : UserControl
    {
        public PictureViewer PictureViewer;

        public static readonly RoutedEvent UserAuthenticatedRequestFromFavoritesControlEvent =
            EventManager.RegisterRoutedEvent("UserAuthenticatedRequestFromFavoritesControlEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(FavoritesControl));

        public FavoritesControl()
        {
            InitializeComponent();
            PictureViewer = new PictureViewer();
            PictureViewer.NextPageButton.IsEnabled = false;
            PictureViewerControl.Content = PictureViewer;
            PictureViewer.HideFavoriteButton = true;

            AddHandler(PictureViewer.UserAuthenticatedRequestFromPictureViewerEvent,
                       new RoutedEventHandler(UserAuthenticatedRequestFromPictureViewerHandler));

            AddHandler(PictureViewer.ChangeFavoriteFromPictureViewerEvent,
                       new RoutedEventHandler(ChangeFavoriteFromPictureViewerHandler));

            AddHandler(PictureViewer.PreviousPageClickedEvent,
                       new RoutedEventHandler(PageChangedClickedHandler));

            AddHandler(PictureViewer.NextPageClickedEvent,
                       new RoutedEventHandler(PageChangedClickedHandler));
        }

        private void UserAuthenticatedRequestFromPictureViewerHandler(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(FavoritesControl.UserAuthenticatedRequestFromFavoritesControlEvent));
        }

        private void ChangeFavoriteFromPictureViewerHandler(object sender, RoutedEventArgs e)
        {
            if (FlickrManager.Instance.UserInfos() != null && FlickrManager.Instance.LocalFavoriteUserId == FlickrManager.Instance.UserInfos().UserId)
                FlickrManager.Instance.SearchFavoritesAsync(delegate (FlickrResult<PhotoCollection> photos)
                {
                    PictureViewer.SetPictures(photos.Result);
                });
        }

        private void PageChangedClickedHandler(object sender, RoutedEventArgs e)
        {
            FlickrManager.Instance.SearchFavoritesAsync(PictureViewer.CurrentPage, delegate(FlickrResult<PhotoCollection> photos)
            {
                PictureViewer.SetPictures(photos.Result);
                PictureViewer.ScrollerViewer.ScrollToTop();
            });
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text != "")
            {
                try
                {
                    PictureViewer.HideFavoriteButton = (FlickrManager.Instance.UserInfos() != null && SearchTextBox.Text == FlickrManager.Instance.UserInfos().Username) ? (false) : (true);
                    PictureViewer.SetCurrentPage(1);
                    PictureViewer.NextPageButton.IsEnabled = true;
                    FlickrManager.Instance.SearchFavoritesAsync(SearchTextBox.Text, 1, FlickrManager.SearchType.USERNAME, delegate (FlickrResult<PhotoCollection> photos)
                    {
                        PictureViewer.SetPictures(photos.Result, 1);
                        PictureViewer.ScrollerViewer.ScrollToTop();
                    });
                }
                catch (FlickrNet.Exceptions.UserNotFoundException)
                {
                    PictureViewer.NextPageButton.IsEnabled = false;
                    MessageBox.Show("User hasn't been found.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }
    }
}
