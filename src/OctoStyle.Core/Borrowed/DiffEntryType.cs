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
    /// <summary>
    /// Represents a diff record type
    /// </summary>
    public enum DiffEntryType
    {
        /// <summary>
        /// Represents a deleted record
        /// </summary>
        Remove,

        /// <summary>
        /// Represents an added record
        /// </summary>
        Add,

        /// <summary>
        /// Represents a record with no modifications.
        /// </summary>
        Equal
    };
}