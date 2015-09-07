namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Microsoft.CodeAnalysis.Diagnostics;

    public class CodeAnalyzerFactory : ICodeAnalyzerFactory
    {
        private readonly IPathResolver pathResolver;

        public CodeAnalyzerFactory(IPathResolver pathResolver)
        {
            if (pathResolver == null)
            {
                throw new ArgumentNullException(nameof(pathResolver));
            }

            this.pathResolver = pathResolver;
        }

        public ICodeAnalyzer GetAnalyzer(AnalysisEngine engine, string path, params DiagnosticAnalyzer[] analyzers)
        {
            ICodeAnalyzer analyzer;

            switch (engine)
            {
                case AnalysisEngine.StyleCop:
                    analyzer = new StyleCopCodeAnalyzer(this.pathResolver.GetDirectoryPath(path, "*.csproj"));
                    break;
                case AnalysisEngine.Roslyn:
                    var solutionFiles = this.pathResolver.GetFilePaths(path, "*.sln");
                    var solutionFilesEnumerated = solutionFiles as string[] ?? solutionFiles.ToArray();

                    if (solutionFilesEnumerated.Length != 1)
                    {
                        throw new InvalidOperationException(
                            $"There must be exactly one solution file in the file hierarchy, but found {solutionFilesEnumerated.Length}"
                                .ToString(CultureInfo.InvariantCulture));
                    }

                    analyzer = new RoslynCodeAnalyzer(solutionFilesEnumerated.First(), analyzers);
                    break;
                default:
                    throw new ArgumentException($"Unrecognized analyzer: {engine}", nameof(engine));
            }

            return analyzer;
        }
    }
}