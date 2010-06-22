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
using ocmengine;
using Mono.Unix;
using Gtk;

namespace ocmgtk
{


	[System.ComponentModel.ToolboxItem(true)]
	public partial class DescriptionWidget : Gtk.Bin
	{
		HTMLWidget descWidget, hintWidget;
		ListStore tbStore;

		public DescriptionWidget ()
		{
			this.Build ();
			descWidget = new HTMLWidget ();
			hintWidget = new HTMLWidget();
			descScroll.Add (descWidget);
			hintExpander.Add(hintWidget);			
			SetupTBList ();
		}

		public void UpdateCacheInfo ()
		{
			Geocache cache = UIMonitor.getInstance ().SelectedCache;
			if (cache == null)
			{
				descWidget.HTML = "<b>NO CACHE SELECTED</b>";
				return;
			}
			SetDescription (cache);
			SetTravelBugs (Engine.getInstance ().Store.GetTravelBugs (cache.Name));
			if (String.IsNullOrEmpty (cache.Hint) || String.IsNullOrEmpty(cache.Hint.Trim())) {
				hintExpander.Sensitive = false;
				hintExpander.Expanded = false;
			} else {
				hintWidget.HTML = "<div style='font-family:sans-serif;font-size:10pt; background-color:#FFFFFF'>" + cache.Hint  + "</div>";
				hintExpander.Sensitive = true;
				hintExpander.Expanded = false;
			}
		}

		public void SetupTBList ()
		{
			tbStore = new ListStore (typeof(TravelBug));
			CellRendererText tbref_cell = new CellRendererText ();
			CellRendererText tbname_cell = new CellRendererText ();
			TreeViewColumn tbref_col = new TreeViewColumn (Catalog.GetString ("Ref"), tbref_cell);
			TreeViewColumn tbname_col = new TreeViewColumn (Catalog.GetString ("Name"), tbname_cell);
			tbref_col.SetCellDataFunc (tbref_cell, RenderRefCell);
			tbname_col.SetCellDataFunc (tbname_cell, RenderNameCell);
			tbugView.AppendColumn (tbref_col);
			tbugView.AppendColumn (tbname_col);
			tbugView.Model = tbStore;
		}

		private void RenderRefCell (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			TravelBug bug = (TravelBug)model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			text.Text = bug.Ref;
		}

		private void RenderNameCell (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			TravelBug bug = (TravelBug)model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			text.Text = bug.Name;
		}


		public void SetTravelBugs (System.Collections.Generic.List<TravelBug> bugs)
		{
			tbStore.Clear ();
			IEnumerator<TravelBug> itr = bugs.GetEnumerator ();
			while (itr.MoveNext ()) {
				tbStore.AppendValues (itr.Current);
			}
			if (bugs.Count == 0)
				tbugExpander.Sensitive = false;
			else
				tbugExpander.Sensitive = true;
		}

		public void SetDescription (Geocache cache)
		{
			descWidget.HTML = "<div style='font-family:sans-serif;font-size:10pt; background-color:#FFFFFF'>" + cache.ShortDesc + "\n\n" 
				+ cache.LongDesc + "</div>";
		}
	}
}
