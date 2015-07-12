namespace OctoStyle.Core.Tests.Unit
{
    using NUnit.Framework;

    using OctoStyle.Core.Borrowed;

    [TestFixture]
    public class GitDiffEntryFactoryTests
    {
        [Test]
        public void GetEntryShouldReturnEqualGitDiffEntry()
        {
            var factory = GetGitDiffEntryFactory();

            var entry = new DiffEntry<string>(DiffEntryType.Equal, null, 0);
            entry.Count = 3;

            var result = factory.Get(entry, 3);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));
            
            Assert.That(result[0], Is.InstanceOf<EqualGitDiffEntry>());
            Assert.That(result[0].Position, Is.EqualTo(3));
            Assert.That(result[1], Is.InstanceOf<EqualGitDiffEntry>());
            Assert.That(result[1].Position, Is.EqualTo(4));
            Assert.That(result[2], Is.InstanceOf<EqualGitDiffEntry>());
            Assert.That(result[2].Position, Is.EqualTo(5));
        }

        private GitDiffEntryFactory GetGitDiffEntryFactory()
        {
            return new GitDiffEntryFactory();
        }
    }
}
