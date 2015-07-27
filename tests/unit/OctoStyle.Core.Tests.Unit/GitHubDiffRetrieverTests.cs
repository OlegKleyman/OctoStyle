namespace OctoStyle.Core.Tests.Unit
{
    using System;
    using System.Text;

    using Moq;

    using NUnit.Framework;

    using Octokit;
    using Octokit.Internal;

    [TestFixture]
    public class GitHubDiffRetrieverTests
    {
        [Test]
        public async void RetrieveShouldReturnFileContentsByFilePath()
        {
            var retriever = GetGitHubDiffRetriever();

            var diff = await retriever.RetrieveAsync("src/TestLibrary/TestLibrary.csproj", "test_branch", "master");

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
            Assert.That(((ModificationGitDiffEntry)diff[6]).LineNumber, Is.EqualTo(35));
            Assert.That(((ModificationGitDiffEntry)diff[6]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(diff[6].Position, Is.EqualTo(7));
            Assert.That(diff[7], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)diff[7]).LineNumber, Is.EqualTo(36));
            Assert.That(((ModificationGitDiffEntry)diff[7]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(diff[7].Position, Is.EqualTo(8));
            Assert.That(diff[8], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)diff[8]).LineNumber, Is.EqualTo(37));
            Assert.That(((ModificationGitDiffEntry)diff[8]).Status, Is.EqualTo(GitDiffEntryStatus.Removed));
            Assert.That(diff[8].Position, Is.EqualTo(9));
            Assert.That(diff[9], Is.TypeOf<ModificationGitDiffEntry>());
            Assert.That(((ModificationGitDiffEntry)diff[9]).LineNumber, Is.EqualTo(38));
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
            var mockConnection = new Mock<IConnection>();

            var mockResponse = new Mock<IResponse>();

            var newContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(FileContents.TestLibraryCsprojNew));
            var oldContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(FileContents.TestLibraryCsprojOld));

            mockConnection.Setup(
                connection =>
                connection.Get<RepositoryContent>(
                    It.Is<Uri>(
                        uri =>
                        uri.AbsoluteUri
                        == "https://api.github.com/repos/OlegKleyman/OctoStyleTest/contents/src/TestLibrary/TestLibrary.csproj?ref=test_branch"),
                    null,
                    null))
                .ReturnsAsync(new ApiResponse<RepositoryContent>(mockResponse.Object, GetRepositoryContent(newContent)));

            mockConnection.Setup(
                connection =>
                connection.Get<RepositoryContent>(
                    It.Is<Uri>(
                        uri =>
                        uri.AbsoluteUri
                        == "https://api.github.com/repos/OlegKleyman/OctoStyleTest/contents/src/TestLibrary/TestLibrary.csproj?ref=master"),
                    null,
                    null))
                .ReturnsAsync(new ApiResponse<RepositoryContent>(mockResponse.Object, GetRepositoryContent(oldContent)));

            return new GitHubDiffRetriever(mockConnection.Object, new GitHubRepository("OlegKleyman", "OctoStyleTest"));
        }

        private static RepositoryContent GetRepositoryContent(string content)
        {
            return new RepositoryContent(
                null,
                null,
                null,
                0,
                ContentType.File,
                null,
                null,
                null,
                null,
                null,
                content,
                null,
                null);
        }
    }
}