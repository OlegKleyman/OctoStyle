namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using OctoStyle.Core.Borrowed;

    using SharpDiff.FileStructure;

    using Diff = SharpDiff.FileStructure.Diff;

    /// <summary>
    /// Represents a git diff entry factory.
    /// </summary>
    internal class GitDiffEntryFactory : IGitDiffEntryFactory
    {
        /// <summary>
        /// Gets git diff entries.
        /// </summary>
        /// <param name="entry">The <see cref="DiffEntry"/> to build from.</param>
        /// <param name="position">The position the diff entry exists in a git diff.</param>
        /// <returns><see cref="IReadOnlyList{T}"/> of <see cref="GitDiffEntry"/>.</returns>
        public IReadOnlyList<GitDiffEntry> Get(DiffEntry entry, int position)
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
                            "Unable to convert {0} of type {1} to a {2}",
                            typeof(DiffEntry),
                            entry.EntryType,
                            typeof(GitDiffEntry)));
            }

            return gitDiff;
        }

        public IReadOnlyList<GitDiffEntry> Get(ISnippet entry, int position)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            if (entry.OriginalLines == null)
            {
                throw new ArgumentException("OriginalLines are missing.");
            }

            return
                entry.OriginalLines.Select(originalLine => new EqualGitDiffEntry(position++))
                    .Cast<GitDiffEntry>()
                    .ToList();
        }
    }
}