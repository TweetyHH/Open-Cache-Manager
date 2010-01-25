
using System;
using System.Collections.Generic;
using Gtk;
using ocmengine;
using Mono.Unix;

namespace ocmgtk
{
	
	
	[System.ComponentModel.ToolboxItem(true)]
	public partial class CacheList : Gtk.Bin
	{
		const string START_FOUND = "<span fgcolor='darkgreen'>";
		const string START_ARCHIVE = "<span fgcolor='red' strikethrough='true'>";
		const string START_UNAVAIL = "<span fgcolor='red'>";
		const string END_SPAN= "</span>";
		const string FOUND_CACHE = "Geocache Found";
		
		private TreeModelFilter m_QuickFilter;
		private TreeModelSort m_ListSort;
		private ListStore m_cacheModel;
		private Menu filterPopup;
		private bool m_showUnavailble = true;
		private bool m_showArchived = true;
		private bool m_showMine = true;
		private bool m_showFound = true;

		public CacheList()
		{
			this.Build();
			PopulateList();
			CreateFilterPopup();
		}
		
		private void CreateFilterPopup()
		{
			filterPopup = new Menu();
			CheckMenuItem showFoundItem = new CheckMenuItem("Found Caches");
			filterPopup.Add(showFoundItem);
		}
		
		private void PopulateList()
		{
			m_cacheModel = new ListStore(typeof (Geocache));
			filterEntry.Changed += OnFilterChange;
			
			CellRendererText geocode_cell = new CellRendererText();
			CellRendererText geotitle_cell = new CellRendererText();
			CellRendererText geodistance_cell = new CellRendererText();
			CellRendererPixbuf geoicon_cell = new CellRendererPixbuf();
			TreeViewColumn geocache_icon = new TreeViewColumn(Catalog.GetString("Type"), geoicon_cell, "text", 0);
			TreeViewColumn geocache_code = new TreeViewColumn(Catalog.GetString("Code"), geocode_cell, "text", 1);
			TreeViewColumn geocache_distance = new TreeViewColumn(Catalog.GetString("Km"), geodistance_cell, "text", 2);
			TreeViewColumn geocache_title = new TreeViewColumn(Catalog.GetString("Title"), geotitle_cell, "text", 3);
			
			treeview1.AppendColumn(geocache_icon);
			treeview1.AppendColumn(geocache_code);
			treeview1.AppendColumn(geocache_distance);
			treeview1.AppendColumn(geocache_title);
			
			
			geocache_code.SetCellDataFunc (geocode_cell, new TreeCellDataFunc (RenderCacheCode));
			geocache_title.SetCellDataFunc (geotitle_cell, new TreeCellDataFunc(RenderCacheTitle));
			geocache_title.SortColumnId = 0;
			geocache_title.SortIndicator = true;
			geocache_icon.SetCellDataFunc (geoicon_cell, new TreeCellDataFunc(RenderCacheIcon));
			geocache_icon.SortColumnId = 2;
			geocache_distance.SetCellDataFunc (geodistance_cell, new TreeCellDataFunc(RenderCacheDistance));
			geocache_distance.SortColumnId = 1;
			geocache_distance.SortIndicator = true;

			
			m_QuickFilter = new TreeModelFilter(m_cacheModel, null);
			m_QuickFilter.VisibleFunc = QuickFilter;
			m_ListSort = new TreeModelSort(m_QuickFilter);
			m_ListSort.SetSortFunc(0, TitleCompare); 
			m_ListSort.SetSortFunc(1, DistanceCompare);
			m_ListSort.SetSortFunc(2, SymbolCompare);
			treeview1.Model = m_ListSort;
			
			IEnumerator<Geocache> cache_enum =  Engine.getInstance().getCacheEnumerator();
			while (cache_enum.MoveNext())
			{
				m_cacheModel.AppendValues(cache_enum.Current);
			}
			
			treeview1.Selection.Changed += OnSelectionChanged;		
			this.ShowAll();
		}
		
		private void RenderCacheCode (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Geocache cache = (Geocache) model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			if (!cache.Available && ! cache.Archived)
				text.Markup = unavailText(cache.Name);
			else if (cache.Archived)
				text.Markup = archiveText(cache.Name);
			else 
				text.Markup = cache.Name;
		}
 
		private void RenderCacheTitle (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Geocache cache = (Geocache) model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			if (!cache.Available && ! cache.Archived)
				text.Markup = unavailText(cache.CacheName);
			else if (cache.Archived)
				text.Markup = archiveText(cache.CacheName);
			else 
				text.Markup = cache.CacheName;
		}
		
		private void RenderCacheIcon (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Geocache cache = (Geocache) model.GetValue (iter, 0);
			CellRendererPixbuf icon = cell as CellRendererPixbuf;
			if (cache.Symbol == "Geocache Found")
				icon.Pixbuf = UIMonitor.getSmallCacheIcon(Geocache.CacheType.FOUND);
			else
				icon.Pixbuf = UIMonitor.getSmallCacheIcon(cache.TypeOfCache);
		}
		
		private void RenderCacheDistance (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Geocache cache = (Geocache) model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			try
			{
				double dist = Utilities.calculateDisatnce(UIMonitor.getInstance().HomeLat, cache.Lat, UIMonitor.getInstance().HomeLon, cache.Lon);
				text.Text = dist.ToString("0.00");
			}
			catch (Exception e)
			{
				text.Text = "?";
				System.Console.WriteLine(e.Message);
			}
		}
		
		private void OnSelectionChanged(object sender, EventArgs e) 
		{
				TreeIter iter;
                TreeModel model;
 
			    if (((TreeSelection)sender).GetSelected (out model, out iter))
                {

					Geocache val = (Geocache) model.GetValue (iter, 0);
					if (val != null)
                      		UIMonitor.getInstance().setSelectedCache(val);
				}
                
		}
				
		private string unavailText(string text)
		{
			return START_UNAVAIL + text + END_SPAN;
		}
		
		private string archiveText(string text)
		{
			return START_ARCHIVE + text + END_SPAN;
		}
		
		private string foundText(string text)
		{
			return START_FOUND + text + END_SPAN;
		}

		protected virtual void OnFilterChange (object o, EventArgs args)
		{
			m_QuickFilter.Refilter();
			UpdateCountStatus();
		}
		
		private int TitleCompare(TreeModel model, TreeIter tia, TreeIter tib)
		{
			Geocache cacheA = (Geocache) model.GetValue(tia, 0);
			Geocache cacheB = (Geocache) model.GetValue(tib, 0);
			return String.Compare(cacheA.CacheName, cacheB.CacheName);
		}
		
		private int DistanceCompare(TreeModel model, TreeIter tia, TreeIter tib)
		{
			Geocache cacheA = (Geocache) model.GetValue(tia, 0);
			Geocache cacheB = (Geocache) model.GetValue(tib, 0);
			double compare = getDistanceFromHome(cacheA) - getDistanceFromHome(cacheB);
			if (compare > 0)
				return 1;
			else if (compare == 0)
				return 0;
			else 
				return -1;
		}
		
		private int SymbolCompare(TreeModel model, TreeIter tia, TreeIter tib)
		{
			Geocache cacheA = (Geocache) model.GetValue(tia, 0);
			Geocache cacheB = (Geocache) model.GetValue(tib, 0);
			return String.Compare(cacheA.TypeOfCache.ToString(), cacheB.TypeOfCache.ToString());
		}
		
		/*private bool SortCaches(int column, TreeItr itr)
		{
			ts.SetSortFunc (0, col0_compare, IntPtr.Zero, null); // use col0_compare to sort
		}
		
		public int col0_compare (TreeModel model, TreeIter tia, TreeIter tib)	{
		return String.Compare ((string) model.GetValue (tia, 0),
				(string) model.GetValue (tib, 0));
		}*/

		
		private bool QuickFilter(TreeModel model, TreeIter itr)
		{
			Geocache cache = (Geocache) model.GetValue(itr, 0);
			String filterVal = filterEntry.Text.ToLower();
			
			if (!m_showArchived && cache.Archived)
				return false;
			
			if (!m_showUnavailble && !cache.Available)
				return false;
			
			if (!m_showFound && cache.Symbol == FOUND_CACHE)
				return false;
			
			
			if (!String.IsNullOrEmpty(filterVal))
			{
				if ((cache.Name.ToLower().Contains(filterVal)) ||
				    (cache.CacheName.ToLower().Contains(filterVal)))
				{
				    return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return true;
			}
		}
		
		public void ToggleArchivedCaches()
		{
			m_showArchived = !m_showArchived;
			m_QuickFilter.Refilter();
		}
		
		public void ToggleUnavailableCaches()
		{
			m_showUnavailble = !m_showUnavailble;
			m_QuickFilter.Refilter();
		}
		
		public void ToggleFoundCaches()
		{
			m_showFound = !m_showFound;
			m_QuickFilter.Refilter();
		}
		
		public double getDistanceFromHome(Geocache cache)
		{
			return Utilities.calculateDisatnce(UIMonitor.getInstance().HomeLat, cache.Lat, UIMonitor.getInstance().HomeLon, cache.Lon);
		}
		
		
		
		public void UpdateCountStatus()
		{
			int totalCaches = 0;
			UIMonitor.getInstance().setCacheCountStatus(totalCaches);
		}

		protected virtual void OnFilterButtonClick (object sender, System.EventArgs e)
		{
			//filterPopup.Popup(filterButton, null, null, 1, Global.CurrentEventTime);
		}

	}
}
