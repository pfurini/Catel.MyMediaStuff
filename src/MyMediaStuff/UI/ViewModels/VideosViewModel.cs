using System;
using System.Collections.ObjectModel;
using System.Linq;
using Catel.Collections.ObjectModel;
using Catel.Data;
using Catel.MVVM;
using Catel.MVVM.Services;
using MyMediaStuff.DataProviders;

namespace MyMediaStuff.UI.ViewModels
{
    /// <summary>
    /// Videos view model.
    /// </summary>
    public class VideosViewModel : ViewModelBase
    {
        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="VideosViewModel"/> class.
        /// </summary>
        public VideosViewModel(IVideoProvider videoProvider)
        {
            VideoProvider = videoProvider;

            PlaySelectedVideo = new Command<object, object>(OnPlaySelectedVideoExecute, OnPlaySelectedVideoCanExecute);
            StopPlayingVideo = new Command<object>(OnStopPlayingVideoExecute);
            Refresh = new Command<object, object>(OnRefreshExecute, OnRefreshCanExecute);
            ViewInSoftware = new Command<object, object>(OnViewInSoftwareExecute, OnViewInSoftwareCanExecute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title { get { return VideoProvider.Name; } }

        #region Models
        /// <summary>
        /// Gets the video provider.
        /// </summary>
        [Model]
        public IVideoProvider VideoProvider
        {
            get { return GetValue<IVideoProvider>(VideoProviderProperty); }
            private set { SetValue(VideoProviderProperty, value); }
        }

        /// <summary>
        /// Register the VideoProvider property so it is known in the class.
        /// </summary>
        public static readonly PropertyData VideoProviderProperty = RegisterProperty("VideoProvider", typeof(IVideoProvider));
        #endregion

        #region View model
        /// <summary>
        /// Gets the videos.
        /// </summary>
        [ViewModelToModel("VideoProvider", "Items")]
        public ObservableCollection<IVideoInfo> Videos
        {
            get { return GetValue<ObservableCollection<IVideoInfo>>(VideosProperty); }
            set { SetValue(VideosProperty, value); }
        }

        /// <summary>
        /// Register the Videos property so it is known in the class.
        /// </summary>
        public static readonly PropertyData VideosProperty = RegisterProperty("Videos", typeof(ObservableCollection<IVideoInfo>));

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
        public static readonly PropertyData SelectedVideoProperty = RegisterProperty("SelectedVideo", typeof(IVideoInfo));

        /// <summary>
        /// Gets whether the view model is currently playing a video.
        /// </summary>
        public bool IsPlayingVideo
        {
            get { return GetValue<bool>(IsPlayingVideoProperty); }
            private set { SetValue(IsPlayingVideoProperty, value); }
        }

        /// <summary>
        /// Register the IsPlayingVideo property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsPlayingVideoProperty = RegisterProperty("IsPlayingVideo", typeof(bool));
        #endregion
        #endregion

        #region Commands
        /// <summary>
        /// Gets the PlaySelectedVideo command.
        /// </summary>
        public Command<object, object> PlaySelectedVideo { get; private set; }

        /// <summary>
        /// Method to check whether the PlaySelectedVideo command can be executed.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        private bool OnPlaySelectedVideoCanExecute(object parameter)
        {
            return (SelectedVideo != null) && !IsPlayingVideo;
        }

        /// <summary>
        /// Method to invoke when the PlaySelectedVideo command is executed.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        private void OnPlaySelectedVideoExecute(object parameter)
        {
            IsPlayingVideo = true;
        }

        /// <summary>
        /// Gets the StopPlayingVideo command.
        /// </summary>
        public Command<object> StopPlayingVideo { get; private set; }

        /// <summary>
        /// Method to invoke when the StopPlayingVideo command is executed.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        private void OnStopPlayingVideoExecute(object parameter)
        {
            IsPlayingVideo = false;
        }

        /// <summary>
        /// Gets the Refresh command.
        /// </summary>
        public Command<object, object> Refresh { get; private set; }

        /// <summary>
        /// Method to check whether the Refresh command can be executed.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        private bool OnRefreshCanExecute(object parameter)
        {
            return !IsPlayingVideo;
        }

        /// <summary>
        /// Method to invoke when the Refresh command is executed.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        private void OnRefreshExecute(object parameter)
        {
            var pleaseWaitService = GetService<IPleaseWaitService>();
            pleaseWaitService.Show(() => VideoProvider.Refresh(), "Refreshing...");
        }

        /// <summary>
        /// Gets the ViewInSoftware command.
        /// </summary>
        public Command<object, object> ViewInSoftware { get; private set; }

        /// <summary>
        /// Method to check whether the ViewInSoftware command can be executed.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        private bool OnViewInSoftwareCanExecute(object parameter)
        {
            return SelectedVideo != null;
        }

        /// <summary>
        /// Method to invoke when the ViewInSoftware command is executed.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        private void OnViewInSoftwareExecute(object parameter)
        {
            var processService = GetService<IProcessService>();
            processService.StartProcess(SelectedVideo.FileName);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the object by setting default values.
        /// </summary>	
        protected override void Initialize()
        {
            if (Videos.Count > 0)
            {
                SelectedVideo = Videos[0];
            }
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
            if (IsPlayingVideo)
            {
                IsPlayingVideo = false;
            }

            base.Close();
        }
        #endregion
    }

    /// <summary>
    /// Design time version of the <see cref="VideosViewModel"/>.
    /// </summary>
    public class DesignVideosViewModel : VideosViewModel
    {
        private class DesignVideoProvider : IVideoProvider
        {
            private ObservableCollection<IVideoInfo> _items = new ObservableCollection<IVideoInfo>();

            public DesignVideoProvider()
            {
                var videos = VideoHelper.GetVideos(3);

                _items.AddRange((from video in videos
                                 select new VideoInfo(video) as IVideoInfo));
            }

            public ObservableCollection<IVideoInfo> Items
            {
                get { return _items; }
            }

            public string LogoUri
            {
                get { return ""; }
            }

            public string Name
            {
                get { return ""; }
            }

            public void Refresh()
            {
                throw new NotImplementedException();
            }
        }

        public DesignVideosViewModel()
            : base(new DesignVideoProvider())
        {
        }
    }
}
