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
using System.Collections.Generic;
using Gtk;
using Gecko;
using ocmengine;
using Mono.Unix;

namespace ocmgtk
{
	
	
	[System.ComponentModel.ToolboxItem(true)]
	public partial class WaypointWidget : Gtk.Bin
	{
		
		private ListStore m_childPoints;
		private UIMonitor m_mon;
		
		public WaypointWidget()
		{
			this.Build();
			BuildWPTList();
			m_mon = UIMonitor.getInstance();
			this.ShowAll();
		}
		
		public void InitMap()
		{
			//m_map.LoadUrl("file://" + System.Environment.CurrentDirectory + "/../web/wpt_viewer.html");
		}
		
		public void BuildWPTList()
		{
			m_childPoints = new ListStore(typeof (Waypoint));
				
			CellRendererText code_cell = new CellRendererText();
			CellRendererText title_cell = new CellRendererText();
			CellRendererText coord_cell = new CellRendererText();
			TreeViewColumn wpt_code = new TreeViewColumn(Catalog.GetString("Code"), code_cell);
			TreeViewColumn wpt_Lat = new TreeViewColumn(Catalog.GetString("Location"),coord_cell);
			TreeViewColumn wpt_title = new TreeViewColumn(Catalog.GetString("Name"), title_cell);
			
			wptView.AppendColumn(wpt_code);
			wptView.AppendColumn(wpt_Lat);
			wptView.AppendColumn(wpt_title);
			
			
			wpt_code.SetCellDataFunc(code_cell, new TreeCellDataFunc(RenderCode));
			wpt_title.SetCellDataFunc(title_cell, new TreeCellDataFunc(RenderTitle));
			wpt_Lat.SetCellDataFunc(coord_cell, new TreeCellDataFunc(RenderCoord));
			
			wptView.Model = m_childPoints;
			//wptView.Selection.Changed += OnSelectionChanged;
			
			this.ShowAll();
					
		}
		
				
		private void OnSelectionChanged(object sender, EventArgs e) 
		{
				/*TreeIter iter;
                TreeModel model;
 
			    if (((TreeSelection)sender).GetSelected (out model, out iter))
                {

					Waypoint val = (Waypoint) model.GetValue (iter, 0);
					if (val != null)
                      		m_map.LoadUrl("javascript:zoomToPoint(" + val.Lat + "," + val.Lon + ")");
				}*/
                
		}
		
		public void UpdateCacheInfo()
		{
			Geocache cache = m_mon.SelectedCache;
			m_childPoints.Clear();
			IEnumerator<Waypoint> wptenum = Engine.getInstance().GetChildWaypoints(cache.Name);
			while (wptenum.MoveNext())
			{
				m_childPoints.AppendValues(wptenum.Current);
			}
			//m_map.LoadUrl("javascript:loadMarkers("+ cache.Lat + "," + cache.Lon  + ")");
			this.ShowAll();
		}
		
		
		private void RenderTitle (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Waypoint wpt = (Waypoint) model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			text.Text = wpt.Desc;
		}
		
		private void RenderCode (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Waypoint wpt = (Waypoint) model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			text.Text = wpt.Name;
		}
		
		private void RenderCoord (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Waypoint wpt = (Waypoint) model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			text.Text = Utilities.getCoordString(wpt.Lat, wpt.Lon);
		}
	
		
	}
}
