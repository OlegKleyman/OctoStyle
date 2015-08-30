namespace OctoStyle.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a github diff retriever.
    /// </summary>
    public interface IGitHubDiffRetriever
    {
        /// <summary>
        /// Retrieves a GitHub pull request file diff.
        /// </summary>
        /// <param name="rawDiff">The pull request file diff content.</param>
        /// <returns>An <see cref="IReadOnlyList{T}"/> of <see cref="GitDiffEntry"/>.</returns>
        IReadOnlyList<GitDiffEntry> Retrieve(string rawDiff);
    }
}