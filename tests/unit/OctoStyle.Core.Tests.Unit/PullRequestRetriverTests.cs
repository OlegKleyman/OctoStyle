namespace OctoStyle.Core.Tests.Unit
{
    using System;
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using Octokit;

    [TestFixture]
    public class PullRequestRetriverTests
    {
        private static readonly string firstModifiedFileName = "src/TestLibrary/TestClass.cs";

        private static readonly Uri renamedContentUrl = new Uri("http://321");

        private static readonly Uri addedContentUrl = new Uri("http://421");

        private static readonly Uri firstModifiedContentUrl = new Uri("http://521");

        private static readonly Uri secondModifiedContentUrl = new Uri("http://621");

        private const int pullRequestNumber = 1;

        private const string renamedFileName = "src/TestLibrary/Nested/TestClass2.cs";

        private const string addedFileName = "src/TestLibrary/Nested/TestClass3.cs";

        private const string secondModifiedFileName = "src/TestLibrary/TestLibrary.csproj";

        private const int renamedChanges = 9;

        private const int addedChanges = 12;

        private const int firstModifiedChanges = 8;

        private const int secondModifiedChanges = 7;

        private const string pullRequestBranch = "test_branch";

        private const string pullRequestMergeBranch = "master";

        private static IPullRequestRetriever GetPullRequestRetriever()
        {
            var client = new Mock<IPullRequestsClient>();

            var repository = new GitRepository("OlegKleyman", "OctoStyle");

            var commits = new List<PullRequestCommit>
                              {
                                  GetPullRequestCommit("123"),
                                  GetPullRequestCommit("124"),
                                  GetPullRequestCommit("125"),
                                  GetPullRequestCommit("126")
                              };
            client.Setup(requestsClient => requestsClient.Commits(repository.Owner, repository.Name, pullRequestNumber))
                .ReturnsAsync(commits);

            var files = new List<PullRequestFile>
                            {
                                GetPullRequestFile(
                                    "321",
                                    renamedFileName,
                                    "renamed",
                                    renamedContentUrl,
                                    renamedChanges),
                                GetPullRequestFile(
                                    "421",
                                    addedFileName,
                                    "added",
                                    addedContentUrl,
                                    addedChanges),
                                GetPullRequestFile(
                                    "521",
                                    firstModifiedFileName,
                                    "modified",
                                    firstModifiedContentUrl,
                                    firstModifiedChanges),
                                GetPullRequestFile(
                                    "621",
                                    secondModifiedFileName,
                                    "modified",
                                    secondModifiedContentUrl,
                                    secondModifiedChanges)
                            };

            client.Setup(requestsClient => requestsClient.Files(repository.Owner, repository.Name, pullRequestNumber))
                .ReturnsAsync(files);

            client.Setup(requestsClient => requestsClient.Get(repository.Owner, repository.Name, pullRequestNumber))
                .ReturnsAsync(
                    new PullRequest(
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        pullRequestNumber,
                        ItemState.Open,
                        null,
                        null,
                        default(DateTimeOffset),
                        default(DateTimeOffset),
                        null,
                        null,
                        new GitReference(null, null, pullRequestBranch, null, null, null),
                        new GitReference(null, null, pullRequestMergeBranch, null, null, null),
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

            return new PullRequestRetriver(client.Object, repository);
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

        [Test]
        public async void RetrieveShouldReturnPullRequest()
        {
            var retriever = GetPullRequestRetriever();
            var pullRequest = await retriever.Retrieve(pullRequestNumber);

            Assert.That(pullRequest.Number, Is.EqualTo(pullRequestNumber));
            Assert.That(pullRequest.LastCommitId, Is.EqualTo("126"));
            Assert.That(pullRequest.Branches.Branch, Is.EqualTo(pullRequestBranch));
            Assert.That(pullRequest.Branches.MergeBranch, Is.EqualTo(pullRequestMergeBranch));
            Assert.That(pullRequest.Files.Count, Is.EqualTo(4));
            Assert.That(pullRequest.Files[0].FileName, Is.EqualTo(renamedFileName));
            Assert.That(pullRequest.Files[0].Status, Is.EqualTo(GitPullRequestFileStatus.Renamed));
            Assert.That(pullRequest.Files[0].Changes, Is.EqualTo(renamedChanges));
            Assert.That(pullRequest.Files[1].FileName, Is.EqualTo(addedFileName));
            Assert.That(pullRequest.Files[1].Status, Is.EqualTo(GitPullRequestFileStatus.Added));
            Assert.That(pullRequest.Files[1].Changes, Is.EqualTo(addedChanges));
            Assert.That(pullRequest.Files[2].FileName, Is.EqualTo(firstModifiedFileName));
            Assert.That(pullRequest.Files[2].Status, Is.EqualTo(GitPullRequestFileStatus.Modified));
            Assert.That(pullRequest.Files[2].Changes, Is.EqualTo(firstModifiedChanges));
            Assert.That(pullRequest.Files[3].FileName, Is.EqualTo(secondModifiedFileName));
            Assert.That(pullRequest.Files[3].Status, Is.EqualTo(GitPullRequestFileStatus.Modified));
            Assert.That(pullRequest.Files[3].Changes, Is.EqualTo(secondModifiedChanges));
        }
    }
}