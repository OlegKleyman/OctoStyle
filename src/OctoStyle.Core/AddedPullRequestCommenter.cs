namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Octokit;

    /// <summary>
    /// Represents a pull request commenter for added files.
    /// </summary>
    public class AddedPullRequestCommenter : PullRequestCommenter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddedPullRequestCommenter"/> class.
        /// </summary>
        /// <param name="client">The <see cref="IPullRequestReviewCommentsClient"/> to use for making comments.</param>
        /// <param name="repository">The <see cref="GitHubRepository"/> containing the pull request to comment on.</param>
        public AddedPullRequestCommenter(IPullRequestReviewCommentsClient client, GitHubRepository repository)
            : base(client, repository)
        {
        }

        /// <summary>
        /// Creates a pull request comment.
        /// </summary>
        /// <param name="file">The <see cref="GitHubPullRequestFile"/> to comment on.</param>
        /// <param name="analyzer">The <see cref="ICodeAnalyzer"/> to use for finding violations.</param>
        /// <param name="physicalFilePath">The physical path of the file stored locally.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> of <see cref="IEnumerable{T}"/> of <see cref="PullRequestReviewComment"/> representing the commenting operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">The file or analyzer arguments are null.</exception>
        public override async Task<IEnumerable<PullRequestReviewComment>> Create(
            GitHubPullRequestFile file,
            ICodeAnalyzer analyzer,
            string physicalFilePath)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (analyzer == null)
            {
                throw new ArgumentNullException("analyzer");
            }

            var comments = new List<PullRequestReviewComment>();

            foreach (var violation in analyzer.Analyze(physicalFilePath))
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} - {1}",
                    violation.RuleId,
                    violation.Message);

                var comment = new PullRequestReviewCommentCreate(
                    message,
                    file.PullRequest.LastCommitId,
                    file.FileName,
                    violation.Position);

                comments.Add(await this.Create(comment, file.PullRequest.Number));
            }

            return comments;
        }
    }
}