namespace OctoStyle.Core
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    public class PathResolver : IPathResolver
    {
        private readonly IFileSystemManager manager;

        public PathResolver(IFileSystemManager manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            this.manager = manager;
        }

        public string GetPath(string initialPath, string fileFilter)
        {
            if (manager.GetFiles(initialPath, fileFilter).Any())
            {
                return initialPath;
            }

            var directoryName = Path.GetDirectoryName(initialPath);
            
            if (directoryName == null)
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.InvariantCulture, "Directory name of {0} was not found", initialPath));
            }

            return GetPath(directoryName, fileFilter);
        }
    }
}