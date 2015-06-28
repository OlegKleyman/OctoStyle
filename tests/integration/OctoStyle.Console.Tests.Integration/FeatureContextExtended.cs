namespace OctoStyle.Console.Tests.Integration
{
    using System;
    using System.Collections.Generic;

    using Octokit;

    using TechTalk.SpecFlow;

    public class FeatureContextExtended
    {
        private static readonly FeatureContextExtended CurrentContext = new FeatureContextExtended(FeatureContext.Current);

        private readonly FeatureContext context;

        private const string GitClientKey = "GIT_CLIENT";
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

        public GitHubClient GitClient
        {
            get
            {
                if (!this.context.ContainsKey(GitClientKey))
                {
                    throw new KeyNotFoundException(RepositoryKey);
                }

                return this.context.Get<GitHubClient>(GitClientKey);
            }
            set
            {
                this.context.Set(value, GitClientKey);
            }
        }
    }
}
