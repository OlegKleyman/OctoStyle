namespace OctoStyle.Core
{
    /// <summary>
    /// Represents a GitHub pull request file status.
    /// </summary>
    public enum GitHubPullRequestFileStatus
    {
        /// <summary>
        /// No status.
        /// </summary>
        None,

        /// <summary>
        /// File has been renamed.
        /// </summary>
        Renamed,

        /// <summary>
        /// File has been added.
        /// </summary>
        Added,

        /// <summary>
        /// File has been modified.
        /// </summary>
        Modified,

        /// <summary>
        /// File has been deleted.
        /// </summary>
        Deleted
    }
}