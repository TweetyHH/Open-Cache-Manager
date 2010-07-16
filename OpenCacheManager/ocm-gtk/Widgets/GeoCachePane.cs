
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
		public BrowserWidget CacheMap
		{
			get { return mapWidget1;}
		}
		
		public GeoCachePane()
		{
			this.Build();
		}

		public void SetCacheSelected()
		{
			cacheInfo.updateCacheInfo();
			logView.updateCacheInfo();
			waypointView.UpdateCacheInfo();
			descWidget.UpdateCacheInfo();
			notesWidget.UpdateCacheInfo();
			
		}
	}
}
