namespace OctoStyle.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGitHubDiffRetriever
    {
        Task<IReadOnlyList<GitDiffEntry>> RetrieveAsync(string filePath, string newBranch, string originalBranch);
    }
}