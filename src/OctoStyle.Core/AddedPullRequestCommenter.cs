namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Octokit;

    public class AddedPullRequestCommenter : PullRequestCommenter
    {
        public AddedPullRequestCommenter(IPullRequestReviewCommentsClient client, GitRepository repository)
            : base(client, repository)
        {
        }

        public async override Task<IEnumerable<PullRequestReviewComment>> Create(GitHubPullRequestFile file)
        {
            throw new NotImplementedException();
        }
    }
}