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
using System.Collections.Generic;

namespace ocmgtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class BrowserWidget : Gtk.Bin
	{
		
		bool loaded = false;
		WebView m_view = new WebView();
		List<string> pending_actions = new List<string>();
		UIMonitor m_monitor;
		
		public BrowserWidget()
		{
			this.Build();
			m_view.NavigationRequested += HandleM_browserNavigationRequested;
			m_view.LoadStarted += HandleM_browserLoadStarted;
			m_view.LoadProgressChanged += HandleM_browserLoadProgressChanged;
			m_view.LoadFinished += HandleM_browserLoadFinished;
			browserAlign.Add(m_view);
			m_monitor = UIMonitor.getInstance();
		}

		void HandleM_browserLoadFinished (object o, LoadFinishedArgs args)
		{
			m_monitor.SetProgressDone(false);
			loaded = true;
			IEnumerator<string> actions = pending_actions.GetEnumerator();
			while (actions.MoveNext())
				LoadScript(actions.Current);
			pending_actions.Clear();
		}

		void HandleM_browserLoadProgressChanged (object o, LoadProgressChangedArgs args)
		{	
				m_monitor.SetProgress(args.Progress, 100, String.Format(Catalog.GetString("Loading Map {0}"), 
			                                                        ((double)args.Progress/(double)100).ToString("0%")), false);
		
		}

		void HandleM_browserLoadStarted (object o, LoadStartedArgs args)
		{
			m_monitor.StartProgressLoad(Catalog.GetString("Loading Map"), false);
			loaded = false;
		}

		void HandleM_browserNavigationRequested (object o, NavigationRequestedArgs args)
		{
			if (args.Request.Uri.StartsWith("ocm://"))
			{
				string[]  request = args.Request.Uri.Substring(6).Split('/');
				if (request[0].Equals("select"))
				{
					m_monitor.SelectCache(request[1]);
				}
				else if (request[0].Equals("mapmoved"))
				{
					m_monitor.CurrLat = double.Parse(request[1], System.Globalization.CultureInfo.InvariantCulture);
					m_monitor.CurrLon = double.Parse(request[2], System.Globalization.CultureInfo.InvariantCulture);
					m_monitor.GetNearByCaches();
				}
				else if (request[0].Equals("setcentre"))
				{
					m_monitor.SetMapCentre(double.Parse(request[1], System.Globalization.CultureInfo.InvariantCulture), double.Parse(request[2], System.Globalization.CultureInfo.InvariantCulture));
				}
				else if (request[0].Equals("sethome"))
				{
					m_monitor.SetHome(double.Parse(request[1], System.Globalization.CultureInfo.InvariantCulture), double.Parse(request[2], System.Globalization.CultureInfo.InvariantCulture));
				}
				else if (request[0].Equals("addlocation"))
				{
					m_monitor.AddLocation(double.Parse(request[1], System.Globalization.CultureInfo.InvariantCulture), double.Parse(request[2], System.Globalization.CultureInfo.InvariantCulture));
				}
				else if (request[0].Equals("addwaypoint"))
				{
					m_monitor.AddChildWaypoint(double.Parse(request[1], System.Globalization.CultureInfo.InvariantCulture), double.Parse(request[2], System.Globalization.CultureInfo.InvariantCulture));
				}
				else if (request[0].Equals("correctcoordinate"))
				{
					m_monitor.CorrectCoordinates(double.Parse(request[1], System.Globalization.CultureInfo.InvariantCulture), double.Parse(request[2], System.Globalization.CultureInfo.InvariantCulture));					
				}
				args.Frame.StopLoading();
			}
		}
		
		public void LoadUrl(String target)
		{
			m_view.Open(target);
		}
		
		public void LoadScript(String script)
		{
			if (loaded)
				m_view.ExecuteScript(script);
			else
				pending_actions.Add(script);
		}
		
		public void SetAutoSelectCache(bool autoSelectCache) {
			LoadScript("setAutoSelectCache('" + autoSelectCache + "');");
		}
		
		public void AddMaps(List<MapDescription> maps) {
			foreach (MapDescription map in maps) {
				if (map.Active) {
					AddMap(map.Code);
				}
			}
		}
			
		public void AddMap(string codeForMap) {
			LoadScript("addMapRenderer(" + codeForMap + "); ");
		}
		
		
		protected virtual void OnUpClick (object sender, System.EventArgs e)
		{
			sizeUpButton.Sensitive = m_monitor.Main.DoUpMap();
			sizeDownButton.Sensitive = true;
		}
		
		protected virtual void OnDownClick (object sender, System.EventArgs e)
		{
			sizeDownButton.Sensitive = m_monitor.Main.DoDownMap();
			sizeUpButton.Sensitive = true;
		}
	}
}
