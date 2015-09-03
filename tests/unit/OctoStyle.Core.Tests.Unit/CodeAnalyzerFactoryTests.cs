namespace OctoStyle.Core.Tests.Unit
{
    using NUnit.Framework;

    [TestFixture]
    public class CodeAnalyzerFactoryTests
    {
        [Test]
        public void GetAnalyzerShouldReturnAnalyzer(AnalysisEngine engine)
        {
            var factory = GetCodeAnalyzerFactory();
            var analyzer = factory.GetAnalyzer(engine);
        }

        private static ICodeAnalyzerFactory GetCodeAnalyzerFactory()
        {
            return new CodeAnalyzerFactory();
        }
    }
}
