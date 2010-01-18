/*
 Copyright 2009 Kyle Campbell
 Licensed under the Apache License, Version 2.0 (the "License"); 
 you may not use this file except in compliance with the License. 
 You may obtain a copy of the License at 
 
 		http://www.apache.org/licenses/LICENSE-2.0 
 
 Unless required by applicable law or agreed to in writing, software distributed under the License 
 is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or 
 implied. See the License for the specific language governing permissions and limitations under the License. 
*/
using System;

namespace ocmengine
{
	
	
	public class CacheLog
	{
		public enum Status{FOUND_IT, DID_NOT_FIND, NOTE, NEEDS_MAINT, NEEDS_ARCHIVE, PUBLISHED, OTHER};
		
		DateTime m_logdate;
		string m_logged_by;
		string m_logmessage;
		CacheLog.Status m_status;
		
		public DateTime LogDate
		{
			get { return m_logdate;}
			set {m_logdate = value;}
		}
		
		public string LoggedBy
		{
			get { return m_logged_by;}
			set { m_logged_by = value;}
		}
		
		public string LogMessage
		{
			get { return m_logmessage;}
			set { m_logmessage = value;}
		}
		
		public CacheLog.Status LogStatus
		{
			 get { return m_status;}
			 set { m_status = value;}
		}
				
		
		public CacheLog()
		{
		}
		
		public String toHTML()
		{
			string logHTML = "<hr/><b>Date:</b> ";
			logHTML += m_logdate.ToLongDateString();
			logHTML += "<br/>";
			logHTML += m_status;
			logHTML += "<br/>";
			logHTML += "<b>Logged By:</b> ";
			logHTML += m_logged_by;
			logHTML += "<hr/>";
			logHTML += m_logmessage;
			logHTML += "<br/><br/>";
			return logHTML;
		}
	}
}
