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

namespace ocmgtk
{


	public partial class Preferences : Gtk.Dialog
	{

		public Preferences ()
		{
			this.Build ();
		}

		public double Lat {
			get {
				return latControl.getCoordinate();
			}
			set
			{
				latControl.SetCoordinate(value, true);
			}
		}

		public double Lon {
			get {
				return lonControl.getCoordinate();
			}
			set
			{
				lonControl.SetCoordinate(value, false);
			}
		}
		
		public string MemberID
		{
			get
			{
				return memberId.Text;
			}
			set
			{
				memberId.Text = value;
			}
		}
		
		public bool ImperialUnits
		{
			get { 
				if (unitsCombo.Active == 1)
					return true;
				else
					return false;
			}
			set
			{
				if (value)
					unitsCombo.Active = 1;
				else
					unitsCombo.Active = 0;
			}				
		}
		
		public bool ShowNearby
		{
			get { 
				if (nearbyCombo.Active == 0)
					return true;
				else
					return false;
			}
			set
			{
				if (value)
					nearbyCombo.Active = 0;
				else
					nearbyCombo.Active = 1;
			}		
		}
		
		public bool ShowAllChildren
		{
			get 
			{
				if (childPointCombo.Active == 0)
					return false;
				else
					return true;
			}
			set
			{
				if (value)
					childPointCombo.Active = 1;
				else
					childPointCombo.Active = 0;
			}
		}
		
		public string DefaultMap
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
		
		public int MapPoints
		{
			get { return Int16.Parse(mapPointEntry.Text);}
			set { mapPointEntry.Text = value.ToString();}
		}

		public bool UsePrefixesForChildWaypoints
		{
			get {
				if (modeCombo.Active == 0)
					return true;
				return false;
			}
			set 
			{
				if (value)
					modeCombo.Active = 0;
				else
					modeCombo.Active = 1;
			}
		}
		
		public bool CheckForUpdates
		{
			get { return updateCheck.Active;}
			set { updateCheck.Active = value;}
		}
		
		public int UpdateInterval
		{
			get { return int.Parse(updateEntry.Text);}
			set { updateEntry.Text = value.ToString();}
		}
		
		
		protected virtual void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			this.Hide();
		}
		protected virtual void OnCancelClicked (object sender, System.EventArgs e)
		{
			this.Hide();
		}
		
		protected virtual void OnUpdateCheckToggle (object sender, System.EventArgs e)
		{
			updateEntry.Sensitive = updateCheck.Active;
		}
		
		
		
	}
}
