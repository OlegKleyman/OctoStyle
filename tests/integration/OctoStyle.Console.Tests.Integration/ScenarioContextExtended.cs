namespace OctoStyle.Console.Tests.Integration
{
    using System;

    using TechTalk.SpecFlow;

    public class ScenarioContextExtended
    {
        public ScenarioContext Context { get; set; }

        public ScenarioContextExtended(ScenarioContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.Context = context;
        }
    }
}
