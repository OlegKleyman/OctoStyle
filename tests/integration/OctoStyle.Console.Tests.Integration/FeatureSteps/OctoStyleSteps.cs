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
                    string.Format(CultureInfo.InvariantCulture, "{0} enviroment variable is missing.", loginKey));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "{0} enviroment variable is missing.", passwordKey));
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

            foreach (var comment in ScenarioContextExtended.Current.CreatedComments)
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
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1303:ConstFieldNamesMustBeginWithUpperCaseLetter", Justification = StyleCopConstants.LocalConstantJustification)]
        public void WhenIRunTheOctoStyle()
        {
            const string relativeSolutionDirectory = @"..\..\..\OctoStyleTest";
            var arguments = string.Format(
                CultureInfo.InvariantCulture,
                "-l {0} -p {1} -d {2} -o {3} -r {4} -pr {5}",
                FeatureContextExtended.Current.GitLogin,
                FeatureContextExtended.Current.GitPassword,
                Path.GetFullPath(relativeSolutionDirectory),
                FeatureContextExtended.Current.RepositoryOwner,
                FeatureContextExtended.Current.Repository,
                ScenarioContextExtended.Current.PullRequestNumber);

            Program.Main(arguments.Split(' '));

            ScenarioContextExtended.Current.CreatedComments = Program.CommentTasks;
        }

        [Then(@"there should be comments on the pull request on the lines of the found violations")]
        public void ThenThereShouldBeCommentsOnThePullRequestOnTheLinesOfTheFoundViolations()
        {
            var client = FeatureContextExtended.Current.GitClient;

            var comments =
                client.PullRequest.Comment.GetAll(
                    FeatureContextExtended.Current.RepositoryOwner,
                    FeatureContextExtended.Current.Repository,
                    ScenarioContextExtended.Current.PullRequestNumber).GetAwaiter().GetResult();

            var testClassComments =
                comments.Where(
                    comment => (comment.Path.EndsWith("TestClass.cs") && comment.Position >= 5 && comment.Position <= 9))
                    .ToList();

            var testClass2Comments =
                comments.Where(comment => (comment.Path.EndsWith("TestClass2.cs") && comment.Position == 1)).ToList();

            var testClass3Comments =
                comments.Where(comment => (comment.Path.EndsWith("TestClass3.cs") && comment.Position <= 9)).ToList();

            Assert.That(testClassComments.Count, Is.GreaterThanOrEqualTo(2));

            Assert.That(
                testClassComments.Any(
                    comment =>
                    comment.Body == "SA1600 - The method must have a documentation header." && comment.Position == 5));

            Assert.That(
                testClassComments.Any(
                    comment =>
                    comment.Body
                    == "SA1513 - Statements or elements wrapped in curly brackets must be followed by a blank line."
                    && comment.Position == 9));

            Assert.That(testClass2Comments.Count, Is.GreaterThanOrEqualTo(1));

            Assert.That(
                testClass2Comments.Any(
                    comment => comment.Body == "Renamed files not supported." && comment.Position == 1));

            Assert.That(testClass3Comments.Count, Is.GreaterThanOrEqualTo(8));

            Assert.That(
                testClass3Comments.Any(
                    comment =>
                    comment.Body
                    == "SA1633 - The file has no header, the header Xml is invalid, or the header is not located at the top of the file."
                    && comment.Position == 1));

            Assert.That(
                testClass3Comments.Any(
                    comment =>
                    comment.Body == "SA1200 - All using directives must be placed inside of the namespace."
                    && comment.Position == 1));

            Assert.That(
                testClass3Comments.Any(
                    comment =>
                    comment.Body == "SA1200 - All using directives must be placed inside of the namespace."
                    && comment.Position == 2));

            Assert.That(
                testClass3Comments.Any(
                    comment =>
                    comment.Body == "SA1200 - All using directives must be placed inside of the namespace."
                    && comment.Position == 3));

            Assert.That(
                testClass3Comments.Any(
                    comment =>
                    comment.Body == "SA1200 - All using directives must be placed inside of the namespace."
                    && comment.Position == 4));

            Assert.That(
                testClass3Comments.Any(
                    comment =>
                    comment.Body == "SA1200 - All using directives must be placed inside of the namespace."
                    && comment.Position == 5));

            Assert.That(
                testClass3Comments.Any(
                    comment =>
                    comment.Body == "SA1600 - The class must have a documentation header." && comment.Position == 9));

            Assert.That(
                testClass3Comments.Any(
                    comment =>
                    comment.Body == "SA1400 - The class must have an access modifier." && comment.Position == 9));
        }
    }
}