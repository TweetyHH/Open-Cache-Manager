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
	public enum WaypointNameMode{CODE,NAME};
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
		
		public int GetNextGUID()
		{
			return guidStart++;
		}
		
		bool m_cancel = false;
		public bool Cancel
		{
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
