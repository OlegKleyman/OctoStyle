namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using OctoStyle.Core.Borrowed;

    public static class DiffEntryListExtensions
    {
        public static IReadOnlyList<GitDiffEntry> ToGitDiff(this IEnumerable<DiffEntry<string>> diff, IGitDiffEntryFactory factory)
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
            var position = 0;

            foreach (var entry in diff)
            {
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
                            String.Format(
                                CultureInfo.InvariantCulture,
                                "Unable to convert DiffEntry of type {0} to GitDiff",
                                entry.EntryType));
                }
            }

            return gitDiff.AsReadOnly();
        }
    }
}