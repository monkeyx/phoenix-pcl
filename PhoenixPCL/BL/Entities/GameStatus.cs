//
// GameStatus.cs
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
using SQLite.Net.Attributes;

namespace Phoenix.BL.Entities
{
    /// <summary>
    /// Game status.
    /// </summary>
    [Table("game_status")]
    public class GameStatus : EntityBase
    {
        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        /// <value>The status message.</value>
        public string StatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the current day.
        /// </summary>
        /// <value>The current day.</value>
        public int CurrentDay { get; set; }

        /// <summary>
        /// Gets or sets the current year.
        /// </summary>
        /// <value>The current year.</value>
        public int CurrentYear { get; set; }

        /// <summary>
        /// Gets or sets the year start.
        /// </summary>
        /// <value>The year start.</value>
        public int YearStart { get; set; }

        /// <summary>
        /// Gets or sets the turns downloaded.
        /// </summary>
        /// <value>The turns downloaded.</value>
        public int TurnsDownloaded { get; set; }

        /// <summary>
        /// Gets or sets the turns processed.
        /// </summary>
        /// <value>The turns processed.</value>
        public int TurnsProcessed { get; set; }

        /// <summary>
        /// Gets or sets the turns uploaded.
        /// </summary>
        /// <value>The turns uploaded.</value>
        public int TurnsUploaded { get; set; }

        /// <summary>
        /// Gets or sets the emails sent.
        /// </summary>
        /// <value>The emails sent.</value>
        public int EmailsSent { get; set; }

        /// <summary>
        /// Gets or sets the specials processed.
        /// </summary>
        /// <value>The specials processed.</value>
        public int SpecialsProcessed { get; set; }

        /// <summary>
        /// Gets or sets the day finished.
        /// </summary>
        /// <value>The day finished.</value>
        public int DayFinished { get; set; }

        /// <summary>
        /// Gets or sets the star date.
        /// </summary>
        /// <value>The star date.</value>
        public string StarDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.GameStatus"/> class.
        /// </summary>
        public GameStatus ()
        {
        }
    }
}

