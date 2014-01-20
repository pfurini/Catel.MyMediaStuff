using System.Collections.ObjectModel;

namespace MyMediaStuff.DataProviders
{
    public interface IMediaProvider
    {
        string LogoUri { get; }

        string Name { get; }

        void Refresh();
    }

    public interface IMediaProvider<TMediaType> : IMediaProvider where TMediaType : IMediaInfo
    {
        ObservableCollection<TMediaType> Items { get; }
    }
}
