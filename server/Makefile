BUILD_DIR_ROOT = $(PWD)
BUILD_STRING = `date +%Y%m%d`" build"`cat $(BUILD_DIR_ROOT)/.build`
VERSIONSTRING = `if [ $(VERSION_STRING) ]; then echo &(VERSION_STRING); else echo "0.0.0"; fi`
ZEBRA_DEBUG_OPT = -g -Wno-deprecated -D_USE_CMD_NAMESPACE -D_ALL_SUPER_GM $(ZEBRA_OPTIONS) $(BATTLE_OPTIONS) -DVS=$(VERSIONSTRING)
ZEBRA_DEBUG_MOBILE_OPT = -g -Wno-deprecated -D_USE_CMD_NAMESPACE -D_ALL_SUPER_GM -D_MOBILE $(ZEBRA_OPTIONS) $(BATTLE_OPTIONS) -DVS=$(VERSIONSTRING)
ZEBRA_DEBUG_PG = -g -pg -Werror -D_USE_CMD_NAMESPACE -D_ALL_SUPER_GM $(ZEBRA_OPTIONS) $(BATTLE_OPTIONS) -DVS=$(VERSIONSTRING)
ZEBRA_DIST_OPT = -g -o2 -Werror -D_USE_CMD_NAMESPACE $(BATTLE_OPTIONS) -DVS=$(VERSIONSTRING)
PACTION_AUTOGETZEBRA_DIST_OPT = -g -o2 -Werror -D_USE_CMD_NAMESPACE $(BATTLE_OPTIONS) -DVS=$(VERSIONSTRING)

SUB_DIRS =  base/EncDec base/lzma920 base SessionServer BillServer GatewayServer RecordServer ScenesServer FLServer SuperServer tools Robot roleRegServer
TEST_SUB_DIRS =  base/EncDec base/lzma920 base roleChangeServer BroadcastServer DumpServer
ALL_SUB_DIRS = $(SUB_DIRS) $(TEST_SUB_DIRS)
NJOBS = `if [ "" == "$(NCPUS)" ]; then echo ""; else echo "-j$(NCPUS)"; fi`
export CXX = /usr/bin/g++

.PHONY : all debug clean mobile

all: debug ctags clean_d_file

all_mobile: mobile ctags clean_d_file

debug:
	@for dir in $(SUB_DIRS); \
	do \
	ZEBRA_COMPILE_OPT='$(ZEBRA_DEBUG_OPT) -D_BUILD_STRING=$(BUILD_STRING)' $(MAKE) $(NJOBS) -C $$dir || exit 1; \
	done

mobile:
	@for dir in $(SUB_DIRS); \
	do \
	ZEBRA_COMPILE_OPT='$(ZEBRA_DEBUG_MOBILE_OPT) -D_BUILD_STRING=$(BUILD_STRING)' $(MAKE) $(NJOBS) -C $$dir || exit 1; \
	done
	
ctags:
	@ctags -R -h ".c.cpp.h" -o tags

release:
	@tar -cvf Server`date +%Y%m%d%H%M%S`.tar *Server/*Server

tar:
	@tar -cvf Work`date +%Y%m%d`.tar /home/wang/WuLin/

docs:
	@doxygen

clean_d_file:
	@rm -rf RecordServer/.*.d 
	@rm -rf SuperServer/.*.d 
	@rm -rf SessionServer/.*.d 
	@rm -rf ScenesServer/.*.d 
	@rm -rf BillServer/.*.d 
	@rm -rf GatewayServer/.*.d 
	@rm -rf base/.*.d 
	@rm -rf Robot/.*.d 
	@rm -rf tools/.*.d 
	@rm -rf FLServer/.*.d 
	@rm -rf roleRegServer/.*.d 
	@rm -rf roleChangeServer/.*.d 
	@rm -rf BroadcastServer/.*.d 


clean:
	@for dir in $(ALL_SUB_DIRS); \
	do \
	$(MAKE) -C $$dir clean; \
	done
