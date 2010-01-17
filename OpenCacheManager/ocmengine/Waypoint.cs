
using System;

namespace ocmengine
{
	
	
	public class Waypoint
	{
		private float m_lat = 0;
		private float m_lon = 0;
		private DateTime m_time = DateTime.Now;
		private string m_name = "";
		private string m_desc = "";
		private Uri m_uri = new Uri("http://nowhere.com");
		private string m_urlname = "";
		private string m_symbol = "";
		private string m_type = "UNDEFINED";
		private bool m_isChild = false;
		
		public string Name
		{
			get { return m_name;}
			set { m_name = value;}
		}
		
		public float Lat
		{
			get { return m_lat;}
			set {m_lat = value;}
		}
		
		public float Lon
		{
			get { return m_lon;}
			set { m_lon = value;}
		}
		
		public Uri URL
		{
			get { return m_uri; }
			set { m_uri = value; }
		}
		
		public string URLName
		{
			get { return m_urlname; }
			set { m_urlname = value; }
		}
		
		public string Desc
		{
			get { return m_desc; }
			set { m_desc = value; }
		}
		
		public string Symbol
		{
			get {return m_symbol; }
			set {m_symbol = value;}
		}
		
		public DateTime Time
		{
			get {return m_time;}
			set {m_time = value;}
		}
		
		
		public string Type
		{
			get { return m_type;}
			set { m_type = value;}
		}
		
		
		public Waypoint()
		{
		}
		
		public override string ToString ()
		{
			return string.Format("[Waypoint: Name={0}, Lat={1}, Lon={2}, URL={3}, URLName={4}, Des={5}, Symbol={6}, Time={7}, Type={8}]", Name, Lat, Lon, URL, URLName, Desc, Symbol, Time, Type);
		}

		
	}
}
