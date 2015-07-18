namespace OctoStyle.Core
{
    using System;

    public class GitHubPullRequestFile
    {
        public string FileName { get; private set; }

        public GitHubPullRequest PullRequest { get; private set; }

        public GitHubPullRequestFile(string fileName, GitHubPullRequest pullRequest)
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
        }
    }
}