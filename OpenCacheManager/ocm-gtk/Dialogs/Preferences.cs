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
				double val = Double.Parse (latEntry.Text);
				if (nsBox.Active == 1)
					return val * -1;
				else
					return val;
			}
		}

		public double Lon {
			get {
				double val = Double.Parse (lonEntry.Text);
				if (ewBox.Active == 1)
					return val * -1;
				else
					return val;
			}
		}

		public void SetLat (double lat)
		{
			if (lat > 0) {
				nsBox.Active = 0;
				latEntry.Text = lat.ToString ("0.000");
			} else {
				nsBox.Active = 1;
				latEntry.Text = (lat * -1).ToString ("0.000");
			}
		}

		public void SetLon (double lon)
		{
			if (lon > 0) {
				ewBox.Active = 0;
				lonEntry.Text = lon.ToString ("0.000");
			} else {
				ewBox.Active = 1;
				lonEntry.Text = (lon * -1).ToString ("0.000");
			}
		}
		protected virtual void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			this.Hide();
			this.Dispose();
		}
		
		
	}
}
