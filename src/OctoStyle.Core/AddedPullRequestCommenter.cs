namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Octokit;

    using StyleCop;

    public class AddedPullRequestCommenter : PullRequestCommenter
    {
        public AddedPullRequestCommenter(IPullRequestReviewCommentsClient client, GitRepository repository)
            : base(client, repository)
        {
        }

        public async override Task<IEnumerable<PullRequestReviewComment>> Create(GitHubPullRequestFile file, IEnumerable<GitHubStyleViolation> violations)
        {
            if (violations == null)
            {
                throw new ArgumentNullException("violations");
            }

            var comments = new List<PullRequestReviewComment>();

            foreach (var violation in violations)
            {
                var message = String.Format(
                                CultureInfo.InvariantCulture,
                                "{0} - {1}",
                                violation.RuleId,
                                violation.Message);

                var comment = new PullRequestReviewCommentCreate(
                    message,
                    file.PullRequest.LastCommitId,
                    file.FileName,
                    violation.Position);

                comments.Add(await Create(comment, file.PullRequest.Number));
            }

            return comments;
        }
    }
}