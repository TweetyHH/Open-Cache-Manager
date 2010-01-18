
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
			HTML htmlwidget = new HTML();
			htmlwidget.LoadEmpty();
			logScroll.Add(htmlwidget);
			
			// TEMP
			FileStream fs = File.Open("../src/OpenCacheManager/test/multi.gpx", FileMode.Open);
			GPXParser parser = new GPXParser();
			ArrayList caches =  parser.parseGPXFile(fs);
		 	cacheinfo.setCacheInfo(caches[0] as Geocache);
			
			List<CacheLog> logs = (caches[0] as Geocache).CacheLogs;
			String html = "";
			for (int i=0; i < logs.Count; i++)
			{
				html += logs[i].toHTML();
			}
			htmlwidget.LoadFromString(html);
		}
	}
}
