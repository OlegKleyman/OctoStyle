namespace OctoStyle.Core.Tests.Unit
{
    using System;

    using Moq;

    using NUnit.Framework;

    using Octokit;

    [TestFixture]
    public class PullRequestCommenterFactoryTests
    {
        [TestCase(GitPullRequestFileStatus.Modified, typeof(ModifiedPullRequestCommenter))]
        [TestCase(GitPullRequestFileStatus.Deleted, typeof(PullRequestCommenter.NoCommentPullRequestCommenter))]
        [TestCase(GitPullRequestFileStatus.Added, typeof(AddedPullRequestCommenter))]
        [TestCase(GitPullRequestFileStatus.Renamed, typeof(RenamedPullRequestCommenter))]
        public void GetShouldReturnPullRequestCommenterObject(GitPullRequestFileStatus status, Type expectedType)
        {
            var factory = GetPullRequestCommenterFactory();

            var commenter = factory.Get(status);

            Assert.That(commenter, Is.InstanceOf(expectedType));
        }

        private static IPullRequestCommenterFactory GetPullRequestCommenterFactory()
        {
            var commentsClient = new Mock<IPullRequestReviewCommentsClient>();
            var diffRetriever = new Mock<IGitHubDiffRetriever>();

            return new PullRequestCommenterFactory(commentsClient.Object, new GitRepository("OlegKleyman", "OctoStyleTest"), diffRetriever.Object);
        }
    }
}
