﻿//
// Application.cs
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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using SQLite.Net;

using Phoenix.BL.Entities;
using Phoenix.BL.Managers;
using Phoenix.DL;
using Phoenix.SAL;
using Phoenix.Util;

namespace Phoenix
{
	/// <summary>
	/// Database interface
	/// </summary>
	public interface IDatabase
	{
		/// <summary>
		/// Gets the connection.
		/// </summary>
		/// <returns>The connection.</returns>
		SQLiteConnection GetConnection();
	}

	/// <summary>
	/// Interface for platform specific logging
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Writes the line.
		/// </summary>
		/// <param name="format">Format.</param>
		/// <param name="arg">Argument.</param>
		void WriteLine(string format, params object[] arg);
	}

	/// <summary>
	/// Interface for document folder
	/// </summary>
	public interface IDocumentFolder
	{
		/// <summary>
		/// Gets the document path.
		/// </summary>
		/// <returns>The document path.</returns>
		string GetDocumentPath();

		/// <summary>
		/// Writes the file.
		/// </summary>
		/// <param name="filename">Filename.</param>
		/// <param name="content">Content.</param>
		void WriteFile(string filename, string content);

		/// <summary>
		/// Reads the file.
		/// </summary>
		/// <returns>The file.</returns>
		/// <param name="filename">Filename.</param>
		string ReadFile(string filename);
	}

	/// <summary>
	/// Rest client interface
	/// </summary>
	public interface IRestClient
	{
		/// <summary>
		/// Async GET method
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="callback">Callback.</param>
		void GetAsync(string url, Action<Stream> callback);

		/// <summary>
		/// Async POST method
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="dto">Dto.</param>
		/// <param name="callback">Callback.</param>
		void PostAsync(string url, object dto, Action<HttpResponseMessage> callback);
	}

	/// <summary>
	/// Phoenix Application.
	/// </summary>
	public static class Application
	{
		/// <summary>
		/// Base URL for Nexus XML
		/// </summary>
		public const string BASE_URL = "http://www.phoenixbse.com/";

		/// <summary>
		/// Gets or sets the user.
		/// </summary>
		/// <value>The user.</value>
		public static User User { get; set; }

		/// <summary>
		/// Gets or sets the user manager.
		/// </summary>
		/// <value>The user manager.</value>
		public static UserManager UserManager { get; private set; }

		/// <summary>
		/// Gets the game status manager.
		/// </summary>
		/// <value>The game status manager.</value>
		public static GameStatusManager GameStatusManager { get; private set; }

		/// <summary>
		/// Gets the info manager.
		/// </summary>
		/// <value>The info manager.</value>
		public static InfoManager InfoManager { get; private set; }

		/// <summary>
		/// Gets the item manager.
		/// </summary>
		/// <value>The item manager.</value>
		public static ItemManager ItemManager { get; private set; }

		/// <summary>
		/// Gets the order type manager.
		/// </summary>
		/// <value>The order type manager.</value>
		public static OrderTypeManager OrderTypeManager { get; private set; }

		/// <summary>
		/// Gets the position manager.
		/// </summary>
		/// <value>The position manager.</value>
		public static PositionManager PositionManager { get; private set; }

		/// <summary>
		/// Gets the star system manager.
		/// </summary>
		/// <value>The star system manager.</value>
		public static StarSystemManager StarSystemManager { get; private set; }

		/// <summary>
		/// Gets the order manager.
		/// </summary>
		/// <value>The order manager.</value>
		public static OrderManager OrderManager { get; private set; }

		/// <summary>
		/// Gets the notification manager.
		/// </summary>
		/// <value>The notification manager.</value>
		public static NotificationManager NotificationManager { get; private set; }

		/// <summary>
		/// Gets the market manager.
		/// </summary>
		/// <value>The market manager.</value>
		public static MarketManager MarketManager { get; private set; }

		/// <summary>
		/// Gets or sets the document folder.
		/// </summary>
		/// <value>The document folder.</value>
		public static IDocumentFolder DocumentFolder { get; set; }

		/// <summary>
		/// Gets or sets the rest client.
		/// </summary>
		/// <value>The rest client.</value>
		public static IRestClient RestClient { get; set; }

		/// <summary>
		/// Initialize the Phoenix system
		/// </summary>
		/// <param name="dabaseProvider">Dabase provider.</param>
		/// <param name="logger">Logger.</param>
		/// <param name="documentFolder">Document folder.</param>
		public static void Initialize(IDatabase dabaseProvider, ILogger logger, IDocumentFolder documentFolder, IRestClient restClient)
		{
			Log.Logger = logger;
			PhoenixDatabase.DatabaseProvider = dabaseProvider;

			#if CLEAR_DATABASE
			PhoenixDatabase.ClearDatabase();
			#else
			PhoenixDatabase.CreateTables ();
			#endif
			UserManager = new UserManager ();

			DocumentFolder = documentFolder;
			RestClient = restClient;
		}

		/// <summary>
		/// Sets up managers when a user is logged in
		/// </summary>
		/// <param name="user">User.</param>
		public static void UserLoggedIn(User user)
		{
			Log.WriteLine (Log.Layer.AL, typeof(Application), "User logged in: " + user);
			User = user;
			GameStatusManager = new GameStatusManager (user);
			InfoManager = new InfoManager (user);
			ItemManager = new ItemManager (user);
			OrderTypeManager = new OrderTypeManager (user);
			PositionManager = new PositionManager (user);
			StarSystemManager = new StarSystemManager (user);
			OrderManager = new OrderManager (user);
			NotificationManager = new NotificationManager (user);
			MarketManager = new MarketManager (user);
		}
	}
}

