namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SharpDiff.FileStructure;

    /// <summary>
    /// Represents a github diff retriever.
    /// </summary>
    public class GitHubDiffRetriever : IGitHubDiffRetriever
    {
        private readonly IDiffer differ;

        private readonly IGitDiffEntryFactory diffFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubDiffRetriever"/> class.
        /// </summary>
        /// <param name="differ">The <see cref="IDiffer"/> instance to use for loading diffs.</param>
        /// <param name="diffFactory">
        /// The <see cref="IGitDiffEntryFactory"/> instance to retrieve <see cref="GitDiffEntry"/> objects with.
        /// </param>
        public GitHubDiffRetriever(IDiffer differ, IGitDiffEntryFactory diffFactory)
        {
            if (differ == null)
            {
                throw new ArgumentNullException("differ");
            }

            if (diffFactory == null)
            {
                throw new ArgumentNullException("diffFactory");
            }

            this.differ = differ;
            this.diffFactory = diffFactory;
        }

        /// <summary>
        /// Retrieves a GitHub pull request file diff.
        /// </summary>
        /// <param name="rawDiff">The pull request file diff content.</param>
        /// <returns>An <see cref="IReadOnlyList{T}"/> of <see cref="GitDiffEntry"/>.</returns>
        public IReadOnlyList<GitDiffEntry> Retrieve(string rawDiff)
        {
            var diff = this.differ.Load(rawDiff);

            var enumeratedDiff = diff as Diff[] ?? diff.ToArray();

            if (diff == null || !enumeratedDiff.Any())
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Unable to retrieve diff for:{0}{1}",
                        Environment.NewLine,
                        rawDiff));
            }

            if (enumeratedDiff.Length > 1)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Multiple diffs found for:{0}{1}",
                        Environment.NewLine,
                        rawDiff));
            }

            var targetDiff = enumeratedDiff.First();
            var diffEntries = new List<GitDiffEntry>();
            var position = 1;

            foreach (var chunk in targetDiff.Chunks)
            {
                var lineNumer = chunk.NewRange.StartLine;

                foreach (var snippet in chunk.Snippets)
                {
                    diffEntries.AddRange(this.diffFactory.GetList(snippet, position, lineNumer));

                    position = diffEntries.Count + 1;
                    lineNumer += snippet.ModifiedLines.Count()
                                 + snippet.OriginalLines.SelectMany(line => line.Spans)
                                       .Count(span => span.Kind == SpanKind.Equal);
                }
            }

            return diffEntries;
        }
    }
}