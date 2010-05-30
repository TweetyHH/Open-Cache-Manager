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

		public GPXWriter ()
		{
		}

		public void WriteGPXFile (String name, List<Geocache> caches)
		{
			XmlWriter writer = new XmlTextWriter (name, System.Text.Encoding.UTF8);
			try {
				writer.WriteStartElement ("gpx", NS_GPX);
				writer.WriteElementString ("name", NS_GPX, "Cache Listing from OCM");
				writer.WriteElementString ("desc", NS_GPX, "Cache Listing from OCM");
				writer.WriteElementString ("author", NS_GPX, "Kyle Campbell");
				writer.WriteElementString ("email", NS_GPX, "kmcamp_ott@yahoo.com");
				writer.WriteElementString ("url", NS_GPX, "http://sourceforge.net/projects/opencachemanage/");
				writer.WriteElementString ("urlname", NS_GPX, "Sourceforge Link");
				writer.WriteElementString ("time", NS_GPX, System.DateTime.Now.ToString (XSD_DT));
				foreach (Geocache cache in caches) {
					this.WriteWaypoint(this, new WriteEventArgs(String.Format("Writing {0}", cache.Name)));
					cache.WriteToGPX (writer);
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

		public void toGPX (Waypoint pt)
		{
			
		}
	}
}
