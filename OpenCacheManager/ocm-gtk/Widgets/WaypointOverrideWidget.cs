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

namespace ocmgtk
{


	[System.ComponentModel.ToolboxItem(true)]
	public partial class WaypointOverrideWidget : Gtk.Bin
	{
		
		public WaypointOverrideWidget ()
		{
			this.Build ();
			PopulateMappings(new Config());
		}
		
		public void PopulateMappings(Config config)
		{
			System.Console.WriteLine("CALLED");
			widgetBox.Add(new SymbolChooser("Geocache|Traditional Cache", config.Get("/apps/ocm/wmappings/Geocache_Traditional_Cache", "Geocache") as string));
			widgetBox.Add(new SymbolChooser("Geocache|Unknown Cache",config.Get("/apps/ocm/wmappings/Geocache_Unknown_Cache", "Geocache") as string));
			widgetBox.Add(new SymbolChooser("Geocache|Virtual Cache", config.Get("/apps/ocm/wmappings/Geocache_Virtual_Cache", "Geocache") as string));
			widgetBox.Add(new SymbolChooser("Geocache|Multi-cache", config.Get("/apps/ocm/wmappings/Geocache_Multi-cache", "Geocache") as string));
			widgetBox.Add(new SymbolChooser("Geocache|Project APE Cache", config.Get("/apps/ocm/wmappings/Geocache_Project_APE_Cache", "Geocache") as string));
			widgetBox.Add(new SymbolChooser("Geocache|Cache In Trash Out Event", config.Get("/apps/ocm/wmappings/Geocache_Cache_In_Trash_Out_Event", "Geocache") as string));
			widgetBox.Add(new SymbolChooser("Geocache|Earthcache", config.Get("/apps/ocm/wmappings/Geocache_Earthcache", "Geocache") as string));
			widgetBox.Add(new SymbolChooser("Geocache|Event Cache", config.Get("/apps/ocm/wmappings/Geocache_Event_Cache", "Geocache") as string));
			widgetBox.Add(new SymbolChooser("Geocache|Letterbox Hybrid", config.Get("/apps/ocm/wmappings/Geocache_Letterbox_Hybrid", "Geocache") as string));
			widgetBox.Add(new SymbolChooser("Geocache|GPS Adventures Exhibit",config.Get("/apps/ocm/wmappings/Geocache_GPS_Adventures_Exhibit", "Geocache") as string));
			widgetBox.Add(new SymbolChooser("Geocache|Mega-Event Cache", config.Get("/apps/ocm/wmappings/Geocache_Mega-Event_Cache", "Geocache") as string));
			widgetBox.Add(new SymbolChooser("Geocache|Locationless Cache",config.Get("/apps/ocm/wmappings/Geocache_Locationless_Cache", "Geocache") as string));
			widgetBox.Add(new SymbolChooser("Geocache|Webcam cache",config.Get("/apps/ocm/wmappings/Geocache_Webcam_Cache", "Geocache") as string));
			widgetBox.Add(new SymbolChooser("Geocache|Wherigo Cache", config.Get("/apps/ocm/wmappings/Geocache_Wherigo_Cache", "Geocache") as string));
			widgetBox.Add(new SymbolChooser("Geocache", config.Get("/apps/ocm/wmappings/Geocache", "Geocache") as string));
			widgetBox.Add(new SymbolChooser("Geocache Found", config.Get("/apps/ocm/wmappings/Geocache_Found", "Geocache Found") as string));
			widgetBox.Add(new SymbolChooser("Waypoint|Final Location", config.Get("/apps/ocm/wmappings/Waypoint_Final_Location", "Pin, Blue") as string));
			widgetBox.Add(new SymbolChooser("Waypoint|Parking Area", config.Get("/apps/ocm/wmappings/Waypoint_Parking_Area", "Parking Area") as string));
			widgetBox.Add(new SymbolChooser("Waypoint|Reference Point", config.Get("/apps/ocm/wmappings/Waypoint_Reference_Point", "Pin, Green") as string));
			widgetBox.Add(new SymbolChooser("Waypoint|Question to Answer", config.Get("/apps/ocm/wmappings/Waypoint_Question_to_Answer", "Pin, Red") as string));
			widgetBox.Add(new SymbolChooser("Waypoint|Stages of a Multicache", config.Get("/apps/ocm/wmappings/Waypoint_Stages_of_a_Multicache", "Pin, Red") as string));
			widgetBox.Add(new SymbolChooser("Waypoint|Trailhead", config.Get("/apps/ocm/wmappings/Waypoint_Trailhead", "Trail Head") as string));
			widgetBox.Add(new SymbolChooser("Waypoint|Other", config.Get("/apps/ocm/wmappings/Waypoint_Other", "Pin, Green") as string));
			widgetBox.Add(null);
			widgetBox.ShowAll();
			scrolledwindow1.ShowAll();
		}	
		
		public void UpdateMappings(Config config)
		{
			System.Console.WriteLine("Update called");
			foreach(SymbolChooser chooser in widgetBox.Children)
			{
				string newKey = chooser.Key.Replace('|','_');
				newKey = newKey.Replace(' ','_');
				System.Console.WriteLine(newKey + " " + chooser.SymbolName);
				config.Set("/apps/ocm/wmappings/" + newKey, chooser.SymbolName);
			}
		}
	}
}
