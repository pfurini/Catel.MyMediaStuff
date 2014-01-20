using System.Collections.ObjectModel;
using System.Linq;
using Catel.Collections.ObjectModel;

namespace MyMediaStuff.DataProviders
{
    public class PictureProvider : MediaProvider, IPictureProvider
    {
        #region Variables
        private readonly ObservableCollection<IPictureInfo> _pictures = new ObservableCollection<IPictureInfo>();
        #endregion

        #region Constructor & destructor
        public PictureProvider()
            : base("Pictures", "/Resources/Images/Pictures.png")
        {
            Refresh();
        }
        #endregion

        #region Properties
        public ObservableCollection<IPictureInfo> Items
        {
            get { return _pictures; }
        }
        #endregion

        #region Methods
        public override void Refresh()
        {
            lock (_pictures)
            {
                _pictures.Clear();

                var files = PictureHelper.GetPictures();

                _pictures.AddRange((from file in files
                                    select new PictureInfo(file) as IPictureInfo));
            }
        }
        #endregion
    }
}
