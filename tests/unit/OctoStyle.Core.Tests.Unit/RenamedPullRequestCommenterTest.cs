namespace OctoStyle.Core.Tests.Unit
{
    using System;

    using Moq;

    using NUnit.Framework;

    using Octokit;

    [TestFixture]
    public class RenamedPullRequestCommenterTest
    {
        [Test]
        public async void CreateShouldCreateComment()
        {
            PullRequestCommenter commenter = GetRenamedPullRequestCommenter();
            var comment = await commenter.Create("src/TestLibrary/Nested/TestClass2.cs", "123", 1);

            Assert.That(comment, Is.Not.Null);
            Assert.That(comment.Path, Is.EqualTo("src/TestLibrary/Nested/TestClass2.cs"));
            Assert.That(comment.Body, Is.EqualTo("Renamed files not supported."));
            Assert.That(comment.Id, Is.EqualTo(1));
            Assert.That(comment.Position, Is.EqualTo(1));
        }

        private RenamedPullRequestCommenter GetRenamedPullRequestCommenter()
        {
            var pullRequestCommentClient = new Mock<IPullRequestReviewCommentsClient>();
            pullRequestCommentClient.Setup(
                client =>
                client.Create(
                    "OlegKleyman",
                    "OctoStyle",
                    1,
                    It.Is<PullRequestReviewCommentCreate>(
                        create =>
                        create.Path == "src/TestLibrary/Nested/TestClass2.cs"
                        && create.Body == "Renamed files not supported." && create.CommitId == "123"
                        && create.Position == 1)))
                .ReturnsAsync(
                    new PullRequestReviewComment(
                        null,
                        1,
                        null,
                        "src/TestLibrary/Nested/TestClass2.cs",
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

            return new RenamedPullRequestCommenter(pullRequestCommentClient.Object, "OlegKleyman", "OctoStyle");
        }
    }
}
