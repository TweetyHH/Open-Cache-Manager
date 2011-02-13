
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Gtk;
using ocmengine;
using Mono.Unix;
using System.Timers;
using System.Text;

namespace ocmgtk
{


	[System.ComponentModel.ToolboxItem(true)]
	public partial class CacheList : Gtk.Bin
	{
		const string START_FOUND = "<span fgcolor='darkgreen'>";
		const string START_ARCHIVE = "<span fgcolor='red' strikethrough='true'>";
		const string START_UNAVAIL = "<span fgcolor='red'>";
		const string START_RECENT_DNF = "<span fgcolor='darkorange'>";
		const string START_MY_DNF = "<span fgcolor='blue'>";
		const string START_ITALICS = "<i>";
		const string START_BOLD = "<b>";
		const string END_BOLD = "</b>";
		const string START_UNDERLINE = "<u>";
		const string END_UNDERLINE = "</u>";
		const string END_SPAN = "</span>";
		const string END_ITALICS = "</i>";
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
		private bool m_disableRefilter = false;
		private double m_loadTotal = 0;
		private double m_intervalCount = 0;
		private double m_loadCount = 0;
		private OCMTreeView treeview1 = null;
		private bool m_pulseMode = false;
		TreeViewColumn m_distanceCol;

		private Timer refreshTimer;

		public CacheList ()
		{
			this.Build ();
			comboBox.Remove(infoComboBox);
			advancedBox.Remove(infoHbox);
			treeview1 = new OCMTreeView();
			treeview1.ButtonPressEvent += DoButtonPress;
			cachescroll.Add(treeview1);
			BuildList ();
			m_monitor = UIMonitor.getInstance ();
			m_monitor.CacheListPane = this;
			refreshTimer = new Timer ();
			refreshTimer.AutoReset = false;
			refreshTimer.Elapsed += HandleRefreshTimerElapsed;
		}
		
		public void ShowInfoBox(bool isCombo)
		{
			ClearInfoBoxes();
			if (!isCombo)
			{
				advancedBox.Add(infoHbox);
				infoHbox.Show();
			}
			else
			{
				comboBox.Add(infoComboBox);
				infoComboBox.Show();
			}
		}
		
		
		public void ClearInfoBoxes()
		{
			advancedBox.Remove(infoHbox);
			comboBox.Remove(infoComboBox);
			ShowAll();
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
			m_distanceCol = new TreeViewColumn (Catalog.GetString ("Km"), geodistance_cell);
			TreeViewColumn geocache_title = new TreeViewColumn (Catalog.GetString ("Title"), geotitle_cell);
			
			treeview1.AppendColumn (geocache_icon);
			treeview1.AppendColumn (geocache_code);
			treeview1.AppendColumn (m_distanceCol);
			treeview1.AppendColumn (geocache_title);
				
			geocache_code.SetCellDataFunc (geocode_cell, new TreeCellDataFunc (RenderCacheCode));
			geocache_code.SortColumnId = 1;
			geocache_title.SetCellDataFunc (geotitle_cell, new TreeCellDataFunc (RenderCacheTitle));
			geocache_title.SortColumnId = 3;
			geocache_icon.SetCellDataFunc (geoicon_cell, new TreeCellDataFunc (RenderCacheIcon));
			geocache_icon.SortColumnId = 0;
			m_distanceCol.SetCellDataFunc (geodistance_cell, new TreeCellDataFunc (RenderCacheDistance));
			m_distanceCol.SortColumnId = 2;
			
			
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
		
		
		public void SetImperial()
		{
			m_distanceCol.Title = Catalog.GetString("Mi");
			distLabel.Text = Catalog.GetString("Miles");
		}
		
		public void SetMetric()
		{
			m_distanceCol.Title = Catalog.GetString("Km");
			distLabel.Text = Catalog.GetString("Km");
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
				if (cache.OwnerID == m_monitor.OwnerID || cache.CacheOwner == m_monitor.OwnerID)
					count++;
			} while (m_ListSort.IterNext (ref itr));
			return count;
		}

		public void PopulateList ()
		{
			CacheStore store = Engine.getInstance().Store;
			m_cacheModel.Clear ();
			m_loadTotal = store.CacheCount;
			m_loadCount = 0;
			m_intervalCount = 0;
			treeview1.Model = null;
			if (store.Filter != null && store.Filter.GetCount() > 1)
				m_pulseMode = true;
			else
				m_pulseMode = false;
			m_ListSort.SetSortColumnId (-1, SortType.Ascending);
			m_monitor.StartProgressLoad(Catalog.GetString("Loading Caches"), true);
			store.ReadCache += HandleStoreReadCache;
			store.Complete += HandleStoreComplete;
			store.GetCaches(m_monitor.CentreLat, m_monitor.CentreLon);
			treeview1.Model = m_ListSort;
			m_ListSort.SetSortColumnId (2, SortType.Ascending);
			System.Console.WriteLine(Engine.getInstance().Store.Filter);
		
			
		}

		void HandleStoreComplete (object sender, EventArgs args)
		{
			Engine.getInstance().Store.ReadCache -= HandleStoreReadCache;
			Engine.getInstance().Store.Complete -= HandleStoreComplete;
			m_monitor.SetProgressDone(true);
		}

		void HandleStoreReadCache (object sender, CacheStore.ReadCacheArgs args)
		{
			m_intervalCount ++;
			if (m_pulseMode)
			{
				if (m_intervalCount == 50)
				{
					m_monitor.SetProgressPulse();
					m_intervalCount = 0;
				}
			}	
			else
			{
				m_loadCount ++;
				
				if (m_intervalCount == 100)
				{
					m_monitor.SetProgress(m_loadCount, m_loadTotal, String.Format(Catalog.GetString("Loading Caches {0}"), (m_loadCount/m_loadTotal).ToString("0%")), true);
					m_intervalCount = 0;
				}
			}
			if (args.Cache != null)
				m_cacheModel.AppendValues(args.Cache);	
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
			
			StringBuilder builder = new StringBuilder();
			if (cache.Children)
				builder.Append(START_BOLD);
			if (!String.IsNullOrEmpty(cache.Notes))
				builder.Append(START_UNDERLINE);
			if (cache.HasCorrected)
				builder.Append(START_ITALICS);
			if (!cache.Available && !cache.Archived)
				builder.Append(unavailText (cache.CacheName));
			else if (cache.Archived)
				builder.Append(archiveText (cache.CacheName));
			else if (cache.DNF)
				builder.Append(START_MY_DNF + GLib.Markup.EscapeText (cache.CacheName) + END_SPAN);
			else if (cache.CheckNotes)
				builder.Append(START_RECENT_DNF + GLib.Markup.EscapeText (cache.CacheName) + END_SPAN);
			else
				builder.Append(GLib.Markup.EscapeText (cache.CacheName));
			if (cache.HasCorrected)
				builder.Append(END_ITALICS);
			if (!String.IsNullOrEmpty(cache.Notes))
				builder.Append(END_UNDERLINE);
			if (cache.Children)
				builder.Append(END_BOLD);
			text.Markup = builder.ToString();
		}

		private void RenderCacheIcon (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Geocache cache = (Geocache)model.GetValue (iter, 0);
			CellRendererPixbuf icon = cell as CellRendererPixbuf;
			if (cache.FTF)
				icon.Pixbuf = IconManager.FTF_S;
			else if (cache.DNF)
				icon.Pixbuf = IconManager.DNF_S;
			else if (cache.Found)
				icon.Pixbuf = IconManager.GetSmallCacheIcon (Geocache.CacheType.FOUND);
			else if ((cache.OwnerID == m_monitor.OwnerID  ) || (cache.CacheOwner == m_monitor.OwnerID))
				icon.Pixbuf = IconManager.GetSmallCacheIcon (Geocache.CacheType.MINE);
			else if (cache.HasCorrected || cache.HasFinal)
			{
				if (m_monitor.Configuration.SolvedModeState == SolvedMode.ALL)
					icon.Pixbuf = IconManager.CORRECTED_S;
				else if ((m_monitor.Configuration.SolvedModeState == SolvedMode.PUZZLES) && 
				        	(cache.TypeOfCache == Geocache.CacheType.MYSTERY))
					icon.Pixbuf = IconManager.CORRECTED_S;
				else
					icon.Pixbuf = IconManager.GetSmallCacheIcon (cache.TypeOfCache);
			}
			else
			{
				icon.Pixbuf = IconManager.GetSmallCacheIcon (cache.TypeOfCache);
			}
		}

		private void RenderCacheDistance (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Geocache cache = (Geocache)model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			try {
				double dist = cache.Distance;
				if (m_monitor.UseImperial)
					dist = Utilities.KmToMiles(dist);
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

		public void RefilterList ()
		{
			if (m_disableRefilter)
				return;
			m_monitor.StartFiltering ();
			m_QuickFilter.Refilter ();
			m_monitor.UpdateStatusBar ();
			m_monitor.GetNearByCaches();
		}

		private int TitleCompare (TreeModel model, TreeIter tia, TreeIter tib)
		{
			Geocache cacheA = (Geocache)model.GetValue (tia, 0);
			Geocache cacheB = (Geocache)model.GetValue (tib, 0);
			if (cacheA == null || cacheB == null)
				return 0 ;
			return String.Compare (cacheA.CacheName, cacheB.CacheName);
		}

		private int DistanceCompare (TreeModel model, TreeIter tia, TreeIter tib)
		{
			Geocache cacheA = (Geocache)model.GetValue (tia, 0);
			Geocache cacheB = (Geocache)model.GetValue (tib, 0);
			if (cacheA == null || cacheB == null)
				return 0;
			double compare = cacheA.Distance - cacheB.Distance;
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
			if (cacheA == null || cacheB == null)
				return 0;
			return String.Compare (cacheA.TypeOfCache.ToString (), cacheB.TypeOfCache.ToString ());
		}


		private bool QuickFilter (TreeModel model, TreeIter itr)
		{
			Geocache cache = (Geocache)model.GetValue (itr, 0);
			if (cache == null)
				return false;
			String filterVal = filterEntry.Text.ToLower ();
			
			if (!m_showArchived && cache.Archived)
				return false;
			
			if (!m_showMine && ((cache.OwnerID == m_monitor.OwnerID  ) || (cache.CacheOwner == m_monitor.OwnerID)))
				return false;
			
			if (!m_showNotFound && cache.Symbol != FOUND_CACHE )
				if (m_showMine && ((cache.OwnerID == m_monitor.OwnerID  ) || (cache.CacheOwner == m_monitor.OwnerID)))
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
				if (cache.Distance > m_maxDistance)
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


		[GLib.ConnectBeforeAttribute]
		protected virtual void DoButtonPress (object o, Gtk.ButtonPressEventArgs args)
		{
			if (args.Event.Button == 3) {
				CreatePopup (args);
			}
		}

		private void CreatePopup (Gtk.ButtonPressEventArgs args)
		{
			Menu popup = new Menu ();
			MenuItem setCenterItem = new MenuItem (Catalog.GetString("Set As Map Centre"));
			MenuItem showOnline = new MenuItem (Catalog.GetString("View Cache Online"));
			MenuItem mark = new MenuItem(Catalog.GetString("Mark"));
			Menu markSub = new Menu();
			MenuItem markFound = new MenuItem(Catalog.GetString("Mark Found"));
			MenuItem markFTF = new MenuItem(Catalog.GetString("Mark First To Find"));
			MenuItem markDNF = new MenuItem(Catalog.GetString("Mark Did Not Find"));
			MenuItem markUnfound = new MenuItem(Catalog.GetString("Mark Unfound"));
			MenuItem markDisabled = new MenuItem(Catalog.GetString("Mark Disabled"));
			MenuItem markArchived = new MenuItem(Catalog.GetString("Mark Archived"));
			MenuItem markAvailable = new MenuItem(Catalog.GetString("Mark Available"));
			MenuItem deleteItem = new MenuItem (Catalog.GetString("Delete..."));
			MenuItem bookmark = new MenuItem(Catalog.GetString("Add to Bookmark List"));
			MenuItem qlandkarte = new MenuItem(Catalog.GetString("View in QLandkarte GT..."));
			MenuItem logCache = new MenuItem(Catalog.GetString("Log Find"));
			
			TreePath path;
			TreeIter itr;
			treeview1.GetPathAtPos((int) args.Event.X,(int) args.Event.Y, out path);
			treeview1.Model.GetIter(out itr, path);
			Geocache cache = (Geocache)treeview1.Model.GetValue (itr, 0);
			
			if (cache != null)
			{
				logCache.Sensitive = true;
				if (!cache.Available)
				{
					markAvailable.Sensitive = true;
					markDisabled.Sensitive = false;
				}
				else
				{
					markAvailable.Sensitive = false;
					markDisabled.Sensitive = true;
				}
				
				if (!cache.Archived)
					markArchived.Sensitive = true;
				else
					markArchived.Sensitive = false;
				
				if (cache.Symbol.Contains("Found"))
				{
					if (cache.FTF)
					{
						markFTF.Sensitive = false;
						markFound.Sensitive = true;
					}
					else
					{
						markFTF.Sensitive = true;
						markFound.Sensitive = false;
					}
					markUnfound.Sensitive = true;
					markDNF.Sensitive = true;
					
				}
				else
				{
					markFound.Sensitive = true;
					markFTF.Sensitive = true;
					if (cache.DNF)
					{
						markDNF.Sensitive = false;
						markUnfound.Sensitive = true;
					}
					else
					{
						markDNF.Sensitive = true;
						markUnfound.Sensitive = false;
					}
				}
			}
			else
			{
				logCache.Sensitive = false;
			}
			
			CacheStore store = Engine.getInstance().Store;
			List<string> bookmarklists = store.GetBookmarkLists();
			if (bookmarklists.Count > 0)
			{
				Menu bookMarksSub = new Menu();
				foreach (String str in bookmarklists)
				{
					MenuItem itm = new MenuItem(str);
					if (str == store.BookmarkList)
						itm.Sensitive = false;
					bookMarksSub.Append(itm);
					itm.Activated += HandleItmActivated;
				}
				bookmark.Submenu = bookMarksSub;
			}		
			else
			{
				bookmark.Sensitive = false;
			}
			
			MenuItem rmvCache = new MenuItem(Catalog.GetString("Remove From Bookmark List"));
			if (store.BookmarkList == null)
				rmvCache.Sensitive = false;
			rmvCache.Activated += HandleRmvCacheActivated;
			
			setCenterItem.Activated += HandleSetCenterItemActivated;
			showOnline.Activated += HandleShowOnlineActivated;
			deleteItem.Activated += HandleDeleteItemActivated;
			markFound.Activated += HandleMarkFoundActivated;
			markUnfound.Activated += HandleMarkUnfoundActivated;
			markDisabled.Activated += HandleMarkDisabledActivated;
			markArchived.Activated += HandleMarkArchivedActivated;
			markAvailable.Activated += HandleMarkAvailableActivated;
			qlandkarte.Activated += HandleQlandkarteActivated;
			logCache.Activated += HandleLogCacheActivated;
			markDNF.Activated += HandleMarkDNFActivated;
			markFTF.Activated += HandleMarkFTFActivated;
		
			
			popup.Add (setCenterItem);
			popup.Add (showOnline);
			popup.Add (new MenuItem());
			popup.Add (logCache);
			popup.Add (mark);
			markSub.Add(markFound);
			markSub.Add(markFTF);
			markSub.Add(markDNF);
			markSub.Add(markUnfound);
			markSub.Add (markDisabled);
			markSub.Add (markArchived);
			markSub.Add (markAvailable);
			mark.Submenu = markSub;
			popup.Add (new MenuItem());
			popup.Add (bookmark);
			popup.Add (rmvCache);
			popup.Add (new MenuItem());
			popup.Add (qlandkarte);
			popup.Add (deleteItem);
			popup.ShowAll ();
			popup.Popup ();
		}

		void HandleMarkFTFActivated (object sender, EventArgs e)
		{
			m_monitor.MarkCacheFTF();
		}

		void HandleMarkDNFActivated (object sender, EventArgs e)
		{
			m_monitor.MarkCacheDNF();
		}

		void HandleLogCacheActivated (object sender, EventArgs e)
		{
			m_monitor.LogFind();
		}

		void HandleMarkUnfoundActivated (object sender, EventArgs e)
		{
			m_monitor.MarkCacheUnfound();
		}

		void HandleMarkFoundActivated (object sender, EventArgs e)
		{
			m_monitor.MarkCacheFound();
		}

		void HandleQlandkarteActivated (object sender, EventArgs e)
		{
			m_monitor.OpenSelectedCacheInQLandKarte();
		}

		void HandleRmvCacheActivated (object sender, EventArgs e)
		{
			m_monitor.RemoveSelFromBookmark();
		}

		void HandleItmActivated (object sender, EventArgs e)
		{
			m_monitor.BookmarkSelectedCache(((sender as MenuItem).Child as Label).Text);
		}

		void HandleMarkAvailableActivated (object sender, EventArgs e)
		{
			m_monitor.MarkCacheAvailable();
		}

		void HandleMarkArchivedActivated (object sender, EventArgs e)
		{
			m_monitor.MarkCacheArchived();
		}

		void HandleMarkDisabledActivated (object sender, EventArgs e)
		{
			m_monitor.MarkCacheDisabled();
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
				if (m_monitor.UseImperial)
					m_maxDistance = Utilities.MilesToKm(m_maxDistance);
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
			if (code == null)
			{
				treeview1.Selection.UnselectAll();
				return;
			}
			
			TreeIter itr;
			TreeModel model = treeview1.Model;
			treeview1.Model.GetIterFirst(out itr);
			do
			{
				Geocache cache = (Geocache)model.GetValue (itr, 0);
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
		
		public void ApplyQuickFilter(QuickFilter filter)
		{
			m_disableRefilter = true;
			FoundButton.Active = filter.Found;
			checkbutton1.Active = filter.NotFound;
			MineButton.Active = filter.Mine;
			checkbutton2.Active = filter.Available;
			UnavailableButton.Active = filter.Unavailable;
			ArchivedButton.Active = filter.Archived;
			if (filter.Distance > 0)
				distanceEntry.Text = filter.Distance.ToString();
			else
				distanceEntry.Text = String.Empty;
			if (!String.IsNullOrEmpty(filter.NameFilter))
				filterEntry.Text = filter.NameFilter;
			else
				filterEntry.Text = String.Empty;
			m_disableRefilter = false;
		}
		
		public void PopulateQuickFilter(QuickFilter filter)
		{
			filter.Found = FoundButton.Active;
			filter.NotFound = checkbutton1.Active;
			filter.Mine = MineButton.Active;
			filter.Available = checkbutton2.Active;
			filter.Unavailable = UnavailableButton.Active;
			filter.Archived = ArchivedButton.Active;
			if (!String.IsNullOrEmpty(distanceEntry.Text))
				filter.Distance = int.Parse(distanceEntry.Text);
			else
				filter.Distance = -1;
			filter.NameFilter = filterEntry.Text;
		}
		
		protected virtual void OnFilterClick (object sender, System.EventArgs e)
		{
			m_monitor.SetAdditonalFilters();
		}	
	
		protected virtual void OnComboClick (object sender, System.EventArgs e)
		{
			m_monitor.DoComboFilter();
		}	
	}
}
