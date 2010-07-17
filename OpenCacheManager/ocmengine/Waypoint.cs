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
using System.Globalization;

namespace ocmengine
{
	
	
	public class Waypoint
	{
		private double m_lat = 0;
		private double m_lon = 0;
		private DateTime m_time = DateTime.Now;
		private string m_name = "";
		private string m_desc = "";
		private Uri m_uri = new Uri("http://www.geocaching.com");
		private string m_urlname = String.Empty;
		private string m_symbol = "Waypoint";
		private string m_type = "Waypoint";
		private string m_parent = null;
		private DateTime m_updated = DateTime.MinValue;
		
		public string Name
		{
			get { return m_name;}
			set { m_name = value;}
		}
		
		public double Lat
		{
			get { return m_lat;}
			set {m_lat = value;}
		}
		
		public double Lon
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
		
		public DateTime Updated
		{
			get {return m_updated;}
			set {m_updated = value;}
		}
		
		
		public string Type
		{
			get { return m_type;}
			set { m_type = value;}
		}
		
		public string Parent
		{
			get {return m_parent;}
			set {m_parent = value;}
		}
		
		
		public Waypoint()
		{
			
		}
		
		public void WriteToGPX(XmlWriter writer, GPXWriter gpx)
		{
			writer.WriteStartElement("wpt", GPXWriter.NS_GPX);
			WriteWPTDetails(writer, gpx);
			writer.WriteEndElement();
			IEnumerator<Waypoint> itr = Engine.getInstance().GetChildWaypoints(this.Name);
			while (itr.MoveNext())
			{
				itr.Current.WriteToGPX(writer, gpx);
			}                          
		}
		
		internal virtual void WriteWPTDetails(XmlWriter writer, GPXWriter gpx)
		{
			writer.WriteAttributeString("lat", this.Lat.ToString(CultureInfo.InvariantCulture));
			writer.WriteAttributeString("lon", this.Lon.ToString(CultureInfo.InvariantCulture));
			writer.WriteElementString("time", this.Time.ToString("o"));
			writer.WriteElementString("name", this.Name);
			writer.WriteElementString("desc", this.Desc);
			if (this.URL != null)
				writer.WriteElementString("url", this.URL.ToString());
			if (!String.IsNullOrEmpty(this.URLName))
				writer.WriteElementString("urlname", this.URLName);
			if (!gpx.UseOCMPtTypes)
				writer.WriteElementString("sym", this.Symbol);
			else
			{
					if (this.Symbol == "Trailhead")
						writer.WriteElementString("sym","Trail Head");
					else if (this.Symbol == "Question to Answer" || this.Symbol == "Stages of a Multicache")
						writer.WriteElementString("sym","Pin, Red");
					else if (this.Symbol == "Reference Point" || this.Symbol == "Other")
						writer.WriteElementString("sym","Pin, Green");
					else if (this.Symbol == "Final Location")
						writer.WriteElementString("sym","Pin, Blue");
					else if (this.Symbol == "TerraCache")
						writer.WriteElementString("sym", "Geocache");
					else 
						writer.WriteElementString("sym", this.Symbol);
				
					if (this is Geocache)
					{
						if (!(this as Geocache).Available)
							writer.WriteElementString("sym","Flag, Red");
					}
			}
			writer.WriteElementString("type", this.Type);
		}
		
		public override string ToString ()
		{
			return string.Format("[Waypoint: Name={0}, Lat={1}, Lon={2}, URL={3}, URLName={4}, Des={5}, Symbol={6}, Time={7}, Type={8}]", Name, Lat, Lon, URL, URLName, Desc, Symbol, Time, Type);
		}

		
	}
}
