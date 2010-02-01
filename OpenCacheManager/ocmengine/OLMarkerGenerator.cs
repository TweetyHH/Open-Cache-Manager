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
		
		public static void GenerateUnfoundCacheMarkerLayer(Engine eng)
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
				builder.Append(GenerateCacheDescription(cache));
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
		
		public static void GenerateChildPointLayer(IEnumerator<Waypoint> wpts)
		{
			StringBuilder builder = new StringBuilder(HEADER_ROW);
			while (wpts.MoveNext())
			{
				Waypoint pt = wpts.Current;
				builder.Append(pt.Lat);
				builder.Append("\t");
				builder.Append(pt.Lon);
				builder.Append("\t");
				builder.Append(pt.Name);
				builder.Append("\t");
				if (pt is Geocache)
					builder.Append((pt as Geocache).CacheName);
				else
					builder.Append(pt.Desc);
				builder.Append("\t");
				if (pt is Geocache)
					builder.Append(GetIconNameForType((pt as Geocache).TypeOfCache));
				else
					builder.Append("../icons/24x24/waypoint-flag-red.png");
				builder.Append("\t");
				builder.Append("24,24");
				builder.Append("\t");
				builder.Append("-12,-12");			
				builder.Append("\n");
			}
			FileStream markerFile = new FileStream("../web/childpts.txt", FileMode.Create);
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
		
		private static String GenerateCacheDescription(Geocache cache)
		{
			StringBuilder desc = new StringBuilder();
			desc.Append("<div style='border: 1px dashed black; font: 10pt Arial'>");
			desc.Append("<a href=\"javascript:doSomething()>");
			desc.Append(cache.CacheName);
			desc.Append("</a>");
			desc.Append("<br/>");
			desc.Append("Difficulty: ");
			desc.Append(cache.Difficulty.ToString("0.0/5  "));
			desc.Append("Terrain: ");
			desc.Append(cache.Terrain.ToString("0.0/5"));
			desc.Append("</div>");
			return desc.ToString();
		}
	}
}
