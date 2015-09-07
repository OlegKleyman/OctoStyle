namespace OctoStyle.Core.Tests.Unit
{
    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public static class PathResolverTests
    {
        private const string ProjectFileFilter = "*.csproj";

        private const string InitialFilePath =
            @"C:\testPath\innerDirectory1\innerDirectory2\someFile.cs";

        private const string InitialOuterMostDirectoryPath = @"C:\testPath\innerDirectory1";

        private const string InitialMiddleDirectoryPath = @"C:\testPath\innerDirectory1";

        private const string ProjectPath = @"C:\testPath";

        [TestCase(InitialFilePath, ProjectPath)]
        [TestCase(InitialOuterMostDirectoryPath, ProjectPath)]
        [TestCase(InitialMiddleDirectoryPath, ProjectPath)]
        [TestCase(ProjectPath, ProjectPath)]
        public static void GetDirectoryPathShouldReturnPath(string initialPath, string expectedPath)
        {
            IPathResolver resolver = GetPathResolver();
            var path = resolver.GetDirectoryPath(initialPath, ProjectFileFilter);
            Assert.That(path, Is.EqualTo(expectedPath));
        }

        private static PathResolver GetPathResolver()
        {
            var mockFileSystemManager = new Mock<IFileSystemManager>();

            mockFileSystemManager.Setup(
                manager => manager.GetFiles(InitialFilePath, ProjectFileFilter))
                .Returns(new string[] { });
            mockFileSystemManager.Setup(
                manager =>
                manager.GetFiles(InitialOuterMostDirectoryPath, ProjectFileFilter))
                .Returns(new string[] { });
            mockFileSystemManager.Setup(
                manager => manager.GetFiles(InitialMiddleDirectoryPath, ProjectFileFilter))
                .Returns(new string[] { });
            mockFileSystemManager.Setup(manager => manager.GetFiles(ProjectPath, ProjectFileFilter))
                .Returns(new[] { "test.csproj" });

            mockFileSystemManager.Setup(manager => manager.IsDirectory(InitialFilePath))
                .Returns(false);
            mockFileSystemManager.Setup(
                manager => manager.IsDirectory(InitialOuterMostDirectoryPath)).Returns(true);
            mockFileSystemManager.Setup(
                manager => manager.IsDirectory(InitialMiddleDirectoryPath)).Returns(true);
            mockFileSystemManager.Setup(manager => manager.IsDirectory(ProjectPath)).Returns(true);
            mockFileSystemManager.Setup(manager => manager.PathExists(It.IsAny<string>())).Returns(true);

            return new PathResolver(mockFileSystemManager.Object);
        }
    }
}