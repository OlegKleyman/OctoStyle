﻿// Copyright (C) 2009  Thomas Bluemel <thomasb@reactsoft.com>
// 
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
//
// Original code found at: https://code.google.com/p/csdiff/

namespace OctoStyle.Core.Borrowed
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    ///     Used for finding a unified diff between two arrays of IComparable objects.
    /// </summary>
    public static class Diff
    {
        #region Public static methods

        /// <summary>
        ///     Creates a list of diff entries that represent the differences between the original and modified arrays.
        /// </summary>
        /// <param name="original">The original lines.</param>
        /// <param name="modified">The modified lines.</param>
        /// <returns>List of DiffEntry classes.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1303:ConstFieldNamesMustBeginWithUpperCaseLetter", Justification = StyleCopConstants.LocalConstantJustification)]
        internal static IReadOnlyList<DiffEntry> CreateDiff(string[] original, string[] modified)
        {
            var start = 0;
            var end = 0;

            // Strip off the beginning and end, if it's equal
            while (start < Math.Min(original.Length, modified.Length))
            {
                if (String.Compare(original[start], modified[start], StringComparison.Ordinal) != 0)
                {
                    break;
                }

                start++;
            }

            if (start == original.Length && start == modified.Length)
            {
                return new List<DiffEntry>().AsReadOnly();
            }

            for (var i = 0; i < Math.Min(original.Length, modified.Length) - start; i++)
            {
                if (String.Compare(original[original.Length - i - 1], modified[modified.Length - i - 1], StringComparison.Ordinal) != 0)
                {
                    break;
                }

                end++;
            }

            var lines1Cnt = original.Length - start - end;
            var lines2Cnt = modified.Length - start - end;

            var lcs = new int[lines1Cnt, lines2Cnt];

            // Calculate longest common sequence
            for (var i = 0; i < lines1Cnt; i++)
            {
                for (var j = 0; j < lines2Cnt; j++)
                {
                    var originalValue = i + start;
                    var modifiedValue = j + start;

                    if (String.Compare(original[originalValue], modified[modifiedValue], StringComparison.Ordinal) != 0)
                    {
                        if (i == 0 && j == 0)
                        {
                            lcs[i, j] = 0;
                        }
                        else if (i == 0 && j != 0)
                        {
                            lcs[i, j] = Math.Max(0, lcs[i, j - 1]);
                        }
                        else if (i != 0 && j == 0)
                        {
                            lcs[i, j] = Math.Max(lcs[i - 1, j], 0);
                        }
                        else
                        {
                            lcs[i, j] = Math.Max(lcs[i - 1, j], lcs[i, j - 1]);
                        }
                    }
                    else
                    {
                        if (i == 0 || j == 0)
                        {
                            lcs[i, j] = 1;
                        }
                        else
                        {
                            lcs[i, j] = 1 + lcs[i - 1, j - 1];
                        }
                    }
                }
            }

            // Build the list of differences
            var stck = new Stack<int[]>();
            var diffList = new List<DiffEntry>();
            DiffEntry lastEqual = null;

            stck.Push(new int[2] { lines1Cnt - 1, lines2Cnt - 1 });
            do
            {
                var data = stck.Pop();

                var i = data[0];
                var j = data[1];

                if (i >= 0 && j >= 0 && String.CompareOrdinal(original[i + start], modified[j + start]) == 0)
                {
                    stck.Push(new int[2] { i - 1, j - 1 });
                    if (lastEqual != null)
                    {
                        lastEqual.Count++;
                    }
                    else
                    {
                        lastEqual = new DiffEntry();
                        diffList.Add(lastEqual);
                    }
                }
                else
                {
                    if (j >= 0 && (i <= 0 || j == 0 || lcs[i, j - 1] >= lcs[i - 1, j]))
                    {
                        stck.Push(new int[2] { i, j - 1 });
                        diffList.Add(new DiffEntry(DiffEntryType.Add, modified[j + start], j + start + 1));
                        lastEqual = null;
                    }
                    else if (i >= 0 && (j <= 0 || i == 0 || lcs[i, j - 1] < lcs[i - 1, j]))
                    {
                        stck.Push(new int[2] { i - 1, j });
                        diffList.Add(new DiffEntry(DiffEntryType.Remove, original[i + start], i + start + 1));
                        lastEqual = null;
                    }
                }
            }
            while (stck.Count > 0);

            diffList.Reverse();

            if (diffList.Count > 0)
            {
                const int maxEqualPadding = 3;
                if (diffList[0].LineNumber > 1)
                {
                    var lineDifference = diffList[0].LineNumber == maxEqualPadding
                                             ? 2
                                             : diffList[0].LineNumber
                                               - Math.Abs(diffList[0].LineNumber - maxEqualPadding);

                    diffList.Insert(0, new DiffEntry { Count = lineDifference });
                }

                var lastEntry = diffList.Last();

                var lastLineDifference = modified.Length - lastEntry.LineNumber;

                if (lastLineDifference > 0)
                {
                    diffList.Add(new DiffEntry { Count = Math.Min(lastLineDifference, maxEqualPadding) });
                }
            }

            return diffList.AsReadOnly();
        }

        #endregion
    }
}