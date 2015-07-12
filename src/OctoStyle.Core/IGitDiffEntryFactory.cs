namespace OctoStyle.Core
{
    using System.Collections.Generic;

    using OctoStyle.Core.Borrowed;

    public interface IGitDiffEntryFactory
    {
        IReadOnlyList<GitDiffEntry> Get(DiffEntry<string> entry, int position);
    }
}