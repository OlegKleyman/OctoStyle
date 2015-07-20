namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Octokit;

    public class AddedPullRequestCommenter : PullRequestCommenter
    {
        public AddedPullRequestCommenter(IPullRequestReviewCommentsClient client, GitRepository repository)
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

            if (analyzer == null)
            {
                throw new ArgumentNullException("analyzer");
            }

            var comments = new List<PullRequestReviewComment>();

            foreach (var violation in analyzer.Analyze(physicalFilePath))
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