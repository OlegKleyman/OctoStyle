namespace OctoStyle.Console
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Microsoft.CodeAnalysis.Diagnostics;

    using Octokit;
    using Octokit.Internal;

    using OctoStyle.Core;

    /// <summary>
    /// Contains application interface members.
    /// </summary>
    public static class Program
    {
        private static readonly Lazy<List<IEnumerable<PullRequestReviewComment>>> Comments =
            new Lazy<List<IEnumerable<PullRequestReviewComment>>>(() => new List<IEnumerable<PullRequestReviewComment>>());

        /// <summary>
        /// Gets <see cref="CommentTasks"/>.
        /// </summary>
        /// <value>The pull request comments made during the <see cref="Main"/> method run.</value>
        public static IEnumerable<PullRequestReviewComment> CommentTasks
        {
            get
            {
                return Comments.Value.SelectMany(comments =>
                    {
                        var pullRequestReviewComments = comments as PullRequestReviewComment[] ?? comments.ToArray();
                        return pullRequestReviewComments;
                    });
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
            
            var pullRequest = pullRequestRetriever.RetrieveAsync(arguments.PullRequestNumber).GetAwaiter().GetResult();

            foreach (var file in pullRequest.Files)
            {
                if (file.FileName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                {
                    var filePath = Path.Combine(arguments.SolutionDirectory, file.FileName);
                    
                    var diffRetriever = new GitHubDiffRetriever(new DifferWrapper(), new GitDiffEntryFactory());

                    var factory = new PullRequestCommenterFactory(client.PullRequest.Comment, repository, diffRetriever);

                    var analyzerAssembly = Assembly.LoadFrom("StyleCop.Analyzers.dll");

                    var analyzers =
                        new List<DiagnosticAnalyzer>(
                            analyzerAssembly.GetTypes()
                                .Where(type => type.IsSubclassOf(typeof(DiagnosticAnalyzer)) && !type.IsAbstract)
                                .Select(type => (DiagnosticAnalyzer)Activator.CreateInstance(type)));

                    var analyzerFactory = new CodeAnalyzerFactory(pathResolver);

                    var analyzer = analyzerFactory.GetAnalyzer(arguments.Engine, filePath, analyzers.ToArray());

                    Comments.Value.Add(
                        factory.GetCommenter(file.Status)
                            .Create(pullRequest, file, analyzer, filePath)
                            .GetAwaiter()
                            .GetResult());
                }
            }
        }
    }
}