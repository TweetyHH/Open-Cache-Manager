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
using GConf;

namespace ocmgtk
{


	public class Config:IConfig
	{
		private GConf.Client m_client;
		public SolvedMode SolvedModeState
		{
			get 
			{ 
				return (SolvedMode) Enum.Parse(typeof(SolvedMode), this.Get("/apps/ocm/solved_mode", 
				                                                                 SolvedMode.PUZZLES.ToString()) 
				                                     							as string); 
			}
			set 
			{ 
				m_client.Set("/apps/ocm/solved_mode", value.ToString()); 
			}
		}

		public Config ()
		{
			m_client = new Client();
		}
		
		public object Get(String key, Object def)
		{
			try
			{
				return m_client.Get(key);
			}
			catch
			{
				return def;
			}
		}
		
		public void Set(String key, Object val)
		{
			m_client.Set(key, val);
		}
		
	}
}
