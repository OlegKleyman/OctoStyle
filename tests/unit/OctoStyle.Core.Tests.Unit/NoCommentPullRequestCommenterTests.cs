namespace OctoStyle.Core.Tests.Unit
{
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class NoCommentPullRequestCommenterTests
    {
        [Test]
        public async void CreateShouldReturnEmptyPullRequestComment()
        {
            var commenter = GetNoCommentPullRequestCommenter();
            var comments = await commenter.Create(null, null, null, null);

            Assert.That(comments, Is.Not.Null);
            Assert.That(comments.Any(), Is.False);
        }

        private static PullRequestCommenter GetNoCommentPullRequestCommenter()
        {
            return PullRequestCommenter.NoComment;
        }
    }
}