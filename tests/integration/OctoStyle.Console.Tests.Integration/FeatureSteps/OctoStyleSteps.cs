namespace OctoStyle.Console.Tests.Integration.FeatureSteps
{
    using TechTalk.SpecFlow;

    [Binding]
    public class OctoStyleSteps : Steps
    {
        [Given(@"I have a pull request with stylistic problems")]
        public void GivenIHaveAPullRequestWithStylisticProblems()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I run the OctoStyle")]
        public void WhenIRunTheOctoStyle()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"there should be comments on the pull request on the lines of the found violations")]
        public void ThenThereShouldBeCommentsOnThePullRequestOnTheLinesOfTheFoundViolations()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
