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
		public class ReadCacheArgs:EventArgs
		{
			private Geocache m_cache;
			
			public Geocache Cache
			{
				get { return m_cache;}
			}
			
			public ReadCacheArgs(Geocache message):base()
			{
				m_cache = message;
			}
		}
		
#region Properties
		
		public event ReadCacheEventHandler ReadCache;
		public event ReadCompleteEventHandler Complete;
		public delegate void ReadCacheEventHandler(object sender, ReadCacheArgs args);
		public delegate void ReadCompleteEventHandler(object sender, EventArgs args);

		
		private IDbConnection m_conn = null;
		private String m_dbFile = null;
		
		private FilterList m_filter = null;		
		public FilterList Filter
		{
			get { return m_filter;}
			set { m_filter = value;}
		}
		
		private string m_bmrkList = null;
		public string BookmarkList
		{
			get { return m_bmrkList;}
			set { m_bmrkList = value;}
		}
		
		public string DBFile
		{
			get { return m_dbFile;}
		}
		
		public int CacheCount
		{
			get { 	
				IDbConnection conn =  OpenConnection ();
				IDbCommand command = conn.CreateCommand();
				command.CommandText = COUNT_GC;
				if (m_bmrkList != null)
					command.CommandText += String.Format(BMRK_FILTER_COUNT, m_bmrkList);
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
		
		public void AddWaypoint(Waypoint point)
		{
			if (point is Geocache)
				AddCache(point as Geocache);
			UpdateWaypoint(point);
		}
		
		public string GenerateNewName(String testname)
		{
			IDbConnection conn =  OpenConnection ();
			IDbCommand command = conn.CreateCommand();
			command.CommandText = String.Format(WPT_EXISTS_CHECK, testname);
			object val = command.ExecuteScalar();
			int count = int.Parse(val.ToString());
			conn.Close();
			if (count > 0)
			{
				string oldNumStr = testname.Substring(testname.Length -2);
				try
				{
					int num = int.Parse(oldNumStr);
					num++;
					testname = testname.Substring(0, testname.Length -2) + num.ToString("00");
				}
				catch
				{
					testname += "01";
				}
				return GenerateNewName(testname);
			}
			return testname;
		}
		
		
		
		public List<Geocache> GetCaches()
		{
			String sql = GET_GC;
			if (null != m_filter)
				sql += m_filter.BuildWhereClause();
			if (null != m_bmrkList)
				sql += String.Format(BMRK_FILTER, m_bmrkList);
			List<Geocache> caches =  GetCacheList(sql);
			if (this.Complete != null)
				this.Complete(this, new EventArgs());
			return caches;
		}
		
		public List<Geocache> GetFinds()
		{
			String sql = GET_GC + FOUND_ONLY;
			List<Geocache> caches =  GetCacheList(sql);
			if (this.Complete != null)
				this.Complete(this, new EventArgs());
			return caches;
		}
		
		public void AddBookmark(String name)
		{
			IDbTransaction trans = StartUpdate();
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = String.Format(ADD_BMRK, name);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			cmd = null;
			EndUpdate(trans);
		}
		
		public void BookMarkCache(String code, String bmrk)
		{
			IDbTransaction trans = StartUpdate();
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = String.Format(BOOKMARK_CACHE, code, bmrk);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			cmd = null;
			EndUpdate(trans);
		}
		
		public void RemoveCacheFromActiveBookmark(string code)
		{
			IDbTransaction trans = StartUpdate();
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = String.Format(REMOVE_CACHE_FROM_BOOKMARK, code, m_bmrkList);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			cmd = null;
			EndUpdate(trans);
		}
		
		public void DeleteBookmark(string bmrk)
		{
			IDbTransaction trans = StartUpdate();
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = String.Format(String.Format(REMOVE_BOOKMARK, bmrk));
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			cmd = null;
			EndUpdate(trans);
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
			InsertOrUpdate (update, insert, cmd);
		}
		
		public void DeleteWaypoint(String name)
		{
			IDbTransaction trans = StartUpdate();
			if (m_conn == null)
				throw new Exception("DB NOT OPEN");
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = String.Format(DELETE_WPT, SQLEscape(name));
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
		
		public void UpdateCacheAtomic(Geocache pt)
		{
			IDbTransaction trans = StartUpdate();
			UpdateCache(pt);
			EndUpdate(trans);
		}
		
		public void UpdateCache(Geocache cache)
		{
			if (m_conn == null)
				throw new Exception("DB NOT OPEN");
			IDbCommand cmd = m_conn.CreateCommand();
			string insert = String.Format(INSERT_GC, cache.Name, SQLEscape(cache.CacheName), cache.CacheID, 
			                                SQLEscape(cache.CacheOwner), cache.OwnerID, SQLEscape(cache.PlacedBy), 
			                                cache.Difficulty.ToString(CultureInfo.InvariantCulture), cache.Terrain.ToString(CultureInfo.InvariantCulture), SQLEscape(cache.Country), 
			                                SQLEscape(cache.State),cache.TypeOfCache.ToString(), 
			                                SQLEscape(cache.ShortDesc), SQLEscape(cache.LongDesc),
			                                SQLEscape(cache.Hint), cache.Container, cache.Archived.ToString(),
			                                cache.Available.ToString(), SQLEscape(cache.Notes), cache.CheckNotes.ToString());
			string update = String.Format(UPDATE_GC, cache.Name, SQLEscape(cache.CacheName), cache.CacheID, 
			                                SQLEscape(cache.CacheOwner), cache.OwnerID, SQLEscape(cache.PlacedBy), 
			                                cache.Difficulty.ToString(CultureInfo.InvariantCulture), cache.Terrain.ToString(CultureInfo.InvariantCulture), SQLEscape(cache.Country), 
			                                SQLEscape(cache.State),cache.TypeOfCache.ToString(), 
			                                SQLEscape(cache.ShortDesc), SQLEscape(cache.LongDesc),
			                                SQLEscape(cache.Hint), cache.Container, cache.Archived.ToString(),
			                                cache.Available.ToString(), SQLEscape(cache.Notes), cache.CheckNotes.ToString());
			InsertOrUpdate (update, insert, cmd);
		}
		
		public void AddCache(Geocache cache)
		{
				if (m_conn == null)
				throw new Exception("DB NOT OPEN");
			IDbCommand cmd = m_conn.CreateCommand();
			string insert = String.Format(INSERT_GC, cache.Name, SQLEscape(cache.CacheName), cache.CacheID, 
			                                SQLEscape(cache.CacheOwner), cache.OwnerID, SQLEscape(cache.PlacedBy), 
			                                cache.Difficulty.ToString(CultureInfo.InvariantCulture), cache.Terrain.ToString(CultureInfo.InvariantCulture), SQLEscape(cache.Country), 
			                                SQLEscape(cache.State),cache.TypeOfCache.ToString(), 
			                                SQLEscape(cache.ShortDesc), SQLEscape(cache.LongDesc),
			                                SQLEscape(cache.Hint), cache.Container, cache.Archived.ToString(),
			                                cache.Available.ToString(), SQLEscape(cache.Notes), cache.CheckNotes.ToString());
			string update = String.Format(ADD_EXISTING_GC, cache.Name, SQLEscape(cache.CacheName), cache.CacheID, 
			                                SQLEscape(cache.CacheOwner), cache.OwnerID, SQLEscape(cache.PlacedBy), 
			                                cache.Difficulty.ToString(CultureInfo.InvariantCulture), cache.Terrain.ToString(CultureInfo.InvariantCulture), SQLEscape(cache.Country), 
			                                SQLEscape(cache.State),cache.TypeOfCache.ToString(), 
			                                SQLEscape(cache.ShortDesc), SQLEscape(cache.LongDesc),
			                                SQLEscape(cache.Hint), cache.Container, cache.Archived.ToString(),
			                                cache.Available.ToString(), cache.CheckNotes.ToString());
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
			
		public DateTime GetLastLogByYou(Geocache cache, String ownerID)
		{
			IDbConnection conn = OpenConnection();
			IDbCommand command = conn.CreateCommand();
			command.CommandText = String.Format(LAST_LOG_BY_YOU, cache.Name, ownerID);
			IDataReader reader = command.ExecuteReader();
			DateTime date = DateTime.MinValue;
			while (reader.Read())
			{
				string val = reader.GetString(0);
				if (!String.IsNullOrEmpty(val))
					date = DateTime.Parse(val);
					
			}
			CloseConnection(ref reader, ref command, ref conn);
			return date;
		}
		
		public DateTime GetLastFindByYou(Geocache cache, String ownerID)
		{
			IDbConnection conn = OpenConnection();
			IDbCommand command = conn.CreateCommand();
			command.CommandText = String.Format(LAST_FIND_BY_YOU, cache.Name, ownerID);
			IDataReader reader = command.ExecuteReader();
			DateTime date = DateTime.MinValue;
			while (reader.Read())
			{
				string val = reader.GetString(0);
				if (!String.IsNullOrEmpty(val))
					date = DateTime.Parse(val);
					
			}
			CloseConnection(ref reader, ref command, ref conn);
			return date;
		}
		
		public CacheLog GetLastFindLogByYou(Geocache cache, String ownerID)
		{
			IDbConnection conn = OpenConnection();
			IDbCommand command = conn.CreateCommand();
			command.CommandText = String.Format(LAST_FIND_BY_YOU, cache.Name, ownerID);
			IDataReader rdr = command.ExecuteReader();
			CacheLog log = new CacheLog();
			while (rdr.Read())
			{
				log.LogDate = DateTime.Parse(rdr.GetString(0));
				log.LoggedBy = rdr.GetString(1);
				log.LogMessage = rdr.GetString(2);
				log.LogStatus = rdr.GetString(3);
				log.FinderID = rdr.GetString(4);
				String encoded = rdr.GetString(5);
				log.Encoded = Boolean.Parse(encoded);
					
			}
			CloseConnection(ref rdr, ref command, ref conn);
			return log;
		}
		
		public List<string> GetBookmarkLists()
		{
			List<string> bmrks = new List<string>();
			IDbConnection conn =  OpenConnection ();
			IDbCommand command = conn.CreateCommand();
			command.CommandText = GET_BMRKS;
			IDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
			 	bmrks.Add(reader.GetString(0));
			}
			CloseConnection (ref reader, ref command, ref conn);	
			return bmrks;
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
				Geocache cache = BuildCache(reader);
				if (cache != null)
					pts.Add(cache);
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
			if (!String.IsNullOrEmpty(url))
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
			Object val = reader.GetValue(26);
			if (val is string)
				cache.Notes = reader.GetString(26);
			val = reader.GetValue(27);
			if (val is string)
				cache.CheckNotes = Boolean.Parse(val as string);
			else
				cache.CheckNotes = false;
			val = reader.GetValue(28);
			if (val != DBNull.Value)
				cache.Children = true;
			else
				cache.Children = false;
			
			
			
			
			if (this.ReadCache != null)
			{
				if (!DoNonDBFilter(cache))
				{
					this.ReadCache(this, new ReadCacheArgs(null));
					return null;
				}
				this.ReadCache(this, new ReadCacheArgs(cache));
			}
			return cache;
			
		}
		
		private bool DoNonDBFilter (Geocache cache)
		{
			if (m_filter != null)
			{
				string ownerID = m_filter.GetCriteria(FilterList.KEY_OWNERID) as String;
				if (m_filter.Contains(FilterList.KEY_PLACEBEFORE))
					if (cache.Time >= ((DateTime) m_filter.GetCriteria(FilterList.KEY_PLACEBEFORE)))
						return false;
				if (m_filter.Contains(FilterList.KEY_PLACEAFTER))
					if (cache.Time <= ((DateTime) m_filter.GetCriteria(FilterList.KEY_PLACEAFTER)))
						return false;
				if (m_filter.Contains(FilterList.KEY_INFOBEFORE))
					if (cache.Updated >= ((DateTime) m_filter.GetCriteria(FilterList.KEY_INFOBEFORE)))
						return false;
				if (m_filter.Contains(FilterList.KEY_INFOAFTER))
					if (cache.Updated <= ((DateTime) m_filter.GetCriteria(FilterList.KEY_INFOAFTER)))
						return false;
				if (m_filter.Contains(FilterList.KEY_FOUNDON))
				{
					if (cache.Found)
					{
						if (Engine.getInstance().Store.GetLastFindByYou(cache, ownerID).Date != ((DateTime) m_filter.GetCriteria(FilterList.KEY_FOUNDON)).Date)
							return false;
					}
					else 
						return false;
				}
				if (m_filter.Contains(FilterList.KEY_FOUNDBEFORE))
				{
					if (cache.Found)
					{
						if (Engine.getInstance().Store.GetLastFindByYou(cache, ownerID).Date > ((DateTime) m_filter.GetCriteria(FilterList.KEY_FOUNDBEFORE)).Date)
							return false;
					}
					else 
						return false;
				}
				if (m_filter.Contains(FilterList.KEY_FOUNDAFTER))
				{
					if (cache.Found)
					{
						if (Engine.getInstance().Store.GetLastFindByYou(cache, ownerID).Date < ((DateTime) m_filter.GetCriteria(FilterList.KEY_FOUNDAFTER)).Date)
							return false;
					}
					else 
						return false;
				}
			}
			return true;
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
		
		public void AddLog(String cachename, CacheLog log)
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
		
		public bool NeedsUpgrade()
		{
			try
			{
				int ver =0;
				ver = GetDBVersion ();
				if (ver < 2)
					return true;
				return false;
			}
			catch
			{
				// OCM version 0.15 DB
				return true;
			}
			
		}
		
		public void Upgrade()
		{
			IDbConnection conn = OpenConnection();
			IDbCommand cmd = conn.CreateCommand();
			int ver = GetDBVersion();
			if (ver == 0)
			{
				cmd.CommandText = UPGRADE_GEOCACHE_V0_V1;
				cmd.ExecuteNonQuery();
				cmd.CommandText = CREATE_TABLE_BMRK;
				cmd.ExecuteNonQuery();
				cmd.CommandText = CREATE_TABLE_BMRK_CACHES;
				cmd.ExecuteNonQuery();
				cmd.CommandText = CREATE_DB_VER;
				cmd.ExecuteNonQuery();
			}
			cmd.CommandText = CLEAR_DB_VER;
			cmd.ExecuteNonQuery();
			cmd.CommandText = UPGRADE_GEOCACHE_V1_V2;
			cmd.ExecuteNonQuery();
			cmd.CommandText = SET_DB_VER;
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			conn.Close();
		//	ScanLogs();
		}
		
		public void Compact()
		{
			IDbConnection conn =  OpenConnection ();
			IDbCommand command = conn.CreateCommand();
			command.CommandText = VACUUM;
			command.ExecuteNonQuery();
			conn.Close();
		}
		
		private int GetDBVersion ()
		{
			int ver = 0;
			IDbConnection conn =  OpenConnection ();
			IDbCommand command = conn.CreateCommand();
			command.CommandText = GET_DB_VER;
			IDataReader rdr = command.ExecuteReader();
			
			while (rdr.Read())
			{
				ver = rdr.GetInt32(0);
			}
			conn.Close();
			return ver;
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
			cmd.CommandText = CREATE_TABLE_BMRK;
			cmd.ExecuteNonQuery();
			cmd.CommandText = CREATE_TABLE_BMRK_CACHES;
			cmd.ExecuteNonQuery();
			cmd.CommandText = CREATE_DB_VER;
			cmd.ExecuteNonQuery();
			cmd.CommandText = SET_DB_VER;
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			conn.Close();
		}		
	}		
}
