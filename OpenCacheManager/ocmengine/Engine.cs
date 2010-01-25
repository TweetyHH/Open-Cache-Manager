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
using System.Collections.Generic;
using System.IO;

namespace ocmengine
{
	
	
	public class Engine
	{
		private CacheStore m_store;
		private static Engine m_instance = null;
		
		private Engine()
		{
			//TEMP: Cachestore is a based on hardcoded GPX
			m_store = new CacheStore();
			FileStream fs = new FileStream("../test/caches.gpx", FileMode.Open);
			GPXParser parser = new GPXParser();
			parser.parseGPXFile(fs, m_store);
		}
		
		public static Engine getInstance()
		{
			lock(typeof (Engine))
			{
				if (null == m_instance)
				{
					m_instance = new Engine();
				}
				return m_instance;
			}
		}
		
		public IEnumerator<Geocache> getCacheEnumerator()
		{
			return m_store.getCacheEnumerator();
		}
		
		public int CacheCount
		{
			get {return m_store.CacheCount;}
		}
		
	}
}
