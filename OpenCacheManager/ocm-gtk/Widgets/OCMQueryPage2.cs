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
	public partial class OCMQueryPage2 : Gtk.Bin
	{

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
		
		public bool PlacedByMe
		{
			get
			{
				return placedMeRadio.Active;
			}
			set
			{
				placedMeRadio.Active = value;
			}
		}
		
		public string DescriptionKeyWords
		{
			get
			{
				if (descCheck.Active)
					return descEntry.Text;
				return null;
			}
			set 
			{
				if (value != null)
				{
					descCheck.Active = true;
					descEntry.Text = value;
				}
			}
		}
		
		public List<String> ContainerTypes
		{
			get
			{
				if (allContRadio.Active)
					return null;
				List<String> list = new List<String>();
				if (microCheck.Active)
					list.Add("Micro");
				if (smallCheck.Active)
					list.Add("Small");
				if (regularCheck.Active)
					list.Add("Regular");
				if (largeCheck.Active)
					list.Add("Large");
				if (notChosenCheck.Active)
					list.Add("Not chosen");
				if (virtualCheck.Active)
					list.Add("Virtual");
				return list;
			}
			set
			{
				if (value == null)
				{
					allContRadio.Active = true;
					return;
				}
				selRado.Active = true;
				IEnumerator<String> val = value.GetEnumerator();
				while (val.MoveNext())
				{
					if (val.Current == "Micro")
						microCheck.Active = true;
					if (val.Current == "Small")
						smallCheck.Active = true;
					if (val.Current == "Regular")
						regularCheck.Active = true;
					if (val.Current == "Large")
						largeCheck.Active = true;
					if (val.Current == "Not chosen")
						notChosenCheck.Active = true;
					if (val.Current == "Virtual")
						virtualCheck.Active = true;
				}
			}
		}
		

		public OCMQueryPage2 ()
		{
			this.Build ();
		}
		
		protected virtual void OnAnyToggle (object sender, System.EventArgs e)
		{
			contFrame.Sensitive = selRado.Active;
		}
		
		protected virtual void OnDescToggled (object sender, System.EventArgs e)
		{
			descEntry.Sensitive = descCheck.Active;
		}
		
		protected virtual void OnPlacedByToggle (object sender, System.EventArgs e)
		{
			placedEntry.Sensitive = placedByRadio.Active;
		}
				
	}
}