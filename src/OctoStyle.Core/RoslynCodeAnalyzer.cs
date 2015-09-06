namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.MSBuild;

    public class RoslynCodeAnalyzer : ICodeAnalyzer
    {
        private readonly string solutionFilePath;

        private readonly DiagnosticAnalyzer[] analyzers;

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

            this.solutionFilePath = solutionFilePath;
            this.analyzers = analyzers;
        }

        public IEnumerable<GitHubStyleViolation> Analyze(string filePath)
        {
            var workspace = MSBuildWorkspace.Create();

            var solution = workspace.OpenSolutionAsync(solutionFilePath).GetAwaiter().GetResult();

            var projectDiagnosticTasks = new List<Task<ImmutableArray<Diagnostic>>>();

            foreach (var project in solution.Projects)
            {
                if (project.Language != LanguageNames.CSharp)
                {
                    continue;
                }

                projectDiagnosticTasks.Add(GetProjectAnalyzerDiagnostics(GetAllAnalyzers(), project));
            }

            var diagnosticBuilder = ImmutableList.CreateBuilder<Diagnostic>();

            foreach (var task in projectDiagnosticTasks)
            {
                diagnosticBuilder.AddRange(task.ConfigureAwait(false).GetAwaiter().GetResult());
            }

            var violations =
                diagnosticBuilder.Where(
                    diagnostic =>
                    string.Compare(diagnostic.Location.SourceTree.FilePath, filePath, StringComparison.OrdinalIgnoreCase) == 0)
                    .Select(
                        diagnostic =>
                        new GitHubStyleViolation(
                            diagnostic.Id,
                            diagnostic.Descriptor.Description.ToString(CultureInfo.InvariantCulture),
                            diagnostic.Location.GetLineSpan().EndLinePosition.Line));
            
            return violations;
        }

        private static ImmutableArray<DiagnosticAnalyzer> GetAllAnalyzers()
        {
            Assembly assembly = typeof(StyleCop.Analyzers.NoCodeFixAttribute).Assembly;

            var diagnosticAnalyzerType = typeof(DiagnosticAnalyzer);

            List<DiagnosticAnalyzer> analyzers = new List<DiagnosticAnalyzer>();

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(diagnosticAnalyzerType) && !type.IsAbstract)
                {
                    analyzers.Add((DiagnosticAnalyzer)Activator.CreateInstance(type));
                }
            }

            return analyzers.ToImmutableArray();
        }

        private static async Task<ImmutableArray<Diagnostic>> GetProjectAnalyzerDiagnostics(ImmutableArray<DiagnosticAnalyzer> analyzers, Project project)
        {
            Compilation compilation = await project.GetCompilationAsync().ConfigureAwait(false);
            CompilationWithAnalyzers compilationWithAnalyzers = compilation.WithAnalyzers(analyzers);

            var allDiagnostics = await compilationWithAnalyzers.GetAllDiagnosticsAsync().ConfigureAwait(false);

            // We want analyzer diagnostics and analyzer exceptions
            return allDiagnostics.RemoveRange(compilation.GetDiagnostics());
        }
    }
}