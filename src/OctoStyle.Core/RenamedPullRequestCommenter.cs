namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Octokit;

    /// <summary>
    /// Represents a renamed pull request commenter.
    /// </summary>
    public class RenamedPullRequestCommenter : PullRequestCommenter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenamedPullRequestCommenter"/> class.
        /// </summary>
        /// <param name="client">The <see cref="IPullRequestReviewCommentsClient"/> to use for making comments.</param>
        /// <param name="repository">The <see cref="GitHubRepository"/> containing the pull request to comment on.</param>
        public RenamedPullRequestCommenter(IPullRequestReviewCommentsClient client, GitHubRepository repository)
            : base(client, repository)
        {
        }

        /// <summary>
        /// Creates a pull request comment.
        /// </summary>
        /// <param name="pullRequest">The <see cref="GitHubPullRequest"/> to comment on.</param>
        /// <param name="file">The <see cref="GitHubPullRequestFile"/> to comment on.</param>
        /// <param name="analyzer">The <see cref="ICodeAnalyzer"/> to use for finding violations.</param>
        /// <param name="physicalFilePath">The physical path of the file stored locally.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> of <see cref="IEnumerable{T}"/> of <see cref="PullRequestReviewComment"/>
        /// representing the commenting operation.
        /// </returns>
        public override async Task<IEnumerable<PullRequestReviewComment>> Create(GitHubPullRequest pullRequest, GitHubPullRequestFile file, ICodeAnalyzer analyzer, string physicalFilePath)
        {
            if (pullRequest == null)
            {
                throw new ArgumentNullException("pullRequest");
            }

            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            var comments = new List<PullRequestReviewComment>();

            if (file.Changes > 0)
            {
                var comment = new PullRequestReviewCommentCreate(
                    "Renamed files not supported.",
                    pullRequest.LastCommitId,
                    file.FileName,
                    1);

                var addedComment = await this.Create(comment, pullRequest.Number);

                comments.Add(addedComment);
            }

            return comments;
        }
    }
}