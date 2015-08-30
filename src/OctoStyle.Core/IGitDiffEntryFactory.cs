namespace OctoStyle.Core
{
    using System.Collections.Generic;

    using SharpDiff.FileStructure;

    /// <summary>
    /// Represents a git diff entry factory.
    /// </summary>
    public interface IGitDiffEntryFactory
    {
        /// <summary>
        /// Gets git diff entries.
        /// </summary>
        /// <param name="snippet">The <see cref="ISnippet"/> containing diff entries.</param>
        /// <param name="position">The position the starting diff entry that exists in the git diff.</param>
        /// <param name="lineNumber">The starting line number of the diff entry.</param>
        /// <returns><see cref="IReadOnlyList{T}"/> of <see cref="GitDiffEntry"/>.</returns>
        IReadOnlyList<GitDiffEntry> GetList(ISnippet snippet, int position, int lineNumber);
    }
}