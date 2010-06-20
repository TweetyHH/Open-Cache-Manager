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

namespace ocmengine
{


	public partial class CacheStore
	{
		const string CREATE_CACHE_TABLE = "CREATE TABLE GEOCACHE (available TEXT, archived TEXT, container TEXT, hint TEXT, longdesc TEXT, shortdesc TEXT, type TEXT, state TEXT, country TEXT, terrain NUMERIC, difficulty NUMERIC, placedby TEXT, name TEXT PRIMARY KEY, fullname TEXT, id TEXT, owner TEXT, ownerID TEXT)";
		const string CREATE_LOGS_TABLE = "CREATE TABLE LOGS(cache TEXT, date text, loggedby TEXT, message TEXT, status TEXT, finderID TEXT, encoded TEXT)";
		const string CREATE_TABLE_TBUGS = "CREATE TABLE TBUGS (cache TEXT, id TEXT, ref TEXT, name TEXT)";
		const string CREATE_TABLE_WPTS = "CREATE TABLE WAYPOINT (lastUpdate TEXT, parent TEXT, symbol TEXT, time TEXT, type TEXT, desc TEXT, urlname TEXT, url TEXT, lon NUMERIC, lat NUMERIC, name TEXT PRIMARY KEY)";
		const string SQL_CONNECT = "Data Source=";
		const string INSERT_WPT = "INSERT INTO WAYPOINT (name,lat,lon,url,urlname,desc,symbol,type,time, parent, lastUpdate) VALUES ('{0}',{1},{2},'{3}','{4}','{5}','{6}','{7}','{8}','{9}', '{10}')";
		const string UPDATE_WPT = "UPDATE WAYPOINT SET lat='{1}', lon='{2}',url='{3}', urlname='{4}', desc='{5}', symbol='{6}', type='{7}', time='{8}', parent='{9}', lastUpdate='{10}' WHERE name='{0}'";
		const string WPT_EXISTS_CHECK = "SELECT * FROM WAYPOINT WHERE name='{0}'";
		const string GET_WPTS = "SELECT name, lat, lon, url, urlname, desc, symbol, type, time, parent, lastUpdate FROM WAYPOINT";
		const string DELETE_LOGS = "DELETE FROM LOGS where cache='{0}'";
		const string DELETE_TBS = "DELETE FROM TBUGS where cache='{0}'";
		const string ADD_LOG = "INSERT INTO LOGS (cache, date, loggedby, message, status, finderID, encoded) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
		const string ADD_TB = "INSERT INTO TBUGS(cache, id, ref, name) VALUES('{0}','{1}','{2}','{3}')";
		const string GET_TB = "SELECT id, ref, name FROM TBUGS WHERE cache='{0}'";
		const string WHERE_PARENT = " WHERE parent='{0}'";
		const string GET_LOGS = "SELECT date, loggedby, message, status, finderID, encoded FROM LOGS WHERE cache='{0}' ORDER BY date DESC";
		const string INSERT_GC = "INSERT INTO GEOCACHE (name, fullname, id, owner, ownerID, placedby, difficulty, terrain, country, state, type, shortdesc, longdesc, hint, container, archived, available)" + " VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')";
		const string UPDATE_GC = "UPDATE GEOCACHE SET fullname='{1}', id='{2}', owner='{3}', ownerID='{4}',  placedby='{5}', difficulty='{6}', terrain='{7}', country='{8}',state='{9}',type='{10}',shortdesc='{11}',longdesc='{12}',hint='{13}',container='{14}',archived='{15}',available='{16}' WHERE name='{0}'";
		const string GC_EXISTS_CHECK = "SELECT * FROM GEOCACHE WHERE name='{0}'";
		const string GET_GC = "SELECT  WAYPOINT.name, WAYPOINT.lat, WAYPOINT.lon, WAYPOINT.url, WAYPOINT.urlname, WAYPOINT.desc, WAYPOINT.symbol, WAYPOINT.type, WAYPOINT.time," + "GEOCACHE.fullname, GEOCACHE.id, GEOCACHE.owner, GEOCACHE.ownerID, GEOCACHE.placedby, GEOCACHE.difficulty, GEOCACHE.terrain, GEOCACHE.country, GEOCACHE.state," + "GEOCACHE.type, GEOCACHE.shortdesc, GEOCACHE.longdesc, GEOCACHE.hint, GEOCACHE.container, GEOCACHE.archived, GEOCACHE.available, WAYPOINT.lastUpdate" + " FROM WAYPOINT, GEOCACHE WHERE WAYPOINT.name = GEOCACHE.name";
		const string COUNT_GC = "SELECT COUNT(name) from GEOCACHE";
		const string COUNT_WPT = "SELECT COUNT(name) from WAYPOINT";
		const string FOUND = " WHERE SYMBOL='Geocache Found'";
		const string INACTIVE = " WHERE AVAILABLE='False'";
		const string DELETE_WPT = "DELETE FROM WAYPOINT WHERE name='{0}'";
		const string DELETE_GC = "DELETE FROM GEOCACHE WHERE name='{0}'";
		const string OR_PARENT = " OR parent='{0}'";
		const string SEPERATOR = ";";
	}
}