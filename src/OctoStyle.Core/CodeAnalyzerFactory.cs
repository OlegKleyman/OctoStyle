namespace OctoStyle.Core
{
    using System;

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
                    analyzer = new RoslynCodeAnalyzer(path, analyzers);
                    break;
                default:
                    throw new ArgumentException($"Unrecognized analyzer: {engine}", nameof(engine));
            }

            return analyzer;
        }
    }
}