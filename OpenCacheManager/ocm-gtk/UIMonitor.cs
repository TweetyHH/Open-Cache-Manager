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
		#endregion

		#region Properties

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
			set { m_mainWin = value; }
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
			string map = (string) m_conf.Get("/apps/ocm/defmap", "osm");
			String dbName = (string)m_conf.Get ("/apps/ocm/currentdb", String.Empty);
			SetCurrentDB (dbName, true);
			LoadMap (map);
			SetSelectedCache(null);
			if (m_useImperial)
				m_cachelist.SetImperial();
			GoHome ();
		}
		
		private void LoadMap (string map)
		{
			m_map.LoadUrl ("file://" + System.Environment.CurrentDirectory + "/web/wpt_viewer.html?map=" + map);
		}

		private void SetCurrentDB (string dbName, bool loadNow)
		{
			String[] dbSplit = dbName.Split ('/');
			String dBShort = dbSplit[dbSplit.Length - 1];
			m_mainWin.Title = dBShort + " - OCM";
			m_conf.Set ("/apps/ocm/currentdb", dbName);
			CacheStore store = Engine.getInstance().Store;
			store.SetDB(dbName);
			if (store.NeedsUpgrade())
				UpgradeDB(dbName, store);
			Engine.getInstance ().Store.SetDB (dbName);
			m_mainWin.UpdateBookmarkList(store.GetBookmarkLists());
			if (loadNow)
				RefreshCaches ();
		}
		
		private void UpgradeDB(String dbname, CacheStore store)
		{
			MessageDialog dlg = new MessageDialog(m_mainWin, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, Catalog.GetString("OCM needs to upgrade your database.\nWould you like to backup your database first?"));
			if ((int) ResponseType.Yes == dlg.Run())
			{
				System.IO.File.Copy(dbname, dbname + ".bak");
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
			string script = "addMarker(" + cache.Lat.ToString(CultureInfo.InvariantCulture) + "," + cache.Lon.ToString(CultureInfo.InvariantCulture) + ",'../icons/24x24/" + GetMapIcon (cache) + "',\"" + cache.Name + "\",\"" + cache.Desc.Replace("\"","''") + "\")";
			m_map.LoadScript (script);
		}

		public void AddOtherCacheToMap (Geocache cache)
		{
			m_map.LoadScript ("addExtraMarker(" + cache.Lat.ToString(CultureInfo.InvariantCulture) + "," + cache.Lon.ToString(CultureInfo.InvariantCulture) + ",'../icons/24x24/" + GetMapIcon (cache) + "',\"" + cache.Name.Replace("\"","'") + "\",\"" + cache.Desc.Replace("\"","''") + "\")");
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
			if (cache.OwnerID == m_instance.OwnerID)
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
				return "parking.png"; else if (symbol.Equals ("Trailhead"))
				return "trailhead.png"; else if (symbol.Equals ("Final Location"))
				return "bluepin.png";
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
					RegisterRecentFile(dlg.Filename);
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
					edlg.Start (dlg.Filename, GetVisibleCacheList ());
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
			if (m_selectedCache.Symbol == "TerraCache")
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
					if (count < 50)
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
				dlg.Start(m_cachelist.getVisibleCaches(), conf);
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
			dlg.Icon = m_mainWin.Icon;
			if ((int) ResponseType.Ok == dlg.Run())
			{
				m_conf.Set ("/apps/ocm/memberid", dlg.MemberID);
				m_conf.Set ("/apps/ocm/homelat", dlg.Lat);
				m_conf.Set ("/apps/ocm/homelon", dlg.Lon);
				m_conf.Set ("/apps/ocm/imperial", dlg.ImperialUnits);
				m_conf.Set ("/apps/ocm/defmap", dlg.DefaultMap);
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
			dlg.Cache = m_selectedCache;
			if ((int)ResponseType.Ok == dlg.Run())
			{
				m_selectedCache = dlg.Cache;
				Engine.getInstance().Store.UpdateCacheAtomic(dlg.Cache);
				SetSelectedCache(dlg.Cache);
			}
		}
		
		public void AddCache()
		{
			ModifyCacheDialog dlg = new ModifyCacheDialog();
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
	}
}
