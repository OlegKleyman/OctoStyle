namespace OctoStyle.Core
{
    using System.Collections.Generic;

    using SharpDiff.FileStructure;

    /// <summary>
    /// Represents a unified diff differ.
    /// </summary>
    public interface IDiffer
    {
        /// <summary>
        /// Loads a unified diff.
        /// </summary>
        /// <param name="diff">The diff string to load.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="Diff"/> representation of the diff.</returns>
        /// <remarks>
        /// This method will attach new line character at the end of the <paramref name="diff"/> parameter value.
        /// </remarks>
        IEnumerable<Diff> Load(string diff);
    }
}