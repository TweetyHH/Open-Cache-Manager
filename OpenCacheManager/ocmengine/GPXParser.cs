
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace ocmengine
{
	
	
	public class GPXParser
	{
		
		public GPXParser()
		{
		}
		
		public Dictionary<string, Waypoint> parseGPXFile(FileStream fs)
		{
			XmlReader reader = XmlReader.Create(fs);
			while (reader.Read())
			{
				switch (reader.NodeType)
				{
					case XmlNodeType.Element:
						if (reader.Name == "wpt")
							processWaypoint(reader);
						break;
					case XmlNodeType.EndElement:
						break;
				}
			}
			
			return null;
		}
		
		private Waypoint processWaypoint(XmlReader reader)
		{
					
			System.Console.WriteLine("processing waypoint...");
			System.Console.WriteLine("-----------------------");
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
			System.Console.WriteLine(newPoint.ToString());
			System.Console.WriteLine("-----------------------");
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
				pt.URL = new Uri(reader.ReadElementContentAsString());
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
		}
		
	}
}
