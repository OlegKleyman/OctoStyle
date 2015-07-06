namespace OctoStyle.Core.Tests.Unit
{
    using NUnit.Framework;

    using OctoStyle.Core.Borrowed;

    [TestFixture]
    public class DiffEntryListExtensionsTests
    {
        [Test]
        public void ToGitDiffShouldReturnGitDiffForSingleChunkDiff()
        {
            var diff = Diff.CreateDiff(
                FileContents.TestLibraryCsprojOld.Split('\n'),
                FileContents.TestLibraryCsprojNew.Split('\n'));

            var gitDiff = diff.ToGitDiff();

            Assert.That(gitDiff.Count, Is.EqualTo(16));
            Assert.That(gitDiff[0], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[0].Position, Is.EqualTo(0));
            Assert.That(gitDiff[1], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[1].Position, Is.EqualTo(1));
            Assert.That(gitDiff[2], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[2].Position, Is.EqualTo(2));
            Assert.That(gitDiff[3], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)gitDiff[3]).LineNumber, Is.EqualTo(33));
            Assert.That(((ModificationGitDiffEntry)gitDiff[3]).Status, Is.EqualTo(GitDiffEntryStatus.New));
            Assert.That(gitDiff[3].Position, Is.EqualTo(3));
            Assert.That(gitDiff[4], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[4].Position, Is.EqualTo(4));
            Assert.That(gitDiff[5], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[5].Position, Is.EqualTo(5));
            Assert.That(gitDiff[6], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)gitDiff[6]).LineNumber, Is.EqualTo(35));
            Assert.That(((ModificationGitDiffEntry)gitDiff[6]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(gitDiff[6].Position, Is.EqualTo(6));
            Assert.That(gitDiff[7], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)gitDiff[7]).LineNumber, Is.EqualTo(36));
            Assert.That(((ModificationGitDiffEntry)gitDiff[7]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(gitDiff[7].Position, Is.EqualTo(7));
            Assert.That(gitDiff[8], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)gitDiff[8]).LineNumber, Is.EqualTo(37));
            Assert.That(((ModificationGitDiffEntry)gitDiff[8]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(gitDiff[8].Position, Is.EqualTo(8));
            Assert.That(gitDiff[9], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)gitDiff[9]).LineNumber, Is.EqualTo(38));
            Assert.That(((ModificationGitDiffEntry)gitDiff[9]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(gitDiff[9].Position, Is.EqualTo(9));
            Assert.That(gitDiff[10], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)gitDiff[10]).LineNumber, Is.EqualTo(36));
            Assert.That(((ModificationGitDiffEntry)gitDiff[10]).Status, Is.EqualTo(GitDiffEntryStatus.New));
            Assert.That(gitDiff[10].Position, Is.EqualTo(10));
            Assert.That(gitDiff[11], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[11].Position, Is.EqualTo(11));
            Assert.That(gitDiff[12], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)gitDiff[12]).LineNumber, Is.EqualTo(38));
            Assert.That(((ModificationGitDiffEntry)gitDiff[12]).Status, Is.EqualTo(GitDiffEntryStatus.New));
            Assert.That(gitDiff[12].Position, Is.EqualTo(12));
            Assert.That(gitDiff[13], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[13].Position, Is.EqualTo(13));
            Assert.That(gitDiff[14], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[14].Position, Is.EqualTo(14));
            Assert.That(gitDiff[15], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[15].Position, Is.EqualTo(15));
        }

        [Test]
        public void ToGitDiffShouldReturnGitDiffForMultiChunkDiff()
        {
            var diff = Diff.CreateDiff(
                FileContents.TestClass2CsOld.Split('\n'),
                FileContents.TestClass2CsNew.Split('\n'));

            var gitDiff = diff.ToGitDiff();

            Assert.That(gitDiff.Count, Is.EqualTo(18));
            Assert.That(gitDiff[0], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)gitDiff[0]).LineNumber, Is.EqualTo(1));
            Assert.That(((ModificationGitDiffEntry)gitDiff[0]).Status, Is.EqualTo(GitDiffEntryStatus.New));
            Assert.That(gitDiff[0].Position, Is.EqualTo(0));
            Assert.That(gitDiff[1], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[1].Position, Is.EqualTo(1));
            Assert.That(gitDiff[2], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[2].Position, Is.EqualTo(2));
            Assert.That(gitDiff[3], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[3].Position, Is.EqualTo(3));
            Assert.That(gitDiff[4], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[4].Position, Is.EqualTo(4));
            Assert.That(gitDiff[5], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[5].Position, Is.EqualTo(5));
            Assert.That(gitDiff[6], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[6].Position, Is.EqualTo(6));
            Assert.That(gitDiff[7], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[7].Position, Is.EqualTo(7));
            Assert.That(gitDiff[8], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)gitDiff[8]).LineNumber, Is.EqualTo(8));
            Assert.That(((ModificationGitDiffEntry)gitDiff[8]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(gitDiff[8].Position, Is.EqualTo(8));
            Assert.That(gitDiff[9], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)gitDiff[9]).LineNumber, Is.EqualTo(9));
            Assert.That(((ModificationGitDiffEntry)gitDiff[9]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(gitDiff[9].Position, Is.EqualTo(9));
            Assert.That(gitDiff[10], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)gitDiff[10]).LineNumber, Is.EqualTo(10));
            Assert.That(((ModificationGitDiffEntry)gitDiff[10]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(gitDiff[10].Position, Is.EqualTo(10));
            Assert.That(gitDiff[11], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)gitDiff[11]).LineNumber, Is.EqualTo(11));
            Assert.That(((ModificationGitDiffEntry)gitDiff[11]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(gitDiff[11].Position, Is.EqualTo(11));
            Assert.That(gitDiff[12], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[12].Position, Is.EqualTo(12));
            Assert.That(gitDiff[13], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)gitDiff[13]).LineNumber, Is.EqualTo(13));
            Assert.That(((ModificationGitDiffEntry)gitDiff[13]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(gitDiff[13].Position, Is.EqualTo(13));
            Assert.That(gitDiff[14], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)gitDiff[14]).LineNumber, Is.EqualTo(10));
            Assert.That(((ModificationGitDiffEntry)gitDiff[14]).Status, Is.EqualTo(GitDiffEntryStatus.New));
            Assert.That(gitDiff[14].Position, Is.EqualTo(14));
            Assert.That(gitDiff[15], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[15].Position, Is.EqualTo(15));
            Assert.That(gitDiff[16], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[16].Position, Is.EqualTo(16));
            Assert.That(gitDiff[17], Is.TypeOf<EqualGitDiffEntry>());
            Assert.That(gitDiff[17].Position, Is.EqualTo(17));
        }
    }
}