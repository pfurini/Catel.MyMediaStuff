using System;

namespace MyMediaStuff.DataProviders
{
    public abstract class MediaProvider : IMediaProvider
    {
        #region Constructor & destructor
        protected MediaProvider(string name, string logoUri)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (logoUri == null)
            {
                throw new ArgumentNullException("logoUri");
            }

            Name = name;
            LogoUri = logoUri;
        }
        #endregion

        #region Properties
        public string Name { get; private set; }

        public string LogoUri { get; private set; }
        #endregion

        #region Methods
        public abstract void Refresh();
        #endregion
    }
}
