// 
//  Copyright 2011  tweety
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
using Mono.Unix;

namespace ocmgtk
{


	[System.ComponentModel.ToolboxItem(true)]
	public partial class MapSelectionWidget : Gtk.Bin
	{
		private List<MapDescription> m_maps;
		private ListStore m_mapModel;
		private UIMonitor m_mon;

		public MapSelectionWidget ()
		{
			this.Build ();
			m_mon = UIMonitor.getInstance ();
			
			m_mapModel = new ListStore (typeof(MapDescription));
			MapManager mapManager = new MapManager ("./maps");
			m_maps = mapManager.getMaps ("./maps/google-sat.xml");
			foreach (MapDescription md in m_maps) {
				m_mapModel.AppendValues (md);
			}
			FillList ();
			
			
			
		}

		private void FillList ()
		{
			//mapView = new Gtk.TreeView();
			
			
//			CellRendererPixbuf activeCell = new CellRendererPixbuf ();
			CellRendererText nameCell = new CellRendererText ();
			CellRendererText layerCell = new CellRendererText ();
			CellRendererText coveredCell = new CellRendererText ();
			
//			TreeViewColumn activeIconColum = new TreeViewColumn (Catalog.GetString ("Active"), activeCell);
			TreeViewColumn nameColumn = new TreeViewColumn (Catalog.GetString ("Name"), nameCell);
			TreeViewColumn layerColumn = new TreeViewColumn (Catalog.GetString ("Layer"), layerCell);
			TreeViewColumn coveredColumn = new TreeViewColumn (Catalog.GetString ("Covered"), coveredCell);
			
//			mapView.AppendColumn (activeIconColum);
			mapView.AppendColumn (nameColumn);
			mapView.AppendColumn (layerColumn);
			mapView.AppendColumn (coveredColumn);
			
//			activeIconColum.SetCellDataFunc (activeCell, new TreeCellDataFunc (RenderCacheIcon));
//			activeIconColum.SortColumnId = 0;
			nameColumn.SetCellDataFunc (nameCell, new TreeCellDataFunc (RenderMapName));
			nameColumn.SortColumnId = 0;
			layerColumn.SetCellDataFunc (layerCell, new TreeCellDataFunc (RenderMapLayer));
			layerColumn.SortColumnId = 1;
			coveredColumn.SetCellDataFunc (coveredCell, new TreeCellDataFunc (RenderMapCovered));
			coveredColumn.SortColumnId = 2;
			
			mapView.Model = m_mapModel;
		}


		private void RenderMapName (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			MapDescription map = (MapDescription)model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			text.Text = map.Name + " " + map.Active;
			
			if (map.Active) {
				text.Strikethrough = false;
			} else {
				text.Strikethrough = true;
			}
		}

		private void RenderMapLayer (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			MapDescription map = (MapDescription)model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			text.Text = "" + map.Layer;
		}

		private void RenderMapCovered (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			MapDescription map = (MapDescription)model.GetValue (iter, 0);
			CellRendererText text = cell as CellRendererText;
			text.Text = map.Covered;
		}

		private void UpdateMaps() {
		}




		protected virtual void OnActivateButtonClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeIter itr;
			Gtk.TreeModel model;
			if (mapView.Selection.GetSelected (out model, out itr)) {
				MapDescription map = (MapDescription)model.GetValue (itr, 0);
				map.Active = true;
				UpdateMaps();
			}
		}


		protected virtual void OnDeactivateButtonClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeIter itr;
			Gtk.TreeModel model;
			if (mapView.Selection.GetSelected (out model, out itr)) {
				MapDescription map = (MapDescription)model.GetValue (itr, 0);
				map.Active = false;
				UpdateMaps();
			}
		}

		protected virtual void OnOpenButtonClicked (object sender, System.EventArgs e)
		{
		}
		
				
		
	}
}
