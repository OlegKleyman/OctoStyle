namespace OctoStyle.Core
{
    using System.Collections.Generic;

    using StyleCop;

    public interface ICodeAnalyzer
    {
        IEnumerable<GitHubStyleViolation> Analyze(string filePath);
    }
}