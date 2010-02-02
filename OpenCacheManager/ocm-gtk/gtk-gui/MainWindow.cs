// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------



public partial class MainWindow {
    
    private Gtk.UIManager UIManager;
    
    private Gtk.Action FileAction;
    
    private Gtk.Action QuitAction;
    
    private Gtk.Action EditAction;
    
    private Gtk.Action ViewAction;
    
    private Gtk.Action HelpAction;
    
    private Gtk.Action missingImageAction;
    
    private Gtk.Action openAction;
    
    private Gtk.Action saveAction;
    
    private Gtk.Action saveAsAction;
    
    private Gtk.Action OpenAction;
    
    private Gtk.Action SaveAction;
    
    private Gtk.Action ExportAction;
    
    private Gtk.Action AboutAction;
    
    private Gtk.Action convertAction;
    
    private Gtk.Action openAction1;
    
    private Gtk.ToggleAction ShowUnavailableCachesAction;
    
    private Gtk.ToggleAction ShowArchivedCachesAction;
    
    private Gtk.ToggleAction ShowCachesPlacedByMeAction;
    
    private Gtk.Action ClearAllFiltersAction;
    
    private Gtk.Action FilterAction;
    
    private Gtk.ToggleAction ShowFoundCachesAction;
    
    private Gtk.Action saveAsAction1;
    
    private Gtk.Action printAction;
    
    private Gtk.Action findAction;
    
    private Gtk.Action SearchAction;
    
    private Gtk.Action printPreviewAction;
    
    private Gtk.Action preferencesAction;
    
    private Gtk.Action MapAction;
    
    private Gtk.Action MapAction1;
    
    private Gtk.Action GPSAction;
    
    private Gtk.Action CacheAction;
    
    private Gtk.Action ViewOnlineAction;
    
    private Gtk.Action DeleteAction;
    
    private Gtk.VBox vbox1;
    
    private Gtk.MenuBar mainmenubar;
    
    private Gtk.Toolbar toolbar1;
    
    private Gtk.HPaned hSplitPane;
    
    private ocmgtk.CacheList cacheList;
    
    private Gtk.VPaned vpaned1;
    
    private ocmgtk.GeoCachePane cachePane;
    
    private ocmgtk.BrowserWidget browserwidget1;
    
    private Gtk.Statusbar statusbar1;
    
    protected virtual void Build() {
        Stetic.Gui.Initialize(this);
        // Widget MainWindow
        this.UIManager = new Gtk.UIManager();
        Gtk.ActionGroup w1 = new Gtk.ActionGroup("Default");
        this.FileAction = new Gtk.Action("FileAction", Mono.Unix.Catalog.GetString("_File"), null, null);
        this.FileAction.ShortLabel = Mono.Unix.Catalog.GetString("_File");
        w1.Add(this.FileAction, "<Alt><Mod2>f");
        this.QuitAction = new Gtk.Action("QuitAction", Mono.Unix.Catalog.GetString("_Quit"), null, "gtk-quit");
        this.QuitAction.ShortLabel = Mono.Unix.Catalog.GetString("_Quit");
        w1.Add(this.QuitAction, null);
        this.EditAction = new Gtk.Action("EditAction", Mono.Unix.Catalog.GetString("Edit"), null, null);
        this.EditAction.ShortLabel = Mono.Unix.Catalog.GetString("Edit");
        w1.Add(this.EditAction, null);
        this.ViewAction = new Gtk.Action("ViewAction", Mono.Unix.Catalog.GetString("View"), null, null);
        this.ViewAction.ShortLabel = Mono.Unix.Catalog.GetString("View");
        w1.Add(this.ViewAction, null);
        this.HelpAction = new Gtk.Action("HelpAction", Mono.Unix.Catalog.GetString("Help"), null, null);
        this.HelpAction.ShortLabel = Mono.Unix.Catalog.GetString("Help");
        w1.Add(this.HelpAction, null);
        this.missingImageAction = new Gtk.Action("missingImageAction", null, null, "gtk-missing-image");
        w1.Add(this.missingImageAction, null);
        this.openAction = new Gtk.Action("openAction", null, null, "gtk-open");
        w1.Add(this.openAction, null);
        this.saveAction = new Gtk.Action("saveAction", null, null, "gtk-save");
        w1.Add(this.saveAction, null);
        this.saveAsAction = new Gtk.Action("saveAsAction", null, null, "gtk-save-as");
        w1.Add(this.saveAsAction, null);
        this.OpenAction = new Gtk.Action("OpenAction", Mono.Unix.Catalog.GetString("Open"), null, null);
        this.OpenAction.ShortLabel = Mono.Unix.Catalog.GetString("Open");
        w1.Add(this.OpenAction, null);
        this.SaveAction = new Gtk.Action("SaveAction", Mono.Unix.Catalog.GetString("Save"), null, null);
        this.SaveAction.ShortLabel = Mono.Unix.Catalog.GetString("Save");
        w1.Add(this.SaveAction, null);
        this.ExportAction = new Gtk.Action("ExportAction", Mono.Unix.Catalog.GetString("Export..."), null, null);
        this.ExportAction.ShortLabel = Mono.Unix.Catalog.GetString("Export...");
        w1.Add(this.ExportAction, null);
        this.AboutAction = new Gtk.Action("AboutAction", Mono.Unix.Catalog.GetString("About"), null, "gtk-about");
        this.AboutAction.ShortLabel = Mono.Unix.Catalog.GetString("About");
        w1.Add(this.AboutAction, null);
        this.convertAction = new Gtk.Action("convertAction", null, null, "gtk-convert");
        w1.Add(this.convertAction, null);
        this.openAction1 = new Gtk.Action("openAction1", null, null, "gtk-open");
        w1.Add(this.openAction1, null);
        this.ShowUnavailableCachesAction = new Gtk.ToggleAction("ShowUnavailableCachesAction", Mono.Unix.Catalog.GetString("Show Unavailable Caches"), null, null);
        this.ShowUnavailableCachesAction.Active = true;
        this.ShowUnavailableCachesAction.ShortLabel = Mono.Unix.Catalog.GetString("Show Unavailable Caches");
        w1.Add(this.ShowUnavailableCachesAction, null);
        this.ShowArchivedCachesAction = new Gtk.ToggleAction("ShowArchivedCachesAction", Mono.Unix.Catalog.GetString("Show Archived Caches"), null, null);
        this.ShowArchivedCachesAction.Active = true;
        this.ShowArchivedCachesAction.ShortLabel = Mono.Unix.Catalog.GetString("Show Archived Caches");
        w1.Add(this.ShowArchivedCachesAction, null);
        this.ShowCachesPlacedByMeAction = new Gtk.ToggleAction("ShowCachesPlacedByMeAction", Mono.Unix.Catalog.GetString("Show Caches Placed by Me"), null, null);
        this.ShowCachesPlacedByMeAction.Active = true;
        this.ShowCachesPlacedByMeAction.ShortLabel = Mono.Unix.Catalog.GetString("Show Caches Placed by Me");
        w1.Add(this.ShowCachesPlacedByMeAction, null);
        this.ClearAllFiltersAction = new Gtk.Action("ClearAllFiltersAction", Mono.Unix.Catalog.GetString("Clear All Filters"), null, null);
        this.ClearAllFiltersAction.ShortLabel = Mono.Unix.Catalog.GetString("Clear All Filters");
        w1.Add(this.ClearAllFiltersAction, null);
        this.FilterAction = new Gtk.Action("FilterAction", Mono.Unix.Catalog.GetString("Filter..."), null, null);
        this.FilterAction.ShortLabel = Mono.Unix.Catalog.GetString("Filter...");
        w1.Add(this.FilterAction, null);
        this.ShowFoundCachesAction = new Gtk.ToggleAction("ShowFoundCachesAction", Mono.Unix.Catalog.GetString("Show Found Caches"), null, null);
        this.ShowFoundCachesAction.Active = true;
        this.ShowFoundCachesAction.ShortLabel = Mono.Unix.Catalog.GetString("Show Found Caches");
        w1.Add(this.ShowFoundCachesAction, null);
        this.saveAsAction1 = new Gtk.Action("saveAsAction1", null, null, "gtk-save-as");
        w1.Add(this.saveAsAction1, null);
        this.printAction = new Gtk.Action("printAction", null, null, "gtk-print");
        w1.Add(this.printAction, null);
        this.findAction = new Gtk.Action("findAction", null, null, "gtk-find");
        w1.Add(this.findAction, null);
        this.SearchAction = new Gtk.Action("SearchAction", Mono.Unix.Catalog.GetString("Search"), null, null);
        this.SearchAction.ShortLabel = Mono.Unix.Catalog.GetString("Search");
        w1.Add(this.SearchAction, null);
        this.printPreviewAction = new Gtk.Action("printPreviewAction", null, null, "gtk-print-preview");
        w1.Add(this.printPreviewAction, null);
        this.preferencesAction = new Gtk.Action("preferencesAction", null, null, "gtk-preferences");
        w1.Add(this.preferencesAction, null);
        this.MapAction = new Gtk.Action("MapAction", Mono.Unix.Catalog.GetString("_Map"), null, "map");
        this.MapAction.ShortLabel = Mono.Unix.Catalog.GetString("_Map");
        w1.Add(this.MapAction, null);
        this.MapAction1 = new Gtk.Action("MapAction1", Mono.Unix.Catalog.GetString("_Map"), null, "map");
        this.MapAction1.Sensitive = false;
        this.MapAction1.ShortLabel = Mono.Unix.Catalog.GetString("_Map");
        w1.Add(this.MapAction1, null);
        this.GPSAction = new Gtk.Action("GPSAction", Mono.Unix.Catalog.GetString("GPS"), null, null);
        this.GPSAction.Sensitive = false;
        this.GPSAction.ShortLabel = Mono.Unix.Catalog.GetString("GPS");
        w1.Add(this.GPSAction, null);
        this.CacheAction = new Gtk.Action("CacheAction", Mono.Unix.Catalog.GetString("Cache"), null, null);
        this.CacheAction.Sensitive = false;
        this.CacheAction.ShortLabel = Mono.Unix.Catalog.GetString("Cache");
        w1.Add(this.CacheAction, null);
        this.ViewOnlineAction = new Gtk.Action("ViewOnlineAction", Mono.Unix.Catalog.GetString("_View Online"), null, "gtk-info");
        this.ViewOnlineAction.ShortLabel = Mono.Unix.Catalog.GetString("_View Online");
        w1.Add(this.ViewOnlineAction, null);
        this.DeleteAction = new Gtk.Action("DeleteAction", Mono.Unix.Catalog.GetString("_Delete"), null, "gtk-delete");
        this.DeleteAction.ShortLabel = Mono.Unix.Catalog.GetString("_Delete");
        w1.Add(this.DeleteAction, null);
        this.UIManager.InsertActionGroup(w1, 0);
        this.AddAccelGroup(this.UIManager.AccelGroup);
        this.Name = "MainWindow";
        this.Title = Mono.Unix.Catalog.GetString("Open Cache Manager");
        this.WindowPosition = ((Gtk.WindowPosition)(1));
        // Container child MainWindow.Gtk.Container+ContainerChild
        this.vbox1 = new Gtk.VBox();
        this.vbox1.Name = "vbox1";
        // Container child vbox1.Gtk.Box+BoxChild
        this.UIManager.AddUiFromString("<ui><menubar name='mainmenubar'><menu name='FileAction' action='FileAction'><menuitem name='OpenAction' action='OpenAction'/><menuitem name='ExportAction' action='ExportAction'/><separator/><menuitem name='QuitAction' action='QuitAction'/></menu><menu name='EditAction' action='EditAction'/><menu name='ViewAction' action='ViewAction'><menuitem name='ShowFoundCachesAction' action='ShowFoundCachesAction'/><menuitem name='ShowUnavailableCachesAction' action='ShowUnavailableCachesAction'/><menuitem name='ShowArchivedCachesAction' action='ShowArchivedCachesAction'/><menuitem name='ShowCachesPlacedByMeAction' action='ShowCachesPlacedByMeAction'/><separator/><menuitem name='FilterAction' action='FilterAction'/><menuitem name='ClearAllFiltersAction' action='ClearAllFiltersAction'/></menu><menu name='SearchAction' action='SearchAction'/><menu name='CacheAction' action='CacheAction'><menuitem name='ViewOnlineAction' action='ViewOnlineAction'/><separator/><menuitem name='DeleteAction' action='DeleteAction'/></menu><menu name='GPSAction' action='GPSAction'/><menu name='HelpAction' action='HelpAction'><menuitem name='AboutAction' action='AboutAction'/></menu></menubar></ui>");
        this.mainmenubar = ((Gtk.MenuBar)(this.UIManager.GetWidget("/mainmenubar")));
        this.mainmenubar.Name = "mainmenubar";
        this.vbox1.Add(this.mainmenubar);
        Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox1[this.mainmenubar]));
        w2.Position = 0;
        w2.Expand = false;
        w2.Fill = false;
        // Container child vbox1.Gtk.Box+BoxChild
        this.UIManager.AddUiFromString("<ui><toolbar name='toolbar1'><toolitem name='openAction1' action='openAction1'/><toolitem name='saveAsAction1' action='saveAsAction1'/><separator/><toolitem name='preferencesAction' action='preferencesAction'/><separator/><toolitem name='findAction' action='findAction'/></toolbar></ui>");
        this.toolbar1 = ((Gtk.Toolbar)(this.UIManager.GetWidget("/toolbar1")));
        this.toolbar1.Name = "toolbar1";
        this.toolbar1.ShowArrow = false;
        this.toolbar1.ToolbarStyle = ((Gtk.ToolbarStyle)(2));
        this.toolbar1.IconSize = ((Gtk.IconSize)(3));
        this.vbox1.Add(this.toolbar1);
        Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.vbox1[this.toolbar1]));
        w3.Position = 1;
        w3.Expand = false;
        w3.Fill = false;
        // Container child vbox1.Gtk.Box+BoxChild
        this.hSplitPane = new Gtk.HPaned();
        this.hSplitPane.CanFocus = true;
        this.hSplitPane.Name = "hSplitPane";
        this.hSplitPane.Position = 398;
        // Container child hSplitPane.Gtk.Paned+PanedChild
        this.cacheList = null;
        this.hSplitPane.Add(this.cacheList);
        Gtk.Paned.PanedChild w4 = ((Gtk.Paned.PanedChild)(this.hSplitPane[this.cacheList]));
        w4.Resize = false;
        // Container child hSplitPane.Gtk.Paned+PanedChild
        this.vpaned1 = new Gtk.VPaned();
        this.vpaned1.CanFocus = true;
        this.vpaned1.Name = "vpaned1";
        this.vpaned1.Position = 441;
        // Container child vpaned1.Gtk.Paned+PanedChild
        this.cachePane = null;
        this.vpaned1.Add(this.cachePane);
        Gtk.Paned.PanedChild w5 = ((Gtk.Paned.PanedChild)(this.vpaned1[this.cachePane]));
        w5.Resize = false;
        // Container child vpaned1.Gtk.Paned+PanedChild
        this.browserwidget1 = null;
        this.vpaned1.Add(this.browserwidget1);
        this.hSplitPane.Add(this.vpaned1);
        this.vbox1.Add(this.hSplitPane);
        Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.vbox1[this.hSplitPane]));
        w8.Position = 2;
        // Container child vbox1.Gtk.Box+BoxChild
        this.statusbar1 = new Gtk.Statusbar();
        this.statusbar1.Name = "statusbar1";
        this.statusbar1.Spacing = 6;
        this.vbox1.Add(this.statusbar1);
        Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.vbox1[this.statusbar1]));
        w9.PackType = ((Gtk.PackType)(1));
        w9.Position = 3;
        w9.Expand = false;
        w9.Fill = false;
        this.Add(this.vbox1);
        if ((this.Child != null)) {
            this.Child.ShowAll();
        }
        this.DefaultWidth = 1073;
        this.DefaultHeight = 1201;
        this.Show();
        this.DeleteEvent += new Gtk.DeleteEventHandler(this.OnDeleteEvent);
        this.QuitAction.Activated += new System.EventHandler(this.OnQuit);
        this.AboutAction.Activated += new System.EventHandler(this.doAbout);
        this.openAction1.Activated += new System.EventHandler(this.OnOpenClicked);
        this.ShowUnavailableCachesAction.Toggled += new System.EventHandler(this.OnToggleUnavailable);
        this.ShowArchivedCachesAction.Toggled += new System.EventHandler(this.OnToggleArchive);
        this.ShowCachesPlacedByMeAction.Toggled += new System.EventHandler(this.OnToggleMine);
        this.ShowFoundCachesAction.Toggled += new System.EventHandler(this.OnToggleFound);
        this.MapAction1.Activated += new System.EventHandler(this.OnMapClick);
        this.ViewOnlineAction.Activated += new System.EventHandler(this.OnViewOnline);
    }
}
