namespace OctoStyle.Core
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Octokit;

    public class PullRequestRetriver : IPullRequestRetriever
    {
        private readonly IPullRequestsClient client;

        private readonly GitHubRepository repository;

        public PullRequestRetriver(IPullRequestsClient client, GitHubRepository repository)
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

        public async Task<GitHubPullRequest> Retrieve(int number)
        {
            var pullrequestInformationMessage = string.Format(
                CultureInfo.InvariantCulture,
                "{0}Owner: {1}, Repository: {2}, Number: {3}",
                Environment.NewLine,
                this.repository.Owner,
                this.repository.Name,
                number);

            var commits = await this.client.Commits(this.repository.Owner, this.repository.Name, number);

            if (commits.Count == 0)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "No commits found for pull request{0}",
                        pullrequestInformationMessage));
            }

            var files = await this.client.Files(this.repository.Owner, this.repository.Name, number);
            var pull = await this.client.Get(this.repository.Owner, this.repository.Name, number);

            return new GitHubPullRequest(
                number,
                commits.Last().Sha,
                files,
                new GitHubPullRequestBranches(pull.Head.Ref, pull.Base.Ref));
        }
    }
}