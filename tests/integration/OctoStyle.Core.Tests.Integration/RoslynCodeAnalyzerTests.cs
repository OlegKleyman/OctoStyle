namespace OctoStyle.Core.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Microsoft.CodeAnalysis.Diagnostics;

    using NUnit.Framework;

    [TestFixture]
    public class RoslynCodeAnalyzerTests
    {
        [Test]
        public void AnalyzeShouldReturnAllAnalysisIssues()
        {
            const string relativeSolutionPath = @"..\..\..\OctoStyleTest";
            var projectPath = Path.GetFullPath(relativeSolutionPath);

            if (projectPath == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Unable to retrieve project directory off relative path: {0}",
                        relativeSolutionPath));
            }

            var analyzerAssembly = Assembly.LoadFrom("StyleCop.Analyzers.dll");

            var analyzers =
                new List<DiagnosticAnalyzer>(
                    analyzerAssembly.GetTypes()
                        .Where(type => type.IsSubclassOf(typeof(DiagnosticAnalyzer)) && !type.IsAbstract)
                        .Select(type => (DiagnosticAnalyzer)Activator.CreateInstance(type)));

            var analyzer = new RoslynCodeAnalyzer(Path.Combine(projectPath, "OctoStyleTest.sln"), analyzers.ToArray());

            var violations =
                analyzer.Analyze(Path.Combine(projectPath, @"src\TestLibrary\TestClass.cs"))
                    .OrderBy(violation => violation.LineNumber).ThenBy(violation => violation.RuleId).ToList();
            
            Assert.That(violations.Count, Is.EqualTo(14));

            Assert.That(violations[0].Message, Is.EqualTo("A single-line comment within a C# code file does not begin with a single space."));
            Assert.That(violations[0].RuleId, Is.EqualTo("SA1005"));
            Assert.That(violations[0].LineNumber, Is.EqualTo(0));

            Assert.That(violations[1].Message, Is.EqualTo("A C# code file is missing a standard file header."));
            Assert.That(violations[1].RuleId, Is.EqualTo("SA1633"));
            Assert.That(violations[1].LineNumber, Is.EqualTo(0));
            
            Assert.That(violations[2].Message, Is.EqualTo("A C# using directive is placed outside of a namespace element."));
            Assert.That(violations[2].RuleId, Is.EqualTo("SA1200"));
            Assert.That(violations[2].LineNumber, Is.EqualTo(1));

            Assert.That(violations[3].Message, Is.EqualTo("A C# using directive is placed outside of a namespace element."));
            Assert.That(violations[3].RuleId, Is.EqualTo("SA1200"));
            Assert.That(violations[3].LineNumber, Is.EqualTo(2));

            Assert.That(violations[4].Message, Is.EqualTo("A C# using directive is placed outside of a namespace element."));
            Assert.That(violations[4].RuleId, Is.EqualTo("SA1200"));
            Assert.That(violations[4].LineNumber, Is.EqualTo(3));

            Assert.That(violations[5].Message, Is.EqualTo("A C# using directive is placed outside of a namespace element."));
            Assert.That(violations[5].RuleId, Is.EqualTo("SA1200"));
            Assert.That(violations[5].LineNumber, Is.EqualTo(4));

            Assert.That(violations[6].Message, Is.EqualTo("A C# using directive is placed outside of a namespace element."));
            Assert.That(violations[6].RuleId, Is.EqualTo("SA1200"));
            Assert.That(violations[6].LineNumber, Is.EqualTo(5));

            Assert.That(violations[7].Message, Is.EqualTo("A C# code element is missing a documentation header."));
            Assert.That(violations[7].RuleId, Is.EqualTo("SA1600"));
            Assert.That(violations[7].LineNumber, Is.EqualTo(9));

            Assert.That(violations[8].Message, Is.EqualTo("A C# code element is missing a documentation header."));
            Assert.That(violations[8].RuleId, Is.EqualTo("SA1600"));
            Assert.That(violations[8].LineNumber, Is.EqualTo(11));

            Assert.That(violations[9].Message, Is.EqualTo("An opening curly bracket within a C# element, statement, or expression is followed by a blank line."));
            Assert.That(violations[9].RuleId, Is.EqualTo("SA1505"));
            Assert.That(violations[9].LineNumber, Is.EqualTo(14));

            Assert.That(violations[10].Message, Is.EqualTo("A closing curly bracket within a C# element, statement, or expression is preceded by a blank line."));
            Assert.That(violations[10].RuleId, Is.EqualTo("SA1508"));
            Assert.That(violations[10].LineNumber, Is.EqualTo(16));

            Assert.That(violations[11].Message, Is.EqualTo("A closing curly bracket within a C# element, statement, or expression is not followed by a blank line."));
            Assert.That(violations[11].RuleId, Is.EqualTo("SA1513"));
            Assert.That(violations[11].LineNumber, Is.EqualTo(17));

            Assert.That(violations[12].Message, Is.EqualTo("A C# code element is missing a documentation header."));
            Assert.That(violations[12].RuleId, Is.EqualTo("SA1600"));
            Assert.That(violations[12].LineNumber, Is.EqualTo(20));

            Assert.That(violations[13].Message, Is.EqualTo("A closing curly bracket within a C# element, statement, or expression is not followed by a blank line."));
            Assert.That(violations[13].RuleId, Is.EqualTo("SA1513"));
            Assert.That(violations[13].LineNumber, Is.EqualTo(25));
        }
    }
}
