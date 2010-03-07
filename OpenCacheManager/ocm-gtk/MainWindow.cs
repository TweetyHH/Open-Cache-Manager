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
using System.Threading;
using Gtk;
using Mono.Unix;
using ocmgtk;
using ocmengine;

public partial class MainWindow: Gtk.Window
{
	UIMonitor m_monitor;
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		m_monitor = UIMonitor.getInstance();
	   	m_monitor.GeoPane = cachePane;
		m_monitor.StatusBar = statusbar1;	
		m_monitor.Main = this;
		m_monitor.Map = mapwidget;
		mapwidget.LoadUrl("file://" + System.Environment.CurrentDirectory + "/web/wpt_viewer.html");
		m_monitor.updateCaches();
		this.Maximize();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected virtual void OnQuit (object sender, System.EventArgs e)
	{
		Application.Quit();
	}

	protected virtual void doAbout(object sender, System.EventArgs e)
	{
		ocmgtk.OCMAboutDialog dlg = new ocmgtk.OCMAboutDialog();
		dlg.Show();
	}

	protected virtual void OnToggleArchive (object sender, System.EventArgs e)
	{
		cacheList.ToggleArchivedCaches();
	}

	protected virtual void OnToggleUnavailable (object sender, System.EventArgs e)
	{
		cacheList.ToggleUnavailableCaches();
	}

	protected virtual void OnToggleMine (object sender, System.EventArgs e)
	{
		
	}

	protected virtual void OnToggleFound (object sender, System.EventArgs e)
	{
		cacheList.ToggleFoundCaches();
	}

	protected virtual void OnOpenClicked (object o, System.EventArgs args)
	{
		FileChooserDialog dlg = new FileChooserDialog(Catalog.GetString("Open GPX/LOC File"), 
		                                              this, FileChooserAction.Open, 
		                                              Catalog.GetString("Cancel"), ResponseType.Cancel, 
		                                              Catalog.GetString("Open"), ResponseType.Accept);
		FileFilter filter = new FileFilter();
		filter.Name = "GPX Files";
		filter.AddMimeType("text/xml");
		filter.AddMimeType("application/xml");
		filter.AddPattern("*.gpx");
		dlg.AddFilter(filter);
		
		if (dlg.Run() == (int) ResponseType.Accept)
		{
			System.IO.FileStream fs = System.IO.File.Open(dlg.Filename, System.IO.FileMode.Open);
			dlg.Hide();
			GPXParser parser = Engine.getInstance().Parser;
			CacheStore store = Engine.getInstance().Store;
			ProgressDialog pdlg = new ProgressDialog(parser);
			//pdlg.Run();
			pdlg.Modal = true;
			pdlg.Start(fs, store);
			//parser.parseGPXFile(fs, store);
			m_monitor.updateCaches();
			fs.Close();
		}
		dlg.Destroy();
		this.ShowAll();
	}
	
	public void SetCacheSelected()
	{
		this.CacheAction.Sensitive = true;
		this.ShowAll();
	}

	protected virtual void OnViewOnline (object sender, System.EventArgs e)
	{
		Process.Start(m_monitor.SelectedCache.URL.ToString());
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

	protected virtual void OnSave (object sender, System.EventArgs e)
	{
		
		FileChooserDialog dlg = new FileChooserDialog(Catalog.GetString("Save GPX File"), 
		                                              this, FileChooserAction.Save, 
		                                              Catalog.GetString("Cancel"), ResponseType.Cancel, 
		                                              Catalog.GetString("Open"), ResponseType.Accept);
		FileFilter filter = new FileFilter();
		filter.Name = "GPX Files";
		filter.AddMimeType("text/xml");
		filter.AddMimeType("application/xml");
		filter.AddPattern("*.gpx");
		dlg.AddFilter(filter);
		
		if (dlg.Run() == (int) ResponseType.Accept)
		{
			GPXWriter writer = new GPXWriter();
			writer.WriteGPXFile(dlg.Filename, m_monitor.GetVisibleCacheList());
		}
		dlg.Destroy();
		this.ShowAll();
	}
	
	/*
	 * protected virtual void OnBtnLoadFileClicked(object sender, System.EventArgs e)
	{
		Gtk.FileChooserDialog fc=
		new Gtk.FileChooserDialog("Choose the file to open",
		                            this,
		                            FileChooserAction.Open,
		                            "Cancel",ResponseType.Cancel,
		                            "Open",ResponseType.Accept);

		if (fc.Run() == (int)ResponseType.Accept) 
		{
			System.IO.FileStream file=System.IO.File.OpenRead(fc.Filename);
			file.Close();
		}
		//Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
		fc.Destroy();
	}*/
  
}