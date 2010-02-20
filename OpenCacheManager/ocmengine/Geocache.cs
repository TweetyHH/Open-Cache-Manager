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
using System.Xml;
using System.Collections.Generic;

namespace ocmengine
{
	
	
	public class Geocache : Waypoint
	{
		public enum CacheType { TRADITIONAL, 
			MULTI, 
			APE, 
			MYSTERY, 
			LETTERBOX,
			EVENT, 
			WHERIGO,
			MEGAEVENT,
			CITO,
			EARTH,
			MAZE,
			VIRTUAL,
			WEBCAM,
			REVERSE,
			FOUND,
			OTHER};
		
		
		private string m_cacheName;
		private string m_cacheOwner;
		private string m_placedBy;
		private float m_difficulty;
		private float m_terrain;
		private string m_country;
		private string m_state;
		private CacheType m_cachetype;
		private string m_shortdesc;
		private string m_longdesc;
		private string m_hint;
		private List<CacheLog> m_logs;
		private List<TravelBug> m_travelbugs;
		private string m_container;
		private bool m_available;
		private bool m_archived;
		private string m_cacheID;
		private string m_ownerID;
		
		public string CacheName
		{
			get { return m_cacheName; }
			set {m_cacheName = value;}
		}
		
		public string CacheOwner
		{
			get {return m_cacheOwner; }
			set {m_cacheOwner = value;}
		}
		
		public string PlacedBy
		{
			get {return m_placedBy;}
			set {m_placedBy = value;}
		}
		
		public float Difficulty
		{
			get {return m_difficulty;}
			set {m_difficulty = value;}
		}
		
		public float Terrain
		{
			get { return m_terrain;}
			set {m_terrain = value;}
		}
		
		public string Country
		{
			get {return m_country;}
			set {m_country = value;}
		}
		
		public string State
		{
			get {return m_state;}
			set { m_state = value;}
		}
		
		public CacheType TypeOfCache
		{
			get { return m_cachetype;}
			set {m_cachetype = value;}
		}
		
		public string ShortDesc
		{
			get {return m_shortdesc;}
			set {m_shortdesc = value;}
		}
		
		public string LongDesc
		{
			get { return m_longdesc;}
			set {m_longdesc = value;}
		}
		
		public string Hint
		{
			get {return m_hint;}
			set {m_hint = value;}
		}
		
		public string Container
		{
			get { return m_container;}
			set {m_container = value;}
		}
		
		public List<CacheLog> CacheLogs
		{
			get {return m_logs;}
			set {m_logs = value;}
		}
		
		public bool Archived
		{
			get {return m_archived;}
			set {m_archived = value;}
		}
		
		public bool Available
		{
			get {return m_available;}
			set {m_available = value;}
		}
		
		public bool Found
		{
			get { return  this.Symbol.Equals("Geocache Found");}
		}
		
		public string OwnerID
		{
			get { return m_ownerID;}
			set { m_ownerID = value; }
		}
		
		public string CacheID
		{
			get { return m_cacheID;}
			set { m_cacheID = value; }
		}
		
		public List<TravelBug> TravelBugs
		{
			get { return m_travelbugs;}
		}
					
		
		/// <summary>
		/// Creates a new Geocache object. Only public for testing purposes.
		/// </summary>
		public Geocache()
		{
			m_travelbugs = new List<TravelBug>();
		}
		
		public static Geocache convertFromWaypoint(Waypoint original)
		{
			Geocache cache = new Geocache();
			cache.Name = original.Name;
			cache.Time = original.Time;
			cache.Lat = original.Lat;
			cache.Lon = original.Lon;
			cache.Desc = original.Desc;
			cache.Symbol = original.Symbol;
			cache.URL = original.URL;
			cache.URLName = original.URLName;
			cache.Type = original.Type;
			return cache;
		}
		
		public override string ToString ()
		{
			String val = base.ToString();
			val += "\n Geocache Info:\n";
			val += "Cache Name:";
			val += this.CacheName;
			val += "\nDescription:" ;
			val += this.LongDesc;
			return val;
		}
		
		public void AddLog(CacheLog log)
		{
			if (m_logs == null)
			{
				m_logs = new List<CacheLog>();
			}
			m_logs.Add(log);
		}
		
		internal override void WriteWPTDetails (XmlWriter writer)
		{
			base.WriteWPTDetails (writer);
			writer.WriteStartElement("cache", GPXWriter.NS_CACHE);
			writer.WriteAttributeString("id", CacheID);
			writer.WriteAttributeString("available", Available.ToString());
			writer.WriteAttributeString("archived", Archived.ToString());
			writer.WriteElementString("name", CacheName);
			writer.WriteElementString("placed_by", PlacedBy);
			writer.WriteStartElement("owner");
			writer.WriteAttributeString("id", OwnerID);
			writer.WriteString(CacheOwner);
			writer.WriteEndElement();
			writer.WriteElementString("type", GetCTypeString(TypeOfCache));
			writer.WriteElementString("container", Container);
			writer.WriteElementString("difficulty", Difficulty.ToString("0.0"));
			writer.WriteElementString("terrain", Difficulty.ToString("0.0"));
			writer.WriteElementString("country", Country);
			writer.WriteElementString("state", State);
			writer.WriteElementString("short_description", ShortDesc);
			writer.WriteElementString("long_description", LongDesc);
			writer.WriteElementString("encoded_hints", Hint);
			writer.WriteStartElement("logs");
			foreach(CacheLog log in m_logs)
			{
				log.WriteToGPX(writer);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("travelbugs");
			foreach(TravelBug bug in m_travelbugs)
			{
				bug.WriteToGPX(writer);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
		}
		
		
		private string GetCTypeString(CacheType ct)
		{
			switch (ct)
			{
			case CacheType.APE:
				return "Project APE Cache";
			case CacheType.CITO:
				return "Cache In Trash Out Event";
			case CacheType.EARTH:
				return "Earthcache";
			case CacheType.EVENT:
				return "Event Cache";
			case CacheType.LETTERBOX:
				return "Letterbox Hybrid";
			case CacheType.MAZE:
				return "GPS Adventures Exhibit";
			case CacheType.MEGAEVENT:
				return "Mega-Event Cache";
			case CacheType.MULTI:
				return "Multi-cache";
			case CacheType.MYSTERY:
				return "Unknown Cache";
			case CacheType.REVERSE:
				return "Locationless Cache";
			case CacheType.TRADITIONAL:
				return "Traditional Cache";
			case CacheType.VIRTUAL:
				return "Virtual Cache";
			case CacheType.WEBCAM:
				return "Webcam Cache";
			case CacheType.WHERIGO:
				return "Wherigo Cache";
			default:
				throw new Exception("UNHANDLED CACHE TYPE");
			}
		}
	}
}
