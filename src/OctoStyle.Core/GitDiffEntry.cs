namespace OctoStyle.Core
{
    /// <summary>
    /// Represents a git diff line entry.
    /// </summary>
    public abstract class GitDiffEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitDiffEntry"/> class.
        /// </summary>
        /// <param name="position">The position of the diff entry line in a GitHub pull request.</param>
        protected GitDiffEntry(int position)
        {
            this.Position = position;
        }

        /// <summary>
        /// Gets the <see cref="Position"/>.
        /// </summary>
        /// <value>Gets the position of the diff entry line in a GitHub pull request.</value>
        public int Position { get; private set; }
    }
}