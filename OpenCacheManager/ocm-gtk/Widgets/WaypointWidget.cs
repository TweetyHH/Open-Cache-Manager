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
using ocmengine;
using Mono.Unix;

namespace ocmgtk
{


	[System.ComponentModel.ToolboxItem(true)]
	public partial class WaypointWidget : Gtk.Bin
	{

		private ListStore m_childPoints;
		private UIMonitor m_mon;

		public WaypointWidget ()
		{
			this.Build ();
			BuildWPTList ();
			m_mon = UIMonitor.getInstance ();
		}

		public void BuildWPTList ()
		{
			m_childPoints = new ListStore (typeof(Waypoint));
			
			CellRendererText code_cell = new CellRendererText ();
			CellRendererText title_cell = new CellRendererText ();
			CellRendererText coord_cell = new CellRendererText ();
			TreeViewColumn wpt_code = new TreeViewColumn (Catalog.GetString ("Code"), code_cell);
			TreeViewColumn wpt_Lat = new TreeViewColumn (Catalog.GetString ("Location"), coord_cell);
			TreeViewColumn wpt_title = new TreeViewColumn (Catalog.GetString ("Name"), title_cell);
			
			wptView.AppendColumn (wpt_code);
			wptView.AppendColumn (wpt_Lat);
			wptView.AppendColumn (wpt_title);
			
			
			wpt_code.SetCellDataFunc (code_cell, new TreeCellDataFunc (RenderCode));
			wpt_title.SetCellDataFunc (title_cell, new TreeCellDataFunc (RenderTitle));
			wpt_Lat.SetCellDataFunc (coord_cell, new TreeCellDataFunc (RenderCoord));
			
			wptView.Model = m_childPoints;
			wptView.Selection.Changed += OnSelectionChanged;
		}

		private void OnSelectionChanged (object sender, EventArgs e)
		{
			TreeIter iter;
			TreeModel model;
			
			if (((TreeSelection)sender).GetSelected (out model, out iter)) {
				Waypoint val = (Waypoint)model.GetValue (iter, 0);
				if (val != null)
					m_mon.ZoomToPoint (val.Lat, val.Lon);
				if (val is Geocache) {
					editButton.Sensitive = false;
					deleteButton.Sensitive = false;
				} else {
					editButton.Sensitive = true;
					deleteButton.Sensitive = true;
				}
			} else {
				editButton.Sensitive = false;
				deleteButton.Sensitive = false;
			}
		}

		public void UpdateCacheInfo ()
		{
			Geocache cache = m_mon.SelectedCache;
			m_childPoints.Clear ();
			m_mon.ClearMarkers ();
			if (cache == null)
				return;
			IEnumerator<Waypoint> wptenum = Engine.getInstance ().GetChildWaypoints (cache.Name);
			m_childPoints.AppendValues (cache);
			m_mon.AddMapCache (cache);
			while (wptenum.MoveNext ()) {
				m_childPoints.AppendValues (wptenum.Current);
				m_mon.AddMapWayPoint (wptenum.Current);
				
			}
			
			m_mon.ZoomToPoint (cache.Lat, cache.Lon);
		}


		private void RenderTitle (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Waypoint wpt = (Waypoint)model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			text.Text = wpt.Desc;
		}

		private void RenderCode (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Waypoint wpt = (Waypoint)model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			text.Text = wpt.Name;
		}

		private void RenderCoord (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Waypoint wpt = (Waypoint)model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			text.Text = Utilities.getCoordString (wpt.Lat, wpt.Lon);
		}

		protected virtual void DoEdit (object sender, System.EventArgs e)
		{
			try {
				WaypointDialog dlg = new WaypointDialog ();
				Gtk.TreeIter itr;
				Gtk.TreeModel model;
				if (wptView.Selection.GetSelected (out model, out itr)) {
					Waypoint wpt = (Waypoint)model.GetValue (itr, 0);
					String origname = wpt.Name;
					dlg.SetPoint (wpt);
					if ((int)ResponseType.Ok == dlg.Run ()) {
						wpt = dlg.GetPoint ();
						CacheStore store = Engine.getInstance().Store;
						if (wpt.Name == origname)
							store.UpdateWaypointAtomic (wpt);
						else
						{
							store.DeleteWaypoint(origname);
							store.UpdateWaypointAtomic(wpt);
						}
						dlg.Dispose ();
						UpdateCacheInfo ();
					}
				}
			} catch (Exception ex) {
				UIMonitor.ShowException (ex);
			}
		}

		protected virtual void doAdd (object sender, System.EventArgs e)
		{
			try {
				Waypoint newPoint = new Waypoint ();
				Geocache parent = m_mon.SelectedCache;
				newPoint.Symbol = "Final Location";
				newPoint.Parent = parent.Name;
				newPoint.Lat = parent.Lat;
				newPoint.Lon = parent.Lon;
				newPoint.Name = "FL" + parent.Name.Substring (2);
				WaypointDialog dlg = new WaypointDialog ();
				dlg.SetPoint (newPoint);
				if ((int)ResponseType.Ok == dlg.Run ()) {
					newPoint = dlg.GetPoint ();
					CacheStore store = Engine.getInstance ().Store;
					store.AddWaypointAtomic (newPoint);
					dlg.Dispose ();
					UpdateCacheInfo ();
				}
			} catch (Exception ex) {
				UIMonitor.ShowException (ex);
			}
		}

		protected virtual void doRemove (object sender, System.EventArgs e)
		{
			Waypoint toDelete = GetSelectedWaypoint ();
			
			MessageDialog md = new MessageDialog (null, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.YesNo, "Are you sure you wish to delete " + toDelete.Name);
			
			try {
				if ((int)ResponseType.Yes == md.Run ()) {
					
					Engine.getInstance ().Store.DeleteWaypoint (toDelete.Name);
					UpdateCacheInfo ();
				}
				md.Hide ();
				md.Dispose ();
			} catch (Exception ex) {
				md.Hide ();
				md.Dispose ();
				UIMonitor.ShowException (ex);
			}
			
			
			
			
		}

		private Waypoint GetSelectedWaypoint ()
		{
			Gtk.TreeIter itr;
			Gtk.TreeModel model;
			if (wptView.Selection.GetSelected (out model, out itr)) {
				return (Waypoint)model.GetValue (itr, 0);
			}
			return null;
		}

		protected virtual void OnRefresh (object sender, System.EventArgs e)
		{
		}

		protected virtual void OnStopActionActivated (object sender, System.EventArgs e)
		{
		}
		
	}
}
