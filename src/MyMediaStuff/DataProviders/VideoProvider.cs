using System.Collections.ObjectModel;
using System.Linq;
using Catel.Collections.ObjectModel;

namespace MyMediaStuff.DataProviders
{
    public class VideoProvider : MediaProvider, IVideoProvider
    {
        #region Variables
        private readonly ObservableCollection<IVideoInfo> _videos = new ObservableCollection<IVideoInfo>();
        #endregion

        #region Constructor & destructor
        public VideoProvider()
            : base("Videos", "/Resources/Images/Videos.png")
        {
            Refresh();
        }
        #endregion

        #region Properties
        public ObservableCollection<IVideoInfo> Items
        {
            get { return _videos; }
        }
        #endregion

        #region Methods
        public override void Refresh()
        {
            lock (_videos)
            {
                _videos.Clear();

                var files = VideoHelper.GetVideos();

                _videos.AddRange((from file in files
                                  select new VideoInfo(file) as IVideoInfo));
            }
        }
        #endregion
    }
}
