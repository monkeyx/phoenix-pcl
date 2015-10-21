//
// SubmitOrdersRequest.cs
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
using System.Text;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

using Phoenix.BL.Entities;
using Phoenix.Util;

namespace Phoenix.SAL
{
	/// <summary>
	/// Submit orders request.
	/// </summary>
	public class SubmitOrdersRequest : NexusRequest<Order>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.SAL.SubmitOrdersRequest"/> class.
		/// </summary>
		/// <param name="UID">User interface.</param>
		/// <param name="Code">Code.</param>
		/// <param name="positionId">Position identifier.</param>
		public SubmitOrdersRequest (int UID, string Code, int positionId) : base(UID,Code,"send_orders", positionId)
		{
		}

		/// <summary>
		/// Converts DTO to XML
		/// </summary>
		/// <returns>The xml.</returns>
		/// <param name="dto">DTO.</param>
		protected override string ToXml(object dto)
		{
			List<Order> orders = (List<Order>)dto;
			StringBuilder sb = new StringBuilder ();
			sb.Append ("<turns>");
			sb.Append ("<turn pos_id=\"" + PositionId + "\" seq=\"\" seq_after=\"\" append=\"true\">");
			sb.Append ("<orders>");
			foreach (Order o in orders) {
				sb.Append ("<order id=\"" + o.OrderTypeId + "\">");
				foreach (OrderParameter param in o.Parameters) {
					sb.Append ("<param>");
					sb.Append (param.Value);
					sb.Append ("</param>");
				}
				sb.Append ("</order>");
			}
			sb.Append ("</orders>");
			sb.Append ("</turn>");
			sb.Append ("</turns>");
			string xml = sb.ToString ();
			Log.WriteLine (Log.Layer.SAL, GetType (), "Submit Orders for Position: " + PositionId);
			Log.WriteLine (Log.Layer.SAL, GetType (), xml);
			return xml;
		}
	}
}

