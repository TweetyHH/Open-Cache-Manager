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
    
    
    public partial class DescriptionWidget {
        
        private Gtk.VBox vbox2;
        
        private Gtk.HBox hbox1;
        
        private Gtk.Expander hintExpander;
        
        private Gtk.ScrolledWindow GtkScrolledWindow;
        
        private Gtk.TextView hintField;
        
        private Gtk.Label GtkLabel21;
        
        private Gtk.Expander tbugExpander;
        
        private Gtk.ScrolledWindow scrolledwindow1;
        
        private Gtk.TreeView tbugView;
        
        private Gtk.Label GtkLabel2;
        
        private Gtk.ScrolledWindow descScroll;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.DescriptionWidget
            Stetic.BinContainer.Attach(this);
            this.Name = "ocmgtk.DescriptionWidget";
            // Container child ocmgtk.DescriptionWidget.Gtk.Container+ContainerChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            this.vbox2.BorderWidth = ((uint)(6));
            // Container child vbox2.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Homogeneous = true;
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.hintExpander = new Gtk.Expander(null);
            this.hintExpander.CanFocus = true;
            this.hintExpander.Name = "hintExpander";
            // Container child hintExpander.Gtk.Container+ContainerChild
            this.GtkScrolledWindow = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow.Name = "GtkScrolledWindow";
            this.GtkScrolledWindow.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow.Gtk.Container+ContainerChild
            this.hintField = new Gtk.TextView();
            this.hintField.CanFocus = true;
            this.hintField.Name = "hintField";
            this.hintField.Editable = false;
            this.hintField.CursorVisible = false;
            this.hintField.AcceptsTab = false;
            this.GtkScrolledWindow.Add(this.hintField);
            this.hintExpander.Add(this.GtkScrolledWindow);
            this.GtkLabel21 = new Gtk.Label();
            this.GtkLabel21.Name = "GtkLabel21";
            this.GtkLabel21.LabelProp = Mono.Unix.Catalog.GetString("<b>Hint</b>");
            this.GtkLabel21.UseMarkup = true;
            this.GtkLabel21.UseUnderline = true;
            this.hintExpander.LabelWidget = this.GtkLabel21;
            this.hbox1.Add(this.hintExpander);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox1[this.hintExpander]));
            w3.Position = 0;
            // Container child hbox1.Gtk.Box+BoxChild
            this.tbugExpander = new Gtk.Expander(null);
            this.tbugExpander.CanFocus = true;
            this.tbugExpander.Name = "tbugExpander";
            // Container child tbugExpander.Gtk.Container+ContainerChild
            this.scrolledwindow1 = new Gtk.ScrolledWindow();
            this.scrolledwindow1.CanFocus = true;
            this.scrolledwindow1.Name = "scrolledwindow1";
            this.scrolledwindow1.ShadowType = ((Gtk.ShadowType)(1));
            // Container child scrolledwindow1.Gtk.Container+ContainerChild
            this.tbugView = new Gtk.TreeView();
            this.tbugView.CanFocus = true;
            this.tbugView.Name = "tbugView";
            this.scrolledwindow1.Add(this.tbugView);
            this.tbugExpander.Add(this.scrolledwindow1);
            this.GtkLabel2 = new Gtk.Label();
            this.GtkLabel2.Name = "GtkLabel2";
            this.GtkLabel2.Xalign = 0F;
            this.GtkLabel2.LabelProp = Mono.Unix.Catalog.GetString("<b>Travel Bugs</b>");
            this.GtkLabel2.UseMarkup = true;
            this.GtkLabel2.UseUnderline = true;
            this.tbugExpander.LabelWidget = this.GtkLabel2;
            this.hbox1.Add(this.tbugExpander);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.hbox1[this.tbugExpander]));
            w6.Position = 1;
            this.vbox2.Add(this.hbox1);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.vbox2[this.hbox1]));
            w7.Position = 0;
            w7.Expand = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.descScroll = new Gtk.ScrolledWindow();
            this.descScroll.CanFocus = true;
            this.descScroll.Name = "descScroll";
            this.descScroll.ShadowType = ((Gtk.ShadowType)(1));
            this.vbox2.Add(this.descScroll);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.vbox2[this.descScroll]));
            w8.Position = 1;
            this.Add(this.vbox2);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Hide();
        }
    }
}