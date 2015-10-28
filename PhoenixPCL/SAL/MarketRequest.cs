//
// MarketRequest.cs
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
	public class MarketRequest : NexusRequest<MarketBase>
	{
		public MarketRequest (int UID, string Code) : base(UID,Code,"markets", 0)
		{
		}

		protected override void Success (XmlReader xmlReader, Action<IEnumerable<MarketBase>, Exception> callback)
		{
			Log.WriteLine (Log.Layer.SAL, this.GetType (), "Success");

			List<MarketBase> list = new List<MarketBase> ();

			MarketBase mb = null;
			MarketItem mi = null;

			bool inItem = false;

			while (xmlReader.Read ()) {
				if (xmlReader.IsStartElement ()) {
					try {
						switch(xmlReader.Name){
						case "starbase":
							mb = new MarketBase{
								Id = Int32.Parse(xmlReader.GetAttribute("id"))
							};
							list.Add(mb);
							inItem = false;
							break;
						case "aff":
							if(mb != null){
								if(xmlReader.Read())
									mb.AffiliationCode = xmlReader.Value.Trim ();
							}
							break;
						case "name":
							if(inItem && mi != null){
								if(xmlReader.Read())
									mi.ItemName = xmlReader.Value.Trim ();
							}
							else if(mb != null){
								if(xmlReader.Read())
									mb.Name = xmlReader.Value.Trim ();
							}
							break;
						case "system":
							if(mb != null){
								mb.StarSystemId = Int32.Parse(xmlReader.GetAttribute("id"));
								if(xmlReader.Read())
									mb.StarSystemName = xmlReader.Value.Trim ();
							}
							break;
						case "cbody":
							if(mb != null){
								mb.CelestialBodyId =  Int32.Parse(xmlReader.GetAttribute("id"));
								if(xmlReader.Read())
									mb.CelestialBodyName = xmlReader.Value.Trim ();
							}
							break;
						case "hiport":
							if(mb != null){
								mb.Hiport = Int32.Parse(xmlReader.GetAttribute("quant"));
							}
							break;
						case "patches":
							if(mb != null){
								mb.PatchPrice = Int32.Parse(xmlReader.GetAttribute("price"));
							}
							break;
						case "docks":
							if(mb != null){
								mb.DockCapacity = Int32.Parse(xmlReader.GetAttribute("quant"));
							}
							break;
						case "maintenance":
							if(mb != null){
								mb.MaintenanceComplexes = Int32.Parse(xmlReader.GetAttribute("quant"));
							}
							break;
						case "item":
							mi = new MarketItem{
								ItemId = Int32.Parse(xmlReader.GetAttribute("id"))
							};
							inItem = true;
							if(mb != null){
								mb.Items.Add(mi);
							}
							break;
						case "sell":
							if(mi != null){
								mi.SellPrice = float.Parse(xmlReader.GetAttribute("price"));
								mi.SellQuantity = Int32.Parse(xmlReader.GetAttribute("quant"));
							}
							break;
						case "buy":
							if(mi != null){
								mi.BuyPrice = float.Parse(xmlReader.GetAttribute("price"));
								mi.BuyQuantity = Int32.Parse(xmlReader.GetAttribute("quant"));
							}
							break;
						}
					} catch (Exception e) {
						Log.WriteLine (Log.Layer.SAL, this.GetType (), "Error Parsing Market " + mb + ": " + e);
					}
				}
			}

			callback (list, null);
		}

		protected override string GetRequestAction()
		{
			return "game&type=xml";
		}
	}
}

