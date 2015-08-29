namespace OctoStyle.Core
{
    using System.Collections.Generic;

    using SharpDiff.FileStructure;

    public interface IDiffer
    {
        IEnumerable<Diff> Load(string diff);
    }
}