namespace OctoStyle.Console.Tests.Integration.FeatureSteps
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    using Octokit;
    using Octokit.Internal;

    using TechTalk.SpecFlow;

    [Binding]
    public class OctoStyleSteps : Steps
    {
        [BeforeFeature("octoStyle")]
        public static void BeforeFeature()
        {
            const string loginKey = "OCTOSTYLE_LOGIN";
            const string passwordKey = "OCTOSTYLE_PASSWORD";

            var login = Environment.GetEnvironmentVariable(loginKey);
            var password = Environment.GetEnvironmentVariable(passwordKey);

            if (String.IsNullOrEmpty(login))
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "{0} enviroment variable is missing.", loginKey));
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "{0} enviroment variable is missing.", passwordKey));
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
        public void CleanUpComments()
        {
            var client = FeatureContextExtended.Current.GitClient;

            var comments = client.PullRequest.Comment.GetAll(
                FeatureContextExtended.Current.RepositoryOwner,
                FeatureContextExtended.Current.Repository,
                ScenarioContextExtended.Current.PullRequestNumber).GetAwaiter().GetResult();

            foreach (var comment in comments)
            {
                client.PullRequest.Comment.Delete(
                    FeatureContextExtended.Current.RepositoryOwner,
                    FeatureContextExtended.Current.Repository,
                    comment.Id).GetAwaiter().GetResult();
            }
        }

        [Given(@"I have a pull request with stylistic problems")]
        public void GivenIHaveAPullRequestWithStylisticProblems()
        {
            ScenarioContextExtended.Current.PullRequestNumber = 1;
        }

        [When(@"I run the OctoStyle")]
        public void WhenIRunTheOctoStyle()
        {
            const string relativeSolutionDirectory = @"..\..\..\..\..\Common\OctoStyleTest";
            var arguments = String.Format(
                CultureInfo.InvariantCulture,
                "-l {0} -p {1} -d {2} -o {3} -r {4} -pr {5}",
                FeatureContextExtended.Current.GitLogin,
                FeatureContextExtended.Current.GitPassword,
                Path.GetFullPath(relativeSolutionDirectory),
                FeatureContextExtended.Current.RepositoryOwner,
                FeatureContextExtended.Current.Repository,
                ScenarioContextExtended.Current.PullRequestNumber);
            
            Program.Main(arguments.Split(' '));
        }

        [Then(@"there should be comments on the pull request on the lines of the found violations")]
        public void ThenThereShouldBeCommentsOnThePullRequestOnTheLinesOfTheFoundViolations()
        {
            var client = FeatureContextExtended.Current.GitClient;

            var comments = client.PullRequest.Comment.GetAll(
                FeatureContextExtended.Current.RepositoryOwner,
                FeatureContextExtended.Current.Repository,
                ScenarioContextExtended.Current.PullRequestNumber).GetAwaiter().GetResult();

            var testClassComments =
                comments.Where(
                    comment => (comment.Path.EndsWith("TestClass.cs") && comment.Position > 3 && comment.Position < 12))
                    .ToList();

            var testClass2Comments =
                comments.Where(comment => (comment.Path.EndsWith("TestClass2.cs") && comment.Position == 9)).ToList();

            Assert.That(testClassComments.Count, Is.GreaterThanOrEqualTo(2));
            
            Assert.That(
                testClassComments.Any(
                    comment =>
                    comment.Body == "SA1600: The method must have a documentation header." && comment.Position == 5));

            Assert.That(
                testClassComments.Any(
                    comment =>
                    comment.Body
                    == "SA1513: Statements or elements wrapped in curly brackets must be followed by a blank line."
                    && comment.Position == 9));

            Assert.That(testClass2Comments.Count, Is.GreaterThanOrEqualTo(1));

            Assert.That(
                testClass2Comments.Any(
                    comment =>
                    comment.Body
                    == "SA1306: Variable names and private field names must start with a lower-case letter: TestVar."
                    && comment.Position == 9));
        }
    }
}
