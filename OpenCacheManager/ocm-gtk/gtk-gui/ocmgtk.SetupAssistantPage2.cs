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
    
    
    public partial class SetupAssistantPage2 {
        
        private Gtk.VBox vbox2;
        
        private Gtk.Label label1;
        
        private Gtk.HBox hbox1;
        
        private Gtk.Entry dbEntry;
        
        private Gtk.Button openButton;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.SetupAssistantPage2
            Stetic.BinContainer.Attach(this);
            this.Name = "ocmgtk.SetupAssistantPage2";
            // Container child ocmgtk.SetupAssistantPage2.Gtk.Container+ContainerChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            this.vbox2.BorderWidth = ((uint)(6));
            // Container child vbox2.Gtk.Box+BoxChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.Xalign = 0F;
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("You will need to create a database to use OCM, or open an existing OCM database if you already have one from somewhere else. ");
            this.label1.Wrap = true;
            this.vbox2.Add(this.label1);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.vbox2[this.label1]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.dbEntry = new Gtk.Entry();
            this.dbEntry.CanFocus = true;
            this.dbEntry.Name = "dbEntry";
            this.dbEntry.IsEditable = true;
            this.dbEntry.InvisibleChar = '•';
            this.hbox1.Add(this.dbEntry);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.hbox1[this.dbEntry]));
            w2.Position = 0;
            // Container child hbox1.Gtk.Box+BoxChild
            this.openButton = new Gtk.Button();
            this.openButton.CanFocus = true;
            this.openButton.Name = "openButton";
            this.openButton.UseStock = true;
            this.openButton.UseUnderline = true;
            this.openButton.Label = "gtk-open";
            this.hbox1.Add(this.openButton);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox1[this.openButton]));
            w3.Position = 1;
            w3.Expand = false;
            w3.Fill = false;
            this.vbox2.Add(this.hbox1);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.vbox2[this.hbox1]));
            w4.Position = 1;
            w4.Expand = false;
            w4.Fill = false;
            this.Add(this.vbox2);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Hide();
            this.openButton.Clicked += new System.EventHandler(this.OnOpenClicked);
        }
    }
}