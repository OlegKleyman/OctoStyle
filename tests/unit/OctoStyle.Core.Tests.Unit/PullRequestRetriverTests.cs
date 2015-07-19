namespace OctoStyle.Core.Tests.Unit
{
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using Octokit;

    [TestFixture]
    public class PullRequestRetriverTests
    {
        private static string firstModifiedFileName = "src/TestLibrary/TestClass.cs";

        private const int pullRequestNumber = 1;

        private const string renamedFileName = "src/TestLibrary/Nested/TestClass2.cs";

        private const string addedFileName = "src/TestLibrary/Nested/TestClass3.cs";

        private const string secondModifiedFileName = "src/TestLibrary/TestLibrary.csproj";

        [Test]
        public async void RetrieveShouldReturnPullRequest()
        {
            var retriever = GetPullRequestRetriever();
            var pullRequest = await retriever.Retrieve(pullRequestNumber);

            Assert.That(pullRequest.LastCommitId, Is.EqualTo("126"));
            Assert.That(pullRequest.Files.Count, Is.EqualTo(4));
            Assert.That(pullRequest.Files[0].FileName, Is.EqualTo(renamedFileName));
            Assert.That(pullRequest.Files[0].Status, Is.EqualTo(GitPullRequestFileStatus.Renamed));
            Assert.That(pullRequest.Files[1].FileName, Is.EqualTo(addedFileName));
            Assert.That(pullRequest.Files[1].Status, Is.EqualTo(GitPullRequestFileStatus.Added));
            Assert.That(pullRequest.Files[2].FileName, Is.EqualTo(firstModifiedFileName));
            Assert.That(pullRequest.Files[2].Status, Is.EqualTo(GitPullRequestFileStatus.Modified));
            Assert.That(pullRequest.Files[3].FileName, Is.EqualTo(secondModifiedFileName));
            Assert.That(pullRequest.Files[3].Status, Is.EqualTo(GitPullRequestFileStatus.Modified));
        }

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
            client.Setup(requestsClient => requestsClient.Commits(repository.Owner, repository.Name, pullRequestNumber)).ReturnsAsync(commits);

            var files = new List<PullRequestFile>
                            {
                                GetPullRequestFile("321", renamedFileName, "renamed"),
                                GetPullRequestFile("421", addedFileName, "added"),
                                GetPullRequestFile("521", firstModifiedFileName, "modified"),
                                GetPullRequestFile("621", secondModifiedFileName, "modified")
                            };

            client.Setup(requestsClient => requestsClient.Files(repository.Owner, repository.Name, pullRequestNumber))
                .ReturnsAsync(files);

            return new PullRequestRetriver(client.Object, repository);
        }

        private static PullRequestFile GetPullRequestFile(string sha, string fileName, string status)
        {
            return new PullRequestFile(sha, fileName, status, 0, 0, 0, null, null, null, null);
        }

        private static PullRequestCommit GetPullRequestCommit(string sha)
        {
            return new PullRequestCommit(null, null, null, null, null, new List<GitReference>(), sha, null);
        }
    }
}