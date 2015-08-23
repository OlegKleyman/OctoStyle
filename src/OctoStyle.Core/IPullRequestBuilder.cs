namespace OctoStyle.Core
{
    using System.Collections.Generic;

    using Octokit;

    public interface IPullRequestBuilder
    {
        GitHubPullRequest Build(
            int number,
            string lastCommitId,
            IReadOnlyList<PullRequestFile> files,
            string diff,
            GitHubPullRequestBranches branches);
    }
}