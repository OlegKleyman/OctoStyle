namespace OctoStyle.Core
{
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a pull request retriever.
    /// </summary>
    public interface IPullRequestRetriever
    {
        /// <summary>
        /// Retrieves a pull request.
        /// </summary>
        /// <param name="number">The pull request number.</param>
        /// <returns>A <see cref="Task{TResult}"/> of <see cref="GitHubPullRequest"/> representing the retrieval operation.</returns>
        Task<GitHubPullRequest> RetrieveAsync(int number);
    }
}