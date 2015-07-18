namespace OctoStyle.Core
{
    using System;

    public class GitHubPullRequest
    {
        public int Number { get; private set; }

        public string LastCommitId { get; private set; }

        public GitHubPullRequest(int number, string lastCommitId)
        {
            if (lastCommitId == null)
            {
                throw new ArgumentNullException("lastCommitId");
            }

            if (lastCommitId.Length == 0)
            {
                throw new ArgumentException("Cannot be empty", "lastCommitId");
            }

            if (number < 1)
            {
                throw new ArgumentException("Must be greater than 0", "number");
            }

            this.Number = number;
            this.LastCommitId = lastCommitId;
        }
    }
}