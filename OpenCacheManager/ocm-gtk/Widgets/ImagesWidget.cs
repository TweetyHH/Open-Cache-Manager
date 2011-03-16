// 
//  Copyright 2011  campbelk
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
using System.IO;
using Gtk;
using Gdk;

namespace ocmgtk
{


	[System.ComponentModel.ToolboxItem(true)]
	public partial class ImagesWidget : Gtk.Bin
	{
		ListStore m_model;
		UIMonitor m_monitor;

		public ImagesWidget ()
		{
			this.Build ();
			m_monitor = UIMonitor.getInstance();
		}
		
		public void UpdateCaceheInfo()
		{
			m_model = new ListStore(typeof(Pixbuf), typeof(string), typeof(string));
			if (m_monitor.SelectedCache == null)
				return;
			if(Directory.Exists(m_monitor.Configuration.DataDirectory + "/images/" + m_monitor.SelectedCache.Name))
			{
				string[] files = Directory.GetFiles(m_monitor.Configuration.DataDirectory + "/images/" + m_monitor.SelectedCache.Name);
				foreach(string file in files)
				{
					Pixbuf buf = new Pixbuf(file,256, 256);
					string[] filePath = file.Split('/');
					m_model.AppendValues(buf, filePath[filePath.Length -1],file);
				}
			}
			imagesView.Model = m_model;
			imagesView.PixbufColumn = 0;
			imagesView.TextColumn = 1;
			imagesView.SelectionMode = SelectionMode.Single;
		}
		
		protected virtual void OnViewClick (object sender, System.EventArgs e)
		{
			TreeIter iter;
			if (imagesView.SelectedItems[0] != null) {
				m_model.GetIter(out iter, imagesView.SelectedItems[0]);
				string  file = (string)m_model.GetValue (iter, 2);
				ImageDialog dlg = new ImageDialog(file);
				dlg.Run();
			}
		}
		
		
	}
}
