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
using System.Text.RegularExpressions;
using ocmengine;
using Mono.Unix;

namespace ocmgtk
{


	[System.ComponentModel.ToolboxItem(true)]
	public partial class ReviewWaypointWidget : Gtk.Bin
	{
		public Match WaypointMatch
		{
			set
			{
				Geocache parent = UIMonitor.getInstance().SelectedCache;
				DegreeMinutes[] coord =  Utilities.ParseCoordString(value.Captures[0].Value);	
				coordLabel.Text = Utilities.getCoordString(coord[0], coord[1]);
				sourceText.Buffer.Text = parent.LongDesc.Substring(value.Index - 100, value.Length + 100);
				String name = "RP" + parent.Name.Substring (2);
				if ((bool) UIMonitor.getInstance().Configuration.Get("/apps/ocm/noprefixes", false))
				{
					name = parent.Name;
				}
				name = Engine.getInstance().Store.GenerateNewName(name);
				nameEntry.Text = name;
				descriptionText.Buffer.Text = Catalog.GetString("Grabbed Waypoint");
			}
		}
		
		
		public ReviewWaypointWidget ()
		{
			this.Build ();
		}
	}
}
