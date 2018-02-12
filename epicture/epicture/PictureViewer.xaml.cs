using FlickrNet;
using System.Windows;
using System.Windows.Controls;

namespace epicture
{
    /// <summary>
    /// Interaction logic for PictureViewer.xaml
    /// </summary>
    public partial class PictureViewer : UserControl
    {
        private PhotoCollection Pictures;
        /// <summary>
        /// The current page displayed in the <see cref="PictureViewer"/>
        /// </summary>
        public uint     CurrentPage { get; private set; }
        /// <summary>
        /// Hiding or not the "Favorite" button
        /// </summary>
        public bool     HideFavoriteButton { get; set; }

        /// <summary>
        /// Event raised if the user is asking an action where he should be authentified from an <see cref="PictureViewer"/>
        /// </summary>
        public static readonly RoutedEvent UserAuthenticatedRequestFromPictureViewerEvent =
            EventManager.RegisterRoutedEvent("UserAuthenticatedRequestFromPictureViewerEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(PictureViewer));

        /// <summary>
        /// Event raised when the user is asking to change the favorite state of a <see cref="Picture"/>
        /// </summary>
        public static readonly RoutedEvent ChangeFavoriteFromPictureViewerEvent =
            EventManager.RegisterRoutedEvent("ChangeFavoriteFromPictureViewerEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(PictureViewer));

        /// <summary>
        /// Event raised if the user clicked the "Prev page" button from an <see cref="PictureViewer"/>
        /// </summary>
        public static readonly RoutedEvent PreviousPageClickedEvent =
            EventManager.RegisterRoutedEvent("PreviousPageClickedEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(PictureViewer));

        /// <summary>
        /// Event raised if the user clicked the "Next page" button from an <see cref="PictureViewer"/>
        /// </summary>
        public static readonly RoutedEvent NextPageClickedEvent =
            EventManager.RegisterRoutedEvent("NextPageClickedEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventArgs), typeof(PictureViewer));

        /// <summary>
        /// Constructor of the class <see cref="PictureViewer"/>
        /// </summary>
        public PictureViewer()
        {
            InitializeComponent();
            CurrentPage = 1;
            HideFavoriteButton = false;
            Pictures = new PhotoCollection();

            AddHandler(Picture.UserAuthenticatedRequestFromPictureEvent,
                       new RoutedEventHandler(UserAuthenticatedRequestFromPictureHandler));

            AddHandler(Picture.ChangeFavoriteFromPictureEvent,
                       new RoutedEventHandler(ChangeFavoriteHandler));
        }

        /// <summary>
        /// Setting the current display page
        /// </summary>
        /// <param name="page">The page index</param>
        public void SetCurrentPage(uint page)
        {
            CurrentPage = (page == 0) ? (1) : (page);
            if (page == 1)
                PrevPageButton.IsEnabled = false;
        }

        private void UserAuthenticatedRequestFromPictureHandler(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PictureViewer.UserAuthenticatedRequestFromPictureViewerEvent));
        }

        private void ChangeFavoriteHandler(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PictureViewer.ChangeFavoriteFromPictureViewerEvent));
        }

        /// <summary>
        /// Setting pictures to be displayed on the <see cref="PictureViewer"/>
        /// </summary>
        /// <param name="pictures">A list of photos informations</param>
        public void SetPictures(PhotoCollection pictures)
        {
            Pictures = pictures;
            PrevPageButton.IsEnabled = (CurrentPage == 1) ? (false) : (true);
            RefreshPicturesOnScreen();
        }

        /// <summary>
        /// Setting pictures to be displayed on the <see cref="PictureViewer"/>
        /// </summary>
        /// <param name="pictures">A list of photos informations</param>
        /// <param name="currentPage">The page index to be displayed</param>
        public void SetPictures(PhotoCollection pictures, uint currentPage)
        {
            Pictures = pictures;
            SetCurrentPage(currentPage);
            RefreshPicturesOnScreen();
        }

        private void RefreshPicturesOnScreen()
        {
            ViewerContainer.Children.Clear();
            var p = Pictures.Count;
            for (var i = 0; i < Pictures.Count; ++i)
            {
                Picture picture = new Picture(Pictures[i]);
                picture.Margin = new Thickness(15);
                picture.PictureFavoriteButton.Visibility = (HideFavoriteButton) ? (Visibility.Hidden) : (Visibility.Visible);
                ViewerContainer.Children.Add(picture);
            }
        }

        private void PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage -= 1;
            if (CurrentPage == 1)
                PrevPageButton.IsEnabled = false;
            RaiseEvent(new RoutedEventArgs(PictureViewer.PreviousPageClickedEvent));
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage += 1;
            PrevPageButton.IsEnabled = true;
            RaiseEvent(new RoutedEventArgs(PictureViewer.NextPageClickedEvent));
        }
    }
}
