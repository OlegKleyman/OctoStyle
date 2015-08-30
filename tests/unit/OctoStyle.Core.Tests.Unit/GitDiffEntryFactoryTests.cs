namespace OctoStyle.Core.Tests.Unit
{
    using System.Linq;

    using NUnit.Framework;

    using SharpDiff;

    [TestFixture]
    public static class GitDiffEntryFactoryTests
    {
        [Test]
        public static void GetShouldReturnGitEqualDiffForContextSnippetLine()
        {
            var diff = Differ.Load(FileContents.TestLibraryCsprojDiff).ToList();

            var factory = GetGitDiffEntryFactory();

            var equalDiff = factory.GetList(diff[0].Chunks[0].Snippets.ToList()[0], 1, 30);

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

            var result = factory.GetList(diff[0].Chunks[0].Snippets.ToList()[1], 4, 33);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.InstanceOf<ModificationGitDiffEntry>());
            
            var modifiedEntry = (ModificationGitDiffEntry)result[0];
            Assert.That(modifiedEntry.LineNumber, Is.EqualTo(33));
            Assert.That(modifiedEntry.Status, Is.EqualTo(GitDiffEntryStatus.New));
            Assert.That(result[0].Position, Is.EqualTo(4));
        }

        [Test]
        public static void GetShouldReturnGitModificationDiffForAdditionAndDeletionSnippet()
        {
            var diff = Differ.Load(FileContents.TestLibraryCsprojDiff).ToList();

            var factory = GetGitDiffEntryFactory();

            var result = factory.GetList(diff[0].Chunks[0].Snippets.ToList()[3], 7, 36);

            Assert.That(result.Count, Is.EqualTo(5));

            Assert.That(result[0], Is.InstanceOf<ModificationGitDiffEntry>());
            
            var removedEntry = (ModificationGitDiffEntry)result[0];
            Assert.That(removedEntry.LineNumber, Is.EqualTo(default(int)));
            Assert.That(removedEntry.Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(result[0].Position, Is.EqualTo(7));

            Assert.That(result[1], Is.InstanceOf<ModificationGitDiffEntry>());

            removedEntry = (ModificationGitDiffEntry)result[1];
            Assert.That(removedEntry.LineNumber, Is.EqualTo(default(int)));
            Assert.That(removedEntry.Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(result[1].Position, Is.EqualTo(8));

            Assert.That(result[2], Is.InstanceOf<ModificationGitDiffEntry>());

            removedEntry = (ModificationGitDiffEntry)result[2];
            Assert.That(removedEntry.LineNumber, Is.EqualTo(default(int)));
            Assert.That(removedEntry.Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(result[2].Position, Is.EqualTo(9));

            Assert.That(result[3], Is.InstanceOf<ModificationGitDiffEntry>());

            removedEntry = (ModificationGitDiffEntry)result[3];
            Assert.That(removedEntry.LineNumber, Is.EqualTo(default(int)));
            Assert.That(removedEntry.Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(result[3].Position, Is.EqualTo(10));

            Assert.That(result[4], Is.InstanceOf<ModificationGitDiffEntry>());
            
            removedEntry = (ModificationGitDiffEntry)result[4];
            Assert.That(removedEntry.LineNumber, Is.EqualTo(36));
            Assert.That(removedEntry.Status, Is.EqualTo(GitDiffEntryStatus.New));
            Assert.That(result[4].Position, Is.EqualTo(11));
        }

        private static GitDiffEntryFactory GetGitDiffEntryFactory()
        {
            return new GitDiffEntryFactory();
        }
    }
}