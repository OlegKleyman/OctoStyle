﻿namespace OctoStyle.Core.Tests.Unit
{
    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class PathResolverTests
    {
        private readonly string ProjectFileFilter = "*.csproj";

        private const string GetPathShouldReturnPathInitialFilePath =
            @"C:\testPath\innerDirectory1\innerDirectory2\someFile.cs";

        private const string GetPathShouldReturnPathInitialOuterMostDirectoryPath = @"C:\testPath\innerDirectory1";

        private const string GetPathShouldReturnPathInitialMiddleDirectoryPath = @"C:\testPath\innerDirectory1";

        private const string ProjectPath = @"C:\testPath";

        [TestCase(GetPathShouldReturnPathInitialFilePath, ProjectPath)]
        [TestCase(GetPathShouldReturnPathInitialOuterMostDirectoryPath, ProjectPath)]
        [TestCase(GetPathShouldReturnPathInitialMiddleDirectoryPath, ProjectPath)]
        [TestCase(ProjectPath, ProjectPath)]
        public void GetPathShouldReturnPath(string initialPath, string expectedPath)
        {
            IPathResolver resolver = this.GetPathResolver();
            var path = resolver.GetPath(initialPath, this.ProjectFileFilter);
            Assert.That(path, Is.EqualTo(expectedPath));
        }

        private PathResolver GetPathResolver()
        {
            var mockFileSystemManager = new Mock<IFileSystemManager>();

            mockFileSystemManager.Setup(
                manager => manager.GetFiles(GetPathShouldReturnPathInitialFilePath, this.ProjectFileFilter))
                .Returns(new string[] { });
            mockFileSystemManager.Setup(
                manager =>
                manager.GetFiles(GetPathShouldReturnPathInitialOuterMostDirectoryPath, this.ProjectFileFilter))
                .Returns(new string[] { });
            mockFileSystemManager.Setup(
                manager => manager.GetFiles(GetPathShouldReturnPathInitialMiddleDirectoryPath, this.ProjectFileFilter))
                .Returns(new string[] { });
            mockFileSystemManager.Setup(manager => manager.GetFiles(ProjectPath, this.ProjectFileFilter))
                .Returns(new[] { "test.csproj" });

            mockFileSystemManager.Setup(manager => manager.IsDirectory(GetPathShouldReturnPathInitialFilePath))
                .Returns(false);
            mockFileSystemManager.Setup(
                manager => manager.IsDirectory(GetPathShouldReturnPathInitialOuterMostDirectoryPath)).Returns(true);
            mockFileSystemManager.Setup(
                manager => manager.IsDirectory(GetPathShouldReturnPathInitialMiddleDirectoryPath)).Returns(true);
            mockFileSystemManager.Setup(manager => manager.IsDirectory(ProjectPath)).Returns(true);

            return new PathResolver(mockFileSystemManager.Object);
        }
    }
}