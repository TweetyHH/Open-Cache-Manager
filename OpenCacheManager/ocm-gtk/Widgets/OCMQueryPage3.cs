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

namespace ocmgtk
{


	[System.ComponentModel.ToolboxItem(true)]
	public partial class OCMQueryPage3 : Gtk.Bin
	{
		
		public DateTime PlaceBefore
		{
			get
			{
				if (placeBeforeCheck.Active)
					return placeBeforeEntry.Date;
				return DateTime.MinValue;				
			}
			set
			{
				if (value == DateTime.MinValue)
					return;
				placeBeforeCheck.Active = true;
				placeBeforeEntry.Date = value;
			}
		}
		
		public DateTime PlaceAfter
		{
			get
			{
				if (placeAfterCheck.Active)
					return placeAfterEntry.Date;
				return DateTime.MinValue;			
			}
			set
			{
				if (value == DateTime.MinValue)
					return;
				placeAfterCheck.Active = true;
				placeAfterEntry.Date = value;
			}
		}
		
		public DateTime InfoAfter
		{
			get
			{
				if (infoAfterCheck.Active)
					return infoAfterEntry1.Date;
				return DateTime.MinValue;				
			}
			set
			{
				if (value == DateTime.MinValue)
					return;
				infoAfterCheck.Active = true;
				infoAfterEntry1.Date = value;
			}
		}
		
		public DateTime InfoBefore
		{
			get 
			{
				if (infoBeforeCheck.Active)
					return infoBeforeEntry.Date;
				return DateTime.MinValue;
			}
			set
			{
				if (value == DateTime.MinValue)
					return;
				infoBeforeCheck.Active = true;
				infoBeforeEntry.Date = value;
			}
		}
		
		
		public OCMQueryPage3 ()
		{
			this.Build ();
		}
		protected virtual void OnPlaceBeforeTogg (object sender, System.EventArgs e)
		{
			placeBeforeEntry.Sensitive = placeBeforeCheck.Active;
		}
		
		protected virtual void OnInfoAfterTog (object sender, System.EventArgs e)
		{
			infoAfterEntry1.Sensitive = infoAfterCheck.Active;
		}
		
		protected virtual void OnInfoBeforeTog (object sender, System.EventArgs e)
		{
			infoBeforeEntry.Sensitive = infoBeforeCheck.Active;
		}
		
		protected virtual void OnPlaceAfterCheck (object sender, System.EventArgs e)
		{
			this.placeAfterEntry.Sensitive = placeAfterCheck.Active;
		}
		
	}
}
