//
// Notification.cs
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
	[Table("Notification")]
	public class Notification : EntityBase
	{
		/// <summary>
		/// Notification type.
		/// </summary>
		public enum NotificationType
		{
			None = 0,
			Turns = 1,
			NewPosition = 2,
			ActiveMission = 3,
			NewMission = 4,
			Battles = 5,
			Deliveries = 6,
			Pickups = 7,
			Buys = 8,
			Sells = 9,
			TransferIn = 10,
			PositionTransfers = 11,
			RelationsChanges = 12,
			SystemCharters = 13,
			NewRestrictedStarbases = 14,
			NewRestrictedCelestials = 15,
			NewRestrictedSystemLink = 16,
			NewRestrictedItems = 17,
			NewRestrictedOrders = 18,
			NewRestrictedMissions = 19,
			NewRestrictedSystemLocations = 20,
			DeliveredTo = 21,
			PickedUpFrom = 22,
			MarketSells = 23,
			MarketBuys = 24,
			Boarding = 25,
			Raiding = 26,
			CombatTransactions = 27,
			SoldPosition = 28,
			Restricted = 29,
			PlanetarySales = 30,
			NexusRequestFacebook = 31,
			GamesmasterNote = 32,
			Rumour = 33,
			Spotted = 34,
			TurnError = 35,
			Warning = 36,
			ResearchFinished = 37,
			ComplexVisited = 38,
			TransferOut = 39,
			OrbitalDrop = 40,
			OrbitalResupply = 41,
			SpecialAction = 42,
			Message = 43,
			AgentAction = 44,
			Reminder = 45,
			ComplexChange = 46,
			AccountLow = 47,
			RegisteredBaseActivity = 48,
			OpportunityFire = 49,
			EscapingCombat = 50
		}

		/// <summary>
		/// Notification priority.
		/// </summary>
		public enum NotificationPriority
		{
			Green = 0,
			Amber = 1,
			Red = 2,
			All = 3
		}

		/// <summary>
		/// Notification position type.
		/// </summary>
		public enum NotificationPositionType
		{
			None = 0,
			GroundParty = 1,
			Ship = 2,
			Starbase = 3,
			Debris = 4,
			Political = 5,
			Platform = 6,
			Agent = 7
		}

		/// <summary>
		/// Planetary trade category.
		/// </summary>
		public enum PlanetaryTradeCategory
		{
			TradeGoods = 0,
			Life = 1,
			Drugs = 2
		}

		/// <summary>
		/// Complex visit type.
		/// </summary>
		public enum ComplexVisitType
		{
			Recreation = 0,
			Repair = 1,
			Maintenance = 2,
			Upgrade = 3,
			Refit = 4
		}

		/// <summary>
		/// Complex change type.
		/// </summary>
		public enum ComplexChangeType
		{
			Built = 0,
			Scrapped = 1,
			Activated = 2,
			Deactivated = 3
		}

		/// <summary>
		/// Registered base activity type.
		/// </summary>
		public enum RegisteredBaseActivityType
		{
			Registered = 0,
			Deregistered = 1,
			Attacked = 2,
			Bought = 3
		}

		/// <summary>
		/// Warning type.
		/// </summary>
		public enum WarningType
		{
			DeepCoreStructure = 0,
			ComplexesNotProducing = 1,
			StarbaseLowEfficiency = 2,
			StarbaseLowSecurity = 3,
			ShipLowIntegrity = 4,
			Scouted = 5,
			BaseSubverted = 6,
			BaseRegistrationRevoked = 7
		}

		/// <summary>
		/// Error type.
		/// </summary>
		public enum ErrorType
		{
			NoStargateKey = 0,
			UnknownBase = 1,
			UnknownStarSystem = 2,
			UnknownCelestialBody = 3,
			IncorrectSecurity = 4,
			NoThrust = 5,
			MaintenanceFailed = 6
		}

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[PrimaryKey]
		public override int Id { get; set; }

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
		[Indexed]
		public NotificationType Type { get; set; }

		/// <summary>
		/// Gets the list text.
		/// </summary>
		/// <value>The list text.</value>
		[Ignore]
		public override string ListText { 
			get { 
				return GetTitle ();
			} 
		}

		/// <summary>
		/// Gets the list detail.
		/// </summary>
		/// <value>The list detail.</value>
		[Ignore]
		public override string ListDetail { 
			get { 
				return GetDetail ();
			} 
		}

		[Ignore]
		public override string Group { 
			get { 
				return TypeDescription;
			}
		}

		/// <summary>
		/// Gets or sets the priority.
		/// </summary>
		/// <value>The priority.</value>
		[Indexed]
		public NotificationPriority Priority { get; set; }

		/// <summary>
		/// Gets or sets the day.
		/// </summary>
		/// <value>The day.</value>
		[Indexed]
		public int Day { get; set; }

		/// <summary>
		/// Gets or sets the days ago.
		/// </summary>
		/// <value>The days ago.</value>
		[Indexed]
		public int DaysAgo { get; set; }

		/// <summary>
		/// Gets or sets the position identifier.
		/// </summary>
		/// <value>The position identifier.</value>
		public int PositionId { get; set; }

		/// <summary>
		/// Gets or sets the name of the position.
		/// </summary>
		/// <value>The name of the position.</value>
		public string PositionName { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>The position.</value>
		[Ignore]
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets the star system identifier.
		/// </summary>
		/// <value>The system identifier.</value>
		public int StarSystemId { get; set; }

		/// <summary>
		/// Gets or sets the name of the star system.
		/// </summary>
		/// <value>The name of the system.</value>
		public string StarSystemName { get; set; }

		/// <summary>
		/// Gets or sets the system.
		/// </summary>
		/// <value>The system.</value>
		[Ignore]
		public StarSystem StarSystem { get; set; }

		/// <summary>
		/// Gets or sets the celestial body identifier.
		/// </summary>
		/// <value>The celestial body identifier.</value>
		public int CelestialBodyId { get; set; }

		/// <summary>
		/// Gets or sets the name of the celestial body.
		/// </summary>
		/// <value>The name of the celestial body.</value>
		public string CelestialBodyName { get; set; }

		/// <summary>
		/// Gets or sets the celestial body.
		/// </summary>
		/// <value>The celestial body.</value>
		[Ignore]
		public CelestialBody CelestialBody { get; set; }

		/// <summary>
		/// Gets or sets the order identifier.
		/// </summary>
		/// <value>The order identifier.</value>
		public int OrderId { get; set; }

		/// <summary>
		/// Gets or sets the name of the order.
		/// </summary>
		/// <value>The name of the order.</value>
		public string OrderName { get; set; }

		/// <summary>
		/// Gets or sets the order.
		/// </summary>
		/// <value>The order.</value>
		[Ignore]
		public OrderType Order { get; set; }

		/// <summary>
		/// Gets or sets the type of the position.
		/// </summary>
		/// <value>The type of the position.</value>
		public NotificationPositionType PositionType { get; set; }

		/// <summary>
		/// Gets or sets the squadron identifier.
		/// </summary>
		/// <value>The squadron identifier.</value>
		public int SquadronId { get; set; }

		/// <summary>
		/// Gets or sets the name of the squadron.
		/// </summary>
		/// <value>The name of the squadron.</value>
		public string SquadronName { get; set; }

		/// <summary>
		/// Gets or sets the mission identifier.
		/// </summary>
		/// <value>The mission identifier.</value>
		public int MissionId { get; set; }

		/// <summary>
		/// Gets or sets the location.
		/// </summary>
		/// <value>The location.</value>
		public string Location { get; set; }

		/// <summary>
		/// Gets or sets the number of ships.
		/// </summary>
		/// <value>The number of ships.</value>
		public int NumberOfShips { get; set; }

		/// <summary>
		/// Gets or sets the quantity.
		/// </summary>
		/// <value>The quantity.</value>
		public int Quantity { get; set; }

		/// <summary>
		/// Gets or sets the item identifier.
		/// </summary>
		/// <value>The item identifier.</value>
		public int ItemId { get; set; }

		/// <summary>
		/// Gets or sets the name of the item.
		/// </summary>
		/// <value>The name of the item.</value>
		public string ItemName { get; set; }

		/// <summary>
		/// Gets or sets the item.
		/// </summary>
		/// <value>The item.</value>
		[Ignore]
		public Item Item { get; set; }

		/// <summary>
		/// Gets or sets the base identifier.
		/// </summary>
		/// <value>The base identifier.</value>
		public int BaseId { get; set; }

		/// <summary>
		/// Gets or sets the name of the base.
		/// </summary>
		/// <value>The name of the base.</value>
		public string BaseName { get; set; }

		/// <summary>
		/// Gets or sets the base.
		/// </summary>
		/// <value>The base.</value>
		[Ignore]
		public Position Base { get; set; }

		/// <summary>
		/// Gets or sets the deliver to identifier.
		/// </summary>
		/// <value>The deliver to identifier.</value>
		public int DeliverToId { get; set; }

		/// <summary>
		/// Gets or sets the name of the deliver to.
		/// </summary>
		/// <value>The name of the deliver to.</value>
		public string DeliverToName { get; set; }

		/// <summary>
		/// Gets or sets the deliver to.
		/// </summary>
		/// <value>The deliver to.</value>
		[Ignore]
		public Position DeliverTo { get; set; }

		/// <summary>
		/// Gets or sets the picked up from identifier.
		/// </summary>
		/// <value>The picked up from identifier.</value>
		public int PickedUpFromId { get; set; }

		/// <summary>
		/// Gets or sets the name of the picked up from.
		/// </summary>
		/// <value>The name of the picked upfrom.</value>
		public string PickedUpFromName { get; set; }

		/// <summary>
		/// Gets or sets the picked up from.
		/// </summary>
		/// <value>The picked up from.</value>
		[Ignore]
		public Position PickedUpFrom { get; set; }

		/// <summary>
		/// Gets or sets the bought from identifier.
		/// </summary>
		/// <value>The bought from identifier.</value>
		public int BoughtFromId { get; set; }

		/// <summary>
		/// Gets or sets the name of the bought from.
		/// </summary>
		/// <value>The name of the bought from.</value>
		public string BoughtFromName { get; set; }

		/// <summary>
		/// Gets or sets the bought from.
		/// </summary>
		/// <value>The bought from.</value>
		[Ignore]
		public Position BoughtFrom { get; set; }

		/// <summary>
		/// Gets or sets the sold to identifier.
		/// </summary>
		/// <value>The sold to identifier.</value>
		public int SoldToId { get; set; }

		/// <summary>
		/// Gets or sets the name of the sold to.
		/// </summary>
		/// <value>The name of the sold to.</value>
		public string SoldToName { get; set; }

		/// <summary>
		/// Gets or sets the sold to.
		/// </summary>
		/// <value>The sold to.</value>
		[Ignore]
		public Position SoldTo { get; set; }

		/// <summary>
		/// Gets or sets the by position identifier.
		/// </summary>
		/// <value>The by position identifier.</value>
		public int ByPositionId { get; set; }

		/// <summary>
		/// Gets or sets the name of the by position.
		/// </summary>
		/// <value>The name of the by position.</value>
		public string ByPositionName { get; set; }

		/// <summary>
		/// Gets or sets the by position.
		/// </summary>
		/// <value>The by position.</value>
		[Ignore]
		public Position ByPosition { get; set; }

		/// <summary>
		/// Gets or sets the stellars.
		/// </summary>
		/// <value>The stellars.</value>
		public int Stellars { get; set; }

		/// <summary>
		/// Gets or sets the transfer identifier.
		/// </summary>
		/// <value>The transfer identifier.</value>
		public int TransferId { get; set; }

		/// <summary>
		/// Gets or sets the affiliation identifier.
		/// </summary>
		/// <value>The affiliation identifier.</value>
		public int AffiliationId { get; set; }

		/// <summary>
		/// Gets or sets the name of the affiliation.
		/// </summary>
		/// <value>The name of the affiliation.</value>
		public string AffiliationName { get; set;}

		/// <summary>
		/// Gets or sets the charter identifier.
		/// </summary>
		/// <value>The charter identifier.</value>
		public int CharterId { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		public string Status { get; set; }

		/// <summary>
		/// Gets or sets the type of the trade.
		/// </summary>
		/// <value>The type of the trade.</value>
		public PlanetaryTradeCategory TradeType { get; set; }

		/// <summary>
		/// Gets or sets the group identifier.
		/// </summary>
		/// <value>The group identifier.</value>
		public int GroupId { get; set; }

		/// <summary>
		/// Gets or sets the subject.
		/// </summary>
		/// <value>The subject.</value>
		public string Subject { get; set; }

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the error code.
		/// </summary>
		/// <value>The error code.</value>
		public ErrorType ErrorCode { get; set; }

		/// <summary>
		/// Gets or sets the error message.
		/// </summary>
		/// <value>The error message.</value>
		public string ErrorMessage { get; set; }

		/// <summary>
		/// Gets or sets the warning code.
		/// </summary>
		/// <value>The warning code.</value>
		public WarningType WarningCode { get; set; }

		/// <summary>
		/// Gets or sets the warning message.
		/// </summary>
		/// <value>The warning message.</value>
		public string WarningMessage { get; set; }

		/// <summary>
		/// Gets or sets the type of the complex.
		/// </summary>
		/// <value>The type of the complex.</value>
		public string ComplexType { get; set; }

		/// <summary>
		/// Gets or sets the name of the complex type.
		/// </summary>
		/// <value>The name of the complex type.</value>
		public string ComplexTypeName { get; set; }

		/// <summary>
		/// Gets or sets the complex item.
		/// </summary>
		/// <value>The complex item.</value>
		[Ignore]
		public Item ComplexItem { get; set; }

		/// <summary>
		/// Gets or sets the type of the change.
		/// </summary>
		/// <value>The type of the change.</value>
		public ComplexChangeType ChangeType { get; set; }

		/// <summary>
		/// Gets or sets the type of the base activity.
		/// </summary>
		/// <value>The type of the base activity.</value>
		public RegisteredBaseActivityType BaseActivityType { get; set; }

		/// <summary>
		/// Gets the type description.
		/// </summary>
		/// <value>The type description.</value>
		[Ignore]
		public string TypeDescription
		{
			get {
				return System.Text.RegularExpressions.Regex.Replace (Type.ToString(), "(\\B[A-Z])", " $1");
			}
		}

		/// <summary>
		/// Gets the position type description.
		/// </summary>
		/// <value>The position type description.</value>
		[Ignore]
		public string PositionTypeDescription {
			get {
				return System.Text.RegularExpressions.Regex.Replace (PositionType.ToString(), "(\\B[A-Z])", " $1");
			}
		}

		/// <summary>
		/// Gets the warning code description.
		/// </summary>
		/// <value>The warning code description.</value>
		[Ignore]
		public string WarningCodeDescription {
			get {
				return System.Text.RegularExpressions.Regex.Replace (WarningCode.ToString(), "(\\B[A-Z])", " $1");
			}
		}

		/// <summary>
		/// Gets the error code description.
		/// </summary>
		/// <value>The error code description.</value>
		[Ignore]
		public string ErrorCodeDescription {
			get {
				return System.Text.RegularExpressions.Regex.Replace (ErrorCode.ToString(), "(\\B[A-Z])", " $1");
			}
		}

		/// <summary>
		/// Sets the data.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="value">Value.</param>
		public void SetData(int index, string value)
		{
			switch (index) {
			case 1:
				SetData1 (value);
				break;
			case 2:
				SetData2 (value);
				break;
			case 3:
				SetData3 (value);
				break;
			case 4:
				SetData4 (value);
				break;
			case 5:
				SetData5 (value);
				break;
			case 6:
				SetData6 (value);
				break;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.BL.Entities.Notification"/> class.
		/// </summary>
		public Notification ()
		{
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.Notification"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.Notification"/>.</returns>
		public override string ToString ()
		{
			return GetTitle ();
		}

		private string GetTitle()
		{
			string title = null;
			switch (Type) {
			case NotificationType.Turns:
				title = PositionName;
				break;
			case NotificationType.NewPosition:
				title = PositionName;
				break;
			case NotificationType.ActiveMission:
				title = PositionName;
				break;
			case NotificationType.NewMission:
				title = PositionName;
				break;
			case NotificationType.Battles:
				title = Location;
				break;
			case NotificationType.Deliveries:
				title = PositionName;
				break;
			case NotificationType.Pickups:
				title = PositionName;
				break;
			case NotificationType.Buys:
				title = PositionName;
				break;
			case NotificationType.Sells:
				title = PositionName;
				break;
			case NotificationType.TransferIn:
				title = PositionName;
				break;
			case NotificationType.PositionTransfers:
				title = PositionName;
				break;
			case NotificationType.RelationsChanges:
				title = AffiliationName;
				break;
			case NotificationType.SystemCharters:
				title = AffiliationName;
				break;
			case NotificationType.NewRestrictedStarbases:
				title = PositionName;
				break;
			case NotificationType.NewRestrictedCelestials:
				title = StarSystemName;
				break;
			case NotificationType.NewRestrictedSystemLink:
				title = StarSystemName;
				break;
			case NotificationType.NewRestrictedItems:
				title = ItemName;
				break;
			case NotificationType.NewRestrictedOrders:
				title = "New Order";
				break;
			case NotificationType.NewRestrictedMissions:
				title = "New Mission";
				break;
			case NotificationType.NewRestrictedSystemLocations:
				title = StarSystemName;
				break;
			case NotificationType.DeliveredTo:
				title = PositionName;
				break;
			case NotificationType.PickedUpFrom:
				title = PositionName;
				break;
			case NotificationType.MarketSells:
				title = PositionName;
				break;
			case NotificationType.MarketBuys:
				title = PositionName;
				break;
			case NotificationType.Boarding:
				title = Location;
				break;
			case NotificationType.Raiding:
				title = Location;
				break;
			case NotificationType.CombatTransactions:
				title = Location;
				break;
			case NotificationType.SoldPosition:
				title = PositionName;
				break;
			case NotificationType.Restricted:
				title = "New Restricted Data";
				break;
			case NotificationType.PlanetarySales:
				title = PositionName;
				break;
			case NotificationType.NexusRequestFacebook:
				title = PositionName;
				break;
			case NotificationType.GamesmasterNote:
				title = PositionName;
				break;
			case NotificationType.Rumour:
				title = "New Rumour";
				break;
			case NotificationType.Spotted:
				title = PositionName;
				break;
			case NotificationType.TurnError:
				title = PositionName;
				break;
			case NotificationType.Warning:
				title = PositionName;
				break;
			case NotificationType.ResearchFinished:
				title = PositionName;
				break;
			case NotificationType.ComplexVisited:
				title = PositionName;
				break;
			case NotificationType.TransferOut:
				title = PositionName;
				break;
			case NotificationType.OrbitalDrop:
				title = PositionName;
				break;
			case NotificationType.OrbitalResupply:
				title = PositionName;
				break;
			case NotificationType.SpecialAction:
				title = PositionName;
				break;
			case NotificationType.Message:
				title = PositionName;
				break;
			case NotificationType.AgentAction:
				title = PositionName;
				break;
			case NotificationType.Reminder:
				title = PositionName;
				break;
			case NotificationType.ComplexChange:
				title = PositionName;
				break;
			case NotificationType.AccountLow:
				title = "Account Balance Low";
				break;
			case NotificationType.RegisteredBaseActivity:
				title = PositionName;
				break;
			case NotificationType.OpportunityFire:
				title = Location;
				break;
			case NotificationType.EscapingCombat:
				title = Location;
				break;
			}
			return title;
		}

		private string GetDetail()
		{
			List<string> details = new List<string> ();

			if (DaysAgo > 0) {
				details.Add( DaysAgo + (DaysAgo == 1 ? " day ago" : " days ago"));
			} else {
				details.Add("Today");
			}

			switch (Type) {
			case NotificationType.Turns:
				details.Add (StarSystemName);
				break;
			case NotificationType.NewPosition:
				details.Add (StarSystemName);
				break;
			case NotificationType.ActiveMission:
				details.Add (PositionName);
				break;
			case NotificationType.Battles:
				details.Add ("Ships: " + NumberOfShips);
				break;
			case NotificationType.Deliveries:
				break;
			case NotificationType.Pickups:
				break;
			case NotificationType.Buys:
				break;
			case NotificationType.Sells:
				break;
			case NotificationType.TransferIn:
				details.Add ("$" + Stellars);
				break;
			case NotificationType.PositionTransfers:
				break;
			case NotificationType.RelationsChanges:
				details.Add (Status);
				break;
			case NotificationType.SystemCharters:
				details.Add (Status);
				break;
			case NotificationType.NewRestrictedCelestials:
				details.Add (CelestialBodyName);
				break;
			case NotificationType.DeliveredTo:
				break;
			case NotificationType.PickedUpFrom:
				break;
			case NotificationType.MarketSells:
				break;
			case NotificationType.MarketBuys:
				break;
			case NotificationType.Boarding:
				details.Add (StarSystemName);
				break;
			case NotificationType.Raiding:
				details.Add (StarSystemName);
				break;
			case NotificationType.CombatTransactions:
				details.Add (StarSystemName);
				break;
			case NotificationType.SoldPosition:
				details.Add ("$" + Stellars);
				break;
			case NotificationType.Restricted:
				break;
			case NotificationType.PlanetarySales:
				details.Add (StarSystemName);
				break;
			case NotificationType.NexusRequestFacebook:
				details.Add ("$" + Stellars);
				break;
			case NotificationType.GamesmasterNote:
				break;
			case NotificationType.Spotted:
				details.Add (StarSystemName);
				break;
			case NotificationType.TurnError:
				details.Add (StarSystemName);
				break;
			case NotificationType.Warning:
				details.Add (StarSystemName);
				break;
			case NotificationType.ResearchFinished:
				details.Add (ItemName);
				break;
			case NotificationType.ComplexVisited:
				details.Add (BaseName);
				break;
			case NotificationType.TransferOut:
				details.Add ("$" + Stellars);
				break;
			case NotificationType.OrbitalDrop:
				details.Add (StarSystemName);
				break;
			case NotificationType.OrbitalResupply:
				details.Add (StarSystemName);
				break;
			case NotificationType.SpecialAction:
				break;
			case NotificationType.Message:
				break;
			case NotificationType.AgentAction:
				details.Add (StarSystemName);
				break;
			case NotificationType.Reminder:
				break;
			case NotificationType.ComplexChange:
				// TODO fix // details.Add (ComplexTypeName);
				break;
			case NotificationType.RegisteredBaseActivity:
				details.Add (StarSystemName);
				break;
			case NotificationType.OpportunityFire:
				details.Add (StarSystemName);
				break;
			case NotificationType.EscapingCombat:
				details.Add (StarSystemName);
				break;
			}

			switch (Type) {
			case NotificationType.Turns:
				details.Add (PositionTypeDescription);
				break;
			case NotificationType.NewPosition:
				details.Add (PositionTypeDescription);
				break;
			case NotificationType.Battles:
				details.Add (StarSystemName);
				break;
			case NotificationType.Deliveries:
				details.Add ("x " + Quantity);
				break;
			case NotificationType.Pickups:
				details.Add ("x " + Quantity);
				break;
			case NotificationType.Buys:
				details.Add ("x " + Quantity);
				break;
			case NotificationType.Sells:
				details.Add ("x " + Quantity);
				break;
			case NotificationType.PositionTransfers:
				details.Add (PositionTypeDescription);
				break;
			case NotificationType.SystemCharters:
				break;
			case NotificationType.DeliveredTo:
				details.Add ("x " + Quantity);
				break;
			case NotificationType.PickedUpFrom:
				details.Add ("x " + Quantity);
				break;
			case NotificationType.MarketSells:
				details.Add ("x " + Quantity);
				break;
			case NotificationType.MarketBuys:
				details.Add ("x " + Quantity);
				break;
			case NotificationType.Boarding:
				details.Add (PositionName);
				break;
			case NotificationType.Raiding:
				details.Add (PositionName);
				break;
			case NotificationType.CombatTransactions:
				details.Add (PositionName);
				break;
			case NotificationType.Restricted:
				break;
			case NotificationType.PlanetarySales:
				details.Add (CelestialBodyName);
				break;
			case NotificationType.GamesmasterNote:
				details.Add (StarSystemName);
				break;
			case NotificationType.Spotted:
				details.Add (Location);
				break;
			case NotificationType.TurnError:
				details.Add (CelestialBodyName);
				break;
			case NotificationType.Warning:
				details.Add (CelestialBodyName);
				break;
			case NotificationType.ComplexVisited:
				details.Add (ComplexTypeName);
				break;
			case NotificationType.OrbitalDrop:
				details.Add ("x " + Quantity);
				break;
			case NotificationType.OrbitalResupply:
				details.Add ("x " + Quantity);
				break;
			case NotificationType.AgentAction:
				break;
			case NotificationType.ComplexChange:
				details.Add (ChangeType.ToString ());
				break;
			case NotificationType.RegisteredBaseActivity:
				details.Add (CelestialBodyName);
				break;
			case NotificationType.OpportunityFire:
				break;
			case NotificationType.EscapingCombat:
				break;
			}

			switch (Type) {
			case NotificationType.Turns:
				details.Add (SquadronName);
				break;
			case NotificationType.NewPosition:
				details.Add (SquadronName);
				break;
			case NotificationType.Deliveries:
				details.Add (ItemName);
				break;
			case NotificationType.Pickups:
				details.Add (ItemName);
				break;
			case NotificationType.Buys:
				details.Add (ItemName);
				break;
			case NotificationType.Sells:
				details.Add (ItemName);
				break;
			case NotificationType.PositionTransfers:
				break;
			case NotificationType.DeliveredTo:
				details.Add (ItemName);
				break;
			case NotificationType.PickedUpFrom:
				details.Add (ItemName);
				break;
			case NotificationType.MarketSells:
				details.Add (ItemName);
				break;
			case NotificationType.MarketBuys:
				details.Add (ItemName);
				break;
			case NotificationType.PlanetarySales:
				details.Add (TradeType.ToString());
				break;
			case NotificationType.GamesmasterNote:
				details.Add (CelestialBodyName);
				break;
			case NotificationType.TurnError:
				details.Add (ErrorCodeDescription);
				break;
			case NotificationType.Warning:
				details.Add (WarningCodeDescription);
				break;
			case NotificationType.OrbitalDrop:
				details.Add (ItemName);
				break;
			case NotificationType.OrbitalResupply:
				details.Add (ItemName);
				break;
			case NotificationType.ComplexChange:
				details.Add ("x " + Quantity);
				break;
			case NotificationType.RegisteredBaseActivity:
				details.Add (BaseActivityType.ToString ());
				break;
			}

			switch (Type) {
			case NotificationType.Deliveries:
				details.Add (DeliverToName);
				break;
			case NotificationType.Pickups:
				details.Add (PickedUpFromName);
				break;
			case NotificationType.Buys:
				details.Add (BoughtFromName);
				break;
			case NotificationType.Sells:
				details.Add (SoldToName);
				break;
			case NotificationType.DeliveredTo:
				details.Add (ByPositionName);
				break;
			case NotificationType.PickedUpFrom:
				details.Add (ByPositionName);
				break;
			case NotificationType.MarketSells:
				details.Add (ByPositionName);
				break;
			case NotificationType.MarketBuys:
				details.Add (ByPositionName);
				break;
			case NotificationType.PlanetarySales:
				details.Add ("$" + Stellars);
				break;
			case NotificationType.GamesmasterNote:
				break;
			case NotificationType.TurnError:
				break;
			case NotificationType.Warning:
				break;
			case NotificationType.OrbitalDrop:
				details.Add (ByPositionName);
				break;
			case NotificationType.OrbitalResupply:
				details.Add (ByPositionName);
				break;
			}

			switch (Type) {
			case NotificationType.Buys:
				details.Add ("$" + Stellars);
				break;
			case NotificationType.Sells:
				details.Add ("$" + Stellars);
				break;
			case NotificationType.MarketSells:
				details.Add ("$" + Stellars);
				break;
			case NotificationType.MarketBuys:
				details.Add ("$" + Stellars);
				break;
			}

			return string.Join (", ", details);
		}

		private void SetData1(string value)
		{
			switch (Type) {
			case NotificationType.Turns:
				PositionId = ParseInt (value);
				break;
			case NotificationType.NewPosition:
				PositionId = ParseInt (value);
				break;
			case NotificationType.ActiveMission:
				MissionId = ParseInt (value);
				break;
			case NotificationType.NewMission:
				MissionId = ParseInt (value);
				break;
			case NotificationType.Battles:
				Location = value;
				break;
			case NotificationType.Deliveries:
				PositionId = ParseInt (value);
				break;
			case NotificationType.Pickups:
				PositionId = ParseInt (value);
				break;
			case NotificationType.Buys:
				PositionId = ParseInt (value);
				break;
			case NotificationType.Sells:
				PositionId = ParseInt (value);
				break;
			case NotificationType.TransferIn:
				PositionId = ParseInt (value);
				break;
			case NotificationType.PositionTransfers:
				PositionId = ParseInt (value);
				break;
			case NotificationType.RelationsChanges:
				AffiliationId = ParseInt (value);
				break;
			case NotificationType.SystemCharters:
				AffiliationId = ParseInt (value);
				break;
			case NotificationType.NewRestrictedStarbases:
				PositionId = ParseInt (value);
				break;
			case NotificationType.NewRestrictedCelestials:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.NewRestrictedSystemLink:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.NewRestrictedItems:
				ItemId = ParseInt (value);
				break;
			case NotificationType.NewRestrictedOrders:
				OrderId = ParseInt (value);
				break;
			case NotificationType.NewRestrictedMissions:
				MissionId = ParseInt (value);
				break;
			case NotificationType.NewRestrictedSystemLocations:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.DeliveredTo:
				PositionId = ParseInt (value);
				break;
			case NotificationType.PickedUpFrom:
				PositionId = ParseInt (value);
				break;
			case NotificationType.MarketSells:
				PositionId = ParseInt (value);
				break;
			case NotificationType.MarketBuys:
				PositionId = ParseInt (value);
				break;
			case NotificationType.Boarding:
				Location = value;
				break;
			case NotificationType.Raiding:
				Location = value;
				break;
			case NotificationType.CombatTransactions:
				Location = value;
				break;
			case NotificationType.SoldPosition:
				PositionId = ParseInt (value);
				break;
			case NotificationType.Restricted:
				break;
			case NotificationType.PlanetarySales:
				PositionId = ParseInt (value);
				break;
			case NotificationType.NexusRequestFacebook:
				PositionId = ParseInt (value);
				break;
			case NotificationType.GamesmasterNote:
				PositionId = ParseInt (value);
				break;
			case NotificationType.Rumour:
				Message = value;
				break;
			case NotificationType.Spotted:
				PositionId = ParseInt (value);
				break;
			case NotificationType.TurnError:
				PositionId = ParseInt (value);
				break;
			case NotificationType.Warning:
				PositionId = ParseInt (value);
				break;
			case NotificationType.ResearchFinished:
				PositionId = ParseInt (value);
				break;
			case NotificationType.ComplexVisited:
				PositionId = ParseInt (value);
				break;
			case NotificationType.TransferOut:
				PositionId = ParseInt (value);
				break;
			case NotificationType.OrbitalDrop:
				PositionId = ParseInt (value);
				break;
			case NotificationType.OrbitalResupply:
				PositionId = ParseInt (value);
				break;
			case NotificationType.SpecialAction:
				PositionId = ParseInt (value);
				break;
			case NotificationType.Message:
				PositionId = ParseInt (value);
				break;
			case NotificationType.AgentAction:
				PositionId = ParseInt (value);
				break;
			case NotificationType.Reminder:
				PositionId = ParseInt (value);
				break;
			case NotificationType.ComplexChange:
				PositionId = ParseInt (value);
				break;
			case NotificationType.AccountLow:
				break;
			case NotificationType.RegisteredBaseActivity:
				PositionId = ParseInt (value);
				break;
			case NotificationType.OpportunityFire:
				Location = value;
				break;
			case NotificationType.EscapingCombat:
				Location = value;
				break;
			}
		}

		private void SetData2(string value)
		{
			switch (Type) {
			case NotificationType.Turns:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.NewPosition:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.ActiveMission:
				PositionId = ParseInt (value);
				break;
			case NotificationType.Battles:
				NumberOfShips = ParseInt (value);
				break;
			case NotificationType.Deliveries:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.Pickups:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.Buys:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.Sells:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.TransferIn:
				Stellars = ParseInt (value);
				break;
			case NotificationType.PositionTransfers:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.RelationsChanges:
				Status = value;
				break;
			case NotificationType.SystemCharters:
				Status = value;
				break;
			case NotificationType.NewRestrictedCelestials:
				CelestialBodyId = ParseInt (value);
				break;
			case NotificationType.DeliveredTo:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.PickedUpFrom:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.MarketSells:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.MarketBuys:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.Boarding:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.Raiding:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.CombatTransactions:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.SoldPosition:
				Stellars = ParseInt (value);
				break;
			case NotificationType.Restricted:
				break;
			case NotificationType.PlanetarySales:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.NexusRequestFacebook:
				Stellars = ParseInt (value);
				break;
			case NotificationType.GamesmasterNote:
				GroupId = ParseInt (value);
				break;
			case NotificationType.Spotted:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.TurnError:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.Warning:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.ResearchFinished:
				ItemId = ParseInt (value);
				break;
			case NotificationType.ComplexVisited:
				BaseId = ParseInt (value);
				break;
			case NotificationType.TransferOut:
				Stellars = ParseInt (value);
				break;
			case NotificationType.OrbitalDrop:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.OrbitalResupply:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.SpecialAction:
				Message = value;
				break;
			case NotificationType.Message:
				Message = value;
				break;
			case NotificationType.AgentAction:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.Reminder:
				Message = value;
				break;
			case NotificationType.ComplexChange:
				ComplexType = value;
				break;
			case NotificationType.RegisteredBaseActivity:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.OpportunityFire:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.EscapingCombat:
				StarSystemId = ParseInt (value);
				break;
			}
		}

		private void SetData3(string value)
		{
			switch (Type) {
			case NotificationType.Turns:
				PositionType = ParsePositonType (value);
				break;
			case NotificationType.NewPosition:
				PositionType = ParsePositonType (value);
				break;
			case NotificationType.Battles:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.Deliveries:
				Quantity = ParseInt (value);
				break;
			case NotificationType.Pickups:
				Quantity = ParseInt (value);
				break;
			case NotificationType.Buys:
				Quantity = ParseInt (value);
				break;
			case NotificationType.Sells:
				Quantity = ParseInt (value);
				break;
			case NotificationType.PositionTransfers:
				PositionType = ParsePositonType (value);
				break;
			case NotificationType.SystemCharters:
				CharterId = ParseInt (value);
				break;
			case NotificationType.DeliveredTo:
				Quantity = ParseInt (value);
				break;
			case NotificationType.PickedUpFrom:
				Quantity = ParseInt (value);
				break;
			case NotificationType.MarketSells:
				Quantity = ParseInt (value);
				break;
			case NotificationType.MarketBuys:
				Quantity = ParseInt (value);
				break;
			case NotificationType.Boarding:
				PositionId = ParseInt (value);
				break;
			case NotificationType.Raiding:
				PositionId = ParseInt (value);
				break;
			case NotificationType.CombatTransactions:
				PositionId = ParseInt (value);
				break;
			case NotificationType.Restricted:
				break;
			case NotificationType.PlanetarySales:
				CelestialBodyId = ParseInt (value);
				break;
			case NotificationType.GamesmasterNote:
				StarSystemId = ParseInt (value);
				break;
			case NotificationType.Spotted:
				Location = value;
				break;
			case NotificationType.TurnError:
				CelestialBodyId = ParseInt (value);
				break;
			case NotificationType.Warning:
				CelestialBodyId = ParseInt (value);
				break;
			case NotificationType.ComplexVisited:
				ComplexType = value;
				ComplexTypeName = ((ComplexVisitType)ParseInt (value)).ToString ();
				break;
			case NotificationType.OrbitalDrop:
				Quantity = ParseInt (value);
				break;
			case NotificationType.OrbitalResupply:
				Quantity = ParseInt (value);
				break;
			case NotificationType.AgentAction:
				Message = value;
				break;
			case NotificationType.ComplexChange:
				ChangeType = (ComplexChangeType)ParseInt(value);
				break;
			case NotificationType.RegisteredBaseActivity:
				CelestialBodyId = ParseInt (value);
				break;
			case NotificationType.OpportunityFire:
				Message = value;
				break;
			case NotificationType.EscapingCombat:
				Message = value;
				break;
			}
		}

		private void SetData4(string value)
		{
			switch (Type) {
			case NotificationType.Turns:
				SquadronId = ParseInt (value);
				break;
			case NotificationType.NewPosition:
				SquadronId = ParseInt (value);
				break;
			case NotificationType.Deliveries:
				ItemId = ParseInt (value);
				break;
			case NotificationType.Pickups:
				ItemId = ParseInt (value);
				break;
			case NotificationType.Buys:
				ItemId = ParseInt (value);
				break;
			case NotificationType.Sells:
				ItemId = ParseInt (value);
				break;
			case NotificationType.PositionTransfers:
				TransferId = ParseInt (value);
				break;
			case NotificationType.DeliveredTo:
				ItemId = ParseInt (value);
				break;
			case NotificationType.PickedUpFrom:
				ItemId = ParseInt (value);
				break;
			case NotificationType.MarketSells:
				ItemId = ParseInt (value);
				break;
			case NotificationType.MarketBuys:
				ItemId = ParseInt (value);
				break;
			case NotificationType.PlanetarySales:
				TradeType = ParseTradeType (value);
				break;
			case NotificationType.GamesmasterNote:
				CelestialBodyId = ParseInt (value);
				break;
			case NotificationType.TurnError:
				ErrorCode = (ErrorType)ParseInt(value);
				break;
			case NotificationType.Warning:
				WarningCode = (WarningType)ParseInt(value);
				break;
			case NotificationType.OrbitalDrop:
				ItemId = ParseInt (value);
				break;
			case NotificationType.OrbitalResupply:
				ItemId = ParseInt (value);
				break;
			case NotificationType.ComplexChange:
				Quantity = ParseInt(value);
				break;
			case NotificationType.RegisteredBaseActivity:
				BaseActivityType = (RegisteredBaseActivityType)ParseInt (value);
				break;
			}
		}

		private void SetData5(string value)
		{
			switch (Type) {
			case NotificationType.Deliveries:
				DeliverToId = ParseInt (value);
				break;
			case NotificationType.Pickups:
				PickedUpFromId = ParseInt (value);
				break;
			case NotificationType.Buys:
				BoughtFromId = ParseInt (value);
				break;
			case NotificationType.Sells:
				SoldToId = ParseInt (value);
				break;
			case NotificationType.DeliveredTo:
				ByPositionId = ParseInt (value);
				break;
			case NotificationType.PickedUpFrom:
				ByPositionId = ParseInt (value);
				break;
			case NotificationType.MarketSells:
				ByPositionId = ParseInt (value);
				break;
			case NotificationType.MarketBuys:
				ByPositionId = ParseInt (value);
				break;
			case NotificationType.PlanetarySales:
				Stellars = ParseInt (value);
				break;
			case NotificationType.GamesmasterNote:
				Message = value;
				break;
			case NotificationType.TurnError:
				ErrorMessage = value;
				break;
			case NotificationType.Warning:
				WarningMessage = value;
				break;
			case NotificationType.OrbitalDrop:
				ByPositionId = ParseInt (value);
				break;
			case NotificationType.OrbitalResupply:
				ByPositionId = ParseInt (value);
				break;
			}
		}

		private void SetData6(string value)
		{
			switch (Type) {
			case NotificationType.Buys:
				Stellars = ParseInt (value);
				break;
			case NotificationType.Sells:
				Stellars = ParseInt (value);
				break;
			case NotificationType.MarketSells:
				Stellars = ParseInt (value);
				break;
			case NotificationType.MarketBuys:
				Stellars = ParseInt (value);
				break;
			}
		}

		private int ParseInt(string value)
		{
			try{
				return Int32.Parse(value);
			}
			catch{
				return 0;
			}
		}

		private NotificationPositionType ParsePositonType(string value)
		{
			try {
				return (NotificationPositionType) Enum.Parse(typeof(NotificationPositionType),value);
			}
			catch{
				return NotificationPositionType.None;
			}
		}

		private PlanetaryTradeCategory ParseTradeType(string value)
		{
			try {
				return (PlanetaryTradeCategory) Enum.Parse(typeof(PlanetaryTradeCategory),value);
			}
			catch{
				return PlanetaryTradeCategory.TradeGoods;
			}
		}
	}
}

