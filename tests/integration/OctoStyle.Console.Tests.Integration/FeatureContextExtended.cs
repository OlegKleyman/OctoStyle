namespace OctoStyle.Console.Tests.Integration
{
    using System;
    using System.Collections.Generic;

    using Octokit;

    using TechTalk.SpecFlow;

    public class FeatureContextExtended
    {
        private const string GitClientKey = "GIT_CLIENT";

        private const string RepositoryKey = "REPOSITORY";

        private const string RepositoryOwnerKey = "REPOSITORY_OWNER";

        private const string GitLoginKey = "GIT_LOGIN";

        private const string GitPasswordKey = "GIT_PASSWORD";

        private static readonly FeatureContextExtended CurrentContext =
            new FeatureContextExtended(FeatureContext.Current);

        private readonly FeatureContext context;

        public FeatureContextExtended(FeatureContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;
        }

        public static FeatureContextExtended Current
        {
            get
            {
                return CurrentContext;
            }
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

        public string GitLogin
        {
            get
            {
                if (!this.context.ContainsKey(GitLoginKey))
                {
                    throw new KeyNotFoundException(GitLoginKey);
                }

                return this.context.Get<string>(GitLoginKey);
            }

            set
            {
                this.context.Set(value, GitLoginKey);
            }
        }

        public string GitPassword
        {
            get
            {
                if (!this.context.ContainsKey(GitPasswordKey))
                {
                    throw new KeyNotFoundException(GitPasswordKey);
                }

                return this.context.Get<string>(GitPasswordKey);
            }

            set
            {
                this.context.Set(value, GitPasswordKey);
            }
        }

        public GitHubClient GitClient
        {
            get
            {
                if (!this.context.ContainsKey(GitClientKey))
                {
                    throw new KeyNotFoundException(GitClientKey);
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