namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.Linq;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.MSBuild;

    public class RoslynCodeAnalyzer : ICodeAnalyzer
    {
        private readonly string solutionFilePath;

        private readonly ImmutableArray<DiagnosticAnalyzer> analyzers;

        public RoslynCodeAnalyzer(string solutionFilePath, params DiagnosticAnalyzer[] analyzers)
        {
            if (solutionFilePath == null)
            {
                throw new ArgumentNullException(nameof(solutionFilePath));
            }

            if (analyzers == null)
            {
                throw new ArgumentNullException(nameof(analyzers));
            }

            if (solutionFilePath.Length == 0)
            {
                throw new ArgumentException("Cannot be empty.", nameof(solutionFilePath));
            }

            if (analyzers.Length == 0)
            {
                throw new ArgumentException("Cannot be empty.", nameof(analyzers));
            }

            this.solutionFilePath = solutionFilePath;
            this.analyzers = analyzers.ToImmutableArray();
        }

        public IEnumerable<GitHubStyleViolation> Analyze(string filePath)
        {
            var workspace = MSBuildWorkspace.Create();

            var solution = workspace.OpenSolutionAsync(this.solutionFilePath).GetAwaiter().GetResult();

            var projectDiagnosticTasks = new List<Diagnostic>();

            foreach (var project in solution.Projects.Where(project => project.Language == LanguageNames.CSharp))
            {
                var compilation = project.GetCompilationAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                var compilationWithAnalyzers = compilation.WithAnalyzers(this.analyzers);

                var allDiagnostics =
                    compilationWithAnalyzers.GetAllDiagnosticsAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                projectDiagnosticTasks.AddRange(allDiagnostics.RemoveRange(compilation.GetDiagnostics()));
            }

            const int lineOffsetCorrection = 1;

            var violations =
                projectDiagnosticTasks.Where(
                    diagnostic => diagnostic.Location.SourceTree != null && string.Compare(
                        diagnostic.Location.SourceTree.FilePath,
                        filePath.Replace('/', '\\'),
                        StringComparison.OrdinalIgnoreCase) == 0)
                    .Select(
                        diagnostic =>
                        new GitHubStyleViolation(
                            diagnostic.Id,
                            diagnostic.Descriptor.Description.ToString(CultureInfo.InvariantCulture),
                            diagnostic.Location.GetLineSpan().EndLinePosition.Line + lineOffsetCorrection)).ToList();

            return violations;
        }
    }
}