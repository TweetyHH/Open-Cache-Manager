
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

OCMENGINE_DLL_SOURCE=../ocmengine/bin/Debug/ocmengine.dll
OCMENGINE_DLL_MDB=
OCM_GTK_EXE_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES =  \
	$(OCM_GTK_EXE_MDB) \
	$(OCMENGINE_DLL) \
	$(OCMENGINE_DLL_MDB)  

BINARIES = \
	$(OCM_GTK)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES) 

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
	ocm-gtk-config.config \
	icons/scalable/cito.svg \
	icons/scalable/earth.svg \
	icons/scalable/found.svg \
	icons/scalable/halfstar.svg \
	icons/scalable/letterbox.svg \
	icons/scalable/multi.svg \
	icons/scalable/other.svg \
	icons/scalable/star.svg \
	icons/scalable/star_empty.svg \
	icons/scalable/traditional.svg \
	icons/scalable/unknown.svg \
	icons/scalable/waypoint-flag-red.svg \
	icons/24x24/waypoint-flag-red.png \
	web/wpt_viewer.html \
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

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

include $(top_srcdir)/Makefile.include

OCMENGINE_DLL = $(BUILD_DIR)/ocmengine.dll
OCM_GTK = $(BUILD_DIR)/ocm-gtk

$(eval $(call emit-deploy-target,OCMENGINE_DLL))
$(eval $(call emit-deploy-target,OCMENGINE_DLL_MDB))
$(eval $(call emit-deploy-wrapper,OCM_GTK,ocm-gtk,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
