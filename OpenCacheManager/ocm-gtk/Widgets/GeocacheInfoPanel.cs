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
		private static Pixbuf EARTHICON = new Pixbuf("../icons/scalable/hicolor/earth.svg", 96, 96);
		private static Pixbuf CITOICON = new Pixbuf("../icons/scalable/hicolor/cito.svg", 96, 96);
		
		UIMonitor m_monitor;	
		WebView descriptionWidget;
		public GeocacheInfoPanel()
		{
			this.Build();
			m_monitor = UIMonitor.getInstance();
			setDifficulty(0);
			setTerrain(0);
		}
		
		public void updateCacheInfo()
		{
			Geocache cache = m_monitor.SelectedCache;
			if (descriptionWidget == null)
			{
				descriptionWidget = new WebView();
				longDescriptionScroll.Add(descriptionWidget);
			}
			cacheCodeLabel.Markup = START_BIG + cache.Name + END_BIG;
			cacheNameLabel.Markup = START_BIG + GLib.Markup.EscapeText(cache.CacheName) + END_BIG;
			descriptionWidget.LoadHtmlString(cache.LongDesc,"http://www.geocaching.com");
			setDifficulty(cache.Difficulty);
			setTerrain(cache.Terrain);
			setCacheIcon(cache.TypeOfCache);
			dateLabel.Text = cache.Time.ToShortDateString();
			hintLabel.Text = cache.Hint;
			setCacheType(cache.TypeOfCache);
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
		
		public void setTerrain(double diff)
		{
			
			terr_i1.Pixbuf = ESTAR_ICON;
			terr_i2.Pixbuf = ESTAR_ICON;
			terr_i3.Pixbuf = ESTAR_ICON;
			terr_i4.Pixbuf = ESTAR_ICON;
			terr_i5.Pixbuf = ESTAR_ICON;
			
			if (diff > 0.5 && diff < 1)
				terr_i1.Pixbuf = HSTAR_ICON;
			if (diff >= 1)
				terr_i1.Pixbuf = STAR_ICON;
			if (diff >= 1.5 && diff < 2)
				terr_i2.Pixbuf = HSTAR_ICON;
			if (diff >= 2)
				terr_i2.Pixbuf = STAR_ICON;
			if (diff >= 2.5 && diff < 3)
				terr_i3.Pixbuf = HSTAR_ICON;
			if (diff >= 3)
				terr_i3.Pixbuf = STAR_ICON;
			if (diff >= 3.5 && diff < 4)
				terr_i4.Pixbuf = HSTAR_ICON;
			if (diff >= 4)
				terr_i4.Pixbuf = STAR_ICON;
			if (diff >= 4.5 && diff < 5)
				terr_i5.Pixbuf = HSTAR_ICON;
			if (diff >= 5)
				terr_i5.Pixbuf = STAR_ICON;
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
				case Geocache.CacheType.CITO:
					cacheIcon.Pixbuf = CITOICON;
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
				co_ordinate += Catalog.GetString(String.Format("N {0}° {1}", lat.Degrees,lat.Minutes.ToString("0.000")));
			else
				co_ordinate += Catalog.GetString(String.Format("S {0}° {1}", lat.Degrees * -1,  lat.Minutes.ToString("0.000")));
			
			if (lon.Degrees > 0)
				co_ordinate += Catalog.GetString(String.Format("  E {0}° {1}", lon.Degrees, lon.Minutes.ToString("#.000")));
			else
				co_ordinate += Catalog.GetString(String.Format("  W {0}° {1}", lon.Degrees *-1 , lon.Minutes.ToString("#.000")));
			
			
			coordinateLabel.Markup = "<span font='bold 10'>" + co_ordinate + "</span>";
			
			
			double distance = Utilities.calculateDistance(m_monitor.HomeLat, cache.Lat, m_monitor.HomeLon, cache.Lon);
			double bearing = Utilities.calculateBearing(m_monitor.HomeLat, cache.Lat, m_monitor.HomeLon, cache.Lon);
			
			string bmarker = "N";
			System.Console.WriteLine(bearing);
			if (bearing > 22.5 && bearing <= 67.5)
				bmarker = "NE";
			else if (bearing > 67.5 && bearing <= 112.5)
				bmarker = "E";
			else if (bearing > 112.5 && bearing <= 157.5)
				bmarker = "SE";
			else if (bearing > 157.5 && bearing <= 202.5)
				bmarker = "S";
			else if (bearing > 202.5 && bearing <= 247.5)
				bmarker= "SW";
			else if (bearing > 247.5 && bearing <= 292.5)
				bmarker = "W";
			else if (bearing > 292.5 && bearing <= 337.5)
				bmarker = "NW";
				
			
			distance_label.Markup = Catalog.GetString(String.Format("<span font='bold italic 10'>({0} km {1} from your home coordinates)</span>", distance.ToString("0.00"), bmarker));
				
		}
		
		private void setCacheType(Geocache.CacheType ctype)
		{
			switch (ctype)
			{
				case Geocache.CacheType.APE:
					cacheTypeLabel.Text =  Catalog.GetString("Project A.P.E");
					break;
				case Geocache.CacheType.CITO:
					cacheTypeLabel.Text =  Catalog.GetString("Cache In Trash Out Event");
					break;
				case Geocache.CacheType.EARTH:
					cacheTypeLabel.Text =  Catalog.GetString("Earth Cache");
					break;
				case Geocache.CacheType.EVENT:
					cacheTypeLabel.Text =  Catalog.GetString("Event Cache");
					break;
				case Geocache.CacheType.LETTERBOX:
					cacheTypeLabel.Text =  Catalog.GetString("Letterbox Hybrid");
					break;
				case Geocache.CacheType.MAZE:
					cacheTypeLabel.Text =  Catalog.GetString("Geo Adventures Maze");
					break;
				case Geocache.CacheType.MEGAEVENT:
					cacheTypeLabel.Text =  Catalog.GetString("Mega Event");
					break;
				case Geocache.CacheType.MULTI:
					cacheTypeLabel.Text =  Catalog.GetString("Multi Cache");
					break;
				case Geocache.CacheType.MYSTERY:
					cacheTypeLabel.Text =  Catalog.GetString("Unknown Cache");
					break;
				case Geocache.CacheType.OTHER:
					cacheTypeLabel.Text =  Catalog.GetString("Undefined Cache Type");
					break;
				case Geocache.CacheType.REVERSE:
					cacheTypeLabel.Text =  Catalog.GetString("Locationless Cache");
					break;
				case Geocache.CacheType.TRADITIONAL:
					cacheTypeLabel.Text =  Catalog.GetString("Traditional Cache");
					break;
				case Geocache.CacheType.VIRTUAL:
					cacheTypeLabel.Text =  Catalog.GetString("Virtual Cache");
					break;
				case Geocache.CacheType.WEBCAM:
					cacheTypeLabel.Text =  Catalog.GetString("Webcam Cache");
					break;
				case Geocache.CacheType.WHERIGO:
					cacheTypeLabel.Text =  Catalog.GetString("Wherigo Cache");
					break;
				default:
					cacheTypeLabel.Text = "NOT_DEFINED";
					break;
			}
		}
	}
}
