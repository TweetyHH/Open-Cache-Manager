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
        
        private Gtk.Button buttonCancel;
        
        private Gtk.Button buttonOk;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ocmgtk.FilterDialog
            this.Name = "ocmgtk.FilterDialog";
            this.Title = Mono.Unix.Catalog.GetString("Additional Filters...");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            // Internal child ocmgtk.FilterDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.notebook1 = new Gtk.Notebook();
            this.notebook1.CanFocus = true;
            this.notebook1.Name = "notebook1";
            this.notebook1.CurrentPage = 1;
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
            w1.Add(this.notebook1);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(w1[this.notebook1]));
            w4.Position = 0;
            // Internal child ocmgtk.FilterDialog.ActionArea
            Gtk.HButtonBox w5 = this.ActionArea;
            w5.Name = "dialog1_ActionArea";
            w5.Spacing = 10;
            w5.BorderWidth = ((uint)(5));
            w5.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w6 = ((Gtk.ButtonBox.ButtonBoxChild)(w5[this.buttonCancel]));
            w6.Expand = false;
            w6.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-ok";
            this.AddActionWidget(this.buttonOk, -5);
            Gtk.ButtonBox.ButtonBoxChild w7 = ((Gtk.ButtonBox.ButtonBoxChild)(w5[this.buttonOk]));
            w7.Position = 1;
            w7.Expand = false;
            w7.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 460;
            this.DefaultHeight = 430;
            this.Show();
            this.buttonCancel.Clicked += new System.EventHandler(this.OnCancel);
            this.buttonOk.Clicked += new System.EventHandler(this.OnOKClicked);
        }
    }
}
