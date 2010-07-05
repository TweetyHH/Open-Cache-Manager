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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ocmengine
{


	public class FilterList
	{
		Hashtable m_criteria;

		public const String KEY_CACHETYPE = "cachetype";
		public const String KEY_CONTAINER = "container";
		public const String KEY_DESCRIPTION = "description";
		public const String KEY_TERRAIN_VAL = "terrain_val";
		public const String KEY_TERRAIN_OP = "terrain_op";
		public const String KEY_DIFF_VAL = "diff_val";
		public const String KEY_DIFF_OP = "diff_op";
		public const String KEY_PLACEDBY = "placedby";
		public const String KEY_MINE = "mine";
		public const String KEY_STATUS = "status";
		public const String KEY_PLACEAFTER = "placeafter";
		public const String KEY_PLACEBEFORE = "placebefore";
		public const String KEY_INFOAFTER = "infoafter";
		public const String KEY_INFOBEFORE = "infobefore";
		public FilterList ()
		{
			
			m_criteria = new Hashtable ();
		}

		public void AddFilterCriteria (String key, Object val)
		{
			m_criteria.Add (key, val);
		}
		
		public void RemoveCriteria(String key)
		{
			m_criteria.Remove(key);
		}
		
		public object GetCriteria(String key)
		{
			return m_criteria[key];
		}
		
		public bool Contains(String key)
		{
			return m_criteria.Contains(key);
		}
		
		public void Clear()
		{
			m_criteria.Clear();
		}

		public String BuildWhereClause ()
		{
			StringBuilder builder = new StringBuilder ();
			String terrain_val = m_criteria[KEY_TERRAIN_VAL] as string;
			String terrain_op = m_criteria[KEY_TERRAIN_OP] as string;
			if (!String.IsNullOrEmpty(terrain_val)) {
				builder.Append (" AND GEOCACHE.terrain ");
				builder.Append (terrain_op);
				builder.Append (" ");
				builder.Append (terrain_val);
			}
			
			String diff_val = m_criteria[KEY_DIFF_VAL] as string;
			String diff_op = m_criteria[KEY_DIFF_OP] as string;
			if (!String.IsNullOrEmpty(diff_val)) {
				builder.Append (" AND GEOCACHE.difficulty ");
				builder.Append (diff_op);
				builder.Append (" ");
				builder.Append (diff_val);
			}
			
			List<String> cacheTypes = m_criteria[KEY_CACHETYPE] as List<String>;
			if (null != cacheTypes)
			{
				builder.Append( " AND GEOCACHE.type IN (");
				IEnumerator<String> ct = cacheTypes.GetEnumerator();
				bool firstDone = false;
				while (ct.MoveNext())
				{
					if (!firstDone)
						firstDone = true;
					else
						builder.Append(",");
					builder.Append("'");
					builder.Append(ct.Current);
					builder.Append("'");
				}
				builder.Append(")");
			}
			
			List<String> contTypes = m_criteria[KEY_CONTAINER] as List<String>;
			if (null != contTypes)
			{
				builder.Append( " AND GEOCACHE.container IN (");
				IEnumerator<String> ct = contTypes.GetEnumerator();
				bool firstDone = false;
				while (ct.MoveNext())
				{
					if (!firstDone)
						firstDone = true;
					else
						builder.Append(",");
					builder.Append("'");
					builder.Append(ct.Current);
					builder.Append("'");
				}
				builder.Append(")");
			}
			
			String placedBy = m_criteria[KEY_PLACEDBY] as string;
			if (null != placedBy)
			{
				builder.Append(" AND GEOCACHE.placedby == '");
				builder.Append(placedBy);
				builder.Append("'");
			}
			
			String mine = m_criteria[KEY_MINE] as string;
			if (null != mine)
			{
				
				builder.Append(" AND GEOCACHE.ownerID == '");
				builder.Append(mine);
				builder.Append("'");
			}
		
			string description = m_criteria[KEY_DESCRIPTION] as string;
			if (null != description)
			{
				String[] words = description.Split(' ');
				builder.Append(" AND (GEOCACHE.longdesc LIKE");
				Boolean firstDone = false;
				foreach (String word in words)
				{
					if (!firstDone)
						firstDone = true;
					else 
						builder.Append(" OR GEOCACHE.longdesc LIKE");
					builder.Append("'% ");
					builder.Append(word);
					builder.Append("%'");
				}
				builder.Append(')');
			}
			
			Boolean[] status = m_criteria[KEY_STATUS] as Boolean[];
			if (status != null)
			{
				if (!status[0] && status [1])
					builder.Append(" AND WAYPOINT.symbol == 'Geocache'");
				if (status[0] && !status [1])
					builder.Append(" AND WAYPOINT.symbol == 'Geocache Found'");
				if (status[2] && !status[3] && status[4])
					builder.Append(" AND GEOCACHE.available == 'True' OR Geocache.archived == 'True'");
				if (!status[2] && !status[3] && status[4])
					builder.Append(" AND GEOCACHE.available == 'False' AND Geocache.archived == 'True'");
				if (!status[2] && status[3] && !status[4])
					builder.Append(" AND GEOCACHE.available == 'False' AND Geocache.archived == 'FALSE'");
				if (status[2] && !status[3] && !status[4])
					builder.Append(" AND GEOCACHE.available == 'True'");
				if (!status[2] && status[3] && status[4])
					builder.Append(" AND GEOCACHE.available == 'False'");
 			}
			
			/*string placeBefore = m_criteria[KEY_PLACEBEFORE] as string;
			if (null != placeBefore)
			{
				builder.Append(" AND WAYPOINT.TIME <= '");
				builder.Append(placeBefore);
				builder.Append("'");
			}*/
			
			 
			System.Console.WriteLine(builder.ToString());
			return builder.ToString ();
		}
		
	}
}
