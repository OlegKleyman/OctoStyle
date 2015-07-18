namespace OctoStyle.Core
{
    using System;
    using System.IO;

    public class FileSystemManager : IFileSystemManager
    {
        /// <summary>
        ///     Gets files from a directory matching a filter.
        /// </summary>
        /// <param name="targetDirectory"> The directory to investigate files in. </param>
        /// <param name="fileFilter"> The filter of the files to be retrieved. </param>
        /// <returns>
        ///     A <see cref="System.String"/> array of file paths.
        /// </returns>
        public string[] GetFiles(string targetDirectory, string fileFilter)
        {
            return Directory.GetFiles(targetDirectory, fileFilter);
        }

        public bool IsDirectory(string path)
        {
            var attributes = File.GetAttributes(path);
            return attributes.HasFlag(FileAttributes.Directory);
        }
    }
}