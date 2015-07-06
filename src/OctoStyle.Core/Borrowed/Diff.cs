//
// Copyright (C) 2009  Thomas Bluemel <thomasb@reactsoft.com>
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
    using System.Linq;

    public static class Diff
    {
        #region Public static methods
        /// <summary>
        /// Creates a list of diff entries that represent the differences between arr1 and arr2.
        /// </summary>
        /// <typeparam name="T">Class that represents the unit to compare</typeparam>
        /// <param name="arr1">Array of units.</param>
        /// <param name="arr2">Array of units.</param>
        /// <returns>List of DiffEntry classes.</returns>
        public static IReadOnlyList<DiffEntry<T>> CreateDiff<T>(T[] arr1, T[] arr2) where T : IComparable
        {
            int start = 0;
            int end = 0;

            // Strip off the beginning and end, if it's equal
            while (start < Math.Min(arr1.Length, arr2.Length))
            {
                if (arr1[start].CompareTo(arr2[start]) != 0)
                    break;
                start++;
            }

            if (start == arr1.Length && start == arr2.Length)
                return new List<DiffEntry<T>>().AsReadOnly();

            for (int i = 0; i < Math.Min(arr1.Length, arr2.Length) - start; i++)
            {
                if (arr1[arr1.Length - i - 1].CompareTo(arr2[arr2.Length - i - 1]) != 0)
                    break;
                end++;
            }

            int lines1Cnt = arr1.Length - start - end;
            int lines2Cnt = arr2.Length - start - end;

            int[,] lcs = new int[lines1Cnt, lines2Cnt];

            // Calculate longest common sequence
            for (int i = 0; i < lines1Cnt; i++)
            {
                for (int j = 0; j < lines2Cnt; j++)
                {
                    int iVal = i + start;
                    int jVal = j + start;

                    if (arr1[iVal].CompareTo(arr2[jVal]) != 0)
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
            Stack<int[]> stck = new Stack<int[]>();
            List<DiffEntry<T>> diffList = new List<DiffEntry<T>>();
            DiffEntry<T> lastEqual = null;

            stck.Push(new int[2] { lines1Cnt - 1, lines2Cnt - 1 });
            do
            {
                int[] data = stck.Pop();

                int i = data[0];
                int j = data[1];

                if (i >= 0 && j >= 0 && arr1[i + start].CompareTo(arr2[j + start]) == 0)
                {
                    stck.Push(new int[2] { i - 1, j - 1 });
                    if (lastEqual != null)
                    {
                        lastEqual.Count++;
                    }
                    else
                    {
                        lastEqual = new DiffEntry<T>();
                        diffList.Add(lastEqual);
                    }
                }
                else
                {
                    if (j >= 0 && (i <= 0 || j == 0 || lcs[i, j - 1] >= lcs[i - 1, j]))
                    {
                        stck.Push(new int[2] { i, j - 1 });
                        diffList.Add(new DiffEntry<T>(DiffEntryType.Add, arr2[j + start], j + start + 1));
                        lastEqual = null;
                    }
                    else if (i >= 0 && (j <= 0 || i == 0 || lcs[i, j - 1] < lcs[i - 1, j]))
                    {
                        stck.Push(new int[2] { i - 1, j });
                        diffList.Add(new DiffEntry<T>(DiffEntryType.Remove, arr1[i + start], i + start + 1));
                        lastEqual = null;
                    }
                }
            } while (stck.Count > 0);

            diffList.Reverse();
            
            if (diffList.Count > 0)
            {
                const int maxEqualPadding = 3;
                if (diffList[0].LineNumber > 1)
                {
                    var lineDifference = diffList[0].LineNumber == maxEqualPadding
                                             ? 2
                                             : diffList[0].LineNumber - Math.Abs(diffList[0].LineNumber - maxEqualPadding);


                    diffList.Insert(0, new DiffEntry<T> { Count = lineDifference });
                }

                var lastEntry = diffList.Last();

                var lastLineDifference = arr2.Length - lastEntry.LineNumber;

                if (lastLineDifference > 0)
                {
                    diffList.Add(new DiffEntry<T> { Count = Math.Min(lastLineDifference, maxEqualPadding) });
                }
            }
            
            return diffList.AsReadOnly();
        }

        #endregion
    }
}
