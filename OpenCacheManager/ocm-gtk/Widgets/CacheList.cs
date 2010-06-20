
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Gtk;
using ocmengine;
using Mono.Unix;
using System.Timers;

namespace ocmgtk
{


	[System.ComponentModel.ToolboxItem(true)]
	public partial class CacheList : Gtk.Bin
	{
		const string START_FOUND = "<span fgcolor='darkgreen'>";
		const string START_ARCHIVE = "<span fgcolor='red' strikethrough='true'>";
		const string START_UNAVAIL = "<span fgcolor='red'>";
		const string END_SPAN = "</span>";
		const string FOUND_CACHE = "Geocache Found";

		private TreeModelFilter m_QuickFilter;
		private TreeModelSort m_ListSort;
		private ListStore m_cacheModel;

		private bool m_showUnavailble = true;
		private bool m_showAvailable = true;
		private bool m_showArchived = true;
		private bool m_showMine = true;
		private bool m_showFound = true;
		private bool m_showNotFound = true;
		private UIMonitor m_monitor;
		private double m_maxDistance = -1;

		private Timer refreshTimer;

		public CacheList ()
		{
			this.Build ();
			BuildList ();
			m_monitor = UIMonitor.getInstance ();
			m_monitor.CacheListPane = this;
			refreshTimer = new Timer ();
			refreshTimer.AutoReset = false;
			refreshTimer.Elapsed += HandleRefreshTimerElapsed;
		}

		void HandleRefreshTimerElapsed (object sender, ElapsedEventArgs e)
		{
			Application.Invoke (delegate { RefilterList (); });
		}


		private void BuildList ()
		{
			m_cacheModel = new ListStore (typeof(Geocache));
			filterEntry.Changed += OnFilterChange;
			distanceEntry.Changed += onEditDone;
			
			CellRendererText geocode_cell = new CellRendererText ();
			CellRendererText geotitle_cell = new CellRendererText ();
			CellRendererText geodistance_cell = new CellRendererText ();
			CellRendererPixbuf geoicon_cell = new CellRendererPixbuf ();
			TreeViewColumn geocache_icon = new TreeViewColumn (Catalog.GetString ("Type"), geoicon_cell);
			TreeViewColumn geocache_code = new TreeViewColumn (Catalog.GetString ("Code"), geocode_cell);
			TreeViewColumn geocache_distance = new TreeViewColumn (Catalog.GetString ("Km"), geodistance_cell);
			TreeViewColumn geocache_title = new TreeViewColumn (Catalog.GetString ("Title"), geotitle_cell);
			
			treeview1.AppendColumn (geocache_icon);
			treeview1.AppendColumn (geocache_code);
			treeview1.AppendColumn (geocache_distance);
			treeview1.AppendColumn (geocache_title);
			
			
			geocache_code.SetCellDataFunc (geocode_cell, new TreeCellDataFunc (RenderCacheCode));
			geocache_code.SortColumnId = 1;
			geocache_title.SetCellDataFunc (geotitle_cell, new TreeCellDataFunc (RenderCacheTitle));
			geocache_title.SortColumnId = 3;
			geocache_icon.SetCellDataFunc (geoicon_cell, new TreeCellDataFunc (RenderCacheIcon));
			geocache_icon.SortColumnId = 0;
			geocache_distance.SetCellDataFunc (geodistance_cell, new TreeCellDataFunc (RenderCacheDistance));
			geocache_distance.SortColumnId = 2;
			
			
			m_QuickFilter = new TreeModelFilter (m_cacheModel, null);
			m_QuickFilter.VisibleFunc = QuickFilter;
			m_ListSort = new TreeModelSort (m_QuickFilter);
			m_ListSort.SetSortFunc (3, TitleCompare);
			m_ListSort.SetSortFunc (2, DistanceCompare);
			m_ListSort.SetSortFunc (0, SymbolCompare);
			treeview1.Model = m_ListSort;
			treeview1.Selection.Changed += OnSelectionChanged;
		}


		public List<Geocache> getVisibleCaches ()
		{
			List<Geocache> caches = new List<Geocache> ();
			TreeIter itr;
			m_ListSort.GetIterFirst (out itr);
			if (!m_ListSort.IterIsValid (itr))
				return caches;
			do {
				caches.Add ((Geocache)m_ListSort.GetValue (itr, 0));
			} while (m_ListSort.IterNext (ref itr));
			return caches;
		}

		public int GetVisibleFoundCacheCount ()
		{
			int count = 0;
			TreeIter itr;
			m_ListSort.GetIterFirst (out itr);
			if (!m_ListSort.IterIsValid (itr))
				return 0;
			do {
				Geocache cache = (Geocache)m_ListSort.GetValue (itr, 0);
				if (cache.Symbol.Equals ("Geocache Found"))
					count++;
			} while (m_ListSort.IterNext (ref itr));
			return count;
		}

		public int GetVisibleInactiveCacheCount ()
		{
			int count = 0;
			TreeIter itr;
			m_ListSort.GetIterFirst (out itr);
			if (!m_ListSort.IterIsValid (itr))
				return 0;
			do {
				Geocache cache = (Geocache)m_ListSort.GetValue (itr, 0);
				if (cache.Available == false)
					count++;
			} while (m_ListSort.IterNext (ref itr));
			return count;
		}

		public int GetOwnedCount ()
		{
			int count = 0;
			TreeIter itr;
			m_ListSort.GetIterFirst (out itr);
			if (!m_ListSort.IterIsValid (itr))
				return 0;
			do {
				Geocache cache = (Geocache)m_ListSort.GetValue (itr, 0);
				if (cache.OwnerID == m_monitor.OwnerID)
					count++;
			} while (m_ListSort.IterNext (ref itr));
			return count;
		}

		public void PopulateList ()
		{
			m_cacheModel.Clear ();
			m_ListSort.SetSortColumnId (-1, SortType.Ascending);
			IEnumerator<Geocache> cache_enum = Engine.getInstance ().getCacheEnumerator ();
			while (cache_enum.MoveNext ()) {
				m_cacheModel.AppendValues (cache_enum.Current);
			}
			m_ListSort.SetSortColumnId (2, SortType.Ascending);
		}

		private void RenderCacheCode (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Geocache cache = (Geocache)model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			if (!cache.Available && !cache.Archived)
				text.Markup = unavailText (cache.Name); else if (cache.Archived)
				text.Markup = archiveText (cache.Name);
			else
				text.Markup = cache.Name;
		}

		private void RenderCacheTitle (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Geocache cache = (Geocache)model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			
			if (!cache.Available && !cache.Archived)
				text.Markup = unavailText (cache.CacheName); else if (cache.Archived)
				text.Markup = archiveText (cache.CacheName);
			else
				text.Markup = GLib.Markup.EscapeText (cache.CacheName);
		}

		private void RenderCacheIcon (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Geocache cache = (Geocache)model.GetValue (iter, 0);
			CellRendererPixbuf icon = cell as CellRendererPixbuf;
			if (cache.Found)
				icon.Pixbuf = UIMonitor.GetSmallCacheIcon (Geocache.CacheType.FOUND); else if (cache.OwnerID == m_monitor.OwnerID)
				icon.Pixbuf = UIMonitor.GetSmallCacheIcon (Geocache.CacheType.MINE);
			else
				icon.Pixbuf = UIMonitor.GetSmallCacheIcon (cache.TypeOfCache);
		}

		private void RenderCacheDistance (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Geocache cache = (Geocache)model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			try {
				double dist = Utilities.calculateDistance (m_monitor.CentreLat, cache.Lat, m_monitor.CentreLon, cache.Lon);
				text.Text = dist.ToString ("0.00");
			} catch (Exception e) {
				text.Text = "?";
				System.Console.WriteLine (e.Message);
			}
		}

		private void OnSelectionChanged (object sender, EventArgs e)
		{
			TreeIter iter;
			TreeModel model;
			
			if (((TreeSelection)sender).GetSelected (out model, out iter)) {
				
				Geocache val = (Geocache)model.GetValue (iter, 0);
				m_monitor.SetSelectedCache (val);
			}
			else
				m_monitor.SetSelectedCache(null);
		}

		private string unavailText (string text)
		{
			return START_UNAVAIL + GLib.Markup.EscapeText (text) + END_SPAN;
		}

		private string archiveText (string text)
		{
			return START_ARCHIVE + GLib.Markup.EscapeText (text) + END_SPAN;
		}

		protected virtual void OnFilterChange (object o, EventArgs args)
		{
			StartRefilterList ();
		}

		private void StartRefilterList ()
		{
			refreshTimer.Interval = 300;
			if (!refreshTimer.Enabled)
				refreshTimer.Enabled = true;
		}

		private void RefilterList ()
		{
			m_monitor.StartFiltering ();
			m_QuickFilter.Refilter ();
			m_monitor.UpdateStatusBar ();
		}

		private int TitleCompare (TreeModel model, TreeIter tia, TreeIter tib)
		{
			Geocache cacheA = (Geocache)model.GetValue (tia, 0);
			Geocache cacheB = (Geocache)model.GetValue (tib, 0);
			return String.Compare (cacheA.CacheName, cacheB.CacheName);
		}

		private int DistanceCompare (TreeModel model, TreeIter tia, TreeIter tib)
		{
			Geocache cacheA = (Geocache)model.GetValue (tia, 0);
			Geocache cacheB = (Geocache)model.GetValue (tib, 0);
			double compare = getDistanceFromHome (cacheA) - getDistanceFromHome (cacheB);
			if (compare > 0)
				return 1; else if (compare == 0)
				return 0;
			else
				return -1;
		}

		private int SymbolCompare (TreeModel model, TreeIter tia, TreeIter tib)
		{
			Geocache cacheA = (Geocache)model.GetValue (tia, 0);
			Geocache cacheB = (Geocache)model.GetValue (tib, 0);
			return String.Compare (cacheA.TypeOfCache.ToString (), cacheB.TypeOfCache.ToString ());
		}


		private bool QuickFilter (TreeModel model, TreeIter itr)
		{
			Geocache cache = (Geocache)model.GetValue (itr, 0);
			if (cache == null)
				return false;
			String filterVal = filterEntry.Text.ToLower ();
			
			//TODO : PUSH THESE STATUS FLAGS TO DATABASE TO IMPROVE
			// PERFORMANCE
			if (!m_showArchived && cache.Archived)
				return false;
			
			if (!m_showMine && (cache.OwnerID == m_monitor.OwnerID))
				return false;
			
			if (!m_showNotFound && cache.Symbol != FOUND_CACHE )
				if (m_showMine && (cache.OwnerID == m_monitor.OwnerID))
					return true;
				else
					return false;
			
			if (!m_showUnavailble && !cache.Available && !cache.Archived)
				return false;
			
			if (!m_showFound && cache.Symbol == FOUND_CACHE)
				if (m_showMine && (cache.OwnerID == m_monitor.OwnerID))
					return true;
				else
					return false;
			
			if (!m_showAvailable && cache.Available)
				return false;
			
			if (m_maxDistance > 0)
				if (getDistanceFromHome (cache) > m_maxDistance)
					return false;
			
			
			if (!String.IsNullOrEmpty (filterVal)) {
				if ((cache.Name.ToLower ().Contains (filterVal)) || (cache.CacheName.ToLower ().Contains (filterVal))) {
					return true;
				} else {
					return false;
				}
			} else {
				return true;
			}
		}

		public void ToggleArchivedCaches ()
		{
			m_showArchived = !m_showArchived;
			RefilterList ();
		}

		public void ToggleUnavailableCaches ()
		{
			m_showUnavailble = !m_showUnavailble;
			RefilterList ();
		}

		public void ToggleFoundCaches ()
		{
			m_showFound = !m_showFound;
			RefilterList ();
		}

		public void ToggleUnFoundCaches ()
		{
			m_showNotFound = !m_showNotFound;
			RefilterList ();
		}

		public void ToggleMyCaches ()
		{
			m_showMine = !m_showMine;
			RefilterList ();
		}

		public double getDistanceFromHome (Geocache cache)
		{
			return Utilities.calculateDistance (m_monitor.CentreLat, cache.Lat, m_monitor.CentreLon, cache.Lon);
		}

		[GLib.ConnectBefore]
		protected virtual void DoButtonPress (object o, Gtk.ButtonPressEventArgs args)
		{
			if (args.Event.Button == 3) {
				CreatePopup ();
			}
		}

		private void CreatePopup ()
		{
			Menu popup = new Menu ();
			MenuItem setCenterItem = new MenuItem ("Set As Map Centre");
			MenuItem showOnline = new MenuItem ("View Cache Online");
			MenuItem deleteItem = new MenuItem ("Delete...");
			
			setCenterItem.Activated += HandleSetCenterItemActivated;
			showOnline.Activated += HandleShowOnlineActivated;
			deleteItem.Activated += HandleDeleteItemActivated;
			
			popup.Add (setCenterItem);
			popup.Add (showOnline);
			popup.Add (deleteItem);
			popup.ShowAll ();
			popup.Popup ();
		}

		void HandleSetHomeItemActivated (object sender, EventArgs e)
		{
			// Clear any sorting and filtering
			m_ListSort.SetSortColumnId (-1, SortType.Ascending);
			
			Geocache cache = m_monitor.SelectedCache;
			if (cache == null)
				return;
			GConf.Client client = new GConf.Client ();
			try {
				m_monitor.CentreLat = (double)client.Get ("/apps/ocm/homelat");
				m_monitor.CentreLon = (double)client.Get ("/apps/ocm/homelon");
			} catch (GConf.NoSuchKeyException) {
				// Do nothing
			}
			m_monitor.RefreshCaches ();
		}

		void HandleDeleteItemActivated (object sender, EventArgs e)
		{
			m_monitor.DeleteCache();
		}

		void HandleShowOnlineActivated (object sender, EventArgs e)
		{
			Process.Start (m_monitor.SelectedCache.URL.ToString ());
		}

		void HandleSetCenterItemActivated (object sender, EventArgs e)
		{
			m_monitor.SetSelectedAsCentre();
		}
		protected virtual void OnFoundButtonToggled (object sender, System.EventArgs e)
		{
			ToggleFoundCaches ();
		}

		protected virtual void OnMineToggled (object sender, System.EventArgs e)
		{
			ToggleMyCaches ();
		}

		protected virtual void OnArchivedButtonToggled (object sender, System.EventArgs e)
		{
			ToggleArchivedCaches ();
		}


		protected virtual void OnUnavailableToggled (object sender, System.EventArgs e)
		{
			ToggleUnavailableCaches ();
		}

		protected virtual void OnClearClicked (object sender, System.EventArgs e)
		{
			filterEntry.Text = String.Empty;
		}

		protected virtual void onEditDone (object sender, System.EventArgs e)
		{
			try {
				m_maxDistance = double.Parse (distanceEntry.Text);
			} catch (Exception) {
				m_maxDistance = -1;
			}
			StartRefilterList ();
		}

		protected virtual void OnClearDistance (object sender, System.EventArgs e)
		{
			distanceEntry.Text = String.Empty;
			m_maxDistance = -1;
		}

		protected virtual void OnNotFoundToggled (object sender, System.EventArgs e)
		{
			ToggleUnFoundCaches ();
		}

		protected virtual void OnEditingDone (object sender, System.EventArgs e)
		{
			RefilterList ();
		}

		public void SelectCache (string code)
		{
			TreeIter itr;
			TreeModel model = treeview1.Model;
			treeview1.Model.GetIterFirst(out itr);
			do
			{
				Geocache cache = (Geocache)model.GetValue (itr, 0);
				System.Console.WriteLine(cache.Name);
				if (cache.Name == code)
				{
					treeview1.Selection.SelectIter(itr);
					TreePath path = treeview1.Model.GetPath(itr);
					treeview1.ScrollToCell(path, treeview1.Columns[0], true, 0, 0);
					return;
				}
			}
			while (model.IterNext(ref itr));
		}
		
		public void ScrollToSelected()
		{
			TreeIter itr;
			if (! treeview1.Selection.GetSelected(out itr))
				return;
			TreePath path = treeview1.Model.GetPath(itr);
			treeview1.ScrollToCell(path, treeview1.Columns[0], true, 0, 0);
		}
		
		protected virtual void OnAvailableToggle (object sender, System.EventArgs e)
		{
			m_showAvailable = !m_showAvailable;
			RefilterList();
		}
		
		
	}
}
