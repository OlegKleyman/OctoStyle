namespace OctoStyle.Core
{
    /// <summary>
    /// Represents an unchanged line in a git diff.
    /// </summary>
    public class EqualGitDiffEntry : GitDiffEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EqualGitDiffEntry"/> class.
        /// </summary>
        /// <param name="position">The position of the diff entry line.</param>
        public EqualGitDiffEntry(int position)
            : base(position)
        {
        }
    }
}