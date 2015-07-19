namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Octokit;

    public class GitHubPullRequest
    {
        public int Number { get; private set; }

        public string LastCommitId { get; private set; }

        public GitHubPullRequestBranches Branches { get; private set; }

        public IReadOnlyList<GitHubPullRequestFile> Files { get; private set; }

        public GitHubPullRequest(int number, string lastCommitId, IEnumerable<PullRequestFile> files, GitHubPullRequestBranches branches)
        {
            if (lastCommitId == null)
            {
                throw new ArgumentNullException("lastCommitId");
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
                throw new ArgumentException("Cannot be empty", "lastCommitId");
            }

            if (number < 1)
            {
                throw new ArgumentException("Must be greater than 0", "number");
            }

            this.Number = number;
            this.LastCommitId = lastCommitId;
            this.Branches = branches;
            this.Files =
                files.Select(
                    file =>
                    new GitHubPullRequestFile(
                        file.FileName,
                        this,
                        (GitPullRequestFileStatus)Enum.Parse(typeof(GitPullRequestFileStatus), file.Status, true),
                        file.Changes)).ToList();
        }
    }
}