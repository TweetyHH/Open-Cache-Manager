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


	[System.ComponentModel.ToolboxItem(true)]
	public partial class LocationWidget : Gtk.Bin
	{

		public double Latitude
		{
			get { return latWidget.getCoordinate();}
			set { latWidget.SetCoordinate(value, true);}
		}
		
		public double Longitude
		{
			get { return lonWidget.getCoordinate();}
			set { lonWidget.SetCoordinate(value, true);}
		}
		
		public LocationWidget ()
		{
			this.Build ();
		}
	}
}
