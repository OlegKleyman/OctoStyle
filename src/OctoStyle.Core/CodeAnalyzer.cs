namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using StyleCop;

    /// <summary>
    /// Represents a style cop code analyzer.
    /// </summary>
    public class CodeAnalyzer : ICodeAnalyzer
    {
        private readonly CodeProject project;

        private readonly Queue<Violation> violations;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeAnalyzer"/> class.
        /// </summary>
        /// <param name="projectPath">The path of the project to analyze.</param>
        /// <exception cref="ArgumentException">The projectPath argument is an empty string.</exception>
        /// <exception cref="ArgumentNullException">The projectPath argument is an empty string.</exception>
        public CodeAnalyzer(string projectPath)
        {
            if (projectPath == null)
            {
                throw new ArgumentNullException("projectPath");
            }

            if (projectPath == string.Empty)
            {
                throw new ArgumentException("Cannot be empty.", "projectPath");
            }

            this.violations = new Queue<Violation>();
            this.project = new CodeProject(0, projectPath, new Configuration(null));
        }

        /// <summary>
        /// Analyzes a code project.
        /// </summary>
        /// <param name="filePath">The code project file path.</param>
        /// <returns>
        /// A <see cref="IEnumerable{T}"/> of <see cref="GitHubStyleViolation"/> containing violations found with the code.
        /// </returns>
        public IEnumerable<GitHubStyleViolation> Analyze(string filePath)
        {
            this.violations.Clear();
            var console = new StyleCopConsole(null, false, null, null, true);

            console.Core.Environment.AddSourceCode(this.project, filePath, null);
            console.ViolationEncountered += this.OnViolationEncountered;

            console.Start(new[] { this.project }, true);

            return
                this.violations.Select(
                    violation => new GitHubStyleViolation(violation.Rule.CheckId, violation.Message, violation.Line));
        }

        private void OnViolationEncountered(object sender, ViolationEventArgs e)
        {
            this.violations.Enqueue(e.Violation);
        }
    }
}