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

namespace ocmgtk
{


	public partial class ImportDirectoryDialog : Gtk.Dialog
	{

		public ImportDirectoryDialog ()
		{
			this.Build ();
		}
		
		public string Directory
		{
			get { return dirChooser.Filename;}
			set { 
				System.Console.WriteLine(value);
				dirChooser.SetCurrentFolder(value);
			}
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
		
		public bool DeleteOnCompletion
		{
			set { deleteCheck.Active = value;}
			get { return deleteCheck.Active;}
		}
	}
}
