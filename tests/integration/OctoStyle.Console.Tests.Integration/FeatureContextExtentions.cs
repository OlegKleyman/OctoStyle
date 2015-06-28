namespace OctoStyle.Console.Tests.Integration
{
    using System;
    using System.Collections.Generic;

    using TechTalk.SpecFlow;

    public class FeatureContextExtended : FeatureContext
    {
        private const string RepositoryOwnerKey = "REPOSITORY_OWNER";

        private readonly FeatureContext currentContext;

        public FeatureContextExtended(FeatureContext context)
            : base(context.FeatureInfo, context.BindingCulture)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.currentContext = context;
        }

        public string RepositoryOwner
        {
            get
            {
                if (!this.currentContext.ContainsKey(RepositoryOwnerKey))
                {
                    throw new KeyNotFoundException(RepositoryOwnerKey);
                }

                return this.currentContext.Get<string>(RepositoryOwnerKey);
            }
            set
            {
                this.currentContext.Set(value, RepositoryOwnerKey);
            }
        }
    }
}
