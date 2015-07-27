namespace OctoStyle.Core
{
    /// <summary>
    /// Represents a file system interface.
    /// </summary>
    public interface IFileSystemManager
    {
        /// <summary>
        ///     Gets files from a directory matching a filter.
        /// </summary>
        /// <param name="targetDirectory"> The directory to investigate files in. </param>
        /// <param name="fileFilter"> The filter of the files to be retrieved. </param>
        /// <returns>
        ///     A <see cref="System.String" /> array of file paths.
        /// </returns>
        string[] GetFiles(string targetDirectory, string fileFilter);

        /// <summary>
        /// Finds whether a given path is a directory.
        /// </summary>
        /// <param name="path">The path to analyze.</param>
        /// <returns>True if the path is a directory.</returns>
        bool IsDirectory(string path);
    }
}