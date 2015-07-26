namespace OctoStyle.Core
{
    public abstract class GitDiffEntry
    {
        protected GitDiffEntry(int position)
        {
            this.Position = position;
        }

        public int Position { get; private set; }
    }
}