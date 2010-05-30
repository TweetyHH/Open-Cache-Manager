
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using ocmengine;
using Gtk;

namespace ocmgtk
{
	
	
	[System.ComponentModel.ToolboxItem(true)]
	public partial class GeoCachePane : Gtk.Bin
	{		
		public GeoCachePane()
		{
			this.Build();
			UIMonitor.getInstance().Map = mapWidget1;
			mapWidget1.LoadUrl ("file://" + System.Environment.CurrentDirectory + "/web/wpt_viewer.html");
		}
		
		public void SetCacheSelected()
		{
			cacheInfo.updateCacheInfo();
			logView.updateCacheInfo();
			waypointView.UpdateCacheInfo();
			descWidget.UpdateCacheInfo();
			
		}
		protected virtual void OnRefresh (object sender, System.EventArgs e)
		{
		}
		
		protected virtual void OnStopActionActivated (object sender, System.EventArgs e)
		{
		}
		
		
		
	}
}
