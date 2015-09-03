namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;

    public class VisualStudioCodeAnalyzer : ICodeAnalyzer
    {
        private readonly string solutionFilePath;

        public VisualStudioCodeAnalyzer(string solutionFilePath)
        {
            if (solutionFilePath == null)
            {
                throw new ArgumentNullException(nameof(solutionFilePath));
            }

            if (solutionFilePath.Length == 0)
            {
                throw new ArgumentException("Cannot be empty.", nameof(solutionFilePath));
            }

            this.solutionFilePath = solutionFilePath;
        }

        public IEnumerable<GitHubStyleViolation> Analyze(string filePath)
        {
            throw new System.NotImplementedException();
        }
    }
}