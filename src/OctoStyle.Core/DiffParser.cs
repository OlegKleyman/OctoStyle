namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a diff parser.
    /// </summary>
    public class DiffParser : IDiffParser
    {
        /// <summary>
        /// Splits a diff by file.
        /// </summary>
        /// <param name="diff">The full diff to split.</param>
        /// <returns>
        /// A <see cref="Dictionary{TKey,TValue}"/> of <see cref="string"/>, <see cref="string"/>
        /// containing the diffs split from the full diff.
        /// </returns>
        public IDictionary<string, string> Split(string diff)
        {
            var regex = new Regex(@"^(diff\s--git\sa/)(.+)(\sb/.+)$", RegexOptions.Multiline);
            
            var splitDiff = regex.Split(diff);
            var diffs = new Dictionary<string, string>();

            for (int i = 1; i < splitDiff.Length - 1; i++)
            {
                var firstDiffHeaderSegment = splitDiff[i];
                var fileNameDiffHeaderSegment = splitDiff[++i];
                var secondDiffHeaderSegment = splitDiff[++i];
                var diffBody = splitDiff[++i];

                diffs.Add(
                    fileNameDiffHeaderSegment,
                    String.Concat(
                        firstDiffHeaderSegment,
                        fileNameDiffHeaderSegment,
                        secondDiffHeaderSegment,
                        diffBody));
            }

            return diffs;
        }
    }
}