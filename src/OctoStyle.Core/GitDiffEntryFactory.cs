namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SharpDiff.FileStructure;

    /// <summary>
    /// Represents a git diff entry factory.
    /// </summary>
    public class GitDiffEntryFactory : IGitDiffEntryFactory
    {
        /// <summary>
        /// Gets git diff entries.
        /// </summary>
        /// <param name="snippet">The <see cref="ISnippet"/> containing diff entries.</param>
        /// <param name="position">The position the starting diff entry that exists in the git diff.</param>
        /// <param name="lineNumber">The starting line number of the diff entry.</param>
        /// <returns><see cref="IReadOnlyList{T}"/> of <see cref="GitDiffEntry"/>.</returns>
        public IReadOnlyList<GitDiffEntry> GetList(ISnippet snippet, int position, int lineNumber)
        {
            if (snippet == null)
            {
                throw new ArgumentNullException("snippet");
            }

            var gitDiff = new List<GitDiffEntry>();

            gitDiff.AddRange(
                snippet.OriginalLines.SelectMany(line => line.Spans)
                    .Where(span => span.Kind == SpanKind.Equal)
                    .Select(originalLine => new EqualGitDiffEntry(position++)));

            gitDiff.AddRange(
                snippet.OriginalLines.SelectMany(line => line.Spans)
                    .Where(span => span.Kind == SpanKind.Deletion)
                    .Select(line => new ModificationGitDiffEntry(position++, GitDiffEntryStatus.Removed, default(int))));

            gitDiff.AddRange(
                snippet.ModifiedLines.SelectMany(line => line.Spans)
                    .Where(span => span.Kind == SpanKind.Addition)
                    .Select(line => new ModificationGitDiffEntry(position++, GitDiffEntryStatus.New, lineNumber++)));

            return gitDiff;
        }
    }
}