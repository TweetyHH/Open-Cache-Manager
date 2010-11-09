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
using ocmengine;

namespace ocmgtk
{


	public partial class CorrectedCoordinatesDlg : Gtk.Dialog
	{
		private Geocache m_cache;
		
		public double CorrectedLat
		{
			get { return correctLat.getCoordinate();}
		}
		
		public double CorrectedLon
		{
			get { return correctLon.getCoordinate();}
		}
		
		bool m_IsCorrected = false;
		public Boolean IsCorrected
		{
			get { return m_IsCorrected;}
		}
		
		private void SetActual(double lat, double lon)
		{
			String latStr = Utilities.getLatString(new DegreeMinutes(lat));
			String lonStr = Utilities.getLonString(new DegreeMinutes(lon));
			origLabel.Text = latStr + " " + lonStr;
		}
		
		public void SetCache(Geocache cache)
		{
			m_cache = cache;
			SetActual(cache.OrigLat, cache.OrigLon);
			if (cache.HasCorrected)
			{
				correctLat.SetCoordinate(cache.CorrectedLat, true);
				correctLon.SetCoordinate(cache.CorrectedLon, false);
				resetButton.Sensitive = true;
			}
			else
			{
				correctLat.SetCoordinate(cache.Lat, true);
				correctLon.SetCoordinate(cache.Lon, false);
			}	
			m_IsCorrected = cache.HasCorrected;
			correctLat.Changed += HandleCorrectLatChanged;
			correctLon.Changed += HandleCorrectLonChanged;
		}

		void HandleCorrectLonChanged (object sender, EventArgs args)
		{
			resetButton.Sensitive = true;
			m_IsCorrected = true;
		}

		void HandleCorrectLatChanged (object sender, EventArgs args)
		{
			resetButton.Sensitive = true;
			m_IsCorrected = true;
		}

		public CorrectedCoordinatesDlg ()
		{
			this.Build ();
		}
		protected virtual void OnResetClick (object sender, System.EventArgs e)
		{
			correctLat.SetCoordinate(m_cache.OrigLat, true);
			correctLon.SetCoordinate(m_cache.OrigLon, false);
			m_IsCorrected = false;
			resetButton.Sensitive = false;
		}
		
		
	}
}
