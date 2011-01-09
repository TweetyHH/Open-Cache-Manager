// 
//  Copyright 2010  campbelk
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
using Mono.Unix;
using System.Diagnostics;

namespace ocmgtk
{


	public partial class ProgressDialog : Gtk.Dialog
	{

		private GPXParser m_parser;
		double m_total = 0;
		double m_progress = 0;
		bool m_isMulti = false;
		bool m_autoClose = false;
		public bool AutoClose
		{
			get { return m_autoClose;}
			set { m_autoClose = value;}
		}
		
		public ProgressDialog (GPXParser parser, int total)
		{
			this.Build ();
			parser.ParseWaypoint += HandleParserParseWaypoint;
			parser.Complete += HandleParserComplete;
			m_parser = parser;
			m_total = total;
			multiFileLabel.Visible = false;
		}

		void HandleParserComplete (object sender, EventArgs args)
		{
			if (!m_isMulti)
				HandleCompletion ();
		}
		
		private void HandleCompletion ()
		{
			progressbar6.Text = Catalog.GetString("Complete");
			waypointName.Markup =  String.Format (Catalog.GetString("<i>Import complete, {0} waypoints processed</i>"), 
			                                      m_progress);
			okButton.Sensitive = true;
			okButton.Show();
			buttonCancel.Hide();
			buttonCancel.Sensitive = false;
			okButton.GrabDefault();
			if (m_autoClose)
				this.Hide();
		}
		
		
		public void StartMulti(String directoryPath, CacheStore store, bool deleteOnCompletion)
		{
			m_isMulti = true;
			string[] files = Directory.GetFiles(directoryPath);
			
			// Count total files
			m_progress = 0;
			m_total = 0;
			multiFileLabel.Visible = true;
			
			// Prescan for zip files and uncompress
			for (int i=0; i < files.Length; i++)
			{
				if (files[i].EndsWith(".zip"))
				{
					ProcessStartInfo start = new ProcessStartInfo();
					start.FileName = "unzip";
					System.Console.WriteLine(files[i] + " -d \"" + directoryPath + "\"");
					start.Arguments = "\"" + files[i] + "\" -d \"" + directoryPath + "\"";
					Process unzip =  Process.Start(start);
					while (!unzip.HasExited)
					{
						// Do nothing until exit	
					}
					if (deleteOnCompletion)
					{
						File.Delete(files[i]);
					}
				}
			}
			
			// Rescan for all GPX files, including those uncompressed by ZIP files
			files = Directory.GetFiles(directoryPath);
			
			for (int i=0; i < files.Length; i++)
			{
				if (files[i].EndsWith(".gpx"))
				{
					FileStream fs =  System.IO.File.OpenRead (files[i]);
					int total = m_parser.preParse(fs);
					System.Console.WriteLine("RunningTOTAL:" + total);
					m_total += total;
					
					fs.Close();
				}
			}
			
			System.Console.WriteLine("TOTAL:" + m_total);
			for (int i=0; i < files.Length; i++)
			{
				if (files[i].EndsWith(".gpx"))
				{
					FileStream fs =  System.IO.File.OpenRead (files[i]);
					// Need to reopen the file
					fs =  System.IO.File.OpenRead (files[i]);
					multiFileLabel.Text = String.Format(Catalog.GetString("Processing File {0} of {1}"), i + 1, files.Length);
					ParseFile(fs, store);
					fs.Close();
					if (deleteOnCompletion)
						File.Delete(files[i]);
				}
			}
			HandleCompletion();
		}


		public void Start (FileStream fs, CacheStore store)
		{
			this.Show ();
			try {
				m_progress = 0;
				ParseFile (fs, store);
			} catch (Exception e) {
				this.Hide ();
				UIMonitor.ShowException(e);
				this.Dispose ();
			}
		}
		
		private void ParseFile (FileStream fs, CacheStore store)
		{
			
				fileLabel.Markup = Catalog.GetString("<b>File: </b>") + fs.Name;
				m_parser.parseGPXFile (fs, store);
		}

		void HandleParserParseWaypoint (object sender, EventArgs args)
		{
			m_progress++;
			double fraction = (double)(m_progress / m_total);
			this.progressbar6.Text = (fraction).ToString ("0%");
			progressbar6.Fraction = fraction;
			this.waypointName.Markup = "<i>" + (args as ParseEventArgs).Message + "</i>";
			while (Gtk.Application.EventsPending ())
				Gtk.Application.RunIteration (false);
		}

		protected virtual void OnCancel (object sender, System.EventArgs e)
		{
			DoCancel ();
		}

		private void DoCancel ()
		{
			m_parser.Cancel = true;
			this.Hide ();
			String message = Catalog.GetString("Import cancelled, all changes reverted.");
			Gtk.MessageDialog dlg = new Gtk.MessageDialog (this, Gtk.DialogFlags.Modal, Gtk.MessageType.Info, 
			                                               Gtk.ButtonsType.Ok, message);
			dlg.Run ();
			dlg.Hide ();
			dlg.Dispose ();
		}
		protected virtual void OnCancel (object o, Gtk.DeleteEventArgs args)
		{
			DoCancel ();
		}
		
		protected virtual void OnButton179Clicked (object sender, System.EventArgs e)
		{
			this.Hide();
			this.Dispose();
		}
		
		
		
	}
}
