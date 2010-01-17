
using System;
using System.Xml;
using System.Collections.Generic;

namespace ocmengine
{
	
	
	public class Geocache : Waypoint
	{
		enum CacheType { TRADITIONAL, 
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
			REVERSE};
		
		
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
		
				
		
		
		/// <summary>
		/// Creates a new Geocache object. Only public for testing purposes.
		/// </summary>
		public Geocache()
		{
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
			return string.Format("[Geocache: Name={0}, Lat={1}, Lon={2}, URL={3}, URLName={4}, Des={5}, Symbol={6}, Time={7}, Type={8}]", Name, Lat, Lon, URL, URLName, Desc, Symbol, Time, Type);
		}

	}
}
