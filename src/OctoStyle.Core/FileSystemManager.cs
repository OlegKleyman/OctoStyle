namespace OctoStyle.Core
{
    using System.IO;

    /// <summary>
    /// Represents a file system manager that interfaces with the file system.
    /// </summary>
    public class FileSystemManager : IFileSystemManager
    {
        /// <summary>
        ///     Gets files from a directory matching a filter.
        /// </summary>
        /// <param name="targetDirectory"> The directory to investigate files in. </param>
        /// <param name="fileFilter"> The filter of the files to be retrieved. </param>
        /// <returns>
        ///     A <see cref="System.String" /> array of file paths.
        /// </returns>
        public string[] GetFiles(string targetDirectory, string fileFilter)
        {
            return Directory.GetFiles(targetDirectory, fileFilter);
        }

        /// <summary>
        /// Finds whether a given path is a directory.
        /// </summary>
        /// <param name="path">The path to analyze.</param>
        /// <returns>True if the path is a directory.</returns>
        public bool IsDirectory(string path)
        {
            var attributes = File.GetAttributes(path);
            return attributes.HasFlag(FileAttributes.Directory);
        }

        /// <summary>
        /// Checks whether a given path exists on the file system.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>
        /// Returns a <see cref="bool"/> of <c>true</c> if the path exists and <c>false</c> if it doesn't.
        /// </returns>
        public bool PathExists(string path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }
    }
}