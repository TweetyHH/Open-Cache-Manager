// 
//  Copyright 2010  Kyle Campbell
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
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace ocmengine
{


	public class GPSBabelWriter
	{
		private int m_Limit = -1;
		public int Limit {
			set { m_Limit = value; }
		}

		private string m_format = "garmin";
		public string BabelFormat {
			set { m_format = value; }
		}
		private string m_file = "usb:";
		public string BabelFile {
			set { m_file = value; }
		}

		public event WriteEventHandler WriteWaypoint;
		public event WriteEventHandler StartSend;
		public event WriteEventHandler Complete;
		public delegate void WriteEventHandler (object sender, EventArgs args);


		public void WriteToGPS (List<Geocache> cacheList)
		{
			GPXWriter writer = new GPXWriter ();
			if (m_Limit > 0)
				writer.Limit = m_Limit;
			if (m_format == "OCM_GPX") {
				WriteFullGPX (cacheList, writer);
				return;
			}
			
			writer.IncludeGroundSpeakExtensions = false;
			writer.Complete += HandleWriterComplete;
			String tempFile = Path.GetTempFileName ();
			writer.WriteWaypoint += HandleWriterWriteWaypoint;
			writer.WriteGPXFile (tempFile, cacheList);
			this.StartSend (this, new WriteEventArgs ("Sending Waypoints to GPS"));
			StringBuilder builder = new StringBuilder ();
			builder.Append ("gpsbabel -i gpx -f ");
			builder.Append (tempFile);
			builder.Append (" -o ");
			builder.Append (m_format);
			builder.Append (" -F ");
			builder.Append (m_file);
			Process babel = Process.Start (builder.ToString ());
			babel.WaitForExit ();
			if (babel.ExitCode != 0)
				throw new Exception ("Failed to send caches to GPS");
			this.Complete (this, new WriteEventArgs ("Complete"));
		}
		
		private void WriteFullGPX (List<Geocache> cacheList, GPXWriter writer)
		{
			writer.IncludeGroundSpeakExtensions = true;
				writer.Complete += HandleWriterComplete;
				writer.WriteWaypoint += HandleWriterWriteWaypoint;
				writer.WriteGPXFile (m_file, cacheList);
				this.Complete (this, new WriteEventArgs ("Complete"));
				return;
		}

		void HandleWriterWriteWaypoint (object sender, EventArgs args)
		{
			this.WriteWaypoint (this, args as WriteEventArgs);
		}

		void HandleWriterComplete (object sender, EventArgs args)
		{
			this.Complete (this, args as WriteEventArgs);
		}
	}
}
