namespace OctoStyle.Core.Tests.Unit
{
    using System.IO;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public static class PathResolverTests
    {
        private const string ProjectFileFilter = "*.csproj";

        private const string SolutionFileFilter = "*.sln";

        private const string InitialFilePath =
            @"C:\testPath\innerDirectory1\innerDirectory2\someFile.cs";

        private const string InitialOuterMostDirectoryPath = @"C:\testPath\innerDirectory1";

        private const string InitialMiddleDirectoryPath = @"C:\testPath\innerDirectory1";

        private const string ProjectDirectoryPath = @"C:\testPath";
        private const string SolutionFilePath = @"C:\testPath\TestSolution.sln";

        [TestCase(InitialFilePath, ProjectDirectoryPath)]
        [TestCase(InitialOuterMostDirectoryPath, ProjectDirectoryPath)]
        [TestCase(InitialMiddleDirectoryPath, ProjectDirectoryPath)]
        [TestCase(ProjectDirectoryPath, ProjectDirectoryPath)]
        public static void GetDirectoryPathShouldReturnPath(string initialPath, string expectedPath)
        {
            IPathResolver resolver = GetPathResolver();
            var path = resolver.GetDirectoryPath(initialPath, ProjectFileFilter);
            Assert.That(path, Is.EqualTo(expectedPath));
        }

        [TestCase(InitialFilePath, SolutionFilePath)]
        [TestCase(InitialOuterMostDirectoryPath, SolutionFilePath)]
        [TestCase(InitialMiddleDirectoryPath, SolutionFilePath)]
        [TestCase(ProjectDirectoryPath, SolutionFilePath)]
        public static void GetFilePathsShouldReturnPath(string initialPath, string expectedPath)
        {
            IPathResolver resolver = GetPathResolver();
            var paths = resolver.GetFilePaths(initialPath, SolutionFileFilter).ToArray();
            Assert.That(paths.Length, Is.EqualTo(1));
            Assert.That(paths[0], Is.EqualTo(expectedPath));
        }

        private static PathResolver GetPathResolver()
        {
            var mockFileSystemManager = new Mock<IFileSystemManager>();

            mockFileSystemManager.Setup(
                manager => manager.GetFiles(InitialFilePath, ProjectFileFilter))
                .Returns(new string[] { });
            mockFileSystemManager.Setup(
                manager =>
                manager.GetFiles(InitialOuterMostDirectoryPath, It.Is<string>(s => s == ProjectFileFilter || s == SolutionFileFilter)))
                .Returns(new string[] { });
            mockFileSystemManager.Setup(
                manager => manager.GetFiles(InitialMiddleDirectoryPath, It.Is<string>(s => s == ProjectFileFilter || s == SolutionFileFilter)))
                .Returns(new string[] { });
            mockFileSystemManager.Setup(manager => manager.GetFiles(ProjectDirectoryPath, ProjectFileFilter))
                .Returns(new[] { "test.csproj" });
            mockFileSystemManager.Setup(manager => manager.GetFiles(ProjectDirectoryPath, SolutionFileFilter))
                .Returns(new[] { SolutionFilePath });

            mockFileSystemManager.Setup(manager => manager.IsDirectory(InitialFilePath))
                .Returns(false);
            mockFileSystemManager.Setup(
                manager => manager.IsDirectory(InitialOuterMostDirectoryPath)).Returns(true);
            mockFileSystemManager.Setup(
                manager => manager.IsDirectory(InitialMiddleDirectoryPath)).Returns(true);
            mockFileSystemManager.Setup(manager => manager.IsDirectory(ProjectDirectoryPath)).Returns(true);
            mockFileSystemManager.Setup(manager => manager.PathExists(It.IsAny<string>())).Returns(true);

            return new PathResolver(mockFileSystemManager.Object);
        }
    }
}