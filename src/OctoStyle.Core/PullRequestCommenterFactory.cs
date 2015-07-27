namespace OctoStyle.Core
{
    using System;
    using System.Globalization;

    using Octokit;

    public class PullRequestCommenterFactory : IPullRequestCommenterFactory
    {
        private readonly IPullRequestReviewCommentsClient client;

        private readonly GitHubRepository repository;

        public PullRequestCommenterFactory(
            IPullRequestReviewCommentsClient client,
            GitHubRepository repository,
            IGitHubDiffRetriever diffRetriever)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.client = client;
            this.repository = repository;

            this.DiffRetriever = diffRetriever;
        }

        public IGitHubDiffRetriever DiffRetriever { get; set; }

        public PullRequestCommenter Get(GitPullRequestFileStatus status)
        {
            PullRequestCommenter commenter;

            switch (status)
            {
                case GitPullRequestFileStatus.Added:
                    commenter = new AddedPullRequestCommenter(this.client, this.repository);
                    break;
                case GitPullRequestFileStatus.Deleted:
                    commenter = PullRequestCommenter.NoCommentPullRequestCommenter.NoComment;
                    break;
                case GitPullRequestFileStatus.Modified:
                    if (this.DiffRetriever == null)
                    {
                        throw new InvalidOperationException("DiffRetriever is null");
                    }

                    commenter = new ModifiedPullRequestCommenter(this.client, this.repository, this.DiffRetriever);
                    break;
                case GitPullRequestFileStatus.Renamed:
                    commenter = new RenamedPullRequestCommenter(this.client, this.repository);
                    break;
                default:
                    throw new ArgumentException(
                        string.Format(CultureInfo.InvariantCulture, "Unknown status: {0}", status),
                        "status");
            }

            return commenter;
        }
    }
}