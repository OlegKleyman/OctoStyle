namespace OctoStyle.Core.Tests.Integration
{
    using NUnit.Framework;

    [TestFixture]
    public class FileSystemManagerTests
    {
        private static readonly object[] GetFilesShouldReturnFilesInDirectoryCases =
            {
                new object[]{ @"FileSystemManagerFiles", "*.txt", new[]
                                                                      {
                                                                          @"FileSystemManagerFiles\TestFile1.txt",
                                                                          @"FileSystemManagerFiles\TestFile2.txt",
                                                                          @"FileSystemManagerFiles\TestFile3.txt",
                                                                          @"FileSystemManagerFiles\TestFile4.txt",
                                                                      } },
                new object[]{ @"FileSystemManagerFiles", "*.csv", new []
                                                                      {
                                                                          
                                                                          @"FileSystemManagerFiles\TestFile1.csv",
                                                                          @"FileSystemManagerFiles\TestFile2.csv",
                                                                      } }
            };

        [Test]
        [TestCaseSource("GetFilesShouldReturnFilesInDirectoryCases")]
        public void GetFilesShouldReturnFilesInDirectory(string directoryPath, string fileFilter, string[] expectedFiles)
        {
            var manager = GetFileSystemManager();
            var files = manager.GetFiles(directoryPath, fileFilter);

            Assert.That(files.Length, Is.EqualTo(expectedFiles.Length));

            for (int i = 0; i < files.Length; i++)
            {
                Assert.That(files[i], Is.EqualTo(expectedFiles[i]));
            }
        }

        [TestCase(@"FileSystemManagerFiles\TestDirectory\TestFile.txt", false)]
        [TestCase(@"FileSystemManagerFiles\TestDirectory", true)]
        public void IsDirectoryShouldReturnWhetherPathIsDiretory(string path, bool expected)
        {
            var manager = GetFileSystemManager();
            
            Assert.That(manager.IsDirectory(path), Is.EqualTo(expected));
        }

        private FileSystemManager GetFileSystemManager()
        {
            return new FileSystemManager();
        }
    }
}
