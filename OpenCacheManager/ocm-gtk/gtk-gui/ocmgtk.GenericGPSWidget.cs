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
    
    
    public partial class GenericGPSWidget {
        
        private Gtk.VBox vbox3;
        
        private Gtk.Table table1;
        
        private Gtk.ComboBox combobox1;
        
        private Gtk.ComboBox combobox2;
        
        private Gtk.Label label1;
        
        private Gtk.Label label2;
        
        private Gtk.CheckButton limitCheck;
        
        private Gtk.Entry limitEntry;
        
        private Gtk.HSeparator hseparator1;
        
        private Gtk.Button hotPlugButton;
        
        private Gtk.Label label4;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Label label5;
        
        private Gtk.Entry formatEntry;
        
        private Gtk.HBox hbox4;
        
        private Gtk.Label label3;
        
        private Gtk.Entry fileEntry;
        
        private Gtk.Button fileButton;
        
        private Gtk.Image image3;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.GenericGPSWidget
            Stetic.BinContainer.Attach(this);
            this.Name = "ocmgtk.GenericGPSWidget";
            // Container child ocmgtk.GenericGPSWidget.Gtk.Container+ContainerChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 6;
            this.vbox3.BorderWidth = ((uint)(6));
            // Container child vbox3.Gtk.Box+BoxChild
            this.table1 = new Gtk.Table(((uint)(3)), ((uint)(2)), false);
            this.table1.Name = "table1";
            this.table1.RowSpacing = ((uint)(6));
            this.table1.ColumnSpacing = ((uint)(6));
            // Container child table1.Gtk.Table+TableChild
            this.combobox1 = Gtk.ComboBox.NewText();
            this.combobox1.AppendText(Mono.Unix.Catalog.GetString("Cache Code"));
            this.combobox1.AppendText(Mono.Unix.Catalog.GetString("Cache Name"));
            this.combobox1.Name = "combobox1";
            this.combobox1.Active = 0;
            this.table1.Add(this.combobox1);
            Gtk.Table.TableChild w1 = ((Gtk.Table.TableChild)(this.table1[this.combobox1]));
            w1.TopAttach = ((uint)(1));
            w1.BottomAttach = ((uint)(2));
            w1.LeftAttach = ((uint)(1));
            w1.RightAttach = ((uint)(2));
            w1.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.combobox2 = Gtk.ComboBox.NewText();
            this.combobox2.AppendText(Mono.Unix.Catalog.GetString("Cache Name"));
            this.combobox2.AppendText(Mono.Unix.Catalog.GetString("Cache Code/Size/Hint"));
            this.combobox2.AppendText(Mono.Unix.Catalog.GetString("Cache Code/Size/Type"));
            this.combobox2.Name = "combobox2";
            this.combobox2.Active = 0;
            this.table1.Add(this.combobox2);
            Gtk.Table.TableChild w2 = ((Gtk.Table.TableChild)(this.table1[this.combobox2]));
            w2.TopAttach = ((uint)(2));
            w2.BottomAttach = ((uint)(3));
            w2.LeftAttach = ((uint)(1));
            w2.RightAttach = ((uint)(2));
            w2.XOptions = ((Gtk.AttachOptions)(4));
            w2.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.Xalign = 0F;
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Waypoint Name Format:");
            this.table1.Add(this.label1);
            Gtk.Table.TableChild w3 = ((Gtk.Table.TableChild)(this.table1[this.label1]));
            w3.TopAttach = ((uint)(1));
            w3.BottomAttach = ((uint)(2));
            w3.XOptions = ((Gtk.AttachOptions)(4));
            w3.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.Xalign = 0F;
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("Waypoint Description Format:");
            this.table1.Add(this.label2);
            Gtk.Table.TableChild w4 = ((Gtk.Table.TableChild)(this.table1[this.label2]));
            w4.TopAttach = ((uint)(2));
            w4.BottomAttach = ((uint)(3));
            w4.XOptions = ((Gtk.AttachOptions)(4));
            w4.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.limitCheck = new Gtk.CheckButton();
            this.limitCheck.CanFocus = true;
            this.limitCheck.Name = "limitCheck";
            this.limitCheck.Label = Mono.Unix.Catalog.GetString("Limit Number of Caches");
            this.limitCheck.DrawIndicator = true;
            this.limitCheck.UseUnderline = true;
            this.table1.Add(this.limitCheck);
            Gtk.Table.TableChild w5 = ((Gtk.Table.TableChild)(this.table1[this.limitCheck]));
            w5.XOptions = ((Gtk.AttachOptions)(4));
            w5.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.limitEntry = new Gtk.Entry();
            this.limitEntry.Sensitive = false;
            this.limitEntry.CanFocus = true;
            this.limitEntry.Name = "limitEntry";
            this.limitEntry.Text = "1000";
            this.limitEntry.IsEditable = true;
            this.limitEntry.InvisibleChar = '•';
            this.table1.Add(this.limitEntry);
            Gtk.Table.TableChild w6 = ((Gtk.Table.TableChild)(this.table1[this.limitEntry]));
            w6.LeftAttach = ((uint)(1));
            w6.RightAttach = ((uint)(2));
            w6.XOptions = ((Gtk.AttachOptions)(4));
            w6.YOptions = ((Gtk.AttachOptions)(4));
            this.vbox3.Add(this.table1);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.vbox3[this.table1]));
            w7.Position = 0;
            w7.Expand = false;
            w7.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.hseparator1 = new Gtk.HSeparator();
            this.hseparator1.Name = "hseparator1";
            this.vbox3.Add(this.hseparator1);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.vbox3[this.hseparator1]));
            w8.Position = 1;
            w8.Expand = false;
            w8.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.hotPlugButton = new Gtk.Button();
            this.hotPlugButton.CanFocus = true;
            this.hotPlugButton.Name = "hotPlugButton";
            this.hotPlugButton.Relief = ((Gtk.ReliefStyle)(2));
            // Container child hotPlugButton.Gtk.Container+ContainerChild
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.Xalign = 0F;
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("Read the GPSBabel Documentation to choose the right values for these fields <span fgcolor=\"blue\">http://www.gpsbabel.org/readme.html</span>");
            this.label4.UseMarkup = true;
            this.label4.Wrap = true;
            this.label4.WidthChars = 50;
            this.hotPlugButton.Add(this.label4);
            this.hotPlugButton.Label = null;
            this.vbox3.Add(this.hotPlugButton);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.vbox3[this.hotPlugButton]));
            w10.Position = 2;
            w10.Expand = false;
            w10.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            // Container child hbox2.Gtk.Box+BoxChild
            this.label5 = new Gtk.Label();
            this.label5.Name = "label5";
            this.label5.LabelProp = Mono.Unix.Catalog.GetString("Output Format:");
            this.hbox2.Add(this.label5);
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(this.hbox2[this.label5]));
            w11.Position = 0;
            w11.Expand = false;
            w11.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.formatEntry = new Gtk.Entry();
            this.formatEntry.CanFocus = true;
            this.formatEntry.Name = "formatEntry";
            this.formatEntry.Text = "geo";
            this.formatEntry.IsEditable = true;
            this.formatEntry.WidthChars = 20;
            this.formatEntry.InvisibleChar = '•';
            this.hbox2.Add(this.formatEntry);
            Gtk.Box.BoxChild w12 = ((Gtk.Box.BoxChild)(this.hbox2[this.formatEntry]));
            w12.Position = 1;
            this.vbox3.Add(this.hbox2);
            Gtk.Box.BoxChild w13 = ((Gtk.Box.BoxChild)(this.vbox3[this.hbox2]));
            w13.Position = 3;
            w13.Expand = false;
            w13.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.hbox4 = new Gtk.HBox();
            this.hbox4.Name = "hbox4";
            this.hbox4.Spacing = 6;
            // Container child hbox4.Gtk.Box+BoxChild
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("Output FIle:");
            this.hbox4.Add(this.label3);
            Gtk.Box.BoxChild w14 = ((Gtk.Box.BoxChild)(this.hbox4[this.label3]));
            w14.Position = 0;
            w14.Expand = false;
            w14.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.fileEntry = new Gtk.Entry();
            this.fileEntry.CanFocus = true;
            this.fileEntry.Name = "fileEntry";
            this.fileEntry.Text = "caches.loc";
            this.fileEntry.IsEditable = true;
            this.fileEntry.InvisibleChar = '•';
            this.hbox4.Add(this.fileEntry);
            Gtk.Box.BoxChild w15 = ((Gtk.Box.BoxChild)(this.hbox4[this.fileEntry]));
            w15.Position = 1;
            // Container child hbox4.Gtk.Box+BoxChild
            this.fileButton = new Gtk.Button();
            this.fileButton.CanFocus = true;
            this.fileButton.Name = "fileButton";
            // Container child fileButton.Gtk.Container+ContainerChild
            this.image3 = new Gtk.Image();
            this.image3.Name = "image3";
            this.image3.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-open", Gtk.IconSize.Menu, 16);
            this.fileButton.Add(this.image3);
            this.fileButton.Label = null;
            this.hbox4.Add(this.fileButton);
            Gtk.Box.BoxChild w17 = ((Gtk.Box.BoxChild)(this.hbox4[this.fileButton]));
            w17.Position = 2;
            w17.Expand = false;
            w17.Fill = false;
            this.vbox3.Add(this.hbox4);
            Gtk.Box.BoxChild w18 = ((Gtk.Box.BoxChild)(this.vbox3[this.hbox4]));
            w18.Position = 4;
            w18.Expand = false;
            w18.Fill = false;
            this.Add(this.vbox3);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Hide();
            this.limitCheck.Toggled += new System.EventHandler(this.OnLimitCheckToggled);
            this.hotPlugButton.Clicked += new System.EventHandler(this.OnInfoClick);
            this.fileButton.Clicked += new System.EventHandler(this.OnFileClick);
        }
    }
}
