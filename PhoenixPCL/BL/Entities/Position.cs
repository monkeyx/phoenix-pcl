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
using System.Collections.Generic;

using SQLite.Net.Attributes; 

namespace Phoenix.BL.Entities
{
    [Table("Position")]
    public class Position : EntityBase
    {
		/// <summary>
		/// Position flags.
		/// </summary>
		public enum PositionFlag {
			None = 0x00,          
			GroundParty = 0x01,           
			Ship = 0x02,           
			Starbase = 0x04,           
			Political = 0x08,           
			Platform = 0x10,           
			Agent = 0x20,           
			Debris = 0x40   
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
		/// Gets or sets the type of the position.
		/// </summary>
		/// <value>The type of the position.</value>
		public int PositionType { get; set; }


		/// <summary>
		/// Gets the type of the position.
		/// </summary>
		/// <value>The type of the position.</value>
		public string PositionTypeString
		{
			get {
				List<string> positionTypes = new List<string>();
				foreach(var mask in Enum.GetValues(typeof(Position.PositionFlag))){
					Position.PositionFlag flag = (Position.PositionFlag)mask;
					if ((PositionType & (int)flag) != 0) {
						positionTypes.Add(
							System.Text.RegularExpressions.Regex.Replace (flag.ToString (), "(\\B[A-Z])", " $1")
						);
					}
				}
				return string.Join(", ", positionTypes);
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
		public override string ListDetail { get { return LocationText; } }

        /// <summary>
        /// Gets or sets the system text.
        /// </summary>
        /// <value>The system text.</value>
        public string SystemText { get; set; }

		/// <summary>
		/// Gets or sets the star system.
		/// </summary>
		/// <value>The star system.</value>
		[Ignore]
		public StarSystem StarSystem { get; set; }

		/// <summary>
        /// Gets or sets the location text.
        /// </summary>
        /// <value>The location text.</value>
        public string LocationText { 
			get{
				return _locationText;
			}
			set{
				ParseLocationText (value);
			}
		}

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
        public string PositionClass { 
			get {
				return positionClass;
			}
			set {
				positionClass = value;
				TryToDeterminePositionFlag ();
			}
		}

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
				return StarSystemName == null ? "None" : StarSystemName;
			}
		}

		// Parsed location data

		/// <summary>
		/// Gets or sets the star system identifier.
		/// </summary>
		/// <value>The star system identifier.</value>
		[Indexed]
		public int StarSystemId { get; set; }
		/// <summary>
		/// Gets or sets the name of the star system.
		/// </summary>
		/// <value>The name of the star system.</value>
		public string StarSystemName { get; set; }
		/// <summary>
		/// Gets or sets the quad.
		/// </summary>
		/// <value>The quad.</value>
		public StarSystem.QuadType Quad { get; set; }
		/// <summary>
		/// Gets or sets the ring.
		/// </summary>
		/// <value>The ring.</value>
		public int Ring { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Phoenix.BL.Entities.Position"/> is docked.
		/// </summary>
		/// <value><c>true</c> if docked; otherwise, <c>false</c>.</value>
		[Indexed]
		public bool Docked { get; set; }

		/// <summary>
		/// Gets or sets the docked position identifier.
		/// </summary>
		/// <value>The docked position identifier.</value>
		[Indexed]
		public int DockedPositionId { get; set; }
		/// <summary>
		/// Gets or sets the name of the docked position.
		/// </summary>
		/// <value>The name of the docked position.</value>
		public string DockedPositionName { get; set; }
		/// <summary>
		/// Gets or sets the type of the docked position.
		/// </summary>
		/// <value>The type of the docked position.</value>
		public string DockedPositionType { get; set; }
		/// <summary>
		/// Gets or sets the docked position aff.
		/// </summary>
		/// <value>The docked position aff.</value>
		public string DockedPositionAff { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Phoenix.BL.Entities.Position"/> is landed.
		/// </summary>
		/// <value><c>true</c> if landed; otherwise, <c>false</c>.</value>
		[Indexed]
		public bool Landed { get; set; }
		/// <summary>
		/// Gets or sets the planet identifier.
		/// </summary>
		/// <value>The planet identifier.</value>
		[Indexed]
		public int PlanetId { get; set; }
		/// <summary>
		/// Gets or sets the name of the planet.
		/// </summary>
		/// <value>The name of the planet.</value>
		public string PlanetName { get; set; }
		/// <summary>
		/// Gets or sets the landed x.
		/// </summary>
		/// <value>The landed x.</value>
		public int LandedX { get; set; }
		/// <summary>
		/// Gets or sets the landed y.
		/// </summary>
		/// <value>The landed y.</value>
		public int LandedY { get; set; }
		/// <summary>
		/// Gets or sets the landed terrain.
		/// </summary>
		/// <value>The landed terrain.</value>
		public string LandedTerrain { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Phoenix.BL.Entities.Position"/> is orbiting.
		/// </summary>
		/// <value><c>true</c> if orbiting; otherwise, <c>false</c>.</value>
		[Indexed]
		public bool Orbiting { get; set; }

		/// <summary>
		/// Gets or sets the market.
		/// </summary>
		/// <value>The market.</value>
		[Ignore]
		public MarketBase Market { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.Position"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.Position"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("{0} ({1})", Name, Id);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.Position"/> class.
        /// </summary>
        public Position ()
        {
        }

		private string _locationText;

		private void ParseLocationText(string value)
		{
			_locationText = value;

			if (string.IsNullOrWhiteSpace (value)) {
				return;
			}

			value = value.Trim ();

			if (value.StartsWith ("Docked")) {
				Docked = true;
				value = value.Replace ("Docked at ", "");
				string[] parts = value.Split (new string[]{ " - " }, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length > 0) {
					ParsePositionString (parts [0]);
				}
				if (parts.Length > 1) {
					ParseStarSystemString (parts [1]);
				}
			} else if (value.StartsWith ("Landed")) {
				Landed = true;
				value = value.Replace ("Landed on ", "");
				string[] parts = value.Split (new string[]{ " - " }, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length > 0) {
					string[] subparts = value.Split(new string[]{" at "}, StringSplitOptions.RemoveEmptyEntries);
					if (subparts.Length > 0) {
						ParsePlanetString (subparts [0]);
					}
					if (subparts.Length > 1) {
						subparts = value.Split(new string[]{" in "}, StringSplitOptions.RemoveEmptyEntries);
						if (subparts.Length > 0) {
							ParsePlanetCoordinateString (subparts [0]);
						}
						if (subparts.Length > 1) {
							LandedTerrain = subparts [1];
						}
					}
				}
				if (parts.Length > 1) {
					ParseSystemCoordinateString (parts [1]);
				}
				if (parts.Length > 2) {
					ParseStarSystemString (parts [2]);
				}
			} else if (value.Contains (" Orbit - ")) {
				Orbiting = true;
				value = value.Replace (" Orbit", "");
				string[] parts = value.Split (new string[]{ " - " }, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length > 0) {
					ParsePlanetString (parts [0]);
				}
				if (parts.Length > 1) {
					ParseSystemCoordinateString (parts [1]);
				}
				if (parts.Length > 2) {
					ParseStarSystemString (parts [2]);
				}
			} else if(value.Contains(" - ")){ // in space!?
				string[] parts = value.Split (new string[]{ " - " }, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length > 0) {
					ParseSystemCoordinateString (parts [0]);
				}
				if (parts.Length > 1) {
					ParseStarSystemString (parts [1]);
				}
			}
		}

		private void ParseStarSystemString(string value)
		{
			if (string.IsNullOrWhiteSpace (value)) {
				return;
			}
			value = value.Trim ();
			string[] parts = value.Split(new string[]{" System "}, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length > 0) {
				StarSystemName = parts [0];
			}
			if (parts.Length > 1) {
				StarSystemId = ParseBracketedIdentifier (parts [1]);
			}
		}

		private void ParsePositionString(string value)
		{
			if (string.IsNullOrWhiteSpace (value)) {
				return;
			}
			value = value.Trim ();
			string[] parts = value.Split(new string[]{" "}, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length > 0) {
				DockedPositionAff = parts [0];
			}
			if (parts.Length > 1) {
				DockedPositionType = parts [1];
			}
			if (parts.Length > 2) {
				DockedPositionName = parts [2];
			}
			if (parts.Length > 3) {
				DockedPositionId = ParseBracketedIdentifier (parts [3]);
			}
		}

		private void ParsePlanetString(string value)
		{
			if (string.IsNullOrWhiteSpace (value)) {
				return;
			}
			value = value.Trim ();
			string[] parts = value.Split(new string[]{" "}, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length > 0) {
				PlanetName = parts [0];
			}
			if (parts.Length > 1) {
				PlanetId = ParseBracketedIdentifier (parts [1]);
			}
		}

		private void ParsePlanetCoordinateString(string value)
		{
			if (string.IsNullOrWhiteSpace (value)) {
				return;
			}
			value = value.Trim ();
			value = value.Replace ("{", "").Replace ("}", "");
			string[] parts = value.Split(new string[]{","}, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length > 0) {
				try{
					LandedX = Int32.Parse(parts[0]);
				}
				catch{
				}
			}
			if (parts.Length > 1) {
				try{
					LandedX = Int32.Parse(parts[1]);
				}
				catch{
				}
			}
		}

		private void ParseSystemCoordinateString(string value)
		{
			if (string.IsNullOrWhiteSpace (value)) {
				return;
			}
			value = value.Trim ().Replace ("Quadrant ", "");
			string[] parts = value.Split(new string[]{" "}, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length > 0) {
				Quad = (StarSystem.QuadType) Enum.Parse (typeof(StarSystem.QuadType), parts [0]);
			}
			if (parts.Length > 1) {
				try{
					Ring = Int32.Parse(parts[1]);
				}
				catch{
				}
			}
		}

		private int ParseBracketedIdentifier(string value)
		{
			if (string.IsNullOrWhiteSpace (value)) {
				return 0;
			}
			value = value.Trim ();
			value = value.Replace ("(", "").Replace (")", "");
			try{
				return Int32.Parse(value);
			}
			catch{
				return 0;
			}
		}

		private void TryToDeterminePositionFlag()
		{
			PositionType = 0;
			if (string.IsNullOrWhiteSpace (positionClass)) {
				// urgh could be a lot of things
				PositionType |= (int)PositionFlag.Agent;
				PositionType |= (int)PositionFlag.Debris;
				PositionType |= (int)PositionFlag.GroundParty;
				PositionType |= (int)PositionFlag.Platform;
				PositionType |= (int)PositionFlag.Political;
			} else if (positionClass == "Starbase") {
				PositionType |= (int)PositionFlag.Starbase;
			} else if (positionClass == "Outpost") {
				PositionType |= (int)PositionFlag.Starbase;
			} else {
				PositionType |= (int)PositionFlag.Ship;
			}
		}

		private string positionClass;
    }

	/// <summary>
	/// Position turn.
	/// </summary>
	public class PositionTurn : EntityBase 
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[PrimaryKey]
		public override int Id { get; set; }

		/// <summary>
		/// Gets or sets the turn path.
		/// </summary>
		/// <value>The turn path.</value>
		[Ignore]
		public string TurnPath { 
			get {
				return Id.ToString () + ".html";
			}
		}

		/// <summary>
		/// Gets or sets the content.
		/// </summary>
		/// <value>The content.</value>
		[Ignore]
		public string Content { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.BL.Entities.PositionTurn"/> class.
		/// </summary>
		public PositionTurn()
		{
		}
	}

	/// <summary>
	/// Position notes.
	/// </summary>
	[Table("PositionNote")]
	public class PositionNote : EntityBase
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[PrimaryKey]
		public override int Id { get; set; }

		/// <summary>
		/// Gets or sets the note.
		/// </summary>
		/// <value>The note.</value>
		public string Note { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.BL.Entities.PositionNotes"/> class.
		/// </summary>
		public PositionNote()
		{
		}
	}
}

