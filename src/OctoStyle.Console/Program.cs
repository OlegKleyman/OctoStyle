namespace OctoStyle.Console
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;

    using Octokit;
    using Octokit.Internal;

    using OctoStyle.Core;

    public class Program
    {
        public static readonly List<Task<IEnumerable<PullRequestReviewComment>>> CommentTasks =
            new List<Task<IEnumerable<PullRequestReviewComment>>>();

        public static void Main(string[] args)
        {
            CommentTasks.Clear();

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
                new InMemoryCredentialStore(new Credentials(arguments.Login, arguments.Password)));

            var pathResolver = new PathResolver(new FileSystemManager());

            var repository = new GitRepository(arguments.RepositoryOwner, arguments.Repository);

            var pullRequestRetriever = new PullRequestRetriver(client.PullRequest, repository);

            var pullRequest = pullRequestRetriever.Retrieve(arguments.PullRequestNumber).GetAwaiter().GetResult();

            foreach (var file in pullRequest.Files)
            {
                if (file.FileName.EndsWith(".cs", true, CultureInfo.InvariantCulture))
                {
                    var filePath = Path.Combine(arguments.SolutionDirectory, file.FileName);

                    var projectPath = pathResolver.GetPath(filePath, "*.csproj");
                    var diffRetriever = new GitHubDiffRetriever(client.Connection, repository);

                    var factory = new PullRequestCommenterFactory(client.PullRequest.Comment, repository, diffRetriever);
                    var analyzer = new CodeAnalyzer(projectPath);

                    CommentTasks.Add(factory.Get(file.Status).Create(file, analyzer, filePath));
                }
            }

            CommentTasks.ForEach(task => task.GetAwaiter().GetResult());
        }
    }
}