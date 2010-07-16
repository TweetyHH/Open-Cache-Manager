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
using Mono.Unix;
using System.Globalization;

namespace ocmengine
{
	
	
	public class Utilities
	{
				
		
		/// <summary>
		/// See http://www.movable-type.co.uk/scripts/latlong.html
		/// </summary>
		/// <param name="lat1">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <param name="lat2">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <param name="lon1">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <param name="lon2">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Double"/>
		/// </returns>
		public static double calculateDistance(double lat1, double lat2, double lon1, double lon2)
		{
			int R = 6371;
			lat1 = toRad(lat1);
			lat2 = toRad(lat2);
			lon1 = toRad(lon1);
			lon2 = toRad(lon2);
			
			double d = Math.Acos(Math.Sin(lat1)*Math.Sin(lat2) + 
			Math.Cos(lat1)*Math.Cos(lat2) *Math.Cos(lon2-lon1)) * R;
			return d;
		}
		
		/// <summary>
		/// See http://www.movable-type.co.uk/scripts/latlong.html
		/// </summary>
		/// <param name="lat1">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <param name="lat2">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <param name="lon1">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <param name="lon2">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Double"/>
		/// </returns>
		public static int calculateBearing(double lat1, double lat2, double lon1, double lon2)
		{
			lat1 = toRad(lat1);
			lat2 = toRad(lat2);
			lon1 = toRad(lon1);
			lon2 = toRad(lon2);
			double dLon = lon2-lon1;
			double y = Math.Sin(dLon) * Math.Cos(lat2);
			double x = Math.Cos(lat1)*Math.Sin(lat2) - Math.Sin(lat1)*Math.Cos(lat2)*Math.Cos(dLon);	
			double b = Math.Atan2(y, x);
			return (int) (toDegrees(b) + 360) % 360;
		}
				
		public static double toRad(double degrees)
		{
			return degrees * (Math.PI/180);
		}
		
		public static double toDegrees(double radians)
		{
			return radians * (180/Math.PI);
		}
		
		public static string getCoordString(double lat, double lon)
		{
			return getCoordString(new DegreeMinutes(lat), new DegreeMinutes(lon));
		}
		
		public static string getCoordString(DegreeMinutes lat, DegreeMinutes lon)
		{
			return getLatString(lat) + " " + getLonString(lon);
		}
		
		public static string getLatString(DegreeMinutes lat)
		{
				
			String co_ordinate = "";
			
			if (lat.Degrees > 0)
				co_ordinate += String.Format(Catalog.GetString("N {0}° {1}"), lat.Degrees,lat.Minutes.ToString("0.000", CultureInfo.InvariantCulture));
			else
				co_ordinate += String.Format(Catalog.GetString("S {0}° {1}"), lat.Degrees * -1,  lat.Minutes.ToString("0.000", CultureInfo.InvariantCulture));
				
			return co_ordinate;
		}
		
		public static string getLonString(DegreeMinutes lon)
		{
			String co_ordinate = "";
			
			if (lon.Degrees > 0)
				co_ordinate += String.Format(Catalog.GetString("  E {0}° {1}"), lon.Degrees, lon.Minutes.ToString("#.000", CultureInfo.InvariantCulture));
			else
				co_ordinate += String.Format(Catalog.GetString("  W {0}° {1}"), lon.Degrees *-1 , lon.Minutes.ToString("#.000", CultureInfo.InvariantCulture));
		
			return co_ordinate;
		}
		
		public float[] ParseCoordString(String val)
		{
			float[] vals = new float[2];
			return vals;
		}
		
		public static double KmToMiles(double km)
		{
			return km * 0.6214;
		}
		
		public static double MilesToKm(double mi)
		{
			return mi / 0.6214;
		}
	}
}
