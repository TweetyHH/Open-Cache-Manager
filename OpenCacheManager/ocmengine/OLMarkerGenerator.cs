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
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace ocmengine
{
	
	
	public class OLMarkerGenerator
	{
		
		const string HEADER_ROW = "lat\tlon\ttitle\tdescription\ticon\ticonSize\ticonOffset\n";
		
		public OLMarkerGenerator()
		{
			
		}
		
		public static void GenerateCaceMarkerLayer(Engine eng)
		{
			StringBuilder builder = new StringBuilder(HEADER_ROW);
			IEnumerator<Geocache> cacheenum = eng.getCacheEnumerator();
			while (cacheenum.MoveNext())
			{
				Geocache cache = cacheenum.Current;
				builder.Append(cache.Lat);
				builder.Append("\t");
				builder.Append(cache.Lon);
				builder.Append("\t");
				builder.Append(cache.Name);
				builder.Append("\t");
				builder.Append("Some description");
				builder.Append("\t");
				builder.Append(GetIconNameForType(cache.TypeOfCache));
				builder.Append("\t");
				builder.Append("24,24");
				builder.Append("\t");
				builder.Append("-12,-12");			
				builder.Append("\n");
			}
			FileStream markerFile = new FileStream("../web/markers.txt", FileMode.Create);
			StreamWriter writer = new StreamWriter(markerFile, Encoding.UTF8);
			writer.Write(builder.ToString());
			writer.Close();			
		}
		
		private static string GetIconNameForType(Geocache.CacheType type)
		{
			switch (type)
			{
				case Geocache.CacheType.TRADITIONAL:
					return "../icons/32x32/traditional.png";
				case Geocache.CacheType.MYSTERY:
					return "../icons/32x32/unknown.png";
				case Geocache.CacheType.MULTI:
					return "../icons/32x32/multi.png";
				case Geocache.CacheType.LETTERBOX:
					return "../icons/32x32/letterbox.png";
				case Geocache.CacheType.EARTH:
					return "../icons/32x32/earth.png";
				case Geocache.CacheType.CITO:
					return "../icons/32x32/cito.png";
				default:
					return "../icons/32x32/unknown.png";
			}
		}
	}
}
