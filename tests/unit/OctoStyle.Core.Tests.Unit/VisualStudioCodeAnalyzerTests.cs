namespace OctoStyle.Core.Tests.Unit
{
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

        private static VisualStudioCodeAnalyzer GetVisualStudioCodeAnalyzer()
        {
            return new VisualStudioCodeAnalyzer(@"C:\OctoStyleTest\OctoStyleTest.sln");
        }
    }
}
