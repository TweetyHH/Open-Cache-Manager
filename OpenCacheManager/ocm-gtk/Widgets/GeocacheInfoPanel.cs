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
using Gdk;
using Mono.Unix;
using WebKit;
using ocmengine;
namespace ocmgtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class GeocacheInfoPanel : Gtk.Bin
	{
		static string START_BIG = "<span font='bold 14'>";
		static string END_BIG = "</span>";
		
		static Pixbuf STAR_ICON = new Pixbuf("../icons/scalable/hicolor/star.svg",16,16);
		static Pixbuf ESTAR_ICON = new Pixbuf("../icons/scalable/hicolor/star_empty.svg", 16, 16);
		static Pixbuf HSTAR_ICON = new Pixbuf("../icons/scalable/hicolor/halfstar.svg",16,16);
		private static Pixbuf TRADICON = new Pixbuf("../icons/scalable/hicolor/traditional.svg", 96, 96);
		private static Pixbuf LETTERICON = new Pixbuf("../icons/scalable/hicolor/letterbox.svg", 96, 96);
		private static Pixbuf MULTIICON = new Pixbuf("../icons/scalable/hicolor/multi.svg", 96, 96);
		private static Pixbuf MYSTERYICON = new Pixbuf("../icons/scalable/hicolor/unknown.svg", 96, 96);
		private static Pixbuf OTHERICON = new Pixbuf("../icons/scalable/hicolor/other.svg", 96, 96);
		private static Pixbuf FOUNDICON = new Pixbuf("../icons/scalable/hicolor/found.svg", 96, 96);
		private static Pixbuf EARTHICON = new Pixbuf("../icons/scalable/hicolor/earth.svg", 96, 96);
	
		WebView descriptionWidget;
		public GeocacheInfoPanel()
		{
			this.Build();
			descriptionWidget = new WebView();
			longDescriptionScroll.Add(descriptionWidget);
			setDifficulty(0);
		}
		
		public void setCacheInfo(Geocache cache)
		{
			cacheCodeLabel.Markup = START_BIG + cache.Name + END_BIG;
			cacheNameLabel.Markup = START_BIG + cache.CacheName + END_BIG;
			descriptionWidget.LoadHtmlString(cache.LongDesc,"http://www.geocaching.com");
			setDifficulty(cache.Difficulty);
			setCacheIcon(cache.TypeOfCache);
			dateLabel.Text = cache.Time.ToShortDateString();
			hintLabel.Text = cache.Hint;
			cacheTypeLabel.Text = cache.TypeOfCache.ToString();
			placedByLabel.Text = cache.PlacedBy;
			cacheSizeLabel.Text = cache.Container;
			setCoordinate(cache);
			this.ShowAll();
		}
		
		public void setDifficulty(double diff)
		{
			
			diff_i1.Pixbuf = ESTAR_ICON;
			diff_i2.Pixbuf = ESTAR_ICON;
			diff_i3.Pixbuf = ESTAR_ICON;
			diff_i4.Pixbuf = ESTAR_ICON;
			diff_i5.Pixbuf = ESTAR_ICON;
			
			if (diff > 0.5 && diff < 1)
				diff_i1.Pixbuf = HSTAR_ICON;
			if (diff >= 1)
				diff_i1.Pixbuf = STAR_ICON;
			if (diff >= 1.5 && diff < 2)
				diff_i2.Pixbuf = HSTAR_ICON;
			if (diff >= 2)
				diff_i2.Pixbuf = STAR_ICON;
			if (diff >= 2.5 && diff < 3)
				diff_i3.Pixbuf = HSTAR_ICON;
			if (diff >= 3)
				diff_i3.Pixbuf = STAR_ICON;
			if (diff >= 3.5 && diff < 4)
				diff_i4.Pixbuf = HSTAR_ICON;
			if (diff >= 4)
				diff_i4.Pixbuf = STAR_ICON;
			if (diff >= 4.5 && diff < 5)
				diff_i5.Pixbuf = HSTAR_ICON;
			if (diff >= 5)
				diff_i5.Pixbuf = STAR_ICON;
			this.ShowAll();
		}
		
		public void setCacheIcon(Geocache.CacheType type)
		{
			switch (type)
			{
				case Geocache.CacheType.TRADITIONAL:
					cacheIcon.Pixbuf = TRADICON;
					break;
				case Geocache.CacheType.MYSTERY:
					cacheIcon.Pixbuf = MYSTERYICON;
					break;
				case Geocache.CacheType.MULTI:
					cacheIcon.Pixbuf = MULTIICON;
					break;
				case Geocache.CacheType.LETTERBOX:
					cacheIcon.Pixbuf = LETTERICON;
					break;
				case Geocache.CacheType.EARTH:
					cacheIcon.Pixbuf = EARTHICON;
					break;
				default:
					cacheIcon.Pixbuf = OTHERICON;
					break;
			}
		}
		
		public void setCoordinate(Geocache cache)
		{
			DegreeMinutes lat = Utilities.convertDDtoDM(cache.Lat);
			DegreeMinutes lon = Utilities.convertDDtoDM(cache.Lon);
			
			String co_ordinate = "";
			
			if (lat.Degrees > 0)
				co_ordinate += Catalog.GetString(String.Format("N {0}° {1}", lat.Degrees,Math.Round(lat.Minutes, 3)));
			else
				co_ordinate += Catalog.GetString(String.Format("S {0}° {1}", lat.Degrees,Math.Round(lat.Minutes, 3)));
			
			if (lon.Degrees > 0)
				co_ordinate += Catalog.GetString(String.Format("  E {0}° {1}", lon.Degrees,Math.Round(lon.Minutes, 3)));
			else
				co_ordinate += Catalog.GetString(String.Format("  W {0}° {1}", lon.Degrees,Math.Round(lon.Minutes, 3)));
			
			coordinateLabel.Markup = "<span font='bold 12'>" + co_ordinate + "</span>";
				
		}
	}
}
