namespace OctoStyle.Core.Tests.Unit
{
    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public static class PathResolverTests
    {
        private const string ProjectFileFilter = "*.csproj";

        private const string GetPathShouldReturnPathInitialFilePath =
            @"C:\testPath\innerDirectory1\innerDirectory2\someFile.cs";

        private const string GetPathShouldReturnPathInitialOuterMostDirectoryPath = @"C:\testPath\innerDirectory1";

        private const string GetPathShouldReturnPathInitialMiddleDirectoryPath = @"C:\testPath\innerDirectory1";

        private const string ProjectPath = @"C:\testPath";

        [TestCase(GetPathShouldReturnPathInitialFilePath, ProjectPath)]
        [TestCase(GetPathShouldReturnPathInitialOuterMostDirectoryPath, ProjectPath)]
        [TestCase(GetPathShouldReturnPathInitialMiddleDirectoryPath, ProjectPath)]
        [TestCase(ProjectPath, ProjectPath)]
        public static void GetPathShouldReturnPath(string initialPath, string expectedPath)
        {
            IPathResolver resolver = GetPathResolver();
            var path = resolver.GetDirectoryPath(initialPath, ProjectFileFilter);
            Assert.That(path, Is.EqualTo(expectedPath));
        }

        private static PathResolver GetPathResolver()
        {
            var mockFileSystemManager = new Mock<IFileSystemManager>();

            mockFileSystemManager.Setup(
                manager => manager.GetFiles(GetPathShouldReturnPathInitialFilePath, ProjectFileFilter))
                .Returns(new string[] { });
            mockFileSystemManager.Setup(
                manager =>
                manager.GetFiles(GetPathShouldReturnPathInitialOuterMostDirectoryPath, ProjectFileFilter))
                .Returns(new string[] { });
            mockFileSystemManager.Setup(
                manager => manager.GetFiles(GetPathShouldReturnPathInitialMiddleDirectoryPath, ProjectFileFilter))
                .Returns(new string[] { });
            mockFileSystemManager.Setup(manager => manager.GetFiles(ProjectPath, ProjectFileFilter))
                .Returns(new[] { "test.csproj" });

            mockFileSystemManager.Setup(manager => manager.IsDirectory(GetPathShouldReturnPathInitialFilePath))
                .Returns(false);
            mockFileSystemManager.Setup(
                manager => manager.IsDirectory(GetPathShouldReturnPathInitialOuterMostDirectoryPath)).Returns(true);
            mockFileSystemManager.Setup(
                manager => manager.IsDirectory(GetPathShouldReturnPathInitialMiddleDirectoryPath)).Returns(true);
            mockFileSystemManager.Setup(manager => manager.IsDirectory(ProjectPath)).Returns(true);
            mockFileSystemManager.Setup(manager => manager.PathExists(It.IsAny<string>())).Returns(true);

            return new PathResolver(mockFileSystemManager.Object);
        }
    }
}