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
using Gtk;
using Mono.Unix;
using ocmengine;

namespace ocmgtk
{


	public partial class ExportPOIDialog : Gtk.Dialog
	{
		
		public string FileName
		{
			get { return fileEntry.Text;}
			set { fileEntry.Text = value;}
		}
		
		public WaypointNameMode NameMode
		{
			get 
			{ 
				switch (nameCombo.Active)
				{
					case 0:
						return WaypointNameMode.CODE;
					default:
						return WaypointNameMode.NAME;
				}
			}
			set {
				if (value == WaypointNameMode.CODE)
					nameCombo.Active = 0;
				else
					nameCombo.Active = 1;
			}
		}
		
		public WaypointDescMode DescMode
		{
			
			get
			{
				switch(descCombo.Active)
				{
				case 0:
					return WaypointDescMode.DESC;
				case 1:
					return WaypointDescMode.CODESIZEANDHINT;
				case 2:
					return WaypointDescMode.CODESIZETYPE;
				default:
					return WaypointDescMode.FULL;
				}
			}
			set
			{
				if (value == WaypointDescMode.DESC)
					descCombo.Active = 0;
				else if (value == WaypointDescMode.CODESIZEANDHINT)
					descCombo.Active = 1;
				else if (value == WaypointDescMode.CODESIZETYPE)
					descCombo.Active = 2;
				else
					descCombo.Active = 3;
			}
		}
		
		public int CacheLimit
		{
			get
			{
				if (limitCaches.Active)
					return int.Parse(limitEntry.Text);
				else 
					return -1;
			}
			set
			{
				if (value > 0)
				{
					limitCaches.Active = true;
					limitEntry.Text = value.ToString();
				}
			}
		}
		
		public bool IncludeChildren
		{
			get
			{
				return includeChildrenCheck.Active;
			}
			set
			{
				includeChildrenCheck.Active = value;
			}
		}
		
		public string Category
		{
			get
			{
				return catagoryEntry.Text;
			}
			set
			{
				catagoryEntry.Text = value;
			}
		}
		
		public string BMPFile
		{
			get
			{
				if (includeBMPCheck.Active)
				{
					return bmpFile.Text;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					includeBMPCheck.Active = true;
					bmpFile.Text = value;
				}
			}
		}		
		
		
		protected virtual void OnFileClick (object sender, System.EventArgs e)
		{
			FileChooserDialog dlg = new FileChooserDialog (Catalog.GetString ("Select GPI location"), null, FileChooserAction.Save, Catalog.GetString ("Cancel"), ResponseType.Cancel, Catalog.GetString ("Select"), ResponseType.Accept);
			dlg.SetCurrentFolder (System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments));
			dlg.CurrentName = "geocaches.gpi";
			FileFilter filter = new FileFilter ();
			filter.Name = "Garmin POI Database";
			filter.AddPattern ("*.gpi");			
			dlg.AddFilter (filter);			
			if (dlg.Run () == (int)ResponseType.Accept) {
				fileEntry.Text = dlg.Filename;
			}
			dlg.Destroy ();
		}
		
		
		public ExportPOIDialog ()
		{
			this.Build ();
			this.fileEntry.Text = "/home/campbelk/test.gpi";
		}
		protected virtual void OnOKClick (object sender, System.EventArgs e)
		{
			this.Hide();
		}
		
		protected virtual void OnCancelClick (object sender, System.EventArgs e)
		{
			this.Hide();
		}
		
		protected virtual void OnBMPClick (object sender, System.EventArgs e)
		{
			FileChooserDialog dlg = new FileChooserDialog (Catalog.GetString ("Select BMP File"), null, FileChooserAction.Save, Catalog.GetString ("Cancel"), ResponseType.Cancel, Catalog.GetString ("Select"), ResponseType.Accept);
			dlg.SetCurrentFolder (System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments));
			FileFilter filter = new FileFilter ();
			filter.Name = "BMP Files";
			filter.AddPattern ("*.bmp");			
			dlg.AddFilter (filter);			
			if (dlg.Run () == (int)ResponseType.Accept) {
				bmpFile.Text = dlg.Filename;
			}
			dlg.Destroy ();
		}
		
		
		
		
	}
}
