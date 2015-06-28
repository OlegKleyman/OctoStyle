namespace OctoStyle.Console.Tests.Unit
{
    using System;
    using System.Globalization;

    using NUnit.Framework;

    [TestFixture]
    public class ArgumentsTests
    {
        [Test]
        public void ParseShouldReturnArguments()
        {
            var arguments = Arguments.Parse(@"-d C:\test -o OlegKleyman -r OctoStyleTest -pr 1".Split(' '));
            Assert.That(arguments.SolutionDirectory, Is.EqualTo(@"C:\test"));
            Assert.That(arguments.RepositoryOwner, Is.EqualTo(@"OlegKleyman"));
            Assert.That(arguments.Repository, Is.EqualTo(@"OctoStyleTest"));
            Assert.That(arguments.PullRequestNumber, Is.EqualTo(1));
        }

        [Test]
        public void ParseShouldThrowArgumentExceptionWithHelpMessageWhenArgumentsAreMissing()
        {
            var ex = Assert.Throws<ArgumentException>(() => Arguments.Parse(new[] { String.Empty }));
            Assert.That(
                ex.Message,
                Is.EqualTo(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{1}{0}{2}{0}{3}{0}{4}",
                        Environment.NewLine,
                        "-d {Solution Directory",
                        "-o {Repository Owner}",
                        "-r {Repository}",
                        "-pr {Pull Request Number}")));
        }

        [Test]
        public void ParseShouldThrowExceptionWhenArgumentIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Arguments.Parse(null));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: args"));
        }
    }
}
