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
using System.Xml;

namespace ocmengine
{
	
	
	public class CacheLog
	{
		DateTime m_logdate;
		const string LOG_PREFIX="groundspeak";
		string m_logged_by;
		string m_logmessage;
		string m_status;
		string m_finder_id;
		string m_logID = null;
		bool m_encoded;
		
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
		
		public string  LogStatus
		{
			 get { return m_status;}
			 set { m_status = value;}
		}
		
		public string FinderID
		{
			get { return m_finder_id;}
			set { m_finder_id = value;}
		}
		
		public string LogID
		{
			get { return m_logID;}
			set { m_logID = value;}
		}
		
		public bool Encoded
		{
			get { return m_encoded;}
			set { m_encoded = value;}
		}
				
		
		public CacheLog()
		{
		}
		
		public String toHTML()
		{
			string logHTML = "<div style='font-family:sans-serif;font-size:10pt'><hr/><b>Date:</b> ";
			logHTML += m_logdate.ToLongDateString();
			logHTML += "<br/><b>";
			logHTML += m_status;
			logHTML += "</b><br/>";
			logHTML += "<b>Logged By:</b> ";
			logHTML += m_logged_by;
			logHTML += "<hr/>";
			logHTML += m_logmessage;
			logHTML += "<br/><br/></div>";
			return logHTML;
		}
		
		public void WriteToGPX(XmlWriter writer)
		{
			writer.WriteStartElement(LOG_PREFIX,"log", GPXWriter.NS_CACHE);
			Random rand = new Random();
			if (!String.IsNullOrEmpty(m_logID))
				writer.WriteAttributeString("id", m_logID);
			else
			{
				rand.Next(50000);
				writer.WriteAttributeString("id", rand.Next(50000).ToString());
			}
			writer.WriteElementString(LOG_PREFIX,"date", GPXWriter.NS_CACHE, this.LogDate.ToString("o"));
			writer.WriteElementString(LOG_PREFIX,"type", GPXWriter.NS_CACHE, this.LogStatus);
			writer.WriteStartElement(LOG_PREFIX,"finder", GPXWriter.NS_CACHE);
			writer.WriteAttributeString("id", FinderID);
			writer.WriteString(LoggedBy);
			writer.WriteEndElement();
			writer.WriteStartElement(LOG_PREFIX,"text", GPXWriter.NS_CACHE);
			writer.WriteAttributeString("encoded", Encoded.ToString());
			writer.WriteString(LogMessage);
			writer.WriteEndElement();			
			writer.WriteEndElement();
		}
	}
}
