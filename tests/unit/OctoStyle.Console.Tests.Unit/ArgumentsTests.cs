namespace OctoStyle.Console.Tests.Unit
{
    using System;
    using System.Globalization;

    using NUnit.Framework;

    [TestFixture]
    public static class ArgumentsTests
    {
        [Test]
        public static void ParseShouldReturnArguments()
        {
            var arguments =
                Arguments.Parse(@"-l TestUser -p testpass -d C:\test -o OlegKleyman -r OctoStyleTest -pr 1".Split(' '));

            Assert.That(arguments.Login, Is.EqualTo("TestUser"));
            Assert.That(arguments.Password, Is.EqualTo("testpass"));
            Assert.That(arguments.SolutionDirectory, Is.EqualTo(@"C:\test"));
            Assert.That(arguments.RepositoryOwner, Is.EqualTo(@"OlegKleyman"));
            Assert.That(arguments.Repository, Is.EqualTo(@"OctoStyleTest"));
            Assert.That(arguments.PullRequestNumber, Is.EqualTo(1));
        }

        [Test]
        public static void ParseShouldThrowArgumentExceptionWithHelpMessageWhenArgumentsAreMissing()
        {
            var ex = Assert.Throws<ArgumentException>(() => Arguments.Parse(new[] { string.Empty }));
            Assert.That(
                ex.Message,
                Is.EqualTo(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}",
                        Environment.NewLine,
                        "-l {Git Login}",
                        "-p {Git Password",
                        "-d {Solution Directory}",
                        "-o {Repository Owner}",
                        "-r {Repository}",
                        "-pr {Pull Request Number}")));
        }

        [Test]
        public static void ParseShouldThrowExceptionWhenArgumentIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Arguments.Parse(null));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: args"));
        }
    }
}