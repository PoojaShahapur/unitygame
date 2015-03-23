#CXX = /usr/bin/g++
CXXFLAGS = $(ZEBRA_COMPILE_OPT) $(PIC_FLAG) -g -I../base -Wall -D_GNU_SOURCE -D_REENRANT `mysql_config --cflags` `xml2-config --cflags` 
CXXLFLAGS = $(ZEBRA_COMPILE_OPT) -Wall -lpthread  -lz `mysql_config --libs_r` `xml2-config --libs` -llog4cxx 
LIB_ENCDEC = ../libencdec.a
DYN_LIB_ENCDEC = ../libencdec.so

OBJS = EncDec.o myrc5.o mydes.o 

SRCS = $(OBJS:%.o=%.cpp)
DEPS = $(OBJS:%.o=.%.d)

all:$(LIB_ENCDEC)

#include $(DEPS)
#$(DEPS): .%.d:%.cpp
#	@$(CXX) -M $(CXXFLAGS) $< > $@.$$$$; sed 's,\($*\)\.o[ :]*,\1.o $@ : ,g' < $@.$$$$ > $@; rm -f $@.$$$$
	
$(OBJS): %.o: %.cpp
	$(CXX) -c $(CXXFLAGS) $< -o $@
	#gcc -c $(CXXFLAGS) md5c.c -o md5c.o

	
$(LIB_ENCDEC): $(OBJS) 
	@rm -rf $(LIB_ENCDEC)
	@ar cru $(LIB_ENCDEC) $(OBJS) #md5c.o
	@ranlib $(LIB_ENCDEC)
	@if [ -n "$(PIC_FLAG)" ]; then \
	    $(CXX) -shared -o $(DYN_LIB_ENCDEC) $(OBJS); \
	    fi

clean:
	@rm -rf $(OBJS) $(LIB_ENCDEC) core *~
