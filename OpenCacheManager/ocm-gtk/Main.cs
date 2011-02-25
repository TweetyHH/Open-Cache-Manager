/*
 Copyright 2009 Kyle Campbell
 Licensed under the Apache License, Version 2.0 (the "License"); 
 you may not use this file except in compliance with the License. 
 You may obtain a copy of the License at 
 
 		http://www.apache.org/licenses/LICENSE-2.0 
 
 Unless required by applicable law or agreed to in writing, software distributed under the License 
 is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or 
 implied. See the License for the specific language governing permissions and limitations under the License. 
*/
using System;
using System.Threading;
using Gtk;
using System.Timers;
using NDesk.DBus;
using org.freedesktop.DBus;

namespace ocmgtk
{
	class MainClass
	{
		static string m_file = null;
		static OCMSplash m_splash = null;
		
		public static void Main (string[] args)
		{
			
			Application.Init ();
			try
			{
				BusG.Init ();
				Bus bus = Bus.Session;
				string busName = "org.ocm.dbus";
				if (bus.RequestName (busName) != RequestNameReply.PrimaryOwner) 
				{
					IDBusComm comm = bus.GetObject<IDBusComm> (busName, new ObjectPath ("/org/ocm/dbus"));
					if (args != null)
					{
						if (args.Length > 0) 
							comm.ImportGPX (args[0]);
					}
					comm.ShowOCM();
					return;
				}
				else 
				{
					DBusComm comm = new DBusComm ();
					bus.Register (new ObjectPath ("/org/ocm/dbus"), comm);
				}
			}
			catch
			{
				System.Console.Error.WriteLine("NO SESSION DBUS RUNNING");
			}
			
			if (args != null)
				if (args.Length > 0)
					m_file = args[0];
			//System.Console.WriteLine("Path is " + "@expanded_datadir@/locale");
			// Set the localeDirectory right both for developement or for installed versions
			String localeDirectory = "@expanded_datadir@/locale";
			if (localeDirectory.Contains("@" + "expanded_datadir" + "@")) {
				localeDirectory = "./locale";
			}
			Mono.Unix.Catalog.Init ("opencachemanager", localeDirectory);
			//Mono.Unix.Catalog.Init ("opencachemanager", "./locale");
			//Mono.Unix.Catalog.Init ("opencachemanager", "@expanded_datadir@/locale");
			bool runWizard = false;
			try {
				GConf.Client client = new GConf.Client ();
				client.Get ("/apps/ocm/wizardone");
				runWizard = false;
			} catch (GConf.NoSuchKeyException) {
				runWizard = true;
			}
			
			if (runWizard) {
				UIMonitor.getInstance ().RunSetupAssistant ();
			} 
			else
			{
				ShowSplash();
			}			
			
			Application.Run ();
			
		}
		
		public static void ShowSplash()
		{
			m_splash = new OCMSplash();
			m_splash.ShowNow();
			System.Timers.Timer splashtime = new System.Timers.Timer();
			splashtime.AutoReset = false;
			splashtime.Interval = 1000;
			splashtime.Elapsed += HandleSplashtimeElapsed;
			splashtime.Start();
		}

		static void HandleSplashtimeElapsed (object sender, ElapsedEventArgs e)
		{
			Application.Invoke(delegate{
				ShowMain();
			});
		}
		
		public static void ShowMain()
		{
			if (m_splash != null)
			{
				m_splash.Hide();
				m_splash.Dispose();
			}
			
			MainWindow win = new MainWindow();
			while (Gtk.Application.EventsPending ())
				Gtk.Application.RunIteration (false);
			
			if (m_file != null)
			{
				UIMonitor.getInstance().LoadConfig(false);
				if (m_file.EndsWith(".ocm"))
				{
					UIMonitor.getInstance().SetCurrentDB(m_file, true);
				}
				else
				{
					
					if (m_file.EndsWith(".zip"))
					{
						UIMonitor.getInstance().ImportZip(m_file);
					}
					else
					{
						UIMonitor.getInstance().ImportGPXFile(m_file);
					}
				}
			}
			else
			{
				UIMonitor.getInstance().LoadConfig(true);
			}
		}		
	}
}
