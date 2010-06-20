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
using System.IO;
using ocmengine;

namespace ocmgtk
{


	[System.ComponentModel.ToolboxItem(true)]
	public partial class SetupAssistant : Gtk.Assistant
	{
		SetupAssistantPage1 page1 = new SetupAssistantPage1 ();
		SetupAssistantPage2 page2 = new SetupAssistantPage2 ();
		SetupAssistantPage3 page3 = new SetupAssistantPage3 ();

		public SetupAssistant ()
		{
			this.Build ();
			this.Parent = UIMonitor.getInstance ().Main;
			
			AppendPage (page1);
			AppendPage (page2);
			AppendPage (page3);
			Title = "Setup Assistant";
			SetPageTitle (page1, "Welcome");
			SetPageComplete (page1, true);
			SetPageType (page1, Gtk.AssistantPageType.Intro);
			SetPageTitle (page2, "Setup a Database");
			SetPageComplete (page2, true);
			SetPageTitle (page3, "User Details");
			SetPageType (page3, Gtk.AssistantPageType.Summary);
			WidthRequest = 600;
			HeightRequest = 400;
			this.Cancel += HandleHandleCancel;
			this.Apply += HandleHandleApply;
			this.Close += HandleHandleClose;
		}

		void HandleHandleClose (object sender, EventArgs e)
		{
			this.Hide ();
			this.Dispose ();
			
			if (!File.Exists(page2.DBFile))
				Engine.getInstance().Store.CreateDB(page2.DBFile);	
			
			GConf.Client client = new GConf.Client();
			client.Set("/apps/ocm/currentdb", page2.DBFile);
			client.Set("/apps/ocm/homelat", page3.HomeLat);
			client.Set("/apps/ocm/homelon", page3.HomeLon);
			client.Set("/apps/ocm/memberid", page3.MemberID);
			client.Set("/apps/ocm/wizardone", "true");			
			
			MainWindow win = new MainWindow ();
			win.Show ();
			win.Maximize();
		}

		void HandleHandleApply (object sender, EventArgs e)
		{
			
		}

		void HandleHandleCancel (object sender, EventArgs e)
		{
			this.Hide ();
		}
	}
}
