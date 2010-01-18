/*
 Copyright 2009 Kyle Campbell
 Licensed under the Apache License, Version 2.0 (the "License"); 
 you may not use this file except in compliance with the License. 
 You may obtain a copy of the License at 
 
 		http://www.apache.org/licenses/LICENSE-2.0 
 
 Unless required by applicable law or agreed to in writing, software distributed under the License 
 is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or 
 implied. See the License for the specific language governing permissions and limitations under the License. 
*/
using System;
using System.IO;
using System.Collections;
using Gtk;
using ocmengine;

namespace ocmgtk
{
	
	
	[System.ComponentModel.ToolboxItem(true)]
	public partial class GeocacheInfoPanel : Gtk.Bin
	{
		HTML descriptionWidget;
		
		public GeocacheInfoPanel()
		{
			this.Build();
			descriptionWidget = new HTML();
			descriptionWidget.LoadEmpty();
			longDescriptionScroll.Add(descriptionWidget);
			
		}
		
		public void setCacheInfo(Geocache cache)
		{
			cacheCodeLabel.Text = cache.Name;
			cacheNameLabel.Text = cache.CacheName;
			cacheDifficultyLabel1.Text = cache.Difficulty.ToString();
			cacheTerrainLabel.Text = cache.Terrain.ToString();
			shortDescriptionLabel.Text = cache.ShortDesc;
			descriptionWidget.LoadFromString(cache.LongDesc);
			dateLabel.Text = cache.Time.ToShortDateString();
			hintLabel.Text = cache.Hint;
			cacheTypeLabel.Text = cache.TypeOfCache.ToString();
			placedByLabel.Text = cache.PlacedBy;
			cacheSizeLabel.Text = cache.Container;
		}
	}
}
