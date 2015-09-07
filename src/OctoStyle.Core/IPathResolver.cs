namespace OctoStyle.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a path resolver.
    /// </summary>
    public interface IPathResolver
    {
        /// <summary>
        /// Gets the path which contains a file that matches the fileFilter argument going up from the initialPath location.
        /// </summary>
        /// <param name="initialPath">The initial path to start looking in.</param>
        /// <param name="fileFilter">The file filter to use to find the path.</param>
        /// <returns>The directory path which contains a file that matches the fileFilter argument.</returns>
        string GetDirectoryPath(string initialPath, string fileFilter);

        IEnumerable<string> GetFilePaths(string path, string fileFilter);
    }
}