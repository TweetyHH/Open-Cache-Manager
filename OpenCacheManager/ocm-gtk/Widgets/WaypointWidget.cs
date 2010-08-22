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
		private TreeModelSort m_ListSort;
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
			CellRendererPixbuf icon_cell = new CellRendererPixbuf ();
			TreeViewColumn wpt_icon = new TreeViewColumn (Catalog.GetString ("Type"), icon_cell);
			wpt_icon.SortColumnId = 3;
			TreeViewColumn wpt_code = new TreeViewColumn (Catalog.GetString ("Code"), code_cell);
			wpt_code.SortColumnId = 0;
			TreeViewColumn wpt_Lat = new TreeViewColumn (Catalog.GetString ("Location"), coord_cell);
			wpt_Lat.SortColumnId = 1;
			TreeViewColumn wpt_title = new TreeViewColumn (Catalog.GetString ("Name"), title_cell);
			wpt_title.SortColumnId = 2;
			
			wptView.AppendColumn (wpt_icon);
			wptView.AppendColumn (wpt_code);
			wptView.AppendColumn (wpt_Lat);
			wptView.AppendColumn (wpt_title);
			
			
			wpt_code.SetCellDataFunc (code_cell, new TreeCellDataFunc (RenderCode));
			wpt_title.SetCellDataFunc (title_cell, new TreeCellDataFunc (RenderTitle));
			wpt_Lat.SetCellDataFunc (coord_cell, new TreeCellDataFunc (RenderCoord));
			wpt_icon.SetCellDataFunc (icon_cell, new TreeCellDataFunc (RenderIcon));
			m_ListSort = new TreeModelSort(m_childPoints);
			m_ListSort.SetSortFunc (3, TypeCompare);
			m_ListSort.SetSortFunc (2, NameCompare);
			m_ListSort.SetSortFunc (1, LocationCompare);
			m_ListSort.SetSortFunc (0, CodeCompare);
			
			wptView.Model = m_ListSort;
			wptView.Selection.Changed += OnSelectionChanged;
		}
		
		private int NameCompare (TreeModel model, TreeIter tia, TreeIter tib)
		{
			Waypoint cacheA = (Waypoint)model.GetValue (tia, 0);
			Waypoint cacheB = (Waypoint)model.GetValue (tib, 0);
			if (cacheA == null || cacheB == null)
				return 0;				
			return String.Compare (cacheA.Desc, cacheB.Desc);
		}
		
		private int TypeCompare (TreeModel model, TreeIter tia, TreeIter tib)
		{
			Waypoint cacheA = (Waypoint)model.GetValue (tia, 0);
			Waypoint cacheB = (Waypoint)model.GetValue (tib, 0);
			if (cacheA == null || cacheB == null)
				return 0;
			return String.Compare (cacheA.Symbol, cacheB.Symbol);
		}
		
		private int LocationCompare (TreeModel model, TreeIter tia, TreeIter tib)
		{
			Waypoint cacheA = (Waypoint)model.GetValue (tia, 0);
			Waypoint cacheB = (Waypoint)model.GetValue (tib, 0);
			if (cacheA == null || cacheB == null)
				return 0;
			double compare = GetDistanceFromParent (cacheA) - GetDistanceFromParent(cacheB);
			if (compare > 0)
				return 1; else if (compare == 0)
				return 0;
			else
				return -1;
		}
		
		public double GetDistanceFromParent (Waypoint pt)
		{
			if (pt == null)
				return 0;
			return Utilities.calculateDistance (m_mon.SelectedCache.Lat, pt.Lat, m_mon.SelectedCache.Lon, pt.Lon);
		}

		
		private int CodeCompare (TreeModel model, TreeIter tia, TreeIter tib)
		{
			Waypoint cacheA = (Waypoint)model.GetValue (tia, 0);
			Waypoint cacheB = (Waypoint)model.GetValue (tib, 0);
			if (cacheA == null || cacheB == null)
				return 0;
			return String.Compare (cacheA.Name, cacheB.Name);
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
					editButton.Sensitive = true;
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
			m_ListSort.SetSortColumnId (1, SortType.Ascending);
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
		
		private void RenderIcon (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Waypoint pt = (Waypoint)model.GetValue (iter, 0);
			CellRendererPixbuf icon = cell as CellRendererPixbuf;
			if (pt is Geocache)
				icon.Pixbuf = IconManager.GetSmallCacheIcon ((pt as Geocache).TypeOfCache);
			else
				icon.Pixbuf = IconManager.GetSmallWaypointIcon(pt.Symbol);
		}

		protected virtual void DoEdit (object sender, System.EventArgs e)
		{
			try {
				
				Gtk.TreeIter itr;
				Gtk.TreeModel model;
				if (wptView.Selection.GetSelected (out model, out itr)) {
					Waypoint wpt = (Waypoint)model.GetValue (itr, 0);
					if (wpt is Geocache)
					{
						m_mon.ModifyCache();
						return;
					}
					WaypointDialog dlg = new WaypointDialog ();
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
				String name = "FL" + parent.Name.Substring (2);
				WaypointDialog dlg = new WaypointDialog ();
				if ((bool) m_mon.Configuration.Get("/apps/ocm/noprefixes", false))
				{
					name = parent.Name;
					dlg.IgnorePrefix = true;
				}
				name = Engine.getInstance().Store.GenerateNewName(name);
				newPoint.Name = name;
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
		
		public void GrabWaypoints()
		{
			Geocache cache = m_mon.SelectedCache;
			String expr = @"[NnSs].[0-9]*. [0-9]*\.[0-9]*. [WwEe].[0-9]*. [0-9]*\.[0-9]*";
			MatchCollection matches = Regex.Matches(cache.LongDesc, expr);
			System.Console.WriteLine(matches.Count);

		}
		
		protected virtual void OnGrabClick (object sender, System.EventArgs e)
		{
			GrabWaypoints();
		}
		
		
	}
}
