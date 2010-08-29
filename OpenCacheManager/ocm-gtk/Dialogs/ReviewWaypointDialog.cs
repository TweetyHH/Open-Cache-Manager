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
using System.Text.RegularExpressions;
using Mono.Unix;
using ocmengine;

namespace ocmgtk
{


	public partial class ReviewWaypointDialog : Gtk.Dialog
	{
		MatchCollection m_matches = null;
		int m_pos = 0;
		public MatchCollection Matches
		{
			set 
			{
				m_matches = value;
				m_pos = 0;
				reviewWidget.WaypointMatch = m_matches[0];
				this.Title = String.Format(Catalog.GetString("Reviewing Waypoint {0} of {1}"), m_pos + 1, m_matches.Count);
			}
		}
		
		protected virtual void OnAddClick (object sender, System.EventArgs e)
		{
			ocmengine.Waypoint pt = reviewWidget.GetPoint();
			Engine.getInstance().Store.AddWaypointAtomic(pt);
			NextMatch();
		}
		
		public void NextMatch()
		{
			m_pos++;
			if (m_pos == m_matches.Count)
			{
				this.Hide();
				m_widget.UpdateCacheInfo();
				return;
			}
			reviewWidget.WaypointMatch = m_matches[m_pos];
			this.Title = String.Format(Catalog.GetString("Reviewing Waypoint {0} of {1}"), m_pos + 1, m_matches.Count);
		}
		
		private WaypointWidget m_widget;
		public ReviewWaypointDialog (WaypointWidget widget)
		{
			this.Build ();
			m_widget = widget;
		}
		
		protected virtual void OnSkipClick (object sender, System.EventArgs e)
		{
			NextMatch();
		}
		
		
		protected virtual void OnCancelClick (object sender, System.EventArgs e)
		{
			this.Hide();
		}
		
		
		
		
		
	}
}
