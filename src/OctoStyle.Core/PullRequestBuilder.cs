namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Octokit;

    /// <summary>
    /// Represents a pull request builder.
    /// </summary>
    public class PullRequestBuilder : IPullRequestBuilder
    {
        private readonly IDiffParser parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="PullRequestBuilder"/> class.
        /// </summary>
        /// <param name="parser">The <see cref="IDiffParser"/> to use to parse diffs.</param>
        public PullRequestBuilder(IDiffParser parser)
        {
            if (parser == null)
            {
                throw new ArgumentNullException("parser");
            }

            this.parser = parser;
        }

        /// <summary>
        /// Builds a <see cref="GitHubPullRequest"/>.
        /// </summary>
        /// <param name="number">The pull request number.</param>
        /// <param name="lastCommitId">The last commit ID.</param>
        /// <param name="files">The files in the pull request.</param>
        /// <param name="diff">The full pull request unified diff.</param>
        /// <param name="branches">The <see cref="GitHubPullRequestBranches"/>.</param>
        /// <returns>A <see cref="GitHubPullRequest"/> object.</returns>
        public GitHubPullRequest Build(
            int number,
            string lastCommitId,
            IReadOnlyList<PullRequestFile> files,
            string diff,
            GitHubPullRequestBranches branches)
        {
            if (files == null)
            {
                throw new ArgumentNullException("files");
            }

            var diffs = this.parser.Split(diff);

            var pullRequestFiles =
                files.Select(
                    file =>
                    new GitHubPullRequestFile(
                        file.FileName,
                        (GitHubPullRequestFileStatus)Enum.Parse(typeof(GitHubPullRequestFileStatus), file.Status, true),
                        file.Changes,
                        diffs[file.FileName])).ToList();

            var pullRequest = new GitHubPullRequest(number, lastCommitId, pullRequestFiles, diff, branches);

            return pullRequest;
        }
    }
}