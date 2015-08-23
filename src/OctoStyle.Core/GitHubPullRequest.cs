namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Octokit;

    /// <summary>
    /// Represents a GitHub pull request.
    /// </summary>
    public class GitHubPullRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubPullRequest"/> class.
        /// </summary>
        /// <param name="number">The pull request number.</param>
        /// <param name="lastCommitId">The last commit hash of the pull request.</param>
        /// <param name="files">The <see cref="IEnumerable{T}"/> of <see cref="GitHubPullRequestFile"/>
        ///     containing files in the pull request.</param>
        /// <param name="diff"></param>
        /// <param name="branches">The <see cref="GitHubPullRequestBranches"/> the branches associated with the pull request.</param>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1303:ConstFieldNamesMustBeginWithUpperCaseLetter",
            Justification = StyleCopConstants.LocalConstantJustification)]
        public GitHubPullRequest(int number, string lastCommitId, IEnumerable<GitHubPullRequestFile> files, string diff, GitHubPullRequestBranches branches)
        {
            const string lastCommitIdParamName = "lastCommitId";

            if (lastCommitId == null)
            {
                throw new ArgumentNullException(lastCommitIdParamName);
            }

            if (files == null)
            {
                throw new ArgumentNullException("files");
            }

            if (branches == null)
            {
                throw new ArgumentNullException("branches");
            }

            if (lastCommitId.Length == 0)
            {
                throw new ArgumentException("Cannot be empty", lastCommitIdParamName);
            }

            if (number < 1)
            {
                throw new ArgumentException("Must be greater than 0", "number");
            }

            this.Number = number;
            this.LastCommitId = lastCommitId;
            this.Diff = diff;
            this.Branches = branches;
            this.Files = files.ToList();
        }

        /// <summary>
        /// Gets <see cref="Number"/>.
        /// </summary>
        /// <value>The pull request number.</value>
        public int Number { get; private set; }

        /// <summary>
        /// Gets the <see cref="LastCommitId"/>
        /// </summary>
        /// <value>The last commit ID in the pull request.</value>
        public string LastCommitId { get; private set; }

        /// <summary>
        /// Gets <see cref="Branches"/>.
        /// </summary>
        /// <value>The <see cref="GitHubPullRequestBranches"/> the branches associated with the pull request.</value>
        public GitHubPullRequestBranches Branches { get; private set; }

        /// <summary>
        /// Gets <see cref="Files"/>
        /// </summary>
        /// <value>The <see cref="IEnumerable{T}"/> of <see cref="PullRequestFile"/> containing files in the pull request.</value>
        public IReadOnlyList<GitHubPullRequestFile> Files { get; private set; }

        public string Diff { get; private set; }
    }
}