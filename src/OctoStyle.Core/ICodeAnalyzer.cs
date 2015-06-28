namespace OctoStyle.Core
{
    using System.Collections.Generic;

    using StyleCop;

    public interface ICodeAnalyzer
    {
        IEnumerable<Violation> Analyze(string filePath);
    }
}