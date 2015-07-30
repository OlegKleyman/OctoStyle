namespace OctoStyle.Core
{
    using System.Collections.Generic;

    using OctoStyle.Core.Borrowed;

    /// <summary>
    /// Represents a git diff entry factory.
    /// </summary>
    public interface IGitDiffEntryFactory
    {
        /// <summary>
        /// Gets git diff entries.
        /// </summary>
        /// <param name="entry">The <see cref="DiffEntry"/> of <see cref="string"/> to build from.</param>
        /// <param name="position">The position the diff entry exists in a git diff.</param>
        /// <returns><see cref="IReadOnlyList{T}"/> of <see cref="GitDiffEntry"/>.</returns>
        IReadOnlyList<GitDiffEntry> Get(DiffEntry entry, int position);
    }
}