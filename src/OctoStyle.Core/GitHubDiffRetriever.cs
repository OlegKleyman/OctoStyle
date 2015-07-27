namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Octokit;

    using OctoStyle.Core.Borrowed;

    public class GitHubDiffRetriever : IGitHubDiffRetriever
    {
        private readonly Uri baseUri;

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

        public IConnection Connection { get; set; }

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