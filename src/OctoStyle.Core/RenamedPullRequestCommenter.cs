namespace OctoStyle.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Octokit;

    using StyleCop;

    public class RenamedPullRequestCommenter : PullRequestCommenter
    {
        public RenamedPullRequestCommenter(
            IPullRequestReviewCommentsClient client,
            GitRepository repository)
            : base(client, repository)
        {

        }

        public async override Task<IEnumerable<PullRequestReviewComment>> Create(GitHubPullRequestFile file, IEnumerable<Violation> violations)
        {
            var comment = new PullRequestReviewCommentCreate(
                "Renamed files not supported.",
                file.PullRequest.LastCommitId,
                file.FileName,
                1);

            var addedComment = await Create(comment, file.PullRequest.Number);

            return new List<PullRequestReviewComment> { addedComment };
        }
    }
}