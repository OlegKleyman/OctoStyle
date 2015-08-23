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
        /// <param name="status">The <see cref="GitHubPullRequestFileStatus"/> of the file.</param>
        /// <param name="changes">The amount of changes in the file.</param>
        /// <param name="diff"></param>
        public GitHubPullRequestFile(string fileName, GitHubPullRequestFileStatus status, int changes, string diff)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (diff == null)
            {
                throw new ArgumentNullException("diff");
            }

            const string cannotBeEmptyMessage = "Cannot be empty";
            
            if (fileName.Length == 0)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "fileName");
            }

            if (diff.Length == 0)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "diff");
            }

            this.FileName = fileName;
            this.Status = status;
            this.Changes = changes;
            this.Diff = diff;
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
        public GitHubPullRequest PullRequest { get; internal set; }

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

        public string Diff { get; private set; }
    }
}