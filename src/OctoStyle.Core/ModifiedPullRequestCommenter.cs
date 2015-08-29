namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Octokit;

    /// <summary>
    /// Represents a modification pull request commenter.
    /// </summary>
    public class ModifiedPullRequestCommenter : PullRequestCommenter
    {
        private readonly IGitHubDiffRetriever diffRetriever;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiedPullRequestCommenter"/> class.
        /// </summary>
        /// <param name="client">The <see cref="IPullRequestReviewCommentsClient"/> to use for interfacing with github.</param>
        /// <param name="repository">The <see cref="GitHubRepository"/> the pull requests are in.</param>
        /// <param name="diffRetriever">The <see cref="IGitHubDiffRetriever"/> to used to retrieve diffs.</param>
        public ModifiedPullRequestCommenter(
            IPullRequestReviewCommentsClient client,
            GitHubRepository repository,
            IGitHubDiffRetriever diffRetriever)
            : base(client, repository)
        {
            if (diffRetriever == null)
            {
                throw new ArgumentNullException("diffRetriever");
            }

            this.diffRetriever = diffRetriever;
        }

        /// <summary>
        /// Creates a pull request comment.
        /// </summary>
        /// <param name="pullRequest"></param>
        /// <param name="file">The <see cref="GitHubPullRequestFile"/> to comment on.</param>
        /// <param name="analyzer">The <see cref="ICodeAnalyzer"/> to use for finding violations.</param>
        /// <param name="physicalFilePath">The physical path of the file stored locally.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> of <see cref="IEnumerable{T}"/> of <see cref="PullRequestReviewComment"/>
        /// representing the commenting operation.
        /// </returns>
        public override async Task<IEnumerable<PullRequestReviewComment>> Create(
            GitHubPullRequest pullRequest,
            GitHubPullRequestFile file,
            ICodeAnalyzer analyzer,
            string physicalFilePath)
        {
            if (pullRequest == null)
            {
                throw new ArgumentNullException("pullRequest");
            }

            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (analyzer == null)
            {
                throw new ArgumentNullException("analyzer");
            }

            var diff = this.diffRetriever.Retrieve(file.Diff).OfType<ModificationGitDiffEntry>();

            var violations = analyzer.Analyze(physicalFilePath);

            var accessibleViolations = diff.Where(entry => entry.Status == GitDiffEntryStatus.New)
                .Join(
                    violations,
                    entry => entry.LineNumber,
                    violation => violation.LineNumber,
                    (entry, violation) => new { violation.RuleId, violation.Message, entry.Position });

            var comments = new List<PullRequestReviewComment>();

            foreach (var violation in accessibleViolations)
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} - {1}",
                    violation.RuleId,
                    violation.Message);

                var comment = new PullRequestReviewCommentCreate(
                    message,
                    pullRequest.LastCommitId,
                    file.FileName,
                    violation.Position);

                comments.Add(await this.Create(comment, pullRequest.Number));
            }

            return comments;
        }
    }
}