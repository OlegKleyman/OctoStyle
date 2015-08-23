namespace OctoStyle.Core.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Moq;

    using NUnit.Framework;

    using Octokit;
    using Octokit.Internal;

    [TestFixture]
    public class PullRequestRetrieverTests
    {
        private const string FirstModifiedFileName = "src/TestLibrary/TestClass.cs";

        private const int PullRequestNumber = 1;

        private const string RenamedFileName = "src/TestLibrary/Nested/TestClass2.cs";

        private const string AddedFileName = "src/TestLibrary/Nested/TestClass3.cs";

        private const string SecondModifiedFileName = "src/TestLibrary/TestLibrary.csproj";

        private const int RenamedChanges = 9;

        private const int AddedChanges = 12;

        private const int FirstModifiedChanges = 8;

        private const int SecondModifiedChanges = 7;

        private const string PullRequestBranch = "test_branch";

        private const string PullRequestMergeBranch = "master";

        private const string LastCommitId = "126";

        private static readonly Uri RenamedContentUrl = new Uri("http://321");

        private static readonly Uri AddedContentUrl = new Uri("http://421");

        private static readonly Uri FirstModifiedContentUrl = new Uri("http://521");

        private static readonly Uri SecondModifiedContentUrl = new Uri("http://621");

        [Test]
        public async void RetrieveShouldReturnPullRequest()
        {
            var retriever = GetPullRequestRetriever();
            var pullRequest = await retriever.RetrieveAsync(PullRequestNumber);

            Assert.That(pullRequest.Number, Is.EqualTo(PullRequestNumber));
            Assert.That(pullRequest.LastCommitId, Is.EqualTo(LastCommitId));
            Assert.That(pullRequest.Branches.Branch, Is.EqualTo(PullRequestBranch));
            Assert.That(pullRequest.Branches.MergeBranch, Is.EqualTo(PullRequestMergeBranch));
            Assert.That(pullRequest.Diff, Is.EqualTo(FileContents.FullDiff));
            Assert.That(pullRequest.Files.Count, Is.EqualTo(4));
            Assert.That(pullRequest.Files[0].FileName, Is.EqualTo(RenamedFileName));
            Assert.That(pullRequest.Files[0].Status, Is.EqualTo(GitHubPullRequestFileStatus.Renamed));
            Assert.That(pullRequest.Files[0].Changes, Is.EqualTo(RenamedChanges));
            Assert.That(pullRequest.Files[0].Diff, Is.EqualTo(FileContents.TestClass2CsDiff));
            Assert.That(pullRequest.Files[1].FileName, Is.EqualTo(AddedFileName));
            Assert.That(pullRequest.Files[1].Status, Is.EqualTo(GitHubPullRequestFileStatus.Added));
            Assert.That(pullRequest.Files[1].Changes, Is.EqualTo(AddedChanges));
            Assert.That(pullRequest.Files[1].Diff, Is.EqualTo(FileContents.TestClass3CsDiff));
            Assert.That(pullRequest.Files[2].FileName, Is.EqualTo(FirstModifiedFileName));
            Assert.That(pullRequest.Files[2].Status, Is.EqualTo(GitHubPullRequestFileStatus.Modified));
            Assert.That(pullRequest.Files[2].Changes, Is.EqualTo(FirstModifiedChanges));
            Assert.That(pullRequest.Files[2].Diff, Is.EqualTo(FileContents.TestClassCsDiff));
            Assert.That(pullRequest.Files[3].FileName, Is.EqualTo(SecondModifiedFileName));
            Assert.That(pullRequest.Files[3].Status, Is.EqualTo(GitHubPullRequestFileStatus.Modified));
            Assert.That(pullRequest.Files[3].Changes, Is.EqualTo(SecondModifiedChanges));
            Assert.That(pullRequest.Files[3].Diff, Is.EqualTo(FileContents.TestLibraryCsprojDiff));
        }

        private static IPullRequestRetriever GetPullRequestRetriever()
        {
            var client = new Mock<IPullRequestsClient>();

            var repository = new GitHubRepository("OlegKleyman", "OctoStyle");

            var commits = new List<PullRequestCommit>
                              {
                                  GetPullRequestCommit("123"),
                                  GetPullRequestCommit("124"),
                                  GetPullRequestCommit("125"),
                                  GetPullRequestCommit(LastCommitId)
                              };
            client.Setup(requestsClient => requestsClient.Commits(repository.Owner, repository.Name, PullRequestNumber))
                .ReturnsAsync(commits);

            var files = new List<PullRequestFile>
                            {
                                GetPullRequestFile(
                                    "321",
                                    RenamedFileName,
                                    "renamed",
                                    RenamedContentUrl,
                                    RenamedChanges),
                                GetPullRequestFile(
                                    "421",
                                    AddedFileName,
                                    "added",
                                    AddedContentUrl,
                                    AddedChanges),
                                GetPullRequestFile(
                                    "521",
                                    FirstModifiedFileName,
                                    "modified",
                                    FirstModifiedContentUrl,
                                    FirstModifiedChanges),
                                GetPullRequestFile(
                                    "621",
                                    SecondModifiedFileName,
                                    "modified",
                                    SecondModifiedContentUrl,
                                    SecondModifiedChanges)
                            };

            client.Setup(requestsClient => requestsClient.Files(repository.Owner, repository.Name, PullRequestNumber))
                .ReturnsAsync(files);

            var diffUrl =
                new Uri(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "http://github.com/{0}/{1}/pull/{2}",
                        repository.Owner,
                        repository.Name,
                        PullRequestNumber));

            client.Setup(requestsClient => requestsClient.Get(repository.Owner, repository.Name, PullRequestNumber))
                .ReturnsAsync(
                    new PullRequest(
                        null,
                        null,
                        diffUrl,
                        null,
                        null,
                        null,
                        PullRequestNumber,
                        ItemState.Open,
                        null,
                        null,
                        default(DateTimeOffset),
                        default(DateTimeOffset),
                        null,
                        null,
                        new GitReference(null, null, PullRequestBranch, null, null, null),
                        new GitReference(null, null, PullRequestMergeBranch, null, null, null),
                        null,
                        null,
                        true,
                        null,
                        null,
                        0,
                        0,
                        0,
                        0,
                        0));

            var connection = new Mock<IConnection>();
            var builder = new Mock<IPullRequestBuilder>();
            var mockResponse = new Mock<IResponse>();

            connection.Setup(
                con => con.Get<string>(It.Is<Uri>(uri => uri.AbsoluteUri == diffUrl.AbsoluteUri), null, null))
                .ReturnsAsync(new ApiResponse<string>(mockResponse.Object, FileContents.FullDiff));

            var pullRequestFiles = new List<GitHubPullRequestFile>
                                       {
                                           new GitHubPullRequestFile(
                                               RenamedFileName,
                                               GitHubPullRequestFileStatus.Renamed,
                                               RenamedChanges,
                                               FileContents.TestClass2CsDiff),
                                           new GitHubPullRequestFile(
                                               AddedFileName,
                                               GitHubPullRequestFileStatus.Added,
                                               AddedChanges,
                                               FileContents.TestClass3CsDiff),
                                           new GitHubPullRequestFile(
                                               FirstModifiedFileName,
                                               GitHubPullRequestFileStatus.Modified,
                                               FirstModifiedChanges,
                                               FileContents.TestClassCsDiff),
                                           new GitHubPullRequestFile(
                                               SecondModifiedFileName,
                                               GitHubPullRequestFileStatus.Modified,
                                               SecondModifiedChanges,
                                               FileContents.TestLibraryCsprojDiff),
                                       };


            var pullRequest = new GitHubPullRequest(
                PullRequestNumber,
                LastCommitId,
                pullRequestFiles,
                FileContents.FullDiff,
                new GitHubPullRequestBranches(PullRequestBranch, PullRequestMergeBranch));

            builder.Setup(
                requestBuilder =>
                requestBuilder.Build(
                    PullRequestNumber,
                    LastCommitId,
                    files,
                    FileContents.FullDiff,
                    It.Is<GitHubPullRequestBranches>(
                        branches =>
                        branches.Branch == PullRequestBranch & branches.MergeBranch == PullRequestMergeBranch))).Returns(pullRequest);

            return new PullRequestRetriever(builder.Object, client.Object, connection.Object, repository);
        }

        private static PullRequestFile GetPullRequestFile(
            string sha,
            string fileName,
            string status,
            Uri contentUrl,
            int changes)
        {
            return new PullRequestFile(sha, fileName, status, 0, 0, changes, null, null, contentUrl, null);
        }

        private static PullRequestCommit GetPullRequestCommit(string sha)
        {
            return new PullRequestCommit(null, null, null, null, null, new List<GitReference>(), sha, null);
        }
    }
}