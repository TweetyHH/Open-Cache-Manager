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


	[System.ComponentModel.ToolboxItem(true)]
	public partial class OCMQueryPage3 : Gtk.Bin
	{
		protected virtual void OnPlacedByToggle (object sender, System.EventArgs e)
		{
			placedEntry.Sensitive = placedByRadio.Active;
		}
		
		public String PlacedBy
		{
			get { 
				if (placedByRadio.Active)
					return placedEntry.Text;
				return null;
			}
			set
			{
				if (value == null)
					return;
				placedEntry.Text = value;
				placedByRadio.Active = true;
			}
		}
		
		public DateTime PlaceBefore
		{
			get
			{
				if (hiddenCheck.Active && hiddenCombo.Active == 0)
					return hiddenDateEntry.Date;
				return DateTime.MinValue;				
			}
			set
			{
				if (value == DateTime.MinValue)
					return;
				hiddenCheck.Active = true;
				hiddenCombo.Active = 0;
				hiddenDateEntry.Date = value;
			}
		}
		
		public DateTime PlaceAfter
		{
			get
			{
				if (hiddenCheck.Active && hiddenCombo.Active == 1)
					return hiddenDateEntry.Date;
				return DateTime.MinValue;			
			}
			set
			{
				if (value == DateTime.MinValue)
					return;
				hiddenCheck.Active = true;
				hiddenCombo.Active = 1;
				hiddenDateEntry.Date = value;
			}
		}
		
		
		
	
		public string Country
		{
			get { 
				if (countryCheck.Active)
					return countryEntry.Text;
				else
					return null;
				
			}
			set { 
				if (value != null)
				{
					countryEntry.Text = value;
					countryCheck.Active = true;
				}
				else
				{
					countryCheck.Active = false;
				}
			}
		}
		
		public string Province
		{
			get { 
				if (stateCheck.Active)
					return stateEntry.Text;
				else
					return null;
			}
			set { 
				if (value != null)
				{
					stateEntry.Text = value;
					stateCheck.Active = true;
				}
				else
				{
					stateCheck.Active = false;
				}
			}
		}
		
		
		public OCMQueryPage3 ()
		{
			this.Build ();
		}
		
		protected virtual void OnCountryToggle (object sender, System.EventArgs e)
		{
			countryEntry.Sensitive = countryCheck.Active;
		}
			
		protected virtual void OnStateCheckToggle (object sender, System.EventArgs e)
		{
			stateEntry.Sensitive = stateCheck.Active;
		}
		
		protected virtual void OnHiddenToggle (object sender, System.EventArgs e)
		{
			hiddenCombo.Sensitive = hiddenCheck.Active;
			hiddenDateEntry.Sensitive = hiddenCheck.Active;
		}
		
		
		
	}
}
