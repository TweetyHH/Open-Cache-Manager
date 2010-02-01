
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
		LogViewerWidget logView;
		
		public GeoCachePane()
		{
			this.Build();
			logView = new LogViewerWidget();
			alignment3.Add(logView);
		}
		
		public void SetCacheSelected()
		{
			cacheInfo.updateCacheInfo();
			logView.updateCacheInfo();
			waypointView.UpdateCacheInfo();
		}
	}
}
