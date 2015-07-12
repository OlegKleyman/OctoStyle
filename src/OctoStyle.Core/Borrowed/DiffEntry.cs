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

    /// <summary>
    /// Represents a record in a unified diff
    /// </summary>
    /// <typeparam name="T">Type that will be diffed</typeparam>
    public class DiffEntry<T>
    {
        /// <summary>
        /// Gets the line number of the record
        /// </summary>
        public int LineNumber { get; private set; }

        #region Fields
        
        private readonly DiffEntryType entryType;
        private readonly T obj;
        private int count;

        #endregion

        #region Properties
        /// <summary>
        /// Gets the type of this entry.
        /// </summary>
        public DiffEntryType EntryType { get { return this.entryType; } }
        /// <summary>
        /// Gets the associated object for Add/Remove entries.
        /// </summary>
        public T Object
        {
            get
            {
                if (this.entryType == DiffEntryType.Equal)
                    throw new InvalidOperationException("Object is only valid for Add/Remove entries");

                return this.obj;
            }
        }
        /// <summary>
        /// Gets the number of Equal entries.
        /// </summary>
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

        #region Constructors
        public DiffEntry(DiffEntryType entryType, T obj, int lineNumber)
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
    }
}
