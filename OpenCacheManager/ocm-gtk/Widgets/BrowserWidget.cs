// 
//  Copyright 2010  Kyle Campbell
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
using Mono.Unix;
using WebKit;

namespace ocmgtk
{
	
	
	[System.ComponentModel.ToolboxItem(true)]
	public partial class BrowserWidget : Gtk.Bin
	{
		WebView m_browser;
		
		public BrowserWidget()
		{
			this.Build();
			m_browser = new WebView();
			m_browser.NavigationRequested += HandleM_browserNavigationRequested;
			m_browser.LoadStarted += HandleM_browserLoadStarted;
			m_browser.LoadProgressChanged += HandleM_browserLoadProgressChanged;
			m_browser.LoadFinished += HandleM_browserLoadFinished;
			browserPlace.Add(m_browser);
			this.ShowAll();
		}

		void HandleM_browserLoadFinished (object o, LoadFinishedArgs args)
		{
			browsestatusBar.Push(browsestatusBar.GetContextId("load"), Catalog.GetString("Done"));
		}

		void HandleM_browserLoadProgressChanged (object o, LoadProgressChangedArgs args)
		{
			browsestatusBar.Push(browsestatusBar.GetContextId("load"), args.Progress.ToString());		
		}

		void HandleM_browserLoadStarted (object o, LoadStartedArgs args)
		{
			browsestatusBar.Push(browsestatusBar.GetContextId("load"), "Loading " + args.Frame.Uri);
		}

		void HandleM_browserNavigationRequested (object o, NavigationRequestedArgs args)
		{
			browsestatusBar.Push(browsestatusBar.GetContextId("nav"), "Opening" + args.Request.Uri);
		}
		
		public void LoadUrl(String target)
		{
			m_browser.Open(target);
		}
		
		public void LoadScript(String script)
		{
			m_browser.ExecuteScript(script);
		}

		protected virtual void OnRefresh (object sender, System.EventArgs e)
		{
			
		}

		protected virtual void OnStopActionActivated (object sender, System.EventArgs e)
		{
			
		}
	}
}
