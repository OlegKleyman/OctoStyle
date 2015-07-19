namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Octokit;

    public class ModifiedPullRequestCommenter : PullRequestCommenter
    {
        private readonly IGitHubDiffRetriever diffRetriever;

        public ModifiedPullRequestCommenter(
            IPullRequestReviewCommentsClient client,
            GitRepository repository,
            IGitHubDiffRetriever diffRetriever)
            : base(client, repository)
        {
            if (diffRetriever == null)
            {
                throw new ArgumentNullException("diffRetriever");
            }

            this.diffRetriever = diffRetriever;
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

            var diff =
                diffRetriever.RetrieveAsync(
                    file.FileName,
                    file.PullRequest.Branches.Branch,
                    file.PullRequest.Branches.MergeBranch).GetAwaiter().GetResult().OfType<ModificationGitDiffEntry>();

            var violations = analyzer.Analyze(physicalFilePath);

            var accessibleViolations = diff.Join(
                violations,
                entry => entry.LineNumber,
                violation => violation.Position,
                (entry, violation) => new GitHubStyleViolation(violation.RuleId, violation.Message, entry.Position));

            var comments = new List<PullRequestReviewComment>();

            foreach (var violation in accessibleViolations)
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