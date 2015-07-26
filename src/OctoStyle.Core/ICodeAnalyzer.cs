namespace OctoStyle.Core
{
    using System.Collections.Generic;

    public interface ICodeAnalyzer
    {
        IEnumerable<GitHubStyleViolation> Analyze(string filePath);
    }
}