namespace OctoStyle.Console.Tests.Integration
{
    using System;

    using TechTalk.SpecFlow;

    public class ScenarioContextExtended
    {
        private static readonly ScenarioContextExtended context = new ScenarioContextExtended(ScenarioContext.Current);

        public static ScenarioContextExtended Current
        {
            get
            {
                return context;
            }
        }

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
