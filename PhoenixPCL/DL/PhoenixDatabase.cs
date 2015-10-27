//
// PhoenixDatabase.cs
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
using System.Linq;
using SQLite.Net;

using Phoenix;
using Phoenix.BL.Entities;
using Phoenix.Util;

namespace Phoenix.DL
{
	/// <summary>
    /// Phoenix database.
    /// </summary>
    public static class PhoenixDatabase
    {
        /// <summary>
        /// Gets or sets the database provider.
        /// </summary>
        /// <value>The database provider.</value>
        public static IDatabase DatabaseProvider { get; set; }

        /// <summary>
        /// Creates the tables.
        /// </summary>
        public static void CreateTables()
        {
			CreateTable<GameStatus> ();
			CreateTable<InfoData> ();
			CreateTable<Item> ();
			CreateTable<RawMaterial> ();
			CreateTable<ItemProperty> ();
			CreateTable<OrderType> ();
			CreateTable<OrderParameterType> ();
			CreateTable<Order> ();
			CreateTable<OrderParameter> ();
			CreateTable<Position> ();
			CreateTable<PositionTurn> ();
			CreateTable<StarSystem> ();
			CreateTable<CelestialBody> ();
			CreateTable<JumpLink> ();
			CreateTable<User> ();
			CreateTable<Notification> ();
			CreateTable<PositionNote> ();
        }

        /// <summary>
        /// Clears the database.
        /// </summary>
        public static void ClearDatabase()
        {
            DropTable<GameStatus> ();
            DropTable<InfoData> ();
            DropTable<Item> ();
            DropTable<RawMaterial> ();
            DropTable<ItemProperty> ();
            DropTable<OrderType> ();
            DropTable<OrderParameterType> ();
			DropTable<Order> ();
			DropTable<OrderParameter> ();
            DropTable<Position> ();
			DropTable<PositionTurn> ();
            DropTable<StarSystem> ();
            DropTable<CelestialBody> ();
            DropTable<JumpLink> ();
			DropTable<User> ();
			DropTable<Notification> ();
			DropTable<PositionNote> ();
            CreateTables();
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <returns>The items.</returns>
        /// <param name="orderBy">Order by.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> GetItems<T>(string orderBy = "Id ASC") where T : EntityBase, new()
        {
            try {
                // querying directly as db.Table<T>.Where... throws errors when table is empty
				string sql = string.Format("select * from \"{0}\" order by {1}",TableName(typeof(T)), orderBy);
                lock(_locker){
                   return DatabaseProvider.GetConnection().Query<T>(sql);
                }
            } catch(Exception e) {
				Log.WriteLine(Log.Layer.DL, typeof(PhoenixDatabase), ErrorMessage("GetItems", typeof(T), e));
                return new List<T>();
            }
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetItem<T>(int id) where T : EntityBase, new()
        {
            try {
                // querying directly as db.Table<T>.Where... throws errors when table is empty
                string sql = string.Format("select * from \"{0}\" where Id = ?",TableName(typeof(T)));
                lock(_locker){
                    List<T> list = DatabaseProvider.GetConnection().Query<T>(sql,id);
                    if(list != null && list.Count > 0){
                        T obj = list[0];
                        return obj;
                    }
                    return default(T);
                }
            } catch(Exception e) {
				Log.WriteLine(Log.Layer.DL, typeof(PhoenixDatabase), ErrorMessage("GetItem", typeof(T), e, id));
                return default(T);
            }
        }

        /// <summary>
        /// Gets the first item.
        /// </summary>
        /// <returns>The first item.</returns>
        /// <param name="orderBy">Order by - defaults to id ascending.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetFirstItem<T>(string orderBy = "Id ASC") where T : EntityBase, new()
        {
            try {
                // querying directly as db.Table<T>.Where... throws errors when table is empty
                string sql = string.Format("select * from \"{0}\" order by {1} limit 1",TableName(typeof(T)), orderBy);
                lock(_locker){
                    List<T> list = DatabaseProvider.GetConnection().Query<T>(sql);
                    if(list != null && list.Count > 0){
                        T obj = list[0];
                        return obj;
                    }
                    return default(T);
                }
            } catch(Exception e) {
				Log.WriteLine(Log.Layer.DL, typeof(PhoenixDatabase), ErrorMessage("GetFirstItem", typeof(T), e));
                return default(T);
            }
        }

        /// <summary>
        /// Determines the item specified with the specified id exists
        /// </summary>
        /// <returns><c>true</c> if the item with specified id exists; otherwise, <c>false</c>.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool HasItem<T>(int id) where T : IEntity, new()
        {
            try {
                // querying directly as db.Table<T>.Where... throws errors when table is empty
                string sql = string.Format("select count(*) from \"{0}\" where Id = ?",TableName(typeof(T)));
                lock(_locker){
                    if(ExecuteScalar<int>(sql, id) > 0){
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            } catch(Exception e) {
				Log.WriteLine(Log.Layer.DL, typeof(PhoenixDatabase), ErrorMessage("HasItem", typeof(T), e, id));
                return false;
            }
        }

        /// <summary>
        /// Saves the item.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="insertOnly">If set to <c>true</c> insert only.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void SaveItem<T>(T item, bool insertOnly = false) where T : IEntity, new()
        {
            if (item == null)
                return;

            try {
                item.UpdatedAt = DateTime.Now;
				if(item.Id != 0 && HasItem<T>(item.Id)){
                    if (!insertOnly) {
                        lock (_locker) {
                            DatabaseProvider.GetConnection().Update(item);
                        }
                    }
                } else {
                    item.CreatedAt = DateTime.Now;
                    lock (_locker) {
                        DatabaseProvider.GetConnection().Insert(item);
                    }
                }
            } catch (Exception e) {
				Log.WriteLine(Log.Layer.DL, typeof(PhoenixDatabase), ErrorMessage("SaveItem", item.GetType(), e, (item == null ? -1 : item.Id)));
            }
        }

        /// <summary>
        /// Saves the item if new.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void SaveItemIfNew<T>(T item) where T : IEntity, new()
        {
            SaveItem<T>(item, true);
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="item">Item.</param>
        public static void DeleteItem(IEntity item)
        {
            if (item == null)
                return;
            try {
                lock (_locker) {
                    DatabaseProvider.GetConnection().Delete(item);
                }
            } catch(Exception e) {
				Log.WriteLine(Log.Layer.DL, typeof(PhoenixDatabase), ErrorMessage("DeleteItem", item.GetType(), e, (item == null ?  -1 : item.Id)));
            }
        }

        /// <summary>
        /// Deletes the item by identifier.
        /// </summary>
        /// <returns>The item by identifier.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static int DeleteItemById<T>(int id) where T : IEntity, new()
        {
            string sql = string.Format("delete from \"{0}\" where ID = \"{1}\"", TableName(typeof(T)), id);

            try {
                lock (_locker) {
                    return DatabaseProvider.GetConnection().ExecuteScalar<int>(sql);
                }
            } catch(Exception e) {
				Log.WriteLine(Log.Layer.DL, typeof(PhoenixDatabase), ErrorMessage("DeleteItemById", typeof(T), e, id));
                return -1;
            }

        }

        /// <summary>
        /// Clears the table.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void ClearTable<T>() where T : new()
        {
            try {
                Execute(string.Format ("delete from \"{0}\"", TableName(typeof(T))));
            } catch(Exception e) {
				Log.WriteLine(Log.Layer.DL, typeof(PhoenixDatabase), ErrorMessage("ClearTable", typeof(T), e));
            }
        }

        /// <summary>
        /// Counts the table.
        /// </summary>
        /// <returns>The table.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static int CountTable<T>() where T : IEntity, new()
        {
            try {
                string sql = string.Format ("select count (*) from \"{0}\"", TableName(typeof(T)));
                return ExecuteScalar<int>(sql);
            } catch(Exception e) {
				Log.WriteLine(Log.Layer.DL, typeof(PhoenixDatabase), ErrorMessage("CountTable", typeof(T), e));
                return -1;
            }
        }

		/// <summary>
		/// Gets the info data by group identifier and nexus identifier.
		/// </summary>
		/// <returns>The info data by group identifier and nexus identifier.</returns>
		/// <param name="groupId">Group identifier.</param>
		/// <param name="nexusId">Nexus identifier.</param>
		public static InfoData GetInfoDataByGroupIdAndNexusId(int groupId, int nexusId)
		{
			List<InfoData> list = Query<InfoData> ("select i.* from InfoData i where GroupId = ? and NexusId = ?", groupId, nexusId);
			if (list.Count > 0) {
				return list [0];
			}
			return null;
		}

		/// <summary>
		/// Gets the info data by group identifier.
		/// </summary>
		/// <returns>The info data by group identifier.</returns>
		/// <param name="groupId">Group identifier.</param>
		public static List<InfoData> GetInfoDataByGroupId(int groupId)
		{
			return Query<InfoData> ("select i.* from InfoData i where GroupId = ? order by Name asc, NexusId asc", groupId);
		}

		/// <summary>
		/// Gets the positions with turns.
		/// </summary>
		/// <returns>The positions with turns.</returns>
		/// <param name="daysAgo">Days ago.</param>
		public static List<Position> GetPositionsWithTurns(int daysAgo)
		{
			return Query<Position> ("select p.* from Position p, Notification n where p.Id = n.PositionId and n.Type = ? and n.DaysAgo = ?", Notification.NotificationType.Turns, daysAgo);
		}

		/// <summary>
		/// Gets the positions in star system.
		/// </summary>
		/// <returns>The positions in star system.</returns>
		/// <param name="starSystemId">Star system identifier.</param>
		public static List<Position> GetPositionsInStarSystem(int starSystemId)
		{
			return Query<Position> ("select p.* from Position p where p.StarSystemId = ? order by Name asc", starSystemId);
		}

		/// <summary>
		/// Gets the positions with orders.
		/// </summary>
		/// <returns>The positions with orders.</returns>
		public static List<Position> GetPositionsWithOrders()
		{
			return Query<Position> ("select distinct p.* from Position p, `Order` o where o.PositionId = p.Id order by p.Name asc");
		}

		/// <summary>
		/// Gets the positions with notes.
		/// </summary>
		/// <returns>The positions with notes.</returns>
		public static List<Position> GetPositionsWithNotes()
		{
			return Query<Position> ("select distinct p.* from Position p, PositionNote pn where pn.Id = p.Id order by p.Name asc");
		}

		/// <summary>
		/// Gets the type of the positions of.
		/// </summary>
		/// <returns>The positions of type.</returns>
		/// <param name="positionType">Position type.</param>
		public static List<Position> GetPositionsOfType(Position.PositionFlag positionType)
		{
			if (positionType != Position.PositionFlag.None) {
				return Query<Position> ("select p.* from Position p where p.PositionType = ? order by p.Name asc", positionType);
			} else {
				return Query<Position> ("select p.* from Position p order by p.Name asc");
			}
		}

        /// <summary>
        /// Gets the item properties.
        /// </summary>
        /// <returns>The item properties.</returns>
        /// <param name="itemId">Item identifier.</param>
        public static List<ItemProperty> GetItemProperties(int itemId)
        {
            return Query<ItemProperty> ("select ip.* from ItemProperty ip where ip.ItemId = ?", itemId);
        }

        /// <summary>
        /// Gets the raw materials.
        /// </summary>
        /// <returns>The raw materials.</returns>
        /// <param name="itemId">Item identifier.</param>
        public static List<RawMaterial> GetRawMaterials(int itemId)
        {
            return Query<RawMaterial> ("select rm.* from RawMaterial rm where rm.ItemId = ?", itemId);
        }

        /// <summary>
        /// Gets the celestial bodies.
        /// </summary>
        /// <returns>The celestial bodies.</returns>
        /// <param name="starSystemId">Star system identifier.</param>
        public static List<CelestialBody> GetCelestialBodies(int starSystemId)
        {
            return Query<CelestialBody> ("select cb.* from CelestialBody cb where cb.StarSystemId = ?", starSystemId);
        }

        /// <summary>
        /// Gets the jump links.
        /// </summary>
        /// <returns>The jump links.</returns>
        /// <param name="starSystemId">Star system identifier.</param>
        public static List<JumpLink> GetJumpLinks(int starSystemId)
        {
            return Query<JumpLink> ("select jl.* from JumpLink jl where jl.StarSystemId = ?", starSystemId);
        }

		/// <summary>
		/// Gets the order type parameters.
		/// </summary>
		/// <returns>The order parameters.</returns>
		/// <param name="orderId">Order identifier.</param>
		public static List<OrderParameterType> GetOrderTypeParameters(int orderId)
		{
			return Query<OrderParameterType> ("select op.* from OrderParameterType op where op.OrderId = ?", orderId);
		}

		/// <summary>
		/// Gets the orders for position.
		/// </summary>
		/// <returns>The orders for position.</returns>
		/// <param name="positionId">Position identifier.</param>
		public static List<Order> GetOrdersForPosition(int positionId)
		{
			return Query<Order> ("select o.* from `Order` o where o.PositionId = ? order by Id asc", positionId);
		}

		/// <summary>
		/// Gets the order parameters.
		/// </summary>
		/// <returns>The order parameters.</returns>
		/// <param name="orderId">Order identifier.</param>
		public static List<OrderParameter> GetOrderParameters(int orderId)
		{
			return Query<OrderParameter> ("select op.* from OrderParameter op where op.OrderId = ? order by Id asc", orderId);
		}

		/// <summary>
		/// Gets the notifications for position.
		/// </summary>
		/// <returns>The notifications for position.</returns>
		/// <param name="positionId">Position identifier.</param>
		public static List<Notification> GetNotificationsForPosition(int positionId)
		{
			return Query<Notification> ("select n.* from Notification n where n.PositionId = ? order by DaysAgo asc", positionId);
		}

		/// <summary>
		/// Gets the notifications for position.
		/// </summary>
		/// <returns>The notifications for position.</returns>
		/// <param name="positionId">Position identifier.</param>
		/// <param name="priority">Priority.</param>
		public static List<Notification> GetNotificationsForPosition(int positionId, Notification.NotificationPriority priority)
		{
			return Query<Notification> ("select n.* from Notification n where n.PositionId = ? and n.Priority = ? order by DaysAgo asc", positionId, priority);
		}

        /// <summary>
        /// Deletes the item properties.
        /// </summary>
        /// <param name="itemId">Item identifier.</param>
        public static void DeleteItemProperties(int itemId)
        {
            Execute ("delete from ItemProperty where ItemId = ?", itemId);
        }

        /// <summary>
        /// Deletes the raw materials.
        /// </summary>
        /// <param name="itemId">Item identifier.</param>
        public static void DeleteRawMaterials(int itemId)
        {
            Execute("delete from RawMaterial where ItemId = ?", itemId);
        }

        /// <summary>
        /// Deletes the celestial bodies.
        /// </summary>
        /// <param name="starSystemId">Star system identifier.</param>
        public static void DeleteCelestialBodies(int starSystemId)
        {
            Execute("delete from CelestialBody where StarSystemId = ?", starSystemId);
        }

        /// <summary>
        /// Deletes the jump links.
        /// </summary>
        /// <param name="starSystemId">Star system identifier.</param>
        public static void DeleteJumpLinks(int starSystemId)
        {
			Execute("delete from JumpLink where StarSystemId = ?", starSystemId);
        }

		/// <summary>
		/// Deletes the order type parameters.
		/// </summary>
		/// <param name="orderId">Order identifier.</param>
		public static void DeleteOrderTypeParameters(int orderId)
		{
			Execute ("delete from OrderParameterType where OrderId = ?", orderId);
		}

		/// <summary>
		/// Deletes the orders.
		/// </summary>
		/// <param name="positionid">Positionid.</param>
		public static void DeleteOrders(int positionid)
		{
			Execute ("delete from `Order` where PositionId = ?", positionid);
		}

		/// <summary>
		/// Deletes the order parameters.
		/// </summary>
		/// <param name="orderId">Order identifier.</param>
		public static void DeleteOrderParameters(int orderId)
		{
			Execute ("delete from OrderParameter where OrderId = ?", orderId);
		}

		/// <summary>
        /// The locker.
        /// </summary>
        private static object _locker = new object();

        /// <summary>
        /// Tables the name.
        /// </summary>
        /// <returns>The name.</returns>
        /// <param name="t">T.</param>
        private static string TableName(Type t)
        {
            return t.Name;
        }

        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <returns>The message.</returns>
        /// <param name="op">Op.</param>
        /// <param name="t">T.</param>
        /// <param name="e">E.</param>
        /// <param name="id">Identifier.</param>
        private static string ErrorMessage(string op, Type t, Exception e, int id = -1)
        {
            return "Exception " + e.GetType().Name + " performing " + op + " on " + t.Name + " (id: " + id + "): " + e.ToString();
        }

        /// <summary>
        /// Query the specified query and args.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <param name="args">Arguments.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private static List<T> Query<T>(string query, params object[] args) where T : EntityBase, new() 
        {
            try {
                lock(_locker) {
                    return DatabaseProvider.GetConnection().Query<T> (query, args).ToList();
                }
            } catch(Exception e) {
				Log.WriteLine(Log.Layer.DL, typeof(PhoenixDatabase), ErrorMessage("Query", typeof(T), e));
                return new List<T>();
            }
        }

        /// <summary>
        /// Execute the specified sql and args.
        /// </summary>
        /// <param name="sql">Sql.</param>
        /// <param name="args">Arguments.</param>
        private static void Execute(string sql, params object[] args)
        {
			try {
	            lock (_locker) {
	                DatabaseProvider.GetConnection().Execute(sql, args);
	            }
			} catch(Exception e) {
				Log.WriteLine(Log.Layer.DL, typeof(PhoenixDatabase), ErrorMessage("Execute", typeof(string), e));
			}
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <returns>The scalar.</returns>
        /// <param name="sql">Sql.</param>
        /// <param name="args">Arguments.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private static T ExecuteScalar<T>(string sql, params object[] args) where T : new()
        {
			try {
	            lock (_locker) {
	                var c = DatabaseProvider.GetConnection().CreateCommand(sql, args);
	                return c.ExecuteScalar<T>();
	            }
			} catch(Exception e) {
				Log.WriteLine(Log.Layer.DL, typeof(PhoenixDatabase), ErrorMessage("ExecuteScalar", typeof(T), e));
				return default(T);
			}
        }

		/// <summary>
		/// Creates the table.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private static void CreateTable<T>() where T : new()
		{
			try {
				lock(_locker){
					DatabaseProvider.GetConnection().CreateTable<T>();
					Log.WriteLine (Log.Layer.DL, typeof(PhoenixDatabase), "Successfully created table " + TableName(typeof(T)));
				}
			}
			catch(Exception e) {
				Log.WriteLine (Log.Layer.DL, typeof(PhoenixDatabase), ErrorMessage ("CreateTable", typeof(T), e));
			}
		}

		/// <summary>
		/// Drops the table.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private static void DropTable<T>() where T : new()
		{
			try {
				lock(_locker){
					DatabaseProvider.GetConnection().DropTable<T>();
					Log.WriteLine (Log.Layer.DL, typeof(PhoenixDatabase), "Successfully dropped table " + TableName(typeof(T)));
				}
			}
			catch(Exception e) {
				Log.WriteLine (Log.Layer.DL, typeof(PhoenixDatabase), ErrorMessage ("DropTable", typeof(T), e));
			}
		}
    }
}

