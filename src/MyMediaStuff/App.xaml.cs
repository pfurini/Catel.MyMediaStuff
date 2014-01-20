using System.Configuration;
using System.Windows;
using Catel.Windows;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using MyMediaStuff.DataProviders;

namespace MyMediaStuff
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ConfigureIoCContainer();

            StyleHelper.CreateStyleForwardersForDefaultStyles();
        }

        /// <summary>
        /// Configures the IoC container.
        /// </summary>
        private static void ConfigureIoCContainer()
        {
            Catel.IoC.UnityContainer.Instance.Container.RegisterType<IHomeProvider, HomeProvider>();
            Catel.IoC.UnityContainer.Instance.Container.RegisterType<IPictureProvider, PictureProvider>();
            Catel.IoC.UnityContainer.Instance.Container.RegisterType<IVideoProvider, VideoProvider>();

            // Load from config, overrides defaults
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            if ((section != null) && (section.Containers.Count > 0))
            {
                section.Configure(Catel.IoC.UnityContainer.Instance.Container);
            }
        }
    }
}
