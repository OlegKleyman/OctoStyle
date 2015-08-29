namespace OctoStyle.Core
{
    using System.Collections.Generic;

    using Octokit;

    /// <summary>
    /// Represents a pull request builder.
    /// </summary>
    public interface IPullRequestBuilder
    {
        /// <summary>
        /// Builds a <see cref="GitHubPullRequest"/>.
        /// </summary>
        /// <param name="number">The pull request number.</param>
        /// <param name="lastCommitId">The last commit ID.</param>
        /// <param name="files">The files in the pull request.</param>
        /// <param name="diff">The full pull request unified diff.</param>
        /// <param name="branches">The <see cref="GitHubPullRequestBranches"/>.</param>
        /// <returns>A <see cref="GitHubPullRequest"/> object.</returns>
        GitHubPullRequest Build(
            int number,
            string lastCommitId,
            IReadOnlyList<PullRequestFile> files,
            string diff,
            GitHubPullRequestBranches branches);
    }
}