using System.Collections.Generic;
using Catel.Data;
using Catel.MVVM;
using Microsoft.Practices.Unity;
using MyMediaStuff.DataProviders;

namespace MyMediaStuff.UI.ViewModels
{
    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            MediaProviders = new List<IMediaProvider>();

            var unityContainer = Catel.IoC.UnityContainer.Instance.Container;
            MediaProviders.Add(unityContainer.Resolve<IHomeProvider>());
            MediaProviders.Add(unityContainer.Resolve<IPictureProvider>());
            MediaProviders.Add(unityContainer.Resolve<IVideoProvider>());
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title { get { return "My Media Stuff"; } }

        #region Models
        #endregion

        #region View model
        /// <summary>
        /// Gets or sets the selected media provider.
        /// </summary>
        public IMediaProvider SelectedMediaProvider
        {
            get { return GetValue<IMediaProvider>(SelectedMediaProviderProperty); }
            set { SetValue(SelectedMediaProviderProperty, value); }
        }

        /// <summary>
        /// Register the SelectedMediaProvider property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SelectedMediaProviderProperty = RegisterProperty("SelectedMediaProvider", typeof(IMediaProvider));

        /// <summary>
        /// Gets or sets the list of available providers.
        /// </summary>
        public List<IMediaProvider> MediaProviders
        {
            get { return GetValue<List<IMediaProvider>>(MediaProvidersProperty); }
            set { SetValue(MediaProvidersProperty, value); }
        }

        /// <summary>
        /// Register the MediaProviders property so it is known in the class.
        /// </summary>
        public static readonly PropertyData MediaProvidersProperty = RegisterProperty("MediaProviders", typeof(List<IMediaProvider>));
        #endregion
        #endregion

        #region Commands
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the object by setting default values.
        /// </summary>	
        protected override void Initialize()
        {
            SelectedMediaProvider = MediaProviders[0];
        }
        #endregion
    }
}
