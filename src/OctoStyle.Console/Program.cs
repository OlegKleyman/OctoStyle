namespace OctoStyle.Console
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Octokit;
    using Octokit.Internal;

    using OctoStyle.Core;

    public class Program
    {
        public static void Main(string[] args)
        {
            Arguments arguments;

            try
            {
                arguments = Arguments.Parse(args);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            var client = new GitHubClient(
                new ProductHeaderValue("OctoStyle"),
                new InMemoryCredentialStore(
                    new Credentials(arguments.Login, "e5459d51eed64fd5e9f7ae947ca444d621aa386a")));

            var pullrequestInformationMessage = String.Format(
                CultureInfo.InvariantCulture,
                "{0}Owner: {1}, Repository: {2}, Number: {3}",
                Environment.NewLine,
                arguments.RepositoryOwner,
                arguments.Repository,
                arguments.PullRequestNumber);

            var commits =
                client.PullRequest.Commits(arguments.RepositoryOwner, arguments.Repository, arguments.PullRequestNumber)
                    .GetAwaiter()
                    .GetResult();

            if (commits == null)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "Unable to retrieve commits for pull request{0}",
                        pullrequestInformationMessage));
            }

            if (commits.Count == 0)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "No commits found for pull request{0}",
                        pullrequestInformationMessage));
            }

            var files =
                client.PullRequest.Files(arguments.RepositoryOwner, arguments.Repository, arguments.PullRequestNumber)
                    .GetAwaiter()
                    .GetResult();

            var analyzer = new CodeAnalyzer(arguments.SolutionDirectory);

            foreach (var file in files)
            {
                if (file.Status == "modified")
                {
                    
                }
                else if (file.Status == "added")
                {
                    var violations =
                        analyzer.Analyze(Path.Combine(arguments.SolutionDirectory, file.FileName).Replace("/", @"\"));
                    foreach (var violation in violations)
                    {
                        var message = String.Format(
                            CultureInfo.InvariantCulture,
                            "{0} - {1}",
                            violation.Rule.CheckId,
                            violation.Message);

                        var comment = new PullRequestReviewCommentCreate(message, commits.Last().Sha, file.FileName, violation.Line);

                        client.PullRequest.Comment.Create(
                            arguments.RepositoryOwner,
                            arguments.Repository,
                            arguments.PullRequestNumber,
                            comment).GetAwaiter().GetResult();
                    }
                }
                else if (file.Status == "renamed")
                {
                    if (file.Changes > 0)
                    {
                        var comment = new PullRequestReviewCommentCreate("Renamed files not supported", commits.Last().Sha, file.FileName, 1);

                        client.PullRequest.Comment.Create(
                            arguments.RepositoryOwner,
                            arguments.Repository,
                            arguments.PullRequestNumber,
                            comment).GetAwaiter().GetResult();
                    }
                }
                else if (file.Status == "deleted")
                {
                    //no work to do
                }
                else
                {
                    throw new InvalidOperationException(
                        String.Format(CultureInfo.InvariantCulture, "Unknown file status: {0}.", file.Status));
                }
            }
        }
    }
}
