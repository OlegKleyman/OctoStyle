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
                gitDiff.AddRange(factory.Get(entry, position++));
            }

            return gitDiff.AsReadOnly();
        }
    }
}