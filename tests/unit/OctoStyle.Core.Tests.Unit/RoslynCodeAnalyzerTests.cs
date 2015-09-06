﻿namespace OctoStyle.Core.Tests.Unit
{
    using System;

    using Microsoft.CodeAnalysis.Diagnostics;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class RoslynCodeAnalyzerTests
    {
        [Test]
        public void ConstructorShouldCreateObject()
        {
            GetRoslynCodeAnalyzer();

            Assert.Pass("If the object is created without an exeption then the constructor is considered working.");
        }

        [Test]
        public void ConstructorShouldThrowArgumentNullExceptionWhenSolutionPathArgumentIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new RoslynCodeAnalyzer(null));

            Assert.That(ex.ParamName, Is.EqualTo("solutionFilePath"));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: solutionFilePath"));
        }

        [Test]
        public void ConstructorShouldThrowArgumentExceptionWhenSolutionPathArgumentIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => new RoslynCodeAnalyzer(string.Empty));

            Assert.That(ex.ParamName, Is.EqualTo("solutionFilePath"));
            Assert.That(ex.Message, Is.EqualTo("Cannot be empty.\r\nParameter name: solutionFilePath"));
        }

        [Test]
        public void ConstructorShouldThrowArgumentNullExceptionWhenAnalyzersArgumentIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new RoslynCodeAnalyzer(string.Empty, null));

            Assert.That(ex.ParamName, Is.EqualTo("analyzers"));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: analyzers"));
        }

        [Test]
        public void ConstructorShouldThrowArgumentExceptionWhenAnalyzersArgumentIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => new RoslynCodeAnalyzer("test"));

            Assert.That(ex.ParamName, Is.EqualTo("analyzers"));
            Assert.That(ex.Message, Is.EqualTo("Cannot be empty.\r\nParameter name: analyzers"));
        }

        private static RoslynCodeAnalyzer GetRoslynCodeAnalyzer()
        {
            var diagnosticAnalyzer = new Mock<DiagnosticAnalyzer>();

            return new RoslynCodeAnalyzer(@"C:\OctoStyleTest\OctoStyleTest.sln", diagnosticAnalyzer.Object);
        }
    }
}
