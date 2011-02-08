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
using ocmengine;
using Gtk;
using Mono.Unix;

namespace ocmgtk
{


	public partial class Preferences : Gtk.Dialog
	{
		private IConfig m_config = null;

		public Preferences (IConfig config, QuickFilters filters)
		{
			this.Build ();
			m_config = config;
			coordinateEntry.Latitude = config.HomeLat;
			coordinateEntry.Longitude = config.HomeLon;
			memberId.Text = config.OwnerID;
			unitsCombo.Active = config.ImperialUnits ? 1:0;
			nearbyCombo.Active = config.ShowNearby ? 0:1;
			childPointCombo.Active = config.ShowAllChildren? 1:0;
			dataDirEntry.Text = config.DataDirectory;
			importDirEntry.Text = config.ImportDirectory;
			WaypointSolvedMode = config.SolvedModeState;
			SetStartupFilter(filters, config.StartupFilter);
			DefaultMap = config.MapType;
			MapPoints = config.MapPoints;
			prefixModeCombo.Active = config.IgnoreWaypointPrefixes ? 1:0;
			updateCheck.Active = config.CheckForUpdates;
			UpdateInterval = config.UpdateInterval;
			directEntryCheck.Active = config.UseDirectEntryMode;
			autoCloseCheck.Active = config.AutoCloseWindows;
			pollEntry.Text = config.GPSDPoll.ToString();
			recenterCheck.Active = config.GPSDAutoMoveMap;
		}
		
		private SolvedMode WaypointSolvedMode
		{
			get
			{
				if (solvedAllRadio.Active)
					return SolvedMode.ALL;
				else if (solvedNoneRadio.Active)
					return SolvedMode.NONE;
				else
					return SolvedMode.PUZZLES;
			}
			set
			{
				switch (value)
				{
					case SolvedMode.ALL:
						solvedAllRadio.Active = true;
						break;
					case SolvedMode.PUZZLES:
						solvedPuzzRadio.Active = true;
						break;
					default:
						solvedNoneRadio.Active = true;
						break;
				}
			}
		}
		
		private  void SetStartupFilter(QuickFilters filterList, String  filterName)
		{
			int i=0;
			foreach(QuickFilter item in filterList.FilterArray)
			{
				startupFilterCombo.AppendText(item.Name);
				if (item.Name == filterName)
					startupFilterCombo.Active = i;
				i++;
			}
			if (startupFilterCombo.Active < 0)
				startupFilterCombo.Active = 0;
		}
		
		private string DefaultMap
		{
			get {
				switch (mapsCombo.Active)
				{
					case 1:
						return "ghyb";
					case 2:
						return "gmap";
					case 3:
						return "gphy";
					default:
						return "osm";
				}
			}
			set {
				if (value == "ghyb")
					mapsCombo.Active = 1;
				else if (value == "gmap")
					mapsCombo.Active = 2;
				else if (value == "gphy")
					mapsCombo.Active = 3;
				else
					mapsCombo.Active = 0;
			}
		}
		
		private int MapPoints
		{
			get { return Int16.Parse(mapPointEntry.Text);}
			set { mapPointEntry.Text = value.ToString();}
		}

		private int UpdateInterval
		{
			get { return int.Parse(updateEntry.Text);}
			set { updateEntry.Text = value.ToString();}
		}
		
		
		protected virtual void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			this.Hide();
			m_config.HomeLat = coordinateEntry.Latitude;
			m_config.HomeLon = coordinateEntry.Longitude;
			m_config.OwnerID = memberId.Text;
			m_config.ImperialUnits = (unitsCombo.Active == 1)? true:false;
			m_config.ShowNearby = (nearbyCombo.Active == 0)?true:false;
			m_config.ShowAllChildren = (childPointCombo.Active == 1)?true:false;
			m_config.MapPoints = MapPoints;
			m_config.DataDirectory = dataDirEntry.Text;
			m_config.ImportDirectory = importDirEntry.Text;
			m_config.SolvedModeState = WaypointSolvedMode;
			m_config.StartupFilter = startupFilterCombo.ActiveText;
			m_config.MapType = DefaultMap;
			m_config.IgnoreWaypointPrefixes = (prefixModeCombo.Active == 1)?true:false;
			m_config.CheckForUpdates = updateCheck.Active;
			m_config.UpdateInterval = UpdateInterval;
			m_config.UseDirectEntryMode = directEntryCheck.Active;
			m_config.AutoCloseWindows = autoCloseCheck.Active;
			m_config.GPSDPoll = int.Parse(pollEntry.Text);
			m_config.GPSDAutoMoveMap = recenterCheck.Active;
		}
		
		protected virtual void OnCancelClicked (object sender, System.EventArgs e)
		{
			this.Hide();
		}
		
		protected virtual void OnUpdateCheckToggle (object sender, System.EventArgs e)
		{
			updateEntry.Sensitive = updateCheck.Active;
		}
		
		protected virtual void OnDBDirClick (object sender, System.EventArgs e)
		{
			FileChooserDialog dlg = new FileChooserDialog (Catalog.GetString ("Choose Directory"), this, FileChooserAction.SelectFolder, Catalog.GetString ("Cancel"), ResponseType.Cancel, Catalog.GetString ("OK"), ResponseType.Accept);
			dlg.SetCurrentFolder (dataDirEntry.Text);
			if (dlg.Run () == (int)ResponseType.Accept) {
				dataDirEntry.Text = dlg.Filename;
			}
			dlg.Destroy ();
		}

		protected virtual void OnImpDirClicked (object sender, System.EventArgs e)
		{
			FileChooserDialog dlg = new FileChooserDialog (Catalog.GetString ("Choose Directory"), this, FileChooserAction.SelectFolder, Catalog.GetString ("Cancel"), ResponseType.Cancel, Catalog.GetString ("OK"), ResponseType.Accept);
			dlg.SetCurrentFolder (importDirEntry.Text);
			if (dlg.Run () == (int)ResponseType.Accept) {
				importDirEntry.Text = dlg.Filename;
			}
			dlg.Destroy ();
		}
		
		
	}
}
