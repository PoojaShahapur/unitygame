#CXX = /usr/bin/g++
CXXFLAGS = $(ZEBRA_COMPILE_OPT) $(PIC_FLAG)-I. -Wall -D_GNU_SOURCE -D_REENRANT `mysql_config --cflags` `xml2-config --cflags` 
CXXLFLAGS = $(ZEBRA_COMPILE_OPT) -Wall -lencdec -lpthread -llzma -lrt -lz `mysql_config --libs_r` `xml2-config --libs` -llog4cxx

LIB_ZEBRA = libzebra.a
DYN_LIB_ZEBRA = libzebra.so

OBJS = zArg.o zConfile.o Zebra.o zLogger.o zMetaData.o zMysqlDBConnPool.o zNetService.o \
       zProperties.o zRegex.o zService.o zSocket.o zSubNetService.o zTCPClient.o zTCPClientTask.o zTCPClientTaskPool.o \
       zTCPServer.o zTCPTask.o zTCPTaskPool.o zThread.o zTime.o zXMLParser.o zMTCPServer.o zMNetService.o \
       ServerACL.o zXML.o zDatabaseManager.o zDatabase.o SaveObject.o TableManager.o WordFilterManager.o\
       zHttpTask.o zHttpTaskPool.o #zDebugNew.o

SRCS = $(OBJS:%.o=%.cpp)
DEPS = $(OBJS:%.o=.%.d)

all: $(LIB_ZEBRA)

include $(DEPS)
$(DEPS): .%.d: %.cpp
	@$(CXX) -M $(CXXFLAGS) $< > $@.$$$$; sed 's,\($*\)\.o[ :]*,\1.o $@ : ,g' < $@.$$$$ > $@; rm -f $@.$$$$
	
$(OBJS): %.o: %.cpp
	$(CXX) -c $(CXXFLAGS) $< -o $@
	
$(LIB_ZEBRA): $(OBJS) 
	@rm -rf $(LIB_ZEBRA)
	@ar cru $(LIB_ZEBRA) $(OBJS)
	@ranlib $(LIB_ZEBRA)
	@if [ -n "$(PIC_FLAG)" ]; then \
	    $(CXX) -shared -o $(DYN_LIB_ZEBRA) $(OBJS); \
	    fi

clean:
	@rm -rf $(OBJS) $(LIB_ZEBRA) core *~

