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
		public CacheStore Store
		{
			get { return m_store;}
		}
		
		private static Engine m_instance = null;
		private static string m_OwnerID = null;
		GPXParser m_parser = null;
		public GPXParser Parser
		{
			get{ return m_parser;}
		}
		
		private Engine()
		{
			m_store = new CacheStore();
			m_parser = new GPXParser();
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
		
		public IEnumerator<CacheLog> GetLogs(string code)
		{
			IEnumerator<CacheLog> logs = m_store.GetCacheLogs(code).GetEnumerator();
			return logs;
		}
		
		public IEnumerator<Waypoint> GetChildWaypoints(string code)
		{
			IEnumerator<Waypoint> pts =  m_store.GetChildren(code).GetEnumerator();
			return m_store.GetChildren(code).GetEnumerator();
		}
		
	}
}
