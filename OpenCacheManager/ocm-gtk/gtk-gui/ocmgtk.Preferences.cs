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
    
    
    public partial class Preferences {
        
        private Gtk.Notebook notebook1;
        
        private Gtk.Table table2;
        
        private Gtk.Label label1;
        
        private Gtk.Label label2;
        
        private ocmgtk.CoordinateWidget latControl;
        
        private ocmgtk.CoordinateWidget lonControl;
        
        private Gtk.Entry memberId;
        
        private Gtk.Label label6;
        
        private Gtk.Table table1;
        
        private Gtk.CheckButton autoCloseCheck;
        
        private Gtk.Button button937;
        
        private Gtk.Image image43;
        
        private Gtk.Button button938;
        
        private Gtk.Image image44;
        
        private Gtk.Label childLabel;
        
        private Gtk.Entry dataDirEntry;
        
        private Gtk.HSeparator hseparator1;
        
        private Gtk.Entry importDirEntry;
        
        private Gtk.Label label15;
        
        private Gtk.Label label16;
        
        private Gtk.Label label17;
        
        private Gtk.Label label7;
        
        private Gtk.ComboBox modeCombo;
        
        private Gtk.ComboBox startupFilterCombo;
        
        private Gtk.ComboBox unitsCombo;
        
        private Gtk.Label label3;
        
        private Gtk.Table table3;
        
        private Gtk.ComboBox childPointCombo;
        
        private Gtk.Label label12;
        
        private Gtk.Label label13;
        
        private Gtk.Label label4;
        
        private Gtk.Label label5;
        
        private Gtk.Entry mapPointEntry;
        
        private Gtk.ComboBox mapsCombo;
        
        private Gtk.ComboBox nearbyCombo;
        
        private Gtk.Label label14;
        
        private Gtk.Table table4;
        
        private Gtk.Label label11;
        
        private Gtk.CheckButton updateCheck;
        
        private Gtk.Entry updateEntry;
        
        private Gtk.Label label8;
        
        private Gtk.Button buttonCancel;
        
        private Gtk.Button buttonOk;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.Preferences
            this.Name = "ocmgtk.Preferences";
            this.Title = Mono.Unix.Catalog.GetString("Preferences...");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.Modal = true;
            this.BorderWidth = ((uint)(6));
            this.Resizable = false;
            this.AllowGrow = false;
            // Internal child ocmgtk.Preferences.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.notebook1 = new Gtk.Notebook();
            this.notebook1.CanFocus = true;
            this.notebook1.Name = "notebook1";
            this.notebook1.CurrentPage = 1;
            this.notebook1.TabPos = ((Gtk.PositionType)(0));
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.table2 = new Gtk.Table(((uint)(3)), ((uint)(2)), false);
            this.table2.Name = "table2";
            this.table2.RowSpacing = ((uint)(6));
            this.table2.ColumnSpacing = ((uint)(6));
            this.table2.BorderWidth = ((uint)(6));
            // Container child table2.Gtk.Table+TableChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.Xalign = 0F;
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Home Coordinates:");
            this.table2.Add(this.label1);
            Gtk.Table.TableChild w2 = ((Gtk.Table.TableChild)(this.table2[this.label1]));
            w2.XOptions = ((Gtk.AttachOptions)(4));
            w2.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("User name or Account ID:");
            this.table2.Add(this.label2);
            Gtk.Table.TableChild w3 = ((Gtk.Table.TableChild)(this.table2[this.label2]));
            w3.TopAttach = ((uint)(2));
            w3.BottomAttach = ((uint)(3));
            w3.XOptions = ((Gtk.AttachOptions)(4));
            w3.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.latControl = new ocmgtk.CoordinateWidget();
            this.latControl.Events = ((Gdk.EventMask)(256));
            this.latControl.Name = "latControl";
            this.table2.Add(this.latControl);
            Gtk.Table.TableChild w4 = ((Gtk.Table.TableChild)(this.table2[this.latControl]));
            w4.LeftAttach = ((uint)(1));
            w4.RightAttach = ((uint)(2));
            w4.XOptions = ((Gtk.AttachOptions)(4));
            w4.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.lonControl = new ocmgtk.CoordinateWidget();
            this.lonControl.Events = ((Gdk.EventMask)(256));
            this.lonControl.Name = "lonControl";
            this.table2.Add(this.lonControl);
            Gtk.Table.TableChild w5 = ((Gtk.Table.TableChild)(this.table2[this.lonControl]));
            w5.TopAttach = ((uint)(1));
            w5.BottomAttach = ((uint)(2));
            w5.LeftAttach = ((uint)(1));
            w5.RightAttach = ((uint)(2));
            w5.XOptions = ((Gtk.AttachOptions)(4));
            w5.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.memberId = new Gtk.Entry();
            this.memberId.CanFocus = true;
            this.memberId.Name = "memberId";
            this.memberId.IsEditable = true;
            this.memberId.InvisibleChar = '•';
            this.table2.Add(this.memberId);
            Gtk.Table.TableChild w6 = ((Gtk.Table.TableChild)(this.table2[this.memberId]));
            w6.TopAttach = ((uint)(2));
            w6.BottomAttach = ((uint)(3));
            w6.LeftAttach = ((uint)(1));
            w6.RightAttach = ((uint)(2));
            w6.XOptions = ((Gtk.AttachOptions)(4));
            w6.YOptions = ((Gtk.AttachOptions)(4));
            this.notebook1.Add(this.table2);
            // Notebook tab
            this.label6 = new Gtk.Label();
            this.label6.Name = "label6";
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("Home/Account");
            this.notebook1.SetTabLabel(this.table2, this.label6);
            this.label6.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.table1 = new Gtk.Table(((uint)(7)), ((uint)(3)), false);
            this.table1.Name = "table1";
            this.table1.RowSpacing = ((uint)(6));
            this.table1.ColumnSpacing = ((uint)(6));
            this.table1.BorderWidth = ((uint)(6));
            // Container child table1.Gtk.Table+TableChild
            this.autoCloseCheck = new Gtk.CheckButton();
            this.autoCloseCheck.Sensitive = false;
            this.autoCloseCheck.CanFocus = true;
            this.autoCloseCheck.Name = "autoCloseCheck";
            this.autoCloseCheck.Label = Mono.Unix.Catalog.GetString("Don't show import/export summary on completion");
            this.autoCloseCheck.DrawIndicator = true;
            this.autoCloseCheck.UseUnderline = true;
            this.table1.Add(this.autoCloseCheck);
            Gtk.Table.TableChild w8 = ((Gtk.Table.TableChild)(this.table1[this.autoCloseCheck]));
            w8.TopAttach = ((uint)(6));
            w8.BottomAttach = ((uint)(7));
            w8.RightAttach = ((uint)(2));
            w8.XOptions = ((Gtk.AttachOptions)(4));
            w8.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.button937 = new Gtk.Button();
            this.button937.CanFocus = true;
            this.button937.Name = "button937";
            // Container child button937.Gtk.Container+ContainerChild
            this.image43 = new Gtk.Image();
            this.image43.Name = "image43";
            this.image43.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-open", Gtk.IconSize.Button, 20);
            this.button937.Add(this.image43);
            this.button937.Label = null;
            this.table1.Add(this.button937);
            Gtk.Table.TableChild w10 = ((Gtk.Table.TableChild)(this.table1[this.button937]));
            w10.LeftAttach = ((uint)(2));
            w10.RightAttach = ((uint)(3));
            w10.XOptions = ((Gtk.AttachOptions)(4));
            w10.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.button938 = new Gtk.Button();
            this.button938.CanFocus = true;
            this.button938.Name = "button938";
            // Container child button938.Gtk.Container+ContainerChild
            this.image44 = new Gtk.Image();
            this.image44.Name = "image44";
            this.image44.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-open", Gtk.IconSize.Button, 20);
            this.button938.Add(this.image44);
            this.button938.Label = null;
            this.table1.Add(this.button938);
            Gtk.Table.TableChild w12 = ((Gtk.Table.TableChild)(this.table1[this.button938]));
            w12.TopAttach = ((uint)(1));
            w12.BottomAttach = ((uint)(2));
            w12.LeftAttach = ((uint)(2));
            w12.RightAttach = ((uint)(3));
            w12.XOptions = ((Gtk.AttachOptions)(4));
            w12.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.childLabel = new Gtk.Label();
            this.childLabel.Name = "childLabel";
            this.childLabel.Xalign = 0F;
            this.childLabel.LabelProp = Mono.Unix.Catalog.GetString("Child Waypoints:");
            this.table1.Add(this.childLabel);
            Gtk.Table.TableChild w13 = ((Gtk.Table.TableChild)(this.table1[this.childLabel]));
            w13.TopAttach = ((uint)(4));
            w13.BottomAttach = ((uint)(5));
            w13.XOptions = ((Gtk.AttachOptions)(4));
            w13.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.dataDirEntry = new Gtk.Entry();
            this.dataDirEntry.CanFocus = true;
            this.dataDirEntry.Name = "dataDirEntry";
            this.dataDirEntry.IsEditable = true;
            this.dataDirEntry.InvisibleChar = '•';
            this.table1.Add(this.dataDirEntry);
            Gtk.Table.TableChild w14 = ((Gtk.Table.TableChild)(this.table1[this.dataDirEntry]));
            w14.LeftAttach = ((uint)(1));
            w14.RightAttach = ((uint)(2));
            w14.XOptions = ((Gtk.AttachOptions)(4));
            w14.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.hseparator1 = new Gtk.HSeparator();
            this.hseparator1.Name = "hseparator1";
            this.table1.Add(this.hseparator1);
            Gtk.Table.TableChild w15 = ((Gtk.Table.TableChild)(this.table1[this.hseparator1]));
            w15.TopAttach = ((uint)(5));
            w15.BottomAttach = ((uint)(6));
            w15.RightAttach = ((uint)(3));
            w15.XOptions = ((Gtk.AttachOptions)(4));
            w15.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.importDirEntry = new Gtk.Entry();
            this.importDirEntry.CanFocus = true;
            this.importDirEntry.Name = "importDirEntry";
            this.importDirEntry.IsEditable = true;
            this.importDirEntry.InvisibleChar = '•';
            this.table1.Add(this.importDirEntry);
            Gtk.Table.TableChild w16 = ((Gtk.Table.TableChild)(this.table1[this.importDirEntry]));
            w16.TopAttach = ((uint)(1));
            w16.BottomAttach = ((uint)(2));
            w16.LeftAttach = ((uint)(1));
            w16.RightAttach = ((uint)(2));
            w16.XOptions = ((Gtk.AttachOptions)(4));
            w16.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label15 = new Gtk.Label();
            this.label15.Name = "label15";
            this.label15.Xalign = 0F;
            this.label15.LabelProp = Mono.Unix.Catalog.GetString("Startup Filter:");
            this.table1.Add(this.label15);
            Gtk.Table.TableChild w17 = ((Gtk.Table.TableChild)(this.table1[this.label15]));
            w17.TopAttach = ((uint)(2));
            w17.BottomAttach = ((uint)(3));
            w17.XOptions = ((Gtk.AttachOptions)(4));
            w17.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label16 = new Gtk.Label();
            this.label16.Name = "label16";
            this.label16.Xalign = 0F;
            this.label16.LabelProp = Mono.Unix.Catalog.GetString("Database Default Directory:");
            this.table1.Add(this.label16);
            Gtk.Table.TableChild w18 = ((Gtk.Table.TableChild)(this.table1[this.label16]));
            w18.XOptions = ((Gtk.AttachOptions)(4));
            w18.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label17 = new Gtk.Label();
            this.label17.Name = "label17";
            this.label17.Xalign = 0F;
            this.label17.LabelProp = Mono.Unix.Catalog.GetString("Import Default Directory:");
            this.table1.Add(this.label17);
            Gtk.Table.TableChild w19 = ((Gtk.Table.TableChild)(this.table1[this.label17]));
            w19.TopAttach = ((uint)(1));
            w19.BottomAttach = ((uint)(2));
            w19.XOptions = ((Gtk.AttachOptions)(4));
            w19.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label7 = new Gtk.Label();
            this.label7.Name = "label7";
            this.label7.Xalign = 0F;
            this.label7.LabelProp = Mono.Unix.Catalog.GetString("Units:");
            this.table1.Add(this.label7);
            Gtk.Table.TableChild w20 = ((Gtk.Table.TableChild)(this.table1[this.label7]));
            w20.TopAttach = ((uint)(3));
            w20.BottomAttach = ((uint)(4));
            w20.XOptions = ((Gtk.AttachOptions)(4));
            w20.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.modeCombo = Gtk.ComboBox.NewText();
            this.modeCombo.AppendText(Mono.Unix.Catalog.GetString("Prefix with Type (e.g. PKABCD01)"));
            this.modeCombo.AppendText(Mono.Unix.Catalog.GetString("Use GC name (e.g. GCABCD01)"));
            this.modeCombo.Name = "modeCombo";
            this.modeCombo.Active = 0;
            this.table1.Add(this.modeCombo);
            Gtk.Table.TableChild w21 = ((Gtk.Table.TableChild)(this.table1[this.modeCombo]));
            w21.TopAttach = ((uint)(4));
            w21.BottomAttach = ((uint)(5));
            w21.LeftAttach = ((uint)(1));
            w21.RightAttach = ((uint)(2));
            w21.XOptions = ((Gtk.AttachOptions)(4));
            w21.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.startupFilterCombo = Gtk.ComboBox.NewText();
            this.startupFilterCombo.Name = "startupFilterCombo";
            this.table1.Add(this.startupFilterCombo);
            Gtk.Table.TableChild w22 = ((Gtk.Table.TableChild)(this.table1[this.startupFilterCombo]));
            w22.TopAttach = ((uint)(2));
            w22.BottomAttach = ((uint)(3));
            w22.LeftAttach = ((uint)(1));
            w22.RightAttach = ((uint)(2));
            w22.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.unitsCombo = Gtk.ComboBox.NewText();
            this.unitsCombo.AppendText(Mono.Unix.Catalog.GetString("Metric"));
            this.unitsCombo.AppendText(Mono.Unix.Catalog.GetString("U.S./Imperial"));
            this.unitsCombo.Name = "unitsCombo";
            this.unitsCombo.Active = 0;
            this.table1.Add(this.unitsCombo);
            Gtk.Table.TableChild w23 = ((Gtk.Table.TableChild)(this.table1[this.unitsCombo]));
            w23.TopAttach = ((uint)(3));
            w23.BottomAttach = ((uint)(4));
            w23.LeftAttach = ((uint)(1));
            w23.RightAttach = ((uint)(2));
            w23.XOptions = ((Gtk.AttachOptions)(4));
            w23.YOptions = ((Gtk.AttachOptions)(4));
            this.notebook1.Add(this.table1);
            Gtk.Notebook.NotebookChild w24 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.table1]));
            w24.Position = 1;
            // Notebook tab
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("General");
            this.notebook1.SetTabLabel(this.table1, this.label3);
            this.label3.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.table3 = new Gtk.Table(((uint)(4)), ((uint)(2)), false);
            this.table3.Name = "table3";
            this.table3.RowSpacing = ((uint)(6));
            this.table3.ColumnSpacing = ((uint)(6));
            this.table3.BorderWidth = ((uint)(6));
            // Container child table3.Gtk.Table+TableChild
            this.childPointCombo = Gtk.ComboBox.NewText();
            this.childPointCombo.AppendText(Mono.Unix.Catalog.GetString("Selected Cache Only"));
            this.childPointCombo.AppendText(Mono.Unix.Catalog.GetString("All Waypoints"));
            this.childPointCombo.Name = "childPointCombo";
            this.childPointCombo.Active = 0;
            this.table3.Add(this.childPointCombo);
            Gtk.Table.TableChild w25 = ((Gtk.Table.TableChild)(this.table3[this.childPointCombo]));
            w25.TopAttach = ((uint)(2));
            w25.BottomAttach = ((uint)(3));
            w25.LeftAttach = ((uint)(1));
            w25.RightAttach = ((uint)(2));
            w25.XOptions = ((Gtk.AttachOptions)(4));
            w25.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.label12 = new Gtk.Label();
            this.label12.Name = "label12";
            this.label12.Xalign = 0F;
            this.label12.LabelProp = Mono.Unix.Catalog.GetString("Child Waypoints:");
            this.table3.Add(this.label12);
            Gtk.Table.TableChild w26 = ((Gtk.Table.TableChild)(this.table3[this.label12]));
            w26.TopAttach = ((uint)(2));
            w26.BottomAttach = ((uint)(3));
            w26.XOptions = ((Gtk.AttachOptions)(4));
            w26.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.label13 = new Gtk.Label();
            this.label13.Name = "label13";
            this.label13.Xalign = 0F;
            this.label13.LabelProp = Mono.Unix.Catalog.GetString("Caches on Map:");
            this.table3.Add(this.label13);
            Gtk.Table.TableChild w27 = ((Gtk.Table.TableChild)(this.table3[this.label13]));
            w27.TopAttach = ((uint)(3));
            w27.BottomAttach = ((uint)(4));
            w27.XOptions = ((Gtk.AttachOptions)(4));
            w27.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.Xalign = 0F;
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("Default Map:");
            this.table3.Add(this.label4);
            Gtk.Table.TableChild w28 = ((Gtk.Table.TableChild)(this.table3[this.label4]));
            w28.XOptions = ((Gtk.AttachOptions)(4));
            w28.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.label5 = new Gtk.Label();
            this.label5.Name = "label5";
            this.label5.Xalign = 0F;
            this.label5.LabelProp = Mono.Unix.Catalog.GetString("Nearby Caches on Map:");
            this.table3.Add(this.label5);
            Gtk.Table.TableChild w29 = ((Gtk.Table.TableChild)(this.table3[this.label5]));
            w29.TopAttach = ((uint)(1));
            w29.BottomAttach = ((uint)(2));
            w29.XOptions = ((Gtk.AttachOptions)(4));
            w29.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.mapPointEntry = new Gtk.Entry();
            this.mapPointEntry.CanFocus = true;
            this.mapPointEntry.Name = "mapPointEntry";
            this.mapPointEntry.Text = Mono.Unix.Catalog.GetString("100");
            this.mapPointEntry.IsEditable = true;
            this.mapPointEntry.InvisibleChar = '•';
            this.table3.Add(this.mapPointEntry);
            Gtk.Table.TableChild w30 = ((Gtk.Table.TableChild)(this.table3[this.mapPointEntry]));
            w30.TopAttach = ((uint)(3));
            w30.BottomAttach = ((uint)(4));
            w30.LeftAttach = ((uint)(1));
            w30.RightAttach = ((uint)(2));
            w30.XOptions = ((Gtk.AttachOptions)(4));
            w30.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.mapsCombo = Gtk.ComboBox.NewText();
            this.mapsCombo.AppendText(Mono.Unix.Catalog.GetString("Open Street Maps"));
            this.mapsCombo.AppendText(Mono.Unix.Catalog.GetString("Google Hybrid"));
            this.mapsCombo.AppendText(Mono.Unix.Catalog.GetString("Google Street"));
            this.mapsCombo.AppendText(Mono.Unix.Catalog.GetString("Google Terrain"));
            this.mapsCombo.Name = "mapsCombo";
            this.mapsCombo.Active = 0;
            this.table3.Add(this.mapsCombo);
            Gtk.Table.TableChild w31 = ((Gtk.Table.TableChild)(this.table3[this.mapsCombo]));
            w31.LeftAttach = ((uint)(1));
            w31.RightAttach = ((uint)(2));
            w31.XOptions = ((Gtk.AttachOptions)(4));
            w31.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.nearbyCombo = Gtk.ComboBox.NewText();
            this.nearbyCombo.AppendText(Mono.Unix.Catalog.GetString("Enabled on Startup"));
            this.nearbyCombo.AppendText(Mono.Unix.Catalog.GetString("Disabled on Startup"));
            this.nearbyCombo.Name = "nearbyCombo";
            this.nearbyCombo.Active = 0;
            this.table3.Add(this.nearbyCombo);
            Gtk.Table.TableChild w32 = ((Gtk.Table.TableChild)(this.table3[this.nearbyCombo]));
            w32.TopAttach = ((uint)(1));
            w32.BottomAttach = ((uint)(2));
            w32.LeftAttach = ((uint)(1));
            w32.RightAttach = ((uint)(2));
            w32.XOptions = ((Gtk.AttachOptions)(4));
            w32.YOptions = ((Gtk.AttachOptions)(4));
            this.notebook1.Add(this.table3);
            Gtk.Notebook.NotebookChild w33 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.table3]));
            w33.Position = 2;
            // Notebook tab
            this.label14 = new Gtk.Label();
            this.label14.Name = "label14";
            this.label14.LabelProp = Mono.Unix.Catalog.GetString("Map");
            this.notebook1.SetTabLabel(this.table3, this.label14);
            this.label14.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.table4 = new Gtk.Table(((uint)(2)), ((uint)(3)), false);
            this.table4.Name = "table4";
            this.table4.RowSpacing = ((uint)(6));
            this.table4.ColumnSpacing = ((uint)(6));
            this.table4.BorderWidth = ((uint)(6));
            // Container child table4.Gtk.Table+TableChild
            this.label11 = new Gtk.Label();
            this.label11.Name = "label11";
            this.label11.LabelProp = Mono.Unix.Catalog.GetString("days");
            this.table4.Add(this.label11);
            Gtk.Table.TableChild w34 = ((Gtk.Table.TableChild)(this.table4[this.label11]));
            w34.LeftAttach = ((uint)(2));
            w34.RightAttach = ((uint)(3));
            w34.XOptions = ((Gtk.AttachOptions)(4));
            w34.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table4.Gtk.Table+TableChild
            this.updateCheck = new Gtk.CheckButton();
            this.updateCheck.CanFocus = true;
            this.updateCheck.Name = "updateCheck";
            this.updateCheck.Label = Mono.Unix.Catalog.GetString("Check for updates every");
            this.updateCheck.Active = true;
            this.updateCheck.DrawIndicator = true;
            this.updateCheck.UseUnderline = true;
            this.table4.Add(this.updateCheck);
            Gtk.Table.TableChild w35 = ((Gtk.Table.TableChild)(this.table4[this.updateCheck]));
            w35.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table4.Gtk.Table+TableChild
            this.updateEntry = new Gtk.Entry();
            this.updateEntry.CanFocus = true;
            this.updateEntry.Name = "updateEntry";
            this.updateEntry.Text = Mono.Unix.Catalog.GetString("7");
            this.updateEntry.IsEditable = true;
            this.updateEntry.InvisibleChar = '•';
            this.updateEntry.Xalign = 1F;
            this.table4.Add(this.updateEntry);
            Gtk.Table.TableChild w36 = ((Gtk.Table.TableChild)(this.table4[this.updateEntry]));
            w36.LeftAttach = ((uint)(1));
            w36.RightAttach = ((uint)(2));
            w36.YOptions = ((Gtk.AttachOptions)(4));
            this.notebook1.Add(this.table4);
            Gtk.Notebook.NotebookChild w37 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.table4]));
            w37.Position = 3;
            // Notebook tab
            this.label8 = new Gtk.Label();
            this.label8.Name = "label8";
            this.label8.LabelProp = Mono.Unix.Catalog.GetString("Update");
            this.notebook1.SetTabLabel(this.table4, this.label8);
            this.label8.ShowAll();
            w1.Add(this.notebook1);
            Gtk.Box.BoxChild w38 = ((Gtk.Box.BoxChild)(w1[this.notebook1]));
            w38.Position = 0;
            // Internal child ocmgtk.Preferences.ActionArea
            Gtk.HButtonBox w39 = this.ActionArea;
            w39.Name = "dialog1_ActionArea";
            w39.Spacing = 10;
            w39.BorderWidth = ((uint)(5));
            w39.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w40 = ((Gtk.ButtonBox.ButtonBoxChild)(w39[this.buttonCancel]));
            w40.Expand = false;
            w40.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-ok";
            this.AddActionWidget(this.buttonOk, -5);
            Gtk.ButtonBox.ButtonBoxChild w41 = ((Gtk.ButtonBox.ButtonBoxChild)(w39[this.buttonOk]));
            w41.Position = 1;
            w41.Expand = false;
            w41.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 636;
            this.DefaultHeight = 373;
            this.buttonOk.HasDefault = true;
            this.Show();
            this.button938.Clicked += new System.EventHandler(this.OnImpDirClicked);
            this.button937.Clicked += new System.EventHandler(this.OnDBDirClick);
            this.updateCheck.Toggled += new System.EventHandler(this.OnUpdateCheckToggle);
            this.buttonCancel.Clicked += new System.EventHandler(this.OnCancelClicked);
            this.buttonOk.Clicked += new System.EventHandler(this.OnButtonOkClicked);
        }
    }
}
