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

namespace ocmgtk
{


	public partial class GPSConfiguration : Gtk.Dialog
	{
		GarminUSBWidget gusbwidet = new GarminUSBWidget ();
		GenericGPSWidget gpswidget = new GenericGPSWidget();


		protected virtual void OnGUSBToggle (object sender, System.EventArgs e)
		{
			if (gusbRadio.Active) {
				table1.Remove (gpxwidget);
				table1.Remove(gpswidget);
				table1.Add (gusbwidet);
				Gtk.Table.TableChild props = ((Gtk.Table.TableChild)(this.table1[this.gusbwidet]));
				props.TopAttach = 4;
				props.RightAttach = 2;
				props.BottomAttach = 5;
				gusbwidet.Show ();
			}
		}

		public IGPSConfig GPSConfig {
			get {
				if (gusbRadio.Active)
					return gusbwidet;
				else if (otherRadio.Active)
					return gpswidget;
				else
					return gpxwidget;
			}
		}

		public GPSConfiguration (Config config)
		{
			this.Build ();
			try
			{
				SavedGPSConf saved = new SavedGPSConf(config);
				if (saved.GetBabelFormat() == "garmin")
				{
					gusbRadio.Active = true;
					gusbwidet.SetCacheLimit(saved.GetCacheLimit());
				}
				else if (saved.GetBabelFormat()== "OCM_GPX")
				{
					gpxRadio.Active = true;
					gpxwidget.SetCacheLimit(saved.GetCacheLimit());
					gpxwidget.SetOutputFile(saved.GetOutputFile());
				}
				else
				{
					otherRadio.Active = true;
					
					gpswidget.SetCacheLimit(saved.GetCacheLimit());
					gpswidget.SetOutputFile(saved.GetOutputFile());
					gpswidget.SetBabelFormat(saved.GetBabelFormat());
				}
			}
			catch (GConf.NoSuchKeyException)
			{
				//Ignore
			}
		}

		protected virtual void OnGPXToggled (object sender, System.EventArgs e)
		{
			if (gpxRadio.Active) {
				table1.Remove (gusbwidet);
				table1.Remove (gpswidget);
				table1.Add (gpxwidget);
				Gtk.Table.TableChild props = ((Gtk.Table.TableChild)(this.table1[this.gpxwidget]));
				props.TopAttach = 4;
				props.RightAttach = 2;
				props.BottomAttach = 5;
				gpxwidget.Show ();
			}
		}

		protected virtual void OnButtonClick (object sender, System.EventArgs e)
		{
			this.Hide ();
		}
		protected virtual void OnOtherToggle (object sender, System.EventArgs e)
		{
			if (otherRadio.Active) {
				table1.Remove (gpxwidget);
				table1.Remove (gusbwidet);
				table1.Add (gpswidget);
				Gtk.Table.TableChild props = ((Gtk.Table.TableChild)(this.table1[this.gpswidget]));
				props.TopAttach = 4;
				props.RightAttach = 2;
				props.BottomAttach = 5;
				gpswidget.Show ();
			}
		}
		
		
	}
}
