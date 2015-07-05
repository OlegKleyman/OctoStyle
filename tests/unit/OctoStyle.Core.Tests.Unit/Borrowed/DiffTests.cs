namespace OctoStyle.Core.Tests.Unit.Borrowed
{
    using NUnit.Framework;

    using OctoStyle.Core.Borrowed;

    [TestFixture]
    public class DiffTests
    {
        [Test]
        public void DiffShouldReturnDiffListForSingleChunkDiff()
        {
            var diff = Diff.CreateDiff(
                FileContents.TestLibraryCsprojOld.Split('\n'),
                FileContents.TestLibraryCsprojNew.Split('\n'));

            Assert.That(diff.Count, Is.EqualTo(11));
            Assert.That(diff[0].LineNumber, Is.EqualTo(0));
            Assert.That(diff[0].EntryType, Is.EqualTo(DiffEntryType.Equal));
            Assert.That(diff[0].Count, Is.EqualTo(3));
            Assert.That(diff[1].LineNumber, Is.EqualTo(33));
            Assert.That(diff[1].EntryType, Is.EqualTo(DiffEntryType.Add));
            Assert.That(diff[1].Object, Is.EqualTo("    <Compile Include=\"Nested\\TestClass3.cs\" />"));
            Assert.That(diff[2].LineNumber, Is.EqualTo(0));
            Assert.That(diff[2].EntryType, Is.EqualTo(DiffEntryType.Equal));
            Assert.That(diff[2].Count, Is.EqualTo(2));
            Assert.That(diff[3].LineNumber, Is.EqualTo(35));
            Assert.That(diff[3].EntryType, Is.EqualTo(DiffEntryType.Remove));
            Assert.That(diff[3].Object, Is.EqualTo("    <Compile Include=\"TestClass2.cs\" />"));
            Assert.That(diff[4].LineNumber, Is.EqualTo(36));
            Assert.That(diff[4].EntryType, Is.EqualTo(DiffEntryType.Remove));
            Assert.That(diff[4].Object, Is.EqualTo("  </ItemGroup>"));
            Assert.That(diff[5].LineNumber, Is.EqualTo(37));
            Assert.That(diff[5].EntryType, Is.EqualTo(DiffEntryType.Remove));
            Assert.That(diff[5].Object, Is.EqualTo("  <ItemGroup>"));
            Assert.That(diff[6].LineNumber, Is.EqualTo(38));
            Assert.That(diff[6].EntryType, Is.EqualTo(DiffEntryType.Remove));
            Assert.That(diff[6].Object, Is.EqualTo("    <Folder Include=\"Nested\\\" />"));
            Assert.That(diff[7].LineNumber, Is.EqualTo(36));
            Assert.That(diff[7].EntryType, Is.EqualTo(DiffEntryType.Add));
            Assert.That(diff[7].Object, Is.EqualTo("    <Compile Include=\"Nested\\TestClass2.cs\" />"));
            Assert.That(diff[8].LineNumber, Is.EqualTo(0));
            Assert.That(diff[8].EntryType, Is.EqualTo(DiffEntryType.Equal));
            Assert.That(diff[8].Count, Is.EqualTo(1));
            Assert.That(diff[9].LineNumber, Is.EqualTo(38));
            Assert.That(diff[9].EntryType, Is.EqualTo(DiffEntryType.Add));
            Assert.That(diff[9].Object, Is.EqualTo("  <ItemGroup />"));
            Assert.That(diff[10].LineNumber, Is.EqualTo(0));
            Assert.That(diff[10].EntryType, Is.EqualTo(DiffEntryType.Equal));
            Assert.That(diff[10].Count, Is.EqualTo(3));
        }
    }
}
