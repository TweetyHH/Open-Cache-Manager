
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:3 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../../bind/ocm-gtk.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../../../bind/ocmengine.dll
BUILD_DIR = ../../../bind

LIBSTETIC_DLL_SOURCE=../../../../../../usr/lib/monodevelop/AddIns/MonoDevelop.GtkCore/libstetic.dll
LIBSTETIC_DLL_CONFIG_SOURCE=../../../../../../usr/lib/monodevelop/AddIns/MonoDevelop.GtkCore/libstetic.dll.config
OCMENGINE_DLL_SOURCE=../../../bind/ocmengine.dll
OCMENGINE_DLL_MDB_SOURCE=../../../bind/ocmengine.dll.mdb
OCMENGINE_DLL_MDB=$(BUILD_DIR)/ocmengine.dll.mdb
OCM_GTK_EXE_MDB_SOURCE=../../../bind/ocm-gtk.exe.mdb
OCM_GTK_EXE_MDB=$(BUILD_DIR)/ocm-gtk.exe.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/Release/ocm-gtk.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../ocmengine/bin/Release/ocmengine.dll
BUILD_DIR = bin/Release

LIBSTETIC_DLL_SOURCE=../../../../../../usr/lib/monodevelop/AddIns/MonoDevelop.GtkCore/libstetic.dll
LIBSTETIC_DLL_CONFIG_SOURCE=../../../../../../usr/lib/monodevelop/AddIns/MonoDevelop.GtkCore/libstetic.dll.config
OCMENGINE_DLL_SOURCE=../ocmengine/bin/Release/ocmengine.dll
OCMENGINE_DLL_MDB=
OCM_GTK_EXE_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(LIBSTETIC_DLL) \
	$(LIBSTETIC_DLL_CONFIG) \
	$(OCMENGINE_DLL) \
	$(OCMENGINE_DLL_MDB) \
	$(OCM_GTK_EXE_MDB)  

BINARIES = \
	$(OCM_GTK)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES) 

FILES = \
	gtk-gui/generated.cs \
	MainWindow.cs \
	gtk-gui/MainWindow.cs \
	Main.cs \
	AssemblyInfo.cs \
	gtk-gui/ocmgtk.GeocacheInfoPanel.cs \
	gtk-gui/ocmgtk.GeoCachePane.cs \
	gtk-gui/ocmgtk.CacheList.cs \
	gtk-gui/ocmgtk.OCMAboutDialog.cs \
	gtk-gui/ocmgtk.MapWidget.cs \
	Widgets/CacheList.cs \
	Widgets/MapWidget.cs \
	Widgets/GeocacheInfoPanel.cs \
	Widgets/GeoCachePane.cs \
	Dialogs/OCMAboutDialog.cs 

DATA_FILES = 

RESOURCES = \
	gtk-gui/gui.stetic \
	resources/map.svg \
	../icons/scalable/hicolor/traditional.svg 

EXTRAS = \
	ocm-gtk.in 

REFERENCES =  \
	System \
	$(GTK_SHARP_20_LIBS) \
	Mono.Posix \
	$(WEBKIT_SHARP_10_LIBS)

DLL_REFERENCES =  \
	../../../../../../usr/lib/monodevelop/AddIns/MonoDevelop.GtkCore/libstetic.dll

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

include $(top_srcdir)/Makefile.include

LIBSTETIC_DLL = $(BUILD_DIR)/libstetic.dll
LIBSTETIC_DLL_CONFIG = $(BUILD_DIR)/libstetic.dll.config
OCMENGINE_DLL = $(BUILD_DIR)/ocmengine.dll
OCM_GTK = $(BUILD_DIR)/ocm-gtk

$(eval $(call emit-deploy-target,LIBSTETIC_DLL))
$(eval $(call emit-deploy-target,LIBSTETIC_DLL_CONFIG))
$(eval $(call emit-deploy-target,OCMENGINE_DLL))
$(eval $(call emit-deploy-wrapper,OCM_GTK,ocm-gtk,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
