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
		
		public List<string> MustHaveIncludeAttributes
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
				if (dogFilt.IsFiltered && dogFilt.IsIncluded)
					attrs.Add(dogFilt.AttributeName);
				if (bikeFilt.IsFiltered && bikeFilt.IsIncluded)
					attrs.Add(bikeFilt.AttributeName);
				if (feeFilt.IsFiltered && feeFilt.IsIncluded)
					attrs.Add(feeFilt.AttributeName);
				if (kidFilt.IsFiltered && kidFilt.IsIncluded)
					attrs.Add(kidFilt.AttributeName);
				if (fireFilt.IsFiltered && fireFilt.IsIncluded)
					attrs.Add(fireFilt.AttributeName);
				if (timeFilt.IsFiltered && timeFilt.IsIncluded)
					attrs.Add(timeFilt.AttributeName);
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
				if (value.Contains(dogFilt.AttributeName))
					dogFilt.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(bikeFilt.AttributeName))
					bikeFilt.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(feeFilt.AttributeName))
					feeFilt.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(kidFilt.AttributeName))
					kidFilt.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(fireFilt.AttributeName))
					fireFilt.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(timeFilt.AttributeName))
					timeFilt.SetState(AttributeFilterWidget.AttrState.YES);
			}
		}
		
		public List<string> MustNotHaveIncludeAttributes
		{
			get
			{
				List<string> attrs = new List<string>();
				if (winterEAttr.IsFiltered && winterEAttr.IsIncluded)
					attrs.Add(winterEAttr.AttributeName);
				if (maintenanceEAttr.IsFiltered && maintenanceEAttr.IsIncluded)
					attrs.Add(maintenanceEAttr.AttributeName);
				if (nightEAttr.IsFiltered && nightEAttr.IsIncluded)
					attrs.Add(nightEAttr.AttributeName);
				if (beaconEFilt.IsFiltered && beaconEFilt.IsIncluded)
					attrs.Add(beaconEFilt.AttributeName);
				if (dogEFilt.IsFiltered && dogEFilt.IsIncluded)
					attrs.Add(dogEFilt.AttributeName);
				if (bikeEFilt.IsFiltered && bikeEFilt.IsIncluded)
					attrs.Add(bikeEFilt.AttributeName);
				if (feeEFilt.IsFiltered && feeEFilt.IsIncluded)
					attrs.Add(feeEFilt.AttributeName);
				if (kidEFilt.IsFiltered && kidEFilt.IsIncluded)
					attrs.Add(kidEFilt.AttributeName);
				if (fireEFilt.IsFiltered && fireEFilt.IsIncluded)
					attrs.Add(fireEFilt.AttributeName);
				if (timeEFilt.IsFiltered && timeEFilt.IsIncluded)
					attrs.Add(timeEFilt.AttributeName);
				return attrs;
			}
			set
			{
				if (value.Contains(winterEAttr.AttributeName))
					winterEAttr.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(maintenanceEAttr.AttributeName))
					maintenanceEAttr.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(nightEAttr.AttributeName))
					nightEAttr.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(beaconEFilt.AttributeName))
					beaconEFilt.SetState(AttributeFilterWidget.AttrState.YES);	
				if (value.Contains(dogEFilt.AttributeName))
					dogEFilt.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(bikeEFilt.AttributeName))
					bikeEFilt.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(feeEFilt.AttributeName))
					feeEFilt.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(kidEFilt.AttributeName))
					kidEFilt.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(fireEFilt.AttributeName))
					fireEFilt.SetState(AttributeFilterWidget.AttrState.YES);
				if (value.Contains(timeEFilt.AttributeName))
					timeEFilt.SetState(AttributeFilterWidget.AttrState.YES);
			}
		}
		
		public List<string> ExcludeMustHaveAttributes
		{
			get
			{
				List<string> attrs = new List<string>();
				if (winterAttr.IsFiltered && !winterAttr.IsIncluded)
					attrs.Add(winterAttr.AttributeName);
				if (nightAttr.IsFiltered && !nightAttr.IsIncluded)
					attrs.Add(nightAttr.AttributeName);
				if (dogFilt.IsFiltered && !dogFilt.IsIncluded)
					attrs.Add(dogFilt.AttributeName);
				if (bikeFilt.IsFiltered && !bikeFilt.IsIncluded)
					attrs.Add(bikeFilt.AttributeName);
				if (kidFilt.IsFiltered && !kidFilt.IsIncluded)
					attrs.Add(kidFilt.AttributeName);
				if (fireFilt.IsFiltered && !fireFilt.IsIncluded)
					attrs.Add(fireFilt.AttributeName);
				if (timeFilt.IsFiltered && !timeFilt.IsIncluded)
					attrs.Add(timeFilt.AttributeName);
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
				if (value.Contains(dogFilt.AttributeName))
					dogFilt.SetState(AttributeFilterWidget.AttrState.NO);
				if (value.Contains(bikeFilt.AttributeName))
					bikeFilt.SetState(AttributeFilterWidget.AttrState.NO);
				if (value.Contains(timeFilt.AttributeName))
					timeFilt.SetState(AttributeFilterWidget.AttrState.NO);
			}
		}
		
		public List<string> ExcludeMustNotHaveAttributes
		{
			get
			{
				List<string> attrs = new List<string>();
				if (winterEAttr.IsFiltered && !winterEAttr.IsIncluded)
					attrs.Add(winterEAttr.AttributeName);
				if (nightEAttr.IsFiltered && !nightAttr.IsIncluded)
					attrs.Add(nightEAttr.AttributeName);
				if (dogEFilt.IsFiltered && !dogEFilt.IsIncluded)
					attrs.Add(dogEFilt.AttributeName);
				if (bikeEFilt.IsFiltered && !bikeEFilt.IsIncluded)
					attrs.Add(bikeEFilt.AttributeName);
				if (kidEFilt.IsFiltered && !kidEFilt.IsIncluded)
					attrs.Add(kidEFilt.AttributeName);
				if (fireEFilt.IsFiltered && !fireEFilt.IsIncluded)
					attrs.Add(fireEFilt.AttributeName);
				if (timeEFilt.IsFiltered && !timeEFilt.IsIncluded)
					attrs.Add(timeEFilt.AttributeName);
				return attrs;
			}
			set
			{	
				if (value.Contains(winterEAttr.AttributeName))
					winterEAttr.SetState(AttributeFilterWidget.AttrState.NO);
				if (value.Contains(maintenanceEAttr.AttributeName))
					maintenanceEAttr.SetState(AttributeFilterWidget.AttrState.NO);
				if (value.Contains(nightEAttr.AttributeName))
					nightEAttr.SetState(AttributeFilterWidget.AttrState.NO);
				if (value.Contains(beaconEFilt.AttributeName))
					beaconEFilt.SetState(AttributeFilterWidget.AttrState.NO);
				if (value.Contains(dogEFilt.AttributeName))
					dogEFilt.SetState(AttributeFilterWidget.AttrState.NO);
				if (value.Contains(bikeEFilt.AttributeName))
					bikeEFilt.SetState(AttributeFilterWidget.AttrState.NO);
				if (value.Contains(kidEFilt.AttributeName))
					kidEFilt.SetState(AttributeFilterWidget.AttrState.NO);
				if (value.Contains(fireEFilt.AttributeName))
					fireEFilt.SetState(AttributeFilterWidget.AttrState.NO);
				if (value.Contains(timeEFilt.AttributeName))
					timeEFilt.SetState(AttributeFilterWidget.AttrState.NO);
			}
		}
	}
}
