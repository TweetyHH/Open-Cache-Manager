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
		private GConf.Client m_conf;
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
				SetCurrentDB (dbName);
			} catch (GConf.NoSuchKeyException) {
				// Do nothing
			}
		}

		private void SetCurrentDB (string dbName)
		{
			String[] dbSplit = dbName.Split('/');
			String dBShort = dbSplit[dbSplit.Length -1];
			m_mainWin.Title = dBShort + " - OpenCacheManager";
			m_conf.Set ("/apps/ocm/currentdb", dbName);
			Engine.getInstance ().Store.SetDB (dbName);
			RefreshCaches ();
		}

		/// <summary>
		/// Refreshes the cache information and list
		/// </summary>
		public void RefreshCaches ()
		{
			m_cachelist.PopulateList ();
			UpdateCacheCountStatus ();
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

		/// <summary>
		/// Adds a waypoint to the map
		/// </summary>
		/// <param name="pt">
		/// The waypoint to display <see cref="Waypoint"/>
		/// </param>
		public void AddMapWayPoint (Waypoint pt)
		{
			m_map.LoadScript ("addMarker(" + pt.Lat + "," + pt.Lon + ",'../icons/24x24/" + GetMapIcon (pt.Symbol) + "')");
		}

		/// <summary>
		/// Displays a specified cache on the map
		/// </summary>
		/// <param name="cache">
		/// The geocache to display <see cref="Geocache"/>
		/// </param>
		public void AddMapCache (Geocache cache)
		{
			m_map.LoadScript ("addMarker(" + cache.Lat + "," + cache.Lon + ",'../icons/24x24/" + GetMapIcon (cache.TypeOfCache) + "')");
		}

		public void StartFiltering ()
		{
			m_statusbar.Push (m_statusbar.GetContextId ("refilter"), "Refiltering, please wait..");
			while (Gtk.Application.EventsPending ())
				Gtk.Application.RunIteration (false);
		}

		/// <summary>
		/// Updates the status bar with the cache count information
		/// </summary>
		public void UpdateCacheCountStatus ()
		{
			Engine engine = Engine.getInstance ();
			int visible = m_cachelist.getVisibleCaches ().Count;
			int total = engine.Store.CacheCount;
			int found = m_cachelist.GetVisibleFoundCacheCount ();
			int inactive = m_cachelist.GetVisibleInactiveCacheCount ();
			int mine = m_cachelist.GetOwnedCount ();
			m_statusbar.Push (m_statusbar.GetContextId ("count"), String.Format (Catalog.GetString ("Showing {0} of {1} caches, {2} found, {3} unavailable/archived, {4} placed by me"), visible, total, found, inactive, mine));
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
		private static string GetMapIcon (Geocache.CacheType type)
		{
			switch (type) {
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
			//TODO: MORE ICONS NEEDED
			return "waypoint-flag-red.png";
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
			filter.AddMimeType ("application/x-sqlite3");
			filter.AddPattern ("*.ocm");
			dlg.AddFilter (filter);
			
			if (dlg.Run () == (int)ResponseType.Accept) {
				Engine.getInstance ().Store.CreateDB (dlg.Filename);
				SetCurrentDB (dlg.Filename);
			}
			dlg.Destroy ();
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
				filter.AddMimeType ("application/x-sqlite3");
				filter.AddPattern ("*.ocm");
				dlg.AddFilter (filter);
				
				if (dlg.Run () == (int)ResponseType.Accept) {
					SetCurrentDB (dlg.Filename);
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
				String lastFile = null;
				try {
					lastFile = (string)m_conf.Get ("/apps/ocm/lastexport");
				} catch (Exception e) {
				}
				
				
				FileChooserDialog dlg = new FileChooserDialog (Catalog.GetString ("Export GPX File"), m_mainWin, FileChooserAction.Save, Catalog.GetString ("Cancel"), ResponseType.Cancel, Catalog.GetString ("Export"), ResponseType.Accept);
				dlg.SetCurrentFolder (System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments));
				if (lastFile == null) {
					dlg.CurrentName = "export.gpx";
				} else {
					dlg.CurrentName = lastFile;
				}
				FileFilter filter = new FileFilter ();
				filter.Name = "GPX Files";
				filter.AddMimeType ("text/xml");
				filter.AddMimeType ("application/xml");
				filter.AddPattern ("*.gpx");
				
				dlg.AddFilter (filter);
				
				if (dlg.Run () == (int)ResponseType.Accept) {
					dlg.Hide ();
					edlg.Start (dlg.Filename, GetVisibleCacheList ());
					MessageDialog msg = new MessageDialog (m_mainWin, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, "Export Complete!");
					msg.Run ();
					msg.Hide ();
					m_conf.Set ("/apps/ocm/lastexport", dlg.Filename);
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
		}

		public void RunSetupAssistant ()
		{
			SetupAssistant assistant = new SetupAssistant ();
			assistant.SetPosition (Gtk.WindowPosition.Center);
			assistant.ShowAll ();
		}

		public void ImportGPXFile (String filename)
		{
			System.IO.FileStream fs = System.IO.File.Open (filename, System.IO.FileMode.Open);
			GPXParser parser = new GPXParser ();
			CacheStore store = Engine.getInstance ().Store;
			ProgressDialog pdlg = new ProgressDialog (parser);
			pdlg.Modal = true;
			pdlg.Start (fs, store);
			RefreshCaches ();
			fs.Close ();
		}

		public void LogFind ()
		{
			MessageDialog dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Do you wish to mark this cache as found in the database?");
			if ((int)ResponseType.Yes == dlg.Run ()) {
				MarkCacheFound ();
				RefreshCaches ();
			}
			dlg.Hide ();
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
			Engine.getInstance ().Store.UpdateWaypoint (m_selectedCache);
			Engine.getInstance ().Store.AddLog (m_selectedCache.Name, log);
		}

		public void MarkCacheUnfound ()
		{
			MessageDialog dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Are you sure you want to mark" + m_selectedCache.Name + "as unfound?");
			if ((int)ResponseType.Yes == dlg.Run ()) {
				m_selectedCache.Symbol = "Geocache";
				Engine.getInstance ().Store.UpdateWaypoint (m_selectedCache);
			}
			dlg.Hide();			
		}
	}
}
