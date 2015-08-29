namespace OctoStyle.Core
{
    using System.Collections.Generic;

    using SharpDiff;
    using SharpDiff.FileStructure;

    public class DifferWrapper : IDiffer
    {
        public IEnumerable<Diff> Load(string diff)
        {
            return Differ.Load(string.Concat(diff, '\n'));
        }
    }
}