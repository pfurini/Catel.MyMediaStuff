using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Catel.Data;
using Catel.MVVM;
using MyMediaStuff.DataProviders;

namespace MyMediaStuff.UI.ViewModels
{
    /// <summary>
    /// Home view model.
    /// </summary>
    public class HomeViewModel : ViewModelBase
    {
        #region Variables
        private readonly DispatcherTimer _slideshowTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 2, 500) };
        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
        /// </summary>
        public HomeViewModel(IHomeProvider homeProvider)
        {
            HomeProvider = homeProvider;

            _slideshowTimer.Tick += OnSlideshowTimerTick;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title { get { return HomeProvider.Name; } }

        #region Models
        /// <summary>
        /// Gets the home provider.
        /// </summary>
        [Model]
        public IHomeProvider HomeProvider
        {
            get { return GetValue<IHomeProvider>(HomeProviderProperty); }
            private set { SetValue(HomeProviderProperty, value); }
        }

        /// <summary>
        /// Register the HomeProvider property so it is known in the class.
        /// </summary>
        public static readonly PropertyData HomeProviderProperty = RegisterProperty("HomeProvider", typeof(IHomeProvider));
        #endregion

        #region View model
        /// <summary>
        /// Gets or sets all the media items.
        /// </summary>
        [ViewModelToModel("HomeProvider", "Items")]
        public ObservableCollection<IMediaInfo> MediaItems
        {
            get { return GetValue<ObservableCollection<IMediaInfo>>(MediaItemsProperty); }
            set { SetValue(MediaItemsProperty, value); }
        }

        /// <summary>
        /// Register the MediaItems property so it is known in the class.
        /// </summary>
        public static readonly PropertyData MediaItemsProperty = RegisterProperty("MediaItems", typeof(ObservableCollection<IMediaInfo>));

        /// <summary>
        /// Gets or sets the latest pictures.
        /// </summary>
        [ViewModelToModel("HomeProvider", "LatestPictures")]
        public ObservableCollection<IPictureInfo> LatestPictures
        {
            get { return GetValue<ObservableCollection<IPictureInfo>>(LatestPicturesProperty); }
            set { SetValue(LatestPicturesProperty, value); }
        }

        /// <summary>
        /// Register the LatestPictures property so it is known in the class.
        /// </summary>
        public static readonly PropertyData LatestPicturesProperty = RegisterProperty("LatestPictures", typeof(ObservableCollection<IPictureInfo>));

        /// <summary>
        /// Gets or sets the latest videos.
        /// </summary>
        [ViewModelToModel("HomeProvider", "LatestVideos")]
        public ObservableCollection<IVideoInfo> LatestVideos
        {
            get { return GetValue<ObservableCollection<IVideoInfo>>(LatestVideosProperty); }
            set { SetValue(LatestVideosProperty, value); }
        }

        /// <summary>
        /// Register the LatestVideos property so it is known in the class.
        /// </summary>
        public static readonly PropertyData LatestVideosProperty = RegisterProperty("LatestVideos", typeof(ObservableCollection<IVideoInfo>));

        /// <summary>
        /// Gets a random picture.
        /// </summary>
        public IPictureInfo RandomPicture
        {
            get { return GetValue<IPictureInfo>(RandomPictureProperty); }
            set { SetValue(RandomPictureProperty, value); }
        }

        /// <summary>
        /// Register the RandomPicture property so it is known in the class.
        /// </summary>
        public static readonly PropertyData RandomPictureProperty = RegisterProperty("RandomPicture", typeof(IPictureInfo));

        /// <summary>
        /// Gets or sets the selected picture.
        /// </summary>
        public IPictureInfo SelectedPicture
        {
            get { return GetValue<IPictureInfo>(SelectedPictureProperty); }
            set { SetValue(SelectedPictureProperty, value); }
        }

        /// <summary>
        /// Register the SelectedPicture property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SelectedPictureProperty = RegisterProperty("SelectedPicture", typeof(IPictureInfo),
            (sender, e) => ((HomeViewModel)sender).OnSelectedPictureChanged());

        /// <summary>
        /// Gets or sets the selected video.
        /// </summary>
        public IVideoInfo SelectedVideo
        {
            get { return GetValue<IVideoInfo>(SelectedVideoProperty); }
            set { SetValue(SelectedVideoProperty, value); }
        }

        /// <summary>
        /// Register the SelectedVideo property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SelectedVideoProperty = RegisterProperty("SelectedVideo", typeof(IVideoInfo),
            (sender, e) => ((HomeViewModel)sender).OnSelectedVideoChanged());
        #endregion
        #endregion

        #region Commands
        #endregion

        #region Methods
        private void OnSlideshowTimerTick(object sender, EventArgs e)
        {
            RandomPicture = GetRandomPicture();
        }

        private void OnSelectedPictureChanged()
        {
            if (SelectedPicture != null)
            {
                SelectedVideo = null;
            }
        }

        private void OnSelectedVideoChanged()
        {
            if (SelectedVideo != null)
            {
                SelectedPicture = null;
            }
        }

        /// <summary>
        /// Initializes the object by setting default values.
        /// </summary>
        protected override void Initialize()
        {
            RandomPicture = GetRandomPicture();

            _slideshowTimer.Start();
        }

        /// <summary>
        /// Closes this instance. Always called after the <see cref="M:Catel.MVVM.ViewModelBaseWithoutServices.Cancel"/> of <see cref="M:Catel.MVVM.ViewModelBaseWithoutServices.Save"/> method.
        /// </summary>
        /// <remarks>
        /// When implementing this method in a base class, make sure to call the base, otherwise <see cref="P:Catel.MVVM.ViewModelBaseWithoutServices.IsClosed"/> will
        /// not be set to true.
        /// </remarks>
        protected override void Close()
        {
            if (_slideshowTimer.IsEnabled)
            {
                _slideshowTimer.Stop();
            }

            base.Close();
        }

        /// <summary>
        /// Gets a random picture from the picture collection.
        /// </summary>
        /// <returns>Random <see cref="IPictureInfo"/> or <c>null</c> if there are no pictures available.</returns>
        private IPictureInfo GetRandomPicture()
        {
            if (HomeProvider.Pictures.Count > 0)
            {
                Random random = new Random();
                return HomeProvider.Pictures[random.Next(0, HomeProvider.Pictures.Count - 1)];
            }

            return null;
        }
        #endregion
    }
}
