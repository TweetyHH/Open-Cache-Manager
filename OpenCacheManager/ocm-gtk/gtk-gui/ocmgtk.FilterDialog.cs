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
    
    
    public partial class FilterDialog {
        
        private Gtk.Notebook notebook1;
        
        private ocmgtk.OCMQueryPage1 page1;
        
        private Gtk.Label diffLabel;
        
        private ocmgtk.OCMQueryPage2 contPage;
        
        private Gtk.Label contLabel;
        
        private ocmgtk.OCMQueryPage3 datePage;
        
        private Gtk.Label dateLabel;
        
        private ocmgtk.OCMQueryPage4 childrenPage;
        
        private Gtk.Label labelChildren;
        
        private ocmgtk.OCMQueryPage5 attributePage;
        
        private Gtk.Label attrPageLabel;
        
        private ocmgtk.OCMQueryPage6 ocmquerypage61;
        
        private Gtk.Label label6;
        
        private Gtk.Button buttonCancel;
        
        private Gtk.Button buttonOk;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.FilterDialog
            this.Name = "ocmgtk.FilterDialog";
            this.Title = Mono.Unix.Catalog.GetString("Advanced Filters...");
            this.TypeHint = ((Gdk.WindowTypeHint)(1));
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.Modal = true;
            this.BorderWidth = ((uint)(6));
            this.Resizable = false;
            this.AllowGrow = false;
            this.SkipPagerHint = true;
            this.SkipTaskbarHint = true;
            // Internal child ocmgtk.FilterDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.notebook1 = new Gtk.Notebook();
            this.notebook1.CanFocus = true;
            this.notebook1.Name = "notebook1";
            this.notebook1.CurrentPage = 0;
            this.notebook1.TabPos = ((Gtk.PositionType)(0));
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.page1 = new ocmgtk.OCMQueryPage1();
            this.page1.Events = ((Gdk.EventMask)(256));
            this.page1.Name = "page1";
            this.notebook1.Add(this.page1);
            // Notebook tab
            this.diffLabel = new Gtk.Label();
            this.diffLabel.Name = "diffLabel";
            this.diffLabel.LabelProp = Mono.Unix.Catalog.GetString("Difficulty/Type");
            this.notebook1.SetTabLabel(this.page1, this.diffLabel);
            this.diffLabel.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.contPage = new ocmgtk.OCMQueryPage2();
            this.contPage.Events = ((Gdk.EventMask)(256));
            this.contPage.Name = "contPage";
            this.notebook1.Add(this.contPage);
            Gtk.Notebook.NotebookChild w3 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.contPage]));
            w3.Position = 1;
            // Notebook tab
            this.contLabel = new Gtk.Label();
            this.contLabel.Name = "contLabel";
            this.contLabel.LabelProp = Mono.Unix.Catalog.GetString("Container/Description/Placed By");
            this.notebook1.SetTabLabel(this.contPage, this.contLabel);
            this.contLabel.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.datePage = new ocmgtk.OCMQueryPage3();
            this.datePage.Events = ((Gdk.EventMask)(256));
            this.datePage.Name = "datePage";
            this.datePage.PlaceBefore = new System.DateTime(0);
            this.datePage.PlaceAfter = new System.DateTime(0);
            this.datePage.InfoAfter = new System.DateTime(0);
            this.datePage.InfoBefore = new System.DateTime(0);
            this.datePage.FoundOn = new System.DateTime(0);
            this.datePage.FoundBefore = new System.DateTime(0);
            this.datePage.FoundAfter = new System.DateTime(0);
            this.notebook1.Add(this.datePage);
            Gtk.Notebook.NotebookChild w4 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.datePage]));
            w4.Position = 2;
            // Notebook tab
            this.dateLabel = new Gtk.Label();
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.LabelProp = Mono.Unix.Catalog.GetString("Dates/Location");
            this.notebook1.SetTabLabel(this.datePage, this.dateLabel);
            this.dateLabel.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.childrenPage = new ocmgtk.OCMQueryPage4();
            this.childrenPage.Events = ((Gdk.EventMask)(256));
            this.childrenPage.Name = "childrenPage";
            this.childrenPage.HasNotes = false;
            this.childrenPage.HasCorrectedCoords = false;
            this.childrenPage.DoesNotHaveCorrectedCoords = false;
            this.notebook1.Add(this.childrenPage);
            Gtk.Notebook.NotebookChild w5 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.childrenPage]));
            w5.Position = 3;
            // Notebook tab
            this.labelChildren = new Gtk.Label();
            this.labelChildren.Name = "labelChildren";
            this.labelChildren.LabelProp = Mono.Unix.Catalog.GetString("Notes/Children/Corrected");
            this.notebook1.SetTabLabel(this.childrenPage, this.labelChildren);
            this.labelChildren.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.attributePage = new ocmgtk.OCMQueryPage5();
            this.attributePage.Events = ((Gdk.EventMask)(256));
            this.attributePage.Name = "attributePage";
            this.notebook1.Add(this.attributePage);
            Gtk.Notebook.NotebookChild w6 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.attributePage]));
            w6.Position = 4;
            // Notebook tab
            this.attrPageLabel = new Gtk.Label();
            this.attrPageLabel.Name = "attrPageLabel";
            this.attrPageLabel.LabelProp = Mono.Unix.Catalog.GetString("Attributes");
            this.notebook1.SetTabLabel(this.attributePage, this.attrPageLabel);
            this.attrPageLabel.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.ocmquerypage61 = new ocmgtk.OCMQueryPage6();
            this.ocmquerypage61.Events = ((Gdk.EventMask)(256));
            this.ocmquerypage61.Name = "ocmquerypage61";
            this.notebook1.Add(this.ocmquerypage61);
            Gtk.Notebook.NotebookChild w7 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.ocmquerypage61]));
            w7.Position = 5;
            // Notebook tab
            this.label6 = new Gtk.Label();
            this.label6.Name = "label6";
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("Status/Distance");
            this.notebook1.SetTabLabel(this.ocmquerypage61, this.label6);
            this.label6.ShowAll();
            w1.Add(this.notebook1);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(w1[this.notebook1]));
            w8.Position = 0;
            // Internal child ocmgtk.FilterDialog.ActionArea
            Gtk.HButtonBox w9 = this.ActionArea;
            w9.Name = "dialog1_ActionArea";
            w9.Spacing = 10;
            w9.BorderWidth = ((uint)(5));
            w9.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w10 = ((Gtk.ButtonBox.ButtonBoxChild)(w9[this.buttonCancel]));
            w10.Expand = false;
            w10.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-ok";
            this.AddActionWidget(this.buttonOk, -5);
            Gtk.ButtonBox.ButtonBoxChild w11 = ((Gtk.ButtonBox.ButtonBoxChild)(w9[this.buttonOk]));
            w11.Position = 1;
            w11.Expand = false;
            w11.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 1012;
            this.DefaultHeight = 619;
            this.buttonOk.HasDefault = true;
            this.Show();
            this.DeleteEvent += new Gtk.DeleteEventHandler(this.OnDeleteClick);
            this.buttonCancel.Clicked += new System.EventHandler(this.OnCancel);
            this.buttonOk.Clicked += new System.EventHandler(this.OnOKClicked);
        }
    }
}
