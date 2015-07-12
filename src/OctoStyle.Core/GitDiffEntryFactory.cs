namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using OctoStyle.Core.Borrowed;

    public class GitDiffEntryFactory
    {
        public IReadOnlyList<GitDiffEntry> Get(DiffEntry<string> entry, int position)
        {
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
                        String.Format(
                            CultureInfo.InvariantCulture,
                            "Unable to convert DiffEntry of type {0} to GitDiff",
                            entry.EntryType));
            }

            return gitDiff;
        }
    }
}