namespace OctoStyle.Core
{
    /// <summary>
    /// Represents a modification git diff entry line.
    /// </summary>
    public class ModificationGitDiffEntry : GitDiffEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModificationGitDiffEntry"/> class.
        /// </summary>
        /// <param name="position">The position of the diff entry line.</param>
        /// <param name="status">The <see cref="GitDiffEntryStatus"/>.</param>
        /// <param name="lineNumber">The line number of the modification.</param>
        public ModificationGitDiffEntry(int position, GitDiffEntryStatus status, int lineNumber)
            : base(position)
        {
            this.Status = status;
            this.LineNumber = lineNumber;
        }

        /// <summary>
        /// Gets the <see cref="Status"/>.
        /// </summary>
        /// <value>The <see cref="GitDiffEntryStatus"/>.</value>
        public GitDiffEntryStatus Status { get; private set; }

        /// <summary>
        /// Gets the <see cref="LineNumber"/>.
        /// </summary>
        /// <value>The line number of the modification.</value>
        public int LineNumber { get; private set; }
    }
}