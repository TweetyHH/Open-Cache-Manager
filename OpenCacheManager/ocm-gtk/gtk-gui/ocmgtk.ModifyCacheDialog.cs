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
    
    
    public partial class ModifyCacheDialog {
        
        private Gtk.Notebook notebook1;
        
        private Gtk.Table table2;
        
        private Gtk.Entry codeEntry;
        
        private Gtk.ScrolledWindow GtkScrolledWindow;
        
        private Gtk.TextView descriptionEntry;
        
        private Gtk.Label label11;
        
        private Gtk.Label label12;
        
        private Gtk.Label label4;
        
        private Gtk.Label label5;
        
        private Gtk.Label label6;
        
        private Gtk.Label label7;
        
        private Gtk.Label label8;
        
        private ocmgtk.CoordinateWidget latEntry;
        
        private ocmgtk.CoordinateWidget lonEntry;
        
        private Gtk.ComboBox typeEntry;
        
        private Gtk.Entry urlEntry;
        
        private Gtk.Entry urlNameEntry;
        
        private Gtk.Label label2;
        
        private Gtk.Table table3;
        
        private Gtk.ComboBox diffEntry;
        
        private Gtk.ScrolledWindow GtkScrolledWindow1;
        
        private Gtk.TextView shortDescEntry;
        
        private Gtk.ScrolledWindow GtkScrolledWindow2;
        
        private Gtk.TextView longDescEntry;
        
        private Gtk.ScrolledWindow GtkScrolledWindow3;
        
        private Gtk.TextView hintEntry;
        
        private Gtk.Label label13;
        
        private Gtk.Label label14;
        
        private Gtk.Label label15;
        
        private Gtk.Label label16;
        
        private Gtk.Label label17;
        
        private Gtk.Label label18;
        
        private Gtk.Entry nameEntry;
        
        private Gtk.ComboBox terrEntry;
        
        private Gtk.Label name;
        
        private Gtk.Table table1;
        
        private Gtk.Entry cacheIDEntry;
        
        private Gtk.Label label19;
        
        private Gtk.Label label20;
        
        private Gtk.Label label21;
        
        private Gtk.Label label22;
        
        private Gtk.Entry ownerEntry;
        
        private Gtk.Entry ownerIDEntry;
        
        private Gtk.Entry placedByEntry;
        
        private Gtk.Label label1;
        
        private Gtk.Button buttonCancel;
        
        private Gtk.Button buttonOk;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.ModifyCacheDialog
            this.Name = "ocmgtk.ModifyCacheDialog";
            this.Title = Mono.Unix.Catalog.GetString("Add/Modify Geocache...");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.BorderWidth = ((uint)(6));
            // Internal child ocmgtk.ModifyCacheDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.notebook1 = new Gtk.Notebook();
            this.notebook1.CanFocus = true;
            this.notebook1.Name = "notebook1";
            this.notebook1.CurrentPage = 0;
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.table2 = new Gtk.Table(((uint)(7)), ((uint)(2)), false);
            this.table2.Name = "table2";
            this.table2.RowSpacing = ((uint)(6));
            this.table2.ColumnSpacing = ((uint)(6));
            this.table2.BorderWidth = ((uint)(6));
            // Container child table2.Gtk.Table+TableChild
            this.codeEntry = new Gtk.Entry();
            this.codeEntry.CanFocus = true;
            this.codeEntry.Name = "codeEntry";
            this.codeEntry.IsEditable = true;
            this.codeEntry.InvisibleChar = '•';
            this.table2.Add(this.codeEntry);
            Gtk.Table.TableChild w2 = ((Gtk.Table.TableChild)(this.table2[this.codeEntry]));
            w2.LeftAttach = ((uint)(1));
            w2.RightAttach = ((uint)(2));
            w2.XOptions = ((Gtk.AttachOptions)(4));
            w2.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.GtkScrolledWindow = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow.Name = "GtkScrolledWindow";
            this.GtkScrolledWindow.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow.Gtk.Container+ContainerChild
            this.descriptionEntry = new Gtk.TextView();
            this.descriptionEntry.CanFocus = true;
            this.descriptionEntry.Name = "descriptionEntry";
            this.GtkScrolledWindow.Add(this.descriptionEntry);
            this.table2.Add(this.GtkScrolledWindow);
            Gtk.Table.TableChild w4 = ((Gtk.Table.TableChild)(this.table2[this.GtkScrolledWindow]));
            w4.TopAttach = ((uint)(6));
            w4.BottomAttach = ((uint)(7));
            w4.LeftAttach = ((uint)(1));
            w4.RightAttach = ((uint)(2));
            w4.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label11 = new Gtk.Label();
            this.label11.Name = "label11";
            this.label11.Xalign = 0F;
            this.label11.LabelProp = Mono.Unix.Catalog.GetString("URL:");
            this.table2.Add(this.label11);
            Gtk.Table.TableChild w5 = ((Gtk.Table.TableChild)(this.table2[this.label11]));
            w5.TopAttach = ((uint)(4));
            w5.BottomAttach = ((uint)(5));
            w5.XOptions = ((Gtk.AttachOptions)(4));
            w5.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label12 = new Gtk.Label();
            this.label12.Name = "label12";
            this.label12.Xalign = 0F;
            this.label12.LabelProp = Mono.Unix.Catalog.GetString("URL Name:");
            this.table2.Add(this.label12);
            Gtk.Table.TableChild w6 = ((Gtk.Table.TableChild)(this.table2[this.label12]));
            w6.TopAttach = ((uint)(5));
            w6.BottomAttach = ((uint)(6));
            w6.XOptions = ((Gtk.AttachOptions)(4));
            w6.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.Xalign = 0F;
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("Latitude:");
            this.table2.Add(this.label4);
            Gtk.Table.TableChild w7 = ((Gtk.Table.TableChild)(this.table2[this.label4]));
            w7.TopAttach = ((uint)(1));
            w7.BottomAttach = ((uint)(2));
            w7.XOptions = ((Gtk.AttachOptions)(4));
            w7.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label5 = new Gtk.Label();
            this.label5.Name = "label5";
            this.label5.Xalign = 0F;
            this.label5.LabelProp = Mono.Unix.Catalog.GetString("Longitude:");
            this.table2.Add(this.label5);
            Gtk.Table.TableChild w8 = ((Gtk.Table.TableChild)(this.table2[this.label5]));
            w8.TopAttach = ((uint)(2));
            w8.BottomAttach = ((uint)(3));
            w8.XOptions = ((Gtk.AttachOptions)(4));
            w8.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label6 = new Gtk.Label();
            this.label6.Name = "label6";
            this.label6.Ypad = 6;
            this.label6.Xalign = 0F;
            this.label6.Yalign = 0F;
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("Description:");
            this.table2.Add(this.label6);
            Gtk.Table.TableChild w9 = ((Gtk.Table.TableChild)(this.table2[this.label6]));
            w9.TopAttach = ((uint)(6));
            w9.BottomAttach = ((uint)(7));
            w9.XOptions = ((Gtk.AttachOptions)(4));
            w9.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label7 = new Gtk.Label();
            this.label7.Name = "label7";
            this.label7.Xalign = 0F;
            this.label7.LabelProp = Mono.Unix.Catalog.GetString("Cache Type:");
            this.table2.Add(this.label7);
            Gtk.Table.TableChild w10 = ((Gtk.Table.TableChild)(this.table2[this.label7]));
            w10.TopAttach = ((uint)(3));
            w10.BottomAttach = ((uint)(4));
            w10.XOptions = ((Gtk.AttachOptions)(4));
            w10.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label8 = new Gtk.Label();
            this.label8.Name = "label8";
            this.label8.Xalign = 0F;
            this.label8.LabelProp = Mono.Unix.Catalog.GetString("Cache Code:");
            this.table2.Add(this.label8);
            Gtk.Table.TableChild w11 = ((Gtk.Table.TableChild)(this.table2[this.label8]));
            w11.XOptions = ((Gtk.AttachOptions)(4));
            w11.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.latEntry = new ocmgtk.CoordinateWidget();
            this.latEntry.Events = ((Gdk.EventMask)(256));
            this.latEntry.Name = "latEntry";
            this.table2.Add(this.latEntry);
            Gtk.Table.TableChild w12 = ((Gtk.Table.TableChild)(this.table2[this.latEntry]));
            w12.TopAttach = ((uint)(1));
            w12.BottomAttach = ((uint)(2));
            w12.LeftAttach = ((uint)(1));
            w12.RightAttach = ((uint)(2));
            w12.XOptions = ((Gtk.AttachOptions)(4));
            w12.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.lonEntry = new ocmgtk.CoordinateWidget();
            this.lonEntry.Events = ((Gdk.EventMask)(256));
            this.lonEntry.Name = "lonEntry";
            this.table2.Add(this.lonEntry);
            Gtk.Table.TableChild w13 = ((Gtk.Table.TableChild)(this.table2[this.lonEntry]));
            w13.TopAttach = ((uint)(2));
            w13.BottomAttach = ((uint)(3));
            w13.LeftAttach = ((uint)(1));
            w13.RightAttach = ((uint)(2));
            w13.XOptions = ((Gtk.AttachOptions)(4));
            w13.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.typeEntry = Gtk.ComboBox.NewText();
            this.typeEntry.AppendText(Mono.Unix.Catalog.GetString("Generic"));
            this.typeEntry.AppendText(Mono.Unix.Catalog.GetString("Traditional Cache"));
            this.typeEntry.AppendText(Mono.Unix.Catalog.GetString("Unknown Cache"));
            this.typeEntry.AppendText(Mono.Unix.Catalog.GetString("Multi-Cache"));
            this.typeEntry.AppendText(Mono.Unix.Catalog.GetString("Earthcache"));
            this.typeEntry.AppendText(Mono.Unix.Catalog.GetString("Eventcache"));
            this.typeEntry.AppendText(Mono.Unix.Catalog.GetString("Letterbox Hybrid"));
            this.typeEntry.AppendText(Mono.Unix.Catalog.GetString("Project A.P.E Cache"));
            this.typeEntry.AppendText(Mono.Unix.Catalog.GetString("GPS Adventures Exhibit"));
            this.typeEntry.AppendText(Mono.Unix.Catalog.GetString("Locationless Cache"));
            this.typeEntry.AppendText(Mono.Unix.Catalog.GetString("Virtual Cache"));
            this.typeEntry.AppendText(Mono.Unix.Catalog.GetString("Webcam Cache"));
            this.typeEntry.AppendText(Mono.Unix.Catalog.GetString("Wheigo Cache"));
            this.typeEntry.AppendText(Mono.Unix.Catalog.GetString("Cache In Trash Out Event"));
            this.typeEntry.AppendText(Mono.Unix.Catalog.GetString("Mega Event Cache"));
            this.typeEntry.Name = "typeEntry";
            this.typeEntry.Active = 0;
            this.table2.Add(this.typeEntry);
            Gtk.Table.TableChild w14 = ((Gtk.Table.TableChild)(this.table2[this.typeEntry]));
            w14.TopAttach = ((uint)(3));
            w14.BottomAttach = ((uint)(4));
            w14.LeftAttach = ((uint)(1));
            w14.RightAttach = ((uint)(2));
            w14.XOptions = ((Gtk.AttachOptions)(4));
            w14.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.urlEntry = new Gtk.Entry();
            this.urlEntry.CanFocus = true;
            this.urlEntry.Name = "urlEntry";
            this.urlEntry.IsEditable = true;
            this.urlEntry.InvisibleChar = '•';
            this.table2.Add(this.urlEntry);
            Gtk.Table.TableChild w15 = ((Gtk.Table.TableChild)(this.table2[this.urlEntry]));
            w15.TopAttach = ((uint)(4));
            w15.BottomAttach = ((uint)(5));
            w15.LeftAttach = ((uint)(1));
            w15.RightAttach = ((uint)(2));
            w15.XOptions = ((Gtk.AttachOptions)(4));
            w15.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.urlNameEntry = new Gtk.Entry();
            this.urlNameEntry.CanFocus = true;
            this.urlNameEntry.Name = "urlNameEntry";
            this.urlNameEntry.IsEditable = true;
            this.urlNameEntry.InvisibleChar = '•';
            this.table2.Add(this.urlNameEntry);
            Gtk.Table.TableChild w16 = ((Gtk.Table.TableChild)(this.table2[this.urlNameEntry]));
            w16.TopAttach = ((uint)(5));
            w16.BottomAttach = ((uint)(6));
            w16.LeftAttach = ((uint)(1));
            w16.RightAttach = ((uint)(2));
            w16.XOptions = ((Gtk.AttachOptions)(4));
            w16.YOptions = ((Gtk.AttachOptions)(4));
            this.notebook1.Add(this.table2);
            // Notebook tab
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("Waypoint Properties");
            this.notebook1.SetTabLabel(this.table2, this.label2);
            this.label2.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.table3 = new Gtk.Table(((uint)(6)), ((uint)(2)), false);
            this.table3.Name = "table3";
            this.table3.RowSpacing = ((uint)(6));
            this.table3.ColumnSpacing = ((uint)(6));
            this.table3.BorderWidth = ((uint)(6));
            // Container child table3.Gtk.Table+TableChild
            this.diffEntry = Gtk.ComboBox.NewText();
            this.diffEntry.AppendText(Mono.Unix.Catalog.GetString("1"));
            this.diffEntry.AppendText(Mono.Unix.Catalog.GetString("1.5"));
            this.diffEntry.AppendText(Mono.Unix.Catalog.GetString("2"));
            this.diffEntry.AppendText(Mono.Unix.Catalog.GetString("2.5"));
            this.diffEntry.AppendText(Mono.Unix.Catalog.GetString("3"));
            this.diffEntry.AppendText(Mono.Unix.Catalog.GetString("3.5"));
            this.diffEntry.AppendText(Mono.Unix.Catalog.GetString("4"));
            this.diffEntry.AppendText(Mono.Unix.Catalog.GetString("4.5"));
            this.diffEntry.AppendText(Mono.Unix.Catalog.GetString("5"));
            this.diffEntry.Name = "diffEntry";
            this.diffEntry.Active = 0;
            this.table3.Add(this.diffEntry);
            Gtk.Table.TableChild w18 = ((Gtk.Table.TableChild)(this.table3[this.diffEntry]));
            w18.TopAttach = ((uint)(1));
            w18.BottomAttach = ((uint)(2));
            w18.LeftAttach = ((uint)(1));
            w18.RightAttach = ((uint)(2));
            w18.XOptions = ((Gtk.AttachOptions)(4));
            w18.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.GtkScrolledWindow1 = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
            this.GtkScrolledWindow1.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
            this.shortDescEntry = new Gtk.TextView();
            this.shortDescEntry.CanFocus = true;
            this.shortDescEntry.Name = "shortDescEntry";
            this.shortDescEntry.WrapMode = ((Gtk.WrapMode)(2));
            this.GtkScrolledWindow1.Add(this.shortDescEntry);
            this.table3.Add(this.GtkScrolledWindow1);
            Gtk.Table.TableChild w20 = ((Gtk.Table.TableChild)(this.table3[this.GtkScrolledWindow1]));
            w20.TopAttach = ((uint)(3));
            w20.BottomAttach = ((uint)(4));
            w20.LeftAttach = ((uint)(1));
            w20.RightAttach = ((uint)(2));
            w20.XOptions = ((Gtk.AttachOptions)(4));
            w20.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.GtkScrolledWindow2 = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow2.Name = "GtkScrolledWindow2";
            this.GtkScrolledWindow2.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow2.Gtk.Container+ContainerChild
            this.longDescEntry = new Gtk.TextView();
            this.longDescEntry.CanFocus = true;
            this.longDescEntry.Name = "longDescEntry";
            this.longDescEntry.WrapMode = ((Gtk.WrapMode)(2));
            this.GtkScrolledWindow2.Add(this.longDescEntry);
            this.table3.Add(this.GtkScrolledWindow2);
            Gtk.Table.TableChild w22 = ((Gtk.Table.TableChild)(this.table3[this.GtkScrolledWindow2]));
            w22.TopAttach = ((uint)(4));
            w22.BottomAttach = ((uint)(5));
            w22.LeftAttach = ((uint)(1));
            w22.RightAttach = ((uint)(2));
            w22.XOptions = ((Gtk.AttachOptions)(4));
            w22.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.GtkScrolledWindow3 = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow3.Name = "GtkScrolledWindow3";
            this.GtkScrolledWindow3.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow3.Gtk.Container+ContainerChild
            this.hintEntry = new Gtk.TextView();
            this.hintEntry.CanFocus = true;
            this.hintEntry.Name = "hintEntry";
            this.hintEntry.WrapMode = ((Gtk.WrapMode)(2));
            this.GtkScrolledWindow3.Add(this.hintEntry);
            this.table3.Add(this.GtkScrolledWindow3);
            Gtk.Table.TableChild w24 = ((Gtk.Table.TableChild)(this.table3[this.GtkScrolledWindow3]));
            w24.TopAttach = ((uint)(5));
            w24.BottomAttach = ((uint)(6));
            w24.LeftAttach = ((uint)(1));
            w24.RightAttach = ((uint)(2));
            w24.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.label13 = new Gtk.Label();
            this.label13.Name = "label13";
            this.label13.Xalign = 0F;
            this.label13.LabelProp = Mono.Unix.Catalog.GetString("Cache Name:");
            this.table3.Add(this.label13);
            Gtk.Table.TableChild w25 = ((Gtk.Table.TableChild)(this.table3[this.label13]));
            w25.XOptions = ((Gtk.AttachOptions)(4));
            w25.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.label14 = new Gtk.Label();
            this.label14.Name = "label14";
            this.label14.Xalign = 0F;
            this.label14.LabelProp = Mono.Unix.Catalog.GetString("Difficulty:");
            this.table3.Add(this.label14);
            Gtk.Table.TableChild w26 = ((Gtk.Table.TableChild)(this.table3[this.label14]));
            w26.TopAttach = ((uint)(1));
            w26.BottomAttach = ((uint)(2));
            w26.XOptions = ((Gtk.AttachOptions)(4));
            w26.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.label15 = new Gtk.Label();
            this.label15.Name = "label15";
            this.label15.Xalign = 0F;
            this.label15.LabelProp = Mono.Unix.Catalog.GetString("Terrain:");
            this.table3.Add(this.label15);
            Gtk.Table.TableChild w27 = ((Gtk.Table.TableChild)(this.table3[this.label15]));
            w27.TopAttach = ((uint)(2));
            w27.BottomAttach = ((uint)(3));
            w27.XOptions = ((Gtk.AttachOptions)(4));
            w27.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.label16 = new Gtk.Label();
            this.label16.Name = "label16";
            this.label16.Ypad = 6;
            this.label16.Xalign = 0F;
            this.label16.Yalign = 0F;
            this.label16.LabelProp = Mono.Unix.Catalog.GetString("Short Description:");
            this.table3.Add(this.label16);
            Gtk.Table.TableChild w28 = ((Gtk.Table.TableChild)(this.table3[this.label16]));
            w28.TopAttach = ((uint)(3));
            w28.BottomAttach = ((uint)(4));
            w28.XOptions = ((Gtk.AttachOptions)(4));
            w28.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.label17 = new Gtk.Label();
            this.label17.Name = "label17";
            this.label17.Ypad = 6;
            this.label17.Xalign = 0F;
            this.label17.Yalign = 0F;
            this.label17.LabelProp = Mono.Unix.Catalog.GetString("Long Description:");
            this.table3.Add(this.label17);
            Gtk.Table.TableChild w29 = ((Gtk.Table.TableChild)(this.table3[this.label17]));
            w29.TopAttach = ((uint)(4));
            w29.BottomAttach = ((uint)(5));
            w29.XOptions = ((Gtk.AttachOptions)(4));
            w29.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.label18 = new Gtk.Label();
            this.label18.Name = "label18";
            this.label18.Ypad = 6;
            this.label18.Xalign = 0F;
            this.label18.Yalign = 0F;
            this.label18.LabelProp = Mono.Unix.Catalog.GetString("Hint:");
            this.table3.Add(this.label18);
            Gtk.Table.TableChild w30 = ((Gtk.Table.TableChild)(this.table3[this.label18]));
            w30.TopAttach = ((uint)(5));
            w30.BottomAttach = ((uint)(6));
            w30.XOptions = ((Gtk.AttachOptions)(4));
            w30.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.nameEntry = new Gtk.Entry();
            this.nameEntry.CanFocus = true;
            this.nameEntry.Name = "nameEntry";
            this.nameEntry.IsEditable = true;
            this.nameEntry.InvisibleChar = '•';
            this.table3.Add(this.nameEntry);
            Gtk.Table.TableChild w31 = ((Gtk.Table.TableChild)(this.table3[this.nameEntry]));
            w31.LeftAttach = ((uint)(1));
            w31.RightAttach = ((uint)(2));
            w31.XOptions = ((Gtk.AttachOptions)(4));
            w31.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.terrEntry = Gtk.ComboBox.NewText();
            this.terrEntry.AppendText(Mono.Unix.Catalog.GetString("1"));
            this.terrEntry.AppendText(Mono.Unix.Catalog.GetString("1.5"));
            this.terrEntry.AppendText(Mono.Unix.Catalog.GetString("2"));
            this.terrEntry.AppendText(Mono.Unix.Catalog.GetString("2.5"));
            this.terrEntry.AppendText(Mono.Unix.Catalog.GetString("3"));
            this.terrEntry.AppendText(Mono.Unix.Catalog.GetString("3.5"));
            this.terrEntry.AppendText(Mono.Unix.Catalog.GetString("4"));
            this.terrEntry.AppendText(Mono.Unix.Catalog.GetString("4.5"));
            this.terrEntry.AppendText(Mono.Unix.Catalog.GetString("5"));
            this.terrEntry.Name = "terrEntry";
            this.terrEntry.Active = 0;
            this.table3.Add(this.terrEntry);
            Gtk.Table.TableChild w32 = ((Gtk.Table.TableChild)(this.table3[this.terrEntry]));
            w32.TopAttach = ((uint)(2));
            w32.BottomAttach = ((uint)(3));
            w32.LeftAttach = ((uint)(1));
            w32.RightAttach = ((uint)(2));
            w32.XOptions = ((Gtk.AttachOptions)(4));
            w32.YOptions = ((Gtk.AttachOptions)(4));
            this.notebook1.Add(this.table3);
            Gtk.Notebook.NotebookChild w33 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.table3]));
            w33.Position = 1;
            // Notebook tab
            this.name = new Gtk.Label();
            this.name.Name = "name";
            this.name.LabelProp = Mono.Unix.Catalog.GetString("Cache Details");
            this.notebook1.SetTabLabel(this.table3, this.name);
            this.name.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.table1 = new Gtk.Table(((uint)(5)), ((uint)(2)), false);
            this.table1.Name = "table1";
            this.table1.RowSpacing = ((uint)(6));
            this.table1.ColumnSpacing = ((uint)(6));
            this.table1.BorderWidth = ((uint)(6));
            // Container child table1.Gtk.Table+TableChild
            this.cacheIDEntry = new Gtk.Entry();
            this.cacheIDEntry.CanFocus = true;
            this.cacheIDEntry.Name = "cacheIDEntry";
            this.cacheIDEntry.IsEditable = true;
            this.cacheIDEntry.InvisibleChar = '•';
            this.table1.Add(this.cacheIDEntry);
            Gtk.Table.TableChild w34 = ((Gtk.Table.TableChild)(this.table1[this.cacheIDEntry]));
            w34.TopAttach = ((uint)(3));
            w34.BottomAttach = ((uint)(4));
            w34.LeftAttach = ((uint)(1));
            w34.RightAttach = ((uint)(2));
            w34.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label19 = new Gtk.Label();
            this.label19.Name = "label19";
            this.label19.Xalign = 0F;
            this.label19.LabelProp = Mono.Unix.Catalog.GetString("Owner:");
            this.table1.Add(this.label19);
            Gtk.Table.TableChild w35 = ((Gtk.Table.TableChild)(this.table1[this.label19]));
            w35.XOptions = ((Gtk.AttachOptions)(4));
            w35.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label20 = new Gtk.Label();
            this.label20.Name = "label20";
            this.label20.Xalign = 0F;
            this.label20.LabelProp = Mono.Unix.Catalog.GetString("Owner ID:");
            this.table1.Add(this.label20);
            Gtk.Table.TableChild w36 = ((Gtk.Table.TableChild)(this.table1[this.label20]));
            w36.TopAttach = ((uint)(1));
            w36.BottomAttach = ((uint)(2));
            w36.XOptions = ((Gtk.AttachOptions)(4));
            w36.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label21 = new Gtk.Label();
            this.label21.Name = "label21";
            this.label21.Xalign = 0F;
            this.label21.LabelProp = Mono.Unix.Catalog.GetString("Placed By:");
            this.table1.Add(this.label21);
            Gtk.Table.TableChild w37 = ((Gtk.Table.TableChild)(this.table1[this.label21]));
            w37.TopAttach = ((uint)(2));
            w37.BottomAttach = ((uint)(3));
            w37.XOptions = ((Gtk.AttachOptions)(4));
            w37.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label22 = new Gtk.Label();
            this.label22.Name = "label22";
            this.label22.Xalign = 0F;
            this.label22.LabelProp = Mono.Unix.Catalog.GetString("Cache ID:");
            this.table1.Add(this.label22);
            Gtk.Table.TableChild w38 = ((Gtk.Table.TableChild)(this.table1[this.label22]));
            w38.TopAttach = ((uint)(3));
            w38.BottomAttach = ((uint)(4));
            w38.XOptions = ((Gtk.AttachOptions)(4));
            w38.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.ownerEntry = new Gtk.Entry();
            this.ownerEntry.CanFocus = true;
            this.ownerEntry.Name = "ownerEntry";
            this.ownerEntry.IsEditable = true;
            this.ownerEntry.InvisibleChar = '•';
            this.table1.Add(this.ownerEntry);
            Gtk.Table.TableChild w39 = ((Gtk.Table.TableChild)(this.table1[this.ownerEntry]));
            w39.LeftAttach = ((uint)(1));
            w39.RightAttach = ((uint)(2));
            w39.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.ownerIDEntry = new Gtk.Entry();
            this.ownerIDEntry.CanFocus = true;
            this.ownerIDEntry.Name = "ownerIDEntry";
            this.ownerIDEntry.IsEditable = true;
            this.ownerIDEntry.InvisibleChar = '•';
            this.table1.Add(this.ownerIDEntry);
            Gtk.Table.TableChild w40 = ((Gtk.Table.TableChild)(this.table1[this.ownerIDEntry]));
            w40.TopAttach = ((uint)(1));
            w40.BottomAttach = ((uint)(2));
            w40.LeftAttach = ((uint)(1));
            w40.RightAttach = ((uint)(2));
            w40.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.placedByEntry = new Gtk.Entry();
            this.placedByEntry.CanFocus = true;
            this.placedByEntry.Name = "placedByEntry";
            this.placedByEntry.IsEditable = true;
            this.placedByEntry.InvisibleChar = '•';
            this.table1.Add(this.placedByEntry);
            Gtk.Table.TableChild w41 = ((Gtk.Table.TableChild)(this.table1[this.placedByEntry]));
            w41.TopAttach = ((uint)(2));
            w41.BottomAttach = ((uint)(3));
            w41.LeftAttach = ((uint)(1));
            w41.RightAttach = ((uint)(2));
            w41.YOptions = ((Gtk.AttachOptions)(4));
            this.notebook1.Add(this.table1);
            Gtk.Notebook.NotebookChild w42 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.table1]));
            w42.Position = 2;
            // Notebook tab
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Other Properties");
            this.notebook1.SetTabLabel(this.table1, this.label1);
            this.label1.ShowAll();
            w1.Add(this.notebook1);
            Gtk.Box.BoxChild w43 = ((Gtk.Box.BoxChild)(w1[this.notebook1]));
            w43.Position = 0;
            w43.Padding = ((uint)(6));
            // Internal child ocmgtk.ModifyCacheDialog.ActionArea
            Gtk.HButtonBox w44 = this.ActionArea;
            w44.Name = "dialog1_ActionArea";
            w44.Spacing = 10;
            w44.BorderWidth = ((uint)(5));
            w44.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w45 = ((Gtk.ButtonBox.ButtonBoxChild)(w44[this.buttonCancel]));
            w45.Expand = false;
            w45.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-ok";
            this.AddActionWidget(this.buttonOk, -5);
            Gtk.ButtonBox.ButtonBoxChild w46 = ((Gtk.ButtonBox.ButtonBoxChild)(w44[this.buttonOk]));
            w46.Position = 1;
            w46.Expand = false;
            w46.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 672;
            this.DefaultHeight = 520;
            this.Show();
            this.buttonCancel.Clicked += new System.EventHandler(this.OnCancelClicked);
            this.buttonOk.Clicked += new System.EventHandler(this.OnButtonOkClicked);
        }
    }
}
