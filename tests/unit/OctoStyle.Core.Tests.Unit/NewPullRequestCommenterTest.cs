namespace OctoStyle.Core.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using Octokit;

    [TestFixture]
    public class NewPullRequestCommenterTest
    {
        private const string addedFilePath = "src/TestLibrary/Nested/TestClass3.cs";
        private const string addedCommitId = "123";
        private const int pullRequestNumber = 1;

        [Test]
        public async void CreateShouldCreateComment()
        {
            var commenter = GetAddedPullRequestCommenter();
            var pullRequestFile = new GitHubPullRequestFile(
                "src/TestLibrary/Nested/TestClass2.cs",
                new GitHubPullRequest(1, "123"));

            var violations = new List<GitHubStyleViolation>
                                 {
                                     new GitHubStyleViolation("SA1633", "The file has no header, the header Xml is invalid, or the header is not located at the top of the file.", 1),
                                     new GitHubStyleViolation("SA1200", "All using directives must be placed inside of the namespace.", 1),
                                     new GitHubStyleViolation("SA1600", "The class must have a documentation header.", 9)
                                 };
            var comments = (await commenter.Create(pullRequestFile, violations)).ToList();

            Assert.That(comments.Count, Is.EqualTo(3));

            Assert.That(comments[0].Path, Is.EqualTo(addedFilePath));
            Assert.That(comments[0].Body, Is.EqualTo("SA1633 - The file has no header, the header Xml is invalid, or the header is not located at the top of the file."));
            Assert.That(comments[0].Id, Is.EqualTo(1));
            Assert.That(comments[0].Position, Is.EqualTo(1));

            Assert.That(comments[1].Path, Is.EqualTo(addedFilePath));
            Assert.That(comments[1].Body, Is.EqualTo("SA1200 - All using directives must be placed inside of the namespace."));
            Assert.That(comments[1].Id, Is.EqualTo(2));
            Assert.That(comments[1].Position, Is.EqualTo(1));

            Assert.That(comments[2].Path, Is.EqualTo(addedFilePath));
            Assert.That(comments[2].Body, Is.EqualTo("SA1600 - The class must have a documentation header."));
            Assert.That(comments[2].Id, Is.EqualTo(3));
            Assert.That(comments[2].Position, Is.EqualTo(9));
        }

        private static AddedPullRequestCommenter GetAddedPullRequestCommenter()
        {
            var pullRequestCommentClient = new Mock<IPullRequestReviewCommentsClient>();
            pullRequestCommentClient.Setup(
                client =>
                client.Create(
                    "OlegKleyman",
                    "OctoStyle",
                    pullRequestNumber,
                    It.Is<PullRequestReviewCommentCreate>(
                        create =>
                        create.Path == addedFilePath
                        && create.Body == "Renamed files not supported." && create.CommitId == addedCommitId
                        && create.Position == 1)))
                .ReturnsAsync(
                    new PullRequestReviewComment(
                        null,
                        1,
                        null,
                        addedFilePath,
                        1,
                        null,
                        "1",
                        null,
                        null,
                        "Renamed files not supported.",
                        default(DateTimeOffset),
                        default(DateTimeOffset),
                        null,
                        null));

            return new AddedPullRequestCommenter(pullRequestCommentClient.Object, new GitRepository("OlegKleyman", "OctoStyle"));
        }
    }
}
