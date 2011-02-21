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
using System.Text;

namespace ocmengine
{
	
	
	public partial class CacheStore
	{
		private List<String> m_hasChildrenList = new List<String>();
		private List<String> m_hasFinalList = new List<String>();
		private double m_lat=-1;
		private double m_lon=-1;
		
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
		
		private List<FilterList> m_comboList = null;
		public List<FilterList> ComboFilter
		{
			get { return m_comboList;}
			set { 
				m_comboList = value;
				m_filter = null;
			}
		}
		
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
			AddWaypoint(point, false);
		}
		
		public void AddWaypoint(Waypoint point, bool noOverwrite)
		{
			if (point is Geocache)
				AddCache(point as Geocache);
			UpdateWaypoint(point, noOverwrite);
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
		
		
		public Geocache GetCache(String code)
		{
			String sql = GET_GC + " AND GEOCACHE.name =='" + code + "'";
			List<Geocache> caches = GetCacheList(sql);
			if (caches.Count <= 0)
				return null;
			else
				return caches[0];
		}
		
		public List<Geocache> GetCaches(List<string> codes)
		{
			bool isFirst = true;
			StringBuilder builder = new StringBuilder();
			builder.Append(GET_GC);
			builder.Append(" AND GEOCACHE.name IN(");
			foreach(string code in codes)
			{
				if (isFirst)
					isFirst = false;
				else
					builder.Append(",");
				builder.Append("'");
				builder.Append(code);
				builder.Append("'");
			}
			builder.Append(")");
			List<Geocache> caches = GetCacheList(builder.ToString());
			return caches;
		}
		
		public List<Geocache> GetCaches(double lat, double lon)
		{
			m_lat = lat;
			m_lon = lon;
			List<Geocache> caches = new List<Geocache>();
			if (m_comboList != null)
			{
				System.Console.WriteLine("COMBO!");
				foreach (FilterList filter in m_comboList.ToArray())
				{
					m_filter = filter;
					DoBuildCacheList (caches);
				}
			}
			else
			{
				DoBuildCacheList(caches);
			}		
		
			if (this.Complete != null)
				this.Complete(this, new EventArgs());
			return caches;
		}
		
		private void DoBuildCacheList (List<Geocache> caches)
		{
			String sql = GET_GC;
			if (null != m_filter)
				sql += m_filter.BuildWhereClause();
			if (null != m_bmrkList)
				sql += String.Format(BMRK_FILTER, m_bmrkList);
			String prefilter = DoPrefilter();
			if (null != prefilter)
				sql += prefilter;
			//System.Console.WriteLine(sql);
			GetCacheList(sql, caches);
		}
		
		public List<Geocache> GetFinds()
		{
			String sql = GET_GC + FOUND_ONLY;
			List<Geocache> caches =  GetCacheList(sql);
			if (this.Complete != null)
				this.Complete(this, new EventArgs());
			return caches;
		}
		
		public void AddBookmarkAtomic(String name)
		{
			IDbTransaction trans = StartUpdate();
			AddBookmark(name);
			EndUpdate(trans);
		}
		
		public void AddBookmark(String name)
		{
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = String.Format(ADD_BMRK, name);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			cmd = null;
		}
		
		public void BookMarkCacheAtomic(String code, String bmrk)
		{
			IDbTransaction trans = StartUpdate();
			BookMarkCache(code,bmrk);
			EndUpdate(trans);
		}
		
		public void BookMarkCache(String code, String bmrk)
		{
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = String.Format(BOOKMARK_CACHE, code, bmrk);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			cmd = null;
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
			UpdateWaypoint(pt, false);
			EndUpdate(trans);
		}
		
		internal void UpdateWaypoint(Waypoint pt, bool noOverwrite)
		{
			if (m_conn == null)
				throw new Exception("DB NOT OPEN");
			double lat = pt.Lat;
			double lon = pt.Lon;
			if (pt is Geocache)
			{
				lat = (pt as Geocache).OrigLat;
				lon = (pt as Geocache).OrigLon;
			}
			
			
			IDbCommand cmd = m_conn.CreateCommand();			
			string insert = String.Format(INSERT_WPT, SQLEscape(pt.Name), lat.ToString(CultureInfo.InvariantCulture), lon.ToString(CultureInfo.InvariantCulture), pt.URL, 
			                                SQLEscape(pt.URLName), SQLEscape(pt.Desc), pt.Symbol, pt.Type,
			                                pt.Time.ToString("o"), pt.Parent, pt.Updated.ToString("o"));
			string update;
			if (noOverwrite)
				update = String.Format(UPDATE_WPT_NO_SYM, SQLEscape(pt.Name), lat.ToString(CultureInfo.InvariantCulture), lon.ToString(CultureInfo.InvariantCulture), pt.URL, 
			                                SQLEscape(pt.URLName), SQLEscape(pt.Desc), pt.Type,
			                                pt.Time.ToString("o"), pt.Parent, pt.Updated.ToString("o"));	
			else
				update = String.Format(UPDATE_WPT, SQLEscape(pt.Name), lat.ToString(CultureInfo.InvariantCulture), lon.ToString(CultureInfo.InvariantCulture), pt.URL, 
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
		
		public void DeleteGeocacheAtomic(Geocache cache)
		{
			IDbTransaction trans = StartUpdate();
			String cacheName = "'" + SQLEscape(cache.Name) + "'";
			String deleteGC = String.Format(DELETE_GC, cache.Name);
			String deleteTBS = String.Format(DELETE_TBS, cacheName);
			String deleteLogs = String.Format(DELETE_LOGS, cacheName);
			String deleteAttrs = String.Format(DELETE_ATTRIBUTES, cacheName);
			String deleteWpt = String.Format(DELETE_WPT + OR_PARENT, SQLEscape(cache.Name));
			if (m_conn == null)
				throw new Exception("DB NOT OPEN");
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = deleteGC + SEPERATOR + deleteLogs + SEPERATOR + deleteTBS + SEPERATOR + deleteAttrs 
				+ SEPERATOR + deleteWpt; 
			cmd.ExecuteNonQuery();		
			EndUpdate(trans);
		}
		
		public void DeleteGeocache(Geocache cache)
		{
			String deleteGC = String.Format(DELETE_GC, SQLEscape(cache.Name));
			String deleteWpt = String.Format(DELETE_WPT + OR_PARENT, SQLEscape(cache.Name));
			if (m_conn == null)
				throw new Exception("DB NOT OPEN");
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = deleteGC + SEPERATOR + deleteWpt; 
			cmd.ExecuteNonQuery();		
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
			                                cache.Available.ToString(), SQLEscape(cache.Notes), cache.CheckNotes.ToString(),
			                              	cache.CorrectedLat.ToString(CultureInfo.InvariantCulture), cache.CorrectedLon.ToString(CultureInfo.InvariantCulture),
			                              	cache.DNF, cache.FTF, cache.User1, cache.User2, cache.User3, cache.User4);
			string update = String.Format(UPDATE_GC, cache.Name, SQLEscape(cache.CacheName), cache.CacheID, 
			                                SQLEscape(cache.CacheOwner), cache.OwnerID, SQLEscape(cache.PlacedBy), 
			                                cache.Difficulty.ToString(CultureInfo.InvariantCulture), cache.Terrain.ToString(CultureInfo.InvariantCulture), SQLEscape(cache.Country), 
			                                SQLEscape(cache.State),cache.TypeOfCache.ToString(), 
			                                SQLEscape(cache.ShortDesc), SQLEscape(cache.LongDesc),
			                                SQLEscape(cache.Hint), cache.Container, cache.Archived.ToString(),
			                                cache.Available.ToString(), SQLEscape(cache.Notes), cache.CheckNotes.ToString(),
			                              	cache.CorrectedLat.ToString(CultureInfo.InvariantCulture), cache.CorrectedLon.ToString(CultureInfo.InvariantCulture),
			                              	cache.DNF, cache.FTF, cache.User1, cache.User2, cache.User3, cache.User4);
			InsertOrUpdate (update, insert, cmd);
		}
		
		public void AddCache(Geocache cache)
		{
			if (m_conn == null)
				throw new Exception("DB NOT OPEN");
			IDbCommand cmd = m_conn.CreateCommand();
			bool hasExtraFields = false;
			if (cache.HasCorrected ||
			    !String.IsNullOrEmpty(cache.Notes) ||
			    !String.IsNullOrEmpty(cache.User1) ||
			    !String.IsNullOrEmpty(cache.User2) ||
			    !String.IsNullOrEmpty(cache.User3) ||
			    !String.IsNullOrEmpty(cache.User4) ||
			    cache.DNF ||
			    cache.FTF)
				hasExtraFields = true;
			string insert = String.Format(INSERT_GC, cache.Name, SQLEscape(cache.CacheName), cache.CacheID, 
			                                SQLEscape(cache.CacheOwner), cache.OwnerID, SQLEscape(cache.PlacedBy), 
			                                cache.Difficulty.ToString(CultureInfo.InvariantCulture), cache.Terrain.ToString(CultureInfo.InvariantCulture), SQLEscape(cache.Country), 
			                                SQLEscape(cache.State),cache.TypeOfCache.ToString(), 
			                                SQLEscape(cache.ShortDesc), SQLEscape(cache.LongDesc),
			                                SQLEscape(cache.Hint), cache.Container, cache.Archived.ToString(),
			                                cache.Available.ToString(), SQLEscape(cache.Notes), cache.CheckNotes.ToString(),
			                              	cache.CorrectedLat.ToString(CultureInfo.InvariantCulture), cache.CorrectedLon.ToString(CultureInfo.InvariantCulture),
			                              	cache.DNF, cache.FTF, cache.User1, cache.User2, cache.User3, cache.User4);
			string update;
			if (!hasExtraFields)
			{
				update = String.Format(ADD_EXISTING_GC, cache.Name, SQLEscape(cache.CacheName), cache.CacheID, 
			                                SQLEscape(cache.CacheOwner), cache.OwnerID, SQLEscape(cache.PlacedBy), 
			                                cache.Difficulty.ToString(CultureInfo.InvariantCulture), cache.Terrain.ToString(CultureInfo.InvariantCulture), SQLEscape(cache.Country), 
			                                SQLEscape(cache.State),cache.TypeOfCache.ToString(), 
			                                SQLEscape(cache.ShortDesc), SQLEscape(cache.LongDesc),
			                                SQLEscape(cache.Hint), cache.Container, cache.Archived.ToString(),
			                                cache.Available.ToString(), cache.CheckNotes.ToString(),
			                              	cache.CorrectedLat.ToString(CultureInfo.InvariantCulture), cache.CorrectedLon.ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				update =  String.Format(UPDATE_GC, cache.Name, SQLEscape(cache.CacheName), cache.CacheID, 
			                                SQLEscape(cache.CacheOwner), cache.OwnerID, SQLEscape(cache.PlacedBy), 
			                                cache.Difficulty.ToString(CultureInfo.InvariantCulture), cache.Terrain.ToString(CultureInfo.InvariantCulture), SQLEscape(cache.Country), 
			                                SQLEscape(cache.State),cache.TypeOfCache.ToString(), 
			                                SQLEscape(cache.ShortDesc), SQLEscape(cache.LongDesc),
			                                SQLEscape(cache.Hint), cache.Container, cache.Archived.ToString(),
			                                cache.Available.ToString(), SQLEscape(cache.Notes), cache.CheckNotes.ToString(),
			                              	cache.CorrectedLat.ToString(CultureInfo.InvariantCulture), cache.CorrectedLon.ToString(CultureInfo.InvariantCulture), 
				                        	cache.DNF, cache.FTF, cache.User1, cache.User2, cache.User3, cache.User4);
			}
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
		
		public DateTime GetLastFound(Geocache cache)
		{
			IDbConnection conn = OpenConnection();
			IDbCommand command = conn.CreateCommand();
			command.CommandText = String.Format(LAST_FOUND, cache.Name);
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
		
		public DateTime GetLastDNFByYou(Geocache cache, String ownerID)
		{
			IDbConnection conn = OpenConnection();
			IDbCommand command = conn.CreateCommand();
			command.CommandText = String.Format(LAST_DNF_BY_YOU, cache.Name, ownerID);
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
				log.LogID = rdr.GetString(6);
				log.LogKey = rdr.GetString(7);
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
			GetCacheList(sql, pts);
			return pts;
		}
		
		private void GetCacheList(String sql, List<Geocache> pts)
		{
			BuildHasChildrenList();
			IDbConnection conn =  OpenConnection ();
			IDbCommand command = conn.CreateCommand();
			command.CommandText = sql;
			IDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				Geocache cache = BuildCache(reader, pts);
				if (cache != null && !pts.Contains(cache))
					pts.Add(cache);
			}
			CloseConnection (ref reader, ref command, ref conn);			
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
		
		private Geocache BuildCache(IDataReader reader, List<Geocache> caches)
		{
			
			Geocache cache = new Geocache();
			
			// DBVER 0 Fields
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
			
			// From this point, the fields have been added in later DB Schema versions
			// Must check to see if they aren't DBNULL before getting the value
			Object val = reader.GetValue(26);
			if (val is string)
				cache.Notes = reader.GetString(26);
			val = reader.GetValue(27);
			if (val is string)
				cache.CheckNotes = Boolean.Parse(val as string);
			else
				cache.CheckNotes = false;
			val = reader.GetValue(28);
			if (val is string)
				cache.CorrectedLat = Double.Parse(val as string, CultureInfo.InvariantCulture);
			val = reader.GetValue(29);
			if (val is string)
				cache.CorrectedLon = Double.Parse(val as string, CultureInfo.InvariantCulture);
			val = reader.GetValue(30);
			if (val is string)
				cache.DNF = Boolean.Parse(val as string);
			val = reader.GetValue(31);
			if (val is string)
				cache.FTF = Boolean.Parse(val as string);
			val = reader.GetValue(32);
			if (val is string)
					cache.User1 = val as string;
			val = reader.GetValue(33);
			if (val is string)
					cache.User2 = val as string;
			val = reader.GetValue(34);
			if (val is string)
					cache.User3 = val as string;
			val = reader.GetValue(35);
			if (val is string)
					cache.User4 = val as string;
				
				
				
			// Preprocessed fields from other DB tables... These queries were run first for performance reason
			cache.Children = m_hasChildrenList.Contains(cache.Name);
			cache.HasFinal = m_hasFinalList.Contains(cache.Name);
			
			// Calculated properties
			cache.Distance = Utilities.calculateDistance(m_lat, cache.Lat, m_lon, cache.Lon);			
			
			if (this.ReadCache != null)
			{
				if (caches.Contains(cache) || !DoNonDBFilter(cache))
				{
					this.ReadCache(this, new ReadCacheArgs(null));
					return null;
				}
				this.ReadCache(this, new ReadCacheArgs(cache));
			}
			return cache;
			
		}
		
		private void BuildHasChildrenList()
		{
			m_hasChildrenList.Clear();
			IDbConnection conn = OpenConnection();
			IDbCommand cmd = conn.CreateCommand();	
			cmd.CommandText = HASCHILDREN_LIST;
			IDataReader rdr = cmd.ExecuteReader();
			while (rdr.Read())
			{
				m_hasChildrenList.Add(rdr.GetString(0));
			}
			
			m_hasFinalList.Clear();
			cmd = conn.CreateCommand();
			cmd.CommandText = HASFINAL_LIST;
			rdr = cmd.ExecuteReader();
			while (rdr.Read())
			{
				m_hasFinalList.Add(rdr.GetString(0));
			}
			CloseConnection(ref rdr, ref cmd, ref conn);
		}
		
		private string DoPrefilter()
		{
			if (m_filter == null)
				return null;
			
			System.Text.StringBuilder preFilterList = new System.Text.StringBuilder();
			bool atLeastOne = false;
			if (m_filter.Contains(FilterList.KEY_INCATTRS) || m_filter.Contains(FilterList.KEY_EXCATTRS)
			    || m_filter.Contains(FilterList.KEY_CHILDREN))
			{
				StringBuilder refineList = new StringBuilder();
				preFilterList.Append(" AND GEOCACHE.name IN (");
				if (m_filter.Contains(FilterList.KEY_INCATTRS))
				{
					refineList = BuildInclusionList ("SELECT DISTINCT cachename FROM ATTRIBUTES where inc='True' AND value == ",
					                    FilterList.KEY_INCATTRS,
					                    refineList,
					                    out atLeastOne);
				}
				if (m_filter.Contains(FilterList.KEY_EXCATTRS))
				{
					refineList = BuildInclusionList ("SELECT DISTINCT cachename FROM ATTRIBUTES where inc='False' AND value ==", 
					                    FilterList.KEY_EXCATTRS,
					                    refineList,
					                    out atLeastOne);
					
				}

				string childTypes = m_filter.GetCriteria(FilterList.KEY_CHILDREN) as string;
				if (!String.IsNullOrEmpty(childTypes))
				{
					atLeastOne = true;
					IDbConnection conn = OpenConnection();
					IDbCommand cmd = conn.CreateCommand();
	
					cmd.CommandText = String.Format(HAS_WPT_FILT,childTypes);
					if (refineList.Length > 0)
					{
						cmd.CommandText += "AND WAYPOINT.parent IN (";
						cmd.CommandText += refineList.ToString();
						cmd.CommandText += ")";
					}
					refineList = new StringBuilder();
					
					IDataReader rdr = cmd.ExecuteReader();
					bool firstDone = false;
					while (rdr.Read())
					{
						if (!firstDone)
							firstDone = true;
						else
							refineList.Append(",");
						refineList.Append("'");
						refineList.Append(rdr.GetString(0));
						refineList.Append("'");
					}
				}
				
				preFilterList.Append(refineList);
				preFilterList.Append(")");
				
			}
			if (m_filter.Contains(FilterList.KEY_INCNOATTRS) || m_filter.Contains(FilterList.KEY_EXCNOATTRS) ||
			    m_filter.Contains(FilterList.KEY_NOCHILDREN))
			{
				preFilterList.Append(" AND GEOCACHE.name NOT IN (");
				if (m_filter.Contains(FilterList.KEY_INCNOATTRS))
				{
					BuildExclusionList ("SELECT DISTINCT cachename FROM ATTRIBUTES where inc='True' AND value IN (", 
					                    FilterList.KEY_INCNOATTRS,
					                    preFilterList,
					                    out atLeastOne);
					
				}
				if (m_filter.Contains(FilterList.KEY_EXCNOATTRS))
				{
					BuildExclusionList ("SELECT DISTINCT cachename FROM ATTRIBUTES where inc='False' AND value IN (", 
					                    FilterList.KEY_EXCNOATTRS,
					                    preFilterList,
					                    out atLeastOne);
					
				}
				string childTypes = m_filter.GetCriteria(FilterList.KEY_NOCHILDREN) as string;
				if (!String.IsNullOrEmpty(childTypes))
				{
					atLeastOne = true;
					IDbConnection conn = OpenConnection();
					IDbCommand cmd = conn.CreateCommand();	
					cmd.CommandText = String.Format(HAS_WPT_FILT,childTypes);
					IDataReader rdr = cmd.ExecuteReader();
					bool firstDone = false;
					while (rdr.Read())
					{
						if (!firstDone)
							firstDone = true;
						else
							preFilterList.Append(",");
						preFilterList.Append("'");
						preFilterList.Append(rdr.GetString(0));
						preFilterList.Append("'");
					}
				}
				preFilterList.Append(")");
			}
			if (atLeastOne)
				return preFilterList.ToString();
			return null;
		}
		
		private StringBuilder BuildInclusionList (String sql, String key, 
		                                 System.Text.StringBuilder preFilterList, 
		                                 out bool atLeastOne)
		{
				atLeastOne = true;
				StringBuilder refineList = new StringBuilder(preFilterList.ToString());
				List<String> incAttrs = m_filter.GetCriteria(key) as List<String>;
				IEnumerator<String> ct = incAttrs.GetEnumerator();
				bool firstDone = false;
				IDbConnection conn = OpenConnection();
				while (ct.MoveNext())
				{	
					
					IDbCommand cmd = conn.CreateCommand();
					string cmdTxt = sql + "'" + ct.Current + "'";
					if (refineList.Length > 0)
					{
						cmdTxt += " AND cachename IN (";
						cmdTxt += refineList;
						cmdTxt += ")";
						
					}
					if (!firstDone)
						firstDone = true;
					cmd.CommandText = cmdTxt;
					IDataReader rdr = cmd.ExecuteReader();
					refineList.Remove(0, refineList.Length);
					firstDone = false;
					while (rdr.Read())
					{
						if (!firstDone)
							firstDone = true;
						else
							refineList.Append(",");
						refineList.Append("'");
						refineList.Append(rdr.GetString(0));
						refineList.Append("'");
					}
					if (refineList.Length == 0)
						refineList.Append("NULL");
					rdr.Dispose();
					cmd.Dispose();
				}
				conn.Close();
				return refineList;
		}
		
		private void BuildExclusionList (String sql, String key, 
		                                 System.Text.StringBuilder preFilterList, 
		                                 out bool atLeastOne)
		{
				atLeastOne = true;
				System.Text.StringBuilder builder = new System.Text.StringBuilder();
				
				List<String> incAttrs = m_filter.GetCriteria(key) as List<String>;
				IEnumerator<String> ct = incAttrs.GetEnumerator();
				builder.Append(sql);
				bool firstDone = false;
				while (ct.MoveNext())
				{
					if (!firstDone)
						firstDone = true;
					else
						builder.Append(",");
					builder.Append("'");
					builder.Append(ct.Current);
					builder.Append("'");
				}
				builder.Append(")");
				
				IDbConnection conn = OpenConnection();
				IDbCommand cmd = conn.CreateCommand();	
				cmd.CommandText = builder.ToString();
				IDataReader rdr = cmd.ExecuteReader();
				firstDone = false;
				while (rdr.Read())
				{
					if (!firstDone)
						firstDone = true;
					else
						preFilterList.Append(",");
					preFilterList.Append("'");
					preFilterList.Append(rdr.GetString(0));
					preFilterList.Append("'");
				}
				CloseConnection(ref rdr, ref cmd, ref conn);
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
				if (m_filter.Contains(FilterList.KEY_INFO_DAYS))
				{
					int days = (int) m_filter.GetCriteria(FilterList.KEY_INFO_DAYS);
					DateTime dt = DateTime.Now.Subtract(new TimeSpan(days, 0,0,0));
					if (cache.Updated <= dt)
						return false;						
				}
				if (m_filter.Contains(FilterList.KEY_INFO_NDAYS))
				{
					int days = (int) m_filter.GetCriteria(FilterList.KEY_INFO_NDAYS);
					DateTime dt = DateTime.Now.Subtract(new TimeSpan(days, 0,0,0));
					if (cache.Updated >= dt)
						return false;						
				}
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
				if (m_filter.Contains(FilterList.KEY_DIST))
				{
					double lat = m_lat;
					double lon = m_lon;
					if (m_filter.Contains(FilterList.KEY_DIST_LAT))
						lat = (double) m_filter.GetCriteria(FilterList.KEY_DIST_LAT);
					if (m_filter.Contains(FilterList.KEY_DIST_LON))
						lon = (double) m_filter.GetCriteria(FilterList.KEY_DIST_LON);
					
					double limit = (double) m_filter.GetCriteria(FilterList.KEY_DIST);
					double dist = Utilities.calculateDistance(cache.Lat, lat, cache.Lon, lon);
					string op = m_filter.GetCriteria(FilterList.KEY_DIST_OP) as String;
					if (op == "<=")
						if (dist > limit)
							return false;
					if (op == ">=")
						if (dist < limit)
							return false;
					if (op == "==")
						if (dist != limit)
							return false;
				}
			}
			return true;
		}
		
		public void ClearLogs(List<String> caches)
		{
			IDbCommand cmd = m_conn.CreateCommand();
			StringBuilder builder = new StringBuilder();
			IEnumerator<String> ct = caches.GetEnumerator();
			bool firstDone = false;
			while (ct.MoveNext())
			{
				if (!firstDone)
					firstDone = true;
				else
					builder.Append(",");
				builder.Append("'");
				builder.Append(ct.Current);
				builder.Append("'");
			}
			cmd.CommandText = String.Format(DELETE_LOGS, builder.ToString());
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
			if (m_conn == null)
				throw new Exception("DB NOT OPEN");
			IDbCommand cmd = m_conn.CreateCommand();
			String insert = String.Format(ADD_LOG, cachename, log.LogDate.ToString("o"), SQLEscape(log.LoggedBy),
			                                SQLEscape(log.LogMessage), SQLEscape(log.LogStatus), log.FinderID, 
			                                log.Encoded.ToString(), log.LogID, log.LogKey);
			String update = String.Format(UPDATE_LOG, cachename, log.LogDate.ToString("o"), SQLEscape(log.LoggedBy),
			                                SQLEscape(log.LogMessage), SQLEscape(log.LogStatus), log.FinderID, 
			                                log.Encoded.ToString(), log.LogID, log.LogKey);
			InsertOrUpdate(update, insert, cmd);
		}
		
		public void ClearTBs(List<String> caches)
		{
			IDbCommand cmd = m_conn.CreateCommand();
			StringBuilder builder = new StringBuilder();
			IEnumerator<String> ct = caches.GetEnumerator();
			bool firstDone = false;
			while (ct.MoveNext())
			{
				if (!firstDone)
					firstDone = true;
				else
					builder.Append(",");
				builder.Append("'");
				builder.Append(ct.Current);
				builder.Append("'");
			}
			cmd.CommandText = String.Format(DELETE_TBS, builder.ToString());
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
				log.LogID = rdr.GetString(6);
				log.LogKey = rdr.GetString(7);
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
		
		public void ClearAttributes(List<String> caches)
		{
			IDbCommand cmd = m_conn.CreateCommand();
			StringBuilder builder = new StringBuilder();
			IEnumerator<String> ct = caches.GetEnumerator();
			bool firstDone = false;
			while (ct.MoveNext())
			{
				if (!firstDone)
					firstDone = true;
				else
					builder.Append(",");
				builder.Append("'");
				builder.Append(ct.Current);
				builder.Append("'");
			}
			cmd.CommandText = String.Format(DELETE_ATTRIBUTES, builder.ToString());
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			cmd = null;
		}
		
		public void AddAttribute(CacheAttribute attr, String name)
		{
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = String.Format(ADD_ATTRIBUTE, name, attr.ID, attr.Include.ToString(), attr.AttrValue);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			cmd = null;
		}
		
		public List<CacheAttribute> GetAttributes(String name)
		{
			List<CacheAttribute> list = new List<CacheAttribute>();
			IDbConnection conn = OpenConnection();
			IDbCommand cmd = conn.CreateCommand();	
			cmd.CommandText = String.Format(GET_ATTRIBUTES, name);
			IDataReader rdr = cmd.ExecuteReader();
			while (rdr.Read())
			{
				CacheAttribute attr = new CacheAttribute();
				attr.ID = rdr.GetString(0);
				string val = rdr.GetString(1);
				attr.Include = bool.Parse(val);
				attr.AttrValue = rdr.GetString(2);
				list.Add(attr);
			}
			CloseConnection(ref rdr, ref cmd, ref conn);
			return list;
		}
		
		public List<string> GetIncAttributes(String name)
		{
			List<string> list = new List<string>();
			IDbConnection conn = OpenConnection();
			IDbCommand cmd = conn.CreateCommand();	
			cmd.CommandText = String.Format(GET_ATTRIBUTES, name);
			IDataReader rdr = cmd.ExecuteReader();
			while (rdr.Read())
			{
				string val = rdr.GetString(1);
				bool include = bool.Parse(val);
				if (include)
				{
					list.Add(rdr.GetString(2));
				}
			}
			CloseConnection(ref rdr, ref cmd, ref conn);
			return list;
		}
		
		public List<string> GetExcAttributes(String name)
		{
			List<string> list = new List<string>();
			IDbConnection conn = OpenConnection();
			IDbCommand cmd = conn.CreateCommand();	
			cmd.CommandText = String.Format(GET_ATTRIBUTES, name);
			IDataReader rdr = cmd.ExecuteReader();
			while (rdr.Read())
			{
				string val = rdr.GetString(1);
				bool include = bool.Parse(val);
				if (!include)
				{
					list.Add(rdr.GetString(2));
				}
			}
			CloseConnection(ref rdr, ref cmd, ref conn);
			return list;
		}
		
		public static String SQLEscape(String unescapedString)
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
				if (ver < 5)
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
			IDbTransaction trans = conn.BeginTransaction();
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
			if (ver <= 1)
			{
				cmd.CommandText = UPGRADE_GEOCACHE_V1_V2;
				cmd.ExecuteNonQuery();
			}
			if (ver <= 2)
			{
				cmd.CommandText = UPGRADE_GEOCACHE_V2_V3;
				cmd.ExecuteNonQuery();
				cmd.CommandText = CREATE_ATTRS_TABLE;
				cmd.ExecuteNonQuery();
			}
			if (ver <= 3)
			{
				cmd.CommandText = UPGRADE_GEOCACHE_V3_V4A;
				cmd.ExecuteNonQuery();
				cmd.CommandText = UPGRADE_GEOCACHE_V3_V4B;
				cmd.ExecuteNonQuery();
			}
			cmd.CommandText = UPGRADE_GEOCACHE_V4_V5A;
			cmd.ExecuteNonQuery();
			cmd.CommandText = UPGRADE_GEOCACHE_V4_V5B;
			cmd.ExecuteNonQuery();
			cmd.CommandText = UPGRADE_GEOCACHE_V4_V5C;
			cmd.ExecuteNonQuery();
			cmd.CommandText = UPGRADE_GEOCACHE_V4_V5D;
			cmd.ExecuteNonQuery();
			cmd.CommandText = UPGRADE_GEOCACHE_V4_V5E;
			cmd.ExecuteNonQuery();
			cmd.CommandText = UPGRADE_GEOCACHE_V4_V5F;
			cmd.ExecuteNonQuery();
			// Rename old logs table
			cmd.CommandText = UPGRADE_GEOCACHE_V4_V5G;
			cmd.ExecuteNonQuery();
			trans.Commit();		
			trans = conn.BeginTransaction();
			// Create new logs table
			cmd.CommandText = CREATE_LOGS_TABLE;
			cmd.ExecuteNonQuery();
			// Copy to new logs table
		/*	cmd.CommandText = UPGRADE_GEOCACHE_V4_V5H;
			IDataReader rdr = cmd.ExecuteReader();
			Random rand = new Random();			
			List<CacheLog> oldLogs = new List<CacheLog>();
			while (rdr.Read())
			{
				System.Console.WriteLine("READING LOGS");
				CacheLog log = new CacheLog();
				log.CacheCode = rdr.GetString(0);
				log.LogDate = DateTime.Parse(rdr.GetString(1));
				log.LoggedBy = rdr.GetString(2);
				log.LogMessage = rdr.GetString(3);
				log.LogStatus = rdr.GetString(4);
				log.FinderID = rdr.GetString(5);
				String encoded = rdr.GetString(6);
				log.Encoded = Boolean.Parse(encoded);
				object val = rdr.GetValue(7);
				if (val is string)
					log.LogID = val as string;			
				if (String.IsNullOrEmpty(log.LogID))
					log.LogID = rand.Next(5000).ToString();	
				oldLogs.Add(log);
			}	
			
			rdr.Close();
			rdr.Dispose();
			
				
			// Add logs back to new table
			foreach(CacheLog log in oldLogs)
			{
				System.Console.WriteLine("Adding logs");
				AddLog(log.CacheCode, log);
			}
			
			
			*/

			
			// Drop oldlogs table
			cmd.CommandText = UPGRADE_GEOCACHE_V4_V5I;
			cmd.ExecuteNonQuery();	
			
			cmd.CommandText = SET_DB_VER;
			cmd.ExecuteNonQuery();
			cmd.Dispose();
			trans.Commit();
			conn.Close();
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
			cmd.CommandText = CREATE_ATTRS_TABLE;
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
