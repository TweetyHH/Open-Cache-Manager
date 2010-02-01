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

namespace ocmengine
{
	
	
	public class CacheStore
	{
		
		Dictionary<string, Geocache> m_caches; // TEMPORARY STORAGE
		Dictionary<string, Waypoint> m_waypoint;
		
		public int CacheCount
		{
			get { return m_caches.Count;}
		}
		
		public int WaypointCount
		{
			get { return m_waypoint.Count;}
		}
		
		public CacheStore()
		{
			m_caches = new Dictionary<string, Geocache>();
			m_waypoint = new Dictionary<string, Waypoint>();
		}
		
		public void AddCache(Geocache cache)
		{
			m_caches.Add(cache.Name, cache);
		}
		
		public void AddWaypoint(Waypoint point)
		{
			if (point is Geocache)
				AddCache(point as Geocache);
			else
				m_waypoint.Add(point.Name, point);
		}
		
		public Geocache getCache(string name)
		{
			return m_caches[name];
		}
		
		public Waypoint getWayoint(string name)
		{
			return m_waypoint[name];
		}
		
		public IEnumerator<Geocache> getCacheEnumerator()
		{
			return m_caches.Values.GetEnumerator();
		}
		
		public IEnumerator<Waypoint> getWPTEnumerator()
		{
			return m_waypoint.Values.GetEnumerator();
		}
		
		public List<Waypoint> GetChildren(String cachecode)
		{
			//TODO: TEMPORARY IMPLEMENTATION
			List<Waypoint> pts = new List<Waypoint>();
			pts.Add(m_caches[cachecode]);
			IEnumerator<Waypoint> allpoints = m_waypoint.Values.GetEnumerator();
			while (allpoints.MoveNext())
			{
				if (allpoints.Current.Parent == cachecode)
				{
					pts.Add(allpoints.Current);
				}
			}
			return pts;
		}
	}		
}
