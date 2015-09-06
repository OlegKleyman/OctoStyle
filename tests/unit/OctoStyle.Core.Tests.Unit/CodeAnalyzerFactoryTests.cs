namespace OctoStyle.Core.Tests.Unit
{
    using System;

    using Microsoft.CodeAnalysis.Diagnostics;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class CodeAnalyzerFactoryTests
    {
        [TestCase(AnalysisEngine.StyleCop, @"C:\OctoStyleTest", typeof(StyleCopCodeAnalyzer))]
        [TestCase(AnalysisEngine.Roslyn, @"C:\OctoStyleTest\OctoStyleTest.sln", typeof(RoslynCodeAnalyzer))]
        public void GetAnalyzerShouldReturnAnalyzer(AnalysisEngine engine, string path, Type type)
        {
            var factory = GetCodeAnalyzerFactory();
            var diagnosticAnalyzer = new Mock<DiagnosticAnalyzer>();
            var analyzer = factory.GetAnalyzer(engine, path, diagnosticAnalyzer.Object);

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
