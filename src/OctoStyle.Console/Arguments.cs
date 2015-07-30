namespace OctoStyle.Console
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using NDesk.Options;

    using OctoStyle.Core;

    public class Arguments
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1303:ConstFieldNamesMustBeginWithUpperCaseLetter", Justification = StyleCopConstants.LocalConstantJustification)]
        private Arguments(
            string login,
            string password,
            string solutionDirectory,
            string repositoryOwner,
            string repository,
            int pullRequestNumber)
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

            if (login == string.Empty)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "login");
            }

            if (password == string.Empty)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "password");
            }

            if (solutionDirectory == string.Empty)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "solutionDirectory");
            }

            if (repositoryOwner == string.Empty)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "repositoryOwner");
            }

            if (repository == string.Empty)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "repository");
            }

            this.Login = login;
            this.Password = password;
            this.SolutionDirectory = solutionDirectory;
            this.RepositoryOwner = repositoryOwner;
            this.Repository = repository;
            this.PullRequestNumber = pullRequestNumber;
        }

        public string SolutionDirectory { get; private set; }

        public string RepositoryOwner { get; private set; }

        public string Repository { get; private set; }

        public int PullRequestNumber { get; private set; }

        public string Login { get; private set; }

        public string Password { get; set; }

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

            var options =
                new OptionSet().Add("l=", l => login = l)
                    .Add("p=", p => password = p)
                    .Add("d=", d => solutionDirectory = d)
                    .Add("o=", o => repositoryOwner = o)
                    .Add("r=", r => repository = r)
                    .Add(
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
                                        "pu must be an integer referencing an active pull request");
                                }
                            });
            options.Parse(args);

            try
            {
                return new Arguments(login, password, solutionDirectory, repositoryOwner, repository, pullRequestNumber);
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