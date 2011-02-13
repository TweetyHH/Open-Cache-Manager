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

	[Serializable]
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
		public const String KEY_COUNTRY = "country";
		public const String KEY_STATE = "state";
		public const String KEY_FOUNDON = "foundon";
		public const String KEY_FOUNDBEFORE = "foundafter";
		public const String KEY_FOUNDAFTER = "foundbefore";
		public const String KEY_OWNERID = "ownerID";
		public const String KEY_CHILDREN = "children";
		public const String KEY_NOCHILDREN = "nochildren";
		public const String KEY_NOTES = "notes";
		public const String KEY_CORRECTED = "corrected";
		public const String KEY_NOCORRECTED = "nocorreced";
		public const String KEY_INCATTRS = "incattrs";
		public const String KEY_EXCATTRS = "excattrs";
		public const String KEY_INCNOATTRS = "incnoattrs";
		public const String KEY_EXCNOATTRS = "excnoattrs";
		public const String KEY_FTF = "ftf";
		public const String KEY_DNF = "dnf";
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
		
		public int GetCount()
		{
			return m_criteria.Count;
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
				if (!status[2])
				{
					builder.Append(" AND GEOCACHE.owner !='");
					builder.Append(m_criteria[KEY_OWNERID]);
					builder.Append("'");
					builder.Append(" AND GEOCACHE.ownerID !='");
					builder.Append(m_criteria[KEY_OWNERID]);
					builder.Append("'");
				}
				else if (status[2] && !status[0] && !status[1])
				{
					builder.Append(" AND (GEOCACHE.owner =='");
					builder.Append(m_criteria[KEY_OWNERID]);
					builder.Append("'");
					builder.Append(" OR GEOCACHE.ownerID =='");
					builder.Append(m_criteria[KEY_OWNERID]);
					builder.Append("')");
				}					
				if (status[3] && !status[4] && status[5])
					builder.Append(" AND (GEOCACHE.available == 'True' OR Geocache.archived == 'True')");
				if (!status[3] && !status[4] && status[5])
					builder.Append(" AND GEOCACHE.available == 'False' AND Geocache.archived == 'True'");
				if (!status[3] && status[4] && !status[5])
					builder.Append(" AND GEOCACHE.available == 'False' AND Geocache.archived == 'False'");
				if (status[3] && !status[4] && !status[5])
					builder.Append(" AND GEOCACHE.available == 'True'");
				if (status[3] && status[4] && !status[5])
					builder.Append(" AND (GEOCACHE.available == 'True' OR Geocache.archived == 'False')");
				if (!status[3] && status[4] && status[5])
					builder.Append(" AND GEOCACHE.available == 'False'");
 			}
			
			if (m_criteria.ContainsKey(KEY_COUNTRY))
			{
				builder.Append(" AND GEOCACHE.country LIKE '");
				builder.Append(m_criteria[KEY_COUNTRY] as string);
				builder.Append("'");
			}
			
			if (m_criteria.ContainsKey(KEY_STATE))
			{
				builder.Append(" AND GEOCACHE.state LIKE '");
				builder.Append(m_criteria[KEY_STATE] as string);
				builder.Append("'");
			}
			
			if (m_criteria.ContainsKey(KEY_FOUNDAFTER) || m_criteria.ContainsKey(KEY_FOUNDBEFORE) 
			    || m_criteria.ContainsKey(KEY_FOUNDON))
			{
				builder.Append(" AND WAYPOINT.symbol == 'Geocache Found'");
			}
			
			if (m_criteria.Contains(KEY_NOTES))
			{
				builder.Append(" AND GEOCACHE.notes NOT NULL AND GEOCACHE.notes != ''");
			}
			
			if (m_criteria.Contains(KEY_CORRECTED))
			{
				builder.Append(" AND GEOCACHE.corlat NOT NULL AND GEOCACHE.corlat != '-1'");
			}
			
			if (m_criteria.Contains(KEY_NOCORRECTED))
			{
				builder.Append(" AND  (GEOCACHE.corlat IS NULL OR GEOCACHE.corlat ='-1')");
			}
			if (m_criteria.Contains(KEY_FTF))
			{
				builder.Append( " AND Geocache.ftf == '" + ((bool) m_criteria[KEY_FTF]).ToString() + "'");
			}
			if (m_criteria.Contains(KEY_DNF))
			{
				builder.Append( " AND Geocache.dnf == '" + ((bool) m_criteria[KEY_DNF]).ToString() + "'");
			}
			 
			System.Console.WriteLine(builder.ToString());
			return builder.ToString ();
		}
	}
}
