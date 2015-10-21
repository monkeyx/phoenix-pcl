//
// InfoData.cs
//
// Author:
//       Seyed Razavi <monkeyx@gmail.com>
//
// Copyright (c) 2015 Seyed Razavi 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using SQLite.Net.Attributes; 

namespace Phoenix.BL.Entities
{
    [Table("InfoData")]
    public class InfoData : EntityBase
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PrimaryKey, AutoIncrement]
		public override int Id { get; set; }

		/// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>The group.</value>
        [Indexed]
        public override string Group { get; set; }

		/// <summary>
		/// Gets or sets the group identifier.
		/// </summary>
		/// <value>The group identifier.</value>
		[Indexed]
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Indexed]
        public string Name { get; set; }

		/// <summary>
		/// Gets or sets the nexus identifier.
		/// </summary>
		/// <value>The nexus identifier.</value>
        [Indexed]
        public int NexusId { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public int DataType { get; set; }

		/// <summary>
		/// Gets the list text.
		/// </summary>
		/// <value>The list text.</value>
		[Ignore]
		public override string ListText { get { return ToString(); } }

		/// <summary>
		/// Gets the list detail.
		/// </summary>
		/// <value>The list detail.</value>
		[Ignore]
		public override string ListDetail { get { return Group + " (" + GroupId + ")"; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.InfoData"/> class.
        /// </summary>
        public InfoData ()
        {
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.InfoData"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.InfoData"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("{0} ({1})", Name, NexusId);
		}
    }
}

