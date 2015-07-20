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

        private readonly GitRepository repository;

        public PullRequestRetriver(IPullRequestsClient client, GitRepository repository)
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
            var pullrequestInformationMessage = String.Format(
                CultureInfo.InvariantCulture,
                "{0}Owner: {1}, Repository: {2}, Number: {3}",
                Environment.NewLine,
                repository.Owner,
                repository.Name,
                number);

            var commits = await client.Commits(repository.Owner, repository.Name, number);
            
            if (commits.Count == 0)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "No commits found for pull request{0}",
                        pullrequestInformationMessage));
            }

            var files = await client.Files(repository.Owner, repository.Name, number);
            var pull = await client.Get(repository.Owner, repository.Name, number);

            return new GitHubPullRequest(number, commits.Last().Sha, files, new GitHubPullRequestBranches(pull.Head.Ref, pull.Base.Ref));
        }
    }
}