namespace OctoStyle.Core.Tests.Unit
{
    using NUnit.Framework;

    [TestFixture]
    public class PathResolverTests
    {
        [TestCase(@"C:\testPath\innerDirectory1\innerDirectory2\someFile.cs", @"C:\testPath")]
        [TestCase(@"C:\testPath\innerDirectory1\innerDirectory2", @"C:\testPath")]
        [TestCase(@"C:\testPath\innerDirectory1", @"C:\testPath")]
        [TestCase(@"C:\testPath", @"C:\testPath")]
        public void GetPathShouldReturnPath(string initialPath, string expectedPath)
        {
            IPathResolver resolver = GetPathResolver();
            var path = resolver.GetPath(initialPath, "*.csproj");
            Assert.That(path, Is.EqualTo(expectedPath));
        }

        private PathResolver GetPathResolver()
        {
            return new PathResolver();
        }
    }
}
