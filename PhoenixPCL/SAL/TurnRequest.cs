//
// TurnRequest.cs
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
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

using Phoenix.BL.Entities;
using Phoenix.Util;

namespace Phoenix.SAL
{
	/// <summary>
	/// Turn request.
	/// </summary>
	public class TurnRequest : NexusRequest<PositionTurn>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.SAL.TurnRequest"/> class.
		/// </summary>
		/// <param name="UID">User interface.</param>
		/// <param name="Code">Code.</param>
		/// <param name="positionId">Position identifier.</param>
		public TurnRequest (int UID, string Code, int positionId) : base(UID,Code,"turn_data", positionId)
		{
		}

		/// <summary>
		/// Reads the stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		protected override void ReadStream (Stream stream)
		{
			if (stream == null) {
				resultCallback (new List<PositionTurn> (){},new Exception("No turn received"));
				return;
			}
			string fileName = PositionId.ToString () + ".html";
			StreamReader reader = new StreamReader (stream);
			string content = reader.ReadToEnd ().Replace("url(","url(" + Phoenix.Application.BASE_URL);

			List<PositionTurn> list = new List<PositionTurn> () {
				new PositionTurn{
					Id = PositionId,
					Content = content
				}
			};

			Application.DocumentFolder.WriteFile (fileName, content);

			resultCallback (list,null);
		}
	}
}

