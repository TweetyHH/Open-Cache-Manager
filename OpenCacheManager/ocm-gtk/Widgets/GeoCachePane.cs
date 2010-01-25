
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
		}
		
		public void setCache(Geocache cache)
		{
			cacheInfo.setCacheInfo(cache);
		}
	}
}
