namespace OctoStyle.Core.Tests.Unit
{
    using NUnit.Framework;

    [TestFixture]
    public class PathResolverTests
    {
        [Test]
        public void GetPathPathByFilePathShouldReturnPath()
        {
            var resolver = GetPathResolver();
        }

        private PathResolver GetPathResolver()
        {
            return new PathResolver();
        }
    }
}
