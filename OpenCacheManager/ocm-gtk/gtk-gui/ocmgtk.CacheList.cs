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
        
        private Gtk.Entry filterEntry;
        
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
            // Container child vbox2.Gtk.Box+BoxChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Name = "hbox3";
            this.hbox3.Spacing = 6;
            // Container child hbox3.Gtk.Box+BoxChild
            this.filterEntry = new Gtk.Entry();
            this.filterEntry.CanFocus = true;
            this.filterEntry.Name = "filterEntry";
            this.filterEntry.IsEditable = true;
            this.filterEntry.InvisibleChar = '●';
            this.hbox3.Add(this.filterEntry);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox3[this.filterEntry]));
            w3.Position = 0;
            this.vbox2.Add(this.hbox3);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.vbox2[this.hbox3]));
            w4.Position = 0;
            w4.Expand = false;
            w4.Fill = false;
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
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.vbox2[this.GtkScrolledWindow]));
            w6.Position = 1;
            this.Add(this.vbox2);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            w1.SetUiManager(UIManager);
            this.Hide();
            this.treeview1.ButtonPressEvent += new Gtk.ButtonPressEventHandler(this.DoButtonPress);
        }
    }
}
