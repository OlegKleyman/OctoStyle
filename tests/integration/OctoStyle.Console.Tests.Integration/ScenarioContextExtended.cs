namespace OctoStyle.Console.Tests.Integration
{
    using System;

    using TechTalk.SpecFlow;

    public class ScenarioContextExtended
    {
        private static readonly ScenarioContextExtended CurrentContext = new ScenarioContextExtended(ScenarioContext.Current);

        public static ScenarioContextExtended Current
        {
            get
            {
                return CurrentContext;
            }
        }

        private readonly ScenarioContext context;

        public ScenarioContextExtended(ScenarioContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;
        }
    }
}
