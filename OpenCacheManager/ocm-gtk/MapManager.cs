// 
//  Copyright 2011  Florian Plähn
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
using System.IO;
using System.Xml.XPath;

namespace ocmgtk
{
	public class MapManager
	{

		private string m_baseDirectory;
		
		public MapManager (string baseDirectory) 
		{
			if (!Directory.Exists(baseDirectory)) {
				throw new DirectoryNotFoundException("Didn't find directory: " + baseDirectory);
			}
			m_baseDirectory = baseDirectory;
		}
		
		public void addMaps(BrowserWidget browserWidget) {
			foreach (string file in Directory.GetFiles(m_baseDirectory)) {
				if (file.EndsWith(".xml")) {
					XPathNavigator nav = new XPathDocument (file).CreateNavigator();
					
					XPathNodeIterator codes = nav.Select("//code");
					while (codes.MoveNext()) {
						// TODO: tweety: add Map Layer instead of 10
						browserWidget.AddMap(codes.Current.Value, 1);	
					}
				}
			}
		}
	}
}
