namespace OctoStyle.Core.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Moq;

    using NUnit.Framework;

    using Octokit;

    [TestFixture]
    public class ModifiedPullRequestCommenterTest
    {
        private const int NoMethodDocumentationHeaderCommentId = 1;

        private const string ModifiedFilePath = "src/TestLibrary/TestClass.cs";

        private const int PullRequestNumber = 1;

        private const string NoMethodDocumentationHeaderMessage =
            "SA1600 - The method must have a documentation header.";

        private const int NoMethodDocumentationHeaderPosition = 5;

        private const int MustBeFollowedByBlankLineCommentId = 2;

        private const int MustBeFollowedByBlankLinePosition = 9;

        private const string MustBeFollowedByBlankLineMessage =
            "SA1513 - Statements or elements wrapped in curly brackets must be followed by a blank line.";

        [Test]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1303:ConstFieldNamesMustBeginWithUpperCaseLetter", Justification = StyleCopConstants.LocalConstantJustification)]
        public async void CreateShouldCreateComment()
        {
            var commenter = GetAddedPullRequestCommenter();
            var pullRequestFile = new GitHubPullRequestFile(
                ModifiedFilePath,
                GitHubPullRequestFileStatus.Added,
                0,
                FileContents.TestClassCsDiff);

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
                (await
                 commenter.Create(
                     new GitHubPullRequest(
                     1,
                     "123",
                     new List<GitHubPullRequestFile>(),
                     FileContents.TestClassCsDiff,
                     new GitHubPullRequestBranches("test_branch", "master")),
                     pullRequestFile,
                     mockAnalyzer.Object,
                     modifiedPhysicalFilePath)).ToList();

            Assert.That(comments.Count, Is.EqualTo(2));

            Assert.That(comments[0].Path, Is.EqualTo(ModifiedFilePath));
            Assert.That(comments[0].Body, Is.EqualTo(NoMethodDocumentationHeaderMessage));
            Assert.That(comments[0].Id, Is.EqualTo(NoMethodDocumentationHeaderCommentId));
            Assert.That(comments[0].Position, Is.EqualTo(NoMethodDocumentationHeaderPosition));

            Assert.That(comments[1].Path, Is.EqualTo(ModifiedFilePath));
            Assert.That(comments[1].Body, Is.EqualTo(MustBeFollowedByBlankLineMessage));
            Assert.That(comments[1].Id, Is.EqualTo(MustBeFollowedByBlankLineCommentId));
            Assert.That(comments[1].Position, Is.EqualTo(MustBeFollowedByBlankLinePosition));
        }

        private static ModifiedPullRequestCommenter GetAddedPullRequestCommenter()
        {
            var pullRequestCommentClient = new Mock<IPullRequestReviewCommentsClient>();

            pullRequestCommentClient.Setup(
                GetCreateMethodMock(NoMethodDocumentationHeaderMessage, NoMethodDocumentationHeaderPosition))
                .ReturnsAsync(
                    GetComment(
                        NoMethodDocumentationHeaderPosition,
                        NoMethodDocumentationHeaderMessage,
                        NoMethodDocumentationHeaderCommentId));
            pullRequestCommentClient.Setup(
                GetCreateMethodMock(MustBeFollowedByBlankLineMessage, MustBeFollowedByBlankLinePosition))
                .ReturnsAsync(
                    GetComment(
                        MustBeFollowedByBlankLinePosition,
                        MustBeFollowedByBlankLineMessage,
                        MustBeFollowedByBlankLineCommentId));

            var diffRetriever = new Mock<IGitHubDiffRetriever>();
            var diff = new List<GitDiffEntry>
                           {
                               new ModificationGitDiffEntry(4, GitDiffEntryStatus.New, 20),
                               new ModificationGitDiffEntry(5, GitDiffEntryStatus.New, 21),
                               new ModificationGitDiffEntry(6, GitDiffEntryStatus.New, 22),
                               new ModificationGitDiffEntry(7, GitDiffEntryStatus.New, 23),
                               new ModificationGitDiffEntry(8, GitDiffEntryStatus.New, 24),
                               new ModificationGitDiffEntry(9, GitDiffEntryStatus.New, 25),
                               new ModificationGitDiffEntry(10, GitDiffEntryStatus.New, 26),
                               new ModificationGitDiffEntry(11, GitDiffEntryStatus.New, 27)
                           };

            diffRetriever.Setup(retriever => retriever.Retrieve(FileContents.TestClassCsDiff))
                .Returns(diff);

            return new ModifiedPullRequestCommenter(
                pullRequestCommentClient.Object,
                new GitHubRepository("OlegKleyman", "OctoStyle"),
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
                    PullRequestNumber,
                    It.Is<PullRequestReviewCommentCreate>(
                        create =>
                        create.Path == ModifiedFilePath && create.Body == body && create.CommitId == "123"
                        && create.Position == position));
        }

        private static PullRequestReviewComment GetComment(int position, string message, int commentId)
        {
            return new PullRequestReviewComment(
                null,
                commentId,
                null,
                ModifiedFilePath,
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