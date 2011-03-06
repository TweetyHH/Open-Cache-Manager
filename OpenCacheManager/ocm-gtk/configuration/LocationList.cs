// 
//  Copyright 2011  Kyle Campbell
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
using System.Collections.Generic;
using Gtk;
using Mono.Unix;
using System.Runtime.Serialization.Formatters.Binary;


namespace ocmgtk
{

	[Serializable]
	public class LocationList
	{
		[NonSerialized]
		ToggleAction gpsdAction = null;

		public LocationList ()
		{
		}
		
		private Dictionary<string, Location> m_locations = new Dictionary<string, Location>();
		
		public Location[] Locations
		{
			get{
				Location[] locs = new Location[m_locations.Count];
				m_locations.Values.CopyTo(locs, 0);
				return locs;
			}
			set{
				foreach(Location location in value)
				{
					m_locations.Add(location.Name, location);
				}				
			}
		}
		
		public Location GetLocation(String name)
		{
			if (m_locations.ContainsKey(name))
				return m_locations[name];
			return null;
		}

		
		public void AddLocation(Location loc)
		{
			if (m_locations.ContainsKey(loc.Name))
			{
				MessageDialog dlg = new MessageDialog(null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo,
				                                      String.Format(Catalog.GetString("Are you sure you wish to " +
				                                      	"overwrite \"{0}\"?"), loc.Name));
				if ((int) ResponseType.Yes != dlg.Run())
				{
					dlg.Hide();
					return;
				}
				else
				{
					dlg.Hide();
					m_locations.Remove(loc.Name);
				}
			}
			m_locations.Add(loc.Name, loc);
			UpdateLocFile();
		}
		
		public void DeleteLocation(string name)
		{
			m_locations.Remove(name);
			UpdateLocFile();
		}
		
		public static LocationList LoadLocationList()
		{
			System.Console.WriteLine("Reading");
			String path = System.Environment.GetFolderPath (System.Environment.SpecialFolder.ApplicationData);
			if (!File.Exists(path + "/ocm/locs.oqf"))
			{
				return new LocationList();
			}
			FileStream fs = new FileStream(path + "/ocm/locs.oqf", FileMode.Open, FileAccess.Read);	
			BinaryFormatter ser = new BinaryFormatter();
			System.Object filters = ser.Deserialize(fs);
			fs.Close();
			System.Console.WriteLine("Finished Reading");
			return filters as LocationList;
			
		}
		
		private void UpdateLocFile()
		{
			System.Console.WriteLine("Writing");
			String path = System.Environment.GetFolderPath (System.Environment.SpecialFolder.ApplicationData);
			if (!Directory.Exists("ocm"))
				Directory.CreateDirectory(path + "/ocm");
			path = path + "/ocm";
			BinaryFormatter ser = new BinaryFormatter();
			FileStream fs = new FileStream(path + "/locs.oqf", FileMode.Create, FileAccess.ReadWrite);
			ser.Serialize(fs, this);
			fs.Close();
			System.Console.WriteLine("Finished");
		}
		
		public Menu BuildLocationlMenu()
		{
			Menu etMenu = new Menu();
			
			Gtk.Action home_action = new Gtk.Action("Home", Catalog.GetString("Home"));
			etMenu.Append(home_action.CreateMenuItem());
			home_action.Activated += HandleHome_actionActivated;
			int iCount = 0;
			foreach (Location loc in m_locations.Values)
			{
				Gtk.Action action = new Gtk.Action(loc.Name, loc.Name);
				etMenu.Append(action.CreateMenuItem());
				action.Activated += HandleActionActivated;
				iCount ++;
				
			}
			etMenu.Append(new MenuItem());
			gpsdAction = new ToggleAction("UseGPSD", Catalog.GetString("GPSD Position"),null, null);
			gpsdAction.Active = UIMonitor.getInstance().Configuration.UseGPSD;
			gpsdAction.Toggled += HandleGpsdActionToggled;
			etMenu.Append(gpsdAction.CreateMenuItem());	
			etMenu.ShowAll();
			return etMenu;
		}

		void HandleGpsdActionToggled (object sender, EventArgs e)
		{
			UIMonitor mon = UIMonitor.getInstance();
			if (((ToggleAction) sender).Active)
			{
				mon.EnableGPS();
				mon.Configuration.UseGPSD = true;
			}
			else
			{
				mon.DisableGPS();
				mon.Configuration.UseGPSD = false;
			}
		}

		void HandleHome_actionActivated (object sender, EventArgs e)
		{
			gpsdAction.Active = false;
			UIMonitor.getInstance().ResetCenterToHome();
		}
		
		void HandleActionActivated (object sender, EventArgs e)
		{
			Location loc = m_locations[((sender) as Gtk.Action).Name];
			gpsdAction.Active = false;
			UIMonitor.getInstance().SetLocation(loc);
		}
	}
}
