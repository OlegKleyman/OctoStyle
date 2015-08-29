namespace OctoStyle.Core.Tests.Unit
{
    using System;

    using Moq;

    using NUnit.Framework;

    using SharpDiff;

    [TestFixture]
    public static class GitHubDiffRetrieverTests
    {
        [Test]
        public static void RetrieveShouldRetrieveGitDiff()
        {
            var retriever = GetGitHubDiffRetriever();

            var diff = retriever.Retrieve(FileContents.TestLibraryCsprojDiff);
            Assert.That(diff.Count, Is.EqualTo(16));
            Assert.That(diff[0], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(diff[0].Position, Is.EqualTo(1));
            Assert.That(diff[1], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(diff[1].Position, Is.EqualTo(2));
            Assert.That(diff[2], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(diff[2].Position, Is.EqualTo(3));
            Assert.That(diff[3], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)diff[3]).LineNumber, Is.EqualTo(33));
            Assert.That(((ModificationGitDiffEntry)diff[3]).Status, Is.EqualTo(GitDiffEntryStatus.New));
            Assert.That(diff[3].Position, Is.EqualTo(4));
            Assert.That(diff[4], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(diff[4].Position, Is.EqualTo(5));
            Assert.That(diff[5], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(diff[5].Position, Is.EqualTo(6));
            Assert.That(diff[6], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)diff[6]).LineNumber, Is.EqualTo(default(int)));
            Assert.That(((ModificationGitDiffEntry)diff[6]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(diff[6].Position, Is.EqualTo(7));
            Assert.That(diff[7], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)diff[7]).LineNumber, Is.EqualTo(default(int)));
            Assert.That(((ModificationGitDiffEntry)diff[7]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(diff[7].Position, Is.EqualTo(8));
            Assert.That(diff[8], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)diff[8]).LineNumber, Is.EqualTo(default(int)));
            Assert.That(((ModificationGitDiffEntry)diff[8]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(diff[8].Position, Is.EqualTo(9));
            Assert.That(diff[9], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)diff[9]).LineNumber, Is.EqualTo(default(int)));
            Assert.That(((ModificationGitDiffEntry)diff[9]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(diff[9].Position, Is.EqualTo(10));
            Assert.That(diff[10], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)diff[10]).LineNumber, Is.EqualTo(36));
            Assert.That(((ModificationGitDiffEntry)diff[10]).Status, Is.EqualTo(GitDiffEntryStatus.New));
            Assert.That(diff[10].Position, Is.EqualTo(11));
            Assert.That(diff[11], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(diff[11].Position, Is.EqualTo(12));
            Assert.That(diff[12], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)diff[12]).LineNumber, Is.EqualTo(38));
            Assert.That(((ModificationGitDiffEntry)diff[12]).Status, Is.EqualTo(GitDiffEntryStatus.New));
            Assert.That(diff[12].Position, Is.EqualTo(13));
            Assert.That(diff[13], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(diff[13].Position, Is.EqualTo(14));
            Assert.That(diff[14], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(diff[14].Position, Is.EqualTo(15));
            Assert.That(diff[15], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(diff[15].Position, Is.EqualTo(16));
        }

        private static IGitHubDiffRetriever GetGitHubDiffRetriever()
        {
            var mockDiffer = new Mock<IDiffer>();
            mockDiffer.Setup(differ => differ.Load(FileContents.TestLibraryCsprojDiff))
                .Returns(Differ.Load(String.Concat(FileContents.TestLibraryCsprojDiff, "\n")));

            return new GitHubDiffRetriever(mockDiffer.Object);
        }
    }
}