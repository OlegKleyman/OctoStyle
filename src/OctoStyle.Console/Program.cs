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

            var files =
                client.PullRequest.Files(arguments.RepositoryOwner, arguments.Repository, arguments.PullRequestNumber)
                    .GetAwaiter()
                    .GetResult();

            foreach (var file in files)
            {
                if (file.Status == "modified")
                {
                    
                }
                else if (file.Status == "added")
                {
                    
                }
                else if (file.Status == "renamed")
                {
                    
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
