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
using SQLite.Net.Attributes;

namespace Phoenix.BL.Entities
{
    /// <summary>
    /// Star system.
    /// </summary>
    [Table("star_systems")]
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
        }
    }

    /// <summary>
    /// Celestial body.
    /// </summary>
    [Table("celestial_bodies")]
    public class CelestialBody : EntityBase 
    {
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
    }

    /// <summary>
    /// Jump link.
    /// </summary>
    [Table("jump_links")]
    public class JumpLink : EntityBase
    {
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
    }
}

