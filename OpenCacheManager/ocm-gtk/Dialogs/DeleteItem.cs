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
using ocmengine;
using Mono.Unix;

namespace ocmgtk
{


	public partial class DeleteItem : Gtk.Dialog
	{			
		public string Bookmark
		{
			get { return bmrkCombo.ActiveText;}
		}
		
		public DeleteItem ()
		{
			this.Build();
			this.PopulateList();
		}
		
		public DeleteItem (QuickFilters filters)
		{
			this.Build ();
			if (filters != null)
			{
				this.label1.Text = Catalog.GetString("Quick Filter:");
				int iCount = 0;
				foreach (QuickFilter filter in filters.FilterArray)
				{
					if (iCount <= 3)
					{
						iCount++;
						continue;
					}
					bmrkCombo.AppendText(filter.Name);
					bmrkCombo.Active = 0;
					iCount ++;
				}
			}
			else
			{
				PopulateList();
			}
		}
		
		public DeleteItem (LocationList locations)
		{
			this.Build ();
			if (locations != null)
			{
				this.label1.Text = Catalog.GetString("Location:");
				int iCount = 0;
				foreach (Location loc in locations.Locations)
				{
					bmrkCombo.AppendText(loc.Name);
					bmrkCombo.Active = 0;
					iCount ++;
				}
			}
		}
		
		public DeleteItem (GPSProfileList profiles)
		{
			this.Build ();
			if (profiles != null)
			{
				this.label1.Text = Catalog.GetString("GPS Profile:");
				int iCount = 0;
				foreach (GPSProfile loc in profiles.Profiles)
				{
					bmrkCombo.AppendText(loc.Name);
					bmrkCombo.Active = 0;
					iCount ++;
				}
			}
		}
		
		
		private void PopulateList()
		{
			List<string> bmrkList =  Engine.getInstance().Store.GetBookmarkLists();
			foreach (string str in bmrkList)
			{
				bmrkCombo.AppendText(str);
			}
			bmrkCombo.Active = 0;
		}
		
		protected virtual void OnCancelClicked (object sender, System.EventArgs e)
		{
			this.Hide();
		}
		
		protected virtual void OnOKClicked (object sender, System.EventArgs e)
		{
			this.Hide();
		}
		
		
	}
}
