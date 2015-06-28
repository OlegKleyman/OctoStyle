﻿namespace OctoStyle.Core.Tests.Integration
{
    using System;
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class CodeAnalyzerTests
    {
        [Test]
        public void AnalyzeShouldReturnAllAnalysisIssues()
        {
            const string relativeSolutionPath = @"..\..\..\..\..";
            var solutionDirectory = Path.GetFullPath(relativeSolutionPath);

            if (solutionDirectory == null)
            {
                throw new InvalidOperationException("projectPath is null");
            }

            var styleCop = new CodeAnalyzer(solutionDirectory);
            var violations = styleCop.Analyze(Path.Combine(solutionDirectory, Path.Combine(Directory.GetCurrentDirectory(), @"Resources\TestClass.cs"))).ToList();

            Assert.That(violations[0].Message, Is.EqualTo("The class must have a documentation header."));
            Assert.That(violations[0].Rule.CheckId, Is.EqualTo("SA1600"));
            Assert.That(violations[1].Message, Is.EqualTo("The method must have a documentation header."));
            Assert.That(violations[1].Rule.CheckId, Is.EqualTo("SA1600"));
            Assert.That(violations[2].Message, Is.EqualTo("The file header must contain a copyright tag."));
            Assert.That(violations[2].Rule.CheckId, Is.EqualTo("SA1634"));
            Assert.That(violations[3].Message, Is.EqualTo("An opening curly bracket must not be followed by a blank line."));
            Assert.That(violations[3].Rule.CheckId, Is.EqualTo("SA1505"));
            Assert.That(violations[4].Message, Is.EqualTo("A closing curly bracket must not be preceded by a blank line."));
            Assert.That(violations[4].Rule.CheckId, Is.EqualTo("SA1508"));
            Assert.That(violations[5].Message, Is.EqualTo("Statements or elements wrapped in curly brackets must be followed by a blank line."));
            Assert.That(violations[5].Rule.CheckId, Is.EqualTo("SA1513"));
            Assert.That(violations[6].Message, Is.EqualTo("All using directives must be placed inside of the namespace."));
            Assert.That(violations[6].Rule.CheckId, Is.EqualTo("SA1200"));
            Assert.That(violations[7].Message, Is.EqualTo("All using directives must be placed inside of the namespace."));
            Assert.That(violations[7].Rule.CheckId, Is.EqualTo("SA1200"));
            Assert.That(violations[8].Message, Is.EqualTo("All using directives must be placed inside of the namespace."));
            Assert.That(violations[8].Rule.CheckId, Is.EqualTo("SA1200"));
            Assert.That(violations[9].Message, Is.EqualTo("All using directives must be placed inside of the namespace."));
            Assert.That(violations[9].Rule.CheckId, Is.EqualTo("SA1200"));
            Assert.That(violations[10].Message, Is.EqualTo("All using directives must be placed inside of the namespace."));
            Assert.That(violations[10].Rule.CheckId, Is.EqualTo("SA1200"));
            Assert.That(violations[11].Message, Is.EqualTo("The comment must start with a single space. To ignore this error when commenting out a line of code, begin the comment with '////' rather than '//'."));
            Assert.That(violations[11].Rule.CheckId, Is.EqualTo("SA1005"));
        }
    }
}
