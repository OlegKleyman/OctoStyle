namespace OctoStyle.Core
{
    using System;

    /// <summary>
    /// Represents a file in a GitHub pull request.
    /// </summary>
    public class GitHubPullRequestFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubPullRequestFile"/> class.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="pullRequest">The <see cref="GitHubPullRequest"/> the file is a part of.</param>
        /// <param name="status">The <see cref="GitHubPullRequestFileStatus"/> of the file.</param>
        /// <param name="changes">The amount of changes in the file.</param>
        public GitHubPullRequestFile(
            string fileName,
            GitHubPullRequest pullRequest,
            GitHubPullRequestFileStatus status,
            int changes)
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

        /// <summary>
        /// Gets the <see cref="FileName"/>.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the <see cref="PullRequest"/>.
        /// </summary>
        /// <value>The <see cref="GitHubPullRequest"/> the file is a part of.</value>
        public GitHubPullRequest PullRequest { get; private set; }

        /// <summary>
        /// Gets the <see cref="Status"/>.
        /// </summary>
        /// <value>The <see cref="GitHubPullRequestFileStatus"/> of the file.</value>
        public GitHubPullRequestFileStatus Status { get; private set; }

        /// <summary>
        /// Gets the <see cref="Changes"/>.
        /// </summary>
        /// <value>The amount of changes in the file.</value>
        public int Changes { get; private set; }
    }
}