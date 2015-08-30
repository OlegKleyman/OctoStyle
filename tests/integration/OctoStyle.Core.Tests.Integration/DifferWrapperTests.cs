namespace OctoStyle.Core.Tests.Integration
{
    using System.Linq;

    using NUnit.Framework;

    using SharpDiff.FileStructure;

    [TestFixture]
    public static class DifferWrapperTests
    {
        [Test]
        public static void LoadShouldReturnDiff()
        {
            var differ = GetDifferWrapper();

            var diff = differ.Load(FileContents.TestLibraryCsprojDiff).ToList();

            Assert.That(diff.Count, Is.EqualTo(1));
            Assert.That(diff[0].Chunks.Count, Is.EqualTo(1));

            var snippets = diff[0].Chunks[0].Snippets.ToList();
            Assert.That(snippets.Count, Is.EqualTo(7));
            Assert.That(snippets[0], Is.InstanceOf<ContextSnippet>());
            Assert.That(snippets[1], Is.InstanceOf<AdditionSnippet>());
            Assert.That(snippets[2], Is.InstanceOf<ContextSnippet>());
            Assert.That(snippets[3], Is.InstanceOf<ModificationSnippet>());
            Assert.That(snippets[4], Is.InstanceOf<ContextSnippet>());
            Assert.That(snippets[5], Is.InstanceOf<AdditionSnippet>());
            Assert.That(snippets[6], Is.InstanceOf<ContextSnippet>());
        }

        private static IDiffer GetDifferWrapper()
        {
            return new DifferWrapper();
        }
    }
}
