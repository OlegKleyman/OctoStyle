namespace OctoStyle.Core
{
    using OctoStyle.Core.Borrowed;

    public abstract class GitDiffEntry
    {
        public int Position { get; private set; }

        protected GitDiffEntry(int position)
        {
            this.Position = position;
        }
    }
}