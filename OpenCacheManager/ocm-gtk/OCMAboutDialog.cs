
using System;

namespace ocmgtk
{
	
	
	public partial class OCMAboutDialog : Gtk.Dialog
	{

		
		public OCMAboutDialog()
		{
			this.Build();
		}
		
		protected virtual void doClose (object sender, System.EventArgs e)
		{
			this.Hide();
		}		
	}
}
