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

namespace ocmgtk
{


	public partial class SendWaypointsDialog : Gtk.Dialog
	{
		GPSBabelWriter writer = new GPSBabelWriter ();
		double total = 0;
		double count = 0;

		public SendWaypointsDialog ()
		{
			this.Build ();
			writer.StartSend += HandleWriterStartSend;
			writer.Complete += HandleWriterComplete;
			writer.WriteWaypoint += HandleWriterWriteWaypoint;
		}

		public void Start (List<Geocache> caches, IGPSConfig config)
		{
			try {
				if (config.GetCacheLimit () == -1)
					total = caches.Count + 1;
				else
					total = config.GetCacheLimit () + 1;
				writer.Limit = config.GetCacheLimit ();
				writer.BabelFile = config.GetOutputFile();
				writer.BabelFormat = config.GetBabelFormat ();
				writer.WriteToGPS (caches);
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
			writeProgress.Fraction = 1;
			writeProgress.Text = Catalog.GetString("Complete");
			this.infoLabel.Markup = String.Format(Catalog.GetString("<i>Send Complete:{0} geocaches transferred</i>"), count);
			closeButton.Show ();
			buttonCancel.Hide ();
		}

		void HandleWriterStartSend (object sender, EventArgs args)
		{
			this.infoLabel.Markup = Catalog.GetString("<i>Sending Geocaches to Device</i>");
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
		}
	}
}
