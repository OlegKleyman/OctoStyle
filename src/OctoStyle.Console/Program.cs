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
                new InMemoryCredentialStore(
                    new Credentials(arguments.Login, arguments.Password)));

            var pathResolver = new PathResolver(new FileSystemManager());

            var commentTasks = new List<Task<IEnumerable<PullRequestReviewComment>>>();

            var pullRequestRetriever = new PullRequestRetriver(
                client.PullRequest,
                new GitRepository(arguments.RepositoryOwner, arguments.Repository));

            var pullRequest = pullRequestRetriever.Retrieve(arguments.PullRequestNumber).GetAwaiter().GetResult();
            
            foreach (var file in pullRequest.Files)
            {
                if (file.FileName.EndsWith(".cs", true, CultureInfo.InvariantCulture))
                {
                    var filePath = Path.Combine(arguments.SolutionDirectory, file.FileName).Replace('/', '\\');

                    if (file.Status == GitPullRequestFileStatus.Modified)
                    {
                        var originalFile = client.Repository.Content.GetAllContents(
                            arguments.RepositoryOwner,
                            arguments.Repository,
                            file.FileName).GetAwaiter().GetResult();

                        if (originalFile == null || originalFile.Count == 0)
                        {
                            throw new InvalidOperationException(
                                String.Format(
                                    CultureInfo.InvariantCulture,
                                    "Unable to retrieve original file for modified file: {0}",
                                    file.FileName));
                        }

                        if (originalFile.Count > 1)
                        {
                            throw new InvalidOperationException(
                                String.Format(
                                    CultureInfo.InvariantCulture,
                                    "Retrieving original file returned multiple results. File: {0}",
                                    file.FileName));
                        }

                        var modifiedFile = client.Connection.Get<string>(file.ContentUri, null, "application/vnd.github.v3.raw+json").GetAwaiter().GetResult().Body;

                        if (modifiedFile == null)
                        {
                            throw new InvalidOperationException(
                                String.Format(
                                    CultureInfo.InvariantCulture,
                                    "Unale to retrieve modified pull request File: {0}",
                                    file.FileName));
                        }

                        const char githubNewlineDelimeter = '\n';
                        var diff =
                            Diff
                                .CreateDiff(originalFile.First()
                                    .Content.Split(new[] { githubNewlineDelimeter }, StringSplitOptions.None),
                                modifiedFile.Split(new[] { githubNewlineDelimeter }, StringSplitOptions.None))
                                .ToGitDiff(new GitDiffEntryFactory())
                                .OfType<ModificationGitDiffEntry>();

                        var analyzer = new CodeAnalyzer(pathResolver.GetPath(filePath, "*.csproj"));
                        var violations =
                            analyzer.Analyze(
                                filePath);

                        var accessibleViolations = diff.Join(
                            violations,
                            entry => entry.LineNumber,
                            violation => violation.Line,
                            (entry, violation) =>
                            new GitHubStyleViolation(violation.Rule.CheckId, violation.Message, entry.Position));

                        var commenter = new ModifiedPullRequestCommenter(
                            client.PullRequest.Comment,
                            new GitRepository(arguments.RepositoryOwner, arguments.Repository));

                        commentTasks.Add(commenter.Create(file, accessibleViolations));

                    }
                    else if (file.Status == GitPullRequestFileStatus.Added)
                    {
                        var analyzer = new CodeAnalyzer(pathResolver.GetPath(filePath, "*.csproj"));

                        var violations = analyzer.Analyze(filePath);

                        var commenter = new ModifiedPullRequestCommenter(
                            client.PullRequest.Comment,
                            new GitRepository(arguments.RepositoryOwner, arguments.Repository));

                        commentTasks.Add(
                            commenter.Create(
                                file,
                                violations.Select(
                                    violation =>
                                    new GitHubStyleViolation(violation.Rule.CheckId, violation.Message, violation.Line))));
                    }
                    else if (file.Status == GitPullRequestFileStatus.Renamed)
                    {
                        if (file.Changes > 0)
                        {
                            var commenter = new RenamedPullRequestCommenter(
                                client.PullRequest.Comment,
                                new GitRepository(arguments.RepositoryOwner, arguments.Repository));

                            commentTasks.Add(
                                commenter.Create(file, null));
                        }
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
