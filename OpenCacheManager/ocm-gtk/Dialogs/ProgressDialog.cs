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
		double m_total = 0;
		double m_progress = 0;
		public ProgressDialog (GPXParser parser, int total)
		{
			this.Build ();
			parser.ParseWaypoint += HandleParserParseWaypoint;
			parser.Complete += HandleParserComplete;
			m_parser = parser;
			m_total = total;
		}

		void HandleParserComplete (object sender, EventArgs args)
		{
			this.Hide ();
			String message = String.Format ("Import complete, {0} waypoints processed", m_progress);
			Gtk.MessageDialog dlg = new Gtk.MessageDialog (this, Gtk.DialogFlags.Modal, Gtk.MessageType.Info, Gtk.ButtonsType.Ok, message);
			dlg.Run ();
			dlg.Hide ();
			dlg.Dispose ();
			dlg.Destroy ();
			this.Dispose ();
			this.Destroy ();
		}


		public void Start (FileStream fs, CacheStore store)
		{
			this.Show ();
			try {
				m_progress = 0;
				m_parser.parseGPXFile (fs, store);
			} catch (Exception e) {
				this.Hide ();
				Gtk.MessageDialog error = new Gtk.MessageDialog (this, Gtk.DialogFlags.DestroyWithParent, Gtk.MessageType.Error, Gtk.ButtonsType.Ok, e.Message);
				error.Run ();
				error.Hide ();
				error.Dispose ();
				this.Dispose ();
			}
		}

		void HandleParserParseWaypoint (object sender, EventArgs args)
		{
			m_progress++;
			double fraction = (double)(m_progress / m_total);
			this.progressbar6.Text = (fraction).ToString ("0%");
			progressbar6.Fraction = fraction;
			this.waypointName.Text = "Waypoint: " + (args as ParseEventArgs).Message;
			while (Gtk.Application.EventsPending ())
				Gtk.Application.RunIteration (false);
		}

		protected virtual void OnCancel (object sender, System.EventArgs e)
		{
			DoCancel ();
		}

		private void DoCancel ()
		{
			m_parser.Cancel = true;
			this.Hide ();
			String message = String.Format ("Import cancelled, all changes reverted.", m_progress);
			Gtk.MessageDialog dlg = new Gtk.MessageDialog (this, Gtk.DialogFlags.Modal, Gtk.MessageType.Info, Gtk.ButtonsType.Ok, message);
			dlg.Run ();
			dlg.Hide ();
			dlg.Dispose ();
		}
		protected virtual void OnCancel (object o, Gtk.DeleteEventArgs args)
		{
			DoCancel ();
		}
		
		
	}
}
