namespace OctoStyle.Core.Tests.Unit
{
    using System;

    using Moq;

    using NUnit.Framework;

    using Octokit;

    [TestFixture]
    public static class PullRequestCommenterFactoryTests
    {
        [TestCase(GitHubPullRequestFileStatus.Modified, typeof(ModifiedPullRequestCommenter))]
        [TestCase(GitHubPullRequestFileStatus.Added, typeof(AddedPullRequestCommenter))]
        [TestCase(GitHubPullRequestFileStatus.Renamed, typeof(RenamedPullRequestCommenter))]
        public static void GetCommenterShouldReturnPullRequestCommenterObject(GitHubPullRequestFileStatus status, Type expectedType)
        {
            var factory = GetPullRequestCommenterFactory();

            var commenter = factory.GetCommenter(status);

            Assert.That(commenter, Is.InstanceOf(expectedType));
        }

        public static void GetCommenterShouldReturnNoCommentPullRequestCommenterObjectForDeletedFiles()
        {
            var factory = GetPullRequestCommenterFactory();

            var commenter = factory.GetCommenter(GitHubPullRequestFileStatus.Removed);

            Assert.That(commenter, Is.InstanceOf(PullRequestCommenter.NoComment.GetType()));
        }

        private static IPullRequestCommenterFactory GetPullRequestCommenterFactory()
        {
            var commentsClient = new Mock<IPullRequestReviewCommentsClient>();
            var diffRetriever = new Mock<IGitHubDiffRetriever>();

            return new PullRequestCommenterFactory(
                commentsClient.Object,
                new GitHubRepository("OlegKleyman", "OctoStyleTest"),
                diffRetriever.Object);
        }
    }
}