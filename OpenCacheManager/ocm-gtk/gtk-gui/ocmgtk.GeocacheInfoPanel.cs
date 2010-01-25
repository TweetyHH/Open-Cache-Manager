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
        
        private Gtk.HBox hbox1;
        
        private Gtk.Image cacheIcon;
        
        private Gtk.VBox vbox2;
        
        private Gtk.Label cacheNameLabel;
        
        private Gtk.Label cacheCodeLabel;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Label label3;
        
        private Gtk.Label placedByLabel;
        
        private Gtk.Label label5;
        
        private Gtk.Label dateLabel;
        
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
        
        private Gtk.HBox hbox4;
        
        private Gtk.Label label8;
        
        private Gtk.Label cacheTypeLabel;
        
        private Gtk.HSeparator hseparator1;
        
        private Gtk.Expander hintExpander;
        
        private Gtk.Label hintLabel;
        
        private Gtk.Label GtkLabel2;
        
        private Gtk.VBox vbox1;
        
        private Gtk.Label label18;
        
        private Gtk.ScrolledWindow longDescriptionScroll;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.GeocacheInfoPanel
            Stetic.BinContainer.Attach(this);
            this.Name = "ocmgtk.GeocacheInfoPanel";
            // Container child ocmgtk.GeocacheInfoPanel.Gtk.Container+ContainerChild
            this.infoPanelVBOX = new Gtk.VBox();
            this.infoPanelVBOX.Name = "infoPanelVBOX";
            this.infoPanelVBOX.Spacing = 6;
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
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.hbox1[this.cacheIcon]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
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
            this.cacheNameLabel.LabelProp = Mono.Unix.Catalog.GetString("<cacheName>");
            this.vbox2.Add(this.cacheNameLabel);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox2[this.cacheNameLabel]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.cacheCodeLabel = new Gtk.Label();
            this.cacheCodeLabel.Name = "cacheCodeLabel";
            this.cacheCodeLabel.Xpad = 5;
            this.cacheCodeLabel.Ypad = 5;
            this.cacheCodeLabel.Xalign = 0F;
            this.cacheCodeLabel.LabelProp = Mono.Unix.Catalog.GetString("<cacheCode>");
            this.vbox2.Add(this.cacheCodeLabel);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.vbox2[this.cacheCodeLabel]));
            w3.Position = 1;
            w3.Expand = false;
            w3.Fill = false;
            this.hbox1.Add(this.vbox2);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.hbox1[this.vbox2]));
            w4.Position = 1;
            w4.Expand = false;
            w4.Fill = false;
            this.infoPanelVBOX.Add(this.hbox1);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.infoPanelVBOX[this.hbox1]));
            w5.Position = 0;
            w5.Expand = false;
            w5.Fill = false;
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
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.hbox2[this.label3]));
            w6.Position = 0;
            w6.Expand = false;
            w6.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.placedByLabel = new Gtk.Label();
            this.placedByLabel.Name = "placedByLabel";
            this.placedByLabel.Xpad = 5;
            this.placedByLabel.Ypad = 5;
            this.placedByLabel.LabelProp = "<name>";
            this.hbox2.Add(this.placedByLabel);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.hbox2[this.placedByLabel]));
            w7.Position = 1;
            w7.Expand = false;
            w7.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.label5 = new Gtk.Label();
            this.label5.Name = "label5";
            this.label5.Xpad = 5;
            this.label5.Ypad = 5;
            this.label5.LabelProp = Mono.Unix.Catalog.GetString("<b>Hidden on:</b>");
            this.label5.UseMarkup = true;
            this.label5.Justify = ((Gtk.Justification)(1));
            this.hbox2.Add(this.label5);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.hbox2[this.label5]));
            w8.Position = 2;
            w8.Expand = false;
            w8.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.dateLabel = new Gtk.Label();
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Xpad = 5;
            this.dateLabel.Ypad = 5;
            this.dateLabel.Xalign = 0F;
            this.dateLabel.LabelProp = Mono.Unix.Catalog.GetString("<date>");
            this.hbox2.Add(this.dateLabel);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.hbox2[this.dateLabel]));
            w9.Position = 3;
            w9.Expand = false;
            w9.Fill = false;
            this.infoPanelVBOX.Add(this.hbox2);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.infoPanelVBOX[this.hbox2]));
            w10.Position = 1;
            w10.Expand = false;
            w10.Fill = false;
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
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(this.hbox3[this.label1]));
            w11.Position = 0;
            w11.Expand = false;
            w11.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.cacheSizeLabel = new Gtk.Label();
            this.cacheSizeLabel.Name = "cacheSizeLabel";
            this.cacheSizeLabel.Ypad = 5;
            this.cacheSizeLabel.LabelProp = Mono.Unix.Catalog.GetString("<cacheSize>");
            this.hbox3.Add(this.cacheSizeLabel);
            Gtk.Box.BoxChild w12 = ((Gtk.Box.BoxChild)(this.hbox3[this.cacheSizeLabel]));
            w12.Position = 1;
            w12.Expand = false;
            w12.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.diffLabel = new Gtk.Label();
            this.diffLabel.Name = "diffLabel";
            this.diffLabel.Xpad = 5;
            this.diffLabel.Ypad = 5;
            this.diffLabel.LabelProp = Mono.Unix.Catalog.GetString("<b>Difficulty:</b>");
            this.diffLabel.UseMarkup = true;
            this.hbox3.Add(this.diffLabel);
            Gtk.Box.BoxChild w13 = ((Gtk.Box.BoxChild)(this.hbox3[this.diffLabel]));
            w13.Position = 2;
            w13.Expand = false;
            w13.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.diff_i1 = new Gtk.Image();
            this.diff_i1.Name = "diff_i1";
            this.hbox3.Add(this.diff_i1);
            Gtk.Box.BoxChild w14 = ((Gtk.Box.BoxChild)(this.hbox3[this.diff_i1]));
            w14.Position = 3;
            w14.Expand = false;
            w14.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.diff_i2 = new Gtk.Image();
            this.diff_i2.Name = "diff_i2";
            this.hbox3.Add(this.diff_i2);
            Gtk.Box.BoxChild w15 = ((Gtk.Box.BoxChild)(this.hbox3[this.diff_i2]));
            w15.Position = 4;
            w15.Expand = false;
            w15.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.diff_i3 = new Gtk.Image();
            this.diff_i3.Name = "diff_i3";
            this.hbox3.Add(this.diff_i3);
            Gtk.Box.BoxChild w16 = ((Gtk.Box.BoxChild)(this.hbox3[this.diff_i3]));
            w16.Position = 5;
            w16.Expand = false;
            w16.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.diff_i4 = new Gtk.Image();
            this.diff_i4.Name = "diff_i4";
            this.hbox3.Add(this.diff_i4);
            Gtk.Box.BoxChild w17 = ((Gtk.Box.BoxChild)(this.hbox3[this.diff_i4]));
            w17.Position = 6;
            w17.Expand = false;
            w17.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.diff_i5 = new Gtk.Image();
            this.diff_i5.Name = "diff_i5";
            this.hbox3.Add(this.diff_i5);
            Gtk.Box.BoxChild w18 = ((Gtk.Box.BoxChild)(this.hbox3[this.diff_i5]));
            w18.Position = 7;
            w18.Expand = false;
            w18.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.Xpad = 5;
            this.label4.Ypad = 5;
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("<b>Terrain:</b>");
            this.label4.UseMarkup = true;
            this.hbox3.Add(this.label4);
            Gtk.Box.BoxChild w19 = ((Gtk.Box.BoxChild)(this.hbox3[this.label4]));
            w19.Position = 8;
            w19.Expand = false;
            w19.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.terr_i1 = new Gtk.Image();
            this.terr_i1.Name = "terr_i1";
            this.hbox3.Add(this.terr_i1);
            Gtk.Box.BoxChild w20 = ((Gtk.Box.BoxChild)(this.hbox3[this.terr_i1]));
            w20.Position = 9;
            w20.Expand = false;
            w20.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.terr_i2 = new Gtk.Image();
            this.terr_i2.Name = "terr_i2";
            this.hbox3.Add(this.terr_i2);
            Gtk.Box.BoxChild w21 = ((Gtk.Box.BoxChild)(this.hbox3[this.terr_i2]));
            w21.Position = 10;
            w21.Expand = false;
            w21.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.terr_i3 = new Gtk.Image();
            this.terr_i3.Name = "terr_i3";
            this.hbox3.Add(this.terr_i3);
            Gtk.Box.BoxChild w22 = ((Gtk.Box.BoxChild)(this.hbox3[this.terr_i3]));
            w22.Position = 11;
            w22.Expand = false;
            w22.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.terr_i4 = new Gtk.Image();
            this.terr_i4.Name = "terr_i4";
            this.hbox3.Add(this.terr_i4);
            Gtk.Box.BoxChild w23 = ((Gtk.Box.BoxChild)(this.hbox3[this.terr_i4]));
            w23.Position = 12;
            w23.Expand = false;
            w23.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.terr_i5 = new Gtk.Image();
            this.terr_i5.Name = "terr_i5";
            this.hbox3.Add(this.terr_i5);
            Gtk.Box.BoxChild w24 = ((Gtk.Box.BoxChild)(this.hbox3[this.terr_i5]));
            w24.Position = 13;
            w24.Expand = false;
            w24.Fill = false;
            this.infoPanelVBOX.Add(this.hbox3);
            Gtk.Box.BoxChild w25 = ((Gtk.Box.BoxChild)(this.infoPanelVBOX[this.hbox3]));
            w25.Position = 2;
            w25.Expand = false;
            w25.Fill = false;
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
            this.coordinateLabel.LabelProp = Mono.Unix.Catalog.GetString("<big>Some Coordinate</big>");
            this.coordinateLabel.UseMarkup = true;
            this.hbox7.Add(this.coordinateLabel);
            Gtk.Box.BoxChild w26 = ((Gtk.Box.BoxChild)(this.hbox7[this.coordinateLabel]));
            w26.Position = 0;
            w26.Expand = false;
            w26.Fill = false;
            this.alignment1.Add(this.hbox7);
            this.infoPanelVBOX.Add(this.alignment1);
            Gtk.Box.BoxChild w28 = ((Gtk.Box.BoxChild)(this.infoPanelVBOX[this.alignment1]));
            w28.Position = 3;
            w28.Expand = false;
            w28.Fill = false;
            // Container child infoPanelVBOX.Gtk.Box+BoxChild
            this.hbox4 = new Gtk.HBox();
            this.hbox4.Name = "hbox4";
            this.hbox4.Spacing = 6;
            // Container child hbox4.Gtk.Box+BoxChild
            this.label8 = new Gtk.Label();
            this.label8.Name = "label8";
            this.label8.Xpad = 5;
            this.label8.Ypad = 5;
            this.label8.LabelProp = Mono.Unix.Catalog.GetString("<b>Cache type</b>");
            this.label8.UseMarkup = true;
            this.hbox4.Add(this.label8);
            Gtk.Box.BoxChild w29 = ((Gtk.Box.BoxChild)(this.hbox4[this.label8]));
            w29.Position = 0;
            w29.Expand = false;
            w29.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.cacheTypeLabel = new Gtk.Label();
            this.cacheTypeLabel.Name = "cacheTypeLabel";
            this.cacheTypeLabel.Ypad = 5;
            this.cacheTypeLabel.LabelProp = Mono.Unix.Catalog.GetString("<cacheType>");
            this.hbox4.Add(this.cacheTypeLabel);
            Gtk.Box.BoxChild w30 = ((Gtk.Box.BoxChild)(this.hbox4[this.cacheTypeLabel]));
            w30.Position = 1;
            w30.Expand = false;
            w30.Fill = false;
            this.infoPanelVBOX.Add(this.hbox4);
            Gtk.Box.BoxChild w31 = ((Gtk.Box.BoxChild)(this.infoPanelVBOX[this.hbox4]));
            w31.Position = 4;
            w31.Expand = false;
            w31.Fill = false;
            // Container child infoPanelVBOX.Gtk.Box+BoxChild
            this.hseparator1 = new Gtk.HSeparator();
            this.hseparator1.Name = "hseparator1";
            this.infoPanelVBOX.Add(this.hseparator1);
            Gtk.Box.BoxChild w32 = ((Gtk.Box.BoxChild)(this.infoPanelVBOX[this.hseparator1]));
            w32.Position = 5;
            w32.Expand = false;
            w32.Fill = false;
            // Container child infoPanelVBOX.Gtk.Box+BoxChild
            this.hintExpander = new Gtk.Expander(null);
            this.hintExpander.CanFocus = true;
            this.hintExpander.Name = "hintExpander";
            // Container child hintExpander.Gtk.Container+ContainerChild
            this.hintLabel = new Gtk.Label();
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Xpad = 5;
            this.hintLabel.Ypad = 5;
            this.hintLabel.Xalign = 0F;
            this.hintExpander.Add(this.hintLabel);
            this.GtkLabel2 = new Gtk.Label();
            this.GtkLabel2.Name = "GtkLabel2";
            this.GtkLabel2.LabelProp = Mono.Unix.Catalog.GetString("<b>Hint</b>");
            this.GtkLabel2.UseMarkup = true;
            this.GtkLabel2.UseUnderline = true;
            this.hintExpander.LabelWidget = this.GtkLabel2;
            this.infoPanelVBOX.Add(this.hintExpander);
            Gtk.Box.BoxChild w34 = ((Gtk.Box.BoxChild)(this.infoPanelVBOX[this.hintExpander]));
            w34.Position = 6;
            w34.Expand = false;
            w34.Fill = false;
            // Container child infoPanelVBOX.Gtk.Box+BoxChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this.label18 = new Gtk.Label();
            this.label18.Name = "label18";
            this.label18.Xpad = 5;
            this.label18.Ypad = 5;
            this.label18.Xalign = 0F;
            this.label18.LabelProp = Mono.Unix.Catalog.GetString("<b>Description</b>");
            this.label18.UseMarkup = true;
            this.vbox1.Add(this.label18);
            Gtk.Box.BoxChild w35 = ((Gtk.Box.BoxChild)(this.vbox1[this.label18]));
            w35.Position = 0;
            w35.Expand = false;
            w35.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.longDescriptionScroll = new Gtk.ScrolledWindow();
            this.longDescriptionScroll.HeightRequest = 450;
            this.longDescriptionScroll.CanFocus = true;
            this.longDescriptionScroll.Name = "longDescriptionScroll";
            this.longDescriptionScroll.ShadowType = ((Gtk.ShadowType)(1));
            this.vbox1.Add(this.longDescriptionScroll);
            Gtk.Box.BoxChild w36 = ((Gtk.Box.BoxChild)(this.vbox1[this.longDescriptionScroll]));
            w36.PackType = ((Gtk.PackType)(1));
            w36.Position = 1;
            this.infoPanelVBOX.Add(this.vbox1);
            Gtk.Box.BoxChild w37 = ((Gtk.Box.BoxChild)(this.infoPanelVBOX[this.vbox1]));
            w37.Position = 7;
            this.Add(this.infoPanelVBOX);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Hide();
        }
    }
}
