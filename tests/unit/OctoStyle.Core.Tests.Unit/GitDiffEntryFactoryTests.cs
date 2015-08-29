namespace OctoStyle.Core.Tests.Unit
{
    using System.Linq;

    using NUnit.Framework;

    using OctoStyle.Core.Borrowed;

    using SharpDiff;

    [TestFixture]
    public static class GitDiffEntryFactoryTests
    {
        [Test]
        public static void GetEntryShouldReturnEqualGitDiffEntry()
        {
            var factory = GetGitDiffEntryFactory();

            var entry = new DiffEntry(DiffEntryType.Equal, null, 0);
            entry.Count = 3;

            var result = factory.Get(entry, 3);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));

            Assert.That(result[0], Is.InstanceOf<EqualGitDiffEntry>());
            Assert.That(result[0].Position, Is.EqualTo(3));
            Assert.That(result[1], Is.InstanceOf<EqualGitDiffEntry>());
            Assert.That(result[1].Position, Is.EqualTo(4));
            Assert.That(result[2], Is.InstanceOf<EqualGitDiffEntry>());
            Assert.That(result[2].Position, Is.EqualTo(5));
        }

        [Test]
        public static void GetEntryShouldReturnModificationGitDiffEntryForNewLines()
        {
            var factory = GetGitDiffEntryFactory();

            var entry = new DiffEntry(DiffEntryType.Add, null, 4);

            var result = factory.Get(entry, 3);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));

            Assert.That(result[0], Is.InstanceOf<ModificationGitDiffEntry>());
            Assert.That(result[0].Position, Is.EqualTo(3));
            var modifiedEntry = (ModificationGitDiffEntry)result[0];
            Assert.That(modifiedEntry.LineNumber, Is.EqualTo(4));
            Assert.That(modifiedEntry.Status, Is.EqualTo(GitDiffEntryStatus.New));
        }

        [Test]
        public static void GetEntryShouldReturnModificationGitDiffEntryForRemovedLines()
        {
            var factory = GetGitDiffEntryFactory();

            var entry = new DiffEntry(DiffEntryType.Remove, null, 4);

            var result = factory.Get(entry, 3);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));

            Assert.That(result[0], Is.InstanceOf<ModificationGitDiffEntry>());
            Assert.That(result[0].Position, Is.EqualTo(3));
            var modifiedEntry = (ModificationGitDiffEntry)result[0];
            Assert.That(modifiedEntry.LineNumber, Is.EqualTo(4));
            Assert.That(modifiedEntry.Status, Is.EqualTo(GitDiffEntryStatus.Removed));
        }

        [Test]
        public static void GetShouldReturnGitEqualDiffForContextSnippetLine()
        {
            var diff = Differ.Load(FileContents.TestLibraryCsprojDiff).ToList();

            var factory = GetGitDiffEntryFactory();

            var equalDiff = factory.Get(diff[0].Chunks[0].Snippets.ToList()[0], 1, 30);

            Assert.That(equalDiff.Count, Is.EqualTo(3));
            Assert.That(equalDiff[0], Is.InstanceOf<EqualGitDiffEntry>());
            Assert.That(equalDiff[0].Position, Is.EqualTo(1));
            Assert.That(equalDiff[1], Is.InstanceOf<EqualGitDiffEntry>());
            Assert.That(equalDiff[1].Position, Is.EqualTo(2));
            Assert.That(equalDiff[2], Is.InstanceOf<EqualGitDiffEntry>());
            Assert.That(equalDiff[2].Position, Is.EqualTo(3));
        }

        [Test]
        public static void GetShouldReturnGitModificationDiffForAdditionSnippet()
        {
            var diff = Differ.Load(FileContents.TestLibraryCsprojDiff).ToList();

            var factory = GetGitDiffEntryFactory();

            var result = factory.Get(diff[0].Chunks[0].Snippets.ToList()[1], 4, 33);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.InstanceOf<ModificationGitDiffEntry>());
            
            var modifiedEntry = (ModificationGitDiffEntry)result[0];
            Assert.That(modifiedEntry.LineNumber, Is.EqualTo(33));
            Assert.That(modifiedEntry.Status, Is.EqualTo(GitDiffEntryStatus.New));
            Assert.That(result[0].Position, Is.EqualTo(4));
        }

        private static GitDiffEntryFactory GetGitDiffEntryFactory()
        {
            return new GitDiffEntryFactory();
        }
    }
}