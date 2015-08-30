namespace OctoStyle.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a diff parser.
    /// </summary>
    public interface IDiffParser
    {
        /// <summary>
        /// Splits a diff by file.
        /// </summary>
        /// <param name="diff">The full diff to split.</param>
        /// <returns>
        /// A <see cref="Dictionary{TKey,TValue}"/> of <see cref="string"/>, <see cref="string"/>
        /// containing the diffs split from the full diff.
        /// </returns>
        IDictionary<string, string> Split(string diff);
    }
}