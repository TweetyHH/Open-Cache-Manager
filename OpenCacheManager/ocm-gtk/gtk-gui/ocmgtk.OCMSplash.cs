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
    
    
    public partial class OCMSplash {
        
        private Gtk.VBox vbox2;
        
        private Gtk.HBox hbox1;
        
        private Gtk.Image image16;
        
        private Gtk.Label label1;
        
        private Gtk.EventBox eventbox1;
        
        private Gtk.HBox hbox4;
        
        private Gtk.Label label137;
        
        private Gtk.Image image20;
        
        private Gtk.Label label135;
        
        private Gtk.Label label136;
        
        private Gtk.HSeparator hseparator1;
        
        private Gtk.Label pby;
        
        private Gtk.Label versionLabel;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.OCMSplash
            this.Name = "ocmgtk.OCMSplash";
            this.Title = Mono.Unix.Catalog.GetString("OCMSplash");
            this.TypeHint = ((Gdk.WindowTypeHint)(4));
            this.WindowPosition = ((Gtk.WindowPosition)(1));
            this.Resizable = false;
            this.AllowGrow = false;
            this.Decorated = false;
            // Container child ocmgtk.OCMSplash.Gtk.Container+ContainerChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            this.vbox2.BorderWidth = ((uint)(12));
            // Container child vbox2.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.image16 = new Gtk.Image();
            this.image16.Name = "image16";
            this.hbox1.Add(this.image16);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.hbox1[this.image16]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            w1.Padding = ((uint)(6));
            // Container child hbox1.Gtk.Box+BoxChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = "<span font='24'><b>Open Cache Manager</b></span>";
            this.label1.UseMarkup = true;
            this.hbox1.Add(this.label1);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.hbox1[this.label1]));
            w2.Position = 1;
            this.vbox2.Add(this.hbox1);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.vbox2[this.hbox1]));
            w3.Position = 0;
            w3.Expand = false;
            w3.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.eventbox1 = new Gtk.EventBox();
            this.eventbox1.Name = "eventbox1";
            // Container child eventbox1.Gtk.Container+ContainerChild
            this.hbox4 = new Gtk.HBox();
            this.hbox4.Name = "hbox4";
            this.hbox4.Spacing = 6;
            // Container child hbox4.Gtk.Box+BoxChild
            this.label137 = new Gtk.Label();
            this.label137.Name = "label137";
            this.hbox4.Add(this.label137);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.hbox4[this.label137]));
            w4.Position = 0;
            w4.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.image20 = new Gtk.Image();
            this.image20.Name = "image20";
            this.image20.Xpad = 6;
            this.image20.Ypad = 6;
            this.image20.Pixbuf = Gdk.Pixbuf.LoadFromResource("ocmgtk.icons.OpenLayers.png");
            this.hbox4.Add(this.image20);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.hbox4[this.image20]));
            w5.Position = 1;
            w5.Expand = false;
            w5.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.label135 = new Gtk.Label();
            this.label135.Name = "label135";
            this.label135.LabelProp = "<span font=\"sans-serif nomal 14\" fgcolor=\"#FFFFFF\">Open Layers</span>";
            this.label135.UseMarkup = true;
            this.hbox4.Add(this.label135);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.hbox4[this.label135]));
            w6.Position = 2;
            w6.Expand = false;
            w6.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.label136 = new Gtk.Label();
            this.label136.Name = "label136";
            this.hbox4.Add(this.label136);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.hbox4[this.label136]));
            w7.Position = 3;
            w7.Fill = false;
            this.eventbox1.Add(this.hbox4);
            this.vbox2.Add(this.eventbox1);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.vbox2[this.eventbox1]));
            w9.PackType = ((Gtk.PackType)(1));
            w9.Position = 1;
            w9.Expand = false;
            w9.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.hseparator1 = new Gtk.HSeparator();
            this.hseparator1.Name = "hseparator1";
            this.vbox2.Add(this.hseparator1);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.vbox2[this.hseparator1]));
            w10.PackType = ((Gtk.PackType)(1));
            w10.Position = 2;
            w10.Expand = false;
            w10.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.pby = new Gtk.Label();
            this.pby.Name = "pby";
            this.pby.Xalign = 0F;
            this.pby.LabelProp = Mono.Unix.Catalog.GetString("Includes:");
            this.vbox2.Add(this.pby);
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(this.vbox2[this.pby]));
            w11.PackType = ((Gtk.PackType)(1));
            w11.Position = 3;
            w11.Expand = false;
            w11.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.versionLabel = new Gtk.Label();
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.LabelProp = "Version 0.16 Alpha";
            this.vbox2.Add(this.versionLabel);
            Gtk.Box.BoxChild w12 = ((Gtk.Box.BoxChild)(this.vbox2[this.versionLabel]));
            w12.PackType = ((Gtk.PackType)(1));
            w12.Position = 4;
            w12.Expand = false;
            w12.Fill = false;
            this.Add(this.vbox2);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 456;
            this.DefaultHeight = 196;
            this.Show();
        }
    }
}
