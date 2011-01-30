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
using System.Collections.Generic;
using Mono.Unix;
using Gtk;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ocmgtk
{

	[Serializable]
	public class GPSProfileList
	{

		public GPSProfileList ()
		{
		}
		
		private Dictionary<string, GPSProfile> m_profiles = new Dictionary<string, GPSProfile>();
		
		public GPSProfile[] Profiles
		{
			get{
				GPSProfile[] profs = new GPSProfile[m_profiles.Count];
				m_profiles.Values.CopyTo(profs, 0);
				return profs;
			}
			set{
				foreach(GPSProfile location in value)
				{
					m_profiles.Add(location.Name, location);
				}				
			}
		}
		
		public GPSProfile GetActiveProfile()
		{
			string active = UIMonitor.getInstance().Configuration.GPSProf;
			if (active == null || !m_profiles.ContainsKey(active))
				return null;
			return m_profiles[active];
		}

		
		public void AddProfile(GPSProfile prof)
		{
			if (m_profiles.ContainsKey(prof.Name))
			{
				MessageDialog dlg = new MessageDialog(null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo,
				                                      String.Format(Catalog.GetString("Are you sure you wish to " +
				                                      	"overwrite \"{0}\"?"), prof.Name));
				if ((int) ResponseType.Yes != dlg.Run())
				{
					dlg.Hide();
					return;
				}
				else
				{
					dlg.Hide();
					m_profiles.Remove(prof.Name);
				}
			}
			m_profiles.Add(prof.Name, prof);
			UpdateProfFile();
		}
		
		public void EditProfile(GPSProfile prof)
		{
			m_profiles.Remove(prof.Name);
			m_profiles.Add(prof.Name, prof);
			UpdateProfFile();
		}
		
		public void DeleteProfile(string name)
		{
			m_profiles.Remove(name);
			UpdateProfFile();
		}
		
		public static GPSProfileList LoadProfileList()
		{
			String path = System.Environment.GetFolderPath (System.Environment.SpecialFolder.ApplicationData);
			if (!File.Exists(path + "/ocm/gps.oqf"))
			{
				return new GPSProfileList();
			}
			FileStream fs = new FileStream(path + "/ocm/gps.oqf", FileMode.Open, FileAccess.Read);	
			BinaryFormatter ser = new BinaryFormatter();
			System.Object filters = ser.Deserialize(fs);
			fs.Close();
			return filters as GPSProfileList;
		}
		
		private void UpdateProfFile()
		{
			String path = System.Environment.GetFolderPath (System.Environment.SpecialFolder.ApplicationData);
			if (!Directory.Exists("ocm"))
				Directory.CreateDirectory(path + "/ocm");
			path = path + "/ocm";
			BinaryFormatter ser = new BinaryFormatter();
			FileStream fs = new FileStream(path + "/gps.oqf", FileMode.Create, FileAccess.ReadWrite);
			ser.Serialize(fs, this);
			fs.Close();
		}
		
		public Menu BuildProfileMenu()
		{
			Menu etMenu = new Menu();
			int iCount = 0;
			Gtk.RadioAction first = null;
			foreach (GPSProfile loc in m_profiles.Values)
			{
				Gtk.RadioAction action = new Gtk.RadioAction(loc.Name, loc.Name, null, null, iCount);
				if (iCount == 0)
					first = action;
				else
					action.Group = first.Group;
				if (UIMonitor.getInstance().Configuration.GPSProf == loc.Name)
					action.Active = true;
				else
					action.Active = false;
				etMenu.Append(action.CreateMenuItem());
				action.Toggled += HandleActionToggled;
				iCount ++;
				
			}
			return etMenu;
		}
		
		public Menu BuildProfileEditMenu()
		{
			Menu etMenu = new Menu();
			int iCount = 0;
			foreach (GPSProfile loc in m_profiles.Values)
			{
				Gtk.Action action = new Gtk.Action(loc.Name, loc.Name);
				etMenu.Append(action.CreateMenuItem());
				action.Activated += HandleActionActivated;
				iCount ++;
				
			}
			return etMenu;
		}

		void HandleActionActivated (object sender, EventArgs e)
		{
			GPSProfile prof = m_profiles[((sender) as Gtk.Action).Name];
			UIMonitor.getInstance().EditProfile(prof);
		}

		void HandleActionToggled (object sender, EventArgs e)
		{
			GPSProfile prof = m_profiles[((sender) as Gtk.Action).Name];
			prof.SetAsCurrent();
		}
	}
}
