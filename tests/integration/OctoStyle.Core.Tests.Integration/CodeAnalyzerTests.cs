namespace OctoStyle.Core.Tests.Integration
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public static class CodeAnalyzerTests
    {
        [Test]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1303:ConstFieldNamesMustBeginWithUpperCaseLetter", Justification = StyleCopConstants.LocalConstantJustification)]
        public static void AnalyzeShouldReturnAllAnalysisIssues()
        {
            const string relativeSolutionPath = @"..\..";
            var projectPath = Path.GetFullPath(relativeSolutionPath);
            Console.WriteLine(projectPath);

            if (projectPath == null)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "Unable to retrieve project directory off relative path: {0}",
                        relativeSolutionPath));
            }

            var styleCop = new CodeAnalyzer(projectPath);
            var violations =
                styleCop.Analyze(Path.Combine(Directory.GetCurrentDirectory(), @"Resources\TestClass.cs")).ToList();

            Assert.That(
                violations[0].Message,
                Is.EqualTo("An opening curly bracket must not be followed by a blank line."));
            Assert.That(violations[0].RuleId, Is.EqualTo("SA1505"));
            Assert.That(
                violations[1].Message,
                Is.EqualTo("A closing curly bracket must not be preceded by a blank line."));
            Assert.That(violations[1].RuleId, Is.EqualTo("SA1508"));
            Assert.That(
                violations[2].Message,
                Is.EqualTo("Statements or elements wrapped in curly brackets must be followed by a blank line."));
            Assert.That(violations[2].RuleId, Is.EqualTo("SA1513"));
            Assert.That(
                violations[3].Message,
                Is.EqualTo(
                    "The comment must start with a single space. To ignore this error when commenting out a line of code, begin the comment with '////' rather than '//'."));
            Assert.That(violations[3].RuleId, Is.EqualTo("SA1005"));
        }
    }
}