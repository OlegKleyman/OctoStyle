namespace OctoStyle.Core
{
    public class CodeAnalyzerFactory : ICodeAnalyzerFactory
    {
        public ICodeAnalyzer GetAnalyzer(AnalysisEngine engine, string path)
        {
            return new StyleCopCodeAnalyzer(path);
        }
    }
}