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
    
    
    public partial class GPSConfiguration {
        
        private Gtk.Table table1;
        
        private Gtk.RadioButton gpxRadio;
        
        private ocmgtk.GPXWidget gpxwidget;
        
        private Gtk.RadioButton gusbRadio;
        
        private Gtk.Label label1;
        
        private Gtk.RadioButton otherRadio;
        
        private Gtk.Button buttonCancel;
        
        private Gtk.Button buttonOk;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.GPSConfiguration
            this.WidthRequest = 500;
            this.HeightRequest = 350;
            this.Name = "ocmgtk.GPSConfiguration";
            this.Title = Mono.Unix.Catalog.GetString("GPS Configuration...");
            this.TypeHint = ((Gdk.WindowTypeHint)(1));
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.Modal = true;
            this.BorderWidth = ((uint)(6));
            this.Resizable = false;
            this.AllowGrow = false;
            this.SkipPagerHint = true;
            this.SkipTaskbarHint = true;
            // Internal child ocmgtk.GPSConfiguration.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.table1 = new Gtk.Table(((uint)(4)), ((uint)(2)), false);
            this.table1.Name = "table1";
            this.table1.RowSpacing = ((uint)(6));
            this.table1.ColumnSpacing = ((uint)(6));
            this.table1.BorderWidth = ((uint)(6));
            // Container child table1.Gtk.Table+TableChild
            this.gpxRadio = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("Garmin Colorado/Oregon/Dakota/Nuvi"));
            this.gpxRadio.CanFocus = true;
            this.gpxRadio.Name = "gpxRadio";
            this.gpxRadio.DrawIndicator = true;
            this.gpxRadio.UseUnderline = true;
            this.gpxRadio.Group = new GLib.SList(System.IntPtr.Zero);
            this.table1.Add(this.gpxRadio);
            Gtk.Table.TableChild w2 = ((Gtk.Table.TableChild)(this.table1[this.gpxRadio]));
            w2.LeftAttach = ((uint)(1));
            w2.RightAttach = ((uint)(2));
            w2.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.gpxwidget = new ocmgtk.GPXWidget();
            this.gpxwidget.Events = ((Gdk.EventMask)(256));
            this.gpxwidget.Name = "gpxwidget";
            this.table1.Add(this.gpxwidget);
            Gtk.Table.TableChild w3 = ((Gtk.Table.TableChild)(this.table1[this.gpxwidget]));
            w3.TopAttach = ((uint)(3));
            w3.BottomAttach = ((uint)(4));
            w3.RightAttach = ((uint)(2));
            w3.XOptions = ((Gtk.AttachOptions)(4));
            w3.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.gusbRadio = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("Garmin eTrex/GPSMap/Street Pilot"));
            this.gusbRadio.CanFocus = true;
            this.gusbRadio.Name = "gusbRadio";
            this.gusbRadio.DrawIndicator = true;
            this.gusbRadio.UseUnderline = true;
            this.gusbRadio.Group = this.gpxRadio.Group;
            this.table1.Add(this.gusbRadio);
            Gtk.Table.TableChild w4 = ((Gtk.Table.TableChild)(this.table1[this.gusbRadio]));
            w4.TopAttach = ((uint)(1));
            w4.BottomAttach = ((uint)(2));
            w4.LeftAttach = ((uint)(1));
            w4.RightAttach = ((uint)(2));
            w4.XOptions = ((Gtk.AttachOptions)(4));
            w4.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Select a GPS Type:");
            this.table1.Add(this.label1);
            Gtk.Table.TableChild w5 = ((Gtk.Table.TableChild)(this.table1[this.label1]));
            w5.XOptions = ((Gtk.AttachOptions)(4));
            w5.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.otherRadio = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("Other"));
            this.otherRadio.CanFocus = true;
            this.otherRadio.Name = "otherRadio";
            this.otherRadio.DrawIndicator = true;
            this.otherRadio.UseUnderline = true;
            this.otherRadio.Group = this.gpxRadio.Group;
            this.table1.Add(this.otherRadio);
            Gtk.Table.TableChild w6 = ((Gtk.Table.TableChild)(this.table1[this.otherRadio]));
            w6.TopAttach = ((uint)(2));
            w6.BottomAttach = ((uint)(3));
            w6.LeftAttach = ((uint)(1));
            w6.RightAttach = ((uint)(2));
            w6.XOptions = ((Gtk.AttachOptions)(4));
            w6.YOptions = ((Gtk.AttachOptions)(4));
            w1.Add(this.table1);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(w1[this.table1]));
            w7.Position = 0;
            w7.Expand = false;
            w7.Fill = false;
            // Internal child ocmgtk.GPSConfiguration.ActionArea
            Gtk.HButtonBox w8 = this.ActionArea;
            w8.Name = "dialog1_ActionArea";
            w8.Spacing = 10;
            w8.BorderWidth = ((uint)(5));
            w8.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w9 = ((Gtk.ButtonBox.ButtonBoxChild)(w8[this.buttonCancel]));
            w9.Expand = false;
            w9.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-ok";
            this.AddActionWidget(this.buttonOk, -5);
            Gtk.ButtonBox.ButtonBoxChild w10 = ((Gtk.ButtonBox.ButtonBoxChild)(w8[this.buttonOk]));
            w10.Position = 1;
            w10.Expand = false;
            w10.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 502;
            this.DefaultHeight = 379;
            this.buttonOk.HasDefault = true;
            this.Show();
            this.otherRadio.Toggled += new System.EventHandler(this.OnOtherToggle);
            this.gusbRadio.Toggled += new System.EventHandler(this.OnGUSBToggle);
            this.gpxRadio.Toggled += new System.EventHandler(this.OnGPXToggled);
            this.buttonCancel.Clicked += new System.EventHandler(this.OnButtonClick);
            this.buttonOk.Clicked += new System.EventHandler(this.OnButtonClick);
        }
    }
}
