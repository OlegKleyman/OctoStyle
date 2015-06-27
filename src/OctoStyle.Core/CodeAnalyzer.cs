namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;

    using StyleCop;

    public class CodeAnalyzer
    {
        private readonly string projectPath;

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

            this.projectPath = projectPath;
        }

        public IEnumerable<Violation> Analyze(string filePath, int startLine, int endLine)
        {
            throw new System.NotImplementedException();
        }
    }
}