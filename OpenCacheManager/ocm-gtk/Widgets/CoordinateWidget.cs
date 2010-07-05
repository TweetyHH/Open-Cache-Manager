// 
//  Copyright 2010  campbelk
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


	[System.ComponentModel.ToolboxItem(true)]
	public partial class CoordinateWidget : Gtk.Bin
	{
		
		public CoordinateWidget ()
		{
			this.Build ();
		}
		
		public void SetCoordinate(double coord, bool isLat)
		{
			ocmengine.DegreeMinutes conv = new ocmengine.DegreeMinutes(coord);
			SetLatBox(isLat);
			if (conv.Degrees < 0)
			{
				degreeEntry.Text = (conv.Degrees * -1).ToString();
				directionCombo.Active = 1;
			}
			else
			{
				degreeEntry.Text = conv.Degrees.ToString();
				directionCombo.Active = 0;
			}
			
			minuteEntry.Text = conv.Minutes.ToString("0.000");
		}
		
		public double getCoordinate()
		{
			int degrees = int.Parse(degreeEntry.Text);
			if (directionCombo.Active == 1)
				degrees = degrees *-1;
			double minutes = double.Parse(minuteEntry.Text);
			ocmengine.DegreeMinutes conv = new ocmengine.DegreeMinutes(degrees, minutes);	
			return conv.GetDecimalDegrees();
		}
		
		private void SetLatBox(bool lat)
		{
			if (lat)
			{
				directionCombo.AppendText(Catalog.GetString("N"));
				directionCombo.AppendText(Catalog.GetString("S"));
			}
			else
			{
				directionCombo.AppendText(Mono.Unix.Catalog.GetString("E"));
				directionCombo.AppendText(Mono.Unix.Catalog.GetString("W"));
			}
		}
		
		public bool ValidateEntry()
		{
			try
			{
				int degrees = int.Parse(degreeEntry.Text);
				double minutes = double.Parse(minuteEntry.Text);
				
				if (degrees < 0 || minutes < 0)
					throw new Exception();
				return true;
			}
			catch (Exception)
			{
				Gtk.MessageDialog dlg = new Gtk.MessageDialog(null, Gtk.DialogFlags.DestroyWithParent,
				                                              Gtk.MessageType.Error, Gtk.ButtonsType.Ok,
				                                              Catalog.GetString("Invalid Coordinate"));
				dlg.Run();
				dlg.Hide();
				dlg.Dispose();
				return false;
			}
				
		}
		
	}
}
