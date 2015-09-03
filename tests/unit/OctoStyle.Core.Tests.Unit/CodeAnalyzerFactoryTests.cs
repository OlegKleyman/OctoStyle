namespace OctoStyle.Core.Tests.Unit
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class CodeAnalyzerFactoryTests
    {
        [TestCase(AnalysisEngine.StyleCop, @"C:\OctoStyleTest", typeof(StyleCopCodeAnalyzer))]
        [TestCase(AnalysisEngine.VisualStudio, @"C:\OctoStyleTest\OctoStyleTest.sln", typeof(VisualStudioCodeAnalyzer))]
        public void GetAnalyzerShouldReturnAnalyzer(AnalysisEngine engine, string path, Type type)
        {
            var factory = GetCodeAnalyzerFactory();
            var analyzer = factory.GetAnalyzer(engine, path);

            Assert.That(analyzer, Is.TypeOf(type));
        }

        private static ICodeAnalyzerFactory GetCodeAnalyzerFactory()
        {
            return new CodeAnalyzerFactory();
        }
    }
}
