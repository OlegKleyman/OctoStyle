namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Octokit;

    using OctoStyle.Core.Borrowed;

    /// <summary>
    /// Represents a github diff retriever.
    /// </summary>
    public class GitHubDiffRetriever : IGitHubDiffRetriever
    {
        private readonly Uri baseUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubDiffRetriever"/> class.
        /// </summary>
        /// <param name="connection">The <see cref="IConnection"/> to use for interfacing with GitHub.</param>
        /// <param name="repository">The <see cref="GitHubRepository"/> to interface with.</param>
        public GitHubDiffRetriever(IConnection connection, GitHubRepository repository)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.Connection = connection;
            this.baseUri =
                new Uri(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "https://api.github.com/repos/{0}/{1}/contents/",
                        repository.Owner,
                        repository.Name));
        }

        /// <summary>
        /// Gets the <see cref="IConnection"/> used by this instance.
        /// </summary>
        /// <value>Gets the <see cref="IConnection"/> object which interfaces with GitHub.</value>
        public IConnection Connection { get; private set; }

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
        public async Task<IReadOnlyList<GitDiffEntry>> RetrieveAsync(
            string filePath,
            string newBranch,
            string originalBranch)
        {
            var newFileEndpoint = new Uri(this.baseUri, string.Concat(filePath, "?ref=", newBranch));
            var originalFileEndpoint = new Uri(this.baseUri, string.Concat(filePath, "?ref=", originalBranch));
            var newFileContents = await this.Connection.Get<RepositoryContent>(newFileEndpoint, null, null);
            var originalFileContents = await this.Connection.Get<RepositoryContent>(originalFileEndpoint, null, null);

            var delimeter = new[] { '\n' };

            var diff =
                Diff.CreateDiff(
                    originalFileContents.Body.Content.Split(delimeter, StringSplitOptions.None),
                    newFileContents.Body.Content.Split(delimeter, StringSplitOptions.None))
                    .ToGitDiff(new GitDiffEntryFactory());

            return diff;
        }
    }
}