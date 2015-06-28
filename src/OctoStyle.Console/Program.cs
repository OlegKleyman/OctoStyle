namespace OctoStyle.Console
{
    using System;

    using Octokit;

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

            var gitClient = new GitHubClient(new ProductHeaderValue("OctoStyle"));
            var pullRequest =
                gitClient.PullRequest.Get(arguments.RepositoryOwner, arguments.Repository, arguments.PullRequestNumber)
                    .GetAwaiter()
                    .GetResult();
        }
    }
}
