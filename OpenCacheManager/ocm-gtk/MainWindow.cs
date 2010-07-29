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

public partial class MainWindow : Gtk.Window
{
	UIMonitor m_monitor;
	MenuItem bmrkLists;
	MenuItem addVisibleCaches;
	MenuItem addCacheTo;
	MenuItem removeSelected;
	
	public int HPos
	{
		get { return hSplitPane.Position;}
		set { hSplitPane.Position = value;}
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
		m_monitor.Map = cachePane.CacheMap;
	}

	void HandleHandleConfigureEvent (object o, ConfigureEventArgs args)
	{
		System.Console.WriteLine("CONFIGURE!");
	}

	protected void BuildBookMarkMenu()
	{
		MenuItem bmrks = new MenuItem(Catalog.GetString("_Bookmarks"));
		mainmenubar.Insert(bmrks, 3);	
		Menu bmrksMenu = new Menu();
		MenuItem newBList = new MenuItem(Catalog.GetString("_Create Bookmark List..."));
		MenuItem delBList = new MenuItem(Catalog.GetString("_Delete Bookmark List..."));
		bmrkLists = new MenuItem(Catalog.GetString("Bookmark _List"));
		addVisibleCaches = new MenuItem(Catalog.GetString("_Add All Visible Caches to"));
		addCacheTo = new MenuItem(Catalog.GetString("Add _Selected Cache to"));
		addCacheTo.Sensitive = false;
		removeSelected = new MenuItem(Catalog.GetString("_Remove Selected Cache from Bookmark List"));
		bmrksMenu.Append(newBList);
		bmrksMenu.Append(delBList);
		bmrksMenu.Append(new MenuItem());
		bmrksMenu.Append(bmrkLists);
		bmrksMenu.Append(addVisibleCaches);
		bmrksMenu.Append(addCacheTo);
		bmrksMenu.Append(removeSelected);
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
		Application.Quit ();
		a.RetVal = true;
	}

	protected virtual void OnQuit (object sender, System.EventArgs e)
	{
		m_monitor.SaveWinSettings();
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
		
		dialog.Authors = new String[] { "Kyle Campbell - Programming", "Madelayne DeGrâce - Icons", "Harrie Klomp - Dutch Translation" };
		
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
		m_monitor.ImportGPX();
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
			MarkArchivedAction.Sensitive = true;
			MarkAvailableAction.Sensitive = true;
			MarkDisabledAction.Sensitive = true;
			MarkUnfoundAction.Sensitive = true;
			ModifyCacheAction.Sensitive = true;
			DeleteCacheAction.Sensitive = true;			
		}
		else
		{
			this.ZoomToSelectedCacheAction.Sensitive = false;
			this.SetSelectedCacheAsCentreAction.Sensitive = false;
			this.addCacheTo.Sensitive = false;
			this.removeSelected.Sensitive = false;
			ViewOnlineAction.Sensitive = false;
			LogFindAction.Sensitive = false;
			MarkArchivedAction.Sensitive = false;
			MarkAvailableAction.Sensitive = false;
			MarkDisabledAction.Sensitive = false;
			MarkUnfoundAction.Sensitive = false;
			ModifyCacheAction.Sensitive = false;
			DeleteCacheAction.Sensitive = false;	
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

	protected virtual void OnNewActionActivated (object sender, System.EventArgs e)
	{
		m_monitor.CreateDB ();
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
		m_monitor.ConfigureGPS();
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
			bookmarkAll.Activated += HandleBookmarkAllActivated;
			addAllSub.Append(bookmarkAll);
			MenuItem bookmarkSel = new MenuItem(str);
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
	
	
	
	
	
}
