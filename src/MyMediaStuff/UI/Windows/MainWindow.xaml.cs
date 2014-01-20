using System;
using System.Collections.ObjectModel;
using System.Windows;
using MyMediaStuff.Data;
using MyMediaStuff.UI.ViewModels;
using Catel.Windows;

namespace MyMediaStuff.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DataWindow<MainWindowViewModel>
    {
        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
            : base(DataWindowMode.Custom)
        {
            InitializeComponent();

            AvailableThemes = new ObservableCollection<ThemeInfo>();
            AvailableThemes.Add(new ThemeInfo("Aero - normal", "/Catel.Windows;component/themes/aero/catel.aero.normal.xaml"));
            AvailableThemes.Add(new ThemeInfo("Aero - large", "/Catel.Windows;component/themes/aero/catel.aero.large.xaml"));
            AvailableThemes.Add(new ThemeInfo("Expression Dark - normal", "/Catel.Windows;component/themes/expressiondark/catel.expressiondark.normal.xaml"));
            AvailableThemes.Add(new ThemeInfo("Expression Dark - large", "/Catel.Windows;component/themes/expressiondark/catel.expressiondark.large.xaml"));
            AvailableThemes.Add(new ThemeInfo("Sunny Orange", "/Catel.Windows;component/themes/sunnyorange/generic.xaml"));
            SelectedTheme = AvailableThemes[2];
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the available themes.
        /// </summary>
        /// <value>The available themes.</value>
        public ObservableCollection<ThemeInfo> AvailableThemes
        {
            get { return (ObservableCollection<ThemeInfo>)GetValue(AvailableThemesProperty); }
            set { SetValue(AvailableThemesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AvailableThemes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvailableThemesProperty =
            DependencyProperty.Register("AvailableThemes", typeof(ObservableCollection<ThemeInfo>), typeof(MainWindow), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the selected theme.
        /// </summary>
        /// <value>The selected theme.</value>
        public ThemeInfo SelectedTheme
        {
            get { return (ThemeInfo)GetValue(SelectedThemeProperty); }
            set { SetValue(SelectedThemeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTheme.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedThemeProperty =
            DependencyProperty.Register("SelectedTheme", typeof(ThemeInfo), typeof(MainWindow), 
            new UIPropertyMetadata(null, (sender, e) => ((MainWindow)sender).OnSelectedThemeChanged()));
        #endregion

        #region Methods
        /// <summary>
        /// Called when the <see cref="SelectedTheme"/> property has changed.
        /// </summary>
        private void OnSelectedThemeChanged()
        {
            var currentApp = Application.Current;
            if (currentApp == null)
            {
                return;
            }

            // Need to call this twice because the first update fixes the dictionaries, and the second one actually updates the UI
            UpdateApplicationResources(currentApp);
            UpdateApplicationResources(currentApp);
        }

        /// <summary>
        /// Updates the application resources.
        /// </summary>
        /// <param name="currentApp">The current application.</param>
        private void UpdateApplicationResources(Application currentApp)
        {
            currentApp.Resources.Clear();
            currentApp.Resources.MergedDictionaries.Clear();

            ResourceDictionary resourceDictionary = new ResourceDictionary();
            resourceDictionary.Source = new Uri(SelectedTheme.Source, UriKind.RelativeOrAbsolute);

            currentApp.Resources.MergedDictionaries.Add(resourceDictionary);

            StyleHelper.CreateStyleForwardersForDefaultStyles();
        }
        #endregion
    }
}
