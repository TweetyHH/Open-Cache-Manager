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
using Mono.Unix;

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
				GetKeyWordFilter (filter);
				GetContainerFilter (filter);
				GetDateFilter (filter);
				GetCountryFilter (filter);
				GetStateFilter (filter);
				GetFoundOnFilter (filter);
				GetFoundBeforeFilter (filter);
				GetFoundAfterFilter (filter);
				HasNotesFilter (filter);
				GetChildrenFilter (filter);
				GetNoChildrenFilter (filter);
				GetCorrectedCoordsFilter (filter);
				GetNoCorrectedCoordsFilter (filter);
				GetMustHaveAttributes (filter);
				GetMustNotHaveSetAttributesFilter (filter);
				GetMarkedFilter (filter);	
				GetStatusFilter (filter);
				filter.AddFilterCriteria(FilterList.KEY_OWNERID, UIMonitor.getInstance().OwnerID);
				return filter;
			}
			set {
				if (value == null) {
					return;
				}
				SetDiffTerTabFilters (value);
				SetContainerTabFilters (value);
				SetDateTabFilters (value);
				SetChildrenTabFilters (value);
				SetAttributeTabFilters (value);
 			}
		}
		
		private void GetStatusFilter (FilterList filter)
		{
			if (contPage.Status != null)
			{
				filter.AddFilterCriteria(FilterList.KEY_STATUS, contPage.Status);
			}
			else
			{
				filter.RemoveCriteria(FilterList.KEY_STATUS);
			}
		}
		
		private void GetMarkedFilter (FilterList filter)
		{
			if (contPage.hasDNF)
			{
				filter.AddFilterCriteria(FilterList.KEY_DNF, true);
				filter.RemoveCriteria(FilterList.KEY_FTF);
			}
			else if (contPage.hasNoDNF)
			{
				filter.AddFilterCriteria(FilterList.KEY_DNF, false);
				filter.RemoveCriteria(FilterList.KEY_FTF);
			}
			else if (contPage.hasFTF)
			{
				filter.AddFilterCriteria(FilterList.KEY_FTF, true);
				filter.RemoveCriteria(FilterList.KEY_DNF);
			}
			else if (contPage.hasNoFTF)
			{
				filter.AddFilterCriteria(FilterList.KEY_FTF, false);
				filter.RemoveCriteria(FilterList.KEY_DNF);
			}
			else
			{
				filter.RemoveCriteria(FilterList.KEY_DNF);
				filter.RemoveCriteria(FilterList.KEY_FTF);
			}
		}
		
		private void SetAttributeTabFilters (FilterList list)
		{
			bool atLeastOne = false;
			if (list.Contains(FilterList.KEY_INCATTRS))
			{
				atLeastOne = true;
				attributePage.MustHaveIncludeAttributes = (List<String>) list.GetCriteria(FilterList.KEY_INCATTRS);
			}
			if (list.Contains(FilterList.KEY_EXCATTRS))
			{
				atLeastOne = true;
				attributePage.MustHaveNegAttributes = (List<String>) list.GetCriteria(FilterList.KEY_EXCATTRS);
			}
			if (list.Contains(FilterList.KEY_INCNOATTRS))
			{
				atLeastOne = true;
				attributePage.MustNotHaveIncludeAttributes = (List<String>) list.GetCriteria(FilterList.KEY_INCNOATTRS);
			}
			if (list.Contains(FilterList.KEY_EXCNOATTRS))
			{
				atLeastOne = true;
				attributePage.MustNotHaveNegAttributes = (List<String>) list.GetCriteria(FilterList.KEY_EXCNOATTRS);
			}
			if (atLeastOne)
			{
				attrPageLabel.Markup = "<b>" + Catalog.GetString("Attributes") + "</b>";
			}
		}
		
		private void SetChildrenTabFilters (FilterList list)
		{
			bool atLeastOne = false;
			if (list.Contains(FilterList.KEY_CHILDREN))
			{
				childrenPage.ChildrenFilter = list.GetCriteria(FilterList.KEY_CHILDREN) as string;
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_NOCHILDREN))
			{
				childrenPage.NoChildrenFilter = list.GetCriteria(FilterList.KEY_NOCHILDREN) as string;
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_NOTES))
			{
				childrenPage.HasNotes = (Boolean) list.GetCriteria(FilterList.KEY_NOTES);
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_CORRECTED))
			{
				childrenPage.HasCorrectedCoords = true;
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_NOCORRECTED))
			{
				childrenPage.DoesNotHaveCorrectedCoords = true;
				atLeastOne = true;
			}
			if (atLeastOne)
			{
				labelChildren.Markup = "<b>" + Catalog.GetString("Notes/Children/Corrected") + "</b>";
			}
		}
		
		private void SetDateTabFilters (FilterList list)
		{
			bool atLeastOne = false;
			if (list.Contains(FilterList.KEY_COUNTRY))
			{
				placementPage.Country = list.GetCriteria(FilterList.KEY_COUNTRY) as string;
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_STATE))
			{
				placementPage.Province = list.GetCriteria(FilterList.KEY_STATE) as string;
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_PLACEBEFORE))
			{
				placementPage.PlaceBefore = (DateTime) list.GetCriteria(FilterList.KEY_PLACEBEFORE);
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_PLACEAFTER))
			{
				placementPage.PlaceAfter = (DateTime) list.GetCriteria(FilterList.KEY_PLACEAFTER);
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_INFOBEFORE))
			{
				updatedPage.InfoBefore = (DateTime) list.GetCriteria(FilterList.KEY_INFOBEFORE);
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_INFOAFTER))
			{
				updatedPage.InfoAfter = (DateTime) list.GetCriteria(FilterList.KEY_INFOAFTER);
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_FOUNDON))
			{
				updatedPage.FoundOn = (DateTime) list.GetCriteria(FilterList.KEY_FOUNDON);
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_FOUNDBEFORE))
			{
				updatedPage.FoundBefore = (DateTime) list.GetCriteria(FilterList.KEY_FOUNDBEFORE);
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_FOUNDAFTER))
			{
				updatedPage.FoundAfter = (DateTime) list.GetCriteria(FilterList.KEY_FOUNDAFTER);
				atLeastOne = true;
			}
			if (atLeastOne)
				dateLabel.Markup = "<b>" + Catalog.GetString("Dates/Location") + "</b>";
			if (list.Contains(FilterList.KEY_PLACEDBY))
			{
				placementPage.PlacedBy = list.GetCriteria(FilterList.KEY_PLACEDBY) as string;
				atLeastOne = true;
			}
		}
		
		private void SetContainerTabFilters (FilterList list)
		{
			bool atLeastOne = false;
			if (list.Contains(FilterList.KEY_CONTAINER))
			{
				contPage.ContainerTypes = list.GetCriteria(FilterList.KEY_CONTAINER) as List<string>;
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_FTF))
			{
				bool ftf = (bool) list.GetCriteria(FilterList.KEY_FTF);
				if (ftf)
					contPage.hasFTF = true;
				else
					contPage.hasNoFTF = true;
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_DNF))
			{
				bool ftf = (bool) list.GetCriteria(FilterList.KEY_DNF);
				if (ftf)
					contPage.hasDNF = true;
				else
					contPage.hasNoDNF = true;
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_DESCRIPTION))
			{
				contPage.DescriptionKeyWords = list.GetCriteria(FilterList.KEY_DESCRIPTION) as string;
				atLeastOne = true;
			}
			if (list.Contains(FilterList.KEY_STATUS))
			{
				contPage.Status = list.GetCriteria(FilterList.KEY_STATUS) as bool[];
				atLeastOne = true;
			}
			if (atLeastOne)
				contLabel.Markup = "<b>" + Catalog.GetString("Container/Description/Status") + "</b>";
			
		}
		
		private void SetDiffTerTabFilters (FilterList list)
		{
			bool atLeastOne = false;
			if (list.Contains(FilterList.KEY_TERRAIN_VAL))
			{
				atLeastOne = true;
				difficultyPage.TerrValue = list.GetCriteria (FilterList.KEY_TERRAIN_VAL) as string;
				difficultyPage.TerrOperator = list.GetCriteria (FilterList.KEY_TERRAIN_OP) as string;
			}
			if (list.Contains(FilterList.KEY_DIFF_VAL))
			{
				atLeastOne = true;
				difficultyPage.DifficultyValue = list.GetCriteria (FilterList.KEY_DIFF_VAL) as string;
				difficultyPage.DifficultyOperator = list.GetCriteria (FilterList.KEY_DIFF_OP) as string;
			}
			if (list.Contains(FilterList.KEY_CACHETYPE))
			{
				atLeastOne = true;
				difficultyPage.SelectedCacheTypes = list.GetCriteria(FilterList.KEY_CACHETYPE) as List<string>;
			}
			if (atLeastOne)
			{
				diffLabel.Markup = "<b>" + Catalog.GetString("Difficulty/Terrain/Type") + "</b>";
			}
		}
		
		private void GetMustNotHaveSetAttributesFilter (FilterList filter)
		{
			if (attributePage.MustNotHaveIncludeAttributes.Count > 0)
					filter.AddFilterCriteria(FilterList.KEY_INCNOATTRS, attributePage.MustNotHaveIncludeAttributes);
				else
					filter.RemoveCriteria(FilterList.KEY_INCNOATTRS);	
			if (attributePage.MustNotHaveNegAttributes.Count > 0)
					filter.AddFilterCriteria(FilterList.KEY_EXCNOATTRS, attributePage.MustNotHaveNegAttributes);
				else
					filter.RemoveCriteria(FilterList.KEY_EXCNOATTRS);
		}
		
		private void GetNoCorrectedCoordsFilter (FilterList filter)
		{
			if (childrenPage.DoesNotHaveCorrectedCoords)
					filter.AddFilterCriteria(FilterList.KEY_NOCORRECTED, true);
				else
					filter.RemoveCriteria(FilterList.KEY_NOCORRECTED);
		}
		
		private void GetCorrectedCoordsFilter (FilterList filter)
		{
			if (childrenPage.HasCorrectedCoords)
					filter.AddFilterCriteria(FilterList.KEY_CORRECTED, true);
				else
					filter.RemoveCriteria(FilterList.KEY_CORRECTED);
		}
		
		private void GetStateFilter (FilterList filter)
		{
			string state = placementPage.Province;
				if (!String.IsNullOrEmpty(state))
					filter.AddFilterCriteria(FilterList.KEY_STATE, placementPage.Province);
		}
		
		private void GetCountryFilter (FilterList filter)
		{
			string cntry = placementPage.Country;
				if (!String.IsNullOrEmpty(cntry))
					filter.AddFilterCriteria(FilterList.KEY_COUNTRY, placementPage.Country);
		}
		
		private void GetMustHaveAttributes (FilterList filter)
		{
							if (attributePage.MustHaveIncludeAttributes.Count > 0)
					filter.AddFilterCriteria(FilterList.KEY_INCATTRS, attributePage.MustHaveIncludeAttributes);
				else
					filter.RemoveCriteria(FilterList.KEY_INCATTRS);
				if (attributePage.MustHaveNegAttributes.Count > 0)
					filter.AddFilterCriteria(FilterList.KEY_EXCATTRS, attributePage.MustHaveNegAttributes);
				else
					filter.RemoveCriteria(FilterList.KEY_EXCATTRS);
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
			if (updatedPage.FoundAfter != DateTime.MinValue)
					filter.AddFilterCriteria(FilterList.KEY_FOUNDAFTER, updatedPage.FoundAfter);
				else
					filter.RemoveCriteria(FilterList.KEY_FOUNDAFTER);
		}
		
		private void GetFoundBeforeFilter (FilterList filter)
		{
			if (updatedPage.FoundBefore != DateTime.MinValue)
					filter.AddFilterCriteria(FilterList.KEY_FOUNDBEFORE, updatedPage.FoundBefore);
				else 
					filter.RemoveCriteria(FilterList.KEY_FOUNDBEFORE);
		}
		
		private void GetFoundOnFilter (FilterList filter)
		{
			if (updatedPage.FoundOn != DateTime.MinValue)
					filter.AddFilterCriteria(FilterList.KEY_FOUNDON, updatedPage.FoundOn);
				else
					filter.RemoveCriteria(FilterList.KEY_FOUNDON);
		}
		
		private void GetDateFilter (FilterList filter)
		{
				if (placementPage.PlaceBefore != DateTime.MinValue)
					filter.AddFilterCriteria(FilterList.KEY_PLACEBEFORE, placementPage.PlaceBefore);
				else
					filter.RemoveCriteria(FilterList.KEY_PLACEBEFORE);
				if (placementPage.PlaceAfter != DateTime.MinValue)
					filter.AddFilterCriteria(FilterList.KEY_PLACEAFTER, placementPage.PlaceAfter);
				else
					filter.RemoveCriteria(FilterList.KEY_PLACEAFTER);
				if (updatedPage.InfoBefore != DateTime.MinValue)
					filter.AddFilterCriteria(FilterList.KEY_INFOBEFORE, updatedPage.InfoBefore);
				else
					filter.RemoveCriteria(FilterList.KEY_INFOBEFORE);
				if (updatedPage.InfoAfter != DateTime.MinValue)
					filter.AddFilterCriteria(FilterList.KEY_INFOAFTER, updatedPage.InfoAfter);
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
		
		private void GetPlacedByFilter (FilterList filter)
		{
			String placedby = placementPage.PlacedBy;
				if (null != placedby)
					filter.AddFilterCriteria(FilterList.KEY_PLACEDBY, placedby);
				else
					filter.RemoveCriteria(FilterList.KEY_PLACEDBY);
		}

		private void GetCTypeFilter (FilterList filter)
		{
			if (null != difficultyPage.SelectedCacheTypes) 
				filter.AddFilterCriteria (FilterList.KEY_CACHETYPE, difficultyPage.SelectedCacheTypes);
			else
				filter.RemoveCriteria(FilterList.KEY_CACHETYPE);
		}

		private void GetDifficultyFilter (FilterList filter)
		{
			if (!String.IsNullOrEmpty (difficultyPage.DifficultyValue)) {
				filter.AddFilterCriteria (FilterList.KEY_DIFF_VAL, difficultyPage.DifficultyValue);
				filter.AddFilterCriteria (FilterList.KEY_DIFF_OP, difficultyPage.DifficultyOperator);
			}
			else
			{
				filter.RemoveCriteria(FilterList.KEY_DIFF_OP);
				filter.RemoveCriteria(FilterList.KEY_DIFF_VAL);
			}
		}

		private void GetTerrainFilter (FilterList filter)
		{
			if (!String.IsNullOrEmpty (difficultyPage.TerrValue)) {
				filter.AddFilterCriteria (FilterList.KEY_TERRAIN_VAL, difficultyPage.TerrValue);
				filter.AddFilterCriteria (FilterList.KEY_TERRAIN_OP, difficultyPage.TerrOperator);
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
