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
    
    
    public partial class NotesWidget {
        
        private Gtk.UIManager UIManager;
        
        private Gtk.ToggleAction boldAction;
        
        private Gtk.Action italicAction;
        
        private Gtk.Action missingImageAction;
        
        private Gtk.Action underlineAction;
        
        private Gtk.VBox vbox1;
        
        private ocmgtk.HTMLEditorWidget editorWidget;
        
        private Gtk.HButtonBox hbuttonbox1;
        
        private Gtk.Button saveButton;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.NotesWidget
            Stetic.BinContainer w1 = Stetic.BinContainer.Attach(this);
            this.UIManager = new Gtk.UIManager();
            Gtk.ActionGroup w2 = new Gtk.ActionGroup("Default");
            this.boldAction = new Gtk.ToggleAction("boldAction", null, null, "gtk-bold");
            w2.Add(this.boldAction, null);
            this.italicAction = new Gtk.Action("italicAction", null, null, "gtk-italic");
            w2.Add(this.italicAction, null);
            this.missingImageAction = new Gtk.Action("missingImageAction", null, null, "gtk-missing-image");
            w2.Add(this.missingImageAction, null);
            this.underlineAction = new Gtk.Action("underlineAction", null, null, "gtk-underline");
            w2.Add(this.underlineAction, null);
            this.UIManager.InsertActionGroup(w2, 0);
            this.Name = "ocmgtk.NotesWidget";
            // Container child ocmgtk.NotesWidget.Gtk.Container+ContainerChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            this.vbox1.BorderWidth = ((uint)(6));
            // Container child vbox1.Gtk.Box+BoxChild
            this.editorWidget = new ocmgtk.HTMLEditorWidget();
            this.editorWidget.Events = ((Gdk.EventMask)(256));
            this.editorWidget.Name = "editorWidget";
            this.vbox1.Add(this.editorWidget);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.vbox1[this.editorWidget]));
            w3.Position = 0;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbuttonbox1 = new Gtk.HButtonBox();
            this.hbuttonbox1.Name = "hbuttonbox1";
            this.hbuttonbox1.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child hbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
            this.saveButton = new Gtk.Button();
            this.saveButton.Sensitive = false;
            this.saveButton.CanFocus = true;
            this.saveButton.Name = "saveButton";
            this.saveButton.UseUnderline = true;
            // Container child saveButton.Gtk.Container+ContainerChild
            Gtk.Alignment w4 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w5 = new Gtk.HBox();
            w5.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w6 = new Gtk.Image();
            w6.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-save", Gtk.IconSize.Menu, 16);
            w5.Add(w6);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w8 = new Gtk.Label();
            w8.LabelProp = Mono.Unix.Catalog.GetString("_Save");
            w8.UseUnderline = true;
            w5.Add(w8);
            w4.Add(w5);
            this.saveButton.Add(w4);
            this.hbuttonbox1.Add(this.saveButton);
            Gtk.ButtonBox.ButtonBoxChild w12 = ((Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox1[this.saveButton]));
            w12.Expand = false;
            w12.Fill = false;
            this.vbox1.Add(this.hbuttonbox1);
            Gtk.Box.BoxChild w13 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbuttonbox1]));
            w13.Position = 1;
            w13.Expand = false;
            w13.Fill = false;
            this.Add(this.vbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            w1.SetUiManager(UIManager);
            this.Hide();
            this.saveButton.Clicked += new System.EventHandler(this.OnSaveNotes);
        }
    }
}
