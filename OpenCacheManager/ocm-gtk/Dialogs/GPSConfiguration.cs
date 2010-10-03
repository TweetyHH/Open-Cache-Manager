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
					deviceCombo.Active = 0;
				}
				else if (saved.GetBabelFormat().StartsWith("delbin"))
				{
					delwidget.SetCacheLimit(saved.GetCacheLimit());
					delwidget.SetLogLimit(saved.GetLogLimit());
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
				else 
				{	
					gpswidget.SetCacheLimit(saved.GetCacheLimit());
					gpswidget.SetOutputFile(saved.GetOutputFile());
					gpswidget.SetBabelFormat(saved.GetBabelFormat());
					gpswidget.SetDescMode(saved.GetDescMode());
					gpswidget.SetNameMode(saved.GetNameMode());
					deviceCombo.Active = 4;
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
			Gtk.Table.TableChild props;
			switch (deviceCombo.Active)
			{
				case 0:
					table1.Remove (gusbwidet);
					table1.Remove (gpswidget);
					table1.Remove(garswidget);
					table1.Remove(delwidget);
					table1.Remove(edgeWidget);
					table1.Add (gpxwidget);
					props = ((Gtk.Table.TableChild)(this.table1[this.gpxwidget]));
					props.TopAttach = 2;
					props.RightAttach = 2;
					props.BottomAttach = 3;
					gpxwidget.Show ();
					break;
				case 1:
					table1.Remove (gpxwidget);
					table1.Remove(gpswidget);
					table1.Remove(garswidget);
					table1.Remove(delwidget);
					table1.Remove(edgeWidget);
					table1.Add (gusbwidet);
					props = ((Gtk.Table.TableChild)(this.table1[this.gusbwidet]));
					props.TopAttach = 2;
					props.RightAttach = 2;
					props.BottomAttach = 3;
					gusbwidet.Show ();
					break;
				case 2:
					table1.Remove (gpxwidget);
					table1.Remove(gpswidget);
					table1.Remove(gusbwidet);
					table1.Remove(delwidget);
					table1.Remove(edgeWidget);
					table1.Add (garswidget);
					props = ((Gtk.Table.TableChild)(this.table1[this.garswidget]));
					props.TopAttach = 2;
					props.RightAttach = 2;
					props.BottomAttach = 3;
					garswidget.Show ();
					break;
				case 3:
					table1.Remove (gpxwidget);
					table1.Remove(gpswidget);
					table1.Remove(gusbwidet);
					table1.Remove(delwidget);
					table1.Remove(garswidget);
					table1.Add (edgeWidget);
					props = ((Gtk.Table.TableChild)(this.table1[this.edgeWidget]));
					props.TopAttach = 2;
					props.RightAttach = 2;
					props.BottomAttach = 3;
					edgeWidget.Show ();
					break;
				case 4:
					table1.Remove(gpswidget);
					table1.Remove(garswidget);
					table1.Remove(gusbwidet);
					table1.Remove(gpxwidget);
					table1.Remove(edgeWidget);
					table1.Add(delwidget);
					props = ((Gtk.Table.TableChild)(this.table1[this.delwidget]));
					props.TopAttach = 2;
					props.RightAttach = 2;
					props.BottomAttach = 3;
					delwidget.Show ();
					break;
				default:
					table1.Remove (gpxwidget);
					table1.Remove (gusbwidet);
					table1.Remove (garswidget);
					table1.Remove(delwidget);
					table1.Remove(edgeWidget);
					table1.Add (gpswidget);
					props = ((Gtk.Table.TableChild)(this.table1[this.gpswidget]));
					props.TopAttach = 2;
					props.RightAttach = 2;
					props.BottomAttach = 3;
					gpswidget.Show ();
					break;
			}
		}
		
		
		
	}
}
