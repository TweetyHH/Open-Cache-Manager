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
using NDesk.DBus;
using org.freedesktop.DBus;

namespace ocmgtk
{
	class MainClass
	{

		public static void Main (string[] args)
		{
			
			Application.Init ();
			try
			{
				BusG.Init ();
				Bus bus = Bus.Session;
				string busName = "org.ocm.dbus";
				if (bus.RequestName (busName) != RequestNameReply.PrimaryOwner) {
				if (args != null)
					if (args.Length > 0) {
						IDBusComm comm = bus.GetObject<IDBusComm> (busName, new ObjectPath ("/org/ocm/dbus"));
						comm.ImportGPX (args[0]);
					}
				return;
				} else {
				DBusComm comm = new DBusComm ();
				bus.Register (new ObjectPath ("/org/ocm/dbus"), comm);
				}
			}
			catch
			{
				System.Console.Error.WriteLine("NO SESSION DBUS RUNNING");
			}
			
			Mono.Unix.Catalog.Init ("ocm", "./locale");
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
			} else {
				OCMSplash splash = new OCMSplash ();
				if (args != null)
					if (args.Length > 0)
						splash.OpenFile(args[0]);
				splash.Show ();
			}			
			
			Application.Run ();
			
		}
		
	}
}
