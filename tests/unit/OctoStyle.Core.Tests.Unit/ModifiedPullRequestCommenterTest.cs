namespace OctoStyle.Core.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Moq;

    using NUnit.Framework;

    using Octokit;

    [TestFixture]
    public class ModifiedPullRequestCommenterTest
    {
        private const int noFileHeaderCommentId = 1;
        private const string addedFilePath = "src/TestLibrary/Nested/TestClass3.cs";
        private const int pullRequestNumber = 1;
        private const string noFileHeaderMessage = "SA1633 - The file has no header, the header Xml is invalid, or the header is not located at the top of the file.";
        private const int noFileHeaderPosition = 1;

        private const int allDirectivesInNamespaceCommentId = 2;

        private const int allDirectivesInNamespacePosition = 1;

        private const string allDirectivesInNamespaceMessage = "SA1200 - All using directives must be placed inside of the namespace.";

        private const string classMustHaveDocumentationHeaderMessage = "SA1600 - The class must have a documentation header.";

        private const int classMustHaveDocumentationHeaderCommentId = 3;

        private const int classMustHaveDocumentationHeaderPosition = 9;

        [Test]
        public async void CreateShouldCreateComment()
        {
            var commenter = GetAddedPullRequestCommenter();
            var pullRequestFile = new GitHubPullRequestFile(
                addedFilePath,
                new GitHubPullRequest(1, "123", new List<PullRequestFile>()),
                GitPullRequestFileStatus.Added,
                new Uri("http://localhost"),
                0);

            var violations = new List<GitHubStyleViolation>
                                 {
                                     new GitHubStyleViolation("SA1633", "The file has no header, the header Xml is invalid, or the header is not located at the top of the file.", noFileHeaderPosition),
                                     new GitHubStyleViolation("SA1200", "All using directives must be placed inside of the namespace.", allDirectivesInNamespacePosition),
                                     new GitHubStyleViolation("SA1600", "The class must have a documentation header.", classMustHaveDocumentationHeaderPosition)
                                 };
            var comments = (await commenter.Create(pullRequestFile, violations)).ToList();

            Assert.That(comments.Count, Is.EqualTo(3));

            Assert.That(comments[0].Path, Is.EqualTo(addedFilePath));
            Assert.That(comments[0].Body, Is.EqualTo(noFileHeaderMessage));
            Assert.That(comments[0].Id, Is.EqualTo(noFileHeaderCommentId));
            Assert.That(comments[0].Position, Is.EqualTo(noFileHeaderPosition));

            Assert.That(comments[1].Path, Is.EqualTo(addedFilePath));
            Assert.That(comments[1].Body, Is.EqualTo(allDirectivesInNamespaceMessage));
            Assert.That(comments[1].Id, Is.EqualTo(allDirectivesInNamespaceCommentId));
            Assert.That(comments[1].Position, Is.EqualTo(allDirectivesInNamespacePosition));

            Assert.That(comments[2].Path, Is.EqualTo(addedFilePath));
            Assert.That(comments[2].Body, Is.EqualTo(classMustHaveDocumentationHeaderMessage));
            Assert.That(comments[2].Id, Is.EqualTo(classMustHaveDocumentationHeaderCommentId));
            Assert.That(comments[2].Position, Is.EqualTo(classMustHaveDocumentationHeaderPosition));
        }

        private static ModifiedPullRequestCommenter GetAddedPullRequestCommenter()
        {
            var pullRequestCommentClient = new Mock<IPullRequestReviewCommentsClient>();

            pullRequestCommentClient.Setup(GetCreateMethodMock(noFileHeaderMessage, noFileHeaderPosition))
                .ReturnsAsync(GetComment(noFileHeaderPosition, noFileHeaderMessage, noFileHeaderCommentId));
            pullRequestCommentClient.Setup(
                GetCreateMethodMock(allDirectivesInNamespaceMessage, allDirectivesInNamespacePosition))
                .ReturnsAsync(
                    GetComment(
                        allDirectivesInNamespacePosition,
                        allDirectivesInNamespaceMessage,
                        allDirectivesInNamespaceCommentId));
            pullRequestCommentClient.Setup(
                GetCreateMethodMock(classMustHaveDocumentationHeaderMessage, classMustHaveDocumentationHeaderPosition))
                .ReturnsAsync(
                    GetComment(
                        classMustHaveDocumentationHeaderPosition,
                        classMustHaveDocumentationHeaderMessage,
                        classMustHaveDocumentationHeaderCommentId));

            return new ModifiedPullRequestCommenter(pullRequestCommentClient.Object, new GitRepository("OlegKleyman", "OctoStyle"));
        }

        private static Expression<Func<IPullRequestReviewCommentsClient, Task<PullRequestReviewComment>>>
            GetCreateMethodMock(string body, int position)
        {
            return
                client =>
                client.Create(
                    "OlegKleyman",
                    "OctoStyle",
                    pullRequestNumber,
                    It.Is<PullRequestReviewCommentCreate>(
                        create =>
                        create.Path == addedFilePath && create.Body == body && create.CommitId == "123"
                        && create.Position == position));
        }

        private static PullRequestReviewComment GetComment(int position, string message, int commentId)
        {
            return new PullRequestReviewComment(
                null,
                commentId,
                null,
                addedFilePath,
                position,
                null,
                "1",
                null,
                null,
                message,
                default(DateTimeOffset),
                default(DateTimeOffset),
                null,
                null);
        }
    }
}
