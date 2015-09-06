namespace OctoStyle.Core
{
    using Microsoft.CodeAnalysis.Diagnostics;

    public interface ICodeAnalyzerFactory
    {
        ICodeAnalyzer GetAnalyzer(AnalysisEngine engine, string path, params DiagnosticAnalyzer[] analyzers);
    }
}