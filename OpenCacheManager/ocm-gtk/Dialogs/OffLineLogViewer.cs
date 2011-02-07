// 
//  Copyright 2011  Kyle Campbell
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
using Gtk;
using ocmengine;
using System.Collections.Generic;
using Mono.Unix;

namespace ocmgtk
{


	public partial class OffLineLogViewer : Gtk.Dialog
	{
		private ListStore m_logList;
		private List<CacheLog> m_Logs;
		private TreeModelSort listSort;
		
		public OffLineLogViewer ()
		{
			this.Build ();
			BuildLogTreeWidget();
			this.ShowAll();
		}
		
		public void PopulateLogs(List<CacheLog> logs)
		{
			m_logList.Clear();
			m_Logs = logs;
			listSort.SetSortColumnId(0, SortType.Descending);
			foreach (CacheLog log in logs)
			{
				m_logList.AppendValues(log);
			}
			
		}
		
		private void BuildLogTreeWidget()
		{	
			
			m_logList = new ListStore (typeof(CacheLog));
			
			CellRendererText code_cell = new CellRendererText ();
			CellRendererText name_cell = new CellRendererText ();
			CellRendererText date_cell = new CellRendererText ();
			CellRendererPixbuf type_cell = new CellRendererPixbuf ();
			TreeViewColumn log_icon = new TreeViewColumn (Catalog.GetString ("Type"), type_cell);
			log_icon.SortColumnId = 3;
			TreeViewColumn cache_code = new TreeViewColumn (Catalog.GetString ("Code"), code_cell);
			cache_code.SortColumnId = 1;
			TreeViewColumn cache_name = new TreeViewColumn (Catalog.GetString ("Name"), name_cell);
			cache_name.SortColumnId = 2;
			TreeViewColumn log_date = new TreeViewColumn (Catalog.GetString ("Date"), date_cell);
			log_date.SortColumnId = 0;
			
			cache_code.SetCellDataFunc (code_cell, new TreeCellDataFunc(RenderCode));
			log_date.SetCellDataFunc (date_cell, new TreeCellDataFunc(RenderDate));
			log_icon.SetCellDataFunc(type_cell, new TreeCellDataFunc(RenderType));
			cache_name.SetCellDataFunc (name_cell, new TreeCellDataFunc(RenderName));	
			
			logView.AppendColumn (log_icon);
			logView.AppendColumn (cache_code);
			logView.AppendColumn (log_date);
			logView.AppendColumn (cache_name);
			
			listSort = new TreeModelSort(m_logList);
			
			listSort.SetSortFunc (1, TypeCompare);
			listSort.SetSortFunc (0, DateCompare);
			logView.Model = listSort;
			logView.Selection.Changed += HandleLogViewSelectionChanged;
			
		}

		void HandleLogViewSelectionChanged (object sender, EventArgs e)
		{
			TreeIter iter;
			TreeModel model;			
			if (((TreeSelection)sender).GetSelected (out model, out iter)) {
				CacheLog val = (CacheLog)model.GetValue (iter, 0);
				logEntry.Buffer.Text = val.LogMessage;
				editButton.Sensitive = true;
				deleteButton.Sensitive = true;
			} else {
				editButton.Sensitive = false;
				deleteButton.Sensitive = false;
			}
		}
		
		private void RenderType (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			CacheLog log = (CacheLog)model.GetValue (iter, 0);
			CellRendererPixbuf icon = cell as CellRendererPixbuf;
			if (log.LogStatus == "Found it")
				icon.Pixbuf = IconManager.FOUNDICON_S;
			else if (log.LogStatus == "Didn't find it")
				icon.Pixbuf = IconManager.DNF_S;
			else if (log.LogStatus == "Needs Maintenance")
				icon.Pixbuf = IconManager.NEEDS_MAINT_S;
			else
				icon.Pixbuf = IconManager.WRITENOTE_S;
		}
		
		private void RenderCode (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			CacheLog log = (CacheLog)model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			text.Text = log.CacheCode;
		}
		
		private void RenderName (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			CacheLog log = (CacheLog)model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			
			// TEMP: INEFFICIENT, SHOULD GET ALL POSSIBLE CACHES FIRST
			Geocache cache = Engine.getInstance().Store.GetCache(log.CacheCode);
			if (cache != null)
				text.Text = cache.CacheName;
			else
				text.Text = Catalog.GetString("<Name Unavailable>");
		}
		
		
		private void RenderDate (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			CacheLog log = (CacheLog)model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			text.Text = log.LogDate.ToShortDateString() + " " + log.LogDate.ToShortTimeString();
		}
		
		private int DateCompare (TreeModel model, TreeIter tia, TreeIter tib)
		{
			CacheLog logA = (CacheLog)model.GetValue (tia, 0);
			CacheLog logB = (CacheLog)model.GetValue (tib, 0);
			if (logA == null || logB == null)
				return 0;
			if (logA.LogDate > logB.LogDate)
				return 1;
			else if (logA.LogDate == logB.LogDate)
				return 0;
			else
				return -1;
		}
		
		private int TypeCompare (TreeModel model, TreeIter tia, TreeIter tib)
		{
			CacheLog logA = (CacheLog)model.GetValue (tia, 0);
			CacheLog logB = (CacheLog)model.GetValue (tib, 0);
			if (logA == null || logB == null)
				return 0;
			return logA.LogStatus.CompareTo(logB.LogStatus);
		}
		
		protected virtual void OnCloseClick (object sender, System.EventArgs e)
		{
			this.Hide();
			this.Dispose();
		}	
		protected virtual void OnEditClick (object sender, System.EventArgs e)
		{
			Gtk.TreeIter itr;
			Gtk.TreeModel model;
			if (logView.Selection.GetSelected (out model, out itr)) 
			{
				CacheLog log = (CacheLog)model.GetValue (itr, 0);
				OfflineLogDialog dlg = new OfflineLogDialog();
				dlg.EditMessageOnly = true;
				dlg.Log = log;
				if ((int)ResponseType.Ok == dlg.Run())
				{
					log.LogMessage = dlg.Log.LogMessage;
					Engine.getInstance().Store.AddLogAtomic(log.CacheCode, log);
					logEntry.Buffer.Text = log.LogMessage;
					UpdateFNFile();
				}
				dlg.Hide();
				dlg.Dispose();
			}
		}
		
		protected virtual void OnLogOnlineClick (object sender, System.EventArgs e)
		{
		}
		
		protected virtual void OnDeleteClick (object sender, System.EventArgs e)
		{
			Gtk.TreeIter itr;
			Gtk.TreeModel model;
			if (logView.Selection.GetSelected (out model, out itr)) 
			{
				CacheLog log = (CacheLog)model.GetValue (itr, 0);
				MessageDialog dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Warning, ButtonsType.YesNo,
				                                      Catalog.GetString("Are you sure you want to remove the log for '{0}'?"),
				                                      log.CacheCode);
				m_Logs.Remove(log);
				PopulateLogs(m_Logs);
				UpdateFNFile();
				logEntry.Buffer.Text = String.Empty;
				dlg.Hide();
				dlg.Dispose();
			}
		}
		
		private void UpdateFNFile()
		{
			string fnFile = UIMonitor.getInstance().Configuration.FieldNotesFile;
			FieldNotesHandler.ClearFieldNotes(fnFile);
			FieldNotesHandler.WriteToFile(m_Logs, fnFile);
		}	
	}
}
