namespace OctoStyle.Core
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    public class PathResolver : IPathResolver
    {
        public string GetPath(string initialPath, string fileFilter)
        {
            if (initialPath == null)
            {
                throw new ArgumentNullException("initialPath");
            }

            if (fileFilter == null)
            {
                throw new ArgumentNullException("fileFilter");
            }

            const string cannotBeEmptyMessage = "Cannot be empty";

            if (initialPath.Length == 0)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "initialPath");
            }

            if (fileFilter.Length == 0)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "fileFilter");
            }

            var directoryName = Path.GetDirectoryName(initialPath);
            
            if (directoryName == null)
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.InvariantCulture, "Directory name of {0} was not found", initialPath));
            }

            if (Directory.GetFiles(directoryName, fileFilter).Any())
            {
                return directoryName;
            }

            return GetPath(directoryName, fileFilter);
        }
    }
}