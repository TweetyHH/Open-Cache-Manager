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
		GarminSerialWidget garswidget = new GarminSerialWidget();
		DeLormeWidget delwidget = new DeLormeWidget();
		GarminEdgeWidget edgeWidget = new GarminEdgeWidget();
		DelormeGPXWidget delgpxwidget = new DelormeGPXWidget();
		GPXWidget gpxwidget = new GPXWidget();

		public IGPSConfig GPSConfig {
			get
			{
				switch (deviceCombo.Active)
				{
					case 0:
						return gpxwidget;
					case 1:
						return gusbwidet;
					case 2:
						return garswidget;
					case 3:
						return edgeWidget;
					case 4:
						return delwidget;
					case 5:
						return delgpxwidget;
					default:
						return gpswidget;
				}
			}
		}
		
		public void UpdateWaypointSymbols(Config config)
		{
			waypointWidget.UpdateMappings(config);
		}

		public GPSConfiguration (Config config)
		{
			this.Build ();			
			try
			{
				waypointWidget.PopulateMappings(config);
				waypointWidget.ShowAll();
				SavedGPSConf saved = new SavedGPSConf(config);
				if ((saved.GetBabelFormat() == "garmin") && (saved.GetOutputFile() == "usb:"))
				{
					gusbwidet.SetCacheLimit(saved.GetCacheLimit());
					gusbwidet.SetNameMode(saved.GetNameMode());
					gusbwidet.SetDescMode(saved.GetDescMode());
					deviceCombo.Active = 1;
				}
				else if (saved.GetBabelFormat() == "garmin")
				{
					garswidget.SetCacheLimit(saved.GetCacheLimit());
					garswidget.SetOutputFile(saved.GetOutputFile());
					garswidget.SetNameMode(saved.GetNameMode());
					deviceCombo.Active = 2;
				}
				else if (saved.GetBabelFormat()== "OCM_GPX")
				{
					gpxwidget.SetCacheLimit(saved.GetCacheLimit());
					gpxwidget.SetOutputFile(saved.GetOutputFile());
					gpxwidget.SetLogLimit(saved.GetLogLimit());
					gpxwidget.SetIncludeAttributes(saved.IncludeAttributes());
					deviceCombo.Active = 0;
					ShowDeviceConfig();
				}
				else if (saved.GetBabelFormat().StartsWith("delbin"))
				{
					delwidget.SetCacheLimit(saved.GetCacheLimit());
					delwidget.SetLogLimit(saved.GetLogLimit());
					delwidget.SetIncludeAttributes(saved.IncludeAttributes());
					deviceCombo.Active = 4;
				}
				else if (saved.GetBabelFormat() == "edge")
				{
					edgeWidget.SetCacheLimit(saved.GetCacheLimit());
					edgeWidget.SetOutputFile(saved.GetOutputFile());
					edgeWidget.SetDescMode(saved.GetDescMode());
					edgeWidget.SetNameMode(saved.GetNameMode());
					deviceCombo.Active = 3;
				}
				else if (saved.GetBabelFormat() == "delgpx")
				{
					delgpxwidget.SetCacheLimit(saved.GetCacheLimit());
					delgpxwidget.SetOutputFile(saved.GetOutputFile());
					delgpxwidget.SetLogLimit(saved.GetLogLimit());
					delgpxwidget.SetIncludeAttributes(saved.IncludeAttributes());
					deviceCombo.Active = 5;
				}
				else 
				{	
					gpswidget.SetCacheLimit(saved.GetCacheLimit());
					gpswidget.SetOutputFile(saved.GetOutputFile());
					gpswidget.SetBabelFormat(saved.GetBabelFormat());
					gpswidget.SetDescMode(saved.GetDescMode());
					gpswidget.SetNameMode(saved.GetNameMode());
					deviceCombo.Active = 6;
				}
			}
			catch (GConf.NoSuchKeyException)
			{
				//Ignore
			}
		}


		protected virtual void OnButtonClick (object sender, System.EventArgs e)
		{
			this.Hide ();
		}
		
		
		protected virtual void OnComboChange (object sender, System.EventArgs e)
		{
			ShowDeviceConfig ();
		}
		
		private void ShowDeviceConfig ()
		{
			Gtk.Table.TableChild props;
			
			foreach (Gtk.Widget child in table1.Children)
			{
				if (child != deviceCombo && child != deviceLabel)
					table1.Remove(child);
			}
			
			switch (deviceCombo.Active)
			{
				case 0:
					table1.Add (gpxwidget);
					props = ((Gtk.Table.TableChild)(this.table1[gpxwidget]));
					props.TopAttach = 2;
					props.RightAttach = 2;
					props.BottomAttach = 3;
					gpxwidget.Show ();
					waypointWidget.Sensitive = true;
					break;
				case 1:
					table1.Add (gusbwidet);
					props = ((Gtk.Table.TableChild)(this.table1[gusbwidet]));
					props.TopAttach = 2;
					props.RightAttach = 2;
					props.BottomAttach = 3;
					gusbwidet.Show ();
					waypointWidget.Sensitive = true;
					break;
				case 2:
					table1.Add (garswidget);
					props = ((Gtk.Table.TableChild)(this.table1[garswidget]));
					props.TopAttach = 2;
					props.RightAttach = 2;
					props.BottomAttach = 3;
					garswidget.Show ();
					waypointWidget.Sensitive = true;
					break;
				case 3:
					table1.Add (edgeWidget);
					props = ((Gtk.Table.TableChild)(this.table1[edgeWidget]));
					props.TopAttach = 2;
					props.RightAttach = 2;
					props.BottomAttach = 3;
					edgeWidget.Show ();
					waypointWidget.Sensitive = true;
					break;
				case 4:
					table1.Add(delwidget);
					props = ((Gtk.Table.TableChild)(this.table1[delwidget]));
					props.TopAttach = 2;
					props.RightAttach = 2;
					props.BottomAttach = 3;
					delwidget.Show ();
					waypointWidget.Sensitive = false;
					break;
				case 5:
					table1.Add(delgpxwidget);
					props = ((Gtk.Table.TableChild)(this.table1[delgpxwidget]));
					props.TopAttach = 2;
					props.RightAttach = 2;
					props.BottomAttach = 3;
					delgpxwidget.Show ();
					waypointWidget.Sensitive = false;
					break;
				default:
					table1.Add (gpswidget);
					props = ((Gtk.Table.TableChild)(this.table1[gpswidget]));
					props.TopAttach = 2;
					props.RightAttach = 2;
					props.BottomAttach = 3;
					gpswidget.Show ();
					waypointWidget.Sensitive = true;
					break;
			}
		}		
	}
}
