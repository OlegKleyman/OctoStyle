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
        private const int noMethodDocumentationHeaderCommentId = 1;

        private const string modifiedFilePath = "src/TestLibrary/TestClass.cs";

        private const int pullRequestNumber = 1;

        private const string noMethodDocumentationHeaderMessage =
            "SA1600 - The method must have a documentation header.";

        private const int noMethodDocumentationHeaderPosition = 5;

        private const int mustBeFollowedByBlankLineCommentId = 2;

        private const int mustBeFollowedByBlankLinePosition = 9;

        private const string mustBeFollowedByBlankLineMessage =
            "SA1513 - Statements or elements wrapped in curly brackets must be followed by a blank line.";

        [Test]
        public async void CreateShouldCreateComment()
        {
            var commenter = GetAddedPullRequestCommenter();
            var pullRequestFile = new GitHubPullRequestFile(
                modifiedFilePath,
                new GitHubPullRequest(
                    1,
                    "123",
                    new List<PullRequestFile>(),
                    new GitHubPullRequestBranches("test_branch", "master")),
                GitPullRequestFileStatus.Added,
                0);

            var violations = new List<GitHubStyleViolation>
                                 {
                                     new GitHubStyleViolation(
                                         "SA1600",
                                         "The method must have a documentation header.",
                                         21),
                                     new GitHubStyleViolation(
                                         "SA1513",
                                         "Statements or elements wrapped in curly brackets must be followed by a blank line.",
                                         25)
                                 };
            var mockAnalyzer = new Mock<ICodeAnalyzer>();

            const string modifiedPhysicalFilePath = @"C:\repo\TestLibrary\TestClass.cs";
            mockAnalyzer.Setup(analyzer => analyzer.Analyze(modifiedPhysicalFilePath)).Returns(violations);

            var comments =
                (await commenter.Create(pullRequestFile, mockAnalyzer.Object, modifiedPhysicalFilePath)).ToList();

            Assert.That(comments.Count, Is.EqualTo(2));

            Assert.That(comments[0].Path, Is.EqualTo(modifiedFilePath));
            Assert.That(comments[0].Body, Is.EqualTo(noMethodDocumentationHeaderMessage));
            Assert.That(comments[0].Id, Is.EqualTo(noMethodDocumentationHeaderCommentId));
            Assert.That(comments[0].Position, Is.EqualTo(noMethodDocumentationHeaderPosition));

            Assert.That(comments[1].Path, Is.EqualTo(modifiedFilePath));
            Assert.That(comments[1].Body, Is.EqualTo(mustBeFollowedByBlankLineMessage));
            Assert.That(comments[1].Id, Is.EqualTo(mustBeFollowedByBlankLineCommentId));
            Assert.That(comments[1].Position, Is.EqualTo(mustBeFollowedByBlankLinePosition));
        }

        private static ModifiedPullRequestCommenter GetAddedPullRequestCommenter()
        {
            var pullRequestCommentClient = new Mock<IPullRequestReviewCommentsClient>();

            pullRequestCommentClient.Setup(
                GetCreateMethodMock(noMethodDocumentationHeaderMessage, noMethodDocumentationHeaderPosition))
                .ReturnsAsync(
                    GetComment(
                        noMethodDocumentationHeaderPosition,
                        noMethodDocumentationHeaderMessage,
                        noMethodDocumentationHeaderCommentId));
            pullRequestCommentClient.Setup(
                GetCreateMethodMock(mustBeFollowedByBlankLineMessage, mustBeFollowedByBlankLinePosition))
                .ReturnsAsync(
                    GetComment(
                        mustBeFollowedByBlankLinePosition,
                        mustBeFollowedByBlankLineMessage,
                        mustBeFollowedByBlankLineCommentId));

            var diffRetriever = new Mock<IGitHubDiffRetriever>();
            var diff = new List<GitDiffEntry>
                           {
                               new ModificationGitDiffEntry(4, GitDiffEntryStatus.New,  20),
                               new ModificationGitDiffEntry(5, GitDiffEntryStatus.New,  21),
                               new ModificationGitDiffEntry(6, GitDiffEntryStatus.New,  22),
                               new ModificationGitDiffEntry(7, GitDiffEntryStatus.New,  23),
                               new ModificationGitDiffEntry(8, GitDiffEntryStatus.New,  24),
                               new ModificationGitDiffEntry(9, GitDiffEntryStatus.New,  25),
                               new ModificationGitDiffEntry(10, GitDiffEntryStatus.New,  26),
                               new ModificationGitDiffEntry(11, GitDiffEntryStatus.New,  27)
                           };

            diffRetriever.Setup(retriever => retriever.RetrieveAsync(modifiedFilePath, "test_branch", "master"))
                .ReturnsAsync(diff);

            return new ModifiedPullRequestCommenter(
                pullRequestCommentClient.Object,
                new GitRepository("OlegKleyman", "OctoStyle"),
                diffRetriever.Object);
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
                        create.Path == modifiedFilePath && create.Body == body && create.CommitId == "123"
                        && create.Position == position));
        }

        private static PullRequestReviewComment GetComment(int position, string message, int commentId)
        {
            return new PullRequestReviewComment(
                null,
                commentId,
                null,
                modifiedFilePath,
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
