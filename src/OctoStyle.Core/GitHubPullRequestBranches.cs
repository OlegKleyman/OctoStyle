namespace OctoStyle.Core
{
    using System;

    public class GitHubPullRequestBranches
    {
        public string Branch { get; set; }

        public string MergeBranch { get; set; }

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
    }
}