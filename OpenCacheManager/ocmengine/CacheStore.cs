// 
//  Copyright 2010  Kyle Campbell
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Globalization;

namespace ocmengine
{
	
	
	public partial class CacheStore
	{
		
#region Properties
		
		private IDbConnection m_conn = null;
		private String m_dbFile = null;
		
		private FilterList m_filter = null;		
		public FilterList Filter
		{
			get { return m_filter;}
			set { m_filter = value;}
		}
		
		public int CacheCount
		{
			get { 	
				IDbConnection conn =  OpenConnection ();
				IDbCommand command = conn.CreateCommand();
				command.CommandText = COUNT_GC;
				object val = command.ExecuteScalar();
				int count = int.Parse(val.ToString());
				conn.Close();
				return count;
			}
		}
		
		public int FoundCount
		{
			get { 	
				IDbConnection conn =  OpenConnection ();
				IDbCommand command = conn.CreateCommand();
				command.CommandText = COUNT_GC + FOUND;
				object val = command.ExecuteScalar();
				int count = int.Parse(val.ToString());
				conn.Close();
				return count;
			}
		}
		
		public int InactiveCount
		{
			get { 	
				IDbConnection conn =  OpenConnection ();
				IDbCommand command = conn.CreateCommand();
				command.CommandText = COUNT_GC + INACTIVE;
				object val = command.ExecuteScalar();
				int count = int.Parse(val.ToString());
				conn.Close();
				return count;
			}
		}
		
		public int WaypointCount
		{
			get { 	
				IDbConnection conn =  OpenConnection ();
				IDbCommand command = conn.CreateCommand();
				command.CommandText = COUNT_WPT;
				int count = (int) command.ExecuteScalar();
				conn.Close();
				return count;
			}
		}
		
#endregion
		
		public CacheStore()
		{
		}
		
		
		public void AddWaypointAtomic(Waypoint point)
		{
			IDbTransaction trans = StartUpdate();
			AddWaypoint(point);
			EndUpdate(trans);			
		}
		
		internal void AddWaypoint(Waypoint point)
		{
			if (point is Geocache)
				UpdateCache(point as Geocache);
			UpdateWaypoint(point);
		}
		
		
		
		public IEnumerator<Geocache> getCacheEnumerator()
		{
			String sql = GET_GC;
			if (null != m_filter)
				sql += m_filter.BuildWhereClause();
			List<Geocache> caches =  GetCacheList(sql);
			return caches.GetEnumerator();
		}
		
		
		public List<Waypoint> GetChildren(String cachecode)
		{
			String whereClause = String.Format(WHERE_PARENT, cachecode);
			return GetWayPointList(GET_WPTS + whereClause);
		}
		
		public void UpdateWaypointAtomic(Waypoint pt)
		{
			IDbTransaction trans = StartUpdate();
			UpdateWaypoint(pt);
			EndUpdate(trans);
		}
		
		internal void UpdateWaypoint(Waypoint pt)
		{
			if (m_conn == null)
				throw new Exception("DB NOT OPEN");
			IDbCommand cmd = m_conn.CreateCommand();			
			string insert = String.Format(INSERT_WPT, SQLEscape(pt.Name), pt.Lat.ToString(CultureInfo.InvariantCulture), pt.Lon.ToString(CultureInfo.InvariantCulture), pt.URL, 
			                                SQLEscape(pt.URLName), SQLEscape(pt.Desc), pt.Symbol, pt.Type,
			                                pt.Time.ToString("o"), pt.Parent, pt.Updated.ToString("o"));
			string update = String.Format(UPDATE_WPT, SQLEscape(pt.Name), pt.Lat.ToString(CultureInfo.InvariantCulture), pt.Lon.ToString(CultureInfo.InvariantCulture), pt.URL, 
			                                SQLEscape(pt.URLName), SQLEscape(pt.Desc), pt.Symbol, pt.Type,
			                                pt.Time.ToString("o"), pt.Parent, pt.Updated.ToString("o"));			
			System.Console.WriteLine(insert);
			InsertOrUpdate (update, insert, cmd);
		}
		
		public void DeleteWaypoint(Waypoint pt)
		{
			IDbTransaction trans = StartUpdate();
			if (m_conn == null)
				throw new Exception("DB NOT OPEN");
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = String.Format(DELETE_WPT, SQLEscape(pt.Name));
			cmd.ExecuteNonQuery();
			EndUpdate(trans);
		}
		
		public void DeleteGeocache(Geocache cache)
		{
			IDbTransaction trans = StartUpdate();
			String deleteGC = String.Format(DELETE_GC, SQLEscape(cache.Name));
			String deleteTBS = String.Format(DELETE_TBS, SQLEscape(cache.Name));
			String deleteLogs = String.Format(DELETE_LOGS, SQLEscape(cache.Name));
			String deleteWpt = String.Format(DELETE_WPT + OR_PARENT, SQLEscape(cache.Name));
			if (m_conn == null)
				throw new Exception("DB NOT OPEN");
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = deleteGC + SEPERATOR + deleteLogs + SEPERATOR + deleteTBS + SEPERATOR + deleteWpt; 
			cmd.ExecuteNonQuery();		
			EndUpdate(trans);
		}
		
		public void UpdateCache(Geocache cache)
		{
			if (m_conn == null)
				throw new Exception("DB NOT OPEN");
			IDbCommand cmd = m_conn.CreateCommand();
			string insert = String.Format(INSERT_GC, cache.Name, SQLEscape(cache.CacheName), cache.CacheID, 
			                                SQLEscape(cache.CacheOwner), cache.OwnerID, SQLEscape(cache.PlacedBy), 
			                                cache.Difficulty, cache.Terrain, SQLEscape(cache.Country), 
			                                SQLEscape(cache.State),cache.TypeOfCache.ToString(), 
			                                SQLEscape(cache.ShortDesc), SQLEscape(cache.LongDesc),
			                                SQLEscape(cache.Hint), cache.Container, cache.Archived.ToString(),
			                                cache.Available.ToString());
			string update = String.Format(UPDATE_GC, cache.Name, SQLEscape(cache.CacheName), cache.CacheID, 
			                                SQLEscape(cache.CacheOwner), cache.OwnerID, SQLEscape(cache.PlacedBy), 
			                                cache.Difficulty, cache.Terrain, SQLEscape(cache.Country), 
			                                SQLEscape(cache.State),cache.TypeOfCache.ToString(), 
			                                SQLEscape(cache.ShortDesc), SQLEscape(cache.LongDesc),
			                                SQLEscape(cache.Hint), cache.Container, cache.Archived.ToString(),
			                                cache.Available.ToString());;
			InsertOrUpdate (update, insert, cmd);
		}
		
		private static void InsertOrUpdate (string update, string insert, IDbCommand cmd)
		{
			cmd.CommandText = update;
			int changed = cmd.ExecuteNonQuery();
			if (0 == changed)
			{
				cmd.CommandText = insert;
				cmd.ExecuteNonQuery();
			}
			cmd.Dispose();
			cmd = null;
		}
		
		private List<Waypoint> GetWayPointList(String sql)
		{
			List<Waypoint> pts = new List<Waypoint>();
			IDbConnection conn =  OpenConnection ();
			IDbCommand command = conn.CreateCommand();
			command.CommandText = sql;
			IDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				pts.Add(BuildWaypoint(reader));
			}
			CloseConnection (ref reader, ref command, ref conn);	
			return pts;
		}
		
		private List<Geocache> GetCacheList(String sql)
		{
			List<Geocache> pts = new List<Geocache>();
			IDbConnection conn =  OpenConnection ();
			IDbCommand command = conn.CreateCommand();
			command.CommandText = sql;
			IDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				pts.Add(BuildCache(reader));
			}
			CloseConnection (ref reader, ref command, ref conn);	
			return pts;				
		}
		
		private static void CloseConnection (ref IDataReader reader, ref IDbCommand command, ref IDbConnection conn)
		{
			reader.Close();
			reader = null;
			command.Dispose();
			command = null;
			conn.Close();
			conn = null;	
		}
		
		private Waypoint BuildWaypoint(IDataReader reader)
		{
			Waypoint pt = new Waypoint();
			pt.Name = reader.GetString(0);
			pt.Lat = double.Parse(reader.GetString(1), CultureInfo.InvariantCulture);
			pt.Lon = double.Parse(reader.GetString(2), CultureInfo.InvariantCulture);
			string url = reader.GetString(3);
			if (!String.IsNullOrEmpty(url))
				pt.URL = new Uri(url);
			pt.URLName = reader.GetString(4);
			pt.Desc = reader.GetString(5);
			pt.Symbol = reader.GetString(6);
			pt.Type = reader.GetString(7);
			pt.Time = DateTime.Parse(reader.GetString(8));
			pt.Parent = reader.GetString(9);
			pt.Updated = DateTime.Parse(reader.GetString(10));
			return pt;
		}
		
		private Geocache BuildCache(IDataReader reader)
		{
			Geocache cache = new Geocache();
			cache.Name = reader.GetString(0);
			cache.Lat = double.Parse(reader.GetString(1), CultureInfo.InvariantCulture);
			cache.Lon = double.Parse(reader.GetString(2), CultureInfo.InvariantCulture);
			String url = reader.GetString(3);
			if (url != null)
				cache.URL = new Uri(url);
			cache.URLName = reader.GetString(4);
			cache.Desc = reader.GetString(5);
			cache.Symbol = reader.GetString(6);
			cache.Type = reader.GetString(7);
			String time = reader.GetString(8);
			cache.Time = DateTime.Parse(time);	
			cache.CacheName = reader.GetString(9);
			cache.CacheID = reader.GetString(10);
			cache.CacheOwner = reader.GetString(11);
			cache.OwnerID = reader.GetString(12);
			cache.PlacedBy = reader.GetString(13);
			cache.Difficulty = float.Parse(reader.GetString(14), CultureInfo.InvariantCulture);
			cache.Terrain = float.Parse(reader.GetString(15), CultureInfo.InvariantCulture);
			cache.Country = reader.GetString(16);
			cache.State = reader.GetString(17);
			cache.TypeOfCache = (Geocache.CacheType) Enum.Parse(typeof (Geocache.CacheType), reader.GetString(18));
			cache.ShortDesc = reader.GetString(19);
			cache.LongDesc = reader.GetString(20);
			cache.Hint = reader.GetString(21);
			cache.Container = reader.GetString(22);
			String archived = reader.GetString(23);
			cache.Archived = Boolean.Parse(archived);
			String available = reader.GetString(24);
			cache.Available = Boolean.Parse(available);
			cache.Updated = DateTime.Parse(reader.GetString(25));
			return cache;
			
		}
		
		public void ClearLogs(String cachename)
		{
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = String.Format(DELETE_LOGS, cachename);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			cmd = null;
		}
		
		public void AddLogAtomic(String cachename, CacheLog log)
		{
			IDbTransaction trans = StartUpdate();
			AddLog(cachename, log);
			EndUpdate(trans);
		}
		
		internal void AddLog(String cachename, CacheLog log)
		{
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = String.Format(ADD_LOG, cachename, log.LogDate.ToString("o"), SQLEscape(log.LoggedBy), SQLEscape(log.LogMessage), SQLEscape(log.LogStatus), log.FinderID, log.Encoded.ToString());
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			cmd = null;
		}
		
		public void ClearTBs(String cachename)
		{
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = String.Format(DELETE_TBS, cachename);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			cmd = null;
		}
		
		public void AddTB(String cachename, TravelBug bug)
		{
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = String.Format(ADD_TB, cachename, bug.ID, bug.Ref, SQLEscape(bug.Name));
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			cmd = null;
		}
		
		public IDbTransaction StartUpdate()
		{
			if (m_conn == null)
			{
				m_conn = OpenConnection();
			}
			return m_conn.BeginTransaction();
		}
		
		public void EndUpdate(IDbTransaction trans)
		{
			trans.Commit();
			trans.Dispose();
			m_conn.Close();
			m_conn = null;
		}
		
		public void CancelUpdate(IDbTransaction trans)
		{
			trans.Rollback();
			trans.Dispose();
			m_conn.Close();
			m_conn = null;
		}
		
		public List<CacheLog> GetCacheLogs(String cachename)
		{
			List<CacheLog> logs = new List<CacheLog>();
			IDbConnection conn = OpenConnection();
			IDbCommand cmd =  conn.CreateCommand();
			cmd.CommandText = String.Format(GET_LOGS, cachename);
			IDataReader rdr = cmd.ExecuteReader();
			while (rdr.Read())
			{
				CacheLog log = new CacheLog();
				log.LogDate = DateTime.Parse(rdr.GetString(0));
				log.LoggedBy = rdr.GetString(1);
				log.LogMessage = rdr.GetString(2);
				log.LogStatus = rdr.GetString(3);
				log.FinderID = rdr.GetString(4);
				String encoded = rdr.GetString(5);
				log.Encoded = Boolean.Parse(encoded);
				logs.Add(log);
			}
			CloseConnection(ref rdr, ref cmd, ref conn);
			return logs;			
		}
		
			
		public List<TravelBug> GetTravelBugs(String cachename)
		{
			List<TravelBug> bugs = new List<TravelBug>();
			IDbConnection conn = OpenConnection();
			IDbCommand cmd =  conn.CreateCommand();
			cmd.CommandText = String.Format(GET_TB, cachename);
			IDataReader rdr = cmd.ExecuteReader();
			while (rdr.Read())
			{
				TravelBug bug = new TravelBug();
				bug.ID = rdr.GetString(0);
				bug.Ref = rdr.GetString(1);
				bug.Name = rdr.GetString(2);
				bug.Cache = cachename;
				bugs.Add(bug);
			}
			CloseConnection(ref rdr, ref cmd, ref conn);
			return bugs;			
		}
		
		private static String SQLEscape(String unescapedString)
		{
			return unescapedString.Replace("'", "''");
		}
		
		private IDbConnection OpenConnection ()
		{
			IDbConnection conn = (IDbConnection) new SqliteConnection(SQL_CONNECT + m_dbFile);
			conn.Open();
			return conn;
		}
		
		public void SetDB(String filePath)
		{
			m_dbFile = filePath;
		}
		
		public void CreateDB(String filePath)
		{
			m_dbFile = filePath;
			IDbConnection conn = OpenConnection();
			IDbCommand cmd = conn.CreateCommand();
			cmd.CommandText = CREATE_CACHE_TABLE;
			cmd.ExecuteNonQuery();
			cmd.CommandText = CREATE_LOGS_TABLE;
			cmd.ExecuteNonQuery();
			cmd.CommandText = CREATE_TABLE_TBUGS;
			cmd.ExecuteNonQuery();
			cmd.CommandText = CREATE_TABLE_WPTS;
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			conn.Close();
		}		
	}		
}
