namespace OctoStyle.Core.Tests.Unit.Borrowed
{
    using System;

    using NUnit.Framework;

    using OctoStyle.Core.Borrowed;

    [TestFixture]
    public class DiffTests
    {
        [Test]
        public void DiffShouldReturnDiffListForMultiChunkDiff()
        {
            var diff = Diff.CreateDiff(
                FileContents.TestClass2CsOld.Replace(Environment.NewLine, "\n").Split('\n'),
                FileContents.TestClass2CsNew.Replace(Environment.NewLine, "\n").Split('\n'));

            Assert.That(diff.Count, Is.EqualTo(10));
            Assert.That(diff[0].LineNumber, Is.EqualTo(1));
            Assert.That(diff[0].EntryType, Is.EqualTo(DiffEntryType.Add));
            Assert.That(diff[0].Object, Is.EqualTo("//test"));
            Assert.That(diff[1].LineNumber, Is.EqualTo(0));
            Assert.That(diff[1].EntryType, Is.EqualTo(DiffEntryType.Equal));
            Assert.That(diff[1].Count, Is.EqualTo(7));
            Assert.That(diff[2].LineNumber, Is.EqualTo(8));
            Assert.That(diff[2].EntryType, Is.EqualTo(DiffEntryType.Remove));
            Assert.That(diff[2].Object, Is.EqualTo("            {"));
            Assert.That(diff[3].LineNumber, Is.EqualTo(9));
            Assert.That(diff[3].EntryType, Is.EqualTo(DiffEntryType.Remove));
            Assert.That(diff[3].Object, Is.EqualTo("                "));
            Assert.That(diff[4].LineNumber, Is.EqualTo(10));
            Assert.That(diff[4].EntryType, Is.EqualTo(DiffEntryType.Remove));
            Assert.That(diff[4].Object, Is.EqualTo("            }"));
            Assert.That(diff[5].LineNumber, Is.EqualTo(11));
            Assert.That(diff[5].EntryType, Is.EqualTo(DiffEntryType.Remove));
            Assert.That(diff[5].Object, Is.EqualTo("            foreach (var b in new[] { 1, 2, 3, 4 })"));
            Assert.That(diff[6].LineNumber, Is.EqualTo(0));
            Assert.That(diff[6].EntryType, Is.EqualTo(DiffEntryType.Equal));
            Assert.That(diff[6].Count, Is.EqualTo(1));
            Assert.That(diff[7].LineNumber, Is.EqualTo(13));
            Assert.That(diff[7].EntryType, Is.EqualTo(DiffEntryType.Remove));
            Assert.That(diff[7].Object, Is.EqualTo(String.Empty));
            Assert.That(diff[8].LineNumber, Is.EqualTo(10));
            Assert.That(diff[8].EntryType, Is.EqualTo(DiffEntryType.Add));
            Assert.That(diff[8].Object, Is.EqualTo("                var TestVar = 3;"));
            Assert.That(diff[9].LineNumber, Is.EqualTo(0));
            Assert.That(diff[9].EntryType, Is.EqualTo(DiffEntryType.Equal));
            Assert.That(diff[9].Count, Is.EqualTo(3));
        }

        [Test]
        public void DiffShouldReturnDiffListForSingleChunkDiff()
        {
            var diff = Diff.CreateDiff(
                FileContents.TestLibraryCsprojOld.Replace(Environment.NewLine, "\n").Split('\n'),
                FileContents.TestLibraryCsprojNew.Replace(Environment.NewLine, "\n").Split('\n'));

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