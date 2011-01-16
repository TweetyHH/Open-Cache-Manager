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
using System.Collections.Generic;
using Gtk;
using Gdk;
using Mono.Unix;
using ocmengine;
using System.Text;

namespace ocmgtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class GeocacheInfoPanel : Gtk.Bin
	{
		static string START_BIG = "<span font='bold 12'>";
		static string END_BIG = "</span>";

		static string FOUND_DATE = Catalog.GetString("<span font='bold italic 10' fgcolor='darkgreen'>You have already found this cache on {0}</span>");
		static string FOUND = Catalog.GetString("<span font='bold italic 10' fgcolor='darkgreen'>You have already found this cache</span>");
		static string MINE = Catalog.GetString("<span font='bold italic 10' fgcolor='darkgreen'>You own this cache</span>");
		static string UNAVAILABLE = Catalog.GetString("<span font='bold italic 10' fgcolor='red'>This cache is temporarily unavailable, check the logs for more information.</span>");
		static string ARCHIVED = Catalog.GetString("<span font='bold italic 10' fgcolor='red'>This cache has been archived, check the logs for more information.</span>");
		static string CHECK_LOGS = Catalog.GetString("<span font='bold italic 10' fgcolor='darkorange'>This cache has a recent DNF or requires maintenance, check the logs for more information</span>");


		static Pixbuf STAR_ICON = new Pixbuf ("./icons/scalable/star.svg", 16, 16);
		static Pixbuf ESTAR_ICON = new Pixbuf ("./icons/scalable/star_empty.svg", 16, 16);
		static Pixbuf HSTAR_ICON = new Pixbuf ("./icons/scalable/halfstar.svg", 16, 16);
		private static Pixbuf TRADICON = new Pixbuf ("./icons/scalable/traditional.svg", 64, 64);
		private static Pixbuf LETTERICON = new Pixbuf ("./icons/scalable/letterbox.svg", 64, 64);
		private static Pixbuf MULTIICON = new Pixbuf ("./icons/scalable/multi.svg", 64, 64);
		private static Pixbuf MYSTERYICON = new Pixbuf ("./icons/scalable/unknown.svg", 64, 64);
		private static Pixbuf OTHERICON = new Pixbuf ("./icons/scalable/other.svg", 64, 64);
		private static Pixbuf EARTHICON = new Pixbuf ("./icons/scalable/earth.svg", 64, 64);
		private static Pixbuf CITOICON = new Pixbuf ("./icons/scalable/cito.svg", 64, 64);
		private static Pixbuf MEGAEVENT = new Pixbuf ("./icons/scalable/mega.svg", 64, 64);
		private static Pixbuf EVENT = new Pixbuf ("./icons/scalable/event.svg", 64, 64);
		private static Pixbuf WEBCAM = new Pixbuf ("./icons/scalable/webcam.svg", 64, 64);
		private static Pixbuf WHERIGO = new Pixbuf ("./icons/scalable/wherigo.svg", 64, 64);
		private static Pixbuf VIRTUAL = new Pixbuf ("./icons/scalable/virtual.svg", 64, 64);
		private static Pixbuf GENERIC = new Pixbuf ("./icons/scalable/treasure.svg", 64, 64);

		UIMonitor m_monitor;
		public GeocacheInfoPanel ()
		{
			this.Build ();
			m_monitor = UIMonitor.getInstance ();
			updateCacheInfo();
		}

		public void updateCacheInfo ()
		{
			try {
				Geocache cache = m_monitor.SelectedCache;
				if (cache == null)
				{
					this.Sensitive = false;
					cacheCodeLabel.Markup = START_BIG + Catalog.GetString("NO CACHE SELECTED") + END_BIG;
					cacheNameLabel.Markup = String.Empty;
					setDifficulty(0);
					setTerrain(0);
					dateLabel.Text = String.Empty;
					infoDateLabel.Text = string.Empty;
					statusLabel.Markup = String.Empty;
					placedByLabel.Text = String.Empty;
					cacheSizeLabel.Text = String.Empty;
					coordinateLabel.Text = String.Empty;
					distance_label.Text = String.Empty;
					cacheTypeLabel.Text = String.Empty;
					countryLabel.Text = String.Empty;
					attrLabel.Markup = Catalog.GetString("None");
					origCoord.Markup = String.Empty;
					return;
				}
				this.Sensitive = true;
				cacheCodeLabel.Markup = START_BIG + cache.Name + END_BIG;
				cacheNameLabel.Markup = START_BIG + GLib.Markup.EscapeText (cache.CacheName) + END_BIG;
				setDifficulty (cache.Difficulty);
				setTerrain (cache.Terrain);
				setCacheIcon (cache.TypeOfCache);
				dateLabel.Text = cache.Time.ToShortDateString ();
				infoDateLabel.Text = cache.Updated.ToShortDateString ();
				CacheStore store = Engine.getInstance().Store;
				DateTime lastDate = store.GetLastLogByYou(cache, m_monitor.OwnerID);
				if (lastDate == DateTime.MinValue)
					lastFoundDateLabel.Text = Catalog.GetString("Never");
				else
					lastFoundDateLabel.Text = lastDate.ToShortDateString();
				DateTime lastFound = store.GetLastFindByYou(cache, m_monitor.OwnerID);
				if (cache.Found && lastFound == DateTime.MinValue)
					statusLabel.Markup = FOUND; 
				else if (cache.Found)
					statusLabel.Markup = String.Format(FOUND_DATE, lastFound.ToShortDateString()); 
				else if (cache.Archived)
					statusLabel.Markup = ARCHIVED; 
				else if (!cache.Available)
					statusLabel.Markup = UNAVAILABLE;
				else if (cache.OwnerID == m_monitor.OwnerID || cache.CacheOwner == m_monitor.OwnerID)
					statusLabel.Markup = MINE;
				else if (cache.CheckNotes)
					statusLabel.Markup = CHECK_LOGS;
				else
					statusLabel.Markup = String.Empty;
				setCacheType (cache.TypeOfCache);
				placedByLabel.Text = cache.PlacedBy;
				cacheSizeLabel.Text = cache.Container;
			
				if (cache.State.Trim() != String.Empty)
				{
					countryLabel.Markup = String.Format(Catalog.GetString("<b>Location: </b> {0},{1}"), cache.State, cache.Country);
				}
				else if (cache.Country.Trim() != String.Empty)
				{
					countryLabel.Markup = String.Format(Catalog.GetString("<b>Location: </b> {0}"), cache.Country);
				}
				else
				{
					countryLabel.Text = String.Empty;
				}
				
				setCoordinate (cache);
				
				List<CacheAttribute> attrs = Engine.getInstance().Store.GetAttributes(cache.Name);
				StringBuilder bldr = new StringBuilder();
				if (attrs.Count <= 0)
					bldr.Append(Catalog.GetString("None"));
				bool isFirst = true;
				foreach (Gtk.Widget child in attrTable.Children)
				{
						attrTable.Remove(child);
				}
				Gtk.Table.TableChild props;
				
				uint colCount = 0;
				
				foreach (CacheAttribute attr in attrs)
				{
					Pixbuf buf;
					if (attr.Include)
						buf = IconManager.GetYAttrIcon(attr.AttrValue);
					else
						buf = IconManager.GetNAttrIcon(attr.AttrValue);
					if (buf != null)
					{
						Gtk.Image img = new Gtk.Image();
						img.Pixbuf = buf;
						img.TooltipText = Catalog.GetString(attr.AttrValue);
						attrTable.Add(img);
						props = ((Gtk.Table.TableChild)(this.attrTable[img]));
						props.TopAttach = 0;
						props.LeftAttach = colCount;
						props.RightAttach = colCount + 1;
						props.BottomAttach = 1;
						props.XOptions = AttachOptions.Shrink;
						img.Show();
						colCount++;
						continue;
					}
					
					if (isFirst)
						isFirst = false;
					else
						bldr.Append(", ");					
					if (!attr.Include)
					{
						bldr.Append("<span fgcolor='red' strikethrough='true'>");
						bldr.Append(attr.AttrValue);
						bldr.Append("</span>");
					}
					else
					{
						bldr.Append(attr.AttrValue);
					}
				}
				Label filler = new Label("");
				attrTable.Add(filler);
				props = ((Gtk.Table.TableChild)(this.attrTable[filler]));
				props.TopAttach = 0;
				props.LeftAttach = colCount;
				props.RightAttach = colCount + 1;
				props.BottomAttach = 1;
				props.XOptions = AttachOptions.Expand;
				filler.Show();
				
				attrTable.Add(attrLabel);
				props = ((Gtk.Table.TableChild)(this.attrTable[attrLabel]));
				props.TopAttach = 1;
				props.LeftAttach = 0;
				props.RightAttach = colCount + 1;
				props.BottomAttach = 2;
				attrLabel.Markup = bldr.ToString();
				attrLabel.Show();
			} catch (Exception e) {
				UIMonitor.ShowException(e);
			}
		}

		public void setDifficulty (double diff)
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
		}

		public void setTerrain (double diff)
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
		}

		public void setCacheIcon (Geocache.CacheType type)
		{
			switch (type) {
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
				case Geocache.CacheType.EVENT:
					cacheIcon.Pixbuf = EVENT;
					break;
				case Geocache.CacheType.MEGAEVENT:
					cacheIcon.Pixbuf = MEGAEVENT;
					break;
				case Geocache.CacheType.VIRTUAL:
					cacheIcon.Pixbuf = VIRTUAL;
					break;
				case Geocache.CacheType.WEBCAM:
					cacheIcon.Pixbuf = WEBCAM;
					break;
				case Geocache.CacheType.WHERIGO:
					cacheIcon.Pixbuf = WHERIGO;
					break;
				case Geocache.CacheType.GENERIC:
					cacheIcon.Pixbuf = GENERIC;
					break;
				default:
					cacheIcon.Pixbuf = OTHERICON;
					break;
			}
		}

		public void setCoordinate (Geocache cache)
		{
			
			
			coordinateLabel.Markup  = "<span font='bold 10'>" + Utilities.getCoordString (cache.Lat, cache.Lon) + "</span>";
			if (cache.HasCorrected)
				origCoord.Markup =  Catalog.GetString("<i>Original: ") +  Utilities.getCoordString(cache.OrigLat, cache.OrigLon) + "</i>";
			else 
				origCoord.Markup = String.Empty;;
			
			
			double distance = Utilities.calculateDistance (m_monitor.CentreLat, cache.Lat, m_monitor.CentreLon, cache.Lon);
			double bearing = Utilities.calculateBearing (m_monitor.CentreLat, cache.Lat, m_monitor.CentreLon, cache.Lon);
			
			string bmarker = Catalog.GetString("N");
			if (bearing > 22.5 && bearing <= 67.5)
				bmarker = Catalog.GetString("NE"); else if (bearing > 67.5 && bearing <= 112.5)
				bmarker = Catalog.GetString("E"); else if (bearing > 112.5 && bearing <= 157.5)
				bmarker = Catalog.GetString("SE"); else if (bearing > 157.5 && bearing <= 202.5)
				bmarker = Catalog.GetString("S"); else if (bearing > 202.5 && bearing <= 247.5)
				bmarker = Catalog.GetString("SW"); else if (bearing > 247.5 && bearing <= 292.5)
				bmarker = Catalog.GetString("W"); else if (bearing > 292.5 && bearing <= 337.5)
				bmarker = Catalog.GetString("NW");
			
			
			if (m_monitor.UseImperial)
			{
				distance = Utilities.KmToMiles(distance);
				distance_label.Markup = Catalog.GetString (String.Format (Catalog.GetString("<span font='bold italic 10'>({0} miles {1} from your home coordinates)</span>"), distance.ToString ("0.00"), bmarker));
			}
			else
			{
				distance_label.Markup = Catalog.GetString (String.Format (Catalog.GetString("<span font='bold italic 10'>({0} km {1} from your home coordinates)</span>"), distance.ToString ("0.00"), bmarker));
			}
		}

		private void setCacheType (Geocache.CacheType ctype)
		{
			switch (ctype) {
				case Geocache.CacheType.APE:
					cacheTypeLabel.Text = Catalog.GetString ("Project A.P.E");
					break;
				case Geocache.CacheType.CITO:
					cacheTypeLabel.Text = Catalog.GetString ("Cache In Trash Out Event");
					break;
				case Geocache.CacheType.EARTH:
					cacheTypeLabel.Text = Catalog.GetString ("Earth Cache");
					break;
				case Geocache.CacheType.EVENT:
					cacheTypeLabel.Text = Catalog.GetString ("Event Cache");
					break;
				case Geocache.CacheType.LETTERBOX:
					cacheTypeLabel.Text = Catalog.GetString ("Letterbox Hybrid");
					break;
				case Geocache.CacheType.MAZE:
					cacheTypeLabel.Text = Catalog.GetString ("Geo Adventures Maze");
					break;
				case Geocache.CacheType.MEGAEVENT:
					cacheTypeLabel.Text = Catalog.GetString ("Mega Event");
					break;
				case Geocache.CacheType.MULTI:
					cacheTypeLabel.Text = Catalog.GetString ("Multi Cache");
					break;
				case Geocache.CacheType.MYSTERY:
					cacheTypeLabel.Text = Catalog.GetString ("Unknown Cache");
					break;
				case Geocache.CacheType.OTHER:
					cacheTypeLabel.Text = Catalog.GetString ("Undefined Cache Type");
					break;
				case Geocache.CacheType.REVERSE:
					cacheTypeLabel.Text = Catalog.GetString ("Locationless Cache");
					break;
				case Geocache.CacheType.TRADITIONAL:
					cacheTypeLabel.Text = Catalog.GetString ("Traditional Cache");
					break;
				case Geocache.CacheType.VIRTUAL:
					cacheTypeLabel.Text = Catalog.GetString ("Virtual Cache");
					break;
				case Geocache.CacheType.WEBCAM:
					cacheTypeLabel.Text = Catalog.GetString ("Webcam Cache");
					break;
				case Geocache.CacheType.WHERIGO:
					cacheTypeLabel.Text = Catalog.GetString ("Wherigo Cache");
					break;
				case Geocache.CacheType.GENERIC:
					cacheTypeLabel.Text = Catalog.GetString("Geocache");
					break;
				default:
					cacheTypeLabel.Text = "NOT_DEFINED";
					break;
			}
		}
		protected virtual void OnClickView (object sender, System.EventArgs e)
		{
			System.Diagnostics.Process.Start (m_monitor.SelectedCache.URL.ToString ());
		}

		protected virtual void OnClickLog (object sender, System.EventArgs e)
		{
			UIMonitor.getInstance().LogFind();
		}
		
		
		
	}
}
