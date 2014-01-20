using System.Windows;
using Catel.MVVM.UI;
using MyMediaStuff.UI.ViewModels;
using Catel.Windows.Controls;

namespace MyMediaStuff.UI.Controls
{
    /// <summary>
    /// Interaction logic for VideosView.xaml
    /// </summary>
    public partial class VideosView : UserControl<VideosViewModel>
    {
        public VideosView()
        {
            InitializeComponent();
        }

        [ControlToViewModel]
        public bool IsPlayingVideo
        {
            get { return (bool)GetValue(IsPlayingVideoProperty); }
            set { SetValue(IsPlayingVideoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPlayingVideo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPlayingVideoProperty = DependencyProperty.Register("IsPlayingVideo", typeof(bool), typeof(VideosView),
            new UIPropertyMetadata(false, (sender, e) => ((VideosView)sender).OnIsPlayingVideoChanged(sender, e)));

        private void OnIsPlayingVideoChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsPlayingVideo)
            {
                selectedVideoMediaElement.Play();
            }
            else
            {
                selectedVideoMediaElement.Stop();
            }
        }
    }
}
