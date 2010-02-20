// 
//  Copyright 2010  campbelk
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
using System.Xml;

namespace ocmengine
{


	public class TravelBug
	{
		private string m_id;
		private string m_ref;
		private string m_name;
		
		public String ID
		{
			get { return m_id;}
			set { m_id = value;}
		}
		
		public String Ref
		{
			get { return m_ref;}
			set { m_ref = value;}
		}
		
		public String Name
		{
			get { return m_name;}
			set { m_name = value;}
		}
		
		public void WriteToGPX(XmlWriter writer)
		{
			writer.WriteStartElement("travelbug");
			writer.WriteAttributeString("id", ID);
			writer.WriteAttributeString("ref", Ref);
			writer.WriteElementString("name", Name);
			writer.WriteEndElement();
		}
	}
}
