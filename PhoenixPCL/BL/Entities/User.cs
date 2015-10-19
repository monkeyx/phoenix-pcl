//
// User.cs
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
    /// <summary>
    /// User.
    /// </summary>
    [Table("User")]
	public class User : EntityBase
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }

		/// <summary>
		/// Gets the group that the entity belongs to
		/// </summary>
		/// <value>The group.</value>
		[Ignore]
		public override string Group { 
			get { 
				return null;
			}
		}

		/// <summary>
		/// Gets or sets the preferences flag
		/// </summary>
		/// <value>The preferences.</value>
		public int Preferences { get; set; }

		/// <summary>
		/// Sets the preference.
		/// </summary>
		/// <param name="flag">Flag.</param>
		public void SetPreference(int flag)
		{
			Preferences |= flag;
		}

		/// <summary>
		/// Unsets the preference.
		/// </summary>
		/// <param name="flag">Flag.</param>
		public void UnsetPreference(int flag)
		{
			Preferences &= ~flag;
		}

		/// <summary>
		/// Determines whether this user has a preference with the specified flag.
		/// </summary>
		/// <returns><c>true</c> if this flag is in user's preference; otherwise, <c>false</c>.</returns>
		/// <param name="flag">Flag.</param>
		public bool HasPreference(int flag)
		{
			return ((Preferences & flag) != 0);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.User"/> class.
        /// </summary>
        public User ()
        {
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.User"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.User"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("[User: Id={0}, Code={1}, Preferences={2}]", Id, Code, Preferences);
		}
    }
}

