namespace OctoStyle.Console.Tests.Integration
{
    using System;
    using System.Collections.Generic;

    using TechTalk.SpecFlow;

    public class FeatureContextExtended
    {
        private static readonly FeatureContextExtended CurrentContext = new FeatureContextExtended(FeatureContext.Current);

        private readonly FeatureContext context;
        private const string RepositoryKey = "REPOSITORY";
        private const string RepositoryOwnerKey = "REPOSITORY_OWNER";

        public static FeatureContextExtended Current
        {
            get
            {
                return CurrentContext;
            }
        }

        public FeatureContextExtended(FeatureContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;
        }

        public string RepositoryOwner
        {
            get
            {
                if (!this.context.ContainsKey(RepositoryOwnerKey))
                {
                    throw new KeyNotFoundException(RepositoryOwnerKey);
                }

                return this.context.Get<string>(RepositoryOwnerKey);
            }
            set
            {
                this.context.Set(value, RepositoryOwnerKey);
            }
        }

        public string Repository
        {
            get
            {
                if (!this.context.ContainsKey(RepositoryKey))
                {
                    throw new KeyNotFoundException(RepositoryKey);
                }

                return this.context.Get<string>(RepositoryKey);
            }
            set
            {
                this.context.Set(value, RepositoryKey);
            }
        }
    }
}
