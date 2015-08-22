#CXX = /usr/bin/g++
CXXFLAGS = $(ZEBRA_COMPILE_OPT) $(PIC_FLAG) -g -I../base -Wall -D_GNU_SOURCE -D_REENRANT -D_7ZIP_ST `mysql_config --cflags` `xml2-config --cflags` 
CXXLFLAGS = $(ZEBRA_COMPILE_OPT) -Wall -lpthread  -lz  `mysql_config --libs_r` `xml2-config --libs` -llog4cxx 
LIB_LZMA = ../liblzma.a
DYN_LIB_LZMA = ../liblzma.so

OBJS = MLzma.o 
       #Alloc.o \	.c
       #LzFind.o \	.c
       #LzmaDec.o \	.c
       #LzmaEnc.o \	.c
       #LzmaLib.o \	.c


SRCS = $(OBJS:%.o=%.cpp)
DEPS = $(OBJS:%.o=.%.d)

all:$(LIB_LZMA)

#include $(DEPS)
#$(DEPS): .%.d:%.cpp
#	@$(CXX) -M $(CXXFLAGS) $< > $@.$$$$; sed 's,\($*\)\.o[ :]*,\1.o $@ : ,g' < $@.$$$$ > $@; rm -f $@.$$$$
	
$(OBJS): %.o: %.cpp
	$(CXX) -c $(CXXFLAGS) $< -o $@
	gcc -c $(CXXFLAGS) Alloc.c -o Alloc.o
	gcc -c $(CXXFLAGS) LzFind.c -o LzFind.o
	gcc -c $(CXXFLAGS) LzmaDec.c -o LzmaDec.o
	gcc -c $(CXXFLAGS) LzmaEnc.c -o LzmaEnc.o
	gcc -c $(CXXFLAGS) LzmaLib.c -o LzmaLib.o

	
$(LIB_LZMA): $(OBJS) 
	@rm -rf $(LIB_LZMA)
	@ar cru $(LIB_LZMA) $(OBJS) Alloc.o LzFind.o LzmaDec.o LzmaEnc.o LzmaLib.o
	@ranlib $(LIB_LZMA)
	@if [ -n "$(PIC_FLAG)" ]; then \
	    $(CXX) -shared -o $(DYN_LIB_LZMA) $(OBJS) Alloc.o LzFind.o LzmaDec.o LzmaEnc.o LzmaLib.o; \
	    fi

clean:
	@rm -rf $(OBJS) $(LIB_LZMA) core *~
