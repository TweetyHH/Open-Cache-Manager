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

namespace ocmgtk
{


	public partial class ExportGPXDialog : Gtk.Dialog
	{
		public ExportGPXDialog ()
		{
			this.Build ();
		}
		
		public void SetCurrentFolder(String folder)
		{
			fileChooser.SetCurrentFolder(folder);
		}
		
		public String CurrentName
		{
			set{ fileChooser.CurrentName = value;}
		}
		
		public String Filename
		{
			get { return fileChooser.Filename;}
		}
		
		public void AddFilter(Gtk.FileFilter filter)
		{
			fileChooser.AddFilter(filter);
		}
		
		public bool IsPaperless
		{
			get { return asPaperlessRadio.Active;}
		}
		
		public int CacheLimit
		{
			get {
				if (limitCheck.Active)
					return int.Parse(limitEntry.Text);
				else
					return -1;
			}
		}
		
		public bool IncludeChildren
		{
			get { return includeChildrenCheck.Active; }
		}
		
		public bool UseMappings
		{
			get { return useMappingsCheck.Active;}
		}
		
		public ocmengine.WaypointNameMode GetNameMode()
		{
			if (asPaperlessRadio.Active)
				return ocmengine.WaypointNameMode.CODE;
			switch (nameMode.Active)
			{
				case 0:
					return ocmengine.WaypointNameMode.CODE;
				default:
					return ocmengine.WaypointNameMode.NAME;
			}
		}
		
		public ocmengine.WaypointDescMode GetDescMode()
		{
			if (asPaperlessRadio.Active)
				return ocmengine.WaypointDescMode.DESC;
			switch (descMode.Active)
			{
				case 0:
					return ocmengine.WaypointDescMode.DESC;
				case 1:
					return ocmengine.WaypointDescMode.CODESIZEANDHINT;
				default:
					return ocmengine.WaypointDescMode.CODESIZETYPE;
			}
		}
		
		public int LogLimit
		{
			get { 
				if (logLimitCheck.Active)
				    return int.Parse(logLimitEntry.Text);
				else
					return -1;
			}
		}
		
		public bool MustHaveIncludeAttributes
		{
			get {return attrCheck.Active;}
		}
		
		public bool UsePlainText
		{
			get {return usePlainTextCheck.Active;}
		}
		
		protected virtual void OnPaperlessToggle (object sender, System.EventArgs e)
		{
			paperlessFrame.Sensitive = asPaperlessRadio.Active;
		}
		
		protected virtual void OnAsWaypointToggle (object sender, System.EventArgs e)
		{
			waypointFrame.Sensitive = asWaypointRadio.Active;
		}
		
		protected virtual void OnLimitToggle (object sender, System.EventArgs e)
		{
			limitEntry.Sensitive = limitCheck.Active;
		}
		protected virtual void OnLogLimitToggle (object sender, System.EventArgs e)
		{
			logLimitEntry.Sensitive = logLimitCheck.Active;
		}
		
		
	}
}
