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
using System.IO;
using ocmengine;

namespace ocmgtk
{


	public partial class ProgressDialog : Gtk.Dialog
	{

		private GPXParser m_parser;
		int processCount=0;
		public ProgressDialog (GPXParser parser)
		{
			this.Build ();
			parser.ParseWaypoint += HandleParserParseWaypoint;
			parser.Complete += HandleParserComplete;
			m_parser = parser;
			this.ShowAll();
		}

		void HandleParserComplete (object sender, EventArgs args)
		{
			this.Hide();
			this.Dispose();
		}
		
		public void Start(FileStream fs, CacheStore store)
		{
			this.Show();
			m_parser.parseGPXFile(fs, store);
		}

		void HandleParserParseWaypoint (object sender, EventArgs args)
		{
			this.progressbar6.Text = (args as ParseEventArgs).Message;
			this.progressbar6.Pulse();
			processCount++;
			this.label1.Text = "Processed: " + processCount;
			while (Gtk.Application.EventsPending())
                 Gtk.Application.RunIteration(false);
		}
	}
}
