﻿namespace OctoStyle.Core.Tests.Unit
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class VisualStudioCodeAnalyzerTests
    {
        [Test]
        public void ConstructorShouldCreateObject()
        {
            GetVisualStudioCodeAnalyzer();

            Assert.Pass("If the object is created without an exeption then the constructor is considered working.");
        }

        [Test]
        public void ConstructorShouldThrowArgumentNullExceptionWhenArgumentIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new VisualStudioCodeAnalyzer(null));

            Assert.That(ex.ParamName, Is.EqualTo("solutionFilePath"));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: solutionFilePath"));
        }

        private static VisualStudioCodeAnalyzer GetVisualStudioCodeAnalyzer()
        {
            return new VisualStudioCodeAnalyzer(@"C:\OctoStyleTest\OctoStyleTest.sln");
        }
    }
}
