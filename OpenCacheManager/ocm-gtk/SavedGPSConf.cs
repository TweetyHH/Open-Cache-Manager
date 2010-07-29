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


	public class SavedGPSConf : IGPSConfig
	{
		string m_Format = null;
		public string GetBabelFormat ()
		{
			return m_Format;
		}
		
		int m_Limit = -1;
		public int GetCacheLimit ()
		{
			return m_Limit;
		}

		string m_File = null;
		public string GetOutputFile ()
		{
			return m_File;
		}
		
		int m_LogLimit;
		public int GetLogLimit()
		{
			return m_LogLimit;
		}
		
		bool m_waypointOverrides;
		public bool IgnoreWaypointOverrides()
		{
			return m_waypointOverrides;
		}
		
		bool m_geocacheOverrides;
		public bool IgnoreGeocacheOverrides()
		{
			return m_geocacheOverrides;
		}
		
		ocmengine.WaypointNameMode m_namemode;
		public ocmengine.WaypointNameMode GetNameMode()
		{
			return m_namemode;
		}
		
		ocmengine.WaypointDescMode m_descmode;
		public ocmengine.WaypointDescMode GetDescMode()
		{
			return m_descmode;
		}

		public SavedGPSConf(Config client)
		{
			m_Format = client.Get("/apps/ocm/gps/type", "OCM_GPX") as String;
			m_Limit = (int) client.Get("/apps/ocm/gps/limit", -1);
			m_File = client.Get("/apps/ocm/gps/file", "geocaches.gpx") as String;
			m_namemode = (ocmengine.WaypointNameMode) Enum.Parse(typeof (ocmengine.WaypointNameMode), client.Get("/apps/ocm/gps/namemode", "CODE") as string);
			m_descmode = (ocmengine.WaypointDescMode) Enum.Parse(typeof (ocmengine.WaypointDescMode), client.Get("/apps/ocm/gps/descmode", "DESC") as string);
			m_LogLimit = (int) client.Get("/apps/ocm/gps/loglimit", 5);
			m_waypointOverrides = (Boolean) client.Get("/apps/ocm/gps/ignorewaypointsym", false);
			m_geocacheOverrides = (Boolean) client.Get("/apps/ocm/gps/ignoregeocachesym", false);
		}
	}
}
