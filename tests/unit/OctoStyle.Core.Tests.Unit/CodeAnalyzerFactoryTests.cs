namespace OctoStyle.Core.Tests.Unit
{
    using System;

    using Microsoft.CodeAnalysis.Diagnostics;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class CodeAnalyzerFactoryTests
    {
        [TestCase(AnalysisEngine.StyleCop, @"C:\OctoStyleTest\TestProject\TestClass.cs", typeof(StyleCopCodeAnalyzer))]
        [TestCase(AnalysisEngine.Roslyn, @"C:\OctoStyleTest\TestProject\TestClass.cs", typeof(RoslynCodeAnalyzer))]
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

        [TestCase(@"C:\MultipleSolutionDirectory",
            "There must be exactly one solution file in the file hierarchy, but found 2")]
        [TestCase(@"C:\NoSolutionDirectory",
            "There must be exactly one solution file in the file hierarchy, but found 0")]
        public void GetAnalyzerShouldThrowInvalidOperationExceptionWhenExactlyOneSolutionFileIsNotFound(
            string solutionDirectory,
            object exceptionMessage)
        {
            var factory = GetCodeAnalyzerFactory();
            var diagnosticAnalyzer = new Mock<DiagnosticAnalyzer>();
            var ex =
                Assert.Throws<InvalidOperationException>(
                    () => factory.GetAnalyzer(AnalysisEngine.Roslyn, solutionDirectory, diagnosticAnalyzer.Object));

            Assert.That(ex.Message, Is.EqualTo(exceptionMessage));
        }

        private static ICodeAnalyzerFactory GetCodeAnalyzerFactory()
        {
            var pathResolver = new Mock<IPathResolver>();
            pathResolver.Setup(
                resolver => resolver.GetDirectoryPath(@"C:\OctoStyleTest\TestProject\TestClass.cs", "*.csproj"))
                .Returns(@"C:\OctoStyleTest");

            pathResolver.Setup(
                resolver => resolver.GetFilePaths(@"C:\OctoStyleTest\TestProject\TestClass.cs", "*.sln"))
                .Returns(new[] { @"C:\OctoStyleTest\OctoStyleTest.sln" });

            pathResolver.Setup(resolver => resolver.GetFilePaths(@"C:\MultipleSolutionDirectory", "*.sln"))
                .Returns(
                    new[]
                        { @"C:\MultipleSolutionDirectory\solution1.sln", @"C:\MultipleSolutionDirectory\solution2.sln" });
            pathResolver.Setup(resolver => resolver.GetFilePaths(@"C:\NoSolutionDirectory", "*.sln"))
                .Returns(new string[0]);

            return new CodeAnalyzerFactory(pathResolver.Object);
        }
    }
}
