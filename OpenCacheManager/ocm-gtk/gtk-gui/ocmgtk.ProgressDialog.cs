// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace ocmgtk {
    
    
    public partial class ProgressDialog {
        
        private Gtk.Alignment progressAlign;
        
        private Gtk.ProgressBar progressbar6;
        
        private Gtk.Label label1;
        
        private Gtk.Button buttonCancel;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.ProgressDialog
            this.Name = "ocmgtk.ProgressDialog";
            this.Title = Mono.Unix.Catalog.GetString("Please Wait....");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            // Internal child ocmgtk.ProgressDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(6));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.progressAlign = new Gtk.Alignment(0.5F, 1F, 1F, 1F);
            this.progressAlign.Name = "progressAlign";
            this.progressAlign.LeftPadding = ((uint)(5));
            this.progressAlign.TopPadding = ((uint)(5));
            this.progressAlign.RightPadding = ((uint)(4));
            this.progressAlign.BottomPadding = ((uint)(5));
            // Container child progressAlign.Gtk.Container+ContainerChild
            this.progressbar6 = new Gtk.ProgressBar();
            this.progressbar6.Name = "progressbar6";
            this.progressbar6.Text = Mono.Unix.Catalog.GetString("Loading");
            this.progressAlign.Add(this.progressbar6);
            w1.Add(this.progressAlign);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(w1[this.progressAlign]));
            w3.Position = 0;
            w3.Expand = false;
            w3.Fill = false;
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.Xalign = 0F;
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Processed: 0");
            w1.Add(this.label1);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(w1[this.label1]));
            w4.Position = 1;
            // Internal child ocmgtk.ProgressDialog.ActionArea
            Gtk.HButtonBox w5 = this.ActionArea;
            w5.Name = "dialog1_ActionArea";
            w5.Spacing = 10;
            w5.BorderWidth = ((uint)(5));
            w5.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w6 = ((Gtk.ButtonBox.ButtonBoxChild)(w5[this.buttonCancel]));
            w6.Expand = false;
            w6.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 380;
            this.DefaultHeight = 146;
            this.Show();
        }
    }
}
