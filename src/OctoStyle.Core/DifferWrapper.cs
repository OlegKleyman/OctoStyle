namespace OctoStyle.Core
{
    using System.Collections.Generic;

    using SharpDiff;
    using SharpDiff.FileStructure;

    /// <summary>
    /// Represents a wrapper for the <see cref="Differ"/> class.
    /// </summary>
    public class DifferWrapper : IDiffer
    {
        /// <summary>
        /// A wrapper for the <see cref="Differ.Load"/> method.
        /// </summary>
        /// <param name="diff">The diff string to load.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="Diff"/> representation of the diff.</returns>
        /// <remarks>
        /// This method will attach new line character at the end of the <paramref name="diff"/> parameter value.
        /// </remarks>
        public IEnumerable<Diff> Load(string diff)
        {
            return Differ.Load(string.Concat(diff, '\n'));
        }
    }
}