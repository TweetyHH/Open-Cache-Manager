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
using System.Collections.Generic;

namespace ocmgtk
{


	[System.ComponentModel.ToolboxItem(true)]
	public partial class OCMQueryPage5 : Gtk.Bin
	{

		public OCMQueryPage5 ()
		{
			this.Build ();
		}
		
		public List<string> IncludeAttributes
		{
			get
			{
				List<string> attrs = new List<string>();
				if (winterAttr.IsFiltered && winterAttr.IsIncluded)
					attrs.Add(winterAttr.AttributeName);
				if (maintenanceAttr.IsFiltered && maintenanceAttr.IsIncluded)
					attrs.Add(maintenanceAttr.AttributeName);
				if (nightAttr.IsFiltered && nightAttr.IsIncluded)
					attrs.Add(nightAttr.AttributeName);
				if (beaconFilt.IsFiltered && beaconFilt.IsIncluded)
					attrs.Add(beaconFilt.AttributeName);
				return attrs;
			}
			set
			{
				if (value.Contains(winterAttr.AttributeName))
					winterAttr.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(maintenanceAttr.AttributeName))
					maintenanceAttr.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(nightAttr.AttributeName))
					nightAttr.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(beaconFilt.AttributeName))
					beaconFilt.SetState(AttributeFilterWidget.AttrState.YES);	
			}
		}
		
		public List<string> ExcludeAttributes
		{
			get
			{
				List<string> attrs = new List<string>();
				if (winterAttr.IsFiltered && !winterAttr.IsIncluded)
					attrs.Add(winterAttr.AttributeName);
				if (maintenanceAttr.IsFiltered && !maintenanceAttr.IsIncluded)
					attrs.Add(maintenanceAttr.AttributeName);
				if (nightAttr.IsFiltered && !nightAttr.IsIncluded)
					attrs.Add(nightAttr.AttributeName);
				if (beaconFilt.IsFiltered && !beaconFilt.IsIncluded)
					attrs.Add(beaconFilt.AttributeName);
				return attrs;
			}
			set
			{	
				if (value.Contains(winterAttr.AttributeName))
					winterAttr.SetState(AttributeFilterWidget.AttrState.NO);
				if (value.Contains(maintenanceAttr.AttributeName))
					maintenanceAttr.SetState(AttributeFilterWidget.AttrState.NO);
				if (value.Contains(nightAttr.AttributeName))
					nightAttr.SetState(AttributeFilterWidget.AttrState.NO);
				if (value.Contains(beaconFilt.AttributeName))
					beaconFilt.SetState(AttributeFilterWidget.AttrState.NO);		
			}
		}
		
		/*public List<string> ExcludeAttributes
		{
			get
			{
				
			}
			set
			{
				
			}
		}*/
	}
}
