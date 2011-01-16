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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Text;
using Mono.Unix;

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
			MINE,
			OTHER,
			GENERIC};
		
		const string CACHE_PREFIX="groundspeak";
		
		private string m_cacheName = "";
		private string m_cacheOwner = "";
		private string m_placedBy = "";
		private float m_difficulty = 0;
		private float m_terrain= 0;
		private string m_country = "";
		private string m_state = "";
		private CacheType m_cachetype = Geocache.CacheType.GENERIC;
		private string m_shortdesc = "";
		private string m_longdesc = "";
		private string m_hint= "";
		private List<TravelBug> m_travelbugs = new List<TravelBug>();
		private string m_container = "";
		private bool m_available= false;
		private bool m_archived=false;
		private string m_cacheID = "";
		private string m_ownerID = "";
		private bool m_checkNotes = false;
		private bool m_children = false;
		private double m_correctedLat = -1;
		private double m_correctedLon = -1;
		
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
		
		private string m_notes = String.Empty;
		public string Notes
		{
			get { return m_notes;}
			set { m_notes = value;}
		}
		
		public bool CheckNotes
		{
			get { return m_checkNotes;}
			set { m_checkNotes = value;}
		}
		
		public bool Children
		{
			get { return m_children;}
			set { m_children = value;}
		}
		
		public bool HasCorrected
		{
			get
			{
				if (m_correctedLat != -1)
					return true;
				return false;
			}
			set 
			{
				if (value == false)
				{
					m_correctedLat = -1;
					m_correctedLon = -1;
				}
			}
		}
		
		private bool m_hasFinal = false;
		public bool HasFinal
		{
			get { return m_hasFinal;}
			set { m_hasFinal = value;}
		}
		
		public override double Lat {
			get {
				if (HasCorrected)
					return m_correctedLat;
				return base.Lat;
			}
			set {
				base.Lat = value;
			}
		}
		
		public override double Lon {
			get {
				if (HasCorrected)
					return m_correctedLon;
				return base.Lon;
			}
			set {
				base.Lon = value;
			}
		}
		
		public double OrigLat {
			get {
				return base.Lat;
			}
		}
		
		public double OrigLon {
			get {
				return base.Lon;
			}
		}

		
		public double CorrectedLat
		{
			get { return m_correctedLat;}
			set { m_correctedLat = value;}
		}
		
		public double CorrectedLon
		{
			get { return m_correctedLon;}
			set { m_correctedLon = value;}
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
			cache.Updated = original.Updated;
			cache.CacheName = original.Desc;
			cache.Available = true;
			cache.Archived = false;
			cache.TypeOfCache = Geocache.CacheType.GENERIC;
			cache.ShortDesc = "";
			if (cache.URL != null)
				cache.LongDesc = "<a href=\"" + cache.URL.ToString() + "\">View Online</a>";
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
		
		
		internal override void WriteWPTDetails (XmlWriter writer, GPXWriter gpx)
		{
			base.WriteWPTDetails (writer,gpx);
			if (!gpx.IncludeGroundSpeakExtensions)
				return;
			writer.WriteStartElement(CACHE_PREFIX, "cache", GPXWriter.NS_CACHE);
			if (Symbol == "TerraCache" || Symbol == "Default")
				writer.WriteAttributeString("id", gpx.GetNextGUID().ToString());
			else
				writer.WriteAttributeString("id", CacheID);
			writer.WriteAttributeString("available", Available.ToString());
			writer.WriteAttributeString("archived", Archived.ToString());
			// Temp until smart-tag like support
			if (HasCorrected)
				writer.WriteElementString(CACHE_PREFIX,"name", GPXWriter.NS_CACHE, "(*) " + CacheName);
			else
				writer.WriteElementString(CACHE_PREFIX,"name", GPXWriter.NS_CACHE, CacheName);
			writer.WriteElementString(CACHE_PREFIX,"placed_by", GPXWriter.NS_CACHE,  PlacedBy);
			writer.WriteStartElement(CACHE_PREFIX,"owner", GPXWriter.NS_CACHE);
			writer.WriteAttributeString("id", OwnerID);
			writer.WriteString(CacheOwner);
			writer.WriteEndElement();
			writer.WriteElementString(CACHE_PREFIX,"type", GPXWriter.NS_CACHE, GetCTypeString(TypeOfCache));
			writer.WriteElementString(CACHE_PREFIX,"container", GPXWriter.NS_CACHE, Container);
			writer.WriteElementString(CACHE_PREFIX,"difficulty", GPXWriter.NS_CACHE, Difficulty.ToString("0.#", CultureInfo.InvariantCulture));
			writer.WriteElementString(CACHE_PREFIX,"terrain", GPXWriter.NS_CACHE, Terrain.ToString("0.#", CultureInfo.InvariantCulture));
			writer.WriteElementString(CACHE_PREFIX,"country", GPXWriter.NS_CACHE,  Country);
			writer.WriteElementString(CACHE_PREFIX,"state", GPXWriter.NS_CACHE,  State);
			StringBuilder shortDescription = new StringBuilder();
			if (HasCorrected)
			{
				shortDescription.Append(Catalog.GetString("Original Coordinate:"));
				shortDescription.Append(Utilities.getCoordString(OrigLat, OrigLon));
				shortDescription.Append("<br/>");
			}
			if (gpx.WriteAttributes)
			{
				List <CacheAttribute> attrs = Engine.getInstance().Store.GetAttributes(this.Name);
				IEnumerator<CacheAttribute> attr = attrs.GetEnumerator();
				while (attr.MoveNext())
				{
					CacheAttribute curr = attr.Current;
					if (curr.Include)
					{
						shortDescription.Append(Catalog.GetString("Y:"));
					}
					else
					{
						shortDescription.Append(Catalog.GetString("N:"));
					}
					shortDescription.Append(curr.AttrValue);
					shortDescription.Append("<br/>");
				}
				if (attrs.Count > 0)
					shortDescription.Append("<hr noshade/>");
			}
			if (!String.IsNullOrEmpty(Notes))
			{
				shortDescription.Append(Notes);	
				shortDescription.Append("<hr noshade/>");
			}
			shortDescription.Append(ShortDesc);	
			writer.WriteStartElement(CACHE_PREFIX,"short_description", GPXWriter.NS_CACHE);
			writer.WriteAttributeString("html", "True");
			if (gpx.HTMLOutput == HTMLMode.GARMIN)
				writer.WriteCData(Utilities.HTMLtoGarmin(shortDescription.ToString()));
			else if (gpx.HTMLOutput == HTMLMode.PLAINTEXT)
				writer.WriteCData(Utilities.HTMLtoText(shortDescription.ToString()));
			else
				writer.WriteCData(shortDescription.ToString());
			writer.WriteEndElement();
			writer.WriteStartElement(CACHE_PREFIX,"long_description", GPXWriter.NS_CACHE);
			writer.WriteAttributeString("html", "True");
			if (gpx.HTMLOutput == HTMLMode.GARMIN)
				writer.WriteCData(Utilities.HTMLtoGarmin(LongDesc));
			else if (gpx.HTMLOutput == HTMLMode.PLAINTEXT)
				writer.WriteCData(Utilities.HTMLtoText(LongDesc));
			else
				writer.WriteCData(LongDesc);
			writer.WriteEndElement();
			writer.WriteStartElement(CACHE_PREFIX,"encoded_hints", GPXWriter.NS_CACHE);
			writer.WriteAttributeString("html", "True");
			if (gpx.HTMLOutput == HTMLMode.GARMIN || gpx.HTMLOutput == HTMLMode.PLAINTEXT)
				writer.WriteCData(Utilities.HTMLtoText(Hint));
			else
				writer.WriteCData(Hint);
			writer.WriteEndElement();
			writer.WriteStartElement(CACHE_PREFIX,"logs", GPXWriter.NS_CACHE);
			if (gpx.IsMyFinds)
			{
				CacheLog log = Engine.getInstance().Store.GetLastFindLogByYou(this, gpx.MyFindsOwner);
				if (log.LogStatus == "find")
					log.LogStatus = "Found it";
				log.WriteToGPX(writer);
			}
			else
			{				
				IEnumerator<CacheLog> itr = Engine.getInstance().GetLogs(this.Name);
				int iCount = 0;
				while (itr.MoveNext())
				{
					if ((iCount >= gpx.LogLimit) && (gpx.LogLimit != -1))
						break;
					else
						itr.Current.WriteToGPX(writer);
					iCount++;
			}
			}
			writer.WriteEndElement();
			writer.WriteStartElement(CACHE_PREFIX,"travelbugs", GPXWriter.NS_CACHE);
			IEnumerator<TravelBug> bugs = Engine.getInstance().Store.GetTravelBugs(this.Name).GetEnumerator();
			while (bugs.MoveNext())
			{
				bugs.Current.WriteToGPX(writer);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
		}
		
		
		public static string GetCTypeString(CacheType ct)
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
			case CacheType.GENERIC:
				return "Geocache";
			default:
				throw new Exception("UNHANDLED CACHE TYPE");
			}
		}
	}
}
