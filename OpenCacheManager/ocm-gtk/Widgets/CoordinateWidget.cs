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

namespace ocmgtk
{


	[System.ComponentModel.ToolboxItem(true)]
	public partial class CoordinateWidget : Gtk.Bin
	{
		bool m_IsLat = false;
		
		public CoordinateWidget ()
		{
			this.Build ();
		}
		
		public void SetCoordinate(float coord, bool isLat)
		{
			System.Console.WriteLine("Coord in" + coord);
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
		
		public float getCoordinate()
		{
			int degrees = int.Parse(degreeEntry.Text);
			if (directionCombo.Active == 1)
				degrees = degrees *-1;
			double minutes = double.Parse(minuteEntry.Text);
			ocmengine.DegreeMinutes conv = new ocmengine.DegreeMinutes(degrees, minutes);	
			System.Console.WriteLine("Coord out" + conv.GetDecimalDegrees());
			return (float) conv.GetDecimalDegrees();
		}
		
		private void SetLatBox(bool lat)
		{
			m_IsLat = false;
			if (lat)
			{
				directionCombo.AppendText("N");
				directionCombo.AppendText("S");
			}
			else
			{
				directionCombo.AppendText(Mono.Posix.Catalog.GetString("E"));
				directionCombo.AppendText(Mono.Posix.Catalog.GetString("W"));
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
			catch (Exception e)
			{
				Gtk.MessageDialog dlg = new Gtk.MessageDialog(null, Gtk.DialogFlags.DestroyWithParent,
				                                              Gtk.MessageType.Error, Gtk.ButtonsType.Ok,
				                                              "Invalid Coordinate");
				dlg.Run();
				dlg.Hide();
				dlg.Dispose();
				return false;
			}
				
		}
		
	}
}
