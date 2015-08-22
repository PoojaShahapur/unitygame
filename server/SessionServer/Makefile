#CXX = /usr/bin/g++
CXXFLAGS = $(ZEBRA_COMPILE_OPT) -I../base -Wall -D_GNU_SOURCE -D_REENRANT `mysql_config --cflags` `xml2-config --cflags` `freetype-config --cflags`
CXXLFLAGS = $(ZEBRA_COMPILE_OPT) -Wall -L../base -lzebra -lencdec -llzma -lpthread -lrt -lz `mysql_config --libs_r` `xml2-config --libs` -llog4cxx
LIB_ZEBRA = ../base/libzebra.a

OBJS = \
       SessionServer.o \
       SessionTask.o \
       SessionTaskManager.o \
       Session.o \
       SessionManager.o \
       RecordClient.o \
       TimeTick.o \
       TempArchive.o \
       SessionChat.o \
       HeroCardManager.o \
       CRelationManager.o \
       CSort.o \
       CountryInfo.o \
       NewbieLimit.o \
       MailService.o \



SRCS = $(OBJS:%.o=%.cpp)
DEPS = $(OBJS:%.o=.%.d)

ALL_TARGETS = SessionServer

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
