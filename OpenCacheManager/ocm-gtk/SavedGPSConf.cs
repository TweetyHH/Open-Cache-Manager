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

		public SavedGPSConf(GConf.Client client)
		{
			m_Format = client.Get("/apps/ocm/gps/type") as String;
			m_Limit = (int) client.Get("/apps/ocm/gps/limit");
			m_File = client.Get("/apps/ocm/gps/file") as String;
		}
	}
}
