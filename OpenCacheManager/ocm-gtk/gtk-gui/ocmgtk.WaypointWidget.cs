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
    
    
    public partial class WaypointWidget {
        
        private Gtk.UIManager UIManager;
        
        private Gtk.Action refreshAction;
        
        private Gtk.VBox widgetBox;
        
        private Gtk.HBox hbox1;
        
        private Gtk.ScrolledWindow GtkScrolledWindow;
        
        private Gtk.TreeView wptView;
        
        private Gtk.VButtonBox vbuttonbox1;
        
        private Gtk.Button addButton;
        
        private Gtk.Button editButton;
        
        private Gtk.Button deleteButton;
        
        private Gtk.Button button414;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.WaypointWidget
            Stetic.BinContainer w1 = Stetic.BinContainer.Attach(this);
            this.UIManager = new Gtk.UIManager();
            Gtk.ActionGroup w2 = new Gtk.ActionGroup("Default");
            this.refreshAction = new Gtk.Action("refreshAction", null, null, "gtk-refresh");
            w2.Add(this.refreshAction, null);
            this.UIManager.InsertActionGroup(w2, 0);
            this.Name = "ocmgtk.WaypointWidget";
            // Container child ocmgtk.WaypointWidget.Gtk.Container+ContainerChild
            this.widgetBox = new Gtk.VBox();
            this.widgetBox.Name = "widgetBox";
            // Container child widgetBox.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            this.hbox1.BorderWidth = ((uint)(6));
            // Container child hbox1.Gtk.Box+BoxChild
            this.GtkScrolledWindow = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow.Name = "GtkScrolledWindow";
            this.GtkScrolledWindow.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow.Gtk.Container+ContainerChild
            this.wptView = new Gtk.TreeView();
            this.wptView.CanFocus = true;
            this.wptView.Name = "wptView";
            this.GtkScrolledWindow.Add(this.wptView);
            this.hbox1.Add(this.GtkScrolledWindow);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.hbox1[this.GtkScrolledWindow]));
            w4.Position = 0;
            // Container child hbox1.Gtk.Box+BoxChild
            this.vbuttonbox1 = new Gtk.VButtonBox();
            this.vbuttonbox1.Name = "vbuttonbox1";
            this.vbuttonbox1.Spacing = 6;
            this.vbuttonbox1.LayoutStyle = ((Gtk.ButtonBoxStyle)(3));
            // Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
            this.addButton = new Gtk.Button();
            this.addButton.CanFocus = true;
            this.addButton.Name = "addButton";
            this.addButton.UseStock = true;
            this.addButton.UseUnderline = true;
            this.addButton.Label = "gtk-add";
            this.vbuttonbox1.Add(this.addButton);
            Gtk.ButtonBox.ButtonBoxChild w5 = ((Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1[this.addButton]));
            w5.Expand = false;
            w5.Fill = false;
            // Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
            this.editButton = new Gtk.Button();
            this.editButton.Sensitive = false;
            this.editButton.CanFocus = true;
            this.editButton.Name = "editButton";
            this.editButton.UseStock = true;
            this.editButton.UseUnderline = true;
            this.editButton.Label = "gtk-edit";
            this.vbuttonbox1.Add(this.editButton);
            Gtk.ButtonBox.ButtonBoxChild w6 = ((Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1[this.editButton]));
            w6.Position = 1;
            w6.Expand = false;
            w6.Fill = false;
            // Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
            this.deleteButton = new Gtk.Button();
            this.deleteButton.Sensitive = false;
            this.deleteButton.CanFocus = true;
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.UseStock = true;
            this.deleteButton.UseUnderline = true;
            this.deleteButton.Label = "gtk-delete";
            this.vbuttonbox1.Add(this.deleteButton);
            Gtk.ButtonBox.ButtonBoxChild w7 = ((Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1[this.deleteButton]));
            w7.Position = 2;
            w7.Expand = false;
            w7.Fill = false;
            // Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
            this.button414 = new Gtk.Button();
            this.button414.CanFocus = true;
            this.button414.Name = "button414";
            this.button414.UseUnderline = true;
            this.button414.Label = Mono.Unix.Catalog.GetString("_Grab Waypoints...");
            this.vbuttonbox1.Add(this.button414);
            Gtk.ButtonBox.ButtonBoxChild w8 = ((Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1[this.button414]));
            w8.Position = 3;
            w8.Expand = false;
            w8.Fill = false;
            this.hbox1.Add(this.vbuttonbox1);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.hbox1[this.vbuttonbox1]));
            w9.Position = 1;
            w9.Expand = false;
            w9.Fill = false;
            this.widgetBox.Add(this.hbox1);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.widgetBox[this.hbox1]));
            w10.Position = 0;
            this.Add(this.widgetBox);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            w1.SetUiManager(UIManager);
            this.Hide();
            this.addButton.Clicked += new System.EventHandler(this.doAdd);
            this.editButton.Clicked += new System.EventHandler(this.DoEdit);
            this.deleteButton.Clicked += new System.EventHandler(this.doRemove);
            this.button414.Clicked += new System.EventHandler(this.OnGrabClick);
        }
    }
}
