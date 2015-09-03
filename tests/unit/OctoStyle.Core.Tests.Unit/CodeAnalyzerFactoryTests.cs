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

        [Test]
        public void GetAnalyzerShouldThrowAnArgumentExceptionWhenEngineIsNotFound()
        {
            var factory = GetCodeAnalyzerFactory();
            var ex =
                Assert.Throws<ArgumentException>(
                    () => factory.GetAnalyzer((AnalysisEngine)(-1), @"C:\OctoStyleTest\OctoStyleTest.sln"));

            Assert.That(ex.ParamName, Is.EqualTo("engine"));
            Assert.That(ex.Message, Is.EqualTo("Unrecognized analyzer: -1\r\nParameter name: engine"));

        }

        private static ICodeAnalyzerFactory GetCodeAnalyzerFactory()
        {
            return new CodeAnalyzerFactory();
        }
    }
}
