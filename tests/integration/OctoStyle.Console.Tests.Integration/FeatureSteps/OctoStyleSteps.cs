namespace OctoStyle.Console.Tests.Integration.FeatureSteps
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    using Octokit;
    using Octokit.Internal;

    using OctoStyle.Core;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class OctoStyleSteps : Steps
    {
        [BeforeFeature("octoStyle")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1303:ConstFieldNamesMustBeginWithUpperCaseLetter", Justification = StyleCopConstants.LocalConstantJustification)]
        public static void BeforeFeature()
        {
            const string loginKey = "OCTOSTYLE_LOGIN";
            const string passwordKey = "OCTOSTYLE_PASSWORD";

            var login = Environment.GetEnvironmentVariable(loginKey);
            var password = Environment.GetEnvironmentVariable(passwordKey);

            if (string.IsNullOrEmpty(login))
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "{0} environment variable is missing.", loginKey));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "{0} environment variable is missing.", passwordKey));
            }

            FeatureContextExtended.Current.GitLogin = login;
            FeatureContextExtended.Current.GitPassword = password;
            FeatureContextExtended.Current.RepositoryOwner = "OlegKleyman";
            FeatureContextExtended.Current.Repository = "OctoStyleTest";
            FeatureContextExtended.Current.GitClient = new GitHubClient(
                new ProductHeaderValue("IntegrationTests"),
                new InMemoryCredentialStore(new Credentials(login, password)));
        }

        [AfterScenario("pullRequest")]
        public static void CleanUpComments()
        {
            var client = FeatureContextExtended.Current.GitClient;

            foreach (var comment in ScenarioContextExtended.Current.CreatedComments)
            {
                client.PullRequest.Comment.Delete(
                    FeatureContextExtended.Current.RepositoryOwner,
                    FeatureContextExtended.Current.Repository,
                    comment.Id).GetAwaiter().GetResult();
            }
        }

        [Given(@"I have a pull request with stylistic problems")]
        public static void GivenIHaveAPullRequestWithStylisticProblems()
        {
            ScenarioContextExtended.Current.PullRequestNumber = 1;
        }

        [When(@"I run the OctoStyle using the (StyleCop|Roslyn) engine")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1303:ConstFieldNamesMustBeginWithUpperCaseLetter", Justification = StyleCopConstants.LocalConstantJustification)]
        public static void WhenIrunTheOctoStyle(AnalysisEngine engine)
        {
            const string relativeSolutionDirectory = @"..\..\..\OctoStyleTest";
            var arguments = string.Format(
                CultureInfo.InvariantCulture,
                "-l {0} -p {1} -d {2} -o {3} -r {4} -pr {5} -e {6}",
                FeatureContextExtended.Current.GitLogin,
                FeatureContextExtended.Current.GitPassword,
                Path.GetFullPath(relativeSolutionDirectory),
                FeatureContextExtended.Current.RepositoryOwner,
                FeatureContextExtended.Current.Repository,
                ScenarioContextExtended.Current.PullRequestNumber,
                engine);

            Program.Main(arguments.Split(' '));

            ScenarioContextExtended.Current.CreatedComments = Program.CommentTasks;
        }

        [Then(@"there should be comments on the pull request on the lines of the found violations")]
        public static void ThenThereShouldBeCommentsOnThePullRequestOnTheLinesOfTheFoundViolations(Table expectedComments)
        {
            var client = FeatureContextExtended.Current.GitClient;

            var comments =
                client.PullRequest.Comment.GetAll(
                    FeatureContextExtended.Current.RepositoryOwner,
                    FeatureContextExtended.Current.Repository,
                    ScenarioContextExtended.Current.PullRequestNumber).GetAwaiter().GetResult();

            var expected = expectedComments.CreateSet<PullRequestComment>().ToArray();

            Assert.That(comments.Count, Is.GreaterThanOrEqualTo(expected.Length));
            foreach (var expectedComment in expected)
            {
                Assert.That(
                    comments.Any(
                        comment =>
                        comment.Path == expectedComment.File && comment.Body == expectedComment.Message
                        && comment.Position == expectedComment.Position),
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Comment was not found:{0}File: {1}{0}Position: {2}{0}Message: {3}",
                        Environment.NewLine,
                        expectedComment.File,
                        expectedComment.Position,
                        expectedComment.Message));
            }
        }
    }
}