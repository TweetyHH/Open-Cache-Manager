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
using Gdk;
using Mono.Unix;
using Gtk;
using ocmengine;

namespace ocmgtk
{


	public class UIMonitor
	{
		private static UIMonitor m_instance = null;
		private GeoCachePane m_pane = null;
		private Statusbar m_statusbar = null;
		private static Pixbuf TRADICON_S = new Pixbuf ("./icons/scalable/traditional.svg", 24, 24);
		private static Pixbuf LETTERICON_S = new Pixbuf ("./icons/scalable/letterbox.svg", 24, 24);
		private static Pixbuf MULTIICON_S = new Pixbuf ("./icons/scalable/multi.svg", 24, 24);
		private static Pixbuf MYSTERYICON_S = new Pixbuf ("./icons/scalable/unknown.svg", 24, 24);
		private static Pixbuf OTHERICON_S = new Pixbuf ("./icons/scalable/other.svg", 24, 24);
		private static Pixbuf FOUNDICON_S = new Pixbuf ("./icons/scalable/found.svg", 24, 24);
		private static Pixbuf EARTHICON_S = new Pixbuf ("./icons/scalable/earth.svg", 24, 24);
		private static Pixbuf CITOICON_S = new Pixbuf ("./icons/scalable/cito.svg", 24, 24);
		private static Pixbuf MEGAEVENT_S = new Pixbuf ("./icons/scalable/mega.svg", 24, 24);
		private static Pixbuf EVENT_S = new Pixbuf ("./icons/scalable/event.svg", 24, 24);
		private static Pixbuf WEBCAM_S = new Pixbuf ("./icons/scalable/webcam.svg", 24, 24);
		private static Pixbuf WHERIGO_S = new Pixbuf ("./icons/scalable/wherigo.svg", 24, 24);
		private static Pixbuf VIRTUAL_S = new Pixbuf ("./icons/scalable/virtual.svg", 24, 24);
		private static Pixbuf OWNED_S = new Pixbuf ("./icons/scalable/star.svg", 24, 24);

		private static string TRAD_MI = "traditional.png";
		private static string CITO_MI = "cito.png";
		private static string EARH_MI = "earth.png";
		private static string LETTRBOX_MI = "letterbox.png";
		private static string EVENT_MI = "mega.png";
		private static string MEGA_MI = "mega.png";
		private static string MULTI_MI = "multi.png";
		private static string OTHER_MI = "other.png";
		private static string OWNED_MI = "owned.png";
		private static string UNKNOWN_MI = "unknown.png";
		private static string VIRTUAL_MI = "virtual.png";
		private static string WAYPOINT_MI = "waypoint-flag-red.png";
		private static string WEBCAM_MI = "webcam.png";
		private static string WHERIGO_MI = "wherigo.png";
		
		
		
		private double m_home_lat = 46.49;
		private double m_home_lon = -81.01;
		private String m_ownerid = "1918539";
		private CacheList m_cachelist;
		private Geocache m_selectedCache;
		private MainWindow m_mainWin;
		private BrowserWidget m_map;

		public GeoCachePane GeoPane {
			get { return m_pane; }
			set { m_pane = value; }
		}
		
		public String OwnerID
		{
			get {return m_ownerid;}
			set { m_ownerid=value;}
		}

		public Geocache SelectedCache {
			get { return m_selectedCache; }
		}

		public MainWindow Main {
			get { return m_mainWin; }
			set { m_mainWin = value; }
		}

		public CacheList CacheListPane {
			get { return m_cachelist; }
			set { m_cachelist = value; }
		}

		public Statusbar StatusBar {
			get { return m_statusbar; }
			set { m_statusbar = value; }
		}

		public BrowserWidget Map {
			get { return m_map; }
			set { m_map = value; }
		}


		public double HomeLat {
			get { return m_home_lat; }
			set { m_home_lat = value; }
		}

		public double HomeLon {
			get { return m_home_lon; }
			set { m_home_lon = value; }
		}


		private UIMonitor ()
		{
		}

		public static UIMonitor getInstance ()
		{
			lock (typeof(UIMonitor)) {
				if (null == m_instance)
					m_instance = new UIMonitor ();
				return m_instance;
			}
		}

		public void updateCaches ()
		{
			m_cachelist.PopulateList ();
			UpdateCacheCountStatus ();
		}

		public void setSelectedCache (Geocache cache)
		{
			m_selectedCache = cache;
			m_pane.SetCacheSelected ();
			m_mainWin.SetCacheSelected ();
		}
		
		public void ClearMarkers()
		{
			m_map.LoadScript("clearMarkers()");
		}

		public void ZoomToPoint (double lat, double lon)
		{
			m_map.LoadScript ("zoomToPoint(" + lat + "," + lon + ")");
		}
		
		public void AddMapWayPoint (Waypoint pt)
		{
			m_map.LoadScript ("addMarker(" + pt.Lat + "," + pt.Lon + ",'../icons/24x24/" + getMapIcon(pt.Symbol) + "')");
		}
		
		public void AddMapCache(Geocache cache)
		{
			m_map.LoadScript ("addMarker(" + cache.Lat + "," + cache.Lon + ",'../icons/24x24/" + getMapIcon(cache.TypeOfCache) + "')");
		}

		public void UpdateCacheCountStatus ()
		{
			Engine engine = Engine.getInstance();
			int visible = m_cachelist.getVisibleCaches ().Count;
			int total = engine.Store.CacheCount;
			int found = m_cachelist.GetVisibleFoundCacheCount();
			int inactive = m_cachelist.GetVisibleInactiveCacheCount();
			int mine = m_cachelist.GetOwnedCount();
			m_statusbar.Push (m_statusbar.GetContextId ("count"), String.Format (Catalog.GetString ("Showing {0} of {1} caches, {2} found, {3} not available, {4} placed by me"), visible, total, found, inactive, mine));
		}

		public static Pixbuf getSmallCacheIcon (Geocache.CacheType type)
		{
			switch (type) {
			case Geocache.CacheType.FOUND:
				return UIMonitor.FOUNDICON_S;
			case Geocache.CacheType.TRADITIONAL:
				return UIMonitor.TRADICON_S;
			case Geocache.CacheType.MYSTERY:
				return UIMonitor.MYSTERYICON_S;
			case Geocache.CacheType.MULTI:
				return UIMonitor.MULTIICON_S;
			case Geocache.CacheType.LETTERBOX:
				return UIMonitor.LETTERICON_S;
			case Geocache.CacheType.EARTH:
				return UIMonitor.EARTHICON_S;
			case Geocache.CacheType.CITO:
				return UIMonitor.CITOICON_S;
			case Geocache.CacheType.VIRTUAL:
				return UIMonitor.VIRTUAL_S;
			case Geocache.CacheType.MEGAEVENT:
				return UIMonitor.MEGAEVENT_S;
			case Geocache.CacheType.EVENT:
				return UIMonitor.EVENT_S;
			case Geocache.CacheType.WEBCAM:
				return UIMonitor.WEBCAM_S;
			case Geocache.CacheType.WHERIGO:
				return UIMonitor.WHERIGO_S;
			case Geocache.CacheType.MINE:
				return UIMonitor.OWNED_S;
			default:
				return UIMonitor.OTHERICON_S;
			}
		}
		
		public static string getMapIcon (Geocache.CacheType type)
		{
			switch (type) {
			/*case Geocache.CacheType.FOUND:
				return UIMonitor.FOUNDICON_S;*/
			case Geocache.CacheType.TRADITIONAL:
				return UIMonitor.TRAD_MI;
			case Geocache.CacheType.MYSTERY:
				return UIMonitor.UNKNOWN_MI;
			case Geocache.CacheType.MULTI:
				return UIMonitor.MULTI_MI;
			case Geocache.CacheType.LETTERBOX:
				return UIMonitor.LETTRBOX_MI;
			case Geocache.CacheType.EARTH:
				return UIMonitor.EARH_MI;
			case Geocache.CacheType.CITO:
				return UIMonitor.CITO_MI;
			case Geocache.CacheType.VIRTUAL:
				return UIMonitor.VIRTUAL_MI;
			case Geocache.CacheType.MEGAEVENT:
				return UIMonitor.MEGA_MI;
			case Geocache.CacheType.EVENT:
				return UIMonitor.EVENT_MI;
			case Geocache.CacheType.WEBCAM:
				return UIMonitor.WEBCAM_MI;
			case Geocache.CacheType.WHERIGO:
				return UIMonitor.WHERIGO_MI;
			default:
				return UIMonitor.OTHER_MI;
			}
		}
		
		public static string getMapIcon (String symbol)
		{
			return "waypoint-flag-red.png";
		}

		public List<Geocache> GetVisibleCacheList ()
		{
			return m_cachelist.getVisibleCaches ();
		}
		
	}
}
