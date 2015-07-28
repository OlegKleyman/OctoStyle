namespace OctoStyle.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a code analyzer.
    /// </summary>
    public interface ICodeAnalyzer
    {
        /// <summary>
        /// Analyzes a code project.
        /// </summary>
        /// <param name="filePath">The code project file path.</param>
        /// <returns>
        /// A <see cref="IEnumerable{T}"/> of <see cref="GitHubStyleViolation"/> containing violations found with the code.
        /// </returns>
        IEnumerable<GitHubStyleViolation> Analyze(string filePath);
    }
}