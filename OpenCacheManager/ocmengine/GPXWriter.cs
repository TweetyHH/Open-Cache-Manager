// 
//  Copyright 2010  campbelk
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
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace ocmengine
{
	public enum WaypointNameMode{CODE,NAME,SHORTCODE};
	public enum WaypointDescMode{DESC,CODESIZEANDHINT,CODESIZETYPE};
	
	public class WriteEventArgs:EventArgs
	{
		private string m_message;
		
		public string Message
		{
			get { return m_message;}
		}
		
		public WriteEventArgs(String message):base()
		{
			m_message = message;
		}
	}
	
	public class GPXWriter
	{
		public const string NS_GPX = "http://www.topografix.com/GPX/1/0";
		public const string NS_CACHE = "http://www.groundspeak.com/cache/1/0";
		public const string XSD_DT = "yyyy-MM-ddTHH:mm:ss.fffzzz";
		
		public event WriteEventHandler WriteWaypoint;
		public event WriteEventHandler Complete;
		public delegate void WriteEventHandler(object sender, EventArgs args);
		
		public int guidStart = 9000000;
		
		bool m_UseOCMPtTypes = false;
		public Boolean UseOCMPtTypes
		{
		 	set { m_UseOCMPtTypes = value;}
			get { return m_UseOCMPtTypes;}
		}
		
		bool m_includeChildren = true;
		public Boolean IncludeChildWaypoints
		{
			get { return m_includeChildren;}
			set { m_includeChildren = value;}
		}
		
		bool m_garminHTML = false;
		public Boolean GarminHTML
		{
			get { return m_garminHTML;}
			set { m_garminHTML = value;}
		}
		
		bool m_isMyFinds = false;
		public Boolean IsMyFinds
		{
			get { return m_isMyFinds;}
			set { m_isMyFinds = value;
				if (value)
				{
					m_includeChildren = false;
				}
			}
		}
		
		string m_findsOwner = "ocm";
		public string MyFindsOwner
		{
			get { return m_findsOwner;}
			set { m_findsOwner = value;}
		}
		
		public int GetNextGUID()
		{
			return guidStart++;
		}
		
		bool m_cancel = false;
		public bool Cancel
		{
			get { return m_cancel; }
			set {m_cancel = true;}
		}
		
		private int m_Limit = -1;
		public int Limit
		{
			set { m_Limit = value;}
		}
		
		private int m_LogLimit = -1;
		public int LogLimit
		{
			get { return m_LogLimit;}
			set { m_LogLimit = value;}
		}
		
		private bool m_isFullInfo = true;
		public bool IncludeGroundSpeakExtensions
		{
			set { m_isFullInfo = value;}
			get { return m_isFullInfo;}
		}
		
		WaypointNameMode m_namemode = WaypointNameMode.CODE;
		public WaypointNameMode NameMode
		{
			get { return m_namemode;}
			set { m_namemode = value;}
		}
		
		WaypointDescMode m_descmode = WaypointDescMode.DESC;
		public WaypointDescMode DescriptionMode
		{
			get { return m_descmode;}
			set { m_descmode = value;}
		}
		
		private Dictionary<string,string> m_mappings;
		public Dictionary<string,string> Mappings
		{
			get{return m_mappings;}
		}
		
		private int m_Count = 0;

		public void WriteGPXFile (String name, List<Geocache> caches, Dictionary<string,string> waypointmappings)
		{
			FileStream stream = new System.IO.FileStream(name, FileMode.Create, FileAccess.Write, FileShare.Write, 655356);
			m_mappings = waypointmappings;
			XmlTextWriter writer = new XmlTextWriter (stream, System.Text.Encoding.UTF8);
			//Pretty-print the document
			writer.Formatting = Formatting.Indented;
			writer.Indentation = 1;
			writer.IndentChar = '\t';
			
			try {
				writer.WriteStartElement ("gpx", NS_GPX);
				writer.WriteAttributeString("creator", "OCM");
				if (IsMyFinds)
					writer.WriteElementString("name", NS_GPX, "My Finds Pocket Query");
				else
					writer.WriteElementString ("name", NS_GPX, "Cache Listing from OCM");
				writer.WriteElementString ("desc", NS_GPX, "Cache Listing from OCM");
				writer.WriteElementString ("author", NS_GPX, "Open Cache Manager");
				writer.WriteElementString ("email", NS_GPX, "kmcamp_ott@yahoo.com");
				writer.WriteElementString ("url", NS_GPX, "http://sourceforge.net/projects/opencachemanage/");
				writer.WriteElementString ("urlname", NS_GPX, "Sourceforge Link");
				writer.WriteElementString ("time", NS_GPX, System.DateTime.Now.ToString (XSD_DT));
				foreach (Geocache cache in caches) {
					if (m_cancel || ((m_Count >= m_Limit) && (m_Limit != -1)))
						return;
					m_Count++;
					this.WriteWaypoint(this, new WriteEventArgs(String.Format("Writing {0}", cache.Name)));
					cache.WriteToGPX (writer, this);
				}
				writer.WriteEndElement ();
				this.Complete(this, new WriteEventArgs("Done"));
			} catch (Exception e) {
				throw e;
			} finally {
				writer.Flush ();
				writer.Close ();
			}
		}
	}
}
