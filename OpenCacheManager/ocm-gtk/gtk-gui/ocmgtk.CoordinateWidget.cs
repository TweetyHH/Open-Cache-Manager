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
    
    
    public partial class CoordinateWidget {
        
        private Gtk.HBox hbox5;
        
        private Gtk.ComboBox directionCombo;
        
        private Gtk.Entry degreeEntry;
        
        private Gtk.Label label9;
        
        private Gtk.Entry minuteEntry;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.CoordinateWidget
            Stetic.BinContainer.Attach(this);
            this.Name = "ocmgtk.CoordinateWidget";
            // Container child ocmgtk.CoordinateWidget.Gtk.Container+ContainerChild
            this.hbox5 = new Gtk.HBox();
            this.hbox5.Name = "hbox5";
            this.hbox5.Spacing = 6;
            // Container child hbox5.Gtk.Box+BoxChild
            this.directionCombo = Gtk.ComboBox.NewText();
            this.directionCombo.Name = "directionCombo";
            this.hbox5.Add(this.directionCombo);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.hbox5[this.directionCombo]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child hbox5.Gtk.Box+BoxChild
            this.degreeEntry = new Gtk.Entry();
            this.degreeEntry.CanFocus = true;
            this.degreeEntry.Name = "degreeEntry";
            this.degreeEntry.Text = Mono.Unix.Catalog.GetString("000");
            this.degreeEntry.IsEditable = true;
            this.degreeEntry.WidthChars = 3;
            this.degreeEntry.MaxLength = 3;
            this.degreeEntry.InvisibleChar = '•';
            this.hbox5.Add(this.degreeEntry);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.hbox5[this.degreeEntry]));
            w2.Position = 1;
            w2.Expand = false;
            // Container child hbox5.Gtk.Box+BoxChild
            this.label9 = new Gtk.Label();
            this.label9.Name = "label9";
            this.label9.LabelProp = Mono.Unix.Catalog.GetString("°");
            this.hbox5.Add(this.label9);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox5[this.label9]));
            w3.Position = 2;
            w3.Expand = false;
            w3.Fill = false;
            // Container child hbox5.Gtk.Box+BoxChild
            this.minuteEntry = new Gtk.Entry();
            this.minuteEntry.CanFocus = true;
            this.minuteEntry.Name = "minuteEntry";
            this.minuteEntry.Text = Mono.Unix.Catalog.GetString("0.000");
            this.minuteEntry.IsEditable = true;
            this.minuteEntry.InvisibleChar = '•';
            this.hbox5.Add(this.minuteEntry);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.hbox5[this.minuteEntry]));
            w4.Position = 3;
            this.Add(this.hbox5);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Hide();
        }
    }
}
