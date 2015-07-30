namespace OctoStyle.Core
{
    using System;
    using System.Globalization;

    using Octokit;

    /// <summary>
    /// Represents a factory for <see cref="PullRequestCommenter"/>.
    /// </summary>
    public class PullRequestCommenterFactory : IPullRequestCommenterFactory
    {
        private readonly IPullRequestReviewCommentsClient client;

        private readonly GitHubRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PullRequestCommenterFactory"/> class.
        /// </summary>
        /// <param name="client">The <see cref="IPullRequestReviewCommentsClient"/> to use for interfacing with github.</param>
        /// <param name="repository">The <see cref="GitHubRepository"/> the pull requests are in.</param>
        /// <param name="diffRetriever">The <see cref="IGitHubDiffRetriever"/> to used to retrieve diffs.</param>
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

        /// <summary>
        /// Gets or sets <see cref="DiffRetriever"/>
        /// </summary>
        /// <value>The <see cref="IGitHubDiffRetriever"/> to used to retrieve diffs.</value>
        public IGitHubDiffRetriever DiffRetriever { get; set; }

        /// <summary>
        /// Gets a <see cref="PullRequestCommenter"/>.
        /// </summary>
        /// <param name="status">The target <see cref="GitHubPullRequestFileStatus"/> of file to comment on.</param>
        /// <returns>A <see cref="PullRequestCommenter"/>.</returns>
        public PullRequestCommenter GetCommenter(GitHubPullRequestFileStatus status)
        {
            PullRequestCommenter commenter;

            switch (status)
            {
                case GitHubPullRequestFileStatus.Added:
                    commenter = new AddedPullRequestCommenter(this.client, this.repository);
                    break;
                case GitHubPullRequestFileStatus.Deleted:
                    commenter = PullRequestCommenter.NoCommentPullRequestCommenter.NoComment;
                    break;
                case GitHubPullRequestFileStatus.Modified:
                    if (this.DiffRetriever == null)
                    {
                        throw new InvalidOperationException("DiffRetriever is null");
                    }

                    commenter = new ModifiedPullRequestCommenter(this.client, this.repository, this.DiffRetriever);
                    break;
                case GitHubPullRequestFileStatus.Renamed:
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