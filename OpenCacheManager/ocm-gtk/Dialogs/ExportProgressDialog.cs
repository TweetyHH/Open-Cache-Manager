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
using System.Collections.Generic;
using ocmengine;

namespace ocmgtk
{


	public partial class ExportProgressDialog : Gtk.Dialog
	{
		double total = 100;
		double count = 0;
		GPXWriter m_writer =null;
		
		public ExportProgressDialog (GPXWriter writer)
		{
			this.Build ();
			writer.WriteWaypoint += HandleWriterWriteWaypoint;
			writer.Complete += HandleWriterComplete;
			m_writer = writer;
		}
		
		public void Start(String filename, List<Geocache> list)
		{
			total = list.Count;
			m_writer.WriteGPXFile(filename, list);
			this.Show();
		}

		void HandleWriterComplete (object sender, EventArgs args)
		{
			this.Hide();
			this.Dispose();
		}

		void HandleWriterWriteWaypoint (object sender, EventArgs args)
		{
			count++;
			writeProgress.Fraction = count/total;
			while (Gtk.Application.EventsPending ())
				Gtk.Application.RunIteration (false);
			
		}
	}
}
