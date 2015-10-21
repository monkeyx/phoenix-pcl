//
// EntityBase.cs
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
    /// <summary>
    /// Base class for all entities
    /// </summary>
    public abstract class EntityBase : IEntity, IEquatable<EntityBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.EntityBase"/> class.
        /// </summary>
        public EntityBase ()
        {
        }

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public abstract int Id { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>The created at.</value>
        [Indexed]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the updated at.
        /// </summary>
        /// <value>The updated at.</value>
        [Indexed]
        public DateTime UpdatedAt { get; set; }

		/// <summary>
		/// Gets the list text.
		/// </summary>
		/// <value>The list text.</value>
		[Ignore]
		public virtual string ListText { get { return ToString(); } }

		/// <summary>
		/// Gets the list detail.
		/// </summary>
		/// <value>The list detail.</value>
		[Ignore]
		public virtual string ListDetail { get { return ""; } }

		/// <summary>
		/// Gets the group that the entity belongs to
		/// </summary>
		/// <value>The group.</value>
		public virtual string Group { get; set; }

		/// <summary>
		/// Gets the group short name the entity belongs to
		/// </summary>
		/// <value>The group short name.</value>
		public virtual string GroupShortName { 
			get {
				return string.IsNullOrWhiteSpace (Group) ? "*" : Group.Substring (0, 1);
			}
			set {
			}
		}

        /// <summary>
        /// Determines whether the specified <see cref="Phoenix.EntityBase"/> is equal to the current <see cref="Phoenix.EntityBase"/>.
        /// </summary>
        /// <param name="other">The <see cref="Phoenix.EntityBase"/> to compare with the current <see cref="Phoenix.EntityBase"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Phoenix.EntityBase"/> is equal to the current
        /// <see cref="Phoenix.EntityBase"/>; otherwise, <c>false</c>.</returns>
		public bool Equals(EntityBase other)
        {
			if (other != null && other.GetType() == GetType() && Id == other.Id) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Phoenix.EntityBase"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Phoenix.EntityBase"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Phoenix.EntityBase"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals (object obj)
        {
            if (obj is EntityBase) {
                return Equals((EntityBase)obj);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Phoenix.EntityBase"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() {
			return this.Id.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.EntityBase"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.EntityBase"/>.</returns>
        public override string ToString ()
        {
			return string.Format ("[" + GetType().Name + "]: Id={0}, CreatedAt={1}, UpdatedAt={2}]", Id, CreatedAt, UpdatedAt);
        }
    }
}

