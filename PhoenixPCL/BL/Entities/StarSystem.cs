//
// StarSystem.cs
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
using System.Collections.Generic;
using SQLite;

namespace Phoenix.BL.Entities
{
    /// <summary>
    /// Star system.
    /// </summary>
    [Table("StarSystem")]
    public class StarSystem : EntityBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Indexed]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the celestial bodies.
        /// </summary>
        /// <value>The celestial bodies.</value>
        [Ignore]
        public List<CelestialBody> CelestialBodies { get; set; }

        /// <summary>
        /// Gets or sets the jump links.
        /// </summary>
        /// <value>The jump links.</value>
        [Ignore]
        public List<JumpLink> JumpLinks { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.StarSystem"/> class.
        /// </summary>
        public StarSystem ()
        {
			CelestialBodies = new List<CelestialBody> ();
			JumpLinks = new List<JumpLink> ();
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.StarSystem"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.StarSystem"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("{0} ({1})", Name, Id);
		}
    }

    /// <summary>
    /// Celestial body.
    /// </summary>
    [Table("CelestialBody")]
    public class CelestialBody : EntityBase 
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PrimaryKey, AutoIncrement]
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets the star system identifier.
        /// </summary>
        /// <value>The star system identifier.</value>
        [Indexed]
        public int StarSystemId { get; set; }

        /// <summary>
        /// Gets or sets the local celestial body identifier.
        /// </summary>
        /// <value>The local celestial body identifier.</value>
        [Indexed]
        public int LocalCelestialBodyId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Indexed]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the quad.
        /// </summary>
        /// <value>The quad.</value>
        [Indexed]
        public int Quad { get; set; }

        /// <summary>
        /// Gets or sets the ring.
        /// </summary>
        /// <value>The ring.</value>
        [Indexed]
        public int Ring { get; set; }

        /// <summary>
        /// Gets or sets the type of the C body.
        /// </summary>
        /// <value>The type of the C body.</value>
        public int CBodyType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.CelestialBody"/> class.
        /// </summary>
        public CelestialBody()
        {
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.CelestialBody"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.CelestialBody"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("[CelestialBody: StarSystemId={0}, CelestialBodyId={1}, Name={2}, Quad={3}, Ring={4}, CBodyType={5}]", StarSystemId, LocalCelestialBodyId, Name, Quad, Ring, CBodyType);
		}
    }

    /// <summary>
    /// Jump link.
    /// </summary>
    [Table("JumpLink")]
    public class JumpLink : EntityBase
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PrimaryKey, AutoIncrement]
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets from system identifier.
        /// </summary>
        /// <value>From system identifier.</value>
        [Indexed]
        public int StarSystemId { get; set; }

        /// <summary>
        /// Gets or sets to system identifier.
        /// </summary>
        /// <value>To system identifier.</value>
        [Indexed]
        public int ToSystemId { get; set; }

        /// <summary>
        /// Gets or sets the distance.
        /// </summary>
        /// <value>The distance.</value>
        public int Distance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.JumpLink"/> class.
        /// </summary>
        public JumpLink()
        {
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.JumpLink"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.JumpLink"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("[JumpLink: StarSystemId={0}, ToSystemId={1}, Distance={2}]", StarSystemId, ToSystemId, Distance);
		}
    }
}

