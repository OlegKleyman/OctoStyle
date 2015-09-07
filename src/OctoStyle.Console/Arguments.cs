namespace OctoStyle.Console
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using NDesk.Options;

    using OctoStyle.Core;

    /// <summary>
    /// Represents arguments for the stylecop application.
    /// </summary>
    public class Arguments
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1303:ConstFieldNamesMustBeginWithUpperCaseLetter", Justification = StyleCopConstants.LocalConstantJustification)]
        private Arguments(
            string login,
            string password,
            string solutionDirectory,
            string repositoryOwner,
            string repository,
            int pullRequestNumber,
            AnalysisEngine engine)
        {
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (solutionDirectory == null)
            {
                throw new ArgumentNullException("solutionDirectory");
            }

            if (repositoryOwner == null)
            {
                throw new ArgumentNullException("repositoryOwner");
            }

            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            const string cannotBeEmptyMessage = "Cannot be empty";

            if (login.Length == 0)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "login");
            }

            if (password.Length == 0)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "password");
            }

            if (solutionDirectory.Length == 0)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "solutionDirectory");
            }

            if (repositoryOwner.Length == 0)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "repositoryOwner");
            }

            if (repository.Length == 0)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "repository");
            }

            this.Login = login;
            this.Password = password;
            this.SolutionDirectory = solutionDirectory;
            this.RepositoryOwner = repositoryOwner;
            this.Repository = repository;
            this.PullRequestNumber = pullRequestNumber;
            this.Engine = engine;
        }

        /// <summary>
        /// Gets <see cref="SolutionDirectory"/>.
        /// </summary>
        /// <value>The solution directory containing the target code to inspect.</value>
        public string SolutionDirectory { get; private set; }

        /// <summary>
        /// Gets <see cref="SolutionDirectory"/>.
        /// </summary>
        /// <value>The GitHub repository owner name.</value>
        public string RepositoryOwner { get; private set; }

        /// <summary>
        /// Gets <see cref="SolutionDirectory"/>.
        /// </summary>
        /// <value>The GitHub repository name.</value>
        public string Repository { get; private set; }

        /// <summary>
        /// Gets <see cref="SolutionDirectory"/>.
        /// </summary>
        /// <value>The pull request number to inspect.</value>
        public int PullRequestNumber { get; private set; }

        public AnalysisEngine Engine { get; }

        /// <summary>
        /// Gets <see cref="SolutionDirectory"/>.
        /// </summary>
        /// <value>The login to use for interfacing with the GitHub API.</value>
        public string Login { get; private set; }

        /// <summary>
        /// Gets <see cref="SolutionDirectory"/>.
        /// </summary>
        /// <value>The password to use when interfacing with the GitHub API.</value>
        public string Password { get; private set; }

        /// <summary>
        /// Retrieves application <see cref="Arguments"/>.
        /// </summary>
        /// <param name="args">The <see cref="IEnumerable{T}"/> of <see cref="string"/> containing application arguments.</param>
        /// <returns>The application <see cref="Arguments"/>.</returns>
        public static Arguments Parse(IEnumerable<string> args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            var login = default(string);
            var password = default(string);
            var solutionDirectory = default(string);
            var repositoryOwner = default(string);
            var repository = default(string);
            var pullRequestNumber = default(int);
            var engine = default(AnalysisEngine);

            var options = new OptionSet().Add("l=", l => login = l)
                .Add("p=", p => password = p)
                .Add("d=", d => solutionDirectory = d)
                .Add("o=", o => repositoryOwner = o)
                .Add("r=", r => repository = r)
                .Add(
                    "e=",
                    e =>
                        {
                            if (!Enum.TryParse(e, true, out engine))
                            {
                                throw new ArgumentException(
                                    FormattableString.Invariant(
                                        $"Engine must be {string.Join(", ", Enum.GetNames(typeof(AnalysisEngine)))}."),
                                    nameof(e));
                            }
                        }).Add(
                            "pr=",
                            pu =>
                                {
                                    if (
                                        !int.TryParse(
                                            pu,
                                            NumberStyles.Integer,
                                            CultureInfo.InvariantCulture,
                                            out pullRequestNumber))
                                    {
                                        throw new ArgumentException(
                                            "pu must be an integer referencing an active pull request",
                                            nameof(pu));
                                    }
                                });

            options.Parse(args);

            try
            {
                return new Arguments(
                    login,
                    password,
                    solutionDirectory,
                    repositoryOwner,
                    repository,
                    pullRequestNumber,
                    engine);
            }
            catch (ArgumentException ex)
            {
                var helpMessage = string.Format(
                    CultureInfo.InvariantCulture,
                    "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}",
                    Environment.NewLine,
                    "-l {Git Login}",
                    "-p {Git Password",
                    "-d {Solution Directory}",
                    "-o {Repository Owner}",
                    "-r {Repository}",
                    "-pr {Pull Request Number}");

                throw new ArgumentException(helpMessage, ex);
            }
        }
    }
}