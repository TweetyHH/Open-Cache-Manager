// 
//  Copyright 2011  Kyle Campbell
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
using System.Text;
using System.IO;

namespace ocmengine
{


	public class FieldNotesHandler
	{
		public static void WriteToFile(CacheLog log, String fnFile)
		{
			List<CacheLog> logs = new List<CacheLog>();
			logs.Add(log);
			WriteToFile(logs, fnFile);
			
		}
		
		
		public static void WriteToFile(List<CacheLog> logs, String fnFile)
		{
			FileStream fstream = new FileStream(fnFile, FileMode.Append);
			TextWriter writer = new StreamWriter(fstream, Encoding.Unicode);
			foreach (CacheLog log in logs)
			{
				log.WriteToFieldNotesFile(writer);
			}
			writer.Close();
			fstream.Close();
		}
		
		public static void ClearFieldNotes(String fnFile)
		{
			if (File.Exists(fnFile))
			    File.Delete(fnFile);
		}
		                           
		
		public static List<CacheLog> GetLogs(String fnFile, String OwnerId)
		{
			List<CacheLog> logs = new List<CacheLog>();
			FileStream fstream = File.OpenRead(fnFile);
			TextReader reader = new StreamReader(fnFile, Encoding.Unicode);
			String logLine = reader.ReadLine();
			System.Console.WriteLine(logLine);
			while (!String.IsNullOrEmpty(logLine))
			{
				CacheLog log = new CacheLog();
				String[] parts = logLine.Split(',');
				log.CacheCode = parts[0];
				log.LogDate = DateTime.Parse(parts[1]);
				log.LogStatus = parts[2];
				StringBuilder message = new StringBuilder();
				if (parts.Length > 4)
				{
					RebuildLogMessage (parts);
				}
				if (!parts[3].EndsWith("\"") || parts[3] == "\"")
				{
					if (parts[3] != "\"")
						message.Append(parts[3].Substring(1));
					bool endReached = false;
					do
					{
						logLine = reader.ReadLine();
						if (logLine == "\"")
						{
							endReached = true;
						}
						if (logLine.EndsWith("\""))
						{
						    endReached = true;
							message.Append(logLine.Substring(0, logLine.Length -1));
						}
						else
						{
							message.Append(logLine);
						}
						message.Append('\n');
					}
					while (!endReached);
				}
				else
				{
					message.Append(parts[3].Substring(1,parts[3].Length -2));
				}
				log.LogMessage = message.ToString();
				log.LogKey = parts[0] + log.LogDate.ToFileTime().ToString();
				log.LoggedBy = "OCM";
				log.FinderID = OwnerId;
				logs.Add(log);
				logLine = reader.ReadLine();
			}
			reader.Close();
			fstream.Close();
			return logs;
		}
		
		private static void RebuildLogMessage (string[] parts)
		{
			StringBuilder builder = new StringBuilder();
			for (int i=3; i < parts.Length; i++)
			{
				if (i>3)
					builder.Append(",");
				builder.Append(parts[i]);
			}
			parts[3] = builder.ToString();
		}
	}
}
