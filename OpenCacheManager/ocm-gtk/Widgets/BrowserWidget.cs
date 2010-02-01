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
using Gecko;

namespace ocmgtk
{
	
	
	[System.ComponentModel.ToolboxItem(true)]
	public partial class BrowserWidget : Gtk.Bin
	{
		WebControl m_browser;
		
		public BrowserWidget()
		{
			this.Build();
			m_browser = new WebControl();
			browserPlace.Add(m_browser);
			m_browser.NetStart += HandleNetStart;
			m_browser.NetStop += HandleNetStop;
			m_browser.NetState += HandleNetState;
			m_browser.OpenUri += HandleOpenUri;
			m_browser.ProgressAll += HandleProgressAll;
			this.ShowAll();
		}

		void HandleProgressAll(object o, ProgressAllArgs args)
		{
			String progress = string.Format(Catalog.GetString("Progress {0} of {1}"), args.Curprogress, args.Maxprogress);
			browsestatusBar.Push(browsestatusBar.GetContextId("nav"), progress);
		}

		void HandleOpenUri(object o, OpenUriArgs args)
		{
			browsestatusBar.Push(browsestatusBar.GetContextId("nav"), Catalog.GetString("Opening"));
		}

		void HandleNetState(object o, NetStateArgs args)
		{
			
		}

		void HandleNetStop(object sender, EventArgs e)
		{
			browsestatusBar.Push(browsestatusBar.GetContextId("nav"), Catalog.GetString("Done"));
		}

		void HandleNetStart(object sender, EventArgs e)
		{
			browsestatusBar.Push(browsestatusBar.GetContextId("nav"), Catalog.GetString("Loading"));
		}
		
		public void LoadUrl(String target)
		{
			m_browser.LoadUrl(target);
		}

		protected virtual void OnRefresh (object sender, System.EventArgs e)
		{
			m_browser.Reload((int) Gecko.ReloadFlags.Reloadbypasscache);
		}

		protected virtual void OnStopActionActivated (object sender, System.EventArgs e)
		{
			m_browser.StopLoad();
		}
	}
}
