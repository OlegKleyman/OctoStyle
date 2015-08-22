namespace OctoStyle.Core
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Represents a path resolver.
    /// </summary>
    public class PathResolver : IPathResolver
    {
        private readonly IFileSystemManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathResolver"/> class.
        /// </summary>
        /// <param name="manager">The <see cref="IFileSystemManager"/> to use to interface with the file system.</param>
        public PathResolver(IFileSystemManager manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            this.manager = manager;
        }

        /// <summary>
        /// Gets the path which contains a file that matches the fileFilter argument going up from the initialPath location.
        /// </summary>
        /// <param name="initialPath">The initial path to start looking in.</param>
        /// <param name="fileFilter">The file filter to use to find the path.</param>
        /// <returns>The directory path which contains a file that matches the fileFilter argument.</returns>
        public string GetPath(string initialPath, string fileFilter)
        {
            if (this.manager.PathExists(initialPath) && this.manager.IsDirectory(initialPath) && this.manager.GetFiles(initialPath, fileFilter).Any())
            {
                return initialPath;
            }

            var directoryName = Path.GetDirectoryName(initialPath);

            if (directoryName == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "Directory name of {0} was not found", initialPath));
            }

            return this.GetPath(directoryName, fileFilter);
        }
    }
}