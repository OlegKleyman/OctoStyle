namespace OctoStyle.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a github diff retriever.
    /// </summary>
    public interface IGitHubDiffRetriever
    {
        IReadOnlyList<GitDiffEntry> Retrieve(string rawDiff);
    }
}