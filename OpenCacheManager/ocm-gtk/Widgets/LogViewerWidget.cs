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
using System.Text;
using System.Collections.Generic;
using ocmengine;

namespace ocmgtk
{

	[System.ComponentModel.ToolboxItem(true)]
	public partial class LogViewerWidget : Gtk.Bin
	{
		HTMLWidget m_logPane;
		UIMonitor m_mon;

		public LogViewerWidget ()
		{
			this.Build ();
			m_logPane = new HTMLWidget ();
			logAlign.Add(m_logPane);
			m_mon = UIMonitor.getInstance ();
		}

		public void updateCacheInfo ()
		{
			StringBuilder builder = new StringBuilder ();
			builder.Append ("<HTML><BODY>");
			Geocache cache = m_mon.SelectedCache;
			if (cache != null) {
				IEnumerator<CacheLog> logenum = Engine.getInstance ().GetLogs (cache.Name);
				while (logenum.MoveNext ()) {
					builder.Append (logenum.Current.toHTML ());
				}
				
			}
			else
			{
				builder.Append(Mono.Unix.Catalog.GetString("NO CACHE SELECTED"));
			}
			builder.Append ("</BODY></HTML>");
			m_logPane.HTML = builder.ToString ();
		}
	}
}
