namespace OctoStyle.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Octokit;

    public class RenamedPullRequestCommenter : PullRequestCommenter
    {
        public RenamedPullRequestCommenter(
            IPullRequestReviewCommentsClient client,
            GitRepository repository)
            : base(client, repository)
        {

        }

        public async override Task<IEnumerable<PullRequestReviewComment>> Create(string filePath, string commitId, int pullRequestNumber)
        {
            var comment = new PullRequestReviewCommentCreate(
                "Renamed files not supported.",
                commitId,
                filePath,
                1);

            var addedComment = await Create(comment, pullRequestNumber);

            return new List<PullRequestReviewComment> { addedComment };
        }
    }
}