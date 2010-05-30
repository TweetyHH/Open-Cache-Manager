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
    
    
    public partial class CacheList {
        
        private Gtk.UIManager UIManager;
        
        private Gtk.Action FilterAction;
        
        private Gtk.VBox vbox2;
        
        private Gtk.HBox hbox3;
        
        private Gtk.Image image19;
        
        private Gtk.Entry filterEntry;
        
        private Gtk.Button button154;
        
        private Gtk.Image image17;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Image image23;
        
        private Gtk.Entry distanceEntry;
        
        private Gtk.Label label1;
        
        private Gtk.Button button136;
        
        private Gtk.Image image16;
        
        private Gtk.HBox hbox1;
        
        private Gtk.CheckButton FoundButton;
        
        private Gtk.CheckButton checkbutton1;
        
        private Gtk.CheckButton MineButton;
        
        private Gtk.CheckButton UnavailableButton;
        
        private Gtk.CheckButton ArchivedButton;
        
        private Gtk.ScrolledWindow GtkScrolledWindow;
        
        private Gtk.TreeView treeview1;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.CacheList
            Stetic.BinContainer w1 = Stetic.BinContainer.Attach(this);
            this.UIManager = new Gtk.UIManager();
            Gtk.ActionGroup w2 = new Gtk.ActionGroup("Default");
            this.FilterAction = new Gtk.Action("FilterAction", Mono.Unix.Catalog.GetString("Filter"), null, null);
            this.FilterAction.ShortLabel = Mono.Unix.Catalog.GetString("Filter");
            w2.Add(this.FilterAction, null);
            this.UIManager.InsertActionGroup(w2, 0);
            this.Name = "ocmgtk.CacheList";
            // Container child ocmgtk.CacheList.Gtk.Container+ContainerChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            this.vbox2.BorderWidth = ((uint)(6));
            // Container child vbox2.Gtk.Box+BoxChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Name = "hbox3";
            this.hbox3.Spacing = 6;
            // Container child hbox3.Gtk.Box+BoxChild
            this.image19 = new Gtk.Image();
            this.image19.Name = "image19";
            this.image19.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-find", Gtk.IconSize.Menu, 16);
            this.hbox3.Add(this.image19);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox3[this.image19]));
            w3.Position = 0;
            w3.Expand = false;
            w3.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.filterEntry = new Gtk.Entry();
            this.filterEntry.CanFocus = true;
            this.filterEntry.Name = "filterEntry";
            this.filterEntry.IsEditable = true;
            this.filterEntry.InvisibleChar = '●';
            this.hbox3.Add(this.filterEntry);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.hbox3[this.filterEntry]));
            w4.Position = 1;
            // Container child hbox3.Gtk.Box+BoxChild
            this.button154 = new Gtk.Button();
            this.button154.CanFocus = true;
            this.button154.Name = "button154";
            // Container child button154.Gtk.Container+ContainerChild
            this.image17 = new Gtk.Image();
            this.image17.Name = "image17";
            this.image17.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-clear", Gtk.IconSize.Menu, 16);
            this.button154.Add(this.image17);
            this.button154.Label = null;
            this.hbox3.Add(this.button154);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.hbox3[this.button154]));
            w6.Position = 2;
            w6.Expand = false;
            w6.Fill = false;
            this.vbox2.Add(this.hbox3);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.vbox2[this.hbox3]));
            w7.Position = 0;
            w7.Expand = false;
            w7.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            // Container child hbox2.Gtk.Box+BoxChild
            this.image23 = new Gtk.Image();
            this.image23.Name = "image23";
            this.image23.Pixbuf = Stetic.IconLoader.LoadIcon(this, "stock_draw-dimension-line", Gtk.IconSize.Menu, 16);
            this.hbox2.Add(this.image23);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.hbox2[this.image23]));
            w8.Position = 0;
            w8.Expand = false;
            w8.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.distanceEntry = new Gtk.Entry();
            this.distanceEntry.CanFocus = true;
            this.distanceEntry.Name = "distanceEntry";
            this.distanceEntry.IsEditable = true;
            this.distanceEntry.WidthChars = 5;
            this.distanceEntry.InvisibleChar = '•';
            this.hbox2.Add(this.distanceEntry);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.hbox2[this.distanceEntry]));
            w9.Position = 1;
            // Container child hbox2.Gtk.Box+BoxChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("km");
            this.hbox2.Add(this.label1);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.hbox2[this.label1]));
            w10.Position = 2;
            w10.Expand = false;
            w10.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.button136 = new Gtk.Button();
            this.button136.CanFocus = true;
            this.button136.Name = "button136";
            // Container child button136.Gtk.Container+ContainerChild
            this.image16 = new Gtk.Image();
            this.image16.Name = "image16";
            this.image16.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-clear", Gtk.IconSize.Menu, 16);
            this.button136.Add(this.image16);
            this.button136.Label = null;
            this.hbox2.Add(this.button136);
            Gtk.Box.BoxChild w12 = ((Gtk.Box.BoxChild)(this.hbox2[this.button136]));
            w12.Position = 3;
            w12.Expand = false;
            w12.Fill = false;
            this.vbox2.Add(this.hbox2);
            Gtk.Box.BoxChild w13 = ((Gtk.Box.BoxChild)(this.vbox2[this.hbox2]));
            w13.Position = 1;
            w13.Expand = false;
            w13.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.FoundButton = new Gtk.CheckButton();
            this.FoundButton.CanFocus = true;
            this.FoundButton.Name = "FoundButton";
            this.FoundButton.Label = Mono.Unix.Catalog.GetString("Found");
            this.FoundButton.Active = true;
            this.FoundButton.DrawIndicator = true;
            this.FoundButton.UseUnderline = true;
            this.hbox1.Add(this.FoundButton);
            Gtk.Box.BoxChild w14 = ((Gtk.Box.BoxChild)(this.hbox1[this.FoundButton]));
            w14.Position = 0;
            // Container child hbox1.Gtk.Box+BoxChild
            this.checkbutton1 = new Gtk.CheckButton();
            this.checkbutton1.CanFocus = true;
            this.checkbutton1.Name = "checkbutton1";
            this.checkbutton1.Label = Mono.Unix.Catalog.GetString("Not Found");
            this.checkbutton1.Active = true;
            this.checkbutton1.DrawIndicator = true;
            this.checkbutton1.UseUnderline = true;
            this.hbox1.Add(this.checkbutton1);
            Gtk.Box.BoxChild w15 = ((Gtk.Box.BoxChild)(this.hbox1[this.checkbutton1]));
            w15.Position = 1;
            // Container child hbox1.Gtk.Box+BoxChild
            this.MineButton = new Gtk.CheckButton();
            this.MineButton.CanFocus = true;
            this.MineButton.Name = "MineButton";
            this.MineButton.Label = Mono.Unix.Catalog.GetString("Mine");
            this.MineButton.Active = true;
            this.MineButton.DrawIndicator = true;
            this.MineButton.UseUnderline = true;
            this.hbox1.Add(this.MineButton);
            Gtk.Box.BoxChild w16 = ((Gtk.Box.BoxChild)(this.hbox1[this.MineButton]));
            w16.Position = 2;
            // Container child hbox1.Gtk.Box+BoxChild
            this.UnavailableButton = new Gtk.CheckButton();
            this.UnavailableButton.CanFocus = true;
            this.UnavailableButton.Name = "UnavailableButton";
            this.UnavailableButton.Label = Mono.Unix.Catalog.GetString("Unavailable");
            this.UnavailableButton.Active = true;
            this.UnavailableButton.DrawIndicator = true;
            this.UnavailableButton.UseUnderline = true;
            this.hbox1.Add(this.UnavailableButton);
            Gtk.Box.BoxChild w17 = ((Gtk.Box.BoxChild)(this.hbox1[this.UnavailableButton]));
            w17.Position = 3;
            // Container child hbox1.Gtk.Box+BoxChild
            this.ArchivedButton = new Gtk.CheckButton();
            this.ArchivedButton.CanFocus = true;
            this.ArchivedButton.Name = "ArchivedButton";
            this.ArchivedButton.Label = Mono.Unix.Catalog.GetString("Archived");
            this.ArchivedButton.Active = true;
            this.ArchivedButton.DrawIndicator = true;
            this.ArchivedButton.UseUnderline = true;
            this.hbox1.Add(this.ArchivedButton);
            Gtk.Box.BoxChild w18 = ((Gtk.Box.BoxChild)(this.hbox1[this.ArchivedButton]));
            w18.Position = 4;
            this.vbox2.Add(this.hbox1);
            Gtk.Box.BoxChild w19 = ((Gtk.Box.BoxChild)(this.vbox2[this.hbox1]));
            w19.Position = 2;
            w19.Expand = false;
            w19.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.GtkScrolledWindow = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow.Name = "GtkScrolledWindow";
            this.GtkScrolledWindow.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow.Gtk.Container+ContainerChild
            this.treeview1 = new Gtk.TreeView();
            this.treeview1.CanFocus = true;
            this.treeview1.Name = "treeview1";
            this.treeview1.Reorderable = true;
            this.GtkScrolledWindow.Add(this.treeview1);
            this.vbox2.Add(this.GtkScrolledWindow);
            Gtk.Box.BoxChild w21 = ((Gtk.Box.BoxChild)(this.vbox2[this.GtkScrolledWindow]));
            w21.Position = 3;
            this.Add(this.vbox2);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            w1.SetUiManager(UIManager);
            this.Hide();
            this.button154.Clicked += new System.EventHandler(this.OnClearClicked);
            this.button136.Clicked += new System.EventHandler(this.OnClearDistance);
            this.FoundButton.Toggled += new System.EventHandler(this.OnFoundButtonToggled);
            this.checkbutton1.Toggled += new System.EventHandler(this.OnNotFoundToggled);
            this.MineButton.Toggled += new System.EventHandler(this.OnMineToggled);
            this.UnavailableButton.Toggled += new System.EventHandler(this.OnUnavailableToggled);
            this.ArchivedButton.Toggled += new System.EventHandler(this.OnArchivedButtonToggled);
            this.treeview1.ButtonPressEvent += new Gtk.ButtonPressEventHandler(this.DoButtonPress);
        }
    }
}
