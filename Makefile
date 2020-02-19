CSC			= /cygdrive/c/windows/microsoft.net/framework/v4.0.30319/csc.exe
RC			= "/cygdrive/c/Program Files (x86)/Windows Kits/10/bin/10.0.18362.0/x64/rc.exe"
7ZIP		= "/cygdrive/c/Program Files/7-Zip/7z.exe"
REPO		= https://github.com/Yanorei32/MultiShortcut

PROJ_NAME	= マルチショートカット
TARGET		= マルチショートカット(ショートカット作成).exe 
SRC			= src\\Program.cs \
			  src\\ScheduledTask.cs
SAMPLE_DIR	= サンプル
SET_DIR		= セット1
DROP_HERE	= ここに入れる

CSC_FLAGS	= /nologo \
			  /target:winexe \
			  /win32res:res\\resource.res \
			  /utf8output

LANG		= jp

DEBUG_FLAGS		= 
RELEASE_FLAGS	= 

ifeq ($(LANG),en)
	CSC_FLAGS+=/define:english
	TARGET		=	MultiShortcut(CreateShortcut).exe
	PROJ_NAME	=	MultiShortcut
	DROP_HERE	=	DropShortcutHere
	SAMPLE_DIR	=	Sample
	SET_DIR		=	Set1
endif

RELEASE_DIR	= $(PROJ_NAME)

.PHONY: debug
debug: CSC_FLAGS+=$(DEBUG_FLAGS)
debug: all

.PHONY: release
release: CSC_FLAGS+=$(RELEASE_FLAGS)
release: all

.PHONY: genzip
genzip: $(PROJ_NAME).zip

$(RELEASE_DIR)/README.url:
	echo -ne \
		"[InternetShortcut]\r\nURL=https://github.com/Yanorei32/MultiShortcut/blob/master/README.md" \
		> "$(RELEASE_DIR)/README.url"

$(RELEASE_DIR)/README(en).url:
	echo -ne \
		"[InternetShortcut]\r\nURL=https://github.com/Yanorei32/MultiShortcut/blob/master/README.en.md" \
		> "$(RELEASE_DIR)/README(en).url"

$(RELEASE_DIR)/LICENSE.txt: LICENSE
	-mkdir -p $(RELEASE_DIR)
	cp \
		LICENSE \
		$(RELEASE_DIR)/LICENSE.txt

demo_files: demo/*
	-mkdir -p $(RELEASE_DIR)
	-mkdir -p $(RELEASE_DIR)/$(SET_DIR)
	touch $(RELEASE_DIR)/$(SET_DIR)/$(DROP_HERE)
	-mkdir -p $(RELEASE_DIR)/$(SAMPLE_DIR)

	cp -r \
		demo/* \
		$(RELEASE_DIR)/$(SAMPLE_DIR)

res/resource.res: res/resource.rc res/*.ico
	cd res && $(RC) /r resource.rc


$(RELEASE_DIR)/$(TARGET): $(SRC) res/resource.res
	-mkdir -p $(RELEASE_DIR)
	$(CSC) $(CSC_FLAGS) "/out:$(RELEASE_DIR)\\$(TARGET)" $(SRC)

.PHONY: all
all: $(RELEASE_DIR)/$(TARGET) \
	$(RELEASE_DIR)/LICENSE.txt \
	$(RELEASE_DIR)/README.url \
	$(RELEASE_DIR)/README(en).url \
	demo_files

$(PROJ_NAME).zip: all
	rm -f $(RELEASE_DIR)/*.lnk
	$(7ZIP) a $(PROJ_NAME).zip $(RELEASE_DIR)

.PHONY: clean
clean:
	rm -rf \
		res/resource.res \
		$(RELEASE_DIR) \
		$(PROJ_NAME).zip


