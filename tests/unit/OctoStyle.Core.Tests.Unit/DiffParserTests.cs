namespace OctoStyle.Core.Tests.Unit
{
    using NUnit.Framework;

    [TestFixture]
    public static class DiffParserTests
    {
        [Test]
        public static void SplitShouldReturnDiffSplit()
        {
            var parser = GetDiffParser();

            var result = parser.Split(FileContents.FullDiff);

            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result["src/TestLibrary/Nested/TestClass2.cs"], Is.EqualTo(FileContents.TestClass2CsDiff));
            Assert.That(
                result["src/TestLibrary/TestClass.cs"],
                Is.EqualTo(FileContents.TestClassCsDiff));
            Assert.That(result["src/TestLibrary/TestLibrary.csproj"], Is.EqualTo(FileContents.TestLibraryCsprojDiff));
            Assert.That(result["src/TestLibrary/Nested/TestClass3.cs"], Is.EqualTo(FileContents.TestClass3CsDiff));
            Assert.That(result["src/TestLibrary/TestClass2.cs"], Is.EqualTo(FileContents.TestClass2CsDeletedDiff));
        }

        private static IDiffParser GetDiffParser()
        {
            return new DiffParser();
        }
    }
}
