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
        /// Retrieves a diff of a file between two GitHub branches.
        /// </summary>
        /// <param name="filePath">The file path as in the GitHub repository.</param>
        /// <param name="newBranch">The branch of the modified file.</param>
        /// <param name="originalBranch">The branch of the original file.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="GitDiffEntry"/> that
        /// represents the retrieval asynchronous retrieval operation.
        /// </returns>
        Task<IReadOnlyList<GitDiffEntry>> RetrieveAsync(string filePath, string newBranch, string originalBranch);
    }
}