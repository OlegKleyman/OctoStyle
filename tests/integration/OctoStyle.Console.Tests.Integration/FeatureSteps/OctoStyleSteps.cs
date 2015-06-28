namespace OctoStyle.Console.Tests.Integration.FeatureSteps
{
    using System.Globalization;
    using System.IO;

    using TechTalk.SpecFlow;

    [Binding]
    public class OctoStyleSteps : Steps
    {
        [BeforeFeature("OctoStyle")]
        public static void BeforeFeature()
        {
            FeatureContextExtended.Current.RepositoryOwner = "OlegKleyman";
            FeatureContextExtended.Current.Repository = "OctoStyleTest";
        }

        [Given(@"I have a pull request with stylistic problems")]
        public void GivenIHaveAPullRequestWithStylisticProblems()
        {
            ScenarioContextExtended.Current.PullRequestNumber = 1;
        }

        [When(@"I run the OctoStyle")]
        public void WhenIRunTheOctoStyle()
        {
            const string relativeSolutionDirectory = @"..\..\..\..\..\Common\OctoStyle";
            Program.Main(new[] { Path.GetFullPath(relativeSolutionDirectory),
                                 FeatureContextExtended.Current.RepositoryOwner,
                                 FeatureContextExtended.Current.Repository,
                                 ScenarioContextExtended.Current.PullRequestNumber.ToString(CultureInfo.InvariantCulture) });
        }

        [Then(@"there should be comments on the pull request on the lines of the found violations")]
        public void ThenThereShouldBeCommentsOnThePullRequestOnTheLinesOfTheFoundViolations()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
