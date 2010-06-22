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
using Gtk;
using Mono.Unix;
using ocmgtk;
using ocmengine;

public partial class MainWindow : Gtk.Window
{
	UIMonitor m_monitor;
	
	public MainWindow () : base(Gtk.WindowType.Toplevel)
	{	
		Build ();
		m_monitor = UIMonitor.getInstance ();
		m_monitor.Main = this;
		m_monitor.StatusProgress = statProgBar;
		m_monitor.GeoPane = cachePane;
		m_monitor.StatusBar = statusbar1;
		m_monitor.CacheListPane = cacheList;
		m_monitor.CentreLabel = coordLabel;
		m_monitor.Map = cachePane.CacheMap;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected virtual void OnQuit (object sender, System.EventArgs e)
	{
		Application.Quit ();
	}

	protected virtual void doAbout (object sender, System.EventArgs e)
	{
		AboutDialog dialog = new AboutDialog ();
		
		dialog.ProgramName = "Open Cache Manager";
		
		dialog.Version = "ALPHA 0.15";
		
		//dialog.Comments = (asm.GetCustomAttributes (typeof(AssemblyDescriptionAttribute), false)[0] as AssemblyDescriptionAttribute).Description;
		
		dialog.Copyright = "Copyright Kyle Campbell (c) 2010";
		
		System.IO.StreamReader reader = new System.IO.StreamReader(new System.IO.FileStream("licence/Licence.txt",System.IO.FileMode.Open,System.IO.FileAccess.Read));
		
		dialog.License = reader.ReadToEnd();
		
		reader.Close();
		
		dialog.Authors = new String[] { "Kyle Campbell" };
		
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

	protected virtual void OnToggleMine (object sender, System.EventArgs e)
	{
		
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

	public void SetCacheSelected ()
	{
		this.CacheAction.Sensitive = true;
		this.ZoomToSelectedCacheAction.Sensitive = true;
		this.SetSelectedCacheAsCentreAction.Sensitive = true;
	}

	protected virtual void OnViewOnline (object sender, System.EventArgs e)
	{
		Process.Start (m_monitor.SelectedCache.URL.ToString ());
	}

	protected virtual void OnMapClick (object sender, System.EventArgs e)
	{
		
	}

	protected virtual void OnRefresh (object sender, System.EventArgs e)
	{
	}

	protected virtual void OnStopActionActivated (object sender, System.EventArgs e)
	{
	}

	protected virtual void OnExportClicked (object sender, System.EventArgs e)
	{
		m_monitor.ExportGPX();		
	}

	protected virtual void OnPreferences (object sender, System.EventArgs e)
	{
	}


	protected virtual void OnNewActionActivated (object sender, System.EventArgs e)
	{
		m_monitor.CreateDB ();
	}
	protected virtual void OnHomePageActivate (object sender, System.EventArgs e)
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



	protected virtual void OnViewOnlineActivate (object sender, System.EventArgs e)
	{
	}

	protected virtual void OnLogActivate (object sender, System.EventArgs e)
	{
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
		ClearAdditonalFiltersAction.Sensitive = allow;
	}
	
	protected virtual void OnShowAllToggle (object sender, System.EventArgs e)
	{
		m_monitor.ShowNearby = ShowNearbyCachesAction.Active;
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
	
	
	
}
