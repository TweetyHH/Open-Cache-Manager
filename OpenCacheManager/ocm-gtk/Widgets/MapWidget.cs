
using System;
using WebKit;

namespace ocmgtk
{
	
	
	[System.ComponentModel.ToolboxItem(true)]
	public partial class MapWidget : Gtk.Bin
	{
		WebView view;
		protected virtual void goToPoint (object sender, System.EventArgs e)
		{
			System.Console.WriteLine("CLICKED!");
		 	view.ExecuteScript("goToPoint()");
		}		
		
		public MapWidget()
		{
			this.Build();
			/*view = new WebView();
			view.Open("file:///home/campbelk/svn/src/OpenCacheManager/web/yahoo_viewer.html");
			mapWindow.Add(view);*/
		}
	}
}
