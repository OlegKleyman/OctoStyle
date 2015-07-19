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

        private PullRequestCommenter() { }

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
        
        public abstract Task<IEnumerable<PullRequestReviewComment>> Create(GitHubPullRequestFile file, ICodeAnalyzer analyzer, string physicalFilePath);

        protected async Task<PullRequestReviewComment> Create(PullRequestReviewCommentCreate comment, int pullRequestNumber)
        {
            return await client.Create(repository.Owner, repository.Name, pullRequestNumber, comment);
        }

        public class NoCommentPullRequestCommenter : PullRequestCommenter
        {
            private NoCommentPullRequestCommenter()
            {
            }

            private static readonly NoCommentPullRequestCommenter commenter = new NoCommentPullRequestCommenter();

            public static PullRequestCommenter NoComment
            {
                get
                {
                    return commenter;
                }
            }

            public override Task<IEnumerable<PullRequestReviewComment>> Create(GitHubPullRequestFile file, ICodeAnalyzer analyzer, string physicalFilePath)
            {
                var task = new Task<IEnumerable<PullRequestReviewComment>>(() => new List<PullRequestReviewComment>());
                task.RunSynchronously();
                return task;
            }
        }
    }
}