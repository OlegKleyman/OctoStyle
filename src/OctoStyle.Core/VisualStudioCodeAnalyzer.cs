namespace OctoStyle.Core
{
    using System.Collections.Generic;

    public class VisualStudioCodeAnalyzer : ICodeAnalyzer
    {
        public VisualStudioCodeAnalyzer(string solutionFilePath)
        {
        }

        public IEnumerable<GitHubStyleViolation> Analyze(string filePath)
        {
            throw new System.NotImplementedException();
        }
    }
}