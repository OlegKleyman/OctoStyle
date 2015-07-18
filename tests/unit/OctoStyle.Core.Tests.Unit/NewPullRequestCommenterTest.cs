namespace OctoStyle.Core.Tests.Unit
{
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
            PullRequestCommenter commenter = GetAddedPullRequestCommenter();
            var comment = (await commenter.Create(addedFilePath, addedCommitId, pullRequestNumber)).ToList();

            Assert.That(comment.Count, Is.EqualTo(3));

            Assert.That(comment[0].Path, Is.EqualTo(addedFilePath));
            Assert.That(comment[0].Body, Is.EqualTo("SA1633 - The file has no header, the header Xml is invalid, or the header is not located at the top of the file."));
            Assert.That(comment[0].Id, Is.EqualTo(1));
            Assert.That(comment[0].Position, Is.EqualTo(1));

            Assert.That(comment[1].Path, Is.EqualTo(addedFilePath));
            Assert.That(comment[1].Body, Is.EqualTo("SA1200 - All using directives must be placed inside of the namespace."));
            Assert.That(comment[1].Id, Is.EqualTo(2));
            Assert.That(comment[1].Position, Is.EqualTo(1));

            Assert.That(comment[2].Path, Is.EqualTo(addedFilePath));
            Assert.That(comment[2].Body, Is.EqualTo("SA1600 - The class must have a documentation header."));
            Assert.That(comment[2].Id, Is.EqualTo(3));
            Assert.That(comment[2].Position, Is.EqualTo(9));
        }

        private static AddedPullRequestCommenter GetAddedPullRequestCommenter()
        {
            var pullRequestCommentClient = new Mock<IPullRequestReviewCommentsClient>();

            return new AddedPullRequestCommenter(pullRequestCommentClient.Object, new GitRepository("OlegKleyman", "OctoStyle"));
        }
    }
}
