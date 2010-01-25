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

namespace ocmengine
{
	
	
	public class Utilities
	{
		
		public static DegreeMinutes convertDDtoDM(double dd_coordinate)
		{
			double degrees;
			double decimal_minutes;
			if (dd_coordinate > 0)
			{
				degrees = Math.Floor(dd_coordinate);
				System.Console.WriteLine(degrees);
				decimal_minutes = dd_coordinate - degrees;
				System.Console.WriteLine(decimal_minutes);
			}
			else
			{
				degrees = Math.Ceiling(dd_coordinate);
				decimal_minutes = (dd_coordinate - degrees) * -1;
			}
			decimal_minutes = decimal_minutes * 60;
			return new DegreeMinutes((int) degrees, decimal_minutes);		
		}
		
		
		public static double calculateDisatnce(double lat1, double lat2, double lon1, double lon2)
		{
			int R = 6371; // km
			lat1 = toRad(lat1);
			lat2 = toRad(lat2);
			lon1 = toRad(lon1);
			lon2 = toRad(lon2);
			
			double d = Math.Acos(Math.Sin(lat1)*Math.Sin(lat2) + 
			Math.Cos(lat1)*Math.Cos(lat2) *Math.Cos(lon2-lon1)) * R;
			return d;
		}
				
		public static double toRad(double degrees)
		{
			return degrees * (Math.PI/180);
		}
	}
}
