namespace OctoStyle.Core
{
    public class ModificationGitDiffEntry : GitDiffEntry
    {
        public ModificationGitDiffEntry(int position, GitDiffEntryStatus status, int lineNumber)
            : base(position)
        {
            this.Status = status;
            this.LineNumber = lineNumber;
        }

        public GitDiffEntryStatus Status { get; private set; }

        public int LineNumber { get; private set; }
    }
}