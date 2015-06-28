namespace OctoStyle.Console.Tests.Integration
{
    using System;
    using System.Collections.Generic;

    using TechTalk.SpecFlow;

    public class FeatureContextExtended : FeatureContext
    {
        private static FeatureContextExtended context;

        private readonly FeatureContext currentContext;
        private const string RepositoryKey = "REPOSITORY";
        private const string RepositoryOwnerKey = "REPOSITORY_OWNER";

        public static new FeatureContextExtended Current
        {
            get
            {
                return context ?? (context = new FeatureContextExtended(FeatureContext.Current));
            }
        }

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

        public string Repository
        {
            get
            {
                if (!this.currentContext.ContainsKey(RepositoryKey))
                {
                    throw new KeyNotFoundException(RepositoryKey);
                }

                return this.currentContext.Get<string>(RepositoryKey);
            }
            set
            {
                this.currentContext.Set(value, RepositoryKey);
            }
        }
    }
}
