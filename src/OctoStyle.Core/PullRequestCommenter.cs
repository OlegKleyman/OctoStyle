namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Octokit;

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

        public abstract Task<IEnumerable<PullRequestReviewComment>> Create(GitHubPullRequestFile file);

        protected async Task<PullRequestReviewComment> Create(PullRequestReviewCommentCreate comment, int pullRequestNumber)
        {
            return await client.Create(repository.Owner, repository.Name, pullRequestNumber, comment);
        }
    }
}