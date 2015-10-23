//
// NotesRequest.cs
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
using System.Xml;
using System.Linq;

using Phoenix.BL.Entities;
using Phoenix.Util;

namespace Phoenix.SAL
{
	public class NotesRequest : NexusRequest<Notification>
	{
		public NotesRequest (int UID, string Code) : base(UID,Code,"notes", 0)
		{
		}

		protected override void Success(XmlReader xmlReader, Action<IEnumerable<Notification>, Exception> callback)
		{
			Log.WriteLine (Log.Layer.SAL, this.GetType (), "Success");

			List<Notification> list = new List<Notification> ();

			Notification note = null;

			bool inNotes = false;

			int dataIndex = 0;

			int systemId = 0;

			int currentDay = 0;
				
			while (xmlReader.Read ()) {
				if (xmlReader.IsStartElement ()) {
					if (xmlReader.Name == "current_day") {
						try {
							currentDay = Int32.Parse(xmlReader.GetAttribute("value"));
						}
						catch(Exception e){
							Log.WriteLine (Log.Layer.SAL, this.GetType (), "Unable to parse current day. " + e);
						}
					}
					else if (xmlReader.Name == "note_list") {
						inNotes = true;
					} else if (xmlReader.Name == "status_list") {
						inNotes = false;
					} else if (xmlReader.Name == "note") {
						note = new Notification ();
						list.Add (note);
						dataIndex = 1;
					} else if (inNotes && xmlReader.Name == "type" && note != null) {
						try {
							note.Type = (Notification.NotificationType)Int32.Parse (xmlReader.GetAttribute ("value"));
						} catch (Exception e) {
							Log.WriteLine (Log.Layer.SAL, this.GetType (), "Unknown type. " + e);
						}
					} else if (inNotes && xmlReader.Name == "priority" && note != null) {
						try {
							note.Priority = (Notification.NotificationPriority)Int32.Parse (xmlReader.GetAttribute ("value"));
						} catch (Exception e) {
							Log.WriteLine (Log.Layer.SAL, this.GetType (), "Unknown priority. " + e);
						}
					} else if (inNotes && xmlReader.Name == "day" && note != null) {
						try {
							note.Day = Int32.Parse (xmlReader.GetAttribute ("value"));
						} catch (Exception e) {
							Log.WriteLine (Log.Layer.SAL, this.GetType (), "Unknown day. " + e);
						}
					} else if (inNotes && xmlReader.Name == "data" && note != null && xmlReader.HasAttributes) {
						try {
							note.SetData (dataIndex, xmlReader.GetAttribute ("value"));
						} catch (Exception e) {
							Log.WriteLine (Log.Layer.SAL, this.GetType (), "Problem setting data. " + e);
						}
						dataIndex += 1;
					} else if (inNotes && xmlReader.Name == "id" && note != null) {
						try {
							note.Id = Int32.Parse (xmlReader.GetAttribute ("value"));
						} catch (Exception e) {
							Log.WriteLine (Log.Layer.SAL, this.GetType (), "Unknown identifier. " + e);
						}
					} else if (!inNotes && xmlReader.Name == "pos" && note != null && xmlReader.HasAttributes) {
						try {
							int id = Int32.Parse (xmlReader.GetAttribute ("key"));
							string name = xmlReader.GetAttribute("value");
							UpdatePositionNames(id,name,list);
						} catch (Exception e) {
							Log.WriteLine (Log.Layer.SAL, this.GetType (), "Unknown position. " + e);
						}
					} else if (!inNotes && xmlReader.Name == "sys" && note != null && xmlReader.HasAttributes) {
						try {
							int id = Int32.Parse (xmlReader.GetAttribute ("key"));
							string name = xmlReader.GetAttribute("value");
							UpdateSystemNames(id,name,list);
						} catch (Exception e) {
							Log.WriteLine (Log.Layer.SAL, this.GetType (), "Unknown system. " + e);
						}
					} else if (!inNotes && xmlReader.Name == "squad" && note != null && xmlReader.HasAttributes) {
						try {
							int id = Int32.Parse (xmlReader.GetAttribute ("key"));
							string name = xmlReader.GetAttribute("value");
							UpdateSquadNames(id,name,list);
						} catch (Exception e) {
							Log.WriteLine (Log.Layer.SAL, this.GetType (), "Unknown squadron. " + e);
						}
					} else if (!inNotes && xmlReader.Name == "item" && note != null && xmlReader.HasAttributes) {
						try {
							int id = Int32.Parse (xmlReader.GetAttribute ("key"));
							string name = xmlReader.GetAttribute("value");
							UpdateItemNames(id,name,list);
						} catch (Exception e) {
							Log.WriteLine (Log.Layer.SAL, this.GetType (), "Unknown item. " + e);
						}
					} else if (!inNotes && xmlReader.Name == "cbody" && note != null && xmlReader.HasAttributes) {
						try {
							int id = Int32.Parse (xmlReader.GetAttribute ("key"));
							if(xmlReader.AttributeCount > 1){
								string name = xmlReader.GetAttribute("value");
								UpdateCBodyNames(systemId, id,name,list);
							}
							else {
								systemId = id;
							}
						} catch (Exception e) {
							Log.WriteLine (Log.Layer.SAL, this.GetType (), "Unknown cbody. " + e);
						}
					} else if (!inNotes && xmlReader.Name == "aff" && note != null && xmlReader.HasAttributes) {
						try {
							int id = Int32.Parse (xmlReader.GetAttribute ("key"));
							string name = xmlReader.GetAttribute("value");
							UpdateAffiliationNames(id,name,list);
						} catch (Exception e) {
							Log.WriteLine (Log.Layer.SAL, this.GetType (), "Unknown affiliation. " + e);
						}
					}
				}

			}

			foreach (Notification n in list) {
				n.DaysAgo = currentDay - n.Day;
			}

			callback (from element in list
				orderby element.DaysAgo
				select element, null);
		}

		private void UpdatePositionNames(int positionId, string name, List<Notification> list)
		{
			foreach (Notification n in list) {
				if (n.PositionId == positionId) {
					n.PositionName = name;
				}
				if (n.BaseId == positionId) {
					n.BaseName = name;
				}
				if (n.DeliverToId == positionId) {
					n.DeliverToName = name;
				}
				if (n.PickedUpFromId == positionId) {
					n.PickedUpFromName = name;
				}
				if (n.BoughtFromId == positionId) {
					n.BoughtFromName = name;
				}
				if (n.SoldToId == positionId) {
					n.SoldToName = name;
				}
				if (n.ByPositionId == positionId) {
					n.ByPositionName = name;
				}
			}
		}

		private void UpdateSystemNames(int systemId, string name, List<Notification> list)
		{
			foreach (Notification n in list) {
				if (n.SystemId == systemId) {
					n.SystemName = name;
				}
			}
		}

		private void UpdateSquadNames(int squadId, string name, List<Notification> list)
		{
			foreach (Notification n in list) {
				if (n.SquadronId == squadId) {
					n.SquadronName = name;
				}
			}
		}

		private void UpdateItemNames(int itemId, string name, List<Notification> list)
		{
			foreach (Notification n in list) {
				if (n.ItemId == itemId) {
					n.ItemName = name;
				}
			}
		}

		private void UpdateCBodyNames(int systemId, int cbodyId, string name, List<Notification> list)
		{
			foreach (Notification n in list) {
				if (n.SystemId == systemId && n.CelestialBodyId == cbodyId) {
					n.CelestialBodyName = name;
				}
			}
		}

		private void UpdateAffiliationNames(int affId, string name, List<Notification> list)
		{
			foreach (Notification n in list) {
				if (n.AffiliationId == affId) {
					n.AffiliationName = name;
				}
			}
		}
	}
}

