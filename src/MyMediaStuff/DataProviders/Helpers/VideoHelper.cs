using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using Path = Catel.IO.Path;

namespace MyMediaStuff.DataProviders
{
    public static class VideoHelper
    {
        #region Variables
        private static ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public static string MyVideos
        {
            get
            {
                // Quite tricky to get the right directory, why isn't there a MyVideos in the SpecialFolder enum...
                return Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..\\Videos"));
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the videos in the "My Videos" folder.
        /// </summary>
        /// <returns><see cref="IEnumerable{string}"/> containing the found filenames.</returns>
        public static IEnumerable<string> GetVideos()
        {
            return GetVideos(-1);
        }

        /// <summary>
        /// Gets the videos in the "My Videos" folder.
        /// </summary>
        /// <param name="maxFiles">The maximum number of files, unlimited if 0 or smaller.</param>
        /// <returns><see cref="IEnumerable{string}"/> containing the found filenames.</returns>
        public static IEnumerable<string> GetVideos(int maxFiles)
        {
            return GetVideos(MyVideos, maxFiles);
        }

        /// <summary>
        /// Gets the videos in the specified folder.
        /// </summary>
        /// <param name="path">The path to search for video files.</param>
        /// <returns>
        /// 	<see cref="IEnumerable{string}"/> containing the found filenames.
        /// </returns>
        public static IEnumerable<string> GetVideos(string path)
        {
            return GetVideos(path, -1);
        }

        /// <summary>
        /// Gets the videos in the specified folder.
        /// </summary>
        /// <param name="path">The path to search for video files.</param>
        /// <param name="maxFiles">The maximum number of files, unlimited if 0 or smaller.</param>
        /// <returns>
        /// 	<see cref="IEnumerable{string}"/> containing the found filenames.
        /// </returns>
        public static IEnumerable<string> GetVideos(string path, int maxFiles)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            try
            {
                var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                    .Where(file => file.ToLower().EndsWith(".avi") || file.ToLower().EndsWith(".mp4") ||
                        file.ToLower().EndsWith(".mpeg") || file.ToLower().EndsWith(".wmv"));

                return maxFiles > 0 ? files.Take(maxFiles) : files;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get videos from '{0}'", path);

                return new string[] { };
            }
        }
        #endregion
    }
}
