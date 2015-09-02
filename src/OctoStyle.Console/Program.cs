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

    /// <summary>
    /// Contains application interface members.
    /// </summary>
    public static class Program
    {
        private static readonly Lazy<List<Task<IEnumerable<PullRequestReviewComment>>>> Comments =
            new Lazy<List<Task<IEnumerable<PullRequestReviewComment>>>>(() => new List<Task<IEnumerable<PullRequestReviewComment>>>());

        /// <summary>
        /// Gets <see cref="CommentTasks"/>.
        /// </summary>
        /// <value>The pull request comments made during the <see cref="Main"/> method run.</value>
        public static IEnumerable<PullRequestReviewComment> CommentTasks
        {
            get
            {
                return Comments.Value.SelectMany(task => task.GetAwaiter().GetResult());
            }
        }

        /// <summary>
        /// The entry method into the application.
        /// </summary>
        /// <param name="args">The application arguments.</param>
        public static void Main(string[] args)
        {
            Comments.Value.Clear();

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

            var repository = new GitHubRepository(arguments.RepositoryOwner, arguments.Repository);

            var builder = new PullRequestBuilder(new DiffParser());
            var pullRequestRetriever = new PullRequestRetriever(builder, client.PullRequest, client.Connection, repository);
            var con = new Connection(new ProductHeaderValue("OctoStyle"),
                new InMemoryCredentialStore(new Credentials(arguments.Login, arguments.Password)));
            
            var pullRequest = pullRequestRetriever.RetrieveAsync(arguments.PullRequestNumber).GetAwaiter().GetResult();

            foreach (var file in pullRequest.Files)
            {
                if (file.FileName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                {
                    var filePath = Path.Combine(arguments.SolutionDirectory, file.FileName);

                    var projectPath = pathResolver.GetPath(filePath, "*.csproj");
                    var diffRetriever = new GitHubDiffRetriever(new DifferWrapper(), new GitDiffEntryFactory());

                    var factory = new PullRequestCommenterFactory(client.PullRequest.Comment, repository, diffRetriever);
                    var analyzer = new CodeAnalyzer(projectPath);

                    Comments.Value.Add(factory.GetCommenter(file.Status).Create(pullRequest, file, analyzer, filePath));
                }
            }

            Comments.Value.ForEach(task => task.GetAwaiter().GetResult());
        }
    }
}