namespace OctoStyle.Core
{
    using System;

    public class CodeAnalyzerFactory : ICodeAnalyzerFactory
    {
        public ICodeAnalyzer GetAnalyzer(AnalysisEngine engine, string path)
        {
            ICodeAnalyzer analyzer;

            switch (engine)
            {
                case AnalysisEngine.StyleCop:
                    analyzer = new StyleCopCodeAnalyzer(path);
                    break;
                case AnalysisEngine.Roslyn:
                    analyzer = new RoslynCodeAnalyzer(path);
                    break;
                default:
                    throw new ArgumentException($"Unrecognized analyzer: {engine}", nameof(engine));
            }

            return analyzer;
        }
    }
}