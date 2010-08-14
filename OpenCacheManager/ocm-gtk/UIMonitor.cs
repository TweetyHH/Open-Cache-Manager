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
using System.Diagnostics;
using System.Globalization;
using System.Timers;
using org.freedesktop.DBus;
using NDesk.DBus;

namespace ocmgtk
{


	/// <summary>
	/// This class is responsible for managing UI interactions between the various widgets.
	/// </summary>
	public class UIMonitor
	{

		#region Members
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
		private static Pixbuf GENERIC_S = new Pixbuf ("./icons/scalable/treasure.svg", 24, 24);
		private static Pixbuf PARKING_S = new Pixbuf ("./icons/scalable/parking.svg", 24, 24);
		private static Pixbuf TRAILHEAD_S = new Pixbuf ("./icons/scalable/trailhead.svg", 24, 24);
		private static Pixbuf GREENPIN_S = new Pixbuf ("./icons/scalable/greenpin.svg", 24, 24);
		private static Pixbuf BLUEPIN_S = new Pixbuf ("./icons/scalable/bluepin.svg", 24, 24);
		private static Pixbuf REDPIN_S = new Pixbuf ("./icons/scalable/pushpin.svg", 24, 24);

		private static string TRAD_MI = "traditional.png";
		private static string CITO_MI = "cito.png";
		private static string EARH_MI = "earth.png";
		private static string LETTRBOX_MI = "letterbox.png";
		private static string EVENT_MI = "event.png";
		private static string MEGA_MI = "mega.png";
		private static string MULTI_MI = "multi.png";
		private static string OTHER_MI = "other.png";
		private static string OWNED_MI = "owned.png";
		private static string FOUND_MI = "found.png";
		private static string UNKNOWN_MI = "unknown.png";
		private static string VIRTUAL_MI = "virtual.png";
		private static string WEBCAM_MI = "webcam.png";
		private static string WHERIGO_MI = "wherigo.png";
		private static string GENERIC_MI = "treasure.png";



		private double m_home_lat = 46.49;
		private double m_home_lon = -81.01;
		private double m_mapLat = 46.49;
		private double m_mapLon = -81.01;
		private String m_ownerid = "0";
		private CacheList m_cachelist;
		private Geocache m_selectedCache;
		private MainWindow m_mainWin;
		private BrowserWidget m_map;
		private Config m_conf;
		private Label m_centreLabel;
		private ProgressBar m_progress;
		private bool m_showNearby;
		private bool m_useImperial;
		private Timer m_gpsTimer;
		private GPS m_gps;
		private int m_width;
		private int m_height;
		private QuickFilters m_filters;
		#endregion

		#region Properties
		
		public Config Configuration
		{
			get { return m_conf;}
		}

		/// <summary>
		/// The Geocache Pane widget
		/// </summary>
		public GeoCachePane GeoPane {
			get { return m_pane; }
			set { m_pane = value; }
		}

		/// <summary>
		/// The geocaching.com Account ID used to filter out
		/// owned caches
		/// </summary>
		public String OwnerID {
			get { return m_ownerid; }
			set { m_ownerid = value; }
		}

		/// <summary>
		/// The currently selected cache
		/// </summary>
		public Geocache SelectedCache {
			get { return m_selectedCache; }
		}

		/// <summary>
		/// The main window
		/// </summary>
		public MainWindow Main {
			get { return m_mainWin; }
			set { m_mainWin = value;}
		}

		/// <summary>
		/// The Cache List widget
		/// </summary>
		public CacheList CacheListPane {
			get { return m_cachelist; }
			set { m_cachelist = value; }
		}

		/// <summary>
		/// The status bar on the main window
		/// </summary>
		public Statusbar StatusBar {
			get { return m_statusbar; }
			set { m_statusbar = value; }
		}

		/// <summary>
		/// The slippy map used to display caches
		/// </summary>
		public BrowserWidget Map {
			get { return m_map; }
			set { m_map = value; }
		}


		/// <summary>
		/// Current centrepoint latitude
		/// </summary>
		public double CentreLat {
			get { return m_home_lat; }
			set { m_home_lat = value; }
		}

		/// <summary>
		/// Current centrepoint longitude
		/// </summary>
		public double CentreLon {
			get { return m_home_lon; }
			set { m_home_lon = value; }
		}

		public Label CentreLabel {
			set { m_centreLabel = value; }
		}

		private string m_centreName = "Home";
		public string CenterName {
			set { m_centreName = value; }
		}

		public ProgressBar StatusProgress {
			set { m_progress = value; }
		}
		
		private Dictionary<string, string> m_WaypointMappings;
		public Dictionary<string, string> WaypointMappings
		{
			get{return m_WaypointMappings;}
		}
		
		private void BuildWaypointMappings()
		{
			Dictionary<string, string> mappings = new Dictionary<string, string>();
			mappings.Add("Geocache|Traditional Cache", m_conf.Get("/apps/ocm/wmappings/Geocache_Traditional_Cache", "Geocache") as string);
			mappings.Add("Geocache|Unknown Cache",m_conf.Get("/apps/ocm/wmappings/Geocache_Unknown_Cache", "Geocache") as string);
			mappings.Add("Geocache|Virtual Cache", m_conf.Get("/apps/ocm/wmappings/Geocache_Virtual_Cache", "Geocache") as string);
			mappings.Add("Geocache|Multi-cache", m_conf.Get("/apps/ocm/wmappings/Geocache_Multi-cache", "Geocache") as string);
			mappings.Add("Geocache|Project APE Cache", m_conf.Get("/apps/ocm/wmappings/Geocache_Project_APE_Cache", "Geocache") as string);
			mappings.Add("Geocache|Cache In Trash Out Event", m_conf.Get("/apps/ocm/wmappings/Geocache_Cache_In_Trash_Out_Event", "Geocache") as string);
			mappings.Add("Geocache|Earthcache", m_conf.Get("/apps/ocm/wmappings/Geocache_Earthcache", "Geocache") as string);
			mappings.Add("Geocache|Event Cache", m_conf.Get("/apps/ocm/wmappings/Geocache_Event_Cache", "Geocache") as string);
			mappings.Add("Geocache|Letterbox Hybrid", m_conf.Get("/apps/ocm/wmappings/Geocache_Letterbox_Hybrid", "Geocache") as string);
			mappings.Add("Geocache|GPS Adventures Exhibit",m_conf.Get("/apps/ocm/wmappings/Geocache_GPS_Adventures_Exhibit", "Geocache") as string);
			mappings.Add("Geocache|Mega-Event Cache", m_conf.Get("/apps/ocm/wmappings/Geocache_Mega-Event_Cache", "Geocache") as string);
			mappings.Add("Geocache|Locationless Cache",m_conf.Get("/apps/ocm/wmappings/Geocache_Locationless_Cache", "Geocache") as string);
			mappings.Add("Geocache|Webcam Cache", m_conf.Get("/apps/ocm/wmappings/Geocache_Webcam_Cache", "Geocache") as string);
			mappings.Add("Geocache|Wherigo Cache", m_conf.Get("/apps/ocm/wmappings/Geocache_Wherigo_Cache", "Geocache") as string);
			mappings.Add("Geocache", m_conf.Get("/apps/ocm/wmappings/Geocache", "Geocache") as string);
			mappings.Add("Geocache Found", m_conf.Get("/apps/ocm/wmappings/Geocache_Found", "Geocache Found") as string);
			mappings.Add("Waypoint|Final Location", m_conf.Get("/apps/ocm/wmappings/Waypoint_Final_Location", "Pin, Blue") as string);
			mappings.Add("Waypoint|Parking Area", m_conf.Get("/apps/ocm/wmappings/Waypoint_Parking_Area", "Parking Area") as string);
			mappings.Add("Waypoint|Reference Point", m_conf.Get("/apps/ocm/wmappings/Waypoint_Reference_Point", "Pin, Green") as string);
			mappings.Add("Waypoint|Question to Answer", m_conf.Get("/apps/ocm/wmappings/Waypoint_Question_to_Answer", "Pin, Red") as string);
			mappings.Add("Waypoint|Stages of a Multicache", m_conf.Get("/apps/ocm/wmappings/Waypoint_Stages_of_a_Multicache", "Pin, Red") as string);
			mappings.Add("Waypoint|Trailhead", m_conf.Get("/apps/ocm/wmappings/Waypoint_Trailhead", "Trail Head") as string);
			mappings.Add("Waypoint|Other", m_conf.Get("/apps/ocm/wmappings/Waypoint_Other", "Pin, Green") as string);
			m_WaypointMappings = mappings;
		}
		
		

		public Boolean ShowNearby {
			set { m_showNearby = value; 
				if (value == false)
				{
					m_map.LoadScript("clearExtraMarkers()");
				}
				else
				{
					GetNearByCaches(m_mapLat, m_mapLon);
				}
			}
		}
		
		public Boolean UseImperial
		{
			get { return m_useImperial; } 
		}

		#endregion

		/// <summary>
		/// This class cannot be constructed directly. Use GetInstance()
		/// to get a UIMonitor
		/// </summary>
		private UIMonitor ()
		{
			m_conf = new Config();
		}

		/// <summary>
		/// Returns the single instance of this class
		/// </summary>
		/// <returns>
		/// A <see cref="UIMonitor"/>
		/// </returns>
		public static UIMonitor getInstance ()
		{
			lock (typeof(UIMonitor)) {
				if (null == m_instance)
					m_instance = new UIMonitor ();
				return m_instance;
			}
		}

		/// <summary>
		/// Loads the configuration from GConf
		/// </summary>
		public void LoadConfig ()
		{
			CentreLat = (double) m_conf.Get ("/apps/ocm/lastlat", 0.0);
			CentreLon = (double)m_conf.Get ("/apps/ocm/lastlon", 0.0);
			CenterName = (string) m_conf.Get("/apps/ocm/lastname", Catalog.GetString("Home"));
			if (CentreLat == 0 && CentreLon == 0)
			{
				CentreLat = (double) m_conf.Get ("/apps/ocm/homelat", 0.0);
				CentreLon = (double)m_conf.Get ("/apps/ocm/homelon", 0.0);
			}
			if (m_centreName != Catalog.GetString("Home"))
			{
				m_mainWin.EnableResetCentre();
			}
			OwnerID = (string)m_conf.Get ("/apps/ocm/memberid", String.Empty);
			m_useImperial = (Boolean) m_conf.Get("/apps/ocm/imperial", false);
			int win_width = (int) m_conf.Get("/apps/ocm/winwidth", 1024);
			int win_height = (int) m_conf.Get("/apps/ocm/winheight", 768);
			m_pane.VPos = (int) m_conf.Get("/apps/ocm/vpos", 300);
			m_mainWin.HPos = (int) m_conf.Get("/apps/ocm/hpos", 400);
			m_mainWin.Resize(win_width, win_height);
			string map = (string) m_conf.Get("/apps/ocm/defmap", "osm");
			String dbName = (string)m_conf.Get ("/apps/ocm/currentdb", String.Empty);
			bool enableGPS = (bool) m_conf.Get("/apps/ocm/gpsd/onstartup", false);
			if (enableGPS)
				m_mainWin.SetGPSDOn();
			m_mainWin.SizeAllocated += HandleM_mainWinSizeAllocated;
			while (Gtk.Application.EventsPending ())
				Gtk.Application.RunIteration (false);
			SetCurrentDB (dbName, true);
			LoadMap (map);
			SetSelectedCache(null);
			BuildWaypointMappings();
			m_filters = QuickFilters.LoadQuickFilters();
			m_mainWin.RebuildQuickFilterMenu(m_filters);
			bool showNearby = (Boolean) m_conf.Get("/apps/ocm/shownearby", true);
			if (showNearby)
				m_mainWin.SetNearbyEnabled();
			if (m_useImperial)
				m_cachelist.SetImperial();
			if (true == (bool) m_conf.Get("/apps/ocm/userownerid", true))
				Engine.getInstance().Mode = UserMode.OWNER_ID;
			else
				Engine.getInstance().Mode = UserMode.USERNAME;
			GoHome ();
		}

		void HandleM_mainWinSizeAllocated (object o, SizeAllocatedArgs args)
		{
			m_width = args.Allocation.Width;
			m_height = args.Allocation.Height;
		}

		private void LoadMap (string map)
		{
			m_map.LoadUrl ("file://" + System.Environment.CurrentDirectory + "/web/wpt_viewer.html?map=" + map);
		}

		private void SetCurrentDB (string dbName, bool loadNow)
		{
			if (! System.IO.File.Exists(dbName))
			{
				NoDBDialog dlg = new NoDBDialog();
				int resp = dlg.Run();
				if (resp == (int) ResponseType.Yes)
				{
					OpenDB();
					return;
				}
				else if (resp == (int) ResponseType.No)
				{
					CreateDB();
					return;
				}
				else
				{
					Environment.Exit(1);
				}
			}
			
			String[] dbSplit = dbName.Split ('/');
			String dBShort = dbSplit[dbSplit.Length - 1];
			m_mainWin.Title = dBShort + " - OCM";
			m_conf.Set ("/apps/ocm/currentdb", dbName);
			CacheStore store = Engine.getInstance().Store;
			store.SetDB(dbName);
			if (store.NeedsUpgrade())
				UpgradeDB(dbName, store);
			Engine.getInstance ().Store.SetDB (dbName);
			RegisterRecentFile(dbName);
			m_mainWin.UpdateBookmarkList(store.GetBookmarkLists());
			if (loadNow)
				RefreshCaches ();
		}
		
		private void UpgradeDB(String dbname, CacheStore store)
		{
			MessageDialog dlg = new MessageDialog(m_mainWin, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, Catalog.GetString("OCM needs to upgrade your database.\nWould you like to backup your database first?"));
			if ((int) ResponseType.Yes == dlg.Run())
			{
				System.IO.File.Copy(dbname, dbname + ".bak", true);
			}
			dlg.Hide();
			store.Upgrade();
			dlg = new MessageDialog(m_mainWin, DialogFlags.Modal, MessageType.Question, ButtonsType.Ok, Catalog.GetString("Upgrade complete"));
			dlg.Run();
			dlg.Hide();
		}

		/// <summary>
		/// Refreshes the cache information and list
		/// </summary>
		public void RefreshCaches ()
		{
			m_mainWin.GdkWindow.Cursor = new Cursor (Gdk.CursorType.Watch);
			m_statusbar.Push (m_statusbar.GetContextId ("refilter"), "Retrieving caches, please wait..");
			UpdateCentrePointStatus ();
			DoGUIUpdate ();
			m_cachelist.PopulateList ();
			UpdateStatusBar ();
		}

		private static void DoGUIUpdate ()
		{
			while (Gtk.Application.EventsPending ())
				Gtk.Application.RunIteration (false);
		}

		/// <summary>
		/// Sets the current active geocache
		/// </summary>
		/// <param name="cache">
		/// The cache to display<see cref="Geocache"/>
		/// </param>
		public void SetSelectedCache (Geocache cache)
		{
			m_selectedCache = cache;
			m_pane.SetCacheSelected ();
			m_mainWin.SetSelectedCache(cache);
		}

		/// <summary>
		/// Clears all markers from the map
		/// </summary>
		public void ClearMarkers ()
		{
			m_map.LoadScript ("clearMarkers()");
		}

		/// <summary>
		/// Zooms to a point on the map and highlights it with a red circle
		/// </summary>
		/// <param name="lat">
		/// Latitude coord <see cref="System.Double"/>
		/// </param>
		/// <param name="lon">
		/// Longitue coord <see cref="System.Double"/>
		/// </param>
		public void ZoomToPoint (double lat, double lon)
		{
			m_map.LoadScript ("zoomToPoint(" + lat.ToString(CultureInfo.InvariantCulture) + "," + lon.ToString(CultureInfo.InvariantCulture) + ")");
		}

		public void GoHome ()
		{
			m_mapLat = CentreLat;
			m_mapLat = CentreLon;
			m_map.LoadScript ("goHome(" + CentreLat.ToString(CultureInfo.InvariantCulture) + "," + CentreLon.ToString(CultureInfo.InvariantCulture) + ")");
		}
		
		public void ZoomToSelected ()
		{
			if (m_selectedCache == null)
				return;
			m_mapLat = m_selectedCache.Lat;
			m_mapLat = m_selectedCache.Lon;
			m_map.LoadScript ("zoomToPoint(" + m_selectedCache.Lat.ToString(CultureInfo.InvariantCulture) + "," +  m_selectedCache.Lon.ToString(CultureInfo.InvariantCulture) + ")");
		}
		
		public void ResetCenterToHome()
		{
			CentreLat = (double)m_conf.Get ("/apps/ocm/homelat", 0.0);
			CentreLon = (double)m_conf.Get ("/apps/ocm/homelon", 0.0);
			m_conf.Set("/apps/ocm/lastlat", m_home_lat);
			m_conf.Set("/apps/ocm/lastlon", m_home_lon);
			m_conf.Set("/apps/ocm/lastname", Catalog.GetString("Home"));
			Geocache selected = m_selectedCache;
			m_centreName = Catalog.GetString("Home");
			RefreshCaches();
			if (selected != null)
				SelectCache(selected.Name);
		}
		
		public void SetSelectedAsCentre()
		{
			m_home_lat = m_selectedCache.Lat;
			m_home_lon = m_selectedCache.Lon;
			Geocache selected = m_selectedCache;
			m_centreName = selected.Name;
			m_conf.Set("/apps/ocm/lastlat", m_home_lat);
			m_conf.Set("/apps/ocm/lastlon", m_home_lon);
			m_conf.Set("/apps/ocm/lastname", m_centreName);
			m_mainWin.EnableResetCentre();
			RefreshCaches();
			SelectCache(selected.Name);
		}


		/// <summary>
		/// Adds a waypoint to the map
		/// </summary>
		/// <param name="pt">
		/// The waypoint to display <see cref="Waypoint"/>
		/// </param>
		public void AddMapWayPoint (Waypoint pt)
		{
			m_map.LoadScript ("addMarker(" + pt.Lat.ToString(CultureInfo.InvariantCulture) + "," + pt.Lon.ToString(CultureInfo.InvariantCulture) + ",'../icons/24x24/" + GetMapIcon (pt.Symbol) + "',\"" + pt.Name + "\",\"" + pt.Desc.Replace("\"","''") + "\")");
		}

		/// <summary>
		/// Displays a specified cache on the map
		/// </summary>
		/// <param name="cache">
		/// The geocache to display <see cref="Geocache"/>
		/// </param>
		public void AddMapCache (Geocache cache)
		{
			string mode = String.Empty;
			if (cache.Archived)
				mode = "archived";
			else if (!cache.Available)
				mode = "disabled";
			else if (cache.CheckNotes)
				mode = "checknotes";
			string cachedesc = "<div style=font-size:10pt;>" + Catalog.GetString("<b>A cache by:</b> ") + cache.PlacedBy + Catalog.GetString("<br><b>Hidden on: </b>") 
				+  cache.Time.ToShortDateString() + "<br><b>Difficulty: </b>" + cache.Difficulty + "<br><b>Terrain: </b>" + cache.Terrain +
					"<br><b>Cache size: </b>" + cache.Container + "</div>";

			m_map.LoadScript ("addMarker(" + cache.Lat.ToString(CultureInfo.InvariantCulture) + ","
			                  + cache.Lon.ToString(CultureInfo.InvariantCulture) + ",'../icons/24x24/" 
			                  + GetMapIcon (cache) + "',\"" 
			                  + cache.Name + "\",\"" + cache.CacheName.Replace("\"","'") + "\",\"" 
			                  + cachedesc.Replace("\"","''") + "\",\"" + mode + "\")");
		}

		public void AddOtherCacheToMap (Geocache cache)
		{
			string mode = String.Empty;
			if (cache.Archived)
				mode = "archived";
			else if (!cache.Available)
				mode = "disabled";
			else if (cache.CheckNotes)
				mode = "checknotes";
			string cachedesc = "<div style=font-size:10pt;>" + Catalog.GetString("<b>A cache by:</b> ") + cache.PlacedBy + Catalog.GetString("<br><b>Hidden on: </b>") 
				+  cache.Time.ToShortDateString() + "<br><b>Difficulty: </b>" + cache.Difficulty + "<br><b>Terrain: </b>" + cache.Terrain +
					"<br><b>Cache size: </b>" + cache.Container + "</div>";

			m_map.LoadScript ("addExtraMarker(" + cache.Lat.ToString(CultureInfo.InvariantCulture) + ","
			                  + cache.Lon.ToString(CultureInfo.InvariantCulture) + ",'../icons/24x24/" 
			                  + GetMapIcon (cache) + "',\"" 
			                  + cache.Name + "\",\"" + cache.CacheName.Replace("\"","'") + "\",\"" 
			                  + cachedesc.Replace("\"","''") + "\",\"" + mode + "\")");
		}

		/*public void AddMapOtherCache (Geocache cache)
		{
			m_map.LoadScript ("addExtraMarker(" + cache.Lat + "," + cache.Lon + ",'../icons/24x24/" + GetMapIcon (cache) + "',\"" + cache.Name + "\",\"" +cache.Desc + "\")");
		}*/

		public void StartFiltering ()
		{
			m_mainWin.GdkWindow.Cursor = new Cursor (Gdk.CursorType.Watch);
			m_statusbar.Push (m_statusbar.GetContextId ("refilter"), "Refiltering, please wait..");
			while (Gtk.Application.EventsPending ())
				Gtk.Application.RunIteration (false);
		}

		/// <summary>
		/// Updates the status bar with the cache count information
		/// </summary>
		public void UpdateStatusBar ()
		{
			GetNearByCaches(m_mapLat, m_mapLon);
			Engine engine = Engine.getInstance ();
			int visible = m_cachelist.getVisibleCaches ().Count;
			int total = engine.Store.CacheCount;
			int found = m_cachelist.GetVisibleFoundCacheCount ();
			int inactive = m_cachelist.GetVisibleInactiveCacheCount ();
			int mine = m_cachelist.GetOwnedCount ();
			if (engine.Store.BookmarkList == null)
				m_statusbar.Push (m_statusbar.GetContextId ("count"), String.Format (Catalog.GetString ("Showing {0} of {1} caches, {2} found, {3} unavailable/archived, {4} placed by me"), visible, total, found, inactive, mine));
			else 
				m_statusbar.Push (m_statusbar.GetContextId ("count"), String.Format (Catalog.GetString ("Showing {0} of {1} caches from {2}, {3} found, {4} unavailable/archived, {5} placed by me"), visible, total, engine.Store.BookmarkList, found, inactive, mine));
			m_mainWin.GdkWindow.Cursor = new Cursor (CursorType.Arrow);
			UpdateCentrePointStatus ();
			DoGUIUpdate ();
			m_cachelist.ScrollToSelected();
		}

		public void UpdateCentrePointStatus ()
		{
			m_centreLabel.Text = String.Format (Catalog.GetString ("Centred on {0} ({1})"), m_centreName, Utilities.getCoordString (CentreLat, CentreLon));
			
		}

		/// <summary>
		/// Returns a PixBuf containing the 16x16 icon for the specified cache type
		/// </summary>
		/// <param name="type">
		/// Cache Type <see cref="Geocache.CacheType"/>
		/// </param>
		/// <returns>
		/// A pixbuf containing the icon <see cref="Pixbuf"/>
		/// </returns>
		public static Pixbuf GetSmallCacheIcon (Geocache.CacheType type)
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
			case Geocache.CacheType.GENERIC:
				return UIMonitor.GENERIC_S;
			default:
				return UIMonitor.OTHERICON_S;
			}
		}
		
		public static Pixbuf GetSmallWaypointIcon (String symbol)
		{
			if (symbol.Equals ("Parking Area"))
				return PARKING_S; 
			else if (symbol.Equals ("Trailhead"))
				return TRAILHEAD_S; 
			else if (symbol.Equals ("Final Location"))
				return BLUEPIN_S;
			else if ((symbol.Equals("Other")) || symbol.Equals("Reference Point"))
				return GREENPIN_S;
			return REDPIN_S;
		}

		/// <summary>
		/// Gets the icon name for the corresponding cache type
		/// </summary>
		/// <param name="type">
		/// A geocache type <see cref="Geocache.CacheType"/>
		/// </param>
		/// <returns>
		/// Icon file name <see cref="System.String"/>
		/// </returns>
		private static string GetMapIcon (Geocache cache)
		{
			if (cache.Found)
				return FOUND_MI;
			if ((cache.OwnerID == m_instance.OwnerID) ||(cache.CacheOwner == m_instance.OwnerID))
				return OWNED_MI;
			switch (cache.TypeOfCache) {
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
				case Geocache.CacheType.GENERIC:
					return UIMonitor.GENERIC_MI;
				default:
					return UIMonitor.OTHER_MI;
			}
		}

		/// <summary>
		/// Returns the map icon for a given waypoint symbol
		/// </summary>
		/// <param name="symbol">
		/// Waypoint symbol name<see cref="String"/>
		/// </param>
		/// <returns>
		/// Icon file path <see cref="System.String"/>
		/// </returns>
		private static string GetMapIcon (String symbol)
		{
			if (symbol.Equals ("Parking Area"))
				return "parking.png"; 
			else if (symbol.Equals ("Trailhead"))
				return "trailhead.png"; 
			else if (symbol.Equals ("Final Location"))
				return "bluepin.png";
			else if ((symbol.Equals("Other")) || symbol.Equals("Reference Point"))
				return "greenpin.png";
			return "pushpin.png";
		}

		/// <summary>
		/// Gets the list of caches in the cache list page as currently filtered
		/// </summary>
		/// <returns>
		/// A list of caches <see cref="List<Geocache>"/>
		/// </returns>
		public List<Geocache> GetVisibleCacheList ()
		{
			return m_cachelist.getVisibleCaches ();
		}

		/// <summary>
		/// Creates a new cache Database
		/// </summary>
		public void CreateDB ()
		{
			FileChooserDialog dlg = new FileChooserDialog (Catalog.GetString ("Create database"), m_mainWin, FileChooserAction.Save, Catalog.GetString ("Cancel"), ResponseType.Cancel, Catalog.GetString ("Save"), ResponseType.Accept);
			dlg.SetCurrentFolder (System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments));
			dlg.CurrentName = "newdb.ocm";
			FileFilter filter = new FileFilter ();
			filter.Name = "OCM Databases";
		//	filter.AddMimeType ("application/x-sqlite3");
			filter.AddPattern ("*.ocm");
			dlg.AddFilter (filter);
			
			if (dlg.Run () == (int)ResponseType.Accept) {
				RegisterRecentFile (dlg.Filename);
				Engine.getInstance ().Store.CreateDB (dlg.Filename);
				SetCurrentDB (dlg.Filename, true);
			}
			dlg.Destroy ();
		}
		
		private static void RegisterRecentFile (String filename)
		{
			RecentManager manager = RecentManager.Default;
				manager.AddItem("file://" + filename);
		}

		/// <summary>
		/// Opens an OCM database
		/// </summary>
		public void OpenDB ()
		{
			try {
				FileChooserDialog dlg = new FileChooserDialog (Catalog.GetString ("Open Database"), m_mainWin, FileChooserAction.Open, Catalog.GetString ("Cancel"), ResponseType.Cancel, Catalog.GetString ("Open"), ResponseType.Accept);
				dlg.SetCurrentFolder (System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments));
				FileFilter filter = new FileFilter ();
				filter.Name = "OCM Databases";
			//	filter.AddMimeType ("application/x-ocm");
				filter.AddPattern ("*.ocm");
				dlg.AddFilter (filter);
				
				if (dlg.Run () == (int)ResponseType.Accept) {
					dlg.Hide ();
					SetCurrentDB (dlg.Filename, true);
				}
				dlg.Destroy ();
			} catch (Exception e) {
				ShowException (e);
			}
		}

		/// <summary>
		/// Exports a GPX file from the database
		/// </summary>
		public void ExportGPX ()
		{
			ExportProgressDialog edlg = new ExportProgressDialog (new GPXWriter ());
			
			try {
				FileChooserDialog dlg = new FileChooserDialog (Catalog.GetString ("Save GPX File"), m_mainWin, FileChooserAction.Save, Catalog.GetString ("Cancel"), ResponseType.Cancel, Catalog.GetString ("Export"), ResponseType.Accept);
				dlg.SetCurrentFolder (System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments));
				dlg.CurrentName = "export.gpx";
				FileFilter filter = new FileFilter ();
				filter.Name = "GPX Files";
			//	filter.AddMimeType ("application/x-gpx");
				filter.AddPattern ("*.gpx");
				
				dlg.AddFilter (filter);
				
				if (dlg.Run () == (int)ResponseType.Accept) {
					dlg.Hide ();
					edlg.Icon = m_mainWin.Icon;
					edlg.Start (dlg.Filename, GetVisibleCacheList (), m_WaypointMappings);
					RecentManager manager = RecentManager.Default;
					manager.AddItem("file://" + dlg.Filename);
				}
				dlg.Destroy ();
			} catch (Exception e) {
				ShowException (e);
				edlg.Hide ();
			}
		}

		/// <summary>
		/// Opens up the Geocaching.com Pocket Query page in the default browser
		/// </summary>
		public static void ViewPocketQueries ()
		{
			Process.Start ("http://www.geocaching.com/pocket/default.aspx");
		}

		/// <summary>
		/// Opens up the Geocaching.com Seek page in the default browser
		/// </summary>
		public static void FindCacheOnline ()
		{
			Process.Start ("http://www.geocaching.com/seek/default.aspx");
		}

		/// <summary>
		/// Opens up the Geocaching.com account profile in the default browser
		/// </summary>
		public static void ViewAccountDetails ()
		{
			Process.Start ("http://www.geocaching.com/account/default.aspx");
		}

		/// <summary>
		/// Opens up the Geocaching.com user profile page in the default browser
		/// </summary>
		public static void ViewProfile ()
		{
			Process.Start ("http://www.geocaching.com/my");
		}

		/// <summary>
		/// Opens up the Geocaching.com home page
		/// </summary>
		public static void ViewHomePage ()
		{
			Process.Start ("http://www.geocaching.com");
		}

		public static void ShowException (Exception e)
		{
			MessageDialog errorDialog = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, e.Message);
			errorDialog.Run ();
			errorDialog.Hide ();
			System.Console.WriteLine(e.StackTrace);
		}

		public void RunSetupAssistant ()
		{
			SetupAssistant assistant = new SetupAssistant ();
			assistant.SetPosition (Gtk.WindowPosition.Center);
			assistant.ShowAll ();
		}

		public void ImportGPX ()
		{
			FileChooserDialog dlg = new FileChooserDialog (Catalog.GetString ("Import GPX File"), m_mainWin, FileChooserAction.Open, Catalog.GetString ("Cancel"), ResponseType.Cancel, Catalog.GetString ("Import"), ResponseType.Accept);
			dlg.SetCurrentFolder (System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments));
			FileFilter filter = new FileFilter ();
			filter.Name = "Waypoint Files";
			filter.AddPattern ("*.gpx");
			filter.AddPattern ("*.loc");
			dlg.AddFilter (filter);
			
			if (dlg.Run () == (int)ResponseType.Accept) {
				RegisterRecentFile(dlg.Filename);
				dlg.Hide ();
				ImportGPXFile (dlg.Filename);
			}
			dlg.Destroy ();
			;
		}

		public void ImportGPXFile (String filename)
		{
			System.IO.FileStream fs = System.IO.File.OpenRead (filename);
			GPXParser parser = new GPXParser ();
			parser.CacheOwner = OwnerID;
			int total = parser.preParse (fs);
			fs = System.IO.File.OpenRead (filename);
			CacheStore store = Engine.getInstance ().Store;
			ProgressDialog pdlg = new ProgressDialog (parser, total);
			pdlg.Icon = m_mainWin.Icon;
			pdlg.Modal = true;
			pdlg.Start (fs, store);
			RefreshCaches ();
			fs.Close ();
		}

		public void LogFind ()
		{
			MessageDialog dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Do you wish to mark this cache as found in the database?");
			if ((int)ResponseType.Yes == dlg.Run ()) 
			{
				MarkCacheFound ();
				dlg.Hide();
			}
			dlg.Hide();
			System.Console.WriteLine(m_selectedCache.URL);
			if (m_selectedCache.URL == null)
				return;
			else if (!m_selectedCache.URL.ToString().Contains("geocaching"))
				System.Diagnostics.Process.Start (m_selectedCache.URL.ToString());
			else
				System.Diagnostics.Process.Start ("http://www.geocaching.com/seek/log.aspx?ID=" + m_selectedCache.CacheID);
		}

		public void MarkCacheFound ()
		{
			m_selectedCache.Symbol = "Geocache Found";
			CacheLog log = new CacheLog ();
			log.FinderID = OwnerID;
			log.LogDate = System.DateTime.Now;
			log.LoggedBy = "OCM";
			log.LogStatus = "Found it";
			log.LogMessage = "AUTO LOG: OCM";
			Engine.getInstance ().Store.UpdateWaypointAtomic (m_selectedCache);
			Engine.getInstance ().Store.AddLogAtomic (m_selectedCache.Name, log);
			SetSelectedCache(m_selectedCache);
		}

		public void MarkCacheUnfound ()
		{
			MessageDialog dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Are you sure you want to mark " + m_selectedCache.Name + " as unfound?");
			if ((int)ResponseType.Yes == dlg.Run ()) {
				m_selectedCache.Symbol = "Geocache";
				Engine.getInstance ().Store.UpdateWaypointAtomic (m_selectedCache);
				SetSelectedCache(m_selectedCache);
			}
			dlg.Hide ();
		}
		
		public void MarkCacheDisabled ()
		{
			MessageDialog dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Are you sure you want to mark " + m_selectedCache.Name + " as disabled?");
			if ((int)ResponseType.Yes == dlg.Run ()) {
				m_selectedCache.Available = false;
				m_selectedCache.Archived = false;
				Engine.getInstance ().Store.UpdateCacheAtomic (m_selectedCache);
				SetSelectedCache(m_selectedCache);
			}
			dlg.Hide ();
		}
		
		public void MarkCacheArchived ()
		{
			MessageDialog dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Are you sure you want to mark " + m_selectedCache.Name + " as archived?");
			if ((int)ResponseType.Yes == dlg.Run ()) {
				m_selectedCache.Available = false;
				m_selectedCache.Archived = true;
				Engine.getInstance ().Store.UpdateCacheAtomic (m_selectedCache);
				SetSelectedCache(m_selectedCache);
			}
			dlg.Hide ();
		}
		
		public void MarkCacheAvailable ()
		{
			MessageDialog dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Are you sure you want to mark " + m_selectedCache.Name + " as available?");
			if ((int)ResponseType.Yes == dlg.Run ()) {
				m_selectedCache.Available = true;
				m_selectedCache.Archived = false;
				Engine.getInstance ().Store.UpdateCacheAtomic (m_selectedCache);
				SetSelectedCache(m_selectedCache);
			}
			dlg.Hide ();
		}

		public void DeleteCache ()
		{
			MessageDialog dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, String.Format(Catalog.GetString("Are you sure you want to delete {0}?"), m_selectedCache.Name));
			if ((int)ResponseType.Yes == dlg.Run ()) {
				Engine.getInstance ().Store.DeleteGeocache (m_selectedCache);
				RefreshCaches ();
			}
			dlg.Hide ();
		}

		public void SetAdditonalFilters ()
		{
			CacheStore store = Engine.getInstance ().Store;
			FilterDialog dlg = new FilterDialog ();
			dlg.Filter = store.Filter;
			if (((int)ResponseType.Ok) == dlg.Run ()) {
				store.Filter = dlg.Filter;
				m_mainWin.SetAllowClearFilter (true);
				RefreshCaches ();
			}
		}

		public void ClearFilters ()
		{
			MessageDialog dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, Catalog.GetString("Are you sure you want to clear all advanced filters?"));
			if ((int)ResponseType.Yes == dlg.Run ()) {
				dlg.Hide ();
				Engine.getInstance ().Store.Filter = null;
				m_mainWin.SetAllowClearFilter (false);
				RefreshCaches ();
			} else
				dlg.Hide ();
			dlg.Dispose ();
		}

		public void StartProgressLoad ()
		{
			m_progress.Show ();
			m_progress.Fraction = 0;
			m_progress.Text = "Loading Map 0%";
			DoGUIUpdate ();
		}

		public void SetProgress (int progress)
		{
			m_progress.Fraction = ((double)progress / 100);
			m_progress.Text = String.Format ("Loading Map ({0}%)", progress.ToString ());
			DoGUIUpdate ();
		}

		public void SetProgressDone ()
		{
			m_progress.Hide ();
			DoGUIUpdate ();
		}

		public void GetNearByCaches (double lat, double lon)
		{
			m_mapLat = lat;
			m_mapLon = lon;
			if (m_showNearby) {
				m_map.LoadScript ("clearExtraMarkers()");
				List<Geocache> visibleCaches = GetVisibleCacheList ();
				visibleCaches.Sort (new CacheDistanceSorter (lat, lon));
				int count = 0;
				IEnumerator<Geocache> cache = visibleCaches.GetEnumerator ();
				while (cache.MoveNext ()) {
					if (m_selectedCache != null && cache.Current.Name == m_selectedCache.Name)
						continue;
					else
						AddOtherCacheToMap (cache.Current);
					if (count < 100)
						count++;
					else
						return;
				}
			}
		}

		public void SelectCache (string code)
		{
			m_cachelist.SelectCache (code);
		}

		public class CacheDistanceSorter : IComparer<Geocache>
		{

			double orig_lat, orig_lon;

			public CacheDistanceSorter (double lat, double lon)
			{
				orig_lat = lat;
				orig_lon = lon;
			}

			public int Compare (Geocache obj1, Geocache obj2)
			{
				double d1 = Utilities.calculateDistance (orig_lat, obj1.Lat, orig_lon, obj1.Lon);
				double d2 = Utilities.calculateDistance (orig_lat, obj2.Lat, orig_lon, obj2.Lon);
				if (d2 > d1)
					return -1; else if (d2 == d1)
					return 0;
				else
					return 1;
			}
		}
		
		public void ConfigureGPS()
		{
			GPSConfiguration dlg = new GPSConfiguration(m_conf);
			dlg.Parent = m_mainWin;
			dlg.Icon = m_mainWin.Icon;
			if ((int) ResponseType.Ok == dlg.Run())
			{
				m_conf.Set("/apps/ocm/gps/type", dlg.GPSConfig.GetBabelFormat());
				m_conf.Set("/apps/ocm/gps/file", dlg.GPSConfig.GetOutputFile());
				m_conf.Set("/apps/ocm/gps/limit", dlg.GPSConfig.GetCacheLimit());
				m_conf.Set("/apps/ocm/gps/namemode", dlg.GPSConfig.GetNameMode().ToString());
				m_conf.Set("/apps/ocm/gps/descmode", dlg.GPSConfig.GetDescMode().ToString());
				m_conf.Set("/apps/ocm/gps/loglimit", dlg.GPSConfig.GetLogLimit());
				m_conf.Set("/apps/ocm/gps/ignorewaypointsym", dlg.GPSConfig.IgnoreWaypointOverrides());
				m_conf.Set("/apps/ocm/gps/ignoregeocachesym", dlg.GPSConfig.IgnoreGeocacheOverrides());
				dlg.UpdateWaypointSymbols(m_conf);
				BuildWaypointMappings();
			}
		}
		
		public void SendToGPS()
		{
			try
			{
				SendWaypointsDialog dlg = new SendWaypointsDialog();
				dlg.Parent = m_mainWin;
				dlg.Icon = m_mainWin.Icon;
				SavedGPSConf conf = new SavedGPSConf(m_conf);
				dlg.Start(m_cachelist.getVisibleCaches(), conf, m_WaypointMappings);
			}
			catch (GConf.NoSuchKeyException)
			{
				MessageDialog err = new MessageDialog(m_mainWin, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "You must configure a GPS Device first.");
				err.Run();
				err.Dispose();
			}
		}
		
		public void ShowPreferences()
		{
			Preferences dlg = new Preferences();
			dlg.MemberID = (String) m_conf.Get ("/apps/ocm/memberid", String.Empty);
			dlg.Lat = (Double) m_conf.Get ("/apps/ocm/homelat", 0.0);
			dlg.Lon = (Double) m_conf.Get ("/apps/ocm/homelon", 0.0);
			dlg.ImperialUnits = (Boolean) m_conf.Get("/apps/ocm/imperial", false);
			dlg.DefaultMap = (String) m_conf.Get("/apps/ocm/defmap", "osm");
			dlg.ShowNearby = (Boolean) m_conf.Get("/apps/ocm/shownearby", true);
			dlg.UsePrefixesForChildWaypoints = !((bool) m_conf.Get("/apps/ocm/noprefixes", false));
			dlg.Icon = m_mainWin.Icon;
			if ((int) ResponseType.Ok == dlg.Run())
			{
				m_conf.Set ("/apps/ocm/memberid", dlg.MemberID);
				m_conf.Set ("/apps/ocm/homelat", dlg.Lat);
				m_conf.Set ("/apps/ocm/homelon", dlg.Lon);
				m_conf.Set ("/apps/ocm/imperial", dlg.ImperialUnits);
				m_conf.Set ("/apps/ocm/defmap", dlg.DefaultMap);
				m_conf.Set ("/apps/ocm/shownearby", dlg.ShowNearby);
				m_conf.Set ("/apps/ocm/noprefixes", !dlg.UsePrefixesForChildWaypoints);
				m_home_lat = dlg.Lat;
				m_home_lon = dlg.Lon;
				m_centreLabel.Text = Catalog.GetString("Home");
				m_useImperial = dlg.ImperialUnits;
				if (dlg.ImperialUnits)
				{
					m_cachelist.SetImperial();
				}
				else
				{
					m_cachelist.SetMetric();
				}
				LoadMap(dlg.DefaultMap);
				RefreshCaches();
			}
			dlg.Dispose();
		}
		
		public static void TerraHome()
		{
			System.Diagnostics.Process.Start("http://www.terracaching.com");
		}
		
		public static void TerraTodo()
		{
			System.Diagnostics.Process.Start("http://www.terracaching.com/tdl.cgi?NF=1");
		}
		
		public static void TerraLocTodo()
		{
			System.Diagnostics.Process.Start("http://www.terracaching.com/tdl.cgi?NF=1&L=1");
		}
		
		public static void NaviHome()
		{
			System.Diagnostics.Process.Start("http://www.navicache.com");
		}
		
		public static void MyNavi()
		{
			System.Diagnostics.Process.Start("http://www.navicache.com/cgi-bin/db/MyNaviCacheHome.pl");
		}
		
		public static void OCMHome()
		{
			System.Diagnostics.Process.Start("http://opencachemanage.sourceforge.net/");
		}
		
		public static void BabelHome()
		{
			System.Diagnostics.Process.Start("http://www.gpsbabel.org/");
		}
		
		public static void GPSDHome()
		{
			System.Diagnostics.Process.Start("http://gpsd.berlios.de/");
		}
		
		public void AddBookmark()
		{
			AddBookMarkDialog dlg = new AddBookMarkDialog();
			if ((int) ResponseType.Ok == dlg.Run())
			{
				CacheStore store = Engine.getInstance().Store;
				store.AddBookmark(dlg.BookmarkName);
				m_mainWin.UpdateBookmarkList(store.GetBookmarkLists());
			}
		}
		
		public void SetBookmark(String bmrk)
		{
			CacheStore store = Engine.getInstance().Store;
			store.BookmarkList = bmrk;
			m_mainWin.SetBookmarkList(bmrk);
			RefreshCaches();
		}
		
		public void BookmarkSelectedCache(String bmrk)
		{
			CacheStore store = Engine.getInstance().Store;
			store.BookMarkCache(m_selectedCache.Name, bmrk);
			m_statusbar.Push (m_statusbar.GetContextId ("bmrk"), String.Format (Catalog.GetString ("{0} added to {1}"),m_selectedCache.Name, bmrk));
		}
		
		public void BookmarkVisible(String bmrk)
		{
			CacheStore store = Engine.getInstance().Store;
			List<Geocache> caches = GetVisibleCacheList();
			foreach(Geocache cache in caches)
			{
				store.BookMarkCache(cache.Name, bmrk);
			}
			m_statusbar.Push (m_statusbar.GetContextId ("bmrk"), String.Format (Catalog.GetString ("{0} caches added to {1}"),caches.Count, bmrk));
		}
		
		public void RemoveSelFromBookmark()
		{
			CacheStore store = Engine.getInstance().Store;
			MessageDialog dlg = new MessageDialog(m_mainWin, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, String.Format(Catalog.GetString("Are you sure you want to remove {0} from {1}?"), m_selectedCache.Name, store.BookmarkList));
			if ((int) ResponseType.Yes == dlg.Run())
			{
				store.RemoveCacheFromActiveBookmark(m_selectedCache.Name);
				dlg.Hide();
				RefreshCaches();
			}
			else
			{
				dlg.Hide();
			}
			dlg.Dispose();
		}
		
		public void RemoveBookmark()
		{
			CacheStore store = Engine.getInstance().Store;
			DeleteBookmark dlg = new DeleteBookmark();
			if ((int) ResponseType.Ok == dlg.Run())
			{
				store.DeleteBookmark(dlg.Bookmark);
				m_mainWin.UpdateBookmarkList(store.GetBookmarkLists());
				if (dlg.Bookmark == store.BookmarkList)
				{
					store.BookmarkList  = null;
					RefreshCaches();
				}
			}
			dlg.Dispose();
		}
		
		public void ModifyCache()
		{
			ModifyCacheDialog dlg = new ModifyCacheDialog();
			dlg.IsModifyDialog = true;
			dlg.Cache = m_selectedCache;
			if ((int)ResponseType.Ok == dlg.Run())
			{
				m_selectedCache = dlg.Cache;
				Engine.getInstance().Store.UpdateWaypointAtomic(dlg.Cache);
				Engine.getInstance().Store.UpdateCacheAtomic(dlg.Cache);
				SetSelectedCache(dlg.Cache);
			}
		}
		
		public void AddCache()
		{
			ModifyCacheDialog dlg = new ModifyCacheDialog();
			dlg.IsModifyDialog = false;
			Geocache cache = new Geocache();
			cache.Name = "GC_CHANGE_THIS";
			cache.Symbol = "Geocache";
			cache.Type = "Geocache";
			cache.Available = true;
			dlg.Cache = cache;
			if ((int)ResponseType.Ok == dlg.Run())
			{
				m_selectedCache = dlg.Cache;
				Engine.getInstance().Store.UpdateWaypointAtomic(dlg.Cache);
				Engine.getInstance().Store.UpdateCacheAtomic(dlg.Cache);
				RefreshCaches();
				SetSelectedCache(dlg.Cache);
			}
		}
		
		public void DeleteAll()
		{
			List<Geocache> list = GetVisibleCacheList();
			MessageDialog dlg = new MessageDialog(m_mainWin, DialogFlags.Modal, MessageType.Warning,
			                                      ButtonsType.YesNo, 
			                                      String.Format(Catalog.GetString("Are you sure you want to delete these {0} caches?\nThis operation cannot be undone."), list.Count));
			if ((int)ResponseType.Yes == dlg.Run ()) {
				dlg.Hide();
				CacheStore store = Engine.getInstance().Store;
				foreach(Geocache cache in list)
				{
					store.DeleteGeocache (cache);
				}
				SetSelectedCache(null);
				RefreshCaches ();
			}
			else
			{
				dlg.Hide();
			}
		}
		
		public void EnableGPS()
		{
			m_gps = new GPS();

			int interval = (int) m_conf.Get("/apps/ocm/gpsd/poll", 30);
			m_gpsTimer = new Timer(interval * 1000);
			m_gpsTimer.AutoReset = true;
			m_gpsTimer.Enabled = true;
			m_gpsTimer.Elapsed += HandleM_gpsTimerElapsed;
			m_centreName = "GPS";
			m_home_lat = m_gps.Lat;
			m_home_lon = m_gps.Lon;
			UpdateCentrePointStatus();
			Timer init = new Timer(1000);
			init.AutoReset = false;
			init.Elapsed += HandleM_gpsTimerElapsed;
			init.Start();
		}

		void HandleM_gpsTimerElapsed (object sender, ElapsedEventArgs e)
		{
			
			Application.Invoke(delegate{
			m_centreName = "GPS";
			m_home_lat = m_gps.Lat;
			m_home_lon = m_gps.Lon;
			m_cachelist.RefilterList();
			bool recenterMap = (bool) m_conf.Get("/apps/ocm/gpsd/recenter", true);
			if (m_selectedCache == null && recenterMap)
				GoHome();
			});
			
			
		}
		
		public void DisableGPS()
		{
			if (null == m_gpsTimer)
				return;
			m_gpsTimer.AutoReset = false;
			m_gpsTimer.Stop();
			m_gpsTimer = null;
			m_gps = null;
		}
		
		public void SaveWinSettings()
		{
			m_conf.Set("/apps/ocm/winwidth", m_width);
			m_conf.Set("/apps/ocm/winheight",m_height);
			m_conf.Set("/apps/ocm/hpos", m_mainWin.HPos);
			m_conf.Set("/apps/ocm/vpos",m_pane.VPos);
		}
		
		public void ConfigureGPSD()
		{
			GPSDConfig dlg = new GPSDConfig();
			dlg.Icon = m_mainWin.Icon;
			dlg.GPSDOnStartup = (bool) m_conf.Get("/apps/ocm/gpsd/onstartup", false);
			dlg.RecenterMap = (bool) m_conf.Get("/apps/ocm/gpsd/recenter", true);
			dlg.PollInterval = (int) m_conf.Get("/apps/ocm/gpsd/poll", 30);
			if ((int) ResponseType.Ok == dlg.Run())
			{
				m_conf.Set("/apps/ocm/gpsd/onstartup", dlg.GPSDOnStartup);
				m_conf.Set("/apps/ocm/gpsd/recenter", dlg.RecenterMap);
				m_conf.Set("/apps/ocm/gpsd/poll", dlg.PollInterval);
			}
		}
		
		public string GetOCMVersion()
		{
			String version = "Unknown";	
			System.IO.StreamReader reader = new System.IO.StreamReader(new System.IO.FileStream("version/Version.txt",System.IO.FileMode.Open,System.IO.FileAccess.Read));
			version = reader.ReadToEnd();
			reader.Close();
			return version;
		}
		
		public void OpenInQLandKarte()
		{
			GPXWriter writer = new GPXWriter();
			String tempPath = System.IO.Path.GetTempPath();
			String tempFile = tempPath + "ocm_export.gpx";
			ExportProgressDialog dlg = new ExportProgressDialog(writer);
			
			/*Connection DbusConnection = Bus.Session;
			IQLandkarteGT ql = DbusConnection.GetObject<IQLandkarteGT> ("org.qlandkarte.dbus", new ObjectPath ("QLandkarteGT"));
			System.Console.WriteLine("Connected");
			ql.loadGeoData("/home/campbelk/Desktop/export.gpx");	*/		
			
			dlg.AutoClose = true;
			dlg.Title = Catalog.GetString("Preparing to send to QLandKarte GT");
			dlg.WaypointsOnly = true;
			dlg.CompleteCommand = "qlandkartegt " + tempFile;
			dlg.Icon = m_mainWin.Icon;
			dlg.Start(tempFile, GetVisibleCacheList(), m_WaypointMappings);
		}
		
		public void OpenSelectedCacheInQLandKarte()
		{
			GPXWriter writer = new GPXWriter();
			String tempPath = System.IO.Path.GetTempPath();
			String tempFile = tempPath + "ocm_export.gpx";
			List<Geocache> cache = new List<Geocache>();
			cache.Add(m_selectedCache);
			ExportProgressDialog dlg = new ExportProgressDialog(writer);
			dlg.AutoClose = true;
			dlg.Title = Catalog.GetString("Preparing to send to QLandKarte GT");
			dlg.WaypointsOnly = true;
			dlg.CompleteCommand = "qlandkartegt " + tempFile;
			dlg.Icon = m_mainWin.Icon;
			dlg.Start(tempFile, cache, m_WaypointMappings);
		}
		
		public void ApplyQuickFilter(QuickFilter filter)
		{
			m_cachelist.ApplyQuickFilter(filter);
			Engine.getInstance().Store.Filter = filter.AdvancedFilters;
			if (filter.AdvancedFilters != null)
				m_mainWin.SetAllowClearFilter (true);
			else
				m_mainWin.SetAllowClearFilter(false);
			RefreshCaches();
		}
		
		public void SaveQuickFilter()
		{
			AddBookMarkDialog dlg = new AddBookMarkDialog();
			dlg.Title = Catalog.GetString("Save QuickFilter");
			if (((int) ResponseType.Ok) == dlg.Run())
			{
				QuickFilter filter = new QuickFilter();
				filter.AdvancedFilters = Engine.getInstance().Store.Filter;
				m_cachelist.PopulateQuickFilter(filter);
				filter.Name = dlg.BookmarkName;
				m_filters.AddFilter(filter);
				m_mainWin.RebuildQuickFilterMenu(m_filters);
			}
		}
		
		public void DeleteQuickFilter()
		{
			DeleteBookmark dlg = new DeleteBookmark(m_filters);
			if ((int) ResponseType.Ok == dlg.Run())
			{
				m_filters.DeleteFilter(dlg.Bookmark);
				m_mainWin.RebuildQuickFilterMenu(m_filters);
			}
		}
		
		public static void ViewOCMWiki()
		{
			Process.Start("http://sourceforge.net/apps/mediawiki/opencachemanage/");
		}

	}
}
