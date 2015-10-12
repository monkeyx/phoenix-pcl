//
// GameStatusRequest.cs
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

using Phoenix.BL.Entities;

namespace Phoenix.SAL
{
    /// <summary>
    /// Game status request.
    /// </summary>
    public class GameStatusRequest : NexusRequest<GameStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.SAL.GameStatusRequest"/> class.
        /// </summary>
        /// <param name="UID">User Id</param>
        /// <param name="Code">Code.</param>
        public GameStatusRequest (int UID, string Code) : base(UID,Code,"game_status", 0)
        {
        }

        /// <summary>
        /// Successfully fetched data and should be processed by subclass
        /// </summary>
        /// <param name="xmlReader">Xml reader.</param>
        /// <param name="callback">Callback.</param>
        protected override void Success(XmlReader xmlReader, Action<List<GameStatus>> callback)
        {
            List<GameStatus> list = new List<GameStatus> ();
            GameStatus status = new GameStatus ();
            list.Add (status);

            while (xmlReader.Read ()) {
                if (xmlReader.Name == "status") {
                    status.StatusMessage = xmlReader.ReadContentAsString ();
                }
                else if (xmlReader.Name == "current_day") {
                    status.CurrentDay = Int32.Parse(xmlReader.ReadContentAsString ());
                }
                else if (xmlReader.Name == "year_start") {
                    status.YearStart = Int32.Parse(xmlReader.ReadContentAsString ());
                }
                else if (xmlReader.Name == "turns_downloaded") {
                    status.TurnsDownloaded = Int32.Parse(xmlReader.ReadContentAsString ());
                }
                else if (xmlReader.Name == "turns_processed") {
                    status.TurnsProcessed = Int32.Parse(xmlReader.ReadContentAsString ());
                }
                else if (xmlReader.Name == "turns_uploaded") {
                    status.TurnsUploaded = Int32.Parse(xmlReader.ReadContentAsString ());
                }
                else if (xmlReader.Name == "emails_sent") {
                    status.EmailsSent = Int32.Parse(xmlReader.ReadContentAsString ());
                }
                else if (xmlReader.Name == "specials_processed") {
                    status.SpecialsProcessed = Int32.Parse(xmlReader.ReadContentAsString ());
                }
                else if (xmlReader.Name == "day_finished") {
                    status.DayFinished = Int32.Parse(xmlReader.ReadContentAsString ());
                }
                else if (xmlReader.Name == "star_date") {
                    status.StarDate = xmlReader.ReadContentAsString ();
                }
            }

            callback (list);
        }
    }
}

