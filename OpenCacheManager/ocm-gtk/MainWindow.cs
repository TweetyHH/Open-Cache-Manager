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
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using Gtk;
using Mono.Unix;
using ocmgtk;
using ocmengine;
using NDesk.DBus;
using org.freedesktop.DBus;

public partial class MainWindow : Gtk.Window
{
	UIMonitor m_monitor;
	MenuItem bmrkLists;
	MenuItem addVisibleCaches;
	MenuItem addCacheTo;
	MenuItem removeSelected;
	
	public MenuBar MenuBar
	{
		get { return mainmenubar;}
	}
	
	public int HPos
	{
		get { return hSplitPane.Position;}
		set { hSplitPane.Position = value;}
	}
	
	public int VPos
	{
		get { return vPane.Position;}
		set { vPane.Position = value;}
	}
	
	public MainWindow () : base(Gtk.WindowType.Toplevel)
	{	
		Build ();
		BuildBookMarkMenu();
		m_monitor = UIMonitor.getInstance ();
		m_monitor.Main = this;
		m_monitor.StatusProgress = statProgBar;
		m_monitor.GeoPane = cachePane;
		m_monitor.StatusBar = statusbar1;
		m_monitor.CacheListPane = cacheList;
		m_monitor.CentreLabel = coordLabel;
		m_monitor.Map = mapPane;
		this.ShowAll();
	}

	protected void BuildBookMarkMenu()
	{
		MenuItem bmrks = new MenuItem(Catalog.GetString("_Bookmarks"));
		mainmenubar.Insert(bmrks, 5);	
		Menu bmrksMenu = new Menu();
		MenuItem newBList = new MenuItem(Catalog.GetString("_Create Bookmark List..."));
		MenuItem delBList = new MenuItem(Catalog.GetString("_Delete Bookmark List..."));
		bmrkLists = new MenuItem(Catalog.GetString("Bookmark _List"));
		addVisibleCaches = new MenuItem(Catalog.GetString("_Add All Unfiltered Caches to"));
		addCacheTo = new MenuItem(Catalog.GetString("Add _Selected Cache to"));
		addCacheTo.Sensitive = false;
		removeSelected = new MenuItem(Catalog.GetString("_Remove Selected Cache from Bookmark List"));
		bmrksMenu.Append(bmrkLists);
		bmrksMenu.Append(addVisibleCaches);
		bmrksMenu.Append(addCacheTo);
		bmrksMenu.Append(removeSelected);
		bmrksMenu.Append(new MenuItem());
		bmrksMenu.Append(newBList);
		bmrksMenu.Append(delBList);
		
	
		bmrks.Submenu = bmrksMenu;
		newBList.Activated += HandleNewBListActivated;
		removeSelected.Activated += HandleRemoveSelectedActivated;
		delBList.Activated += HandleDelBListActivated;
		bmrks.ShowAll();
	}

	void HandleDelBListActivated (object sender, EventArgs e)
	{
		m_monitor.RemoveBookmark();
	}

	void HandleRemoveSelectedActivated (object sender, EventArgs e)
	{
		m_monitor.RemoveSelFromBookmark();
	}

	void HandleNewBListActivated (object sender, EventArgs e)
	{
		m_monitor.AddBookmark();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		m_monitor.SaveWinSettings();
		DeregisterBus ();
		Application.Quit ();
		a.RetVal = true;
	}
	
	private static void DeregisterBus ()
	{
		try
		{
			BusG.Init ();
			Bus bus = Bus.Session;
			bus.Unregister(new ObjectPath ("/org/ocm/dbus"));
			bus.ReleaseName("org.ocm.dbus"); 
		}
		catch 
		{
			System.Console.WriteLine("WARNING: Could not deregister from DBUS");
		}
	}

	protected virtual void OnQuit (object sender, System.EventArgs e)
	{
		m_monitor.SaveWinSettings();
		DeregisterBus ();
		Application.Quit ();
	}

	protected virtual void doAbout (object sender, System.EventArgs e)
	{
		AboutDialog dialog = new AboutDialog ();
		
		dialog.ProgramName = "Open Cache Manager";
		
		dialog.Icon = this.Icon;
		
		dialog.Version = m_monitor.GetOCMVersion();
		
		dialog.Logo =  new Gdk.Pixbuf ("./icons/scalable/OCMLogo.svg", 96, 96);
		
		dialog.Website = "http://opencachemanage.sourceforge.net/";
		
		//dialog.Comments = (asm.GetCustomAttributes (typeof(AssemblyDescriptionAttribute), false)[0] as AssemblyDescriptionAttribute).Description;
		
		dialog.Copyright = "Copyright Kyle Campbell (c) 2010";
		
		System.IO.StreamReader reader = new System.IO.StreamReader(new System.IO.FileStream("licence/Licence.txt",System.IO.FileMode.Open,System.IO.FileAccess.Read));
		
		dialog.License = reader.ReadToEnd();
		
		reader.Close();
		
		dialog.Authors = new String[] { "Kyle Campbell - Programming", "Madelayne DeGrâce - Icons",
			"Harrie Klomp - Dutch Translation" , "Thor Dekov Buur - Danish Translation",
			"Michael Massoth/Florian Plähn/Maik Bischoff - German Translation",
			"Josef Kulhánek - Czech Translation","Vicen - Spanish Translation"};		
		dialog.Run ();
		dialog.Hide();
		
	}

	protected virtual void OnToggleArchive (object sender, System.EventArgs e)
	{
		cacheList.ToggleArchivedCaches ();
	}

	protected virtual void OnToggleUnavailable (object sender, System.EventArgs e)
	{
		cacheList.ToggleUnavailableCaches ();
	}

	protected virtual void OnToggleFound (object sender, System.EventArgs e)
	{
		cacheList.ToggleFoundCaches ();
	}

	protected virtual void OnOpenDatabaseClicked (object o, System.EventArgs args)
	{
		m_monitor.OpenDB();
	}

	protected virtual void OnImportClicked (object o, System.EventArgs args)
	{
		m_monitor.ImportFile();
	}

	public void SetSelectedCache(Geocache cache)
	{
		if (cache != null)
		{
			this.ZoomToSelectedCacheAction.Sensitive = true;
			this.SetSelectedCacheAsCentreAction.Sensitive = true;
			this.addCacheTo.Sensitive = true;
			if (Engine.getInstance().Store.BookmarkList != null)
				this.removeSelected.Sensitive = true;
			ViewOnlineAction.Sensitive = true;
			LogFindAction.Sensitive = true;
			printAction.Sensitive = true;
			if (cache.Symbol.Contains("Found"))
			{
				MarkFoundAction.Sensitive = false;
				MarkUnfoundAction1.Sensitive = true;
			}
			else
			{
				MarkFoundAction.Sensitive = true;
				MarkUnfoundAction1.Sensitive = false;	
			}
			CorrectedCoordinatesAction.Sensitive = true;
			MarkArchivedAction1.Sensitive = false;
			MarkDisabledAction1.Sensitive = false;
			MarkAvailableAction1.Sensitive = false;			
			if (cache.Archived == false)
				MarkArchivedAction1.Sensitive = true;
			if (!cache.Available)
				MarkAvailableAction1.Sensitive = true;
			else
				MarkDisabledAction1.Sensitive = true;
			
			ModifyCacheAction.Sensitive = true;
			DeleteAction1.Sensitive = true;
			ViewSelectedCacheInQLandkarteGTAction.Sensitive = true;
			AddChildWaypointAction.Sensitive = true;
		}
		else
		{
			this.ZoomToSelectedCacheAction.Sensitive = false;
			this.SetSelectedCacheAsCentreAction.Sensitive = false;
			this.addCacheTo.Sensitive = false;
			this.removeSelected.Sensitive = false;
			ViewOnlineAction.Sensitive = false;
			LogFindAction.Sensitive = false;
			MarkFoundAction.Sensitive = false;
			MarkArchivedAction1.Sensitive = false;
			MarkAvailableAction1.Sensitive = false;
			MarkDisabledAction1.Sensitive = false;
			MarkUnfoundAction1.Sensitive = false;
			ModifyCacheAction.Sensitive = false;
			DeleteAction1.Sensitive = false;
			CorrectedCoordinatesAction.Sensitive = false;
			ViewSelectedCacheInQLandkarteGTAction.Sensitive = false;
			printAction.Sensitive = false;
			AddChildWaypointAction.Sensitive = false;
		}
	}

	protected virtual void OnViewOnline (object sender, System.EventArgs e)
	{
		Process.Start (m_monitor.SelectedCache.URL.ToString ());
	}


	protected virtual void OnExportClicked (object sender, System.EventArgs e)
	{
		m_monitor.ExportGPX();		
	}

	protected virtual void OnHomePageActivated (object sender, System.EventArgs e)
	{
		UIMonitor.ViewHomePage ();
	}



	protected virtual void OnViewProfileActivate (object sender, System.EventArgs e)
	{
		UIMonitor.ViewProfile ();
	}



	protected virtual void OnPocketQueryActivate (object sender, System.EventArgs e)
	{
		UIMonitor.ViewPocketQueries ();
	}


	protected virtual void OnFindOnlineActivate (object sender, System.EventArgs e)
	{
		UIMonitor.FindCacheOnline ();
	}

	protected virtual void OnAccountActivated (object sender, System.EventArgs e)
	{
		UIMonitor.ViewAccountDetails ();
	}
		
	protected virtual void OnClickLog (object sender, System.EventArgs e)
	{
		m_monitor.LogFind();
	}
	
	protected virtual void OnClickMarkUnfound (object sender, System.EventArgs e)
	{
		m_monitor.MarkCacheUnfound();
	}
	
	protected virtual void OnDelete (object sender, System.EventArgs e)
	{
		m_monitor.DeleteCache();
	}
	
	protected virtual void OnFilterClick (object sender, System.EventArgs e)
	{
		m_monitor.SetAdditonalFilters();
	}
	
	protected virtual void OnClickClearFilters (object sender, System.EventArgs e)
	{
		m_monitor.ClearFilters();
	}
	
	public void SetAllowClearFilter(bool allow)
	{
		ClearAdvancedFiltersAction.Sensitive = allow;
	}
	
	protected virtual void OnShowAllToggle (object sender, System.EventArgs e)
	{
		m_monitor.ShowNearby = ShowNearbyCachesAction.Active;
	}
	
	public void SetNearbyEnabled()
	{
		ShowNearbyCachesAction.Active = true;
	}
	
	protected virtual void OnZoomHome (object sender, System.EventArgs e)
	{
		m_monitor.GoHome();
	}
	
	protected virtual void OnZoomSelected (object sender, System.EventArgs e)
	{
		m_monitor.ZoomToSelected();
	}
	
	protected virtual void OnConfigure (object sender, System.EventArgs e)
	{
		m_monitor.AddGPSProfile();
	}
	
	protected virtual void OnSendCachesActionActivated (object sender, System.EventArgs e)
	{
		m_monitor.SendToGPS();
	}
	
	protected virtual void OnPrefsClicked (object sender, System.EventArgs e)
	{
		m_monitor.ShowPreferences();
	}
	protected virtual void OnChangeHistoryClick (object sender, System.EventArgs e)
	{
		ChangeHistory dlg = new ChangeHistory();
		dlg.Parent = this;
		dlg.Icon = this.Icon;
		dlg.Show();
	}	
	
	public void EnableResetCentre()
	{
		ResetCentreToHomeAction.Sensitive = true;
	}
	
	protected virtual void OnSetCentre (object sender, System.EventArgs e)
	{
		m_monitor.SetSelectedAsCentre();		
	}
	
	protected virtual void OnResetCentre (object sender, System.EventArgs e)
	{
		m_monitor.ResetCenterToHome();
		ResetCentreToHomeAction.Sensitive = false;
	}
	
	protected virtual void OnTerraHome (object sender, System.EventArgs e)
	{
		UIMonitor.TerraHome();
	}
	
	protected virtual void OnTerraTodo (object sender, System.EventArgs e)
	{
		UIMonitor.TerraTodo();
	}
	
	protected virtual void OnTerraLocationless (object sender, System.EventArgs e)
	{
		UIMonitor.TerraLocTodo();
	}
	
	protected virtual void OnNaviHome (object sender, System.EventArgs e)
	{
		UIMonitor.NaviHome();
	}
	
	protected virtual void OnMyNavi (object sender, System.EventArgs e)
	{
		UIMonitor.MyNavi();
	}
	
	protected virtual void OnClickDisable (object sender, System.EventArgs e)
	{
		m_monitor.MarkCacheDisabled();
	}
	
	protected virtual void OnClickArchive (object sender, System.EventArgs e)
	{
		m_monitor.MarkCacheArchived();
	}
	
	protected virtual void OnClickAvailable (object sender, System.EventArgs e)
	{
		m_monitor.MarkCacheAvailable();
	}
	
	public void SetBookmarkList(string str)
	{
		foreach (Gtk.MenuItem item in (addCacheTo.Submenu as Menu))
		{
			item.Sensitive = true;
			if (item.Child != null && item.Child is Label)
				if (str == (item.Child as Label).Text)
					item.Sensitive = false;
		}
		
		foreach (Gtk.MenuItem item in (addVisibleCaches.Submenu as Menu))
		{
			item.Sensitive = true;
			if (item.Child != null && item.Child is Label)
				if (str == (item.Child as Label).Text)
					item.Sensitive = false;
		}
	}
	
#pragma warning disable 219
	public void UpdateBookmarkList(List<string> items)
	{
		Menu bookmarksSub = new Menu();
		Menu addAllSub = new Menu();
		Menu addSelSub = new Menu();
	
		CacheStore store = Engine.getInstance().Store;
		int count = 0;
		GLib.SList grp = new GLib.SList(IntPtr.Zero);
		RadioAction noList = new RadioAction("None", Catalog.GetString("None"), null, null, count);
		noList.Active = true;
		noList.Toggled += HandleNoListToggled;
		bookmarksSub.Append(noList.CreateMenuItem());
		bookmarksSub.Append(new MenuItem());
		foreach (string str in items)
		{
			RadioAction action = new RadioAction(str, str, null, null, count);
			action.Group = noList.Group;
			action.Toggled += HandleActionToggled;
			bookmarksSub.Append(action.CreateMenuItem());
			MenuItem bookmarkAll = new MenuItem(str);
			if (store.BookmarkList == str)
				bookmarkAll.Sensitive = false;
			bookmarkAll.Activated += HandleBookmarkAllActivated;
			addAllSub.Append(bookmarkAll);
			MenuItem bookmarkSel = new MenuItem(str);
			if (store.BookmarkList == str)
				bookmarkSel.Sensitive = false;
			bookmarkSel.Activated += HandleBookmarkSelActivated;
			addSelSub.Append(bookmarkSel);
			if (str == store.BookmarkList)
				action.Active = true;
			count++;
		}
		if (items.Count != 0)
		{
			bmrkLists.Submenu = bookmarksSub;
			bmrkLists.Sensitive = true;
			addVisibleCaches.Submenu = addAllSub;
			addVisibleCaches.Sensitive = true;
			addCacheTo.Submenu = addSelSub;
			addCacheTo.Sensitive = true;
		}
		else
		{
			bmrkLists.Sensitive = false;
			bmrkLists.Submenu = null;
			addVisibleCaches.Submenu = null;
			addVisibleCaches.Sensitive = false;
			addCacheTo.Submenu = null;
			addCacheTo.Sensitive = false;
		}
		bmrkLists.ShowAll();
		addVisibleCaches.ShowAll();
		addCacheTo.ShowAll();		
	}

	void HandleNoListToggled (object sender, EventArgs e)
	{
		if (((sender) as RadioAction).Active)
		{
			m_monitor.SetBookmark(null);
		}			
	}

	void HandleActionToggled (object sender, EventArgs e)
	{
		RadioAction action = sender as RadioAction;
		if (action.Active)
		{
			m_monitor.SetBookmark(action.Label);
		}
	}

	void HandleBookmarkSelActivated (object sender, EventArgs e)
	{
		MenuItem item = sender as MenuItem;
		m_monitor.BookmarkSelectedCache((item.Child as Label).Text);
	}

	void HandleBookmarkAllActivated (object sender, EventArgs e)
	{
		MenuItem item = sender as MenuItem;
		m_monitor.BookmarkVisible((item.Child as Label).Text);
	}

	protected virtual void OnToDoActionActivated (object sender, System.EventArgs e)
	{
	}
	
	protected virtual void OnModifyCacheActionActivated (object sender, System.EventArgs e)
	{
		 m_monitor.ModifyCache();
	}
	
	protected virtual void OnAddCacheActionActivated (object sender, System.EventArgs e)
	{
		m_monitor.AddCache();
	}
	
	protected virtual void OnDeleteAllSelect (object sender, System.EventArgs e)
	{
		m_monitor.DeleteAll();
	}
	
	protected virtual void OnGPSDToggle (object sender, System.EventArgs e)
	{
		if (UseGPSDAsCentreAction.Active)
			m_monitor.EnableGPS();
		else
			m_monitor.DisableGPS();
	}
	protected virtual void ConfigureGPSD (object sender, System.EventArgs e)
	{
		m_monitor.ConfigureGPSD();
	}
	
	public void SetGPSDOn()
	{
		UseGPSDAsCentreAction.Active = true;
	}
	
	protected virtual void OnOCMHome (object sender, System.EventArgs e)
	{
		UIMonitor.OCMHome();
	}
	
	protected virtual void OnBabelHome (object sender, System.EventArgs e)
	{
		UIMonitor.BabelHome();
	}
	
	protected virtual void OnGPSDHome (object sender, System.EventArgs e)
	{
		UIMonitor.GPSDHome();
	}
	
	protected virtual void OnDeselectClick (object sender, System.EventArgs e)
	{
		m_monitor.SetSelectedCache(null);
	}
	
	protected virtual void OnQTLandKarteClick (object sender, System.EventArgs e)
	{
		m_monitor.OpenInQLandKarte();
	}
	
	protected virtual void OnViewSelectedInQlandKarte (object sender, System.EventArgs e)
	{
		m_monitor.OpenSelectedCacheInQLandKarte();
	}
	
	protected virtual void OnAllFilterClick (object sender, System.EventArgs e)
	{
		m_monitor.ApplyQuickFilter(QuickFilter.ALL_FILTER);
	}
	
	protected virtual void OnTodoClick (object sender, System.EventArgs e)
	{
		m_monitor.ApplyQuickFilter(QuickFilter.TODO_FILTER);
	}
	
	protected virtual void OnDoneClick (object sender, System.EventArgs e)
	{
		m_monitor.ApplyQuickFilter(QuickFilter.DONE_FILTER);
	}
	
	protected virtual void OnMineClick (object sender, System.EventArgs e)
	{
		m_monitor.ApplyQuickFilter(QuickFilter.MINE_FILTER);
	}
	
	protected virtual void OnClearAllFilter (object sender, System.EventArgs e)
	{
		m_monitor.ApplyQuickFilter(QuickFilter.ALL_FILTER);
	}
	
	public void RebuildQuickFilterMenu(QuickFilters filters)
	{
			Menu qmenu = filters.BuildQuickFilterMenu();
			(QuickFilterAction.Proxies[0] as MenuItem).Submenu = qmenu;
	}
	
	public void RebuildEToolMenu(EToolList tools)
	{
			Menu emenu = tools.BuildEToolMenu();
			(ExternalToolsAction.Proxies[0] as MenuItem).Submenu = emenu;
	}
	
	public void RebuildLocationsMenu(LocationList locs)
	{
			Menu emenu = locs.BuildLocationlMenu();
			(LocationsMenu.Proxies[0] as MenuItem).Submenu = emenu;
	}
	
	public void RebuildProfEditMenu(GPSProfileList profiles)
	{	
		Menu emenu = profiles.BuildProfileEditMenu();
		(EditGPSProfileAction.Proxies[0] as MenuItem).Submenu = emenu;
	}
	
	public void RebuildProfilesMenu(GPSProfileList profiles)
	{
		Menu tmenu = profiles.BuildProfileTransferMenu();
		(TransferCachesAction.Proxies[0] as MenuItem).Submenu = tmenu;
		Menu rmenu = profiles.BuildProfileReceiveMenu();
		(ReceiveFieldNotesAction.Proxies[0] as MenuItem).Submenu = rmenu;
	}
	
	
	protected virtual void OnSaveQuickFilter (object sender, System.EventArgs e)
	{
		m_monitor.SaveQuickFilter();
	}
	
	protected virtual void OnDeleteQF (object sender, System.EventArgs e)
	{
		m_monitor.DeleteQuickFilter();
	}
	
	protected virtual void OnWikiClick (object sender, System.EventArgs e)
	{
		UIMonitor.ViewOCMWiki();
	}
	
	protected virtual void OnCompact (object sender, System.EventArgs e)
	{
		m_monitor.CompactDB();
	}
	protected virtual void OnCopyVisibleCachesActionActivated (object sender, System.EventArgs e)
	{
		m_monitor.CopyToDB();
	}
	protected virtual void OnMove (object sender, System.EventArgs e)
	{
		m_monitor.MoveToDB();
	}
	
	protected virtual void OnClickMarkFound (object sender, System.EventArgs e)
	{
		m_monitor.MarkCacheFound();
	}
	
	protected virtual void OnClickMarkDisabled (object sender, System.EventArgs e)
	{
		m_monitor.MarkCacheDisabled();
	}
	
	protected virtual void OnClickMarkArchived (object sender, System.EventArgs e)
	{
		m_monitor.MarkCacheArchived();
	}
	
	protected virtual void OnClickMarkAvailable (object sender, System.EventArgs e)
	{
		m_monitor.MarkCacheAvailable();
	}
	protected virtual void OnConfigureETools (object sender, System.EventArgs e)
	{
		m_monitor.ConfigureETools();
	}
	
	protected virtual void OnFindsClicked (object sender, System.EventArgs e)
	{
		m_monitor.ExportFindsGPX();
	}
	
	protected virtual void OnUpdateCheck (object sender, System.EventArgs e)
	{
		m_monitor.CheckForUpdates();
	}
	protected virtual void OnPrintClick (object sender, System.EventArgs e)
	{
		m_monitor.PrintCache();
	}
	protected virtual void OnCorrectedClick (object sender, System.EventArgs e)
	{
		m_monitor.CorrectCoordinates();
	}
	
	protected virtual void OnWorldOCClick (object sender, System.EventArgs e)
	{
		Process.Start("http://opencaching.eu");
	}
	
	protected virtual void OnSelectedChildClick (object sender, System.EventArgs e)
	{
		m_monitor.ShowAllChildren = false;
	}
	
	protected virtual void OnAllChildrenClicked (object sender, System.EventArgs e)
	{
		m_monitor.ShowAllChildren = true;
	}
	
	public void SetShowAllChildren()
	{
		AllWaypointsAction.Active = true;
	}
	protected virtual void OnImportDirClicked (object o, System.EventArgs args)
	{
		m_monitor.ImportDirectory();
	}
	
	protected virtual void OnClickStatistics (object sender, System.EventArgs e)
	{
		Process.Start("http://www.geocaching.com/my/statistics.aspx");
	}
	
	protected virtual void OnChildClick (object o, System.EventArgs args)
	{
		m_monitor.AddChildWaypoint();
	}
	
	public bool DoUpMap()
	{
		if (vPane.Position <= 285)
		{
			vPane.Position = 0;
			return false;
		}
		else if (vPane.Position <= 385)
		{
			vPane.Position = 285;
			return true;
		}
		else 
		{
			vPane.Position = 385;
			return true;
		}
	}
	
	public bool DoDownMap()
	{
		if (vPane.Position >=385)
		{
			vPane.Position = vPane.MaxPosition - 10;
			return false;
		}
		else if (vPane.Position < 285)
		{
			vPane.Position = 285;
			return true;
		}
		else 
		{
			vPane.Position = 385;
			return true;
		}
	}
	
	protected virtual void OnAddLocation (object sender, System.EventArgs e)
	{
		m_monitor.AddLocation();
	}
	
	protected virtual void OnDeleteLocation (object sender, System.EventArgs e)
	{
		m_monitor.RemoveLocation();
	}
	
	protected virtual void OnAddGPS (object sender, System.EventArgs e)
	{
		m_monitor.AddGPSProfile();
	}
	protected virtual void OnDeleteProfile (object sender, System.EventArgs e)
	{
		m_monitor.DeleteGPSProfile();
	}
	
	protected virtual void OnOfflineToggle (object sender, System.EventArgs e)
	{
		m_monitor.Configuration.UseOfflineLogging = !m_monitor.Configuration.UseOfflineLogging;
	}
	
	public void SetOfflineMode(bool isOffline)
	{
		LogToFieldNotesFileAction.Toggled -= OnOfflineToggle;
		LogToFieldNotesFileAction.Active = isOffline;
		LogToFieldNotesFileAction.Toggled += OnOfflineToggle;
	}
	protected virtual void OnViewFieldNotes (object sender, System.EventArgs e)
	{
		m_monitor.ViewOfflineLogs();
	}
	
	protected virtual void OnProcessFieldNotes (object sender, System.EventArgs e)
	{
		m_monitor.ReceiveGPSFieldNotes();
	}
	
	protected virtual void OnClearFieldNotes (object sender, System.EventArgs e)
	{
		m_monitor.ClearFieldNotes();
	}
	
	protected virtual void OnNewAction (object sender, System.EventArgs e)
	{
		m_monitor.CreateDB ();
	}
	
	public void AddAction(Action act, String accel)
	{
		this.UIManager.ActionGroups[0].Add(act, accel);
	}
	
	protected virtual void OnTransferLU (object sender, System.EventArgs e)
	{
		m_monitor.SendToGPS();
	}
	
	protected virtual void OnComboClick (object sender, System.EventArgs e)
	{
		m_monitor.DoComboFilter();
	}
	
	public void SetComboFilterState()
	{
		AdvancedFiltersAction.Sensitive = false;
		ClearAdvancedFiltersAction.Sensitive = false;
		ClearComboFilterAction.Sensitive = true;
	}
	protected virtual void OnClearComboFilter (object sender, System.EventArgs e)
	{
		m_monitor.ClearCombo();
	}
	
	public void ClearCombo ()
	{
		ClearComboFilterAction.Sensitive = false;
		AdvancedFiltersAction.Sensitive = true;
	}
}
