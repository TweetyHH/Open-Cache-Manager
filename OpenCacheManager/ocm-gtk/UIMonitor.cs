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
		private double m_centerLat = 0;
		private double m_centerLon = 0;
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
		private LocationList m_locations;
		private DateTime m_loggingdate = DateTime.Now;
		private bool m_blockMapProgress = false;
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
			get { return m_centerLat; }
			set { m_centerLat = value; }
		}

		/// <summary>
		/// Current centrepoint longitude
		/// </summary>
		public double CentreLon {
			get { return m_centerLon; }
			set { m_centerLon = value; }
		}

		public Label CentreLabel {
			set { m_centreLabel = value; }
		}
		
		private double m_currLat;
		public double CurrLat{
			get { return m_currLat;}
			set { m_currLat = value;}
		}
		
		private double m_currLon;
		public double CurrLon
		{
			get { return m_currLon;}
			set { m_currLon = value;}
		}

		private string m_centreName = "Home";
		public string CenterName {
			set { m_centreName = value; }
		}

		public ProgressBar StatusProgress {
			set { m_progress = value; }
		}
		
		private bool m_ShowAllChildren = false;
		public bool ShowAllChildren
		{
			get { return m_ShowAllChildren;}
			set { 
				m_ShowAllChildren = value;
				if (m_showNearby)
					GetNearByCaches();
				else
					m_map.LoadScript("clearExtraMarkers()");
					
			}
		}
		
		private GPSProfileList m_profiles;
		
		private int m_MapPoints = 100;
		public int MapPoints
		{
			get { return m_MapPoints;}
			set { m_MapPoints = value;}
		}
		
		public Boolean ShowNearby {
			get { return m_showNearby;}
			set { m_showNearby = value; 
				if (value == false)
				{
					m_map.LoadScript("clearExtraMarkers()");
				}
				else
				{
					GetNearByCaches();
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
		public void LoadConfig (bool refreshNow)
		{
			CentreLat = m_conf.LastLat;
			CentreLon = m_conf.LastLon;
			m_currLat = CentreLat;
			m_currLon = CentreLon;
			CenterName = m_conf.LastName;
			if (CentreLat == 0 && CentreLon == 0)
			{
				CentreLat = m_conf.HomeLat;
				CentreLon = m_conf.HomeLon;
			}
			if (m_centreName != Catalog.GetString("Home"))
			{
				m_mainWin.EnableResetCentre();
			}
			MapPoints = m_conf.MapPoints;
			OwnerID = m_conf.OwnerID;
			m_useImperial = m_conf.ImperialUnits;
			m_mainWin.VPos = m_conf.VBarPosition;
			m_mainWin.HPos = m_conf.HBarPosition;
			m_mainWin.Resize(m_conf.WindowWidth, m_conf.WindowHeight);
			if (m_conf.UseGPSD)
				m_mainWin.SetGPSDOn();
			m_mainWin.SizeAllocated += HandleM_mainWinSizeAllocated;
			m_mainWin.ShowNow();
			while (Gtk.Application.EventsPending ())
				Gtk.Application.RunIteration (true);
			m_filters = QuickFilters.LoadQuickFilters();
			m_locations = LocationList.LoadLocationList();
			m_profiles = GPSProfileList.LoadProfileList();
			m_mainWin.SetOfflineMode(m_conf.UseOfflineLogging);
			LoadMap (m_conf.MapType);
			if (m_conf.StartupFilter != String.Empty)
			{
				SetCurrentDB (m_conf.DBFile, false);
				QuickFilter filter = m_filters.GetFilter(m_conf.StartupFilter);
				if (filter != null)
				{
					m_cachelist.ApplyQuickFilter(filter);
					Engine.getInstance().Store.Filter = filter.AdvancedFilters;
					CheckAdvancedFilters(filter.AdvancedFilters);
				}
				if (refreshNow)
					RefreshCaches();
			}
			else
			{
				SetCurrentDB (m_conf.DBFile, refreshNow);
			}
			SetSelectedCache(null);
			m_mainWin.RebuildQuickFilterMenu(m_filters);
			m_mainWin.RebuildLocationsMenu(m_locations);
			m_mainWin.RebuildProfilesMenu(m_profiles);
			m_mainWin.RebuildProfEditMenu(m_profiles);
			m_conf.CheckForDefaultGPS(m_profiles, m_mainWin);
			EToolList tools = EToolList.LoadEToolList();
			m_mainWin.RebuildEToolMenu(tools);
			m_ShowAllChildren = m_conf.ShowAllChildren;
			if (m_conf.ShowNearby)
				m_mainWin.SetNearbyEnabled();
			if (m_ShowAllChildren)
				m_mainWin.SetShowAllChildren();
			if (m_useImperial)
				m_cachelist.SetImperial();
			AutoCheckForUpdates();				
		}

		void HandleM_mainWinSizeAllocated (object o, SizeAllocatedArgs args)
		{
			m_width = args.Allocation.Width;
			m_height = args.Allocation.Height;
		}

		private void LoadMap (string map)
		{
			//System.Console.WriteLine("file://" + System.Environment.CurrentDirectory + "/web/wpt_viewer.html?map=" + map + "&lat=" + m_home_lat + "&lon=" + m_home_lon);
			m_map.LoadUrl ("file://" + System.Environment.CurrentDirectory + "/web/wpt_viewer.html?map=" + map + "&lat=" + m_centerLat.ToString(CultureInfo.InvariantCulture) + "&lon=" + m_centerLon.ToString(CultureInfo.InvariantCulture));
			m_map.SetAutoSelectCache(m_conf.AutoSelectCacheFromMap);
		}

		public void SetCurrentDB (string dbName, bool loadNow)
		{
			if (! System.IO.File.Exists(dbName))
			{
				HandleNoDB ();
				return;
			}
			
			SetSelectedCache(null);
			
			
			String[] dbSplit = dbName.Split ('/');
			String dBShort = dbSplit[dbSplit.Length - 1];
			m_mainWin.Title = dBShort + " - OCM";
			m_conf.DBFile = dbName;
			CacheStore store = Engine.getInstance().Store;
			store.SetDB(dbName);
			store.BookmarkList = null;
			if (store.NeedsUpgrade())
				UpgradeDB(dbName, store);
			Engine.getInstance ().Store.SetDB (dbName);
			RegisterRecentFile(dbName);
			m_mainWin.UpdateBookmarkList(store.GetBookmarkLists());
			if (loadNow)
				RefreshCaches ();
		}
		
		private void HandleNoDB ()
		{
			NoDBDialog dlg = new NoDBDialog();
			int resp = dlg.Run();
			if (resp == (int) ResponseType.Yes)
			{
				if (!OpenDB())
					HandleNoDB();				
				return;
			}
			else if (resp == (int) ResponseType.No)
			{
				if (!CreateDB())
					HandleNoDB();
				return;
			}
			else
			{
				Environment.Exit(1);
			}
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
			SetInsensitive ();
			m_statusbar.Push (m_statusbar.GetContextId ("refilter"), "Retrieving caches, please wait..");
			UpdateCentrePointStatus ();
			DoGUIUpdate ();
			m_cachelist.PopulateList ();
			GetNearByCaches();
			UpdateStatusBar ();
		}
		
		private void SetInsensitive ()
		{
			m_cachelist.Sensitive = false;
			m_mainWin.MenuBar.Sensitive = false;
			m_pane.Sensitive = false;
			m_map.Sensitive = false;
		}
		
		private void SetSensitive ()
		{
			m_cachelist.Sensitive = true;
			m_mainWin.MenuBar.Sensitive = true;
			m_pane.Sensitive = true;
			m_map.Sensitive = true;
		}

		public static void DoGUIUpdate ()
		{
			while (Gtk.Application.EventsPending ())
				Gtk.Application.RunIteration (true);
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
			if (cache == null)
			{
				m_cachelist.SelectCache(null);
			}
			m_mainWin.SetSelectedCache(cache);
			ZoomToSelected();
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
			m_currLat = CentreLat;
			m_currLon = CentreLon;
			m_map.LoadScript ("goHome(" + CentreLat.ToString(CultureInfo.InvariantCulture) + "," + CentreLon.ToString(CultureInfo.InvariantCulture) + ")");
		}
		
		public void ZoomToSelected ()
		{
			if (m_selectedCache == null)
				return;
			m_currLat = m_selectedCache.Lat;
			m_currLon = m_selectedCache.Lon;
			m_map.LoadScript ("zoomToPoint(" + m_selectedCache.Lat.ToString(CultureInfo.InvariantCulture) + "," +  m_selectedCache.Lon.ToString(CultureInfo.InvariantCulture) + ")");
		}
		
		public void ResetCenterToHome()
		{
			m_centerLat = m_conf.HomeLat;
			m_centerLon = m_conf.HomeLon;
			m_centreName = Catalog.GetString("Home");
			SaveCenterPos ();
			Geocache selected = m_selectedCache;
			RecenterMap();
			RefreshCaches();
			if (selected != null)
				SelectCache(selected.Name);
		}
		
		private void SaveCenterPos ()
		{
			m_conf.LastLat = m_centerLat;
			m_conf.LastLon = m_centerLon;
			m_conf.LastName = m_centreName;
		}
		
		public void SetSelectedAsCentre()
		{
			m_centerLat = m_selectedCache.Lat;
			m_centerLon = m_selectedCache.Lon;
			m_centreName = m_selectedCache.Name;
			SaveCenterPos();
			Geocache selected = m_selectedCache;
			m_mainWin.EnableResetCentre();
			m_cachelist.RefilterList();
			SelectCache(selected.Name);
		}


		/// <summary>
		/// Adds a waypoint to the map
		/// </summary>
		/// <param name="pt">
		/// The waypoint to display <see cref="Waypoint"/>
		/// </param>
		public void AddMapWayPoint (Waypoint pt, Geocache parent)
		{
			string desc = pt.Desc.Replace("\"","''");
			desc = desc.Replace("\n", "<br/>");
			string iconModifier = String.Empty;
			if (parent.Archived) {
				iconModifier = "archived-";
			}
			else if (!parent.Available) {
				iconModifier = "disabled-";
			}
			m_map.LoadScript ("addMarker(" + pt.Lat.ToString(CultureInfo.InvariantCulture) + "," 
			                  + pt.Lon.ToString(CultureInfo.InvariantCulture) + ",'../icons/24x24/" + iconModifier
			                  + IconManager.GetMapIcon (pt.Symbol) + "',\"" + pt.Name + "\",\"\",\"" + desc + "\")");
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
			string iconModifier = String.Empty;
			if (cache.Archived) {
				mode = "archived";
				iconModifier = "archived-";
			}
			else if (!cache.Available) {
				mode = "disabled";
				iconModifier = "disabled-";
			}
			else if (cache.CheckNotes) {
				mode = "checknotes";
			}
			
			double lat = cache.Lat;
			double lon = cache.Lon;
			if (cache.HasCorrected)
			{
				lat = cache.CorrectedLat;
				lon = cache.CorrectedLon;
			}
			string cachedesc = "<div style=font-size:10pt;>" + Catalog.GetString("<b>A cache by:</b> ") + cache.PlacedBy + Catalog.GetString("<br><b>Hidden on: </b>") 
				+  cache.Time.ToShortDateString() + "<br><b>Difficulty: </b>" + cache.Difficulty + "<br><b>Terrain: </b>" + cache.Terrain +
					"<br><b>Cache size: </b>" + cache.Container + "</div>";

			m_map.LoadScript ("addMarker(" + lat.ToString(CultureInfo.InvariantCulture) + ","
			                  + lon.ToString(CultureInfo.InvariantCulture) + ",'../icons/24x24/" + iconModifier 
			                  + IconManager.GetMapIcon (cache, m_ownerid, this) + "',\"" 
			                  + cache.Name + "\",\"" + cache.CacheName.Replace("\"","'") + "\",\"" 
			                  + cachedesc.Replace("\"","''") + "\",\"" + mode + "\")");
		}

		public void AddOtherCacheToMap (Geocache cache)
		{
			string mode = String.Empty;
			string iconModifier = String.Empty;
			if (cache.Archived) {
				mode = "archived";
				iconModifier = "archived-";
			}
			else if (!cache.Available) {
				mode = "disabled";
				iconModifier = "disabled-";
			}
			else if (cache.CheckNotes)
				mode = "checknotes";
			double lat = cache.Lat;
			double lon = cache.Lon;
			if (cache.HasCorrected)
			{
				lat = cache.CorrectedLat;
				lon = cache.CorrectedLon;
			}
			string cachedesc = "<div style=font-size:10pt;>" + Catalog.GetString("<b>A cache by:</b> ") + cache.PlacedBy + Catalog.GetString("<br><b>Hidden on: </b>") 
				+  cache.Time.ToShortDateString() + "<br><b>Difficulty: </b>" + cache.Difficulty + "<br><b>Terrain: </b>" + cache.Terrain +
					"<br><b>Cache size: </b>" + cache.Container + "</div>";

			m_map.LoadScript ("addExtraMarker(" + lat.ToString(CultureInfo.InvariantCulture) + ","
			                  + lon.ToString(CultureInfo.InvariantCulture) + ",'../icons/24x24/" + iconModifier
			                  + IconManager.GetMapIcon (cache, m_ownerid, this) + "',\"" 
			                  + cache.Name + "\",\"" + cache.CacheName.Replace("\"","'") + "\",\"" 
			                  + cachedesc.Replace("\"","''") + "\",\"" + mode + "\")");
			if (m_ShowAllChildren)
				ShowOtherChildWaypoints(cache);
		}
		
		public void AddOtherMapWayPoint (Geocache cache, Waypoint pt)
		{
			string desc = pt.Desc.Replace("\"","''");
			desc = desc.Replace("\n", "<br/>");
			string iconModifier = String.Empty;
			if (cache.Archived) {
				iconModifier = "archived-";
			}
			else if (! cache.Available) {
				iconModifier = "disabled-";
			}
			m_map.LoadScript ("addExtraMarker(" + pt.Lat.ToString(CultureInfo.InvariantCulture) + "," 
			                  + pt.Lon.ToString(CultureInfo.InvariantCulture) + ",'../icons/24x24/" + iconModifier
			                  + IconManager.GetMapIcon (pt.Symbol) + "',\"" + cache.Name + "\",\""+ cache.CacheName.Replace("\"","'") +"\",\"" + pt.Name + "-" + desc + "\")");
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
				Gtk.Application.RunIteration (true);
		}

		/// <summary>
		/// Updates the status bar with the cache count information
		/// </summary>
		public void UpdateStatusBar ()
		{
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
			SetSensitive();
		}

		public void UpdateCentrePointStatus ()
		{
			m_centreLabel.Text = String.Format (Catalog.GetString ("Centred on {0} ({1})"), m_centreName, Utilities.getCoordString (CentreLat, CentreLon));
			
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
		public bool CreateDB ()
		{
			FileChooserDialog dlg = new FileChooserDialog (Catalog.GetString ("Create database"), m_mainWin, FileChooserAction.Save, Catalog.GetString ("Cancel"), ResponseType.Cancel, Catalog.GetString ("Save"), ResponseType.Accept);
			dlg.SetCurrentFolder(m_conf.DataDirectory);
			dlg.CurrentName = "newdb.ocm";
			FileFilter filter = new FileFilter ();
			filter.Name = "OCM Databases";
			filter.AddPattern ("*.ocm");
			dlg.AddFilter (filter);			
			if (dlg.Run () == (int)ResponseType.Accept) {
				if (dlg.Filename == Engine.getInstance().Store.DBFile)
				{
					dlg.Hide();
					MessageDialog mdlg = new MessageDialog(m_mainWin, DialogFlags.Modal, MessageType.Error,
					                                       ButtonsType.Ok, Catalog.GetString("You cannot overwrite the " +
					                                       	"currently open database."));
					mdlg.Run();
					mdlg.Destroy();
					return false;
				}
				else if (System.IO.File.Exists(dlg.Filename))
				{
					dlg.Hide();
					MessageDialog mdlg = new MessageDialog(m_mainWin, DialogFlags.Modal, MessageType.Warning,
					                                       ButtonsType.YesNo, Catalog.GetString("Are you sure you want to overwrite '{0}'"),
					                                       	dlg.Filename);
					if ((int) ResponseType.No ==  mdlg.Run())
					{
						mdlg.Destroy();
						return false;
					}
					else
					{
						mdlg.Destroy();
						System.IO.File.Delete(dlg.Filename);
					}
				}
				
				RegisterRecentFile (dlg.Filename);
				Engine.getInstance ().Store.CreateDB (dlg.Filename);
				SetCurrentDB (dlg.Filename, true);
				dlg.Destroy ();
				return true;
			}
			dlg.Destroy();
			return false;
		}
		
		private static void RegisterRecentFile (String filename)
		{
			RecentManager manager = RecentManager.Default;
				manager.AddItem("file://" + filename);
		}

		/// <summary>
		/// Opens an OCM database
		/// </summary>
		public bool OpenDB ()
		{
			try {
				FileChooserDialog dlg = new FileChooserDialog (Catalog.GetString ("Open Database"), 
				                                               m_mainWin, FileChooserAction.Open, 
				                                               Catalog.GetString ("Cancel"), 
				                                               ResponseType.Cancel, 
				                                               Catalog.GetString ("Open"), 
				                                               ResponseType.Accept);
				dlg.SetCurrentFolder (m_conf.DataDirectory);
				FileFilter filter = new FileFilter ();
				filter.Name = "OCM Databases";
				filter.AddPattern ("*.ocm");
				dlg.AddFilter (filter);
				
				if (dlg.Run () == (int)ResponseType.Accept) 
				{
					dlg.Hide ();
					SetCurrentDB (dlg.Filename, true);
					dlg.Destroy ();
					return true;
				}
				else
				{
					dlg.Hide();
					return false;
				}
				
			} 
			catch (Exception e) 
			{
				ShowException (e);
				return false;
			}
		}

		/// <summary>
		/// Exports a GPX file from the database
		/// </summary>
		public void ExportGPX ()
		{
			GPXWriter writer = new GPXWriter();
			ExportProgressDialog edlg = new ExportProgressDialog (writer);
			edlg.AutoClose = m_conf.AutoCloseWindows;
			try {
				ExportGPXDialog dlg = new ExportGPXDialog ();
				dlg.SetCurrentFolder (System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments));
				dlg.CurrentName = "export.gpx";
				dlg.WaypointMappings = null;
				FileFilter filter = new FileFilter ();
				filter.Name = "GPX Files";
				filter.AddPattern ("*.gpx");
				dlg.AddFilter (filter);
				
				if (dlg.Run () == (int)ResponseType.Ok) {
					dlg.Hide ();
					writer.Limit = dlg.CacheLimit;
					writer.IncludeGroundSpeakExtensions = dlg.IsPaperless;
					writer.IncludeChildWaypoints = dlg.IncludeChildren;
					writer.UseOCMPtTypes = dlg.UseMappings;
					writer.NameMode = dlg.GetNameMode();
					writer.DescriptionMode = dlg.GetDescMode();
					if (dlg.UsePlainText)
						writer.HTMLOutput = HTMLMode.PLAINTEXT;
					writer.WriteAttributes = dlg.MustHaveIncludeAttributes;
					writer.LogLimit = dlg.LogLimit;
					edlg.Icon = m_mainWin.Icon;
					edlg.Start (dlg.Filename, GetVisibleCacheList (), dlg.WaypointMappings);
					RecentManager manager = RecentManager.Default;
					manager.AddItem("file://" + dlg.Filename);
				}
				else
				{
					edlg.Destroy();
				}
				dlg.Destroy ();
			} catch (Exception e) {
				ShowException (e);
				edlg.Destroy ();
			}
		}
		
		public void ExportFindsGPX ()
		{
			GPXWriter writer = new GPXWriter();
			writer.IsMyFinds = true;
			writer.MyFindsOwner = m_ownerid;
			ExportProgressDialog edlg = new ExportProgressDialog (writer);
			edlg.AutoClose = m_conf.AutoCloseWindows;
			
			try {
				FileChooserDialog dlg = new FileChooserDialog (Catalog.GetString (" Export Finds GPX File"), m_mainWin, FileChooserAction.Save, Catalog.GetString ("Cancel"), ResponseType.Cancel, Catalog.GetString ("Export"), ResponseType.Accept);
				dlg.SetCurrentFolder (System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments));
				dlg.CurrentName = "finds.gpx";
				FileFilter filter = new FileFilter ();
				filter.Name = "GPX Files";
				filter.AddPattern ("*.gpx");
				
				dlg.AddFilter (filter);
				
				if (dlg.Run () == (int)ResponseType.Accept) {
					dlg.Hide ();
					edlg.Icon = m_mainWin.Icon;
					edlg.Start (dlg.Filename, Engine.getInstance().Store.GetFinds(), GPSProfileList.GetDefaultMappings());
					RecentManager manager = RecentManager.Default;
					manager.AddItem("file://" + dlg.Filename);
				}
				else
				{
					edlg.Destroy();
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

		public void ImportFile ()
		{
			ImportDialog dlg = new ImportDialog();
			dlg.SetCurrentFolder(m_conf.ImportDirectory);
			dlg.IgnoreExtraFields = m_conf.ImportIgnoreExtraFields;
			dlg.PreventStatusOverwrite = m_conf.ImportPreventStatusOverwrite;
			dlg.PurgeOldLogs = m_conf.ImportPurgeOldLogs;
			if (dlg.Run () == (int)ResponseType.Accept) {
				RegisterRecentFile(dlg.Filename);
				dlg.Hide ();
				m_conf.ImportIgnoreExtraFields = dlg.IgnoreExtraFields;
				m_conf.ImportPreventStatusOverwrite = dlg.PreventStatusOverwrite;
				m_conf.ImportPurgeOldLogs = dlg.PurgeOldLogs;
				if (dlg.Filename.EndsWith(".zip"))
				    ImportZip(dlg.Filename);
				else
					ImportGPXFile (dlg.Filename);
			}
			dlg.Destroy ();
		}
		
		public void ImportGPXFile (String filename)
		{
			ImportGPXFile(filename, m_conf.AutoCloseWindows);
		}

		public void ImportGPXFile (String filename, bool autoclose)
		{
			System.IO.FileStream fs = System.IO.File.OpenRead (filename);
			GPXParser parser = new GPXParser ();
			parser.IgnoreExtraFields = m_conf.ImportIgnoreExtraFields;
			parser.PreserveFound = m_conf.ImportPreventStatusOverwrite;
			parser.PurgeLogs = m_conf.ImportPurgeOldLogs;
			parser.CacheOwner = OwnerID;
			ProgressDialog pdlg = new ProgressDialog (parser);
			pdlg.AutoClose = autoclose;
			pdlg.Icon = m_mainWin.Icon;
			pdlg.Modal = true;
			pdlg.Start (filename, Engine.getInstance().Store);
			RefreshCaches ();
			fs.Close ();
		}

		public void LogFind ()
		{
			if (m_conf.UseOfflineLogging)
			{
				LogFindOffline ();		
			}
			else
			{
				LogCacheOnline ();
			}
		}
		
		private void LogFindOffline ()
		{
			OfflineLogDialog dlg = new OfflineLogDialog();
			CacheLog log = new CacheLog();
			log.CacheCode = m_selectedCache.Name;
			log.LogDate = m_loggingdate;
			log.LogStatus = "Found it";
			log.LoggedBy = "OCM";
			log.LogKey = m_selectedCache.Name + "-ofl";
			log.LogMessage = String.Empty;
			dlg.Log = log;
			if ((int) ResponseType.Ok == dlg.Run())
			{
				log = dlg.Log;
				ProcessOfflineLog (m_selectedCache, log, dlg.FTF);
				dlg.Hide();
			}
			dlg.Hide();
			dlg.Dispose();		
		}
		
		private void ProcessOfflineLog (Geocache cache, CacheLog log, bool ftf)
		{
				FieldNotesHandler.WriteToFile(log , m_conf.FieldNotesFile);
			
				if (cache == null)
					return;
				CacheStore store = Engine.getInstance().Store;
				store.AddLogAtomic(log.CacheCode, log);
				if (log.LogStatus == "Found it")
				{
					cache.DNF = false;
					cache.FTF = ftf;
					cache.Symbol = "Geocache Found";
					store.UpdateCacheAtomic(cache);
					store.UpdateWaypointAtomic(cache);
				}
				else if (log.LogStatus == "Didn't find it")
				{
					cache.DNF = true;
					cache.FTF = false;
					cache.Symbol = "Geocache";
					store.UpdateCacheAtomic(cache);
					store.UpdateWaypointAtomic(cache);
				}
				else if (log.LogStatus == "Needs Maintenance")
				{
					cache.CheckNotes = true;
				}
		}
		
		private void LogCacheOnline ()
		{
			MarkCacheFound();
			if (m_selectedCache.URL == null)
				return;
			LoggingDialog dlg = new LoggingDialog();
			dlg.LogCache(m_selectedCache);
			dlg.Run();
		}

		public void MarkCacheFound ()
		{
			MarkFoundDialog dlg = new MarkFoundDialog();
			dlg.DialogLabel = String.Format("Do you wish to mark {0} as found?", m_selectedCache.Name);
			dlg.LogDate = m_loggingdate;
			if ((int)ResponseType.Cancel == dlg.Run ()) 
			{
				dlg.Hide();
				return;
			}
			dlg.Hide();
			m_loggingdate = dlg.LogDate;
			m_selectedCache.FTF = false;
			m_selectedCache.DNF = false;
			m_selectedCache.Symbol = "Geocache Found";
			CacheLog log = new CacheLog ();
			log.FinderID = OwnerID;
			log.LogDate = dlg.LogDate;
			log.LoggedBy = "OCM";
			log.LogStatus = "Found it";
			log.LogMessage = "AUTO LOG: OCM";
			log.LogKey = m_selectedCache.Name + log.LogDate.ToFileTime().ToString();
			Engine.getInstance ().Store.UpdateWaypointAtomic (m_selectedCache);
			Engine.getInstance ().Store.UpdateCacheAtomic (m_selectedCache);
			Engine.getInstance ().Store.AddLogAtomic (m_selectedCache.Name, log);
			SetSelectedCache(m_selectedCache);
			if (m_showNearby)
				GetNearByCaches();
			UpdateStatusBar();
			m_mainWin.QueueDraw();
		}

		public void MarkCacheUnfound ()
		{
			MessageDialog dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Are you sure you want to mark " + m_selectedCache.Name + " as unfound?");
			if ((int)ResponseType.Yes == dlg.Run ()) {
				m_selectedCache.Symbol = "Geocache";
				m_selectedCache.FTF = false;
				m_selectedCache.DNF = false;
				Engine.getInstance ().Store.UpdateWaypointAtomic (m_selectedCache);
				Engine.getInstance ().Store.UpdateCacheAtomic (m_selectedCache);
				SetSelectedCache(m_selectedCache);
				UpdateStatusBar();
				if (m_showNearby)
					GetNearByCaches();
				m_mainWin.QueueDraw();
			}
			dlg.Hide ();
		}
		
		public void MarkCacheFTF ()
		{
			MarkFoundDialog dlg = new MarkFoundDialog();
			dlg.Title = Catalog.GetString("Mark First to Find");
			dlg.DialogLabel = String.Format("Do you wish to mark {0} as first to find?", m_selectedCache.Name);
			dlg.LogDate = m_loggingdate;
			if ((int)ResponseType.Cancel == dlg.Run ()) 
			{
				dlg.Hide();
				return;
			}
			dlg.Hide();
			m_loggingdate = dlg.LogDate;
			m_selectedCache.Symbol = "Geocache Found";
			m_selectedCache.FTF = true;
			m_selectedCache.DNF = false;
			CacheLog log = new CacheLog ();
			log.FinderID = OwnerID;
			log.LogDate = dlg.LogDate;
			log.LoggedBy = "OCM";
			log.LogStatus = "Found it";
			log.LogMessage = "AUTO LOG: OCM";
			log.LogKey = m_selectedCache.Name + log.LogDate.ToFileTime().ToString();
			Engine.getInstance ().Store.UpdateWaypointAtomic (m_selectedCache);
			Engine.getInstance ().Store.UpdateCacheAtomic (m_selectedCache);
			Engine.getInstance ().Store.AddLogAtomic (m_selectedCache.Name, log);
			SetSelectedCache(m_selectedCache);
			if (m_showNearby)
				GetNearByCaches();
			UpdateStatusBar();
			m_mainWin.QueueDraw();
		}
		
		public void MarkCacheDNF()
		{
			MarkFoundDialog dlg = new MarkFoundDialog();
			dlg.Title = Catalog.GetString("Mark Did Not Find");
			dlg.DialogLabel = String.Format("Do you wish to mark {0} as did not find?", m_selectedCache.Name);
			dlg.LogDate = m_loggingdate;
			if ((int)ResponseType.Cancel == dlg.Run ()) 
			{
				dlg.Hide();
				return;
			}
			dlg.Hide();
			m_loggingdate = dlg.LogDate;
			m_selectedCache.Symbol = "Geocache";
			m_selectedCache.FTF = false;
			m_selectedCache.DNF = true;
			CacheLog log = new CacheLog ();
			log.FinderID = OwnerID;
			log.LogDate = dlg.LogDate;
			log.LoggedBy = "OCM";
			log.LogStatus = "Didn't find it";
			log.LogMessage = "AUTO LOG: OCM";
			log.LogKey = m_selectedCache.Name + log.LogDate.ToFileTime().ToString();
			Engine.getInstance ().Store.UpdateWaypointAtomic (m_selectedCache);
			Engine.getInstance ().Store.UpdateCacheAtomic (m_selectedCache);
			Engine.getInstance ().Store.AddLogAtomic (m_selectedCache.Name, log);
			SetSelectedCache(m_selectedCache);
			if (m_showNearby)
				GetNearByCaches();
			UpdateStatusBar();
			m_mainWin.QueueDraw();
		}
		
		public void MarkCacheDisabled ()
		{
			MessageDialog dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Are you sure you want to mark " + m_selectedCache.Name + " as disabled?");
			if ((int)ResponseType.Yes == dlg.Run ()) {
				m_selectedCache.Available = false;
				m_selectedCache.Archived = false;
				Engine.getInstance ().Store.UpdateCacheAtomic (m_selectedCache);
				SetSelectedCache(m_selectedCache);
				if (m_showNearby)
					GetNearByCaches();
				m_mainWin.QueueDraw();
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
				if (m_showNearby)
					GetNearByCaches();
				m_mainWin.QueueDraw();
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
				if (m_showNearby)
					GetNearByCaches();
				m_mainWin.QueueDraw();
			}
			dlg.Hide ();
		}

		public void DeleteCache ()
		{
			MessageDialog dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, String.Format(Catalog.GetString("Are you sure you want to delete {0}?"), m_selectedCache.Name));
			if ((int)ResponseType.Yes == dlg.Run ()) {
				Engine.getInstance ().Store.DeleteGeocacheAtomic (m_selectedCache);
				RefreshCaches ();
			}
			dlg.Hide ();
		}

		public void SetAdditonalFilters ()
		{
			CacheStore store = Engine.getInstance ().Store;
			FilterDialog dlg = new FilterDialog ();
			dlg.Filter = store.Filter;
			dlg.Parent = m_mainWin;
			dlg.Icon = m_mainWin.Icon;
			if (((int)ResponseType.Ok) == dlg.Run ()) {
				store.Filter = dlg.Filter;
				CheckAdvancedFilters (dlg.Filter);
				RefreshCaches ();
			}
		}
		
		private void CheckAdvancedFilters (FilterList filters)
		{
			if (filters !=null && filters.GetCount() > 1)
				{
					m_cachelist.ShowInfoBox();
					m_mainWin.SetAllowClearFilter (true);
				}
				else
				{
					m_cachelist.HideInfoBox();
					m_mainWin.SetAllowClearFilter (false);
				}
		}

		public void ClearFilters ()
		{
			MessageDialog dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, Catalog.GetString("Are you sure you want to clear all advanced filters?"));
			if ((int)ResponseType.Yes == dlg.Run ()) {
				dlg.Hide ();
				Engine.getInstance ().Store.Filter = null;
				m_mainWin.SetAllowClearFilter (false);
				m_cachelist.HideInfoBox();
				RefreshCaches ();
			} else
				dlg.Hide ();
			dlg.Dispose ();
		}

		public void StartProgressLoad (String msg, bool isDB)
		{
			m_progress.Show ();
			m_progress.Fraction = 0;
			m_progress.Text = msg;
			if (isDB)
				m_blockMapProgress = true;
			DoGUIUpdate ();
		}

		public void SetProgress (double progress, double total, string msg, bool isDB)
		{
			if (!isDB && m_blockMapProgress)
				return;
			if (!m_progress.Visible)
				m_progress.Show();
			m_progress.Fraction = (progress / total);
			m_progress.Text = String.Format (msg, progress.ToString ());
			DoGUIUpdate ();
		}
		
		public void SetProgressPulse ()
		{
			if (!m_progress.Visible)
				m_progress.Show();
			m_progress.Pulse();
			DoGUIUpdate ();
		}

		public void SetProgressDone (bool isDB)
		{
			if (isDB)
				m_blockMapProgress = false;
			m_progress.Hide ();
			DoGUIUpdate ();
		}

		public void GetNearByCaches ()
		{
			if (m_showNearby) {
				m_map.LoadScript ("clearExtraMarkers()");
				List<Geocache> visibleCaches = GetVisibleCacheList ();
				if (visibleCaches.Count <= 0)
					return;
				visibleCaches.Sort (new CacheDistanceSorter (m_currLat, m_currLon));
				int count = 0;
				IEnumerator<Geocache> cache = visibleCaches.GetEnumerator ();
				while (cache.MoveNext ()) {
					if (m_selectedCache != null && cache.Current.Name == m_selectedCache.Name)
						continue;
					else
						AddOtherCacheToMap (cache.Current);
					if (count < m_MapPoints)
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
		
		public void AddGPSProfile()
		{
			GPSConfiguration dlg = new GPSConfiguration();
			dlg.Parent = m_mainWin;
			dlg.Icon = m_mainWin.Icon;
			if ((int) ResponseType.Ok == dlg.Run())
			{
				GPSProfile prof = new GPSProfile();
				prof.Name = dlg.ProfileName;
				prof.BabelFormat = dlg.GPSConfig.GetBabelFormat();
				prof.CacheLimit = dlg.GPSConfig.GetCacheLimit();
				prof.NameMode = dlg.GPSConfig.GetNameMode();
				prof.DescMode = dlg.GPSConfig.GetDescMode();
				prof.LogLimit = dlg.GPSConfig.GetLogLimit();
				prof.IncludeAttributes = dlg.GPSConfig.IncludeAttributes();
				prof.OutputFile = dlg.GPSConfig.GetOutputFile();
				prof.FieldNotesFile = dlg.GPSConfig.FieldNotesFile;
				prof.WaypointMappings = dlg.GPSMappings;
				if (m_conf.GPSProf == null)
					m_conf.GPSProf = prof.Name;
				m_profiles.AddProfile(prof);
				m_mainWin.RebuildProfEditMenu(m_profiles);
				m_mainWin.RebuildProfilesMenu(m_profiles);
			}
		}
		
		public void EditProfile(GPSProfile prof)
		{
			GPSConfiguration dlg = new GPSConfiguration(prof);
			dlg.Parent = m_mainWin;
			dlg.Icon = m_mainWin.Icon;
			dlg.Title = Catalog.GetString("Edit GPS Profile...");
			if ((int) ResponseType.Ok == dlg.Run())
			{
				string origName = prof.Name;
				bool isActive = false;
				if ((m_profiles.GetActiveProfile()) != null && (m_profiles.GetActiveProfile().Name == origName))
				    isActive = true;
				prof.Name = dlg.ProfileName;
				prof.BabelFormat = dlg.GPSConfig.GetBabelFormat();
				prof.CacheLimit = dlg.GPSConfig.GetCacheLimit();
				prof.NameMode = dlg.GPSConfig.GetNameMode();
				prof.DescMode = dlg.GPSConfig.GetDescMode();
				prof.LogLimit = dlg.GPSConfig.GetLogLimit();
				prof.IncludeAttributes = dlg.GPSConfig.IncludeAttributes();
				prof.OutputFile = dlg.GPSConfig.GetOutputFile();
				prof.WaypointMappings = dlg.GPSMappings;
				prof.FieldNotesFile = dlg.GPSConfig.FieldNotesFile;
				if (origName == prof.Name)
				{
					m_profiles.UpdateProfile(prof);
				}
				else
				{
					m_profiles.DeleteProfile(origName);
					m_profiles.AddProfile(prof);
					if (isActive)
						m_conf.GPSProf = prof.Name;
				}
				m_mainWin.RebuildProfEditMenu(m_profiles);
				m_mainWin.RebuildProfilesMenu(m_profiles);
			}
		}
		
		public void DeleteGPSProfile()
		{
			DeleteItem dlg = new DeleteItem(m_profiles);
			if ((int)ResponseType.Ok == dlg.Run())
			{
				if ((m_profiles.GetActiveProfile() != null) &&(dlg.Bookmark == m_profiles.GetActiveProfile().Name))
				{
					MessageDialog confirm = new MessageDialog(m_mainWin, DialogFlags.Modal, MessageType.Warning,
					                                          ButtonsType.YesNo, Catalog.GetString("\"{0}\" is the active" +
					                                          	" GPS profile. Are you sure you wish to delete it?"),
					                                           dlg.Bookmark);
					if ((int) ResponseType.No == confirm.Run())
					{
						confirm.Hide();
						confirm.Dispose();
						return;
					}
					confirm.Hide();
					confirm.Dispose();
					m_conf.GPSProf = null;
				}
				
				m_profiles.DeleteProfile(dlg.Bookmark);
				m_mainWin.RebuildProfEditMenu(m_profiles);
				m_mainWin.RebuildProfilesMenu(m_profiles);
			}
		}
		
		public void SendToGPS()
		{
			if (m_profiles.GetActiveProfile() == null)
			{
				MessageDialog err = new MessageDialog(m_mainWin, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, 
				                                      Catalog.GetString("There is no active GPS profile. Either select an" +
				                                      	" existing one from the GPS Menu or add a new profile ."));
				err.Run();
				err.Hide();
				err.Dispose();
				return;
			}
			SendWaypointsDialog dlg = new SendWaypointsDialog();
			dlg.Parent = m_mainWin;
			dlg.Icon = m_mainWin.Icon;
			dlg.AutoClose = m_conf.AutoCloseWindows;
			dlg.Start(m_cachelist.getVisibleCaches(), m_profiles.GetActiveProfile());
		}
		
		public void ShowPreferences()
		{
			Preferences dlg = new Preferences(m_conf, m_filters);
			int oldInterval = m_conf.UpdateInterval;	
			if ((int) ResponseType.Ok == dlg.Run())
			{
				if (m_conf.UpdateInterval != oldInterval)
				{
					m_conf.NextUpdateCheck = DateTime.Now.AddDays(m_conf.UpdateInterval);
				}
				m_useImperial = m_conf.ImperialUnits;
				m_ownerid = m_conf.OwnerID;
				m_showNearby = m_conf.ShowNearby;
				m_ShowAllChildren = m_conf.ShowAllChildren;
				m_MapPoints = m_conf.MapPoints;
				if (m_useImperial)
				{
					m_cachelist.SetImperial();
				}
				else
				{
					m_cachelist.SetMetric();
				}
				LoadMap(m_conf.MapType);
				RecenterMap ();
				RefreshCaches();			
			}
			dlg.Dispose();
		}
		
		public void RecenterMap ()
		{
			CentreLat = m_conf.LastLat;
			CentreLon = m_conf.LastLon;
			CenterName = m_conf.LastName;
			if (CentreLat == 0 && CentreLon == 0)
			{
				CentreLat = m_conf.HomeLat;
				CentreLon = m_conf.HomeLon;
				CenterName = Catalog.GetString("Home");
			}
			if (m_centreName != Catalog.GetString("Home"))
			{
				m_mainWin.EnableResetCentre();
			}
			GoHome();
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
			DeleteItem dlg = new DeleteItem();
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
				DoGUIUpdate();
				CopyingProgress ddlg = new CopyingProgress();
				ddlg.StartDelete(GetVisibleCacheList());
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
			m_gpsTimer = new Timer(m_conf.GPSDPoll * 1000);
			m_gpsTimer.AutoReset = true;
			m_gpsTimer.Enabled = true;
			m_gpsTimer.Elapsed += HandleM_gpsTimerElapsed;
			m_centreName = "GPS";
			m_centerLat = m_gps.Lat;
			m_centerLon = m_gps.Lon;
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
			m_centerLat = m_gps.Lat;
			m_centerLon = m_gps.Lon;
			m_cachelist.RefilterList();
			bool recenterMap = m_conf.GPSDAutoMoveMap;
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
			m_conf.WindowWidth = m_width;
			m_conf.WindowHeight = m_height;
			m_conf.HBarPosition = m_mainWin.HPos;
			m_conf.VBarPosition = m_mainWin.VPos;
		}
		
		public void ConfigureGPSD()
		{
			/*GPSDConfig dlg = new GPSDConfig();
			dlg.Icon = m_mainWin.Icon;
			dlg.GPSDOnStartup = (bool) m_conf.Get("/apps/ocm/gpsd/onstartup", false);
			dlg.RecenterMap = (bool) m_conf.Get("/apps/ocm/gpsd/recenter", true);
			dlg.PollInterval = (int) m_conf.Get("/apps/ocm/gpsd/poll", 30);
			if ((int) ResponseType.Ok == dlg.Run())
			{
				m_conf.Set("/apps/ocm/gpsd/onstartup", dlg.GPSDOnStartup);
				m_conf.Set("/apps/ocm/gpsd/recenter", dlg.RecenterMap);
				m_conf.Set("/apps/ocm/gpsd/poll", dlg.PollInterval);
			}*/
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
			dlg.Start(tempFile, GetVisibleCacheList(), GPSProfileList.GetDefaultMappings());
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
			dlg.Start(tempFile, cache, GPSProfileList.GetDefaultMappings());
		}
		
		public void ApplyQuickFilter(QuickFilter filter)
		{
			m_cachelist.ApplyQuickFilter(filter);
			CacheStore store = Engine.getInstance().Store;
			bool needsReload = false;
			if (store.Filter != null)
				needsReload = true;
			store.Filter = filter.AdvancedFilters;
			CheckAdvancedFilters(filter.AdvancedFilters);
			if (filter.AdvancedFilters != null || needsReload)
				RefreshCaches();
			else
				m_cachelist.RefilterList();
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
			DeleteItem dlg = new DeleteItem(m_filters);
			dlg.Title = Catalog.GetString("Delete QuickFilter");
			if ((int) ResponseType.Ok == dlg.Run())
			{
				m_filters.DeleteFilter(dlg.Bookmark);
				m_mainWin.RebuildQuickFilterMenu(m_filters);
			}
		}
		
		public void CompactDB()
		{
			Engine.getInstance().Store.Compact();
			MessageDialog dlg = new MessageDialog(m_mainWin, DialogFlags.Modal, 
			                                      MessageType.Info, ButtonsType.Ok,
			                                      Catalog.GetString("Database Compacted."));
			dlg.Run();
			dlg.Destroy();
		}
		
		public void CopyToDB()
		{
			CopyMoveDialog dlg = new CopyMoveDialog();
			dlg.Title = "Copy Caches to Another Database...";
			dlg.Filename = 	m_conf.DataDirectory;

			if ((int)ResponseType.Ok == dlg.Run())
			{
				CopyingProgress cp = new CopyingProgress();
				cp.Start(dlg.Filename, false, dlg.Mode);
			}
		}
		
		public void MoveToDB()
		{
			CopyMoveDialog dlg = new CopyMoveDialog();
			dlg.Title = Catalog.GetString("Move Geocaches...");
			dlg.Title = "Move Caches to Another Database...";
			dlg.Filename = m_conf.DataDirectory;
			if ((int)ResponseType.Ok == dlg.Run())
			{
				CopyingProgress cp = new CopyingProgress();
				cp.Start(dlg.Filename, true, CopyingProgress.ModeEnum.VISIBLE);
				RefreshCaches();
			}
			
		}
		
		public void ConfigureETools()
		{
			ConfigureEToolsDlg dlg = new ConfigureEToolsDlg();
			dlg.Run();
		}
		
		public static void ViewOCMWiki()
		{
			Process.Start("http://sourceforge.net/apps/mediawiki/opencachemanage/");
		}
		
		public void SetMapCentre(double lat, double lon)
		{
			SetMapCentre(lat, lon, null);
		}
		
		public void SetMapCentre(double lat, double lon, string name)
		{
			CentreLat = lat;
			CentreLon = lon;
			if (name == null)
				CenterName = Catalog.GetString("Map Point");
			else
				CenterName = name;
			SaveCenterPos();
			m_mainWin.EnableResetCentre();
			RecenterMap();
			RefreshCaches();
		}
		
		public void SetHome(double lat, double lon)
		{
			CentreLat = lat;
			CentreLon = lon;
			CenterName = Catalog.GetString("Home");
			m_conf.HomeLat = lat;
			m_conf.HomeLon = lon;
			SaveCenterPos();
			RefreshCaches();
		}
		
		public void PrintCache()
		{
			printing.CachePrinter printer = new printing.CachePrinter();
			printer.StartPrinting(m_selectedCache, m_mainWin);
		}
		
		public void AutoCheckForUpdates()
		{
			try
			{
				if (!m_conf.CheckForUpdates)
					return;
				if (DateTime.Now < m_conf.NextUpdateCheck)
						return;
				string ver = UpdateChecker.GetLatestVer();
				if (ver != GetOCMVersion())
				{
					MessageDialog dlg = new MessageDialog(m_mainWin, DialogFlags.Modal,
					                                      MessageType.Info, ButtonsType.YesNo,
					                                      Catalog.GetString("A new version \"{0}\" of OCM is available" +
					                                                        "\nWould you like to go to the download page now?"),
					                                      					ver);
					if ((int)ResponseType.Yes == dlg.Run())
					{
						dlg.Hide();
						Process.Start("http://sourceforge.net/projects/opencachemanage/files/");
					}
					else
						dlg.Hide();
				}
				m_conf.NextUpdateCheck = DateTime.Now.AddDays(m_conf.UpdateInterval);
			}
			catch (Exception)
			{
				MessageDialog dlg = new MessageDialog(m_mainWin, DialogFlags.Modal,
					                                      MessageType.Error, ButtonsType.Ok,
					                                      Catalog.GetString("Unable to check for updates, check your " +
					                                      	"network connection."));
				dlg.Run();
				dlg.Hide();
			}
		}
		
		public void CheckForUpdates()
		{
			try
			{
				string ver = UpdateChecker.GetLatestVer();
				if (ver != GetOCMVersion())
				{
					MessageDialog dlg = new MessageDialog(m_mainWin, DialogFlags.Modal,
					                                      MessageType.Info, ButtonsType.YesNo,
					                                      Catalog.GetString("A new version \"{0}\" of OCM is available" +
					                                                        "\nWould you like to go to the download page now?"),
					                                      					ver);
					if ((int)ResponseType.Yes == dlg.Run())
					{
						dlg.Hide();
						Process.Start("http://sourceforge.net/projects/opencachemanage/files/");
					}
					else
						dlg.Hide();
				}
				else
				{
					MessageDialog dlg = new MessageDialog(m_mainWin, DialogFlags.Modal,
					                                      MessageType.Info, ButtonsType.Ok,
					                                      Catalog.GetString("OCM is at the latest version."));
					dlg.Run();
					dlg.Hide();
				}
			}
			catch (Exception)
			{
				MessageDialog dlg = new MessageDialog(m_mainWin, DialogFlags.Modal,
					                                      MessageType.Error, ButtonsType.Ok,
					                                      Catalog.GetString("Unable to check for updates, check your " +
					                                      	"network connection."));
				dlg.Run();
				dlg.Hide();
			}
		}
		
		public void CorrectCoordinates()
		{
			if (m_selectedCache == null) {
				MessageDialog mdlg = new MessageDialog(m_mainWin, DialogFlags.Modal, MessageType.Error,
					                                       ButtonsType.Ok, Catalog.GetString("You have to select a cache first."));
				mdlg.Run();
				mdlg.Destroy();
				return;
			}

			CorrectedCoordinatesDlg dlg = new CorrectedCoordinatesDlg();
			dlg.SetCache(m_selectedCache);
			if ((int) ResponseType.Ok == dlg.Run())
			{
				if (dlg.IsCorrected)
				{
					m_selectedCache.CorrectedLat = dlg.CorrectedLat;
					m_selectedCache.CorrectedLon = dlg.CorrectedLon;
				}
				else
				{
					m_selectedCache.HasCorrected = false;
				}
				Engine.getInstance().Store.UpdateCacheAtomic(m_selectedCache);
				SetSelectedCache(m_selectedCache);
			}
			dlg.Hide();
		}
		
		public void CorrectCoordinates(double lat, double lon)
		{
			if (m_selectedCache == null) {
				MessageDialog mdlg = new MessageDialog(m_mainWin, DialogFlags.Modal, MessageType.Error,
					                                       ButtonsType.Ok, Catalog.GetString("You have to select a cache first."));
				mdlg.Run();
				mdlg.Destroy();
				return;
			}
			m_selectedCache.CorrectedLat = lat;
			m_selectedCache.CorrectedLon = lon;
			Engine.getInstance().Store.UpdateCacheAtomic(m_selectedCache);
			SetSelectedCache(m_selectedCache);
			CorrectCoordinates(); // Opens up the GUI for editing/approvel
		}
		 
		public void ImportZip (string filename)
		{
			String tempPath = System.IO.Path.GetTempPath();
			ProcessStartInfo start = new ProcessStartInfo();
			start.FileName = "unzip";
			start.Arguments = "-o " + filename + " -d " + tempPath + "ocm_unzip";
			Process unzip =  Process.Start(start);
			
			while (!unzip.HasExited)
			{
				// Do nothing until exit	
			}
			
			ImportDirectory(tempPath + "ocm_unzip", true, m_conf.AutoCloseWindows);
		}
		
		public void ShowOtherChildWaypoints (Geocache cache)
		{
			List<Waypoint> wpt = Engine.getInstance ().Store.GetChildren(cache.Name);
			IEnumerator<Waypoint> wptenum = wpt.GetEnumerator();
			while (wptenum.MoveNext ()) {
				AddOtherMapWayPoint (cache, wptenum.Current);	
			}
		}
		
		public void ImportDirectory()
		{
			ImportDirectoryDialog dlg = new ImportDirectoryDialog();
			dlg.IgnoreExtraFields = m_conf.ImportIgnoreExtraFields;
			dlg.PreventStatusOverwrite = m_conf.ImportPreventStatusOverwrite;
			dlg.PurgeOldLogs = m_conf.ImportPurgeOldLogs;
			dlg.Directory = m_conf.ImportDirectory;
			dlg.DeleteOnCompletion = m_conf.ImportDeleteFiles;
			if (dlg.Run () == (int)ResponseType.Ok) {
				dlg.Hide();
				m_conf.ImportIgnoreExtraFields = dlg.IgnoreExtraFields;
				m_conf.ImportPreventStatusOverwrite = dlg.PreventStatusOverwrite;
				m_conf.ImportPurgeOldLogs = dlg.PurgeOldLogs;
				m_conf.ImportDeleteFiles = dlg.DeleteOnCompletion;
				ImportDirectory(dlg.Directory, dlg.DeleteOnCompletion, m_conf.AutoCloseWindows);
			}
			dlg.Destroy ();
	
		}
		
		private void ImportDirectory(String path, bool delete, bool autoClose)
		{
				GPXParser parser = new GPXParser ();
				parser.IgnoreExtraFields = m_conf.ImportIgnoreExtraFields;
				parser.PreserveFound = m_conf.ImportPreventStatusOverwrite;
				parser.PurgeLogs = m_conf.ImportPurgeOldLogs;
				parser.CacheOwner = OwnerID;
				ProgressDialog pdlg = new ProgressDialog (parser);
				pdlg.Icon = m_mainWin.Icon;
				pdlg.AutoClose = autoClose;
				pdlg.Modal = true;
				pdlg.StartMulti(path, Engine.getInstance().Store, delete);
				RefreshCaches ();
		}
		
		
		public void AddChildWaypoint(double lat, double lon)
		{
			if (m_selectedCache == null) {
				MessageDialog mdlg = new MessageDialog(m_mainWin, DialogFlags.Modal, MessageType.Error,
					                                       ButtonsType.Ok, Catalog.GetString("You have to select a cache first."));
				mdlg.Run();
				mdlg.Destroy();
				return;
			}

			try {
				Waypoint newPoint = new Waypoint ();
				Geocache parent = m_selectedCache;
				newPoint.Symbol = "Final Location";
				newPoint.Parent = parent.Name;
				newPoint.Lat = lat;
				newPoint.Lon = lon;	
				String name = "FL" + parent.Name.Substring (2);
				WaypointDialog dlg = new WaypointDialog ();
				if (m_conf.IgnoreWaypointPrefixes)
				{
					name = parent.Name;
					dlg.IgnorePrefix = true;
				}
				name = Engine.getInstance().Store.GenerateNewName(name);
				newPoint.Name = name;
				dlg.SetPoint (newPoint);
				if ((int)ResponseType.Ok == dlg.Run ()) {
					newPoint = dlg.GetPoint ();
					if (newPoint.Symbol == "Final Location")
						m_selectedCache.HasFinal = true;
					CacheStore store = Engine.getInstance ().Store;
					store.AddWaypointAtomic (newPoint);
					dlg.Dispose ();
					m_pane.SetCacheSelected();
					m_mainWin.QueueDraw();
					if (m_showNearby)
						GetNearByCaches();
				}
			} catch (Exception ex) {
				UIMonitor.ShowException (ex);
			}
		}
		
		public void AddChildWaypoint()
		{
			AddChildWaypoint(m_selectedCache.Lat, m_selectedCache.Lon);
		}
		
		public void AddLocation()
		{
			AddLocation(m_currLat, m_currLon);
		}
		
		public void AddLocation(double lat, double lon)
		{
			AddLocationDialog dlg = new AddLocationDialog();
			dlg.Location.Latitude = lat;
			dlg.Location.Longitude = lon;
			if ((int)ResponseType.Ok == dlg.Run())
			{
				Location newLoc = new Location();
				newLoc.Latitude = dlg.Location.Latitude;
				newLoc.Longitude = dlg.Location.Longitude;
				newLoc.Name = dlg.LocationName;
				m_locations.AddLocation(newLoc);
				m_mainWin.RebuildLocationsMenu(m_locations);
				SetLocation(newLoc);
			}
		}
		
		public void RemoveLocation()
		{
			DeleteItem dlg = new DeleteItem(m_locations);
			dlg.Title = Catalog.GetString("Delete Location....");
			if ((int) ResponseType.Ok == dlg.Run())
			{
				m_locations.DeleteLocation(dlg.Bookmark);
				m_mainWin.RebuildLocationsMenu(m_locations);
			}
			dlg.Dispose();
		}
		
		
		public void SetLocation(Location loc)
		{
			SetMapCentre(loc.Latitude, loc.Longitude, loc.Name);
		}
		
		public void ViewOfflineLogs()
		{
			if (!System.IO.File.Exists(m_conf.FieldNotesFile))
			{
				MessageDialog mdlg = new MessageDialog(m_mainWin,DialogFlags.Modal, MessageType.Info, ButtonsType.Ok,
				                                      Catalog.GetString("There are no field notes."));
				mdlg.Run();
				mdlg.Hide();
				return;
			}
			
			List<CacheLog> logs = FieldNotesHandler.GetLogs(m_conf.FieldNotesFile);
			OffLineLogViewer dlg = new OffLineLogViewer(this);
			dlg.PopulateLogs(logs);
			dlg.Run();
		}
		
		public void ReceiveGPSFieldNotes()
		{
			
			LoadGPSFieldNotes dlg = new LoadGPSFieldNotes();
			dlg.LastScan = m_profiles.GetActiveProfile().LastFieldNoteScan;
			try
			{
				if ((int) ResponseType.Ok == dlg.Run())
				{
					dlg.Hide();
					GPSProfile prof = m_profiles.GetActiveProfile();
					List<CacheLog> logs = FieldNotesHandler.GetLogs(prof.FieldNotesFile);
					int iCount = 0;
					DateTime latestScan = dlg.LastScan;
					foreach(CacheLog log in logs)
					{
						if (log.LogDate < dlg.LastScan)
							continue;
						Geocache cache = Engine.getInstance().Store.GetCache(log.CacheCode);
						ProcessOfflineLog(cache, log, false);
						if (latestScan < log.LogDate)
							latestScan = log.LogDate;
						iCount ++;
					}
					m_profiles.GetActiveProfile().LastFieldNoteScan = latestScan;
					m_profiles.UpdateProfile(prof);
					MessageDialog mdlg = new MessageDialog(m_mainWin, DialogFlags.Modal, MessageType.Info,
					                                       ButtonsType.Ok, Catalog.GetString("Processed {0} field notes."),
					                                       iCount.ToString());
					mdlg.Run();
					mdlg.Hide();
					RefreshCaches();
				}
				dlg.Hide();
				dlg.Dispose();
			}
			catch (Exception e)
			{
				ShowException(e);
			}
		}
		
		public void ClearFieldNotes()
		{
			MessageDialog dlg = new MessageDialog(m_mainWin, DialogFlags.Modal, MessageType.Warning, ButtonsType.YesNo,
			                                      Catalog.GetString("Are you sure you want to clear all Field Notes?"));
			if ((int)ResponseType.Yes == dlg.Run())
			{
				FieldNotesHandler.ClearFieldNotes(m_conf.FieldNotesFile);
			}
			dlg.Hide();
			dlg.Dispose();
		}
	}
}
