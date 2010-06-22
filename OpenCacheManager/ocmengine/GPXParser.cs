/*
 Copyright 2009 Kyle Campbell
 Licensed under the Apache License, Version 2.0 (the "License"); 
 you may not use this file except in compliance with the License. 
 You may obtain a copy of the License at 
 
 		http://www.apache.org/licenses/LICENSE-2.0 
 
 Unless required by applicable law or agreed to in writing, software distributed under the License 
 is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or 
 implied. See the License for the specific language governing permissions and limitations under the License. 
*/
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using ocmengine;

namespace ocmengine
{
	public class ParseEventArgs:EventArgs
	{
		private string m_message;
		
		public string Message
		{
			get { return m_message;}
		}
		
		public ParseEventArgs(String message):base()
		{
			m_message = message;
		}
	}
	
	public class GPXParser
	{
		private CacheStore m_store = null;
		public delegate void ParseEventHandler(object sender, EventArgs args);
		public event ParseEventHandler ParseWaypoint;
		public event ParseEventHandler Complete;

		public string m_ownid = "";
		public string CacheOwner
		{
			set {m_ownid = value;}
		}
		
		private DateTime gpx_date;
		
		private Boolean m_cancel = false;
		public bool Cancel
		{
			set { m_cancel = true;}
		}
		
		
		public int preParse(FileStream fs)
		{
			XmlReader rdr = XmlReader.Create(fs);
			int count = 0;
			while (rdr.Read())
			{
				if (rdr.Name == "wpt" && rdr.IsStartElement())
					count++;
			}
			rdr.Close();
			return count;
		}
				
		public void parseGPXFile(FileStream fs, CacheStore store)
		{			
			m_store = store;
		 	System.Data.IDbTransaction trans =m_store.StartUpdate();
			XmlReader reader = XmlReader.Create(fs);
			while (reader.Read())
			{
				if (m_cancel)
				{
					m_store.CancelUpdate(trans);
					return;
				}
				switch (reader.NodeType)
				{
					case XmlNodeType.Element:
						if (reader.Name == "time")
							gpx_date = reader.ReadElementContentAsDateTime();
						if (reader.Name == "wpt")
						{
							Waypoint pt = processWaypoint(reader);
							pt.Updated = gpx_date;
							m_store.AddWaypoint(pt);							
						}
						break;
					case XmlNodeType.EndElement:
						break;
				}
			}
			reader.Close();
			m_store.EndUpdate(trans);
			this.Complete(this, EventArgs.Empty);
		}
		
		
		
		private Waypoint processWaypoint(XmlReader reader)
		{
			Waypoint newPoint = new Waypoint();
		
			String lat = reader.GetAttribute("lat");
			String lon = reader.GetAttribute("lon");
			
			newPoint.Lat = float.Parse(lat);
			newPoint.Lon = float.Parse(lon);
			
			bool breakLoop = false;
			
			while (reader.Read() && !breakLoop)
			{
				switch (reader.NodeType)
				{
					case XmlNodeType.Element:					
						processWptElement(ref newPoint, reader);
						break;
					case XmlNodeType.EndElement:
						if (reader.Name == "wpt")
							breakLoop = true;
						break;
				}
			}
			return newPoint;
		}
		
		private void processWptElement(ref Waypoint pt, XmlReader reader)
		{
			if (reader.Name == "name")
			{
				pt.Name = reader.ReadElementContentAsString();
				this.ParseWaypoint(this, new ParseEventArgs(String.Format("Processing Waypoint {0}", pt.Name)));
				pt.Parent = "GC" +  pt.Name.Substring(2);
			}
			else if (reader.Name == "url")
			{
				String url = reader.ReadElementContentAsString().Trim();
				if (!String.IsNullOrEmpty(url))
					pt.URL = new Uri(url);
			}
			else if (reader.Name == "desc")
			{
				pt.Desc = reader.ReadElementContentAsString();
			}
			else if (reader.Name == "time")
			{
				pt.Time = reader.ReadElementContentAsDateTime();
			}
			else if (reader.Name == "link")
			{
				pt.URL = new Uri(reader.GetAttribute("href"));
				//pt.URLName = reader.ReadElementContentAsString();
			}
			else if (reader.Name == "urlname")
			{
				pt.URLName = reader.ReadElementContentAsString();
			}
			else if (reader.Name == "sym")
			{
				pt.Symbol = reader.ReadElementContentAsString();
			}
			else if (reader.Name == "type")
			{
				pt.Type = reader.ReadElementContentAsString();
				if (pt.Type.StartsWith("Geocache") || pt.Type.StartsWith("TerraCache"))
				    pt = Geocache.convertFromWaypoint(pt);
			}
			else if (pt is Geocache)
			{
				parseGeocacheElement(ref pt, reader);
			}
		}
		
		private void parseGeocacheElement(ref Waypoint pt, XmlReader reader)
		{
			Geocache cache = pt as Geocache;
			m_store.ClearTBs(pt.Name);
			if (reader.NamespaceURI.StartsWith("http://www.groundspeak.com/cache"))
			{
				cache = ParseGroundSpeakCache (reader, ref cache);
			}
			else if (reader.NamespaceURI.StartsWith("http://www.TerraCaching.com"))
			{
				if (reader.LocalName == "terracache")
				{
					cache.CacheID = reader.GetAttribute("id");
					cache.Available = true;
					cache.Archived = false;
				}
				else if (reader.LocalName == "name")
				{
					cache.CacheName = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "description")
				{
					cache.LongDesc = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "style")
				{
					ParseCacheType(reader.ReadElementContentAsString(), ref cache);
				}
				else if (reader.LocalName == "owner")
				{
					cache.OwnerID = reader.GetAttribute("id");
					cache.PlacedBy = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "hint")
				{
					cache.Hint = reader.ReadElementContentAsString();
					cache.Hint = cache.Hint.Replace("&gt;", ">");
					cache.Hint = cache.Hint.Replace("&lt;", "<");
					cache.Hint = cache.Hint.Replace("&amp;", "&");
				}
				else if (reader.LocalName == "tps_points")
				{
					cache.ShortDesc += "<b>TPS Points: </b>";
					cache.ShortDesc += reader.ReadElementContentAsString();
					cache.ShortDesc += "<br>";
				}
				else if (reader.LocalName == "mce_score")
				{
					cache.ShortDesc += "<b>MCE Score: </b>";
					cache.ShortDesc += reader.ReadElementContentAsString();
					cache.ShortDesc += "<br>";
				}
				else if (reader.LocalName == "physical_challenge")
				{
					cache.ShortDesc += "<b>Physical Challenge: </b>";
					cache.ShortDesc += reader.ReadElementContentAsString();
					cache.ShortDesc += "<br>";
				}
				else if (reader.LocalName == "mental_challenge")
				{
					cache.ShortDesc += "<b>Mental Challenge: </b>";
					cache.ShortDesc += reader.ReadElementContentAsString();
					cache.ShortDesc += "<br>";
				}
				else if (reader.LocalName == "camo_challenge")
				{
					cache.ShortDesc += "<b>Cammo Challenge: </b>";
					cache.ShortDesc += reader.ReadElementContentAsString();
					cache.ShortDesc += "<hr noshade>";
				}
				else if (reader.LocalName == "size")
				{
					int size = reader.ReadElementContentAsInt();
					switch(size)
					{
						case 1:
							cache.Container = "Large";
							break;
						case 2:
							cache.Container = "Regular";
							break;
						case 3:
							cache.Container = "Small";
							break;
						case 4:
							cache.Container = "Micro";
							break;
						case 5: 
							cache.Container = "Micro";
							break;
					}
				}
				else if (reader.LocalName == "logs" && !reader.IsEmptyElement)
				{
					parseVCacheLogs(ref cache, reader);
				}
			}
		}
		
		private Geocache ParseGroundSpeakCache (XmlReader reader, ref Geocache cache)
		{
			if (reader.LocalName == "cache")
				{
					cache.Available = Boolean.Parse(reader.GetAttribute("available"));
					cache.Archived = Boolean.Parse(reader.GetAttribute("archived"));
					cache.CacheID = reader.GetAttribute("id");
				}
				else if (reader.LocalName == "name")
				{
					cache.CacheName = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "placed_by")
				{
					cache.PlacedBy = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "owner")
				{
					cache.OwnerID = reader.GetAttribute("id");
					cache.CacheOwner = reader.ReadElementContentAsString();					
				}
				else if (reader.LocalName == "type")
				{
					ParseCacheType(reader.ReadElementContentAsString(), ref cache);
				}
				else if (reader.LocalName == "difficulty")
				{
					cache.Difficulty = reader.ReadElementContentAsFloat();
				}
				else if (reader.LocalName == "terrain")
				{
					cache.Terrain = reader.ReadElementContentAsFloat();
				}
				else if (reader.LocalName == "short_description")
				{
					cache.ShortDesc = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "long_description")
				{
					cache.LongDesc = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "encoded_hints")
				{
					cache.Hint = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "container")
				{
					cache.Container = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "logs" && !reader.IsEmptyElement)
				{
					parseCacheLogs(ref cache, reader);
				}
				else if (reader.LocalName == "travelbugs" && !reader.IsEmptyElement)
				{
					ParseTravelBugs(ref cache, reader);
				}
				else if (reader.LocalName == "country")
				{
					cache.Country = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "state")
				{
					cache.State = reader.ReadElementContentAsString();
				}
			return cache;
		}
		
		private void ParseTravelBugs(ref Geocache cache, XmlReader reader)
		{
			m_store.ClearTBs(cache.Name);
			while (reader.Read())
			{
				if (reader.LocalName == "travelbug")
					parseTravelBug(ref cache, reader);
				if (reader.LocalName == "travelbugs")
					return;
			}
		}
	
		
		private void parseTravelBug(ref Geocache cache, XmlReader reader)
		{
			TravelBug bug = new TravelBug();
			bug.ID = reader.GetAttribute("id");
			bug.Ref = reader.GetAttribute("ref");
			while (reader.Read())
			{
				if (reader.LocalName == "travelbug")
				{
						m_store.AddTB(cache.Name, bug);
						return;
				}
				if (reader.LocalName == "name")
				{
					bug.Name = reader.ReadElementContentAsString();
				}
			}			
		}
		
		private void parseCacheLogs(ref Geocache cache, XmlReader reader)
		{
			m_store.ClearLogs(cache.Name);
			while (reader.Read())
			{
				if (reader.LocalName == "log")
				{
					parseCacheLog(ref cache, reader);
				}
				if (reader.LocalName == "logs")
				{
					return;
				}
			}
		}
		
		private void parseCacheLog(ref Geocache cache, XmlReader reader)
		{
			CacheLog log = new CacheLog();
			bool breakLoop = false;
			while (reader.Read() && !breakLoop)
			{
				if (reader.LocalName == "date")
				{
					log.LogDate = reader.ReadElementContentAsDateTime();
				}
				else if (reader.LocalName == "type")
				{
					log.LogStatus = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "finder")
				{
					log.FinderID = reader.GetAttribute("id");
					log.LoggedBy = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "text")
				{
					log.Encoded = Boolean.Parse(reader.GetAttribute("encoded"));
					log.LogMessage = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "log")
				{
					breakLoop = true;
				}
			}
			m_store.AddLog(cache.Name, log);			
		}
		
		private void parseVCacheLogs(ref Geocache cache, XmlReader reader)
		{
			m_store.ClearLogs(cache.Name);
			while (reader.Read())
			{
				if (reader.LocalName == "log")
				{
					parseVCacheLog(ref cache, reader);
				}
				if (reader.LocalName == "logs")
				{
					return;
				}
			}
		}
		
		
		private void parseVCacheLog(ref Geocache cache, XmlReader reader)
		{
			CacheLog log = new CacheLog();
			bool breakLoop = false;
			while (reader.Read() && !breakLoop)
			{
				if (reader.LocalName == "date")
				{
					log.LogDate = reader.ReadElementContentAsDateTime();
				}
				else if (reader.LocalName == "type")
				{
					log.LogStatus = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "user")
				{
					log.FinderID = reader.GetAttribute("id");
					log.LoggedBy = reader.ReadElementContentAsString();
					if (log.FinderID == m_ownid && log.LogStatus == "find")
					{
						cache.Symbol = "Geocache Found";
					}
				}
				else if (reader.LocalName == "entry")
				{
					log.Encoded = false;
					log.LogMessage = reader.ReadElementContentAsString();
					log.LogMessage = log.LogMessage.Replace("&gt;", ">");
					log.LogMessage = log.LogMessage.Replace("&lt;", "<");
					log.LogMessage = log.LogMessage.Replace("&amp;", "&");
				}
				else if (reader.LocalName == "log")
				{
					breakLoop = true;
				}
			}
			m_store.AddLog(cache.Name, log);			
		}
		
		
		private void ParseCacheType(String type, ref Geocache cache)
		{
			if ((type == "Unknown Cache") || (type == "Other"))
				cache.TypeOfCache = Geocache.CacheType.MYSTERY;
			else if ((type == "Traditional Cache") || (type == "Classic"))
				cache.TypeOfCache = Geocache.CacheType.TRADITIONAL;
			else if ((type == "Multi-cache") || (type == "Offset"))
				cache.TypeOfCache = Geocache.CacheType.MULTI;
			else if (type == "Letterbox Hybrid")
				cache.TypeOfCache = Geocache.CacheType.LETTERBOX;
			else if (type == "Earthcache")
				cache.TypeOfCache = Geocache.CacheType.EARTH;
			else if (type =="Wherigo Cache")
				cache.TypeOfCache = Geocache.CacheType.WHERIGO;
			else if (type == "Webcam Cache")
				cache.TypeOfCache = Geocache.CacheType.WEBCAM;
			else if (type == "Cache In Trash Out Event")
				cache.TypeOfCache = Geocache.CacheType.CITO;
			else if (type == "GPS Adventures Exhibit")
				cache.TypeOfCache = Geocache.CacheType.MAZE;
			else if (type == "Mega-Event Cache")
				cache.TypeOfCache = Geocache.CacheType.MEGAEVENT;
			else if ((type == "Event Cache") || (type == "Event"))
				cache.TypeOfCache = Geocache.CacheType.EVENT;
			else if ((type == "Virtual Cache") || (type == "Virtual"))
			    cache.TypeOfCache = Geocache.CacheType.VIRTUAL;
			else if (type == "Project APE Cache")
				cache.TypeOfCache = Geocache.CacheType.APE;
			else if (type == "Lost and Found Event Cache")
				cache.TypeOfCache = Geocache.CacheType.EVENT;
			else
				cache.TypeOfCache = Geocache.CacheType.OTHER;
				
		}
	}
}
