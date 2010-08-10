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
        
        private Gtk.Table table2;
        
        private Gtk.ComboBox combobox2;
        
        private Gtk.ComboBox combobox3;
        
        private Gtk.Label label2;
        
        private Gtk.Label label3;
        
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
            this.label1.WidthChars = 60;
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
            // Container child vbox2.Gtk.Box+BoxChild
            this.table2 = new Gtk.Table(((uint)(2)), ((uint)(2)), false);
            this.table2.Name = "table2";
            this.table2.RowSpacing = ((uint)(6));
            this.table2.ColumnSpacing = ((uint)(6));
            // Container child table2.Gtk.Table+TableChild
            this.combobox2 = Gtk.ComboBox.NewText();
            this.combobox2.AppendText(Mono.Unix.Catalog.GetString("Metric"));
            this.combobox2.AppendText(Mono.Unix.Catalog.GetString("U.S./Imperial"));
            this.combobox2.Name = "combobox2";
            this.combobox2.Active = 0;
            this.table2.Add(this.combobox2);
            Gtk.Table.TableChild w5 = ((Gtk.Table.TableChild)(this.table2[this.combobox2]));
            w5.LeftAttach = ((uint)(1));
            w5.RightAttach = ((uint)(2));
            w5.XOptions = ((Gtk.AttachOptions)(4));
            w5.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.combobox3 = Gtk.ComboBox.NewText();
            this.combobox3.AppendText(Mono.Unix.Catalog.GetString("Open Street Maps"));
            this.combobox3.AppendText(Mono.Unix.Catalog.GetString("Google Hybrid"));
            this.combobox3.AppendText(Mono.Unix.Catalog.GetString("Google Terrain"));
            this.combobox3.AppendText(Mono.Unix.Catalog.GetString("Google Street Maps"));
            this.combobox3.Name = "combobox3";
            this.combobox3.Active = 0;
            this.table2.Add(this.combobox3);
            Gtk.Table.TableChild w6 = ((Gtk.Table.TableChild)(this.table2[this.combobox3]));
            w6.TopAttach = ((uint)(1));
            w6.BottomAttach = ((uint)(2));
            w6.LeftAttach = ((uint)(1));
            w6.RightAttach = ((uint)(2));
            w6.XOptions = ((Gtk.AttachOptions)(4));
            w6.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.Xalign = 0F;
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("OCM should report distances in ");
            this.table2.Add(this.label2);
            Gtk.Table.TableChild w7 = ((Gtk.Table.TableChild)(this.table2[this.label2]));
            w7.XOptions = ((Gtk.AttachOptions)(4));
            w7.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.Xalign = 0F;
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("Default map that OCM should use");
            this.table2.Add(this.label3);
            Gtk.Table.TableChild w8 = ((Gtk.Table.TableChild)(this.table2[this.label3]));
            w8.TopAttach = ((uint)(1));
            w8.BottomAttach = ((uint)(2));
            w8.XOptions = ((Gtk.AttachOptions)(4));
            w8.YOptions = ((Gtk.AttachOptions)(4));
            this.vbox2.Add(this.table2);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.vbox2[this.table2]));
            w9.Position = 2;
            w9.Expand = false;
            w9.Fill = false;
            this.Add(this.vbox2);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Hide();
            this.openButton.Clicked += new System.EventHandler(this.OnOpenClicked);
        }
    }
}
