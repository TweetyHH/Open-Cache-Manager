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
        
        private Gtk.Label label3;
        
        private ocmgtk.OCMQueryPage2 contPage;
        
        private Gtk.Label page2;
        
        private ocmgtk.OCMQueryPage3 datePage;
        
        private Gtk.Label label1;
        
        private Gtk.Button buttonCancel;
        
        private Gtk.Button buttonOk;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.FilterDialog
            this.Name = "ocmgtk.FilterDialog";
            this.Title = Mono.Unix.Catalog.GetString("Advanced Filters...");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            // Internal child ocmgtk.FilterDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.notebook1 = new Gtk.Notebook();
            this.notebook1.CanFocus = true;
            this.notebook1.Name = "notebook1";
            this.notebook1.CurrentPage = 2;
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.page1 = new ocmgtk.OCMQueryPage1();
            this.page1.Events = ((Gdk.EventMask)(256));
            this.page1.Name = "page1";
            this.notebook1.Add(this.page1);
            // Notebook tab
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("Difficulty/Type");
            this.notebook1.SetTabLabel(this.page1, this.label3);
            this.label3.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.contPage = new ocmgtk.OCMQueryPage2();
            this.contPage.Events = ((Gdk.EventMask)(256));
            this.contPage.Name = "contPage";
            this.contPage.PlacedByMe = false;
            this.notebook1.Add(this.contPage);
            Gtk.Notebook.NotebookChild w3 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.contPage]));
            w3.Position = 1;
            // Notebook tab
            this.page2 = new Gtk.Label();
            this.page2.Name = "page2";
            this.page2.LabelProp = Mono.Unix.Catalog.GetString("Container/Description/Placed By");
            this.notebook1.SetTabLabel(this.contPage, this.page2);
            this.page2.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.datePage = new ocmgtk.OCMQueryPage3();
            this.datePage.Events = ((Gdk.EventMask)(256));
            this.datePage.Name = "datePage";
            this.datePage.PlaceBefore = new System.DateTime(0);
            this.datePage.PlaceAfter = new System.DateTime(0);
            this.datePage.InfoAfter = new System.DateTime(0);
            this.datePage.InfoBefore = new System.DateTime(0);
            this.notebook1.Add(this.datePage);
            Gtk.Notebook.NotebookChild w4 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.datePage]));
            w4.Position = 2;
            // Notebook tab
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Dates");
            this.notebook1.SetTabLabel(this.datePage, this.label1);
            this.label1.ShowAll();
            w1.Add(this.notebook1);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(w1[this.notebook1]));
            w5.Position = 0;
            // Internal child ocmgtk.FilterDialog.ActionArea
            Gtk.HButtonBox w6 = this.ActionArea;
            w6.Name = "dialog1_ActionArea";
            w6.Spacing = 10;
            w6.BorderWidth = ((uint)(5));
            w6.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w7 = ((Gtk.ButtonBox.ButtonBoxChild)(w6[this.buttonCancel]));
            w7.Expand = false;
            w7.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-ok";
            this.AddActionWidget(this.buttonOk, -5);
            Gtk.ButtonBox.ButtonBoxChild w8 = ((Gtk.ButtonBox.ButtonBoxChild)(w6[this.buttonOk]));
            w8.Position = 1;
            w8.Expand = false;
            w8.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 460;
            this.DefaultHeight = 430;
            this.buttonOk.HasDefault = true;
            this.Show();
            this.buttonCancel.Clicked += new System.EventHandler(this.OnCancel);
            this.buttonOk.Clicked += new System.EventHandler(this.OnOKClicked);
        }
    }
}
