namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Octokit;

    public class PullRequestBuilder : IPullRequestBuilder
    {
        private readonly IDiffParser parser;

        public PullRequestBuilder(IDiffParser parser)
        {
            if (parser == null)
            {
                throw new ArgumentNullException("parser");
            }

            this.parser = parser;
        }

        public GitHubPullRequest Build(
            int number,
            string lastCommitId,
            IReadOnlyList<PullRequestFile> files,
            string diff,
            GitHubPullRequestBranches branches)
        {
            if (files == null)
            {
                throw new ArgumentNullException("files");
            }

            var diffs = this.parser.Split(diff);

            var pullRequestFiles =
                files.Select(
                    file =>
                    new GitHubPullRequestFile(
                        file.FileName,
                        (GitHubPullRequestFileStatus)Enum.Parse(typeof(GitHubPullRequestFileStatus), file.Status, true),
                        file.Changes,
                        diffs[file.FileName])).ToList();

            var pullRequest = new GitHubPullRequest(number, lastCommitId, pullRequestFiles, diff, branches);

            return pullRequest;
        }
    }
}