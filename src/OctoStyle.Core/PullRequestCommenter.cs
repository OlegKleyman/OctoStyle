namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Octokit;

    using StyleCop;

    public abstract class PullRequestCommenter
    {
        private readonly IPullRequestReviewCommentsClient client;

        private readonly GitRepository repository;

        protected PullRequestCommenter(IPullRequestReviewCommentsClient client, GitRepository repository)
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
        }

        public abstract Task<IEnumerable<PullRequestReviewComment>> Create(GitHubPullRequestFile file, IEnumerable<Violation> violations);

        protected async Task<PullRequestReviewComment> Create(PullRequestReviewCommentCreate comment, int pullRequestNumber)
        {
            return await client.Create(repository.Owner, repository.Name, pullRequestNumber, comment);
        }
    }
}