namespace OctoStyle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class DiffParser : IDiffParser
    {
        public IDictionary<string, string> Split(string diff)
        {
            var splitDiff = Regex.Split(diff, @"^(diff\s--git\sa/.+)$", RegexOptions.Multiline);
            var diffs = new Dictionary<string, string>();

            for (int i = 1; i < splitDiff.Length - 1; i++)
            {
                var match = Regex.Match(splitDiff[i], @"^diff\s--git\sa/(.+)\sb/.+$");

                diffs.Add(match.Groups[1].Value, String.Concat(splitDiff[i], splitDiff[++i].TrimEnd('\n')));
            }

            return diffs;
        }
    }
}