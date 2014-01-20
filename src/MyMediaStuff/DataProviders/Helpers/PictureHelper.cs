using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;

namespace MyMediaStuff.DataProviders
{
    public static class PictureHelper
    {
        #region Variables
        private static ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public static string MyPictures
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the pictures in the "My Pictures" folder.
        /// </summary>
        /// <returns><see cref="IEnumerable{string}"/> containing the found filenames.</returns>
        public static IEnumerable<string> GetPictures()
        {
            return GetPictures(-1);
        }

        /// <summary>
        /// Gets the pictures in the "My Pictures" folder.
        /// </summary>
        /// <param name="maxFiles">The maximum number of files, unlimited if 0 or smaller.</param>
        /// <returns><see cref="IEnumerable{string}"/> containing the found filenames.</returns>
        public static IEnumerable<string> GetPictures(int maxFiles)
        {
            return GetPictures(MyPictures, maxFiles);
        }

        /// <summary>
        /// Gets the pictures in the specified folder.
        /// </summary>
        /// <param name="path">The path to search for picture files.</param>
        /// <returns>
        /// 	<see cref="IEnumerable{string}"/> containing the found filenames.
        /// </returns>
        public static IEnumerable<string> GetPictures(string path)
        {
            return GetPictures(path, -1);
        }

        /// <summary>
        /// Gets the pictures in the specified folder.
        /// </summary>
        /// <param name="path">The path to search for picture files.</param>
        /// <param name="maxFiles">The maximum number of files, unlimited if 0 or smaller.</param>
        /// <returns>
        /// 	<see cref="IEnumerable{string}"/> containing the found filenames.
        /// </returns>
        public static IEnumerable<string> GetPictures(string path, int maxFiles)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            try
            {
                var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                    .Where(file => file.ToLower().EndsWith(".jpg") || file.ToLower().EndsWith(".png"));

                return maxFiles > 0 ? files.Take(maxFiles) : files;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get pictures from '{0}'", path);

                return new string[] {};
            }
        }


        #endregion
    }
}
