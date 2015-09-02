namespace OctoStyle.Core
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Octokit;

    /// <summary>
    /// Represents a pull request retriever.
    /// </summary>
    public class PullRequestRetriever : IPullRequestRetriever
    {
        private readonly IPullRequestBuilder builder;

        private readonly IPullRequestsClient client;

        private readonly IConnection connection;

        private readonly GitHubRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PullRequestRetriever"/> class.
        /// </summary>
        /// <param name="builder">The <see cref="IPullRequestBuilder"/> instance to build pull request objects with.</param>
        /// <param name="client">The <see cref="IPullRequestsClient"/> to use for interfacing with GitHub.</param>
        /// <param name="connection">
        /// The <see cref="IConnection"/> object to interface use when interfacing with GitHub.
        /// </param>
        /// <param name="repository">The <see cref="GitHubRepository"/> containing the pull request to comment on.</param>
        public PullRequestRetriever(
            IPullRequestBuilder builder,
            IPullRequestsClient client,
            IConnection connection,
            GitHubRepository repository)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.builder = builder;
            this.client = client;
            this.connection = connection;
            this.repository = repository;
        }

        /// <summary>
        /// Retrieves a pull request.
        /// </summary>
        /// <param name="number">The pull request number.</param>
        /// <returns>A <see cref="Task{TResult}"/> of <see cref="GitHubPullRequest"/> representing the retrieval operation.</returns>
        public async Task<GitHubPullRequest> RetrieveAsync(int number)
        {
            var pullrequestInformationMessage = string.Format(
                CultureInfo.InvariantCulture,
                "{0}Owner: {1}, Repository: {2}, Number: {3}",
                Environment.NewLine,
                this.repository.Owner,
                this.repository.Name,
                number);

            var commits = await this.client.Commits(this.repository.Owner, this.repository.Name, number);

            if (commits.Count == 0)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "No commits found for pull request{0}",
                        pullrequestInformationMessage));
            }

            var files = await this.client.Files(this.repository.Owner, this.repository.Name, number);
            var pull = await this.client.Get(this.repository.Owner, this.repository.Name, number);
            var diff = await this.connection.Get<string>(pull.Url, null, "application/vnd.github.VERSION.diff");

            return this.builder.Build(
                number,
                commits.Last().Sha,
                files,
                diff.Body,
                new GitHubPullRequestBranches(pull.Head.Ref, pull.Base.Ref));
        }
    }
}