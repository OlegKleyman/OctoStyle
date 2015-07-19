namespace OctoStyle.Console.Tests.Integration
{
    using System;
    using System.Collections.Generic;

    using Octokit;

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

        private const string CreatedCommentsKey = "CREATED_COMMENTS";

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

        public IEnumerable<PullRequestReviewComment> CreatedComments
        {
            get
            {
                if (!this.context.ContainsKey(CreatedCommentsKey))
                {
                    throw new KeyNotFoundException(CreatedCommentsKey);
                }

                return this.context.Get<IEnumerable<PullRequestReviewComment>>(CreatedCommentsKey);
            }
            set
            {
                this.context.Set(value, CreatedCommentsKey);
            }
        }
    }
}
