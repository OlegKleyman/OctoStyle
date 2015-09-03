namespace OctoStyle.Core
{
    public interface ICodeAnalyzerFactory
    {
        ICodeAnalyzer GetAnalyzer(AnalysisEngine engine, string path);
    }
}