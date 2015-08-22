#CXX = /usr/bin/g++
CXXFLAGS = $(ZEBRA_COMPILE_OPT) -I../base -Wall -D_GNU_SOURCE -D_REENRANT `mysql_config --cflags` `xml2-config --cflags` 
CXXLFLAGS = $(ZEBRA_COMPILE_OPT) -Wall -L../base -lzebra -lencdec -lpthread -lrt -lz `mysql_config --libs_r` `xml2-config --libs` -llog4cxx 
LIB_ZEBRA = ../base/libzebra.a
#LIB_CRYPTO = ../base/libcrypto.a

OBJS = \
 HttpTask.o \
 DumpServer.o \

SRCS = $(OBJS:%.o=%.cpp)
DEPS = $(OBJS:%.o=.%.d)

ALL_TARGETS = DumpServer

all: $(ALL_TARGETS)
include $(DEPS)
$(DEPS): .%.d: %.cpp
	@$(CXX) -M $(CXXFLAGS) $< > $@.$$$$; sed 's,\($*\)\.o[ :]*,\1.o $@ : ,g' < $@.$$$$ > $@; rm -f $@.$$$$
	
$(OBJS): %.o: %.cpp
	$(CXX) -c $(CXXFLAGS) $< -o $@
	
$(ALL_TARGETS): $(OBJS) $(LIB_ZEBRA)
	$(CXX) $(OBJS) -o $@ $(CXXLFLAGS)
	
clean:
	@rm -rf *.o $(ALL_TARGETS) core *~
