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
using System.Collections.Generic;
using ocmengine;
using Mono.Unix;
using Gtk;
using System.Data;

namespace ocmgtk
{


	public partial class CopyingProgress : Gtk.Dialog
	{
		double total = 0;
		bool cancel = false;
		
		public CopyingProgress ()
		{
			this.Build ();
		}
		
		public void Start(String targetDB, bool isMove)
		{
			if (isMove)
			{
				Title = Catalog.GetString("Move Caches...");
				copyLabel.Text = Catalog.GetString("Moving Geocaches");
			}
			List<Geocache> caches = UIMonitor.getInstance().GetVisibleCacheList();
			CacheStore target = new CacheStore();
			CacheStore source = Engine.getInstance().Store;
			targetDBLabel.Text = targetDB;
			double count = 0;
			total = caches.Count;
			target.SetDB(targetDB);
			if (target.NeedsUpgrade())
			{
				MessageDialog dlg = new MessageDialog(null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok,
				                                      Catalog.GetString("The target database needs to be upgraded. " +
				                                      	"Please open the target database before trying to copy/move caches."));
				dlg.Run();
				dlg.Hide();
				this.Hide();
				return;
			}
			buttonOk.Visible = false;
			IDbTransaction trans = target.StartUpdate();
			foreach(Geocache cache in caches)
			{
				if (cancel)
				{
					target.CancelUpdate(trans);
					this.Hide();
					this.Dispose();
					return;
				}
				
				count++;
				UpdateProgress(count, cache.Name);
				target.AddCache(cache);
				target.AddWaypoint(cache);
				List<CacheLog> logs = source.GetCacheLogs(cache.Name);
				target.ClearLogs(cache.Name);
				foreach(CacheLog log in logs)
				{
					target.AddLog(cache.Name, log);
				}
				
				List<Waypoint> children = source.GetChildren(cache.Name);
				foreach (Waypoint child in children)
				{
					target.AddWaypoint(child);
				}
				
				if (isMove)
					source.DeleteGeocacheAtomic(cache);
			}
			statusLabel.Markup = Catalog.GetString("<i>Complete</i>");
			progressBar.Text = Catalog.GetString("Complete");
			buttonOk.Visible = true;
			buttonCancel.Visible = false;
			target.EndUpdate(trans);
		}
		
		private void UpdateProgress(double count, string name)
		{
			double fraction = count/total;
			progressBar.Fraction = fraction;
			progressBar.Text = fraction.ToString("0%");
			this.statusLabel.Markup = String.Format(Catalog.GetString("<i>Copying:{0}</i>"), name);
			while (Gtk.Application.EventsPending ())
				Gtk.Application.RunIteration (false);
		}
		protected virtual void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			this.Hide();
			this.Dispose();
		}
		
		protected virtual void OnCancelClick (object sender, System.EventArgs e)
		{
			cancel = true;
		}
		
		protected virtual void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
			cancel = true;
		}
		
		
		
	}
}
