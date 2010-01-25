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
using System.Xml;
using ocmengine;

namespace ocmengine
{
	
	
	public class GPXParser
	{
		
		public GPXParser()
		{
		}
		
		public void parseGPXFile(FileStream fs, CacheStore store)
		{			
			XmlReader reader = XmlReader.Create(fs);
			while (reader.Read())
			{
				switch (reader.NodeType)
				{
					case XmlNodeType.Element:
						if (reader.Name == "wpt")
						{
							Waypoint pt = processWaypoint(reader);
							store.AddWaypoint(pt);							
						}
						break;
					case XmlNodeType.EndElement:
						break;
				}
			}
			
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
				if (pt.Type.StartsWith("Geocache"))
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
			if (reader.NamespaceURI.StartsWith("http://www.groundspeak.com/cache"))
			{
				if (reader.LocalName == "cache")
				{
					cache.Available = Boolean.Parse(reader.GetAttribute("available"));
					cache.Archived = Boolean.Parse(reader.GetAttribute("archived"));
				}
				if (reader.LocalName == "name")
				{
					cache.CacheName = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "placed_by")
				{
					cache.PlacedBy = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "owner")
				{
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
				else if (reader.LocalName == "logs")
				{
					parseCacheLogs(ref cache, reader);
				}
				else if (reader.LocalName == "travelbug")
				{
					parseTravelBug(ref cache, reader);
				}
			}
		}
		
		private void parseTravelBug(ref Geocache cache, XmlReader reader)
		{
			while (reader.Read())
			{
				if (reader.LocalName == "travelbug")
				{
					return;
				}
			}
		}
		
		private void parseCacheLogs(ref Geocache cache, XmlReader reader)
		{
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
					log.LogStatus = CacheLog.Status.OTHER;
				}
				else if (reader.LocalName == "finder")
				{
					log.LoggedBy = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "text")
				{
					log.LogMessage = reader.ReadElementContentAsString();
				}
				else if (reader.LocalName == "log")
				{
					breakLoop = true;
				}
			}
			cache.AddLog(log);			
		}
		
		private void ParseCacheType(String type, ref Geocache cache)
		{
			if (type == "Unknown Cache")
				cache.TypeOfCache = Geocache.CacheType.MYSTERY;
			else if (type == "Traditional Cache")
				cache.TypeOfCache = Geocache.CacheType.TRADITIONAL;
			else if (type == "Multi-cache")
				cache.TypeOfCache = Geocache.CacheType.MULTI;
			else if (type == "Letterbox Hybrid")
				cache.TypeOfCache = Geocache.CacheType.LETTERBOX;
			else if (type == "Earthcache")
				cache.TypeOfCache = Geocache.CacheType.EARTH;
			else if (type =="Wherigo Cache")
				cache.TypeOfCache = Geocache.CacheType.WHERIGO;
			else if (type == "Webcam Cache")
				cache.TypeOfCache = Geocache.CacheType.WEBCAM;
			else
				cache.TypeOfCache = Geocache.CacheType.OTHER;
				
		}
		
		
	}
}
