namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Octokit;

    /// <summary>
    /// Represents a pull request commenter.
    /// </summary>
    public abstract class PullRequestCommenter
    {
        private readonly IPullRequestReviewCommentsClient client;

        private readonly GitHubRepository repository;

        private PullRequestCommenter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PullRequestCommenter"/> class.
        /// </summary>
        /// <param name="client">The <see cref="IPullRequestReviewCommentsClient"/> to use for making comments.</param>
        /// <param name="repository">The <see cref="GitHubRepository"/> containing the pull request to comment on.</param>
        protected PullRequestCommenter(IPullRequestReviewCommentsClient client, GitHubRepository repository)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.client = client;
            this.repository = repository;
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
        public abstract Task<IEnumerable<PullRequestReviewComment>> Create(
            GitHubPullRequestFile file,
            ICodeAnalyzer analyzer,
            string physicalFilePath);

        /// <summary>
        /// Creates a pull request comment.
        /// </summary>
        /// <param name="comment">The <see cref="PullRequestReviewCommentCreate"/> to create.</param>
        /// <param name="pullRequestNumber">The GitHub pull request number.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> of <see cref="PullRequestReviewComment"/> representing the commenting
        /// operation.
        /// </returns>
        protected async Task<PullRequestReviewComment> Create(
            PullRequestReviewCommentCreate comment,
            int pullRequestNumber)
        {
            return await this.client.Create(this.repository.Owner, this.repository.Name, pullRequestNumber, comment);
        }

        /// <summary>
        /// Represents a dumb commenter.
        /// </summary>
        public class NoCommentPullRequestCommenter : PullRequestCommenter
        {
            private static readonly NoCommentPullRequestCommenter commenter = new NoCommentPullRequestCommenter();

            private NoCommentPullRequestCommenter()
            {
            }

            /// <summary>
            /// Gets a <see cref="PullRequestCommenter"/>.
            /// </summary>
            public static PullRequestCommenter NoComment
            {
                get
                {
                    return commenter;
                }
            }

            /// <summary>
            /// Does not do anything.
            /// </summary>
            /// <param name="file">The <see cref="GitHubPullRequestFile"/> to comment on.</param>
            /// <param name="analyzer">The <see cref="ICodeAnalyzer"/> to use for finding violations.</param>
            /// <param name="physicalFilePath">The physical path of the file stored locally.</param>
            /// <returns>
            /// A <see cref="Task{TResult}"/> of <see cref="IEnumerable{T}"/> of <see cref="PullRequestReviewComment"/>
            /// representing the commenting operation.
            /// </returns>
            /// <remarks>
            /// The result of <see cref="Task{TResult}"/> of <see cref="IEnumerable{T}"/> of <see cref="PullRequestReviewComment"/>
            /// will always be an empty enumeration.
            /// </remarks>
            public override Task<IEnumerable<PullRequestReviewComment>> Create(
                GitHubPullRequestFile file,
                ICodeAnalyzer analyzer,
                string physicalFilePath)
            {
                var task = new Task<IEnumerable<PullRequestReviewComment>>(() => new List<PullRequestReviewComment>());
                task.RunSynchronously();
                return task;
            }
        }
    }
}