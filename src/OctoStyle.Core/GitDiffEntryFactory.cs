namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using OctoStyle.Core.Borrowed;

    /// <summary>
    /// Represents a git diff entry factory.
    /// </summary>
    public class GitDiffEntryFactory : IGitDiffEntryFactory
    {
        /// <summary>
        /// Gets git diff entries.
        /// </summary>
        /// <param name="entry">The <see cref="DiffEntry{T}"/> of <see cref="string"/> to build from.</param>
        /// <param name="position">The position the diff entry exists in a git diff.</param>
        /// <returns><see cref="IReadOnlyList{T}"/> of <see cref="GitDiffEntry"/>.</returns>
        public IReadOnlyList<GitDiffEntry> Get(DiffEntry<string> entry, int position)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            var gitDiff = new List<GitDiffEntry>();

            switch (entry.EntryType)
            {
                case DiffEntryType.Equal:
                    for (var i = 0; i < entry.Count; i++)
                    {
                        gitDiff.Add(new EqualGitDiffEntry(position++));
                    }

                    break;
                case DiffEntryType.Add:
                    gitDiff.Add(new ModificationGitDiffEntry(position++, GitDiffEntryStatus.New, entry.LineNumber));
                    break;
                case DiffEntryType.Remove:
                    gitDiff.Add(new ModificationGitDiffEntry(position++, GitDiffEntryStatus.Removed, entry.LineNumber));
                    break;
                default:
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Unable to convert DiffEntry of type {0} to GitDiff",
                            entry.EntryType));
            }

            return gitDiff;
        }
    }
}