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
                var entries = factory.Get(entry, position++);
                
                if (entries == null)
                {
                    throw new InvalidOperationException(
                        String.Format(
                            CultureInfo.InvariantCulture,
                            "Unable to retrieve git diff entry for Type: {0}, Line Number: {1}, Position: {2}",
                            entry.EntryType,
                            entry.LineNumber,
                            position));
                }

                gitDiff.AddRange(entries);
            }

            return gitDiff.AsReadOnly();
        }
    }
}