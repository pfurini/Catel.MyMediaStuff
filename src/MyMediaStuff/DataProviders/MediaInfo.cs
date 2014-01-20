using System;
using Catel.IO;

namespace MyMediaStuff.DataProviders
{
    public abstract class MediaInfo : IMediaInfo
    {
        #region Variables
        #endregion

        #region Constructor & destructor
        protected MediaInfo(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (!File.Exists(fileName))
            {
                throw new NotSupportedException("Non-existing files are not supported");
            }

            FileName = fileName;
            CreationTime = System.IO.File.GetCreationTime(fileName);
        }
        #endregion
        
        #region Properties
        public string FileName { get; private set; }

        public virtual string Title
        {
            get
            {
                string fileName = Path.GetFileName(FileName);

                int lastDotIndex = fileName.LastIndexOf('.');
                if (lastDotIndex != -1)
                {
                    fileName = fileName.Substring(0, fileName.Length - lastDotIndex);
                }

                return fileName;
            }
        }

        public DateTime CreationTime { get; private set; }
        #endregion

        #region Methods
        #endregion
    }
}
