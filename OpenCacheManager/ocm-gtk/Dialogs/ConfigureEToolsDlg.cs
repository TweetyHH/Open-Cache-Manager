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
using Gtk;
using Mono.Unix;

namespace ocmgtk
{


	public partial class ConfigureEToolsDlg : Gtk.Dialog
	{
		EToolList m_toolList;
		ListStore m_treemodel;
		TreeModelSort m_sortmodel;

		public ConfigureEToolsDlg ()
		{
			this.Build ();
			m_toolList = EToolList.LoadEToolList();
			BuildTree();
		}
		
		private void BuildTree()
		{
			CellRendererText cmdname_cell = new CellRendererText ();
			CellRendererText cmdscript_cell = new CellRendererText ();
			TreeViewColumn cmdname = new TreeViewColumn (Catalog.GetString ("Name"), cmdname_cell);
			TreeViewColumn cmdscript = new TreeViewColumn (Catalog.GetString ("Command"), cmdscript_cell);
			commandView.AppendColumn(cmdname);
			commandView.AppendColumn(cmdscript);
			m_treemodel = new ListStore(typeof (ExternalTool));
			m_sortmodel = new TreeModelSort(m_treemodel);
			commandView.Model = m_sortmodel;
		}
		
	}
}
