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
    
    
    public partial class ReviewWaypointDialog {
        
        private ocmgtk.ReviewWaypointWidget reviewWidget;
        
        private Gtk.Button buttonCancel;
        
        private Gtk.Button button595;
        
        private Gtk.Button buttonOk;
        
        private Gtk.Button button603;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.ReviewWaypointDialog
            this.Name = "ocmgtk.ReviewWaypointDialog";
            this.Title = Mono.Unix.Catalog.GetString("Review Waypoint (0 of 0)");
            this.TypeHint = ((Gdk.WindowTypeHint)(1));
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.Modal = true;
            this.BorderWidth = ((uint)(6));
            // Internal child ocmgtk.ReviewWaypointDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.reviewWidget = new ocmgtk.ReviewWaypointWidget();
            this.reviewWidget.Events = ((Gdk.EventMask)(256));
            this.reviewWidget.Name = "reviewWidget";
            w1.Add(this.reviewWidget);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(w1[this.reviewWidget]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            // Internal child ocmgtk.ReviewWaypointDialog.ActionArea
            Gtk.HButtonBox w3 = this.ActionArea;
            w3.Name = "dialog1_ActionArea";
            w3.Spacing = 10;
            w3.BorderWidth = ((uint)(5));
            w3.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w4 = ((Gtk.ButtonBox.ButtonBoxChild)(w3[this.buttonCancel]));
            w4.Expand = false;
            w4.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.button595 = new Gtk.Button();
            this.button595.CanFocus = true;
            this.button595.Name = "button595";
            this.button595.UseUnderline = true;
            this.button595.Label = Mono.Unix.Catalog.GetString("_Skip");
            this.AddActionWidget(this.button595, 0);
            Gtk.ButtonBox.ButtonBoxChild w5 = ((Gtk.ButtonBox.ButtonBoxChild)(w3[this.button595]));
            w5.Position = 1;
            w5.Expand = false;
            w5.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-add";
            this.AddActionWidget(this.buttonOk, 0);
            Gtk.ButtonBox.ButtonBoxChild w6 = ((Gtk.ButtonBox.ButtonBoxChild)(w3[this.buttonOk]));
            w6.Position = 2;
            w6.Expand = false;
            w6.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.button603 = new Gtk.Button();
            this.button603.CanFocus = true;
            this.button603.Name = "button603";
            this.button603.UseUnderline = true;
            this.button603.Label = Mono.Unix.Catalog.GetString("_Close");
            this.AddActionWidget(this.button603, 0);
            Gtk.ButtonBox.ButtonBoxChild w7 = ((Gtk.ButtonBox.ButtonBoxChild)(w3[this.button603]));
            w7.Position = 3;
            w7.Expand = false;
            w7.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 400;
            this.DefaultHeight = 389;
            this.Show();
        }
    }
}
