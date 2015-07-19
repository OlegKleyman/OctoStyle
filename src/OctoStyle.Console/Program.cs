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

            var pathResolver = new PathResolver(new FileSystemManager());

            var commentTasks = new List<Task<IEnumerable<PullRequestReviewComment>>>();
            var pullRequest = new GitHubPullRequest(arguments.PullRequestNumber, commits.Last().Sha, files);

            foreach (var file in files)
            {
                var pullRequestFile = new GitHubPullRequestFile(
                    file.FileName,
                    pullRequest,
                    (GitPullRequestFileStatus)Enum.Parse(typeof(GitPullRequestFileStatus), file.Status, true));

                if (file.FileName.EndsWith(".cs", true, CultureInfo.InvariantCulture))
                {
                    var filePath = Path.Combine(arguments.SolutionDirectory, file.FileName).Replace('/', '\\');

                    if (file.Status == "modified")
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

                        var modifiedFile = client.Connection.Get<string>(file.ContentsUrl, null, "application/vnd.github.v3.raw+json").GetAwaiter().GetResult().Body;

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

                        commentTasks.Add(commenter.Create(pullRequestFile, accessibleViolations));

                    }
                    else if (file.Status == "added")
                    {
                        var analyzer = new CodeAnalyzer(pathResolver.GetPath(filePath, "*.csproj"));

                        var violations = analyzer.Analyze(filePath);

                        var commenter = new ModifiedPullRequestCommenter(
                            client.PullRequest.Comment,
                            new GitRepository(arguments.RepositoryOwner, arguments.Repository));

                        commentTasks.Add(
                            commenter.Create(
                                pullRequestFile,
                                violations.Select(
                                    violation =>
                                    new GitHubStyleViolation(violation.Rule.CheckId, violation.Message, violation.Line))));
                    }
                    else if (file.Status == "renamed")
                    {
                        if (file.Changes > 0)
                        {
                            var commenter = new RenamedPullRequestCommenter(
                                client.PullRequest.Comment,
                                new GitRepository(arguments.RepositoryOwner, arguments.Repository));

                            commentTasks.Add(
                                commenter.Create(pullRequestFile, null));
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

            foreach (var comment in commentTasks)
            {
                comment.GetAwaiter().GetResult();
            }
        }
    }
}
