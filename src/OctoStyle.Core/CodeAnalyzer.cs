namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using StyleCop;

    public class CodeAnalyzer : ICodeAnalyzer
    {
        private readonly Queue<Violation> violations;
        private readonly CodeProject project;

        public CodeAnalyzer(string projectPath)
        {
            if (projectPath == null)
            {
                throw new ArgumentNullException("projectPath");
            }

            if (projectPath == String.Empty)
            {
                throw new ArgumentException("Cannot be empty.", "projectPath");
            }

            this.violations = new Queue<Violation>();
            this.project = new CodeProject(0, projectPath, new Configuration(null));
        }

        public IEnumerable<GitHubStyleViolation> Analyze(string filePath)
        {
            const string filePathparamName = "filePath";

            if (filePath == null)
            {
                throw new ArgumentNullException(filePathparamName);
            }

            if (filePath.Length == 0)
            {
                throw new ArgumentException("Cannot be empty", filePathparamName);
            }

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