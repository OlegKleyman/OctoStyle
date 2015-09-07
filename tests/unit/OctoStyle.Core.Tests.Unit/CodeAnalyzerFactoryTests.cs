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

        [Test]
        public void ConstructorShouldThrowArgumentNullExceptionWhenPathResolverIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new CodeAnalyzerFactory(null));

            Assert.That(ex.ParamName, Is.EqualTo("pathResolver"));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: pathResolver"));
        }

        private static ICodeAnalyzerFactory GetCodeAnalyzerFactory()
        {
            var pathResolver = new Mock<IPathResolver>();

            return new CodeAnalyzerFactory(pathResolver.Object);
        }
    }
}
