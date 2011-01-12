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
				GetContainerFilter (filter);
				GetDateFilter (filter);
				string cntry = datePage.Country;
				if (!String.IsNullOrEmpty(cntry))
					filter.AddFilterCriteria(FilterList.KEY_COUNTRY, datePage.Country);
				string state = datePage.Province;
				if (!String.IsNullOrEmpty(state))
					filter.AddFilterCriteria(FilterList.KEY_STATE, datePage.Province);
				GetFoundOnFilter (filter);
				GetFoundBeforeFilter (filter);
				GetFoundAfterFilter (filter);
				HasNotesFilter (filter);
				GetChildrenFilter (filter);
				GetNoChildrenFilter (filter);
				if (childrenPage.HasCorrectedCoords)
					filter.AddFilterCriteria(FilterList.KEY_CORRECTED, true);
				else
					filter.RemoveCriteria(FilterList.KEY_CORRECTED);
				if (childrenPage.DoesNotHaveCorrectedCoords)
					filter.AddFilterCriteria(FilterList.KEY_NOCORRECTED, true);
				else
					filter.RemoveCriteria(FilterList.KEY_NOCORRECTED);
				filter.AddFilterCriteria(FilterList.KEY_OWNERID, UIMonitor.getInstance().OwnerID);
				if (attributePage.IncludeAttributes.Count > 0)
					filter.AddFilterCriteria(FilterList.KEY_INCATTRS, attributePage.IncludeAttributes);
				else
					filter.RemoveCriteria(FilterList.KEY_INCATTRS);
				if (attributePage.ExcludeAttributes.Count > 0)
					filter.AddFilterCriteria(FilterList.KEY_EXCATTRS, attributePage.ExcludeAttributes);
				else
					filter.RemoveCriteria(FilterList.KEY_EXCATTRS);
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
				if (value.Contains(FilterList.KEY_PLACEBEFORE))
					datePage.PlaceBefore = (DateTime) value.GetCriteria(FilterList.KEY_PLACEBEFORE);
				if (value.Contains(FilterList.KEY_PLACEAFTER))
					datePage.PlaceAfter = (DateTime) value.GetCriteria(FilterList.KEY_PLACEAFTER);
				if (value.Contains(FilterList.KEY_INFOBEFORE))
					datePage.InfoBefore = (DateTime) value.GetCriteria(FilterList.KEY_INFOBEFORE);
				if (value.Contains(FilterList.KEY_INFOAFTER))
					datePage.InfoAfter = (DateTime) value.GetCriteria(FilterList.KEY_INFOAFTER);
				if (value.Contains(FilterList.KEY_COUNTRY))
					datePage.Country = value.GetCriteria(FilterList.KEY_COUNTRY) as string;
				if (value.Contains(FilterList.KEY_STATE))
					datePage.Province = value.GetCriteria(FilterList.KEY_STATE) as string;
				if (value.Contains(FilterList.KEY_FOUNDON))
					datePage.FoundOn = (DateTime) value.GetCriteria(FilterList.KEY_FOUNDON);
				if (value.Contains(FilterList.KEY_FOUNDBEFORE))
					datePage.FoundBefore = (DateTime) value.GetCriteria(FilterList.KEY_FOUNDBEFORE);
				if (value.Contains(FilterList.KEY_FOUNDAFTER))
					datePage.FoundAfter = (DateTime) value.GetCriteria(FilterList.KEY_FOUNDAFTER);
				if (value.Contains(FilterList.KEY_CHILDREN))
					childrenPage.ChildrenFilter = value.GetCriteria(FilterList.KEY_CHILDREN) as string;
				if (value.Contains(FilterList.KEY_NOCHILDREN))
					childrenPage.NoChildrenFilter = value.GetCriteria(FilterList.KEY_NOCHILDREN) as string;
				if (value.Contains(FilterList.KEY_NOTES))
					childrenPage.HasNotes = (Boolean) value.GetCriteria(FilterList.KEY_NOTES);
				if (value.Contains(FilterList.KEY_CORRECTED))
					childrenPage.HasCorrectedCoords = true;
				if (value.Contains(FilterList.KEY_NOCORRECTED))
					childrenPage.DoesNotHaveCorrectedCoords = true;
				if (value.Contains(FilterList.KEY_INCATTRS))
					attributePage.IncludeAttributes = (List<String>) value.GetCriteria(FilterList.KEY_INCATTRS);
				if (value.Contains(FilterList.KEY_EXCATTRS))
					attributePage.ExcludeAttributes = (List<String>) value.GetCriteria(FilterList.KEY_EXCATTRS);
 			}
		}
		
		private void GetNoChildrenFilter (FilterList filter)
		{
				if (childrenPage.NoChildrenFilter != null)
					filter.AddFilterCriteria(FilterList.KEY_NOCHILDREN, childrenPage.NoChildrenFilter);
				else
					filter.RemoveCriteria(FilterList.KEY_NOCHILDREN);
		}
		
		private void GetChildrenFilter (FilterList filter)
		{
			if (childrenPage.ChildrenFilter != null)
					filter.AddFilterCriteria(FilterList.KEY_CHILDREN, childrenPage.ChildrenFilter);
				else
					filter.RemoveCriteria(FilterList.KEY_CHILDREN);
		}
		
		private void HasNotesFilter (FilterList filter)
		{
			if (childrenPage.HasNotes)
					filter.AddFilterCriteria(FilterList.KEY_NOTES, childrenPage.HasNotes);
				else
					filter.RemoveCriteria(FilterList.KEY_NOTES);
		}
		
		private void GetFoundAfterFilter (FilterList filter)
		{
			if (datePage.FoundAfter != DateTime.MinValue)
					filter.AddFilterCriteria(FilterList.KEY_FOUNDAFTER, datePage.FoundAfter);
				else
					filter.RemoveCriteria(FilterList.KEY_FOUNDAFTER);
		}
		
		private void GetFoundBeforeFilter (FilterList filter)
		{
			if (datePage.FoundBefore != DateTime.MinValue)
					filter.AddFilterCriteria(FilterList.KEY_FOUNDBEFORE, datePage.FoundBefore);
				else 
					filter.RemoveCriteria(FilterList.KEY_FOUNDBEFORE);
		}
		
		private void GetFoundOnFilter (FilterList filter)
		{
			if (datePage.FoundOn != DateTime.MinValue)
					filter.AddFilterCriteria(FilterList.KEY_FOUNDON, datePage.FoundOn);
				else
					filter.RemoveCriteria(FilterList.KEY_FOUNDON);
		}
		
		private void GetDateFilter (FilterList filter)
		{
				if (datePage.PlaceBefore != DateTime.MinValue)
					filter.AddFilterCriteria(FilterList.KEY_PLACEBEFORE, datePage.PlaceBefore);
				else
					filter.RemoveCriteria(FilterList.KEY_PLACEBEFORE);
				if (datePage.PlaceAfter != DateTime.MinValue)
					filter.AddFilterCriteria(FilterList.KEY_PLACEAFTER, datePage.PlaceAfter);
				else
					filter.RemoveCriteria(FilterList.KEY_PLACEAFTER);
				if (datePage.InfoBefore != DateTime.MinValue)
					filter.AddFilterCriteria(FilterList.KEY_INFOBEFORE, datePage.InfoBefore);
				else
					filter.RemoveCriteria(FilterList.KEY_INFOBEFORE);
				if (datePage.InfoAfter != DateTime.MinValue)
					filter.AddFilterCriteria(FilterList.KEY_INFOAFTER, datePage.InfoAfter);
				else
					filter.RemoveCriteria(FilterList.KEY_INFOAFTER);
		}
		
		private void GetContainerFilter (FilterList filter)
		{
				if (null != contPage.ContainerTypes)
					filter.AddFilterCriteria(FilterList.KEY_CONTAINER, contPage.ContainerTypes);
				else
					filter.RemoveCriteria(FilterList.KEY_CONTAINER);
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
		
		protected virtual void OnDeleteClick (object o, Gtk.DeleteEventArgs args)
		{
			this.Respond (ResponseType.Cancel);
			this.Hide ();
		}
		
		
		
	}
}
