namespace OctoStyle.Core
{
    using System;

    /// <summary>
    /// Represents the branches associated with a GitHub pull request.
    /// </summary>
    public class GitHubPullRequestBranches
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubPullRequestBranches"/> class.
        /// </summary>
        /// <param name="branch">The branch requesting to be merged.</param>
        /// <param name="mergeBranch">The branch to be merged into.</param>
        public GitHubPullRequestBranches(string branch, string mergeBranch)
        {
            const string branchParamName = "branch";
            const string mergeBranchParamName = "mergeBranch";

            if (branch == null)
            {
                throw new ArgumentNullException(branchParamName);
            }

            if (mergeBranch == null)
            {
                throw new ArgumentNullException(mergeBranchParamName);
            }

            const string cannotBeEmptyMessage = "Cannot be empty";

            if (branch.Length == 0)
            {
                throw new ArgumentException(cannotBeEmptyMessage, branchParamName);
            }

            if (mergeBranch.Length == 0)
            {
                throw new ArgumentException(cannotBeEmptyMessage, mergeBranchParamName);
            }

            this.Branch = branch;
            this.MergeBranch = mergeBranch;
        }

        /// <summary>
        /// Gets the <see cref="Branch"/>.
        /// </summary>
        /// <value>The branch requesting to be merged.</value>
        public string Branch { get; private set; }

        /// <summary>
        /// Gets the <see cref="MergeBranch"/>.
        /// </summary>
        /// <value>The branch to be merged into.</value>
        public string MergeBranch { get; private set; }
    }
}