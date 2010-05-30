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
		m_monitor.GeoPane = cachePane;
		m_monitor.StatusBar = statusbar1;
		m_monitor.Main = this;
		m_monitor.LoadConfig();
		this.Resize(1024,575);
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
		Assembly asm = Assembly.GetExecutingAssembly ();
		
		dialog.ProgramName = "Open Cache Manager";
		
		dialog.Version = "ALPHA 0.10";
		
		//dialog.Comments = (asm.GetCustomAttributes (typeof(AssemblyDescriptionAttribute), false)[0] as AssemblyDescriptionAttribute).Description;
		
		dialog.Copyright = "Copyright Kyle Campbell (c) 2010";
		
		dialog.License = "Apache Licence 2.0";
		
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
		FileChooserDialog dlg = new FileChooserDialog (Catalog.GetString ("Import GPX File"), this, FileChooserAction.Open, Catalog.GetString ("Cancel"), ResponseType.Cancel, Catalog.GetString ("Import"), ResponseType.Accept);
		dlg.SetCurrentFolder (System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments));
		FileFilter filter = new FileFilter ();
		filter.Name = "GPX Files";
		filter.AddMimeType ("text/xml");
		filter.AddMimeType ("application/xml");
		filter.AddPattern ("*.gpx");
		dlg.AddFilter (filter);
		
		if (dlg.Run () == (int)ResponseType.Accept) {
			System.IO.FileStream fs = System.IO.File.Open (dlg.Filename, System.IO.FileMode.Open);
			dlg.Hide ();
			GPXParser parser = new GPXParser();
			CacheStore store = Engine.getInstance ().Store;
			ProgressDialog pdlg = new ProgressDialog (parser);
			pdlg.Modal = true;
			pdlg.Start (fs, store);
			m_monitor.RefreshCaches ();
			fs.Close ();
		}
		dlg.Destroy ();
		this.ShowAll ();
	}

	public void SetCacheSelected ()
	{
		this.CacheAction.Sensitive = true;
		this.ShowAll ();
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
		Preferences dlg = new Preferences ();
		dlg.SetLat (m_monitor.CentreLat);
		dlg.SetLon (m_monitor.CentreLon);
		if ((int)ResponseType.Ok == dlg.Run ()) {
			setHome (dlg.Lat, dlg.Lon);
		}
	}

	protected void setHome (double lat, double lon)
	{
		GConf.Client client = new GConf.Client ();
		try {
			client.Set ("/apps/monoapps/ocm/homelat", lat);
			client.Set ("/apps/monoapps/ocm/homelon", lon);
			m_monitor.CentreLat = lat;
			m_monitor.CentreLon = lon;
		} catch (GConf.NoSuchKeyException) {
			// Do nothing
		}
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
		UIMonitor.getInstance().LogFind();
	}
	
	protected virtual void OnClickMarkUnfound (object sender, System.EventArgs e)
	{
		UIMonitor.getInstance().MarkCacheUnfound();
	}
	
	
	
}
