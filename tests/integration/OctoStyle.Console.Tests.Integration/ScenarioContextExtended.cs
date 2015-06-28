namespace OctoStyle.Console.Tests.Integration
{
    using System;
    using System.Collections.Generic;

    using TechTalk.SpecFlow;

    public class ScenarioContextExtended
    {
        private const string PullRequestNumberKey = "PULL_REQUEST_NUMBER";

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

        public int PullRequestNumber
        {
            get
            {
                if (!this.context.ContainsKey(PullRequestNumberKey))
                {
                    throw new KeyNotFoundException(PullRequestNumberKey);
                }

                return this.context.Get<int>(PullRequestNumberKey);
            }
            set
            {
                this.context.Set(value, PullRequestNumberKey);
            }
        }
    }
}
