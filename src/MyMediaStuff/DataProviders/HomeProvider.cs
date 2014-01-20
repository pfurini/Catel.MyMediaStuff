using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Catel.Collections.ObjectModel;

namespace MyMediaStuff.DataProviders
{
    public class HomeProvider : MediaProvider, IHomeProvider
    {
        #region Variables
        private readonly ObservableCollection<IPictureInfo> _pictures = new ObservableCollection<IPictureInfo>();
        private readonly ObservableCollection<IPictureInfo> _latestPictures = new ObservableCollection<IPictureInfo>();
        private readonly ObservableCollection<IVideoInfo> _videos = new ObservableCollection<IVideoInfo>();
        private readonly ObservableCollection<IVideoInfo> _latestVideos = new ObservableCollection<IVideoInfo>();
        private readonly ObservableCollection<IMediaInfo> _media = new ObservableCollection<IMediaInfo>();
        #endregion

        #region Constructor & destructor
        public HomeProvider()
            : base("Home", "/Resources/Images/Home.png")
        {
            Refresh();
        }
        #endregion

        #region Properties
        public ObservableCollection<IPictureInfo> Pictures
        {
            get { return _pictures; }
        }

        public ObservableCollection<IPictureInfo> LatestPictures
        {
            get { return _latestPictures; }
        }

        public ObservableCollection<IVideoInfo> Videos
        {
            get { return _videos; }
        }

        public ObservableCollection<IVideoInfo> LatestVideos
        {
            get { return _latestVideos; }
        }

        public ObservableCollection<IMediaInfo> Items
        {
            get { return _media; }
        }
        #endregion

        #region Methods
        public override void Refresh()
        {
            lock (_pictures)
            {
                _pictures.Clear();
                _latestPictures.Clear();

                var pictures = PictureHelper.GetPictures();

                _pictures.AddRange((from picture in pictures
                                    orderby new PictureInfo(picture).CreationTime descending
                                    select new PictureInfo(picture) as IPictureInfo));

                _latestPictures.AddRange(_pictures.Take(6));
            }

            lock (_videos)
            {
                _videos.Clear();
                _latestVideos.Clear();

                var videos = VideoHelper.GetVideos();

                _videos.AddRange((from video in videos
                                  orderby new VideoInfo(video).CreationTime descending
                                  select new VideoInfo(video) as IVideoInfo));

                _latestVideos.AddRange(_videos.Take(6));
            }

            lock (_media)
            {
                _media.Clear();

                List<IMediaInfo> media = new List<IMediaInfo>();
                media.AddRange(_pictures.Cast<IMediaInfo>());
                media.AddRange(_videos.Cast<IMediaInfo>());

                _media.AddRange(media.OrderBy(item => item.CreationTime));
            }
        }
        #endregion
    }
}
