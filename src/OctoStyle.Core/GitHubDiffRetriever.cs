namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Octokit;

    using SharpDiff.FileStructure;

    using Diff = OctoStyle.Core.Borrowed.Diff;

    /// <summary>
    /// Represents a github diff retriever.
    /// </summary>
    public class GitHubDiffRetriever : IGitHubDiffRetriever
    {
        private readonly IDiffer differ;

        private readonly Uri baseUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubDiffRetriever"/> class.
        /// </summary>
        /// <param name="differ"></param>
        /// <param name="connection">The <see cref="IConnection"/> to use for interfacing with GitHub.</param>
        /// <param name="repository">The <see cref="GitHubRepository"/> to interface with.</param>
        public GitHubDiffRetriever(IDiffer differ, IConnection connection, GitHubRepository repository)
        {
            if (differ == null)
            {
                throw new ArgumentNullException("differ");
            }

            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.differ = differ;
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

        public IReadOnlyList<GitDiffEntry> Retrieve(string rawDiff)
        {
            var diff = this.differ.Load(rawDiff);

            var enumeratedDiff = diff as SharpDiff.FileStructure.Diff[] ?? diff.ToArray();

            if (diff == null || !enumeratedDiff.Any())
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Unable to retrieve diff for:{0}{1}",
                        Environment.NewLine,
                        rawDiff));
            }

            if (enumeratedDiff.Length > 1)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Multiple diffs found for:{0}{1}",
                        Environment.NewLine,
                        rawDiff));
            }

            var targetDiff = enumeratedDiff.First();
            var diffEntries = new List<GitDiffEntry>();
            var factory = new GitDiffEntryFactory();
            var position = 1;

            foreach (var chunk in targetDiff.Chunks)
            {
                var lineNumer = chunk.NewRange.StartLine;

                foreach (var snippet in chunk.Snippets)
                {
                    diffEntries.AddRange(factory.Get(snippet, position, lineNumer));

                    position = diffEntries.Count + 1;
                    lineNumer += snippet.ModifiedLines.Count()
                                 + snippet.OriginalLines.SelectMany(line => line.Spans)
                                       .Count(span => span.Kind == SpanKind.Equal);
                }
            }

            return diffEntries;
        }
    }
}