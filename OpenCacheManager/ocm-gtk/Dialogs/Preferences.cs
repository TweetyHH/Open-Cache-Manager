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
		
		protected virtual void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			this.Hide();
		}
		protected virtual void OnCancelClicked (object sender, System.EventArgs e)
		{
			this.Hide();
		}
		
		
	}
}
