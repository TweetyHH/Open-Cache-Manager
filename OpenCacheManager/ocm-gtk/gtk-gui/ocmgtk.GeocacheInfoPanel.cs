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
    
    
    public partial class GeocacheInfoPanel {
        
        private Gtk.VBox infoPanelVBOX;
        
        private Gtk.Label statusLabel;
        
        private Gtk.HBox hbox1;
        
        private Gtk.Image cacheIcon;
        
        private Gtk.VBox vbox2;
        
        private Gtk.Label cacheNameLabel;
        
        private Gtk.Label cacheCodeLabel;
        
        private Gtk.Label cacheTypeLabel;
        
        private Gtk.VButtonBox vbuttonbox1;
        
        private Gtk.Button logButton;
        
        private Gtk.Button viewButton;
        
        private Gtk.HBox hbox4;
        
        private Gtk.Label countryLabel;
        
        private Gtk.Label label6;
        
        private Gtk.Label lastFoundDateLabel;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Label label3;
        
        private Gtk.Label placedByLabel;
        
        private Gtk.Label label5;
        
        private Gtk.Label dateLabel;
        
        private Gtk.Label label2;
        
        private Gtk.Label infoDateLabel;
        
        private Gtk.HBox hbox3;
        
        private Gtk.Label label1;
        
        private Gtk.Label cacheSizeLabel;
        
        private Gtk.Label diffLabel;
        
        private Gtk.Image diff_i1;
        
        private Gtk.Image diff_i2;
        
        private Gtk.Image diff_i3;
        
        private Gtk.Image diff_i4;
        
        private Gtk.Image diff_i5;
        
        private Gtk.Label label4;
        
        private Gtk.Image terr_i1;
        
        private Gtk.Image terr_i2;
        
        private Gtk.Image terr_i3;
        
        private Gtk.Image terr_i4;
        
        private Gtk.Image terr_i5;
        
        private Gtk.Alignment alignment1;
        
        private Gtk.HBox hbox7;
        
        private Gtk.Label coordinateLabel;
        
        private Gtk.Label distance_label;
        
        private Gtk.HButtonBox hbuttonbox2;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.GeocacheInfoPanel
            Stetic.BinContainer.Attach(this);
            this.Name = "ocmgtk.GeocacheInfoPanel";
            // Container child ocmgtk.GeocacheInfoPanel.Gtk.Container+ContainerChild
            this.infoPanelVBOX = new Gtk.VBox();
            this.infoPanelVBOX.Name = "infoPanelVBOX";
            this.infoPanelVBOX.Spacing = 6;
            this.infoPanelVBOX.BorderWidth = ((uint)(6));
            // Container child infoPanelVBOX.Gtk.Box+BoxChild
            this.statusLabel = new Gtk.Label();
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.UseMarkup = true;
            this.statusLabel.Justify = ((Gtk.Justification)(2));
            this.infoPanelVBOX.Add(this.statusLabel);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.infoPanelVBOX[this.statusLabel]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child infoPanelVBOX.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.cacheIcon = new Gtk.Image();
            this.cacheIcon.Name = "cacheIcon";
            this.cacheIcon.Xpad = 5;
            this.cacheIcon.Ypad = 5;
            this.hbox1.Add(this.cacheIcon);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.hbox1[this.cacheIcon]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.cacheNameLabel = new Gtk.Label();
            this.cacheNameLabel.Name = "cacheNameLabel";
            this.cacheNameLabel.Xpad = 5;
            this.cacheNameLabel.Ypad = 5;
            this.cacheNameLabel.Xalign = 0F;
            this.cacheNameLabel.LabelProp = "<cacheName>";
            this.cacheNameLabel.Ellipsize = ((Pango.EllipsizeMode)(3));
            this.vbox2.Add(this.cacheNameLabel);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.vbox2[this.cacheNameLabel]));
            w3.Position = 0;
            w3.Expand = false;
            w3.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.cacheCodeLabel = new Gtk.Label();
            this.cacheCodeLabel.Name = "cacheCodeLabel";
            this.cacheCodeLabel.Xpad = 6;
            this.cacheCodeLabel.Ypad = 6;
            this.cacheCodeLabel.Xalign = 0F;
            this.cacheCodeLabel.LabelProp = "<cacheCode>";
            this.cacheCodeLabel.Ellipsize = ((Pango.EllipsizeMode)(3));
            this.vbox2.Add(this.cacheCodeLabel);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.vbox2[this.cacheCodeLabel]));
            w4.Position = 1;
            w4.Expand = false;
            w4.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.cacheTypeLabel = new Gtk.Label();
            this.cacheTypeLabel.Name = "cacheTypeLabel";
            this.cacheTypeLabel.Xpad = 6;
            this.cacheTypeLabel.Ypad = 6;
            this.cacheTypeLabel.Xalign = 0F;
            this.cacheTypeLabel.LabelProp = "<cacheType>";
            this.cacheTypeLabel.Ellipsize = ((Pango.EllipsizeMode)(3));
            this.vbox2.Add(this.cacheTypeLabel);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.vbox2[this.cacheTypeLabel]));
            w5.Position = 2;
            w5.Expand = false;
            w5.Fill = false;
            this.hbox1.Add(this.vbox2);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.hbox1[this.vbox2]));
            w6.Position = 1;
            // Container child hbox1.Gtk.Box+BoxChild
            this.vbuttonbox1 = new Gtk.VButtonBox();
            this.vbuttonbox1.Name = "vbuttonbox1";
            this.vbuttonbox1.Spacing = 6;
            this.vbuttonbox1.BorderWidth = ((uint)(6));
            this.vbuttonbox1.LayoutStyle = ((Gtk.ButtonBoxStyle)(3));
            // Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
            this.logButton = new Gtk.Button();
            this.logButton.CanFocus = true;
            this.logButton.Name = "logButton";
            this.logButton.UseUnderline = true;
            // Container child logButton.Gtk.Container+ContainerChild
            Gtk.Alignment w7 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w8 = new Gtk.HBox();
            w8.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w9 = new Gtk.Image();
            w9.Pixbuf = Stetic.IconLoader.LoadIcon(this, "stock_help-chat", Gtk.IconSize.Menu, 16);
            w8.Add(w9);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w11 = new Gtk.Label();
            w11.LabelProp = Mono.Unix.Catalog.GetString("Log My Find...");
            w11.UseUnderline = true;
            w8.Add(w11);
            w7.Add(w8);
            this.logButton.Add(w7);
            this.vbuttonbox1.Add(this.logButton);
            Gtk.ButtonBox.ButtonBoxChild w15 = ((Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1[this.logButton]));
            w15.Expand = false;
            w15.Fill = false;
            // Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
            this.viewButton = new Gtk.Button();
            this.viewButton.CanFocus = true;
            this.viewButton.Name = "viewButton";
            this.viewButton.UseUnderline = true;
            this.viewButton.Label = Mono.Unix.Catalog.GetString("View Online...");
            this.vbuttonbox1.Add(this.viewButton);
            Gtk.ButtonBox.ButtonBoxChild w16 = ((Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1[this.viewButton]));
            w16.Position = 1;
            w16.Expand = false;
            w16.Fill = false;
            this.hbox1.Add(this.vbuttonbox1);
            Gtk.Box.BoxChild w17 = ((Gtk.Box.BoxChild)(this.hbox1[this.vbuttonbox1]));
            w17.PackType = ((Gtk.PackType)(1));
            w17.Position = 2;
            w17.Expand = false;
            w17.Fill = false;
            this.infoPanelVBOX.Add(this.hbox1);
            Gtk.Box.BoxChild w18 = ((Gtk.Box.BoxChild)(this.infoPanelVBOX[this.hbox1]));
            w18.Position = 1;
            w18.Expand = false;
            w18.Fill = false;
            // Container child infoPanelVBOX.Gtk.Box+BoxChild
            this.hbox4 = new Gtk.HBox();
            this.hbox4.Name = "hbox4";
            this.hbox4.Spacing = 6;
            // Container child hbox4.Gtk.Box+BoxChild
            this.countryLabel = new Gtk.Label();
            this.countryLabel.Name = "countryLabel";
            this.countryLabel.Xpad = 6;
            this.countryLabel.Xalign = 0F;
            this.countryLabel.LabelProp = Mono.Unix.Catalog.GetString("<country>");
            this.hbox4.Add(this.countryLabel);
            Gtk.Box.BoxChild w19 = ((Gtk.Box.BoxChild)(this.hbox4[this.countryLabel]));
            w19.Position = 0;
            w19.Expand = false;
            w19.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.label6 = new Gtk.Label();
            this.label6.Name = "label6";
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("<b>Last Log By You:</b>");
            this.label6.UseMarkup = true;
            this.hbox4.Add(this.label6);
            Gtk.Box.BoxChild w20 = ((Gtk.Box.BoxChild)(this.hbox4[this.label6]));
            w20.Position = 1;
            w20.Expand = false;
            w20.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.lastFoundDateLabel = new Gtk.Label();
            this.lastFoundDateLabel.Name = "lastFoundDateLabel";
            this.lastFoundDateLabel.LabelProp = Mono.Unix.Catalog.GetString("Never");
            this.hbox4.Add(this.lastFoundDateLabel);
            Gtk.Box.BoxChild w21 = ((Gtk.Box.BoxChild)(this.hbox4[this.lastFoundDateLabel]));
            w21.Position = 2;
            w21.Expand = false;
            w21.Fill = false;
            this.infoPanelVBOX.Add(this.hbox4);
            Gtk.Box.BoxChild w22 = ((Gtk.Box.BoxChild)(this.infoPanelVBOX[this.hbox4]));
            w22.Position = 2;
            w22.Expand = false;
            w22.Fill = false;
            // Container child infoPanelVBOX.Gtk.Box+BoxChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            // Container child hbox2.Gtk.Box+BoxChild
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.Xpad = 5;
            this.label3.Ypad = 5;
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("<b>A cache by:</b>");
            this.label3.UseMarkup = true;
            this.hbox2.Add(this.label3);
            Gtk.Box.BoxChild w23 = ((Gtk.Box.BoxChild)(this.hbox2[this.label3]));
            w23.Position = 0;
            w23.Expand = false;
            w23.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.placedByLabel = new Gtk.Label();
            this.placedByLabel.Name = "placedByLabel";
            this.placedByLabel.Xpad = 5;
            this.placedByLabel.Ypad = 5;
            this.placedByLabel.LabelProp = "<name>";
            this.placedByLabel.Ellipsize = ((Pango.EllipsizeMode)(3));
            this.placedByLabel.MaxWidthChars = 15;
            this.hbox2.Add(this.placedByLabel);
            Gtk.Box.BoxChild w24 = ((Gtk.Box.BoxChild)(this.hbox2[this.placedByLabel]));
            w24.Position = 1;
            w24.Expand = false;
            w24.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.label5 = new Gtk.Label();
            this.label5.Name = "label5";
            this.label5.Xpad = 5;
            this.label5.Ypad = 5;
            this.label5.LabelProp = Mono.Unix.Catalog.GetString("<b>Hidden on:</b>");
            this.label5.UseMarkup = true;
            this.label5.Justify = ((Gtk.Justification)(1));
            this.hbox2.Add(this.label5);
            Gtk.Box.BoxChild w25 = ((Gtk.Box.BoxChild)(this.hbox2[this.label5]));
            w25.Position = 2;
            w25.Expand = false;
            w25.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.dateLabel = new Gtk.Label();
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Xpad = 5;
            this.dateLabel.Ypad = 5;
            this.dateLabel.Xalign = 0F;
            this.dateLabel.LabelProp = "<date>";
            this.hbox2.Add(this.dateLabel);
            Gtk.Box.BoxChild w26 = ((Gtk.Box.BoxChild)(this.hbox2[this.dateLabel]));
            w26.Position = 3;
            w26.Expand = false;
            w26.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("<b>Last Update:</b>");
            this.label2.UseMarkup = true;
            this.hbox2.Add(this.label2);
            Gtk.Box.BoxChild w27 = ((Gtk.Box.BoxChild)(this.hbox2[this.label2]));
            w27.Position = 4;
            w27.Expand = false;
            w27.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.infoDateLabel = new Gtk.Label();
            this.infoDateLabel.Name = "infoDateLabel";
            this.infoDateLabel.LabelProp = "<date>";
            this.hbox2.Add(this.infoDateLabel);
            Gtk.Box.BoxChild w28 = ((Gtk.Box.BoxChild)(this.hbox2[this.infoDateLabel]));
            w28.Position = 5;
            w28.Expand = false;
            w28.Fill = false;
            this.infoPanelVBOX.Add(this.hbox2);
            Gtk.Box.BoxChild w29 = ((Gtk.Box.BoxChild)(this.infoPanelVBOX[this.hbox2]));
            w29.Position = 3;
            w29.Expand = false;
            w29.Fill = false;
            // Container child infoPanelVBOX.Gtk.Box+BoxChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Name = "hbox3";
            this.hbox3.Spacing = 6;
            // Container child hbox3.Gtk.Box+BoxChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.Xpad = 5;
            this.label1.Ypad = 5;
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("<b>Cache Size:</b>");
            this.label1.UseMarkup = true;
            this.hbox3.Add(this.label1);
            Gtk.Box.BoxChild w30 = ((Gtk.Box.BoxChild)(this.hbox3[this.label1]));
            w30.Position = 0;
            w30.Expand = false;
            w30.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.cacheSizeLabel = new Gtk.Label();
            this.cacheSizeLabel.Name = "cacheSizeLabel";
            this.cacheSizeLabel.Ypad = 5;
            this.cacheSizeLabel.LabelProp = "<cacheSize>";
            this.hbox3.Add(this.cacheSizeLabel);
            Gtk.Box.BoxChild w31 = ((Gtk.Box.BoxChild)(this.hbox3[this.cacheSizeLabel]));
            w31.Position = 1;
            w31.Expand = false;
            w31.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.diffLabel = new Gtk.Label();
            this.diffLabel.Name = "diffLabel";
            this.diffLabel.Xpad = 5;
            this.diffLabel.Ypad = 5;
            this.diffLabel.LabelProp = Mono.Unix.Catalog.GetString("<b>Difficulty:</b>");
            this.diffLabel.UseMarkup = true;
            this.hbox3.Add(this.diffLabel);
            Gtk.Box.BoxChild w32 = ((Gtk.Box.BoxChild)(this.hbox3[this.diffLabel]));
            w32.Position = 2;
            w32.Expand = false;
            w32.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.diff_i1 = new Gtk.Image();
            this.diff_i1.Name = "diff_i1";
            this.hbox3.Add(this.diff_i1);
            Gtk.Box.BoxChild w33 = ((Gtk.Box.BoxChild)(this.hbox3[this.diff_i1]));
            w33.Position = 3;
            w33.Expand = false;
            w33.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.diff_i2 = new Gtk.Image();
            this.diff_i2.Name = "diff_i2";
            this.hbox3.Add(this.diff_i2);
            Gtk.Box.BoxChild w34 = ((Gtk.Box.BoxChild)(this.hbox3[this.diff_i2]));
            w34.Position = 4;
            w34.Expand = false;
            w34.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.diff_i3 = new Gtk.Image();
            this.diff_i3.Name = "diff_i3";
            this.hbox3.Add(this.diff_i3);
            Gtk.Box.BoxChild w35 = ((Gtk.Box.BoxChild)(this.hbox3[this.diff_i3]));
            w35.Position = 5;
            w35.Expand = false;
            w35.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.diff_i4 = new Gtk.Image();
            this.diff_i4.Name = "diff_i4";
            this.hbox3.Add(this.diff_i4);
            Gtk.Box.BoxChild w36 = ((Gtk.Box.BoxChild)(this.hbox3[this.diff_i4]));
            w36.Position = 6;
            w36.Expand = false;
            w36.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.diff_i5 = new Gtk.Image();
            this.diff_i5.Name = "diff_i5";
            this.hbox3.Add(this.diff_i5);
            Gtk.Box.BoxChild w37 = ((Gtk.Box.BoxChild)(this.hbox3[this.diff_i5]));
            w37.Position = 7;
            w37.Expand = false;
            w37.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.Xpad = 5;
            this.label4.Ypad = 5;
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("<b>Terrain:</b>");
            this.label4.UseMarkup = true;
            this.hbox3.Add(this.label4);
            Gtk.Box.BoxChild w38 = ((Gtk.Box.BoxChild)(this.hbox3[this.label4]));
            w38.Position = 8;
            w38.Expand = false;
            w38.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.terr_i1 = new Gtk.Image();
            this.terr_i1.Name = "terr_i1";
            this.hbox3.Add(this.terr_i1);
            Gtk.Box.BoxChild w39 = ((Gtk.Box.BoxChild)(this.hbox3[this.terr_i1]));
            w39.Position = 9;
            w39.Expand = false;
            w39.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.terr_i2 = new Gtk.Image();
            this.terr_i2.Name = "terr_i2";
            this.hbox3.Add(this.terr_i2);
            Gtk.Box.BoxChild w40 = ((Gtk.Box.BoxChild)(this.hbox3[this.terr_i2]));
            w40.Position = 10;
            w40.Expand = false;
            w40.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.terr_i3 = new Gtk.Image();
            this.terr_i3.Name = "terr_i3";
            this.hbox3.Add(this.terr_i3);
            Gtk.Box.BoxChild w41 = ((Gtk.Box.BoxChild)(this.hbox3[this.terr_i3]));
            w41.Position = 11;
            w41.Expand = false;
            w41.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.terr_i4 = new Gtk.Image();
            this.terr_i4.Name = "terr_i4";
            this.hbox3.Add(this.terr_i4);
            Gtk.Box.BoxChild w42 = ((Gtk.Box.BoxChild)(this.hbox3[this.terr_i4]));
            w42.Position = 12;
            w42.Expand = false;
            w42.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.terr_i5 = new Gtk.Image();
            this.terr_i5.Name = "terr_i5";
            this.hbox3.Add(this.terr_i5);
            Gtk.Box.BoxChild w43 = ((Gtk.Box.BoxChild)(this.hbox3[this.terr_i5]));
            w43.Position = 13;
            w43.Expand = false;
            w43.Fill = false;
            this.infoPanelVBOX.Add(this.hbox3);
            Gtk.Box.BoxChild w44 = ((Gtk.Box.BoxChild)(this.infoPanelVBOX[this.hbox3]));
            w44.Position = 4;
            w44.Expand = false;
            w44.Fill = false;
            // Container child infoPanelVBOX.Gtk.Box+BoxChild
            this.alignment1 = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.alignment1.Name = "alignment1";
            // Container child alignment1.Gtk.Container+ContainerChild
            this.hbox7 = new Gtk.HBox();
            this.hbox7.Name = "hbox7";
            this.hbox7.Spacing = 6;
            // Container child hbox7.Gtk.Box+BoxChild
            this.coordinateLabel = new Gtk.Label();
            this.coordinateLabel.Name = "coordinateLabel";
            this.coordinateLabel.Xpad = 5;
            this.coordinateLabel.Ypad = 5;
            this.coordinateLabel.LabelProp = "<coord>";
            this.hbox7.Add(this.coordinateLabel);
            Gtk.Box.BoxChild w45 = ((Gtk.Box.BoxChild)(this.hbox7[this.coordinateLabel]));
            w45.Position = 0;
            w45.Expand = false;
            w45.Fill = false;
            // Container child hbox7.Gtk.Box+BoxChild
            this.distance_label = new Gtk.Label();
            this.distance_label.Name = "distance_label";
            this.distance_label.LabelProp = "<bearing & distance>";
            this.hbox7.Add(this.distance_label);
            Gtk.Box.BoxChild w46 = ((Gtk.Box.BoxChild)(this.hbox7[this.distance_label]));
            w46.Position = 1;
            w46.Expand = false;
            w46.Fill = false;
            this.alignment1.Add(this.hbox7);
            this.infoPanelVBOX.Add(this.alignment1);
            Gtk.Box.BoxChild w48 = ((Gtk.Box.BoxChild)(this.infoPanelVBOX[this.alignment1]));
            w48.Position = 5;
            w48.Expand = false;
            w48.Fill = false;
            // Container child infoPanelVBOX.Gtk.Box+BoxChild
            this.hbuttonbox2 = new Gtk.HButtonBox();
            this.hbuttonbox2.Name = "hbuttonbox2";
            this.infoPanelVBOX.Add(this.hbuttonbox2);
            Gtk.Box.BoxChild w49 = ((Gtk.Box.BoxChild)(this.infoPanelVBOX[this.hbuttonbox2]));
            w49.Position = 6;
            w49.Expand = false;
            w49.Fill = false;
            this.Add(this.infoPanelVBOX);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Hide();
            this.logButton.Clicked += new System.EventHandler(this.OnClickLog);
            this.viewButton.Clicked += new System.EventHandler(this.OnClickView);
        }
    }
}
