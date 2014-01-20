using System.Collections.ObjectModel;

namespace MyMediaStuff.DataProviders
{
    public interface IHomeProvider : IMediaProvider<IMediaInfo>
    {
        ObservableCollection<IPictureInfo> LatestPictures { get; }

        ObservableCollection<IPictureInfo> Pictures { get; }

        ObservableCollection<IVideoInfo> LatestVideos { get; }

        ObservableCollection<IVideoInfo> Videos { get; }
    }
}
