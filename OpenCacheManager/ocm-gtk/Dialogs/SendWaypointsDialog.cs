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
using Mono.Unix;
using Gtk;

namespace ocmgtk
{


	public partial class SendWaypointsDialog : Gtk.Dialog
	{
		GPSBabelWriter writer = new GPSBabelWriter ();
		double total = 0;
		double count = 0;
		
		public bool noCancel = false;
		public bool done = false;

		public SendWaypointsDialog ()
		{
			this.Build ();
			writer.StartSend += HandleWriterStartSend;
			writer.Complete += HandleWriterComplete;
			writer.WriteWaypoint += HandleWriterWriteWaypoint;
		}

		public void Start (List<Geocache> caches, IGPSConfig config, Dictionary<string,string> wmappings)
		{
			try {
				if (config.GetCacheLimit () == -1)
					total = caches.Count + 1;
				else
					total = config.GetCacheLimit () + 1;
				writer.Limit = config.GetCacheLimit ();
				writer.BabelFile = config.GetOutputFile();
				writer.BabelFormat = config.GetBabelFormat ();
				writer.DescMode = config.GetDescMode();
				writer.NameMode = config.GetNameMode();
				writer.LogLimit = config.GetLogLimit();
				writer.IncludeAttributes = config.IncludeAttributes();
				writer.WriteToGPS (caches, wmappings);
				this.Show ();
			} catch (Exception e) {
				this.Hide ();
				UIMonitor.ShowException (e);
			}
		}

		void HandleWriterWriteWaypoint (object sender, EventArgs args)
		{
			count++;
			double fraction = (count) / total;
			writeProgress.Fraction = fraction;
			this.infoLabel.Markup = "<i>" + (args as WriteEventArgs).Message + "</i>";
			writeProgress.Text = fraction.ToString ("0%");
			while (Gtk.Application.EventsPending ())
				Gtk.Application.RunIteration (false);
		}

		void HandleWriterComplete (object sender, EventArgs args)
		{
			if ((args as WriteEventArgs).Message == "Cancelled")
			{
				this.Hide();
				return;
			}
			writeProgress.Fraction = 1;
			writeProgress.Text = Catalog.GetString("Complete");
			done = true;
			this.infoLabel.Markup = String.Format(Catalog.GetString("<i>Send Complete:{0} geocaches transferred</i>"), count);
			closeButton.Show ();
			buttonCancel.Hide ();
		}

		void HandleWriterStartSend (object sender, EventArgs args)
		{
			this.infoLabel.Markup = Catalog.GetString("<i>Sending Geocaches to Device</i>");
			noCancel = true;
			buttonCancel.Sensitive = false;
			while (Gtk.Application.EventsPending ())
				Gtk.Application.RunIteration (false);
		}
		
		protected virtual void OnCloseClick (object sender, System.EventArgs e)
		{
			this.Hide ();
			this.Dispose ();
		}

		protected virtual void OnCancelClick (object sender, System.EventArgs e)
		{
			Cancel ();
		}
		
		private bool Cancel ()
		{
			MessageDialog dlg = new MessageDialog(null, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel,
			                                      Catalog.GetString("Are you sure you want to cancel?"));
			if ((int) ResponseType.Ok == dlg.Run())
			{
				writer.Cancel();
				dlg.Hide();
				return true;
			}
			dlg.Hide();
			return false;
		}
		protected virtual void OnDelete (object o, Gtk.DeleteEventArgs args)
		{
			if (done)
				return;
			if (noCancel || !Cancel())
				args.RetVal = true;
		}
		
		
	}
}
