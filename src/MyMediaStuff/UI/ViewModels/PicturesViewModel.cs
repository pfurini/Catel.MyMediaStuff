using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using Catel.Collections.ObjectModel;
using Catel.Data;
using Catel.MVVM;
using Catel.MVVM.Services;
using MyMediaStuff.DataProviders;

namespace MyMediaStuff.UI.ViewModels
{
    /// <summary>
    /// Pictures view model.
    /// </summary>
    public class PicturesViewModel : ViewModelBase
    {
        #region Variables
        private readonly DispatcherTimer _slideshowTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 2, 500) };
        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PicturesViewModel"/> class.
        /// </summary>
        public PicturesViewModel(IPictureProvider pictureProvider)
        {
            PictureProvider = pictureProvider;

            PlaySlideshow = new Command<object, object>(OnPlaySlideshowExecute, OnPlaySlideshowCanExecute);
            StopSlideshow = new Command<object>(OnStopSlideshowExecute);
            Refresh = new Command<object, object>(OnRefreshExecute, OnRefreshCanExecute);
            ViewInSoftware = new Command<object, object>(OnViewInSoftwareExecute, OnViewInSoftwareCanExecute);

            _slideshowTimer.Tick += OnSlideshowTimerTick;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title { get { return PictureProvider.Name; } }

        #region Models
        /// <summary>
        /// Gets the picture provider.
        /// </summary>
        [Model]
        public IPictureProvider PictureProvider
        {
            get { return GetValue<IPictureProvider>(PictureProviderProperty); }
            private set { SetValue(PictureProviderProperty, value); }
        }

        /// <summary>
        /// Register the PictureProvider property so it is known in the class.
        /// </summary>
        public static readonly PropertyData PictureProviderProperty = RegisterProperty("PictureProvider", typeof(IPictureProvider));
        #endregion

        #region View model
        /// <summary>
        /// Gets or sets the pictures.
        /// </summary>
        [ViewModelToModel("PictureProvider", "Items")]
        public ObservableCollection<IPictureInfo> Pictures
        {
            get { return GetValue<ObservableCollection<IPictureInfo>>(PicturesProperty); }
            set { SetValue(PicturesProperty, value); }
        }

        /// <summary>
        /// Register the Pictures property so it is known in the class.
        /// </summary>
        public static readonly PropertyData PicturesProperty = RegisterProperty("Pictures", typeof(ObservableCollection<IPictureInfo>));

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
        public static readonly PropertyData SelectedPictureProperty = RegisterProperty("SelectedPicture", typeof(IPictureInfo));

        /// <summary>
        /// Gets a value indicating wether a slideshow is currently being played.
        /// </summary>
        public bool IsPlayingSlideshow
        {
            get { return GetValue<bool>(IsPlayingSlideshowProperty); }
            private set { SetValue(IsPlayingSlideshowProperty, value); }
        }

        /// <summary>
        /// Register the IsPlayingSlideshow property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsPlayingSlideshowProperty = RegisterProperty("IsPlayingSlideshow", typeof(bool));
        #endregion
        #endregion

        #region Commands
        /// <summary>
        /// Gets the PlaySlideshow command.
        /// </summary>
        public Command<object, object> PlaySlideshow { get; private set; }

        /// <summary>
        /// Method to check whether the PlaySlideshow command can be executed.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        private bool OnPlaySlideshowCanExecute(object parameter)
        {
            return (Pictures.Count > 1); // no use to show the same image over and over again
        }

        /// <summary>
        /// Method to invoke when the PlaySlideshow command is executed.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        private void OnPlaySlideshowExecute(object parameter)
        {
            IsPlayingSlideshow = true;

            if (SelectedPicture == null)
            {
                SelectedPicture = Pictures[0];
            }

            _slideshowTimer.Start();
        }

        /// <summary>
        /// Gets the StopSlideshow command.
        /// </summary>
        public Command<object> StopSlideshow { get; private set; }

        /// <summary>
        /// Method to invoke when the StopSlideshow command is executed.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        private void OnStopSlideshowExecute(object parameter)
        {
            _slideshowTimer.Stop();

            IsPlayingSlideshow = false;
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
            return !IsPlayingSlideshow;
        }

        /// <summary>
        /// Method to invoke when the Refresh command is executed.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        private void OnRefreshExecute(object parameter)
        {
            var pleaseWaitService = GetService<IPleaseWaitService>();
            pleaseWaitService.Show(() => PictureProvider.Refresh(), "Refreshing...");
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
            return SelectedPicture != null;
        }

        /// <summary>
        /// Method to invoke when the ViewInSoftware command is executed.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        private void OnViewInSoftwareExecute(object parameter)
        {
            var processService = GetService<IProcessService>();
            processService.StartProcess(SelectedPicture.FileName);
        }
        #endregion

        #region Methods
        private void OnSlideshowTimerTick(object sender, EventArgs e)
        {
            int index = (SelectedPicture == null) ? -1 : Pictures.IndexOf(SelectedPicture);
            if (index == Pictures.Count - 1)
            {
                index = -1;
            }

            SelectedPicture = Pictures[++index];
        }

        /// <summary>
        /// Initializes the object by setting default values.
        /// </summary>
        protected override void Initialize()
        {
            if (Pictures.Count > 0)
            {
                SelectedPicture = Pictures[0];
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
            if (_slideshowTimer.IsEnabled)
            {
                _slideshowTimer.Stop();
            }

            base.Close();
        }
        #endregion
    }

    /// <summary>
    /// Design time version of the <see cref="PicturesViewModel"/>.
    /// </summary>
    public class DesignPicturesViewModel : PicturesViewModel
    {
        private class DesignPictureProvider : IPictureProvider
        {
            private ObservableCollection<IPictureInfo> _items = new ObservableCollection<IPictureInfo>();

            public DesignPictureProvider()
            {
                var pictures = PictureHelper.GetPictures(3);

                _items.AddRange((from picture in pictures
                                 select new PictureInfo(picture) as IPictureInfo));
            }

            public ObservableCollection<IPictureInfo> Items
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

        public DesignPicturesViewModel()
            : base(new DesignPictureProvider())
        {
        }
    }
}
