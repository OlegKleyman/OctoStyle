namespace OctoStyle.Core
{
    using System;
    using System.Globalization;

    using Octokit;

    public class PullRequestCommenterFactory : IPullRequestCommenterFactory
    {
        private readonly IPullRequestReviewCommentsClient client;

        private readonly GitRepository repository;

        public IGitHubDiffRetriever DiffRetriever { get; set; }

        public PullRequestCommenterFactory(
            IPullRequestReviewCommentsClient client,
            GitRepository repository,
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

        public PullRequestCommenter Get(GitPullRequestFileStatus status)
        {
            PullRequestCommenter commenter;

            switch (status)
            {
                case GitPullRequestFileStatus.Added:
                    commenter = new AddedPullRequestCommenter(client, repository);
                    break;
                case GitPullRequestFileStatus.Deleted:
                    commenter = PullRequestCommenter.NoCommentPullRequestCommenter.NoComment;
                    break;
                case GitPullRequestFileStatus.Modified:
                    if (DiffRetriever == null)
                    {
                        throw new InvalidOperationException("DiffRetriever is null");
                    }

                    commenter = new ModifiedPullRequestCommenter(client, repository, this.DiffRetriever);
                    break;
                case GitPullRequestFileStatus.Renamed:
                    commenter = new RenamedPullRequestCommenter(client, repository);
                    break;
                default:
                    throw new ArgumentException(
                        String.Format(CultureInfo.InvariantCulture, "Unknown status: {0}", status),
                        "status");
            }

            return commenter;
        }
    }
}