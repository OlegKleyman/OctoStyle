namespace OctoStyle.Core
{
    using System;

    using Microsoft.CodeAnalysis.Diagnostics;

    public class CodeAnalyzerFactory : ICodeAnalyzerFactory
    {
        public ICodeAnalyzer GetAnalyzer(AnalysisEngine engine, string path, params DiagnosticAnalyzer[] analyzers)
        {
            ICodeAnalyzer analyzer;

            switch (engine)
            {
                case AnalysisEngine.StyleCop:
                    analyzer = new StyleCopCodeAnalyzer(path);
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