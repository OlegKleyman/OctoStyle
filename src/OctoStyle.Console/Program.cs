namespace OctoStyle.Console
{
    using System;
    using System.Globalization;

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

            var client = new GitHubClient(new ProductHeaderValue("OctoStyle"));
            
            var pullRequestUrlFormat = String.Format(
                CultureInfo.InvariantCulture,
                "https://api.github.com/repos/{0}/{1}/pulls/{2}",
                arguments.RepositoryOwner,
                arguments.Repository,
                arguments.PullRequestNumber);
            
            var rawDiff =
                client.Connection.Get<string>(
                    new Uri(pullRequestUrlFormat),
                    null,
                    "application/vnd.github.VERSION.diff",
                    true).GetAwaiter().GetResult();
        }
    }
}
