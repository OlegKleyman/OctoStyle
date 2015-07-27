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
    public class AddedPullRequestCommenterTest
    {
        private const int NoFileHeaderCommentId = 1;

        private const string AddedFilePath = "src/TestLibrary/Nested/TestClass3.cs";

        private const int PullRequestNumber = 1;

        private const string NoFileHeaderMessage =
            "SA1633 - The file has no header, the header Xml is invalid, or the header is not located at the top of the file.";

        private const int NoFileHeaderPosition = 1;

        private const int AllDirectivesInNamespaceCommentId = 2;

        private const int AllDirectivesInNamespacePosition = 1;

        private const string AllDirectivesInNamespaceMessage =
            "SA1200 - All using directives must be placed inside of the namespace.";

        private const string ClassMustHaveDocumentationHeaderMessage =
            "SA1600 - The class must have a documentation header.";

        private const int ClassMustHaveDocumentationHeaderCommentId = 3;

        private const int ClassMustHaveDocumentationHeaderPosition = 9;

        [Test]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1303:ConstFieldNamesMustBeginWithUpperCaseLetter", Justification = StyleCopConstants.LocalConstant)]
        public async void CreateShouldCreateComment()
        {
            var commenter = GetAddedPullRequestCommenter();
            var pullRequestFile = new GitHubPullRequestFile(
                AddedFilePath,
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
                                         "SA1633",
                                         "The file has no header, the header Xml is invalid, or the header is not located at the top of the file.",
                                         NoFileHeaderPosition),
                                     new GitHubStyleViolation(
                                         "SA1200",
                                         "All using directives must be placed inside of the namespace.",
                                         AllDirectivesInNamespacePosition),
                                     new GitHubStyleViolation(
                                         "SA1600",
                                         "The class must have a documentation header.",
                                         ClassMustHaveDocumentationHeaderPosition)
                                 };
            var mockAnalyzer = new Mock<ICodeAnalyzer>();

            const string addedPhysicalFilePath = @"C:\repo\TestLibrary\Nested\TestClass3.cs";

            mockAnalyzer.Setup(analyzer => analyzer.Analyze(addedPhysicalFilePath)).Returns(violations);

            var comments =
                (await commenter.Create(pullRequestFile, mockAnalyzer.Object, addedPhysicalFilePath)).ToList();

            Assert.That(comments.Count, Is.EqualTo(3));

            Assert.That(comments[0].Path, Is.EqualTo(AddedFilePath));
            Assert.That(comments[0].Body, Is.EqualTo(NoFileHeaderMessage));
            Assert.That(comments[0].Id, Is.EqualTo(NoFileHeaderCommentId));
            Assert.That(comments[0].Position, Is.EqualTo(NoFileHeaderPosition));

            Assert.That(comments[1].Path, Is.EqualTo(AddedFilePath));
            Assert.That(comments[1].Body, Is.EqualTo(AllDirectivesInNamespaceMessage));
            Assert.That(comments[1].Id, Is.EqualTo(AllDirectivesInNamespaceCommentId));
            Assert.That(comments[1].Position, Is.EqualTo(AllDirectivesInNamespacePosition));

            Assert.That(comments[2].Path, Is.EqualTo(AddedFilePath));
            Assert.That(comments[2].Body, Is.EqualTo(ClassMustHaveDocumentationHeaderMessage));
            Assert.That(comments[2].Id, Is.EqualTo(ClassMustHaveDocumentationHeaderCommentId));
            Assert.That(comments[2].Position, Is.EqualTo(ClassMustHaveDocumentationHeaderPosition));
        }

        private static AddedPullRequestCommenter GetAddedPullRequestCommenter()
        {
            var pullRequestCommentClient = new Mock<IPullRequestReviewCommentsClient>();

            pullRequestCommentClient.Setup(GetCreateMethodMock(NoFileHeaderMessage, NoFileHeaderPosition))
                .ReturnsAsync(GetComment(NoFileHeaderPosition, NoFileHeaderMessage, NoFileHeaderCommentId));
            pullRequestCommentClient.Setup(
                GetCreateMethodMock(AllDirectivesInNamespaceMessage, AllDirectivesInNamespacePosition))
                .ReturnsAsync(
                    GetComment(
                        AllDirectivesInNamespacePosition,
                        AllDirectivesInNamespaceMessage,
                        AllDirectivesInNamespaceCommentId));
            pullRequestCommentClient.Setup(
                GetCreateMethodMock(ClassMustHaveDocumentationHeaderMessage, ClassMustHaveDocumentationHeaderPosition))
                .ReturnsAsync(
                    GetComment(
                        ClassMustHaveDocumentationHeaderPosition,
                        ClassMustHaveDocumentationHeaderMessage,
                        ClassMustHaveDocumentationHeaderCommentId));

            return new AddedPullRequestCommenter(
                pullRequestCommentClient.Object,
                new GitHubRepository("OlegKleyman", "OctoStyle"));
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
                        create.Path == AddedFilePath && create.Body == body && create.CommitId == "123"
                        && create.Position == position));
        }

        private static PullRequestReviewComment GetComment(int position, string message, int commentId)
        {
            return new PullRequestReviewComment(
                null,
                commentId,
                null,
                AddedFilePath,
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