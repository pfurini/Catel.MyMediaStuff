using System;

namespace MyMediaStuff.DataProviders
{
    public interface IMediaInfo
    {
        string FileName { get; }

        string Title { get; }

        DateTime CreationTime { get; }
    }
}
