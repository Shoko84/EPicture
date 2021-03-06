﻿using FlickrNet;
using System.Windows;
using System.Windows.Controls;

namespace epicture
{
    /// <summary>
    /// Interaction logic for ExploreControl.xaml
    /// </summary>
    public partial class ExploreControl : UserControl
    {
        /// <summary>
        /// The picture viewer for the <see cref="ExploreControl"/>
        /// </summary>
        public PictureViewer PictureViewer { get; private set; }

        /// <summary>
        /// Event raised if the user is asking an action where he should be authentified from an <see cref="ExploreControl"/>
        /// </summary>
        public static readonly RoutedEvent UserAuthenticatedRequestFromExploreControlEvent =
            EventManager.RegisterRoutedEvent("UserAuthenticatedRequestFromExploreControlEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(ExploreControl));

        /// <summary>
        /// Constructor of the class <see cref="ExploreControl"/>
        /// </summary>
        public ExploreControl()
        {
            InitializeComponent();
            PictureViewer = new PictureViewer();
            PictureViewer.NextPageButton.IsEnabled = false;
            PictureViewerControl.Content = PictureViewer;

            AddHandler(PictureViewer.UserAuthenticatedRequestFromPictureViewerEvent,
                       new RoutedEventHandler(UserAuthenticatedRequestFromPictureViewerHandler));

            AddHandler(PictureViewer.ChangeFavoriteFromPictureViewerEvent,
                       new RoutedEventHandler(ChangeFavoriteFromPictureViewerHandler));

            AddHandler(PictureViewer.PreviousPageClickedEvent,
                       new RoutedEventHandler(PageChangedClickedHandler));

            AddHandler(PictureViewer.NextPageClickedEvent,
                       new RoutedEventHandler(PageChangedClickedHandler));
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text != "")
            {
                FlickrManager.Instance.SearchPhotosAsync(SearchTextBox.Text, 50, 1, delegate (PhotoCollection photos)
                {
                    PictureViewer.SetCurrentPage(1);
                    PictureViewer.NextPageButton.IsEnabled = true;
                    PictureViewer.SetPictures(photos, 1);
                    PictureViewer.ScrollerViewer.ScrollToTop();
                });
            }
        }

        private void UserAuthenticatedRequestFromPictureViewerHandler(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ExploreControl.UserAuthenticatedRequestFromExploreControlEvent));
        }

        private void ChangeFavoriteFromPictureViewerHandler(object sender, RoutedEventArgs e)
        {
            FlickrManager.Instance.SearchPhotosAsync(delegate (PhotoCollection photos)
            {
                PictureViewer.SetPictures(photos);
            });
        }

        private void PageChangedClickedHandler(object sender, RoutedEventArgs e)
        {
            FlickrManager.Instance.SearchPhotosAsync(50, PictureViewer.CurrentPage, delegate (PhotoCollection photos)
            {
                PictureViewer.SetPictures(photos);
                PictureViewer.ScrollerViewer.ScrollToTop();
            });
        }
    }
}
