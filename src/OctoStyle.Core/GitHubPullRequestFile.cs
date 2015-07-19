namespace OctoStyle.Core
{
    using System;

    public class GitHubPullRequestFile
    {
        public string FileName { get; private set; }

        public GitHubPullRequest PullRequest { get; private set; }

        public GitPullRequestFileStatus Status { get; private set; }

        public int Changes { get; private set; }

        public GitHubPullRequestFile(string fileName, GitHubPullRequest pullRequest, GitPullRequestFileStatus status, int changes)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (pullRequest == null)
            {
                throw new ArgumentNullException("pullRequest");
            }

            if (fileName.Length == 0)
            {
                throw new ArgumentException("Cannot be empty", "fileName");
            }

            this.FileName = fileName;
            this.PullRequest = pullRequest;
            this.Status = status;
            this.Changes = changes;
        }
    }
}