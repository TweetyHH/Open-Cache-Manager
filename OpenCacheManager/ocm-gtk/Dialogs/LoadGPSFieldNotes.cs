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
using Mono.Unix;

namespace ocmgtk
{


	public partial class LoadGPSFieldNotes : Gtk.Dialog
	{

		public LoadGPSFieldNotes ()
		{
			this.Build ();
			ignoreDate.IncludeTime = true;
		}
		
		public DateTime LastScanTD
		{
			set
			{
				if (value == DateTime.MinValue)
					lastScanTD.Text = Catalog.GetString("Never");
				else
					lastScanTD.Text = value.ToLongDateString();
			}
		}
		
		public DateTime LastScan
		{
			set
			{
				if (value == DateTime.MinValue)
				{
					checkIgnoreLogs.Active = false;
					ignoreDate.Date = value;
					lastScan.Text = Catalog.GetString("Never");
				}
				else
				{
					checkIgnoreLogs.Active = true;
					ignoreDate.Date = value;
					lastScan.Text = value.ToLongDateString();
				}
			}
			get
			{
				if (checkIgnoreLogs.Active)
					return ignoreDate.Date;
				else
					return DateTime.MinValue;
			}
		}
	}
}
