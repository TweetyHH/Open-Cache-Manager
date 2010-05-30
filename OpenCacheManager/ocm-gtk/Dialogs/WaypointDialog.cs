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
	
	
	public partial class WaypointDialog : Gtk.Dialog
	{
		private ocmengine.Waypoint m_point;
		public ocmengine.Waypoint Point
		{
			get { return m_point;}
		}		
		
		public WaypointDialog()
		{
			this.Build();
		}	
		
		public void SetPoint(ocmengine.Waypoint pnt)
		{
			m_point = pnt;
			descriptionEntry.Buffer.Text = pnt.Desc;
			flagEntry.Active = GetPTTypeCode(pnt.Symbol);
			nameEntry.Text = pnt.Name;
			latEntry.SetCoordinate(pnt.Lat, true);
			longEntry.SetCoordinate(pnt.Lon, false);
		}
		
		public ocmengine.Waypoint GetPoint()
		{
			m_point.Name = nameEntry.Text;
			m_point.Desc = descriptionEntry.Buffer.Text;
			m_point.Symbol = GetPTType(flagEntry.Active);
			m_point.Lat = latEntry.getCoordinate();
			m_point.Lon = longEntry.getCoordinate();
			m_point.Type = "Waypoint|" + m_point.Symbol;
			return m_point;
		}
		
		private int GetPTTypeCode(String ptType)
		{
			if (ptType.Equals("Final Location"))
				return 0;
			else if (ptType.Equals("Parking Area"))
				return 1;
			else if (ptType.Equals("Question to Answer"))
				return 2;
			else if (ptType.Equals("Reference Point"))
				return 3;
			else if (ptType.Equals("Stages of a Multicache"))
				return 4;
			else if (ptType.Equals("Trailhead"))
				return 5;
			else 
				return 6;
		}
		
		private string GetPTType(int code)
		{
			switch (code)
			{
			case 0:
				return "Final Location";
			case 1:
				return "Parking Area";
			case 2:
				return "Question to Answer";
			case 3:
				return "Reference Point";
			case 4:
				return "Stages of a Multicache";
			case 5:
				return "Trailhead";
			default:
				return "Other";
			}
		}
		
		protected virtual void OnOKClicked (object sender, System.EventArgs e)
		{
			if (validateEntry())
			{
				this.Hide();
			}
		}
		
		protected virtual void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			this.Hide();
			this.Dispose();
		}
		
		protected virtual void OnSelectionChanged (object sender, System.EventArgs e)
		{
			String prefix = "OT";
			switch (flagEntry.Active)
			{
			case 0:
				prefix = "FL";
				break;
			case 1:
				prefix = "PK";
				break;
			case 2:
				prefix = "QA";
				break;
			case 3:
				prefix = "RP";
				break;
			case 4:
				prefix = "SM";
				break;
			case 5:
				prefix = "TR";
				break;
			default:
				break;
			}
			nameEntry.Text = prefix + m_point.Parent.Substring(2);
		}
		
		private bool validateEntry()
		{
			if (String.IsNullOrEmpty(nameEntry.Text))
			{
				Gtk.MessageDialog msg = new Gtk.MessageDialog(this, Gtk.DialogFlags.DestroyWithParent,
				                                              Gtk.MessageType.Error, Gtk.ButtonsType.Ok,
				                                              "The name field is required.");
				msg.Run();
				msg.Hide();
				msg.Dispose();
				return false;
			}
			if (!latEntry.ValidateEntry())
				return false;
			if (!longEntry.ValidateEntry())
				return false;
			return true;
		}
		
		
	}
}
