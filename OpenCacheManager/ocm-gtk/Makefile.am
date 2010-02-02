
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:3 -optimize- -debug "-define:DEBUG"
ASSEMBLY = bind/ocm-gtk.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../ocmengine/bin/Debug/ocmengine.dll
BUILD_DIR = bind

CITO_SVG_SOURCE=icons/scalable/cito.svg
EARTH_SVG_SOURCE=icons/scalable/earth.svg
FOUND_SVG_SOURCE=icons/scalable/found.svg
HALFSTAR_SVG_SOURCE=icons/scalable/halfstar.svg
LETTERBOX_SVG_SOURCE=icons/scalable/letterbox.svg
MULTI_SVG_SOURCE=icons/scalable/multi.svg
OTHER_SVG_SOURCE=icons/scalable/other.svg
STAR_SVG_SOURCE=icons/scalable/star.svg
STAR_EMPTY_SVG_SOURCE=icons/scalable/star_empty.svg
TRADITIONAL_SVG_SOURCE=icons/scalable/traditional.svg
UNKNOWN_SVG_SOURCE=icons/scalable/unknown.svg
WAYPOINT_FLAG_RED_SVG_SOURCE=icons/scalable/waypoint-flag-red.svg
WAYPOINT_FLAG_RED_PNG_SOURCE=icons/24x24/waypoint-flag-red.png
WPT_VIEWER_HTML_SOURCE=web/wpt_viewer.html
OCMENGINE_DLL_SOURCE=../ocmengine/bin/Debug/ocmengine.dll
OCMENGINE_DLL_MDB_SOURCE=../ocmengine/bin/Debug/ocmengine.dll.mdb
OCMENGINE_DLL_MDB=$(BUILD_DIR)/ocmengine.dll.mdb
OCM_GTK_EXE_MDB_SOURCE=bind/ocm-gtk.exe.mdb
OCM_GTK_EXE_MDB=$(BUILD_DIR)/ocm-gtk.exe.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/ocm-gtk.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../ocmengine/bin/Debug/ocmengine.dll
BUILD_DIR = bin

CITO_SVG_SOURCE=icons/scalable/cito.svg
EARTH_SVG_SOURCE=icons/scalable/earth.svg
FOUND_SVG_SOURCE=icons/scalable/found.svg
HALFSTAR_SVG_SOURCE=icons/scalable/halfstar.svg
LETTERBOX_SVG_SOURCE=icons/scalable/letterbox.svg
MULTI_SVG_SOURCE=icons/scalable/multi.svg
OTHER_SVG_SOURCE=icons/scalable/other.svg
STAR_SVG_SOURCE=icons/scalable/star.svg
STAR_EMPTY_SVG_SOURCE=icons/scalable/star_empty.svg
TRADITIONAL_SVG_SOURCE=icons/scalable/traditional.svg
UNKNOWN_SVG_SOURCE=icons/scalable/unknown.svg
WAYPOINT_FLAG_RED_SVG_SOURCE=icons/scalable/waypoint-flag-red.svg
WAYPOINT_FLAG_RED_PNG_SOURCE=icons/24x24/waypoint-flag-red.png
WPT_VIEWER_HTML_SOURCE=web/wpt_viewer.html
OCMENGINE_DLL_SOURCE=../ocmengine/bin/Debug/ocmengine.dll
OCMENGINE_DLL_MDB=
OCM_GTK_EXE_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES_ICONS_SCALABLE = \
	$(CITO_SVG) \
	$(EARTH_SVG) \
	$(FOUND_SVG) \
	$(HALFSTAR_SVG) \
	$(LETTERBOX_SVG) \
	$(MULTI_SVG) \
	$(OTHER_SVG) \
	$(STAR_SVG) \
	$(STAR_EMPTY_SVG) \
	$(TRADITIONAL_SVG) \
	$(UNKNOWN_SVG) \
	$(WAYPOINT_FLAG_RED_SVG)  

PROGRAMFILES_ICONS_24X24 = \
	$(WAYPOINT_FLAG_RED_PNG)  

PROGRAMFILES_WEB = \
	$(WPT_VIEWER_HTML)  

PROGRAMFILES =  \
	$(OCM_GTK_EXE_MDB) \
	$(OCMENGINE_DLL) \
	$(OCMENGINE_DLL_MDB)  

BINARIES = \
	$(OCM_GTK)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES_ICONS_SCALABLE) $(PROGRAMFILES_ICONS_24X24) $(PROGRAMFILES_WEB) $(PROGRAMFILES) $(BINARIES) 

FILES =  \
	AssemblyInfo.cs \
	Dialogs/OCMAboutDialog.cs \
	Dialogs/WaypointDialog.cs \
	gtk-gui/generated.cs \
	gtk-gui/MainWindow.cs \
	gtk-gui/ocmgtk.BrowserWidget.cs \
	gtk-gui/ocmgtk.CacheList.cs \
	gtk-gui/ocmgtk.GeocacheInfoPanel.cs \
	gtk-gui/ocmgtk.GeoCachePane.cs \
	gtk-gui/ocmgtk.LogViewerWidget.cs \
	gtk-gui/ocmgtk.OCMAboutDialog.cs \
	gtk-gui/ocmgtk.WaypointDialog.cs \
	gtk-gui/ocmgtk.WaypointWidget.cs \
	Main.cs \
	MainWindow.cs \
	UIMonitor.cs \
	Widgets/BrowserWidget.cs \
	Widgets/CacheList.cs \
	Widgets/GeocacheInfoPanel.cs \
	Widgets/GeoCachePane.cs \
	Widgets/HTMLWidget.cs \
	Widgets/LogViewerWidget.cs \
	Widgets/WaypointWidget.cs 

DATA_FILES = 

RESOURCES = gtk-gui/gui.stetic 

EXTRAS = \
	Dialogs \
	Widgets \
	web \
	icons \
	icons/scalable \
	icons/24x24 \
	ocm-gtk.in 

REFERENCES =  \
	../ocmengine/bin/Debug/ocmengine.dll \
	Mono.Posix \
	-pkg:gecko-sharp-2.0 \
	-pkg:gtkhtml-sharp-3.14 \
	-pkg:gtk-sharp-2.0 \
	-pkg:webkit-sharp-1.0 \
	System

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES_ICONS_SCALABLE) $(PROGRAMFILES_ICONS_24X24) $(PROGRAMFILES_WEB) $(PROGRAMFILES) $(BINARIES) 

include $(top_srcdir)/Makefile.include

CITO_SVG = $(BUILD_DIR)/icons/scalable/cito.svg
EARTH_SVG = $(BUILD_DIR)/icons/scalable/earth.svg
FOUND_SVG = $(BUILD_DIR)/icons/scalable/found.svg
HALFSTAR_SVG = $(BUILD_DIR)/icons/scalable/halfstar.svg
LETTERBOX_SVG = $(BUILD_DIR)/icons/scalable/letterbox.svg
MULTI_SVG = $(BUILD_DIR)/icons/scalable/multi.svg
OTHER_SVG = $(BUILD_DIR)/icons/scalable/other.svg
STAR_SVG = $(BUILD_DIR)/icons/scalable/star.svg
STAR_EMPTY_SVG = $(BUILD_DIR)/icons/scalable/star_empty.svg
TRADITIONAL_SVG = $(BUILD_DIR)/icons/scalable/traditional.svg
UNKNOWN_SVG = $(BUILD_DIR)/icons/scalable/unknown.svg
WAYPOINT_FLAG_RED_SVG = $(BUILD_DIR)/icons/scalable/waypoint-flag-red.svg
WAYPOINT_FLAG_RED_PNG = $(BUILD_DIR)/icons/24x24/waypoint-flag-red.png
WPT_VIEWER_HTML = $(BUILD_DIR)/web/wpt_viewer.html
OCMENGINE_DLL = $(BUILD_DIR)/ocmengine.dll
OCM_GTK = $(BUILD_DIR)/ocm-gtk

$(eval $(call emit-deploy-target,CITO_SVG))
$(eval $(call emit-deploy-target,EARTH_SVG))
$(eval $(call emit-deploy-target,FOUND_SVG))
$(eval $(call emit-deploy-target,HALFSTAR_SVG))
$(eval $(call emit-deploy-target,LETTERBOX_SVG))
$(eval $(call emit-deploy-target,MULTI_SVG))
$(eval $(call emit-deploy-target,OTHER_SVG))
$(eval $(call emit-deploy-target,STAR_SVG))
$(eval $(call emit-deploy-target,STAR_EMPTY_SVG))
$(eval $(call emit-deploy-target,TRADITIONAL_SVG))
$(eval $(call emit-deploy-target,UNKNOWN_SVG))
$(eval $(call emit-deploy-target,WAYPOINT_FLAG_RED_SVG))
$(eval $(call emit-deploy-target,WAYPOINT_FLAG_RED_PNG))
$(eval $(call emit-deploy-target,WPT_VIEWER_HTML))
$(eval $(call emit-deploy-target,OCMENGINE_DLL))
$(eval $(call emit-deploy-target,OCMENGINE_DLL_MDB))
$(eval $(call emit-deploy-wrapper,OCM_GTK,ocm-gtk,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
