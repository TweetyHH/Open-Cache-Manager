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
using System.Diagnostics;
using ocmengine;
using Mono.Unix;

namespace ocmgtk
{

	[Serializable]
	public class ExternalTool
	{	
		
		string m_command, m_name;
		public String Command
		{
			get { return m_command;}
			set { m_command = value;}
		}
		
		public String Name
		{
			get { return m_name;}
			set { m_name = value;}
		}
		
		public ExternalTool()
		{
		}
		
		public ExternalTool (String name, String cmd)
		{
			m_command = cmd;
			m_name = name;
		}
		
		public void RunCommand()
		{
			string cmd = m_command;
			if (cmd.Contains("%gpx%"))
			{
				UIMonitor mon = UIMonitor.getInstance();
				GPXWriter writer = new GPXWriter();
				String tempPath = System.IO.Path.GetTempPath();
				String tempFile = tempPath + "ocm_export.gpx";
				ExportProgressDialog dlg = new ExportProgressDialog(writer);	
				dlg.AutoClose = true;
				dlg.Title = Catalog.GetString("Preparing GPX File");
				dlg.WaypointsOnly = true;
				dlg.CompleteCommand = m_command.Replace("%gpx%", tempFile);
				dlg.Start(tempFile, mon.GetVisibleCacheList(), mon.WaypointMappings);
			}
			else if (cmd.Contains("%finds%"))
			{
				UIMonitor mon = UIMonitor.getInstance();
				GPXWriter writer = new GPXWriter();
				writer.IsMyFinds = true;
				writer.MyFindsOwner = mon.OwnerID;
				String tempPath = System.IO.Path.GetTempPath();
				String tempFile = tempPath + "ocm_finds.gpx";
				ExportProgressDialog dlg = new ExportProgressDialog(writer);	
				dlg.AutoClose = true;
				dlg.Title = Catalog.GetString("Preparing GPX File");
				dlg.WaypointsOnly = true;
				dlg.CompleteCommand = m_command.Replace("%finds%", tempFile);
				dlg.Start(tempFile, Engine.getInstance().Store.GetFinds(), mon.WaypointMappings);
			}
			else
			{
				Process.Start(Utilities.StringToStartInfo(m_command));
			}
		}
	}
}
