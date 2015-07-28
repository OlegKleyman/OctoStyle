namespace OctoStyle.Core
{
    /// <summary>
    /// Represents the status of a git diff entry.
    /// </summary>
    public enum GitDiffEntryStatus
    {
        /// <summary>
        /// The status of a removed line.
        /// </summary>
        Removed,

        /// <summary>
        /// The status of an added line.
        /// </summary>
        New
    }
}