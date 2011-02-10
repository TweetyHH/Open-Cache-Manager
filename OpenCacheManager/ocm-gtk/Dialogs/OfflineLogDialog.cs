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
using ocmengine;
using Mono.Unix;

namespace ocmgtk
{


	public partial class OfflineLogDialog : Gtk.Dialog
	{
		public string m_cache = null;

		public OfflineLogDialog ()
		{
			this.Build ();
		}
		
		public Boolean EditMessageOnly
		{
			set {
				if (value == true)
				{
					logDateWidget.Sensitive = false;
					logChoice.Sensitive = false;
					ftfCheck.Sensitive = false;
				}
			}
		}
		
		public Boolean FTF
		{
			get { return ftfCheck.Active;}
		}
		
		public CacheLog Log
		{
			get
			{
				CacheLog log = new CacheLog();
				log.CacheCode = m_cache;
				log.LogDate = logDateWidget.Date;
				log.LogMessage = logEntry.Buffer.Text;
				log.LoggedBy = "OCM";
				log.FinderID = UIMonitor.getInstance().OwnerID;
				log.LogKey = m_cache + log.LogDate.ToFileTime().ToString();
				
				switch (logChoice.Active)
				{
					case 0:
						log.LogStatus = "Found it";
						break;
					case 1:
						log.LogStatus = "Didn't find it";
						break;
					case 2:
						log.LogStatus = "Write note";
						break;
					default:
						log.LogStatus = "Needs Maintenance";
						break;
				}
				return log;
			}
			set
			{
				m_cache = value.CacheCode;
				logDateWidget.Date = value.LogDate;
				logEntry.Buffer.Text = value.LogMessage;
				if (value.LogStatus == "Found it")
					logChoice.Active = 0;
				else if (value.LogStatus == "Didn't find it")
					logChoice.Active = 1;
				else if (value.LogStatus == "Write note")
					logChoice.Active = 2;
				else 
					logChoice.Active = 3;
				
				// Try getting cache from DB
				Geocache cache = Engine.getInstance().Store.GetCache(m_cache);
				if (cache == null)
				{
					cacheIcon.Pixbuf = IconManager.GetSmallCacheIcon(Geocache.CacheType.OTHER);
					CacheLabel.Markup= "<b><big>" + m_cache + ": " + Catalog.GetString("<Name Unavailable>") + "</big></b>";
				}
				else
				{
					cacheIcon.Pixbuf = IconManager.GetSmallCacheIcon(cache.TypeOfCache);
					CacheLabel.Markup = "<b><big>" + m_cache + ": " + cache.CacheName + "</big></b>";
				}
			}
		}
		protected virtual void OnComboChanged (object sender, System.EventArgs e)
		{
			ftfCheck.Sensitive = logChoice.Active == 0?true:false;
		}
		
		
	}
}
