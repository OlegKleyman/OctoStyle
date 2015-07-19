namespace OctoStyle.Console
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Octokit;
    using Octokit.Internal;

    using OctoStyle.Core;
    using OctoStyle.Core.Borrowed;

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
                new InMemoryCredentialStore(new Credentials(arguments.Login, arguments.Password)));

            var pathResolver = new PathResolver(new FileSystemManager());

            var commentTasks = new List<Task<IEnumerable<PullRequestReviewComment>>>();

            var repository = new GitRepository(arguments.RepositoryOwner, arguments.Repository);

            var pullRequestRetriever = new PullRequestRetriver(client.PullRequest, repository);

            var pullRequest = pullRequestRetriever.Retrieve(arguments.PullRequestNumber).GetAwaiter().GetResult();

            foreach (var file in pullRequest.Files)
            {
                if (file.FileName.EndsWith(".cs", true, CultureInfo.InvariantCulture))
                {
                    var filePath = Path.Combine(arguments.SolutionDirectory, file.FileName);

                    var projectPath = pathResolver.GetPath(filePath, "*.csproj");

                    if (file.Status == GitPullRequestFileStatus.Modified)
                    {
                        var diffRetriever = new GitHubDiffRetriever(client.Connection, repository);

                        var analyzer = new CodeAnalyzer(projectPath);

                        var commenter = new ModifiedPullRequestCommenter(client.PullRequest.Comment, repository, diffRetriever);

                        commentTasks.Add(commenter.Create(file, analyzer, filePath));

                    }
                    else if (file.Status == GitPullRequestFileStatus.Added)
                    {
                        var analyzer = new CodeAnalyzer(projectPath);

                        var commenter = new AddedPullRequestCommenter(client.PullRequest.Comment, repository);

                        commentTasks.Add(commenter.Create(file, analyzer, filePath));
                    }
                    else if (file.Status == GitPullRequestFileStatus.Renamed)
                    {
                        var commenter = new RenamedPullRequestCommenter(client.PullRequest.Comment, repository);

                        commentTasks.Add(commenter.Create(file, null, filePath));
                    }
                    else if (file.Status == GitPullRequestFileStatus.Deleted)
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

            foreach (var comment in commentTasks)
            {
                comment.GetAwaiter().GetResult();
            }
        }
    }
}
