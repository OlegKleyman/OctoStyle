namespace OctoStyle.Core
{
    using System.Collections.Generic;

    public class VisualStudioCodeAnalyzer : ICodeAnalyzer
    {
        private readonly string solutionFilePath;

        public VisualStudioCodeAnalyzer(string solutionFilePath)
        {
            this.solutionFilePath = solutionFilePath;
        }

        public IEnumerable<GitHubStyleViolation> Analyze(string filePath)
        {
            throw new System.NotImplementedException();
        }
    }
}