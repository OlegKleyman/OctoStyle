namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Octokit;

    using OctoStyle.Core.Borrowed;

    public class GitDiffRetriever : IGitDiffRetriever
    {
        private readonly Uri baseUri;

        public IConnection Connection { get; set; }

        public GitDiffRetriever(IConnection connection, GitRepository repository)
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
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "https://api.github.com/repos/{0}/{1}/contents/", repository.Owner, repository.Name));
        }

        public async Task<IReadOnlyList<GitDiffEntry>> RetrieveAsync(string filePath, string newBranch, string originalBranch)
        {
            
            var newFileEndpoint = new Uri(baseUri, String.Concat(filePath, "?ref=", newBranch));
            var originalFileEndpoint = new Uri(baseUri, String.Concat(filePath, "?ref=", originalBranch));
            var newFileContents = await Connection.Get<Octokit.RepositoryContent>(newFileEndpoint, null, null);
            var originalFileContents = await Connection.Get<Octokit.RepositoryContent>(originalFileEndpoint, null, null);

            if (newFileContents.Body == null)
            {
                const string filePathParamName = "filePath";

                throw new ArgumentOutOfRangeException(
                    filePathParamName,
                    String.Format(CultureInfo.InvariantCulture, "Unale to retrieve new file {0}", filePath));
            }

            if (originalFileContents.Body == null)
            {
                const string filePathParamName = "filePath";

                throw new ArgumentOutOfRangeException(
                    filePathParamName,
                    String.Format(CultureInfo.InvariantCulture, "Unale to retrieve original file {0}", filePath));
            }

            const char githubNewlineDelimeter = '\n';

            var diff =
                Diff.CreateDiff(
                    originalFileContents.Body.Content.Split(new[] { githubNewlineDelimeter }, StringSplitOptions.None),
                    newFileContents.Body.Content.Split(new[] { githubNewlineDelimeter }, StringSplitOptions.None))
                    .ToGitDiff(new GitDiffEntryFactory());

            return diff;
        }
    }
}