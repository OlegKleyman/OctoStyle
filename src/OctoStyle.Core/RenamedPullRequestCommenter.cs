namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Octokit;

    public class RenamedPullRequestCommenter : PullRequestCommenter
    {
        public RenamedPullRequestCommenter(IPullRequestReviewCommentsClient client, GitHubRepository repository)
            : base(client, repository)
        {
        }

        public override async Task<IEnumerable<PullRequestReviewComment>> Create(
            GitHubPullRequestFile file,
            ICodeAnalyzer analyzer,
            string physicalFilePath)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            var comments = new List<PullRequestReviewComment>();

            if (file.Changes > 0)
            {
                var comment = new PullRequestReviewCommentCreate(
                    "Renamed files not supported.",
                    file.PullRequest.LastCommitId,
                    file.FileName,
                    1);

                var addedComment = await this.Create(comment, file.PullRequest.Number);

                comments.Add(addedComment);
            }

            return comments;
        }
    }
}