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
using Gtk;
using ocmgtk;
using ocmengine;

public partial class MainWindow: Gtk.Window
{
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		UIMonitor mon = UIMonitor.getInstance();
	   	mon.GeoPane = cachePane;
		mon.StatusBar = statusbar1;		
		cacheList.UpdateCountStatus();
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
}