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
    [Table("StarSystem")]
    public class StarSystem : EntityBase
    {
		/// <summary>
		/// Quad type.
		/// </summary>
		public enum QuadType 
		{
			Alpha = 1,
			Beta = 2,
			Gamma = 3,
			Delta = 4
		}

		/// <summary>
		/// Periphery.
		/// </summary>
		public enum Periphery
		{
			None = 0,
			InnerCapellan = 1,
			Darkfold = 2,
			Cluster = 3,
			DewiekHome = 4,
			DewiekPocket = 5,
			DetinusRepublic = 6,
			Twilight = 7,
			InnerEmpire = 8,
			Caliphate = 9,
			FlagritzHome = 10,
			FeliniEmpire = 11,
			OuterCapellan = 12,
			Halo = 13,
			CorewardArm = 14,
			Transpiral = 15,
			OrionSpur = 16,
			PerfidionReach = 17
		}

		/// <summary>
		/// Periphery abbreviation.
		/// </summary>
		public enum PeripheryAbbreviation
		{
			N = 0,
			IC = 1,
			DAR = 2,
			CLU = 3,
			DHP = 4,
			DPP = 5,
			DTR = 6,
			TWI = 7,
			IE = 8,
			CAL = 9,
			FLZ = 10,
			FEL = 11,
			OC = 12,
			HAL = 13,
			CA = 14,
			TSP = 15,
			OS = 16,
			PR = 17
		}

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[PrimaryKey]
		public override int Id { get; set; }

		/// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Indexed]
        public string Name { get; set; }

		/// <summary>
		/// Gets or sets the system periphery.
		/// </summary>
		/// <value>The system periphery.</value>
		[Indexed]
		public Periphery SystemPeriphery { get; set; }

		/// <summary>
		/// Gets the name of the periphery.
		/// </summary>
		/// <value>The name of the periphery.</value>
		[Ignore]
		public string PeripheryName { 
			get {
				return System.Text.RegularExpressions.Regex.Replace (SystemPeriphery.ToString(), "(\\B[A-Z])", " $1");
			}
		}

		/// <summary>
		/// Gets the name of the periphery short.
		/// </summary>
		/// <value>The name of the periphery short.</value>
		[Ignore]
		public string PeripheryShortName {
			get {
				int peripheryCode = (int)SystemPeriphery;
				PeripheryAbbreviation pa = (PeripheryAbbreviation)peripheryCode;
				return pa.ToString ();
			}
		}

		/// <summary>
		/// Gets the name and identifier.
		/// </summary>
		/// <value>The name and identifier.</value>
		[Ignore]
		public string NameAndId { 
			get { 
				return Name + " (" + Id + ")";
			}
		}

		/// <summary>
		/// Gets the list text.
		/// </summary>
		/// <value>The list text.</value>
		[Ignore]
		public override string ListText { get { return NameAndId; } }

		/// <summary>
		/// Gets the list detail.
		/// </summary>
		/// <value>The list detail.</value>
		[Ignore]
		public override string ListDetail { get { return PeripheryName; } }

        /// <summary>
        /// Gets or sets the celestial bodies.
        /// </summary>
        /// <value>The celestial bodies.</value>
        [Ignore]
		public List<CelestialBody> CelestialBodies { get; set; } = new List<CelestialBody> ();

        /// <summary>
        /// Gets or sets the jump links.
        /// </summary>
        /// <value>The jump links.</value>
        [Ignore]
		public List<JumpLink> JumpLinks { get; set; } = new List<JumpLink> ();

		/// <summary>
		/// Gets the group that the entity belongs to
		/// </summary>
		/// <value>The group.</value>
		[Ignore]
		public override string Group { 
			get { 
				return PeripheryName;
			}
		}

		/// <summary>
		/// Gets the group short name the entity belongs to
		/// </summary>
		/// <value>The group short name.</value>
		[Ignore]
		public override string GroupShortName {
			get {
				return PeripheryShortName;
			}
		}

		/// <summary>
		/// Gets the body.
		/// </summary>
		/// <returns>The body.</returns>
		/// <param name="localId">Local identifier.</param>
		public CelestialBody GetCelestialBody(int localId)
		{
			foreach (CelestialBody body in CelestialBodies) {
				if (body.LocalCelestialBodyId == localId) {
					return body;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets or sets the markets.
		/// </summary>
		/// <value>The markets.</value>
		[Ignore]
		public List<MarketBase> Markets { get; set; } = new List<MarketBase>();

		/// <summary>
		/// Determines whether this star system is linked the specified star system by identifier.
		/// </summary>
		/// <returns><c>true</c> if this instance is linked the specified to; otherwise, <c>false</c>.</returns>
		/// <param name="toStarSystemId">Target star system identifier</param>
		public bool IsLinked(int toStarSystemId)
		{
			if (toStarSystemId == 0)
				return false;
			if (toStarSystemId == Id)
				return true;
			foreach (JumpLink jl in JumpLinks) {
				if (jl.ToStarSystemId == toStarSystemId) {
					return true;
				}
			}
			return false;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.StarSystem"/> class.
        /// </summary>
        public StarSystem ()
        {
			
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
		/// Body type.
		/// </summary>
		public enum BodyType
		{
			AsteroidA = 4,
			AsteroidB = 10,
			AsteroidBelt = 5,
			GasGiant = 3,
			Moon = 2,
			Nebula = 6,
			Planet = 1,
			Wormhole = 7
		}

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
		/// Gets the name and identifier.
		/// </summary>
		/// <value>The name and identifier.</value>
		[Ignore]
		public string NameAndId { 
			get { 
				return Name + " (" + Id + ")";
			}
		}

		/// <summary>
		/// Gets the list text.
		/// </summary>
		/// <value>The list text.</value>
		[Ignore]
		public override string ListText { get { return NameAndId; } }

		/// <summary>
		/// Gets the list detail.
		/// </summary>
		/// <value>The list detail.</value>
		[Ignore]
		public override string ListDetail { get { return Location; } }

        /// <summary>
        /// Gets or sets the quad.
        /// </summary>
        /// <value>The quad.</value>
        [Indexed]
		public StarSystem.QuadType Quad { get; set; }

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
		public BodyType CBodyType { get; set; }

		/// <summary>
		/// Gets the location.
		/// </summary>
		/// <value>The location.</value>
		[Ignore]
		public string Location {
			get {
				return Quad.ToString () + " " + Ring;
			}
		}

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
			return string.Format ("{0} ({1}) [{2}]", Name, LocalCelestialBodyId, CBodyType);
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
		/// Gets the list text.
		/// </summary>
		/// <value>The list text.</value>
		[Ignore]
		public override string ListText { get { return ToStarSystemName; } }

		/// <summary>
		/// Gets the list detail.
		/// </summary>
		/// <value>The list detail.</value>
		[Ignore]
		public override string ListDetail { get { return DistanceString; } }

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
        public int ToStarSystemId { get; set; }

		/// <summary>
		/// Gets or sets to star sysytem.
		/// </summary>
		/// <value>To star sysytem.</value>
		[Ignore]
		public StarSystem ToStarSysytem { get; set; }

		/// <summary>
		/// Gets the name of the to star system.
		/// </summary>
		/// <value>The name of the to star system.</value>
		[Ignore]
		public string ToStarSystemName { 
			get {
				return ToStarSysytem == null ? "Unknown" : ToStarSysytem.ToString ();
			}
		}

        /// <summary>
        /// Gets or sets the distance.
        /// </summary>
        /// <value>The distance.</value>
        public int Distance { get; set; }

		/// <summary>
		/// Gets the distance string.
		/// </summary>
		/// <value>The distance string.</value>
		[Ignore]
		public string DistanceString {
			get {
				return Distance.ToString () + (Distance == 1 ? " jump" : " jumps");
			}
		}

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
			return string.Format ("[JumpLink: StarSystemId={0}, ToSystemId={1}, Distance={2}]", StarSystemId, ToStarSystemId, Distance);
		}
    }
}

