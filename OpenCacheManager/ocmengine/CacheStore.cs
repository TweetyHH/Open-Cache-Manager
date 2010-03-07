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
using Mono.Data.SqliteClient;
using System.Collections.Generic;

namespace ocmengine
{
	
	
	public class CacheStore
	{
		const string SQL_CONNECT = "URI=file:/home/campbelk/.ocm/data.db,version=3";
		const string INSERT_WPT = "REPLACE INTO WAYPOINT (name,lat,lon,url,urlname,desc,symbol,type,time, parent) VALUES (" +
							"'{0}',{1},{2},'{3}','{4}','{5}','{6}','{7}','{8}','{9}')";
	
		const string GET_WPTS = "SELECT name, lat, lon, url, urlname, desc, symbol, type, time, parent FROM WAYPOINT";
		const string DELETE_LOGS = "DELETE FROM LOGS where cache='{0}'";
		const string DELETE_TBS = "DELETE FROM TBUGS where cache='{0}'";
		const string ADD_LOG = "INSERT INTO LOGS (cache, date, loggedby, message, status, finderID, encoded) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
		const string ADD_TB = "INSERT INTO TBUGS(cache, id, ref, name) VALUES('{0}','{1}','{2}','{3}')";
		const string WHERE_PARENT =" WHERE parent='{0}'";
		const string GET_LOGS = "SELECT date, loggedby, message, status, finderID, encoded FROM LOGS WHERE cache='{0}' ORDER BY date DESC";
		const string INSERT_GC = "REPLACE INTO GEOCACHE (name, fullname, id, owner, ownerID, placedby, difficulty, terrain, country, state, type, shortdesc, longdesc, hint, container, archived, available)"
			+ " VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')";
		const string GET_GC = "SELECT  WAYPOINT.name, WAYPOINT.lat, WAYPOINT.lon, WAYPOINT.url, WAYPOINT.urlname, WAYPOINT.desc, WAYPOINT.symbol, WAYPOINT.type, WAYPOINT.time,"
			+ "GEOCACHE.fullname, GEOCACHE.id, GEOCACHE.owner, GEOCACHE.ownerID, GEOCACHE.placedby, GEOCACHE.difficulty, GEOCACHE.terrain, GEOCACHE.country, GEOCACHE.state," +
				"GEOCACHE.type, GEOCACHE.shortdesc, GEOCACHE.longdesc, GEOCACHE.hint, GEOCACHE.container, GEOCACHE.archived, GEOCACHE.available" +
				" FROM WAYPOINT, GEOCACHE WHERE WAYPOINT.name = GEOCACHE.name";
		const string COUNT_GC = "SELECT COUNT(name) from GEOCACHE";
		const string COUNT_WPT = "SELECT COUNT(name) from WAYPOINT";
		const string DELETE_WPT = "DELETE FROM WAYPOINT WHERE name='{0}' or parent='{0}'";
		const string DELETE_GC = "DELETE FROM GEOCACHE WHERE name='{0}'";
	   	
		
		private IDbConnection m_conn = null;
		
		public int CacheCount
		{
			get { 	
				IDbConnection conn = (IDbConnection) new SqliteConnection(SQL_CONNECT);
				conn.Open();
				IDbCommand command = conn.CreateCommand();
				command.CommandText = COUNT_GC;
				object val = command.ExecuteScalar();
				int count = int.Parse(val.ToString());
				conn.Close();
				return count;
			}
		}
		
		public int WaypointCount
		{
			get { 	
				IDbConnection conn = (IDbConnection) new SqliteConnection(SQL_CONNECT);
				conn.Open();
				IDbCommand command = conn.CreateCommand();
				command.CommandText = COUNT_WPT;
				int count = (int) command.ExecuteScalar();
				conn.Close();
				return count;
			}
		}
		
		public CacheStore()
		{
		}
		
		
		public void AddWaypoint(Waypoint point)
		{
			if (point is Geocache)
				UpdateCache(point as Geocache);
			UpdateWaypoint(point);
		}
		
		
		
		public IEnumerator<Geocache> getCacheEnumerator()
		{
			List<Geocache> caches =  GetCacheList(GET_GC);
			return caches.GetEnumerator();
		}
		
		
		public List<Waypoint> GetChildren(String cachecode)
		{
			String whereClause = String.Format(WHERE_PARENT, cachecode);
			return GetWayPointList(GET_WPTS + whereClause);
		}
		
		public void UpdateWaypoint(Waypoint pt)
		{
			if (m_conn == null)
				throw new Exception("DB NOT OPEN");
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = String.Format(INSERT_WPT, SQLEscape(pt.Name), pt.Lat, pt.Lon, pt.URL, 
			                                SQLEscape(pt.URLName), SQLEscape(pt.Desc), pt.Symbol, pt.Type,
			                                pt.Time.ToString("o"), pt.Parent);
			cmd.ExecuteNonQuery();
		}
		
		public void UpdateCache(Geocache cache)
		{
			if (m_conn == null)
				throw new Exception("DB NOT OPEN");
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = String.Format(INSERT_GC, cache.Name, SQLEscape(cache.CacheName), cache.CacheID, 
			                                SQLEscape(cache.CacheOwner), cache.OwnerID, SQLEscape(cache.PlacedBy), 
			                                cache.Difficulty, cache.Terrain, SQLEscape(cache.Country), 
			                                SQLEscape(cache.State),cache.TypeOfCache.ToString(), 
			                                SQLEscape(cache.ShortDesc), SQLEscape(cache.LongDesc),
			                                SQLEscape(cache.Hint), cache.Container, cache.Archived.ToString(),
			                                cache.Available.ToString());
			System.Console.WriteLine(cmd.CommandText);
			cmd.ExecuteNonQuery();
		}
		
		private List<Waypoint> GetWayPointList(String sql)
		{
			List<Waypoint> pts = new List<Waypoint>();
			IDbConnection conn = (IDbConnection) new SqliteConnection(SQL_CONNECT);
			conn.Open();
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
			IDbConnection conn = (IDbConnection) new SqliteConnection(SQL_CONNECT);
			conn.Open();
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
			pt.Lat = reader.GetFloat(1);
			pt.Lon = reader.GetFloat(2);
			string url = reader.GetString(3);
			if (null != url)
				pt.URL = new Uri(url);
			pt.URLName = reader.GetString(4);
			pt.Desc = reader.GetString(5);
			pt.Symbol = reader.GetString(6);
			pt.Type = reader.GetString(7);
			pt.Time = DateTime.Parse(reader.GetString(8));
			pt.Parent = reader.GetString(9);
			return pt;
		}
		
		private Geocache BuildCache(IDataReader reader)
		{
			Geocache cache = new Geocache();
			cache.Name = reader.GetString(0);
			cache.Lat = reader.GetFloat(1);
			cache.Lon = reader.GetFloat(2);
			String url = reader.GetString(3);
			if (url != null)
				cache.URL = new Uri(url);
			cache.URLName = reader.GetString(4);
			cache.Desc = reader.GetString(5);
			cache.Symbol = reader.GetString(6);
			cache.Type = reader.GetString(7);
			cache.Time = reader.GetDateTime(8);	
			cache.CacheName = reader.GetString(9);
			cache.CacheID = reader.GetString(10);
			cache.CacheOwner = reader.GetString(11);
			cache.OwnerID = reader.GetString(12);
			cache.PlacedBy = reader.GetString(13);
			cache.Difficulty = reader.GetFloat(14);
			cache.Terrain = reader.GetFloat(15);
			cache.Country = reader.GetString(16);
			cache.State = reader.GetString(17);
			cache.TypeOfCache = (Geocache.CacheType) Enum.Parse(typeof (Geocache.CacheType), reader.GetString(18));
			cache.ShortDesc = reader.GetString(19);
			cache.LongDesc = reader.GetString(20);
			cache.Hint = reader.GetString(21);
			cache.Container = reader.GetString(22);
			cache.Archived = reader.GetBoolean(23);
			cache.Available = reader.GetBoolean(24);
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
		
		public void StartUpdate()
		{
			if (m_conn == null)
			{
				m_conn = (IDbConnection) new SqliteConnection(SQL_CONNECT);
				m_conn.Open();
			}
		}
		
		public void EndUpdate()
		{
			m_conn.Close();
			m_conn = null;
		}
		
		public List<CacheLog> GetCacheLogs(String cachename)
		{
			List<CacheLog> logs = new List<CacheLog>();
			StartUpdate();
			IDbCommand cmd =  m_conn.CreateCommand();
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
				log.Encoded = rdr.GetBoolean(5);
				logs.Add(log);
			}
			return logs;			
		}
		
		private static String SQLEscape(String unescapedString)
		{
			return unescapedString.Replace("'", "''");
		}
	}		
}
