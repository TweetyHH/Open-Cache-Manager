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
using Cairo;
using Gtk;
using Pango;
using System.Text;
using ocmengine;

namespace ocmgtk.printing
{


	public class CachePrinter
	{
		private PrintOperation m_print;
        private static double headerHeight = (10*72/25.4);
        private static double headerGap = (3*72/25.4);
        private static int pangoScale = 1024;
        private double fontSize = 10.0;
        private int linesPerPage;
        private string[] lines;
        private int numLines;
        private int numPages;
		private Geocache m_cache;

		public CachePrinter ()
		{
			m_print = new PrintOperation();
			m_print.BeginPrint += HandleM_printBeginPrint;
			m_print.DrawPage += HandleM_printDrawPage;
			m_print.EndPrint += HandleM_printEndPrint;
		}
		
		public void StartPrinting(Geocache cache, MainWindow win)
		{
			m_cache = cache;
			m_print.Run(PrintOperationAction.PrintDialog, win);
		}
		

		void HandleM_printBeginPrint (object o, BeginPrintArgs args)
		{
			string contents;
            double height,width;

            // Get the Context of the Print Operation
            PrintContext context = args.Context;
           
            // Get the Height of the Context
            height = context.Height;
			width = context.Width;
			
			StringBuilder builder = new StringBuilder();
         
            // From the FontSize and the Height of the Page determine the Lines available per page
            linesPerPage = (int)Math.Floor(height / fontSize);
			int charsPerLine = (int)(Math.Floor(width/ fontSize) * 1.75);
           	contents = Utilities.HTMLtoText(m_cache.LongDesc);
           	
			int iPos =0;
			for (int i=0; i < contents.Length; i++)
			{
				builder.Append(contents[i]);
				if (contents[i] == '\n')
					iPos = 0;
				if (iPos == charsPerLine)
				{
					builder.Append('\n');
					iPos = 0;
				}
				else
				{
					iPos ++;
				}
			}

			contents = builder.ToString();
            // Split the Content into seperate lines
            lines = contents.Split('\n');
           
           
            // Get the Number of lines
            numLines = lines.Length;
           
            // Calculate the Number of Pages by how many lines there are and how many lines are available per page
            numPages = (numLines - 1) / linesPerPage + 1;
           
            // Tell the Print Operation how many pages there are
            m_print.NPages = numPages;
		}
		
		void HandleM_printDrawPage (object o, DrawPageArgs args)
		{
			 // Create a Print Context from the Print Operation
            PrintContext context = args.Context;

            // Create a Cairo Context from the Print Context
            Cairo.Context cr = context.CairoContext;
           
            // Get the width of the Print Context
            double width = context.Width;

            // Create a rectangle to be used for the Content
            cr.Rectangle (0, 0, width, headerHeight);
            cr.SetSourceRGB (0.95, 0.95, 0.95);
            cr.FillPreserve ();

            // Create a Stroke to outline the Content
            cr.SetSourceRGB (0, 0, 0);
            cr.LineWidth = 1;
            cr.Stroke();

            // Create a Pango Layout for the Text
            Pango.Layout layout = context.CreatePangoLayout ();
           
            // Set the Font and Font Size desired
            Pango.FontDescription desc = Pango.FontDescription.FromString ("sans 10");
            layout.FontDescription = desc;
           
            // Create a Header with the FileName and center it on the page
            layout.SetText (m_cache.Name);
            layout.Width = m_cache.Name.Length;
            layout.Alignment = Pango.Alignment.Center;

            // Get the Text Height fromt the Height of the layout and the Height of the Page
            int layoutWidth, layoutHeight;
            layout.GetSize (out layoutWidth, out layoutHeight);
            double textHeight = (double)layoutHeight / (double)pangoScale;

            // Move to the Footer
            cr.MoveTo (width/2, (headerHeight - textHeight) / 2);
            Pango.CairoHelper.ShowLayout (cr, layout);

            // Set the Page Number in the Footer with a right alignment
            string pageStr = String.Format ("{0}/{1}", args.PageNr + 1, numPages);
            layout.SetText (pageStr);
            layout.Alignment = Pango.Alignment.Right;

            cr.MoveTo (width - 2, (headerHeight - textHeight) / 2);
            Pango.CairoHelper.ShowLayout (cr, layout);

            // Create a new Pango Layout for the Content
            layout = null;
            layout = context.CreatePangoLayout ();

            // Set the Description of the Content
            desc = Pango.FontDescription.FromString ("sans");
            desc.Size = (int)(fontSize * pangoScale);
            layout.FontDescription = desc;
           
            // Move to the beginning of the Content, which is after the Header Height and Gap
            cr.MoveTo (0, headerHeight + headerGap);
           
            int line = args.PageNr * linesPerPage;
           
            // Draw the lines on the page according to how many lines there are left and how many lines can fit on the page
            for (int i=0; i < linesPerPage && line < numLines; i++)
            {
              layout.SetText (lines[line].TrimStart());
              Pango.CairoHelper.ShowLayout (cr, layout);
              cr.RelMoveTo (0, fontSize);
              line++;
            }
           
            layout = null;
		}
		
		void HandleM_printEndPrint (object o, EndPrintArgs args)
		{
			
		}

		
	}
}
