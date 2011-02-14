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
using System.Collections.Generic;
using System.Text;
using ocmgtk;
using ocmengine;
using Mono.Unix;
using Gtk;

namespace ocmgtk
{


	public partial class AddComboFilter : Gtk.Dialog
	{
		public List<FilterList> ComboFilter
		{
			get 
			{ 
				List<FilterList> list = new List<FilterList>();
				TreeIter itr;
				m_Store.GetIterFirst (out itr);
				if (!m_Store.IterIsValid (itr))
					return list;
				do 
					list.Add ((FilterList)m_Store.GetValue (itr, 0));
				while (m_Store.IterNext (ref itr));
				return list;
			}
			set 
			{
				m_Store.Clear();
				if (value == null)
					return;
				foreach(FilterList list in value)
				{
					m_Store.AppendValues(list);
				}
			}
		}
		
		ListStore m_Store;
		public AddComboFilter ()
		{
			this.Build ();
			PrepareView();
		}
		
		
		private void PrepareView()
		{
			CellRendererText conditionCell = new CellRendererText();
			conditionCell.WrapMode = Pango.WrapMode.Word;
			conditionCell.WrapWidth = 500;
			TreeViewColumn conditionColumn = new TreeViewColumn(Catalog.GetString("Conditions"),
			                                                    conditionCell);
			conditionColumn.SetCellDataFunc(conditionCell, RenderCondition);
			conditionList.AppendColumn(conditionColumn);
			conditionList.EnableGridLines = TreeViewGridLines.Horizontal;
			m_Store = new ListStore(typeof(FilterList));
			conditionList.Model = m_Store;
		}
		
		private void RenderCondition(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			FilterList list = (FilterList) model.GetValue (iter, 0);
			CellRendererText condition_cell = cell as CellRendererText;
			
			StringBuilder builder = new StringBuilder();
			builder.Append("<u>Filter Condition</u>\n");
			RenderCacheTypes (list, builder);
			RenderCacheDiff (list, builder);
			RenderCacheTerr (list, builder);
			RenderContainer (list, builder);
			RenderDescription (list, builder);
			RenderPlacedBy (list, builder);
			RenderPlacedBefore (list, builder);
			RenderPlacedAfter (list, builder);
			RenderInfoBefore (list, builder);
			RenderInfoAfter (list, builder);
			RenderFoundAfter (list, builder);
			RenderFoundBefore (list, builder);
			RenderFoundOn (list, builder);
			RenderCountry (list, builder);
			RenderState (list, builder);
			RenderNotes (list, builder);
			RenderChildren (list, builder);
			RenderNoChildren (list, builder);
			RenderCorrected (list, builder);
			RenderNoCorrected (list, builder);
			RenderMustHaveAttributes (list, builder);
			RenderMustNotHaveAttributes (list, builder);
			RenderFTF (list, builder);
			RenderDNF (list, builder);
			RenderStatus (list, builder);
			if (list.Contains(FilterList.KEY_DIST))
			{
				builder.Append(Catalog.GetString("<b>Distance: </b>"));
				string op = list.GetCriteria(FilterList.KEY_DIST_OP) as string;
				if (op == "<=")
					builder.Append("Less than ");
				else if (op == ">=")
					builder.Append("Greater than ");
				else
					builder.Append("Equal to ");
				builder.Append(((double)list.GetCriteria(FilterList.KEY_DIST)).ToString());
				if (UIMonitor.getInstance().Configuration.ImperialUnits)
					builder.Append(Catalog.GetString(" mi"));
				else
					builder.Append(Catalog.GetString(" km"));
				
				if (list.Contains(FilterList.KEY_DIST_LAT))
				{
					double lat = (double) list.GetCriteria(FilterList.KEY_DIST_LAT);
					double lon = (double) list.GetCriteria(FilterList.KEY_DIST_LON);
					builder.Append(" From: ");
					builder.Append(Utilities.getCoordString(lat, lon));
				}
				builder.Append("\n");
			}
			System.Console.WriteLine(builder.ToString());
			condition_cell.Markup = builder.ToString();			
		}
		
		private static void RenderStatus (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_STATUS))
			{
				builder.Append(Catalog.GetString("<b>Status: </b>"));
				bool[] status = (bool[]) list.GetCriteria(FilterList.KEY_STATUS);
				if (status[0])
					builder.Append(Catalog.GetString("Found "));
				if (status[1])
					builder.Append(Catalog.GetString("Unfound "));
				if (status[2])
					builder.Append(Catalog.GetString("Mine "));
				if (status[3])
					builder.Append(Catalog.GetString("Available "));
				if (status[4])
					builder.Append(Catalog.GetString("Disabled "));
				if (status[5])
					builder.Append(Catalog.GetString("Archived "));
				builder.Append("\n");
			}
		}
		
		private static void RenderDNF (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_DNF))
			{
				builder.Append("<b>Didn't find it: </b>");
				builder.Append(((bool) list.GetCriteria(FilterList.KEY_DNF)).ToString());
				builder.Append("\n");
			}
		}
		
		private static void RenderFTF (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_FTF))
			{
				builder.Append("<b>First to find: </b>");
				builder.Append(((bool) list.GetCriteria(FilterList.KEY_FTF)).ToString());
				builder.Append("\n");
			}
		}
		
		private static void RenderMustNotHaveAttributes (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_INCNOATTRS) || list.Contains(FilterList.KEY_EXCNOATTRS))
			{
				builder.Append("<b>Must not have the following attributes: </b>");
				bool isFirst = false;
				if (list.Contains(FilterList.KEY_INCNOATTRS))
				{
					List<string> str = (List<String>) list.GetCriteria(FilterList.KEY_INCNOATTRS);
					foreach(String attr in str)
					{
						if (!isFirst)
							isFirst = true;
						else
							builder.Append(",");
						builder.Append(attr);
						builder.Append(" ");
					}
				}
				if (list.Contains(FilterList.KEY_EXCNOATTRS))
				{
					List<string> str = (List<String>) list.GetCriteria(FilterList.KEY_EXCNOATTRS);
					foreach(String attr in str)
					{
						if (!isFirst)
							isFirst = true;
						else
							builder.Append(",");
						builder.Append("<span fgcolor='red' strikethrough='true'>" + attr + "</span> ");
					}
				}
				builder.Append("\n");
			}
		}
		
		private static void RenderMustHaveAttributes (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_INCATTRS) || list.Contains(FilterList.KEY_EXCATTRS))
			{
				builder.Append("<b>Must have the following attributes: </b>");
				bool isFirst = false;
				if (list.Contains(FilterList.KEY_INCATTRS))
				{
					List<string> str = (List<String>) list.GetCriteria(FilterList.KEY_INCATTRS);
					foreach(String attr in str)
					{
						if (!isFirst)
							isFirst = true;
						else
							builder.Append(",");
						builder.Append(attr);
						builder.Append(" ");
					}
				}
				if (list.Contains(FilterList.KEY_EXCATTRS))
				{
					List<string> str = (List<String>) list.GetCriteria(FilterList.KEY_EXCATTRS);
					foreach(String attr in str)
					{
						if (!isFirst)
							isFirst = true;
						else
							builder.Append(",");
						builder.Append("<span fgcolor='red' strikethrough='true'>" + attr + "</span> ");
					}
				}
				builder.Append("\n");
			}
		}
		
		private static void RenderNoCorrected (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_NOCORRECTED))
			{
				builder.Append("<b>Does not have corrected coordinates</b>");
				builder.Append("\n");
			}
		}
		
		private static void RenderCorrected (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_CORRECTED))
			{
				builder.Append("<b>Has corrected coordinates</b>");
				builder.Append("\n");
			}
		}
		
		private static void RenderNoChildren (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_NOCHILDREN))
			{
				builder.Append("<b>Does not have any children of type: </b>");
				builder.Append(Catalog.GetString(list.GetCriteria(FilterList.KEY_NOCHILDREN) as string));
				builder.Append("\n");				
			}
		}
		
		private static void RenderChildren (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_CHILDREN))
			{
				builder.Append("<b>Has at least one child of type: </b>");
				builder.Append(Catalog.GetString(list.GetCriteria(FilterList.KEY_CHILDREN) as string));
				builder.Append("\n");				
			}
		}
		
		private static void RenderNotes (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_NOTES))
			{
				builder.Append("<b>Has user notes</b>");
				builder.Append("\n");
			}
		}
		
		private static void RenderState (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_STATE))
			{
				builder.Append(Catalog.GetString("<b>State/Province: </b>"));
				builder.Append(list.GetCriteria(FilterList.KEY_STATE) as string);
				builder.Append("\n");
			}
		}
		
		private static void RenderCountry (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_COUNTRY))
			{
				builder.Append(Catalog.GetString("<b>Country: </b>"));
				builder.Append(list.GetCriteria(FilterList.KEY_COUNTRY) as string);
				builder.Append("\n");
			}
		}
		
		private static void RenderFoundOn (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_FOUNDON))
			{
				builder.Append(Catalog.GetString("<b>Last found by you on: </b>"));
				builder.Append(((DateTime) list.GetCriteria(FilterList.KEY_FOUNDON)).ToLongDateString());
				builder.Append("\n");
			}
		}
		
		private static void RenderFoundBefore (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_FOUNDBEFORE))
			{
				builder.Append(Catalog.GetString("<b>Last found by you on or before: </b>"));
				builder.Append(((DateTime) list.GetCriteria(FilterList.KEY_FOUNDBEFORE)).ToLongDateString());
				builder.Append("\n");
			}
		}
		
		private static void RenderFoundAfter (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_FOUNDAFTER))
			{
				builder.Append(Catalog.GetString("<b>Last found by you on or after: </b>"));
				builder.Append(((DateTime) list.GetCriteria(FilterList.KEY_FOUNDAFTER)).ToLongDateString());
				builder.Append("\n");
			}
		}
		
		private static void RenderInfoAfter (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_INFOAFTER))
			{
				builder.Append(Catalog.GetString("<b>Last updated on or after: </b>"));
				builder.Append(((DateTime) list.GetCriteria(FilterList.KEY_INFOAFTER)).ToLongDateString());
				builder.Append("\n");
			}
		}
		
		private static void RenderInfoBefore (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_INFOBEFORE))
			{
				builder.Append(Catalog.GetString("<b>Last updated on or before: </b>"));
				builder.Append(((DateTime) list.GetCriteria(FilterList.KEY_INFOBEFORE)).ToLongDateString());
				builder.Append("\n");
			}
		}
		
		private static void RenderPlacedAfter (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_PLACEAFTER))
			{
				builder.Append(Catalog.GetString("<b>Placed on or after: </b>"));
				builder.Append(((DateTime) list.GetCriteria(FilterList.KEY_PLACEAFTER)).ToLongDateString());
				builder.Append("\n");
			}
		}
		
		private static void RenderPlacedBefore (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_PLACEBEFORE))
			{
				builder.Append(Catalog.GetString("<b>Placed on or before: </b>"));
				builder.Append(((DateTime) list.GetCriteria(FilterList.KEY_PLACEBEFORE)).ToLongDateString());
				builder.Append("\n");
			}
		}
		
		private static void RenderDescription (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_DESCRIPTION))
			{
				builder.Append(Catalog.GetString("Description Contains:"));
				builder.Append(list.GetCriteria(FilterList.KEY_DESCRIPTION));
				builder.Append("\n");
			}
		}
		
		private static void RenderPlacedBy (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_PLACEDBY))
			{
				builder.Append(Catalog.GetString("<b>Placed By: </b>"));
				builder.Append(list.GetCriteria(FilterList.KEY_PLACEDBY));
				builder.Append("\n");
			}
		}
		
		private static void RenderContainer (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_CONTAINER))
			{
				builder.Append(Catalog.GetString("<b>Container: </b>"));
				bool isFirst = false;
				foreach(string container in (List<String>) list.GetCriteria(FilterList.KEY_CONTAINER))
				{
					if (!isFirst)
						isFirst = true;
					else
						builder.Append(", ");
					builder.Append(container);
				}
				builder.Append("\n");
			}
		}
		
		private static void RenderCacheTerr (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_TERRAIN_VAL))
			{
				builder.Append(Catalog.GetString("<b>Terrain: </b>"));
				string op = list.GetCriteria(FilterList.KEY_TERRAIN_OP) as string;
				if (op == "==")
					builder.Append(Catalog.GetString("Equal to"));
				else if (op == ">")
					builder.Append(Catalog.GetString("Greater Than"));
				else if (op == ">=")
					builder.Append(Catalog.GetString("Greater Than or Equal To"));
				else if (op == "<=")
					builder.Append(Catalog.GetString("Less Than or Equal To"));
				else if (op == "<")
					builder.Append(Catalog.GetString("Less Than"));
				builder.Append(" ");				
				builder.Append(list.GetCriteria(FilterList.KEY_TERRAIN_VAL));
				builder.Append("\n");
			}
		}
		
		private static void RenderCacheDiff (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_DIFF_VAL))
			{
				builder.Append(Catalog.GetString("<b>Difficulty: </b>"));
				string op = list.GetCriteria(FilterList.KEY_DIFF_OP) as String;
				if (op == "==")
					builder.Append(Catalog.GetString("Equal to"));
				else if (op == ">")
					builder.Append(Catalog.GetString("Greater Than"));
				else if (op == ">=")
					builder.Append(Catalog.GetString("Greater Than or Equal To"));
				else if (op == "<=")
					builder.Append(Catalog.GetString("Less Than or Equal To"));
				else if (op == "<")
					builder.Append(Catalog.GetString("Less Than"));
				builder.Append(" ");				
				builder.Append(list.GetCriteria(FilterList.KEY_DIFF_VAL));
				builder.Append("\n");
			}
		}
		
		private static void RenderCacheTypes (FilterList list, StringBuilder builder)
		{
			if (list.Contains(FilterList.KEY_CACHETYPE))
			{
				
				builder.Append(Catalog.GetString("<b>Cache Types: </b>"));
				bool isFirst = false;
				foreach(string type in (List<String>) list.GetCriteria(FilterList.KEY_CACHETYPE))
				{
					if (!isFirst)
						isFirst = true;
					else
						builder.Append(", ");
					builder.Append(Catalog.GetString(Geocache.GetCTypeString((Geocache.CacheType)Enum.Parse(typeof(Geocache.CacheType),type))));
				}
				builder.Append("\n");
			}
		}
		
		protected virtual void OnAddClick (object sender, System.EventArgs e)
		{
			FilterDialog dlg = new FilterDialog();
			dlg.Filter = new FilterList();
			dlg.Title = Catalog.GetString("Add Condition...");
			if ((int) ResponseType.Ok == dlg.Run())
			{
				m_Store.AppendValues(dlg.Filter);
				this.QueueDraw();
			}			
			dlg.Hide();
			dlg.Dispose();
		}
		
		protected virtual void OnEditClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeIter itr;
			Gtk.TreeModel model;
			if (conditionList.Selection.GetSelected (out model, out itr)) 
			{
				FilterList condition = (FilterList)model.GetValue (itr, 0);
				FilterDialog dlg = new FilterDialog();
				dlg.Title = Catalog.GetString("Edit Condition...");
				dlg.Filter = condition;
				if ((int) ResponseType.Ok == dlg.Run())
				{
					m_Store.SetValue(itr, 0, dlg.Filter);
				}		
			}
		}
		
		protected virtual void OnOkClicked (object sender, System.EventArgs e)
		{
			this.Hide();
		}
		
		protected virtual void OnCancelClicked (object sender, System.EventArgs e)
		{
			this.Hide();
		}
		
		protected virtual void OnRemoveClick (object sender, System.EventArgs e)
		{
			MessageDialog dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo,
			                                      Catalog.GetString("Are you sure you want to remove this filter?"));
			if ((int) ResponseType.No == dlg.Run())
			{
				dlg.Hide();
				dlg.Dispose();
				return;
			}
			
			dlg.Hide();
			dlg.Dispose();
			Gtk.TreeIter itr;
			Gtk.TreeModel model;
			if (conditionList.Selection.GetSelected (out model, out itr)) 
			{
				m_Store.Remove(ref itr);
			}
		}
	}
}
