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
    
    
    public partial class OCMAboutDialog {
        
        private Gtk.VBox vbox3;
        
        private Gtk.HBox hbox4;
        
        private Gtk.Image image1;
        
        private Gtk.Label label14;
        
        private Gtk.Label label15;
        
        private Gtk.Button buttonCancel;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.OCMAboutDialog
            this.Name = "ocmgtk.OCMAboutDialog";
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.HasSeparator = false;
            // Internal child ocmgtk.OCMAboutDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 6;
            // Container child vbox3.Gtk.Box+BoxChild
            this.hbox4 = new Gtk.HBox();
            this.hbox4.Name = "hbox4";
            this.hbox4.Spacing = 6;
            // Container child hbox4.Gtk.Box+BoxChild
            this.image1 = new Gtk.Image();
            this.image1.Name = "image1";
            this.image1.Xpad = 5;
            this.image1.Ypad = 5;
            this.hbox4.Add(this.image1);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.hbox4[this.image1]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.label14 = new Gtk.Label();
            this.label14.Name = "label14";
            this.label14.LabelProp = Mono.Unix.Catalog.GetString("<big><b>Open Cache Manager</b></big>");
            this.label14.UseMarkup = true;
            this.label14.Justify = ((Gtk.Justification)(2));
            this.hbox4.Add(this.label14);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox4[this.label14]));
            w3.Position = 1;
            this.vbox3.Add(this.hbox4);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.vbox3[this.hbox4]));
            w4.Position = 0;
            w4.Expand = false;
            w4.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label15 = new Gtk.Label();
            this.label15.Name = "label15";
            this.label15.LabelProp = Mono.Unix.Catalog.GetString("Copyright 2009, Kyle Campbell");
            this.vbox3.Add(this.label15);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.vbox3[this.label15]));
            w5.Position = 1;
            w5.Expand = false;
            w5.Fill = false;
            w1.Add(this.vbox3);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(w1[this.vbox3]));
            w6.Position = 0;
            w6.Expand = false;
            w6.Fill = false;
            // Internal child ocmgtk.OCMAboutDialog.ActionArea
            Gtk.HButtonBox w7 = this.ActionArea;
            w7.Name = "dialog1_ActionArea";
            w7.Spacing = 6;
            w7.BorderWidth = ((uint)(5));
            w7.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w8 = ((Gtk.ButtonBox.ButtonBoxChild)(w7[this.buttonCancel]));
            w8.Expand = false;
            w8.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 706;
            this.DefaultHeight = 570;
            this.Show();
            this.buttonCancel.Clicked += new System.EventHandler(this.doClose);
        }
    }
}
