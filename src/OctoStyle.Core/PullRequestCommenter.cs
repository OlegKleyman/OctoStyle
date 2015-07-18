namespace OctoStyle.Core
{
    using System;
    using System.Threading.Tasks;

    using Octokit;

    public abstract class PullRequestCommenter
    {
        private readonly IPullRequestReviewCommentsClient client;

        private readonly string repositoryOwner;

        private readonly string repositoryName;

        protected PullRequestCommenter(IPullRequestReviewCommentsClient client, string repositoryOwner, string repositoryName)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            if (repositoryOwner == null)
            {
                throw new ArgumentNullException("repositoryOwner");
            }

            if (repositoryName == null)
            {
                throw new ArgumentNullException("repositoryName");
            }

            const string cannotBeEmptyMessage = "Cannot be empty.";

            if (repositoryOwner.Length == 0)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "repositoryOwner");
            }

            if (repositoryName.Length == 0)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "repositoryName");
            }

            this.client = client;
            this.repositoryOwner = repositoryOwner;
            this.repositoryName = repositoryName;
        }

        public abstract Task<PullRequestReviewComment> Create(string filePath, string commitId, int pullRequestNumber);

        protected async Task<PullRequestReviewComment> Create(PullRequestReviewCommentCreate comment, int pullRequestNumber)
        {
            return await client.Create(repositoryOwner, repositoryName, pullRequestNumber, comment);
        }
    }
}