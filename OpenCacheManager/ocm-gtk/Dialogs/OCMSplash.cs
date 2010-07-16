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
using System.Timers;
using Gdk;
using Gtk;

namespace ocmgtk
{


	public partial class OCMSplash : Gtk.Window
	{
		private static Pixbuf LOGO = new Pixbuf ("./icons/scalable/OCMLogo.svg", 96, 96);
		private static string m_file = null;
		public OCMSplash () : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
			this.image16.Pixbuf = LOGO;
			Timer splashtime = new Timer();
			Color cl = new Color(0,0,0);
			Color.Parse("#9D9FA1", ref cl);
			eventbox1.ModifyBg(StateType.Normal, cl);
			splashtime.AutoReset = false;
			splashtime.Elapsed += HandleSplashtimeElapsed;
			splashtime.Interval = 1000;
			splashtime.Start();
		}
		
		public void OpenFile(String file)
		{
			m_file = file;
		}

		void HandleSplashtimeElapsed (object sender, ElapsedEventArgs e)
		{
			Application.Invoke(delegate{this.Hide();
			MainWindow win = new MainWindow();
			UIMonitor.getInstance().LoadConfig();
			if (m_file != null)
					UIMonitor.getInstance().ImportGPXFile(m_file);
			});
		}
	}
}
