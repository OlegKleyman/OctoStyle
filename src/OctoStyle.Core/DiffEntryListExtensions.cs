namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using OctoStyle.Core.Borrowed;

    /// <summary>
    /// Contains extension methods for <see cref="IEnumerable{T}"/> of <see cref="DiffEntry{T}"/> of <see cref="string"/>.
    /// </summary>
    public static class DiffEntryListExtensions
    {
        /// <summary>
        /// Gets a <see cref="GitDiffEntry"/> representation of <see cref="IEnumerable{T}"/> of <see cref="DiffEntry{T}"/>
        /// of <see cref="string"/>.
        /// </summary>
        /// <param name="diff">A <see cref="IEnumerable{T}"/> of <see cref="DiffEntry{T}"/> of <see cref="string"/> to
        /// use as a template to build from.</param>
        /// <param name="factory">The <see cref="IGitDiffEntryFactory"/> to use when retrieving <see cref="GitDiffEntry"/>
        /// objects.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="GitDiffEntry"/> representation of the <paramref name="diff"/>
        /// parameter.</returns>
        public static IReadOnlyList<GitDiffEntry> ToGitDiff(
            this IEnumerable<DiffEntry> diff,
            IGitDiffEntryFactory factory)
        {
            if (diff == null)
            {
                throw new ArgumentNullException("diff");
            }

            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            var gitDiff = new List<GitDiffEntry>();
            var position = 1;

            foreach (var entry in diff)
            {
                var entries = factory.Get(entry, position);

                if (entries == null)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Unable to retrieve git diff entry for Type: {0}, Line Number: {1}, Position: {2}",
                            entry.EntryType,
                            entry.LineNumber,
                            position));
                }

                gitDiff.AddRange(entries);
                position += entry.Count;
            }

            return gitDiff.AsReadOnly();
        }
    }
}