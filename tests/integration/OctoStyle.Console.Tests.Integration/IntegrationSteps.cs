namespace OctoStyle.Console.Tests.Integration
{
    using System;

    using TechTalk.SpecFlow;

    public class IntegrationSteps : Steps
    {
        public FeatureContextExtended FeatureContext { get; private set; }

        public IntegrationSteps()
        {
            this.FeatureContext = new FeatureContextExtended(TechTalk.SpecFlow.FeatureContext.Current);
        }
    }
}
