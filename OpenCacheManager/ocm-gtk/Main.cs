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
		static MainWindow win = null;

		public static void Main (string[] args)
		{
			BusG.Init ();
			Application.Init ();
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
			
			Mono.Unix.Catalog.Init ("i8n1", "./locale");
			try {
				GConf.Client client = new GConf.Client ();
				client.Get ("/apps/ocm/wizardone");
				MainWindow win = new MainWindow ();
				win.Show ();
				if (args != null)
					if (args.Length > 0)
						UIMonitor.getInstance ().ImportGPXFile (args[0]);
			} catch (Exception e) {
				UIMonitor.getInstance ().RunSetupAssistant ();
			}
			
			Application.Run ();
			
		}
		
	}
}
