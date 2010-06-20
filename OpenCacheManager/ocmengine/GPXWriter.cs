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
		
		private bool m_isFullInfo = true;
		public bool IncludeGroundSpeakExtensions
		{
			set { m_isFullInfo = value;}
		}
		
		private int m_Count = 0;

		public void WriteGPXFile (String name, List<Geocache> caches)
		{
			FileStream stream = new System.IO.FileStream(name, FileMode.Create, FileAccess.Write, FileShare.Write, 655356);
			XmlWriter writer = new XmlTextWriter (stream, System.Text.Encoding.UTF8);
			try {
				writer.WriteStartElement ("gpx", NS_GPX);
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
					cache.WriteToGPX (writer, m_isFullInfo);
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
