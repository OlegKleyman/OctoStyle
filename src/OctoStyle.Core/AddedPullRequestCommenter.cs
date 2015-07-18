namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Octokit;

    using StyleCop;

    public class AddedPullRequestCommenter : PullRequestCommenter
    {
        public AddedPullRequestCommenter(IPullRequestReviewCommentsClient client, GitRepository repository)
            : base(client, repository)
        {
        }

        public async override Task<IEnumerable<PullRequestReviewComment>> Create(GitHubPullRequestFile file, IEnumerable<Violation> violations)
        {
            throw new NotImplementedException();
        }
    }
}