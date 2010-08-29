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
		protected WebKit.WebView m_view;
		bool contentLoaded = false;
		
		public HTMLWidget ()
		{
			this.Build ();
			m_view = new WebKit.WebView();
			htmlScroll.Add(m_view);
			m_view.LoadFinished += HandleM_viewLoadFinished;
			m_view.NavigationRequested += HandleM_viewNavigationRequested;
		}

		void HandleM_viewLoadFinished (object o, WebKit.LoadFinishedArgs args)
		{
			contentLoaded = true;
		}

		void HandleM_viewNavigationRequested (object o, WebKit.NavigationRequestedArgs args)
		{
			if (contentLoaded)
			{
				m_view.StopLoading();
				System.Diagnostics.Process.Start(args.Request.Uri);
			}
		}
		
		public string HTML
		{
			//set { int i=1;}
			set {
				contentLoaded = false;
				m_view.LoadHtmlString(value, "http://www.geocaching.com");
			}
		}
		
		public void ExecuteFunction(string func)
		{
			m_view.ExecuteScript(func);
		}
	}
}
