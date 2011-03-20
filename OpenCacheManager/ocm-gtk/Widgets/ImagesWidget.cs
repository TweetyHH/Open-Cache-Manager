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
using System.Collections.Generic;
using System.Net;
using Mono.Unix;
using ocmengine;
using Gtk;
using Gdk;
using System.Text.RegularExpressions;

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
			{
				this.Sensitive = false;
				return;
			}
			this.Sensitive = true;
			string imagesFolder = GetImagesFolder ();
			fileLabel.Text = String.Format(Catalog.GetString("Images Folder: {0}"), imagesFolder);
			if(Directory.Exists(imagesFolder))
			{
				string[] files = Directory.GetFiles(imagesFolder);
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
		
		private string GetImagesFolder()
		{
			string dbName = GetDBName ();
			return m_monitor.Configuration.DataDirectory + "/ocm_images/" + dbName + "/" +  m_monitor.SelectedCache.Name;
		}
		
		
		private static string GetDBName ()
		{
			string dbFile = Engine.getInstance().Store.DBFile;
			return Utilities.GetFileShortName(dbFile);
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
		
		protected virtual void OnOpenFolderClick (object sender, System.EventArgs e)
		{
			string imagesFolder = GetImagesFolder();
			if (!Directory.Exists(imagesFolder))
				Directory.CreateDirectory(imagesFolder);
			System.Diagnostics.Process.Start(imagesFolder);
		}
		
		protected virtual void OnGrabImagesClick (object sender, System.EventArgs e)
		{
			const string IMG = "(<[Ii][Mm][Gg])([^sS][^rR]*)([Ss][Rr][Cc]\\s?=\\s?)\"([^\"]*)\"([^>]*>)";
			MatchCollection matches = Regex.Matches(m_monitor.SelectedCache.LongDesc, IMG);
			if (matches.Count == 0)
			{
				MessageDialog mdlg = new MessageDialog(null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, Catalog.GetString("No Images Found."));
				mdlg.Run();
				mdlg.Hide();
				return;
			}
			
			List<string> files = new List<string>();
			string imagesFolder = GetImagesFolder();
			if (!Directory.Exists(imagesFolder))
				Directory.CreateDirectory(imagesFolder);
			foreach(Match match in matches)
			{
				files.Add(match.Groups[4].Value);
			}
		
			FileDownloadProgress dlg = new FileDownloadProgress();
			dlg.Start(files, imagesFolder);
			UpdateCaceheInfo();
		}
		protected virtual void OnDoubleClick (object o, Gtk.ItemActivatedArgs args)
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
