namespace OctoStyle.Core
{
    using System;

    public class CodeAnalyzerFactory : ICodeAnalyzerFactory
    {
        public ICodeAnalyzer GetAnalyzer(AnalysisEngine engine, string path)
        {
            var analyzer = default(ICodeAnalyzer);

            switch (engine)
            {
                case AnalysisEngine.StyleCop:
                    analyzer = new StyleCopCodeAnalyzer(path);
                    break;
                case AnalysisEngine.VisualStudio:
                    analyzer = new VisualStudioCodeAnalyzer(path);
                    break;
            }

            return analyzer;
        }
    }
}