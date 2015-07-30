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

    /// <summary>
    ///     Represents a record in a unified diff.
    /// </summary>
    /// <typeparam name="T">Type that will be diffed.</typeparam>
    public class DiffEntry
    {
        #region Fields

        private readonly DiffEntryType entryType;

        private readonly string obj;

        private int count;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DiffEntry"/> class.
        /// </summary>
        /// <param name="entryType">The <see cref="DiffEntryType"/> of the entry.</param>
        /// <param name="obj">The entry.</param>
        /// <param name="lineNumber">The line number of the entry.</param>
        public DiffEntry(DiffEntryType entryType, string obj, int lineNumber)
        {
            this.LineNumber = lineNumber;
            this.entryType = entryType;
            this.obj = obj;
        }

        internal DiffEntry()
        {
            this.entryType = DiffEntryType.Equal;
            this.count = 1;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the line number of the record.
        /// </summary>
        /// <value>Gets the value of <see cref="LineNumber"/>.</value>
        public int LineNumber { get; private set; }

        /// <summary>
        ///     Gets the type of this entry.
        /// </summary>
        /// <value>Gets the value of <see cref="entryType"/>.</value>
        public DiffEntryType EntryType
        {
            get
            {
                return this.entryType;
            }
        }

        /// <summary>
        ///     Gets the associated object for Add/Remove entries.
        /// </summary>
        /// <value>Gets the value of <see cref="obj"/></value>
        public string Object
        {
            get
            {
                if (this.entryType == DiffEntryType.Equal)
                {
                    throw new InvalidOperationException("Object is only valid for Add/Remove entries");
                }

                return this.obj;
            }
        }

        /// <summary>
        ///     Gets or sets the number of Equal entries.
        /// </summary>
        /// <value>Gets or sets <see cref="count"/>.</value>
        public int Count
        {
            get
            {
                if (this.entryType != DiffEntryType.Equal)
                {
                    return 1;
                }

                return this.count;
            }

            set
            {
                this.count = value;
            }
        }

        #endregion
    }
}