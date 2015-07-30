namespace OctoStyle.Core.Tests.Unit
{
    using System;

    using Moq;

    using NUnit.Framework;

    using Octokit;

    [TestFixture]
    public class PullRequestCommenterFactoryTests
    {
        [TestCase(GitHubPullRequestFileStatus.Modified, typeof(ModifiedPullRequestCommenter))]
        [TestCase(GitHubPullRequestFileStatus.Added, typeof(AddedPullRequestCommenter))]
        [TestCase(GitHubPullRequestFileStatus.Renamed, typeof(RenamedPullRequestCommenter))]
        public void GetShouldReturnPullRequestCommenterObject(GitHubPullRequestFileStatus status, Type expectedType)
        {
            var factory = GetPullRequestCommenterFactory();

            var commenter = factory.GetCommenter(status);

            Assert.That(commenter, Is.InstanceOf(expectedType));
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