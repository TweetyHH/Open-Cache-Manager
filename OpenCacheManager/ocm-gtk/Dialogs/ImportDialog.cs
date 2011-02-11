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

namespace ocmgtk
{


	public partial class ImportDialog : Gtk.Dialog
	{

		public ImportDialog ()
		{
			this.Build ();
			FileFilter filter = new FileFilter ();
			filter.Name = "Waypoint Files";
			filter.AddPattern ("*.gpx");
			filter.AddPattern ("*.loc");
			filter.AddPattern ("*.zip");
			fileWidget.AddFilter (filter);
		}
		
		public string Filename
		{
			get { return fileWidget.Filename;}
		}
				
		
		public bool PreventStatusOverwrite
		{
			get { return statusCheck.Active;}
			set { statusCheck.Active = value;}
		}
		
		public bool PurgeOldLogs
		{
			get { return oldLogsCheck.Active;}
			set { oldLogsCheck.Active = value;}
		}
		
		public bool IgnoreExtraFields
		{
			get { return gsakFieldsCheck.Active;}
			set { gsakFieldsCheck.Active = value;}
		}
		
		
		public void SetCurrentFolder(string folder)
		{
			fileWidget.SetCurrentFolder(folder);
		}
		
		protected virtual void OnOkClicked (object sender, System.EventArgs e)
		{
			this.Hide();
		}
		
		protected virtual void OnCancelClicked (object sender, System.EventArgs e)
		{
			this.Hide();
		}
		
		protected virtual void OnFileActivated (object sender, System.EventArgs e)
		{
			this.Respond(ResponseType.Accept);
			this.Hide();
		}
		
		
	}
}