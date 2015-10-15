//
// Position.cs
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
using SQLite;

namespace Phoenix.BL.Entities
{
    [Table("Position")]
    public class Position : EntityBase
    {
		/// <summary>
		/// Position types.
		/// </summary>
		public enum PositionType
		{
			Agent = 7,
			Debris = 4,
			GP = 1,
			None = 0,
			Platform = 6,
			Political = 5,
			PD = 8,
			Ship = 2,
			Starbase = 3,
			Misc = 32
		}

		/// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Indexed]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the system text.
        /// </summary>
        /// <value>The system text.</value>
        [Indexed]
        public string SystemText { get; set; }

        /// <summary>
        /// Gets or sets the location text.
        /// </summary>
        /// <value>The location text.</value>
        public string LocationText { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public string Size { get; set; }

        /// <summary>
        /// Gets or sets the design.
        /// </summary>
        /// <value>The design.</value>
        [Indexed]
        public string Design { get; set; }

        /// <summary>
        /// Gets or sets the position class.
        /// </summary>
        /// <value>The position class.</value>
        [Indexed]
        public string PositionClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Phoenix.Position"/> is orders.
        /// </summary>
        /// <value><c>true</c> if orders; otherwise, <c>false</c>.</value>
        public bool Orders { get; set; }

		/// <summary>
		/// Gets the group that the entity belongs to
		/// </summary>
		/// <value>The group.</value>
		[Ignore]
		public override string Group { 
			get { 
				return PositionClass;
			}
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.Position"/> class.
        /// </summary>
        public Position ()
        {
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.Position"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.Position"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("{0} ({1})", Name, Id);
		}
    }
}

