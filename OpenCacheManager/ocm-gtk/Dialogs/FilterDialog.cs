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
using Gtk;

namespace ocmgtk
{


	public partial class FilterDialog : Gtk.Dialog
	{
		public FilterList Filter {
			get {
				FilterList filter = new FilterList ();
				GetTerrainFilter (filter);
				GetDifficultyFilter (filter);
				GetCTypeFilter (filter);
				GetPlacedByFilter (filter);
				GetPlacedByMeFilter (filter);
				GetKeyWordFilter (filter);
				if (null != contPage.ContainerTypes)
					filter.AddFilterCriteria(FilterList.KEY_CONTAINER, contPage.ContainerTypes);
				else
					filter.RemoveCriteria(FilterList.KEY_CONTAINER);
				return filter;
			}
			set {
				if (value == null) {
					return;
				}
				page1.TerrValue = value.GetCriteria (FilterList.KEY_TERRAIN_VAL) as string;
				page1.TerrOperator = value.GetCriteria (FilterList.KEY_TERRAIN_OP) as string;
				page1.DifficultyValue = value.GetCriteria (FilterList.KEY_DIFF_VAL) as string;
				page1.DifficultyOperator = value.GetCriteria (FilterList.KEY_DIFF_OP) as string;
				page1.SelectedCacheTypes = value.GetCriteria(FilterList.KEY_CACHETYPE) as List<string>;
				contPage.PlacedBy = value.GetCriteria(FilterList.KEY_PLACEDBY) as string;
				if (null != value.GetCriteria(FilterList.KEY_MINE))
					contPage.PlacedByMe = true;
				contPage.DescriptionKeyWords = value.GetCriteria(FilterList.KEY_DESCRIPTION) as string;
				contPage.ContainerTypes = value.GetCriteria(FilterList.KEY_CONTAINER) as List<string>;
	
			}
		}
		
		private void GetKeyWordFilter (FilterList filter)
		{
			if (contPage.DescriptionKeyWords != null)
					filter.AddFilterCriteria(FilterList.KEY_DESCRIPTION, contPage.DescriptionKeyWords);
				else
					filter.RemoveCriteria(FilterList.KEY_DESCRIPTION);
		}
		
		private void GetPlacedByMeFilter (FilterList filter)
		{
			if (contPage.PlacedByMe)
					filter.AddFilterCriteria(FilterList.KEY_MINE, UIMonitor.getInstance().OwnerID);
				else
					filter.RemoveCriteria(FilterList.KEY_MINE);
		}
		
		private void GetPlacedByFilter (FilterList filter)
		{
			String placedby = contPage.PlacedBy;
				if (null != placedby)
					filter.AddFilterCriteria(FilterList.KEY_PLACEDBY, placedby);
				else
					filter.RemoveCriteria(FilterList.KEY_PLACEDBY);
		}

		private void GetCTypeFilter (FilterList filter)
		{
			if (null != page1.SelectedCacheTypes) 
				filter.AddFilterCriteria (FilterList.KEY_CACHETYPE, page1.SelectedCacheTypes);
			else
				filter.RemoveCriteria(FilterList.KEY_CACHETYPE);
		}

		private void GetDifficultyFilter (FilterList filter)
		{
			if (!String.IsNullOrEmpty (page1.DifficultyValue)) {
				filter.AddFilterCriteria (FilterList.KEY_DIFF_VAL, page1.DifficultyValue);
				filter.AddFilterCriteria (FilterList.KEY_DIFF_OP, page1.DifficultyOperator);
			}
			else
			{
				filter.RemoveCriteria(FilterList.KEY_DIFF_OP);
				filter.RemoveCriteria(FilterList.KEY_DIFF_VAL);
			}
		}

		private void GetTerrainFilter (FilterList filter)
		{
			if (!String.IsNullOrEmpty (page1.TerrValue)) {
				filter.AddFilterCriteria (FilterList.KEY_TERRAIN_VAL, page1.TerrValue);
				filter.AddFilterCriteria (FilterList.KEY_TERRAIN_OP, page1.TerrOperator);
			}
			else
			{
				filter.RemoveCriteria(FilterList.KEY_TERRAIN_OP);
				filter.RemoveCriteria(FilterList.KEY_TERRAIN_VAL);
			}
				
		}

		public FilterDialog ()
		{
			this.Build ();
		}

		protected virtual void OnCancel (object sender, System.EventArgs e)
		{
			this.Respond (ResponseType.Cancel);
			this.Hide ();
		}

		protected virtual void OnOKClicked (object sender, System.EventArgs e)
		{
			this.Respond (ResponseType.Ok);
			this.Hide ();
		}
		
		
	}
}
