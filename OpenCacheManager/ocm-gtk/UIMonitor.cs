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



		private double m_home_lat = 46.49;
		private double m_home_lon = -81.01;
		private double m_mapLat = 46.49;
		private double m_mapLon = -81.01;
		private String m_ownerid = "0";
		private CacheList m_cachelist;
		private Geocache m_selectedCache;
		private MainWindow m_mainWin;
		private BrowserWidget m_map;
		private GConf.Client m_conf;
		private Label m_centreLabel;
		private ProgressBar m_progress;
		private bool m_showNearby;
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

		#endregion

		/// <summary>
		/// This class cannot be constructed directly. Use GetInstance()
		/// to get a UIMonitor
		/// </summary>
		private UIMonitor ()
		{
			m_conf = new GConf.Client ();
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
			GConf.Client client = new GConf.Client ();
			
			try {
				CentreLat = (double)client.Get ("/apps/ocm/homelat");
				CentreLon = (double)client.Get ("/apps/ocm/homelon");
				OwnerID = (string)client.Get ("/apps/ocm/memberid");
				String dbName = (string)client.Get ("/apps/ocm/currentdb");
				SetCurrentDB (dbName, true);
				m_map.LoadUrl ("file://" + System.Environment.CurrentDirectory + "/web/wpt_viewer.html");
				SetSelectedCache(null);
				GoHome ();
			} catch (GConf.NoSuchKeyException) {
				// Do nothing
			}
		}

		private void SetCurrentDB (string dbName, bool loadNow)
		{
			String[] dbSplit = dbName.Split ('/');
			String dBShort = dbSplit[dbSplit.Length - 1];
			m_mainWin.Title = dBShort + " - OCM";
			m_conf.Set ("/apps/ocm/currentdb", dbName);
			Engine.getInstance ().Store.SetDB (dbName);
			if (loadNow)
				RefreshCaches ();
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
			m_mainWin.SetCacheSelected ();
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
			m_map.LoadScript ("zoomToPoint(" + lat + "," + lon + ")");
		}

		public void GoHome ()
		{
			m_mapLat = CentreLat;
			m_mapLat = CentreLon;
			m_map.LoadScript ("goHome(" + CentreLat + "," + CentreLon + ")");
		}
		
		public void ZoomToSelected ()
		{
			if (m_selectedCache == null)
				return;
			m_mapLat = m_selectedCache.Lat;
			m_mapLat = m_selectedCache.Lon;
			m_map.LoadScript ("zoomToPoint(" + m_selectedCache.Lat + "," +  m_selectedCache.Lon + ")");
		}
		
		public void ResetCenterToHome()
		{
			CentreLat = (double)m_conf.Get ("/apps/ocm/homelat");
			CentreLon = (double)m_conf.Get ("/apps/ocm/homelon");
			Geocache selected = m_selectedCache;
			m_centreName = "Home";
			RefreshCaches();
			SelectCache(selected.Name);
		}
		
		public void SetSelectedAsCentre()
		{
			m_home_lat = m_selectedCache.Lat;
			m_home_lon = m_selectedCache.Lon;
			Geocache selected = m_selectedCache;
			m_centreName = selected.Name;
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
			m_map.LoadScript ("addMarker(" + pt.Lat + "," + pt.Lon + ",'../icons/24x24/" + GetMapIcon (pt.Symbol) + "',\"" + pt.Name + "\",\"" + pt.Desc.Replace("\"","''") + "\")");
		}

		/// <summary>
		/// Displays a specified cache on the map
		/// </summary>
		/// <param name="cache">
		/// The geocache to display <see cref="Geocache"/>
		/// </param>
		public void AddMapCache (Geocache cache)
		{
			string script = "addMarker(" + cache.Lat + "," + cache.Lon + ",'../icons/24x24/" + GetMapIcon (cache) + "',\"" + cache.Name + "\",\"" + cache.Desc.Replace("\"","''") + "\")";
			m_map.LoadScript (script);
		}

		public void AddOtherCacheToMap (Geocache cache)
		{
			m_map.LoadScript ("addExtraMarker(" + cache.Lat + "," + cache.Lon + ",'../icons/24x24/" + GetMapIcon (cache) + "',\"" + cache.Name.Replace("\"","'") + "\",\"" + cache.Desc.Replace("\"","''") + "\")");
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
			m_statusbar.Push (m_statusbar.GetContextId ("count"), String.Format (Catalog.GetString ("Showing {0} of {1} caches, {2} found, {3} unavailable/archived, {4} placed by me"), visible, total, found, inactive, mine));
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
			;
			
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
					edlg.Start (dlg.Filename, GetVisibleCacheList ());
					MessageDialog msg = new MessageDialog (m_mainWin, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, "Export Complete!");
					msg.Run ();
					msg.Hide ();
					RecentManager manager = RecentManager.Default;
					manager.AddItem("file://" + dlg.Filename);
				}
				edlg.Destroy();
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
			filter.Name = "GPS Exchange Files";
		//	filter.AddMimeType ("application/x-gpx");
			filter.AddPattern ("*.gpx");
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

		public void DeleteCache ()
		{
			MessageDialog dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Are you sure you want to delete " + m_selectedCache.Name);
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
			MessageDialog dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Are you sure you want to clear all additional filters?");
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
			dlg.MemberID = (String) m_conf.Get ("/apps/ocm/memberid");
			dlg.Lat = (Double) m_conf.Get ("/apps/ocm/homelat");
			dlg.Lon = (Double) m_conf.Get ("/apps/ocm/homelon");
			if ((int) ResponseType.Ok == dlg.Run())
			{
				m_conf.Set ("/apps/ocm/memberid", dlg.MemberID);
				m_conf.Set ("/apps/ocm/homelat", dlg.Lat);
				m_conf.Set ("/apps/ocm/homelon", dlg.Lon);
				RefreshCaches();
			}
			dlg.Dispose();
		}
	}
}
