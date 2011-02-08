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
				if (val is Geocache){
					editButton.Sensitive = true;
					deleteButton.Sensitive = false;
				} 
				else if (val.Type == "Geocache - Original")
				{
					editButton.Sensitive = false;
					deleteButton.Sensitive = false;
				}
				else 
				{
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
			{
				addButton.Sensitive = false;
				grabButton.Sensitive = false;
				return;
			}
			else
			{
				addButton.Sensitive = true;
				grabButton.Sensitive = true;
			}
			ShowChildWaypoints (cache);
			if (cache.HasCorrected)
			{
				Waypoint Orig = new Waypoint();
				Orig.Name = cache.Name + "-ORIG";
				Orig.Lat = cache.OrigLat;
				Orig.Lon = cache.OrigLon;
				Orig.Symbol = "Geocache";
				Orig.Type = "Geocache - Original";
				Orig.Desc = Catalog.GetString("Original Location");
				m_mon.AddMapWayPoint (Orig, cache);
				m_childPoints.AppendValues(Orig);
			}
			m_mon.SetProgressDone(false);
			m_ListSort.SetSortColumnId (1, SortType.Ascending);
			m_mon.Main.QueueDraw();
			if (m_mon.ShowNearby)
				m_mon.GetNearByCaches();
		}
		
		public void ShowChildWaypoints (Geocache cache)
		{
			List<Waypoint> wpt = Engine.getInstance ().Store.GetChildren(cache.Name);
			IEnumerator<Waypoint> wptenum = wpt.GetEnumerator();
			m_childPoints.AppendValues (cache);
			m_mon.AddMapCache (cache);
			cache.Children = false;
			while (wptenum.MoveNext ()) {
				m_childPoints.AppendValues (wptenum.Current);
				m_mon.AddMapWayPoint (wptenum.Current, cache);
				cache.Children = true;		
			}
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
				if (m_mon.Configuration.IgnoreWaypointPrefixes)
				{
					name = parent.Name;
					dlg.IgnorePrefix = true;
				}
				name = Engine.getInstance().Store.GenerateNewName(name);
				newPoint.Name = name;
				dlg.SetPoint (newPoint);
				if ((int)ResponseType.Ok == dlg.Run ()) {
					newPoint = dlg.GetPoint ();
					if (newPoint.Symbol == "Final Location")
						parent.HasFinal = true;
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
					if (toDelete.Symbol == "Final Location")
						m_mon.SelectedCache.HasFinal = false;
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
			String expr = @"\b[NnSs] ?[0-9]+.? ?[0-9]*\.[0-9]*\s[WwEe] ?[0-9]+.? ?[0-9]*\.[0-9]*";
			String desc = cache.ShortDesc + cache.LongDesc;
			MatchCollection matches = Regex.Matches(desc, expr);
			m_mon.StartProgressLoad(Catalog.GetString("Grabbing Waypoints"), false);
			if (matches.Count > 0)
			{
				ScanWaypointsDialog dlg = new ScanWaypointsDialog(matches.Count, this, matches);
				dlg.Run();
			}
			else
			{
				MessageDialog dlg = new MessageDialog(m_mon.Main, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok,
				                                      Catalog.GetString("OCM was unable to find any child waypoints."));
				dlg.Run();
				dlg.Hide();
			}
			m_mon.SetProgressDone(false);
		}
		
		public void AutoGenerateChildren (MatchCollection matches)
		{
			int count = 0;
			foreach (Match match in matches)
			{
				m_mon.SetProgress((double) count, (double) matches.Count, Catalog.GetString("Processing Children..."), false);
				DegreeMinutes[] coord =  Utilities.ParseCoordString(match.Captures[0].Value);
				System.Console.WriteLine(Utilities.getCoordString(coord[0], coord[1]));
				
				Waypoint newPoint = new Waypoint ();
				Geocache parent = m_mon.SelectedCache;
				newPoint.Symbol = "Reference Point";
				newPoint.Parent = parent.Name;
				newPoint.Lat = coord[0].GetDecimalDegrees();
				newPoint.Lon = coord[1].GetDecimalDegrees();
				newPoint.Desc = Catalog.GetString("Grabbed Waypoint");
				String name = "RP" + parent.Name.Substring (2);
				if (m_mon.Configuration.IgnoreWaypointPrefixes)
				{
					name = parent.Name;
				}
				name = Engine.getInstance().Store.GenerateNewName(name);
				newPoint.Name = name;
				CacheStore store = Engine.getInstance ().Store;
				store.AddWaypointAtomic (newPoint);
				count++;
			}
		}
		
		protected virtual void OnGrabClick (object sender, System.EventArgs e)
		{
			GrabWaypoints();
		}
		
		
	}
}
