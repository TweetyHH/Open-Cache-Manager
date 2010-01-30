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
using WebKit;
using Mono.Unix;

namespace ocmgtk
{
	
	
	public partial class InternalBrowser : Gtk.Window
	{
		
		public InternalBrowser(String location) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			WebView view = new WebView();
			view.Open(location);
			view.NavigationRequested += OnNavigateRequest;
			view.LoadStarted += OnLoadStarted;
			view.LoadFinished += OnLoadFinished;
			scrolledwindow1.Add(view);
			browserStatus.Push(browserStatus.GetContextId("init"), Catalog.GetString("Initializing..."));
			this.ShowAll();
		}
		
		private void OnNavigateRequest(object o, NavigationRequestedArgs args)
		{
			browserStatus.Push(browserStatus.GetContextId("nav"), Catalog.GetString(string.Format("Opening {0}", args.Request.Uri)));
		}
		
		private void OnLoadStarted(object o, LoadStartedArgs args)
		{
			browserStatus.Push(browserStatus.GetContextId("nav"), Catalog.GetString("Loading data"));
		}
			                                              
		private void OnLoadFinished(object o, LoadFinishedArgs args)
		{
			browserStatus.Push(browserStatus.GetContextId("nav"), Catalog.GetString("Done"));
		}
		
		
	}
}
