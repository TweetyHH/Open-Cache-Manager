// 
//  Copyright 2010  campbelk
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using Gtk;

namespace ocmgtk
{


	[System.ComponentModel.ToolboxItem(true)]
	public partial class HTMLWidget : Gtk.Bin
	{
		WebKit.WebView m_view;
		
		public HTMLWidget ()
		{
			this.Build ();
			m_view = new WebKit.WebView();
			ScrolledWindow win = new ScrolledWindow();
			win.Add(m_view);
			this.Add(win);
			this.ShowAll();
		}
		
		public string HTML
		{
			//set { int i=1;}
			set { m_view.LoadHtmlString(value, "http://www.geocaching.com");}
		}
	}
}
