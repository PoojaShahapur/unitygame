project(vld)

cmake_minimum_required(VERSION 2.8.10)

set (CMAKE_PREFIX_PATH "C:/Program Files (x86)/Windows Kits/8.1/Lib/winv6.3/um/x64" ${CMAKE_PREFIX_PATH})

# CMake policy introduced in relation to Qt 5.1
if("${CMAKE_VERSION}" VERSION_GREATER 2.8.11)   
	cmake_policy(SET CMP0020 OLD)
endif()

set(INCS_FILES
	./src/callstack.h
	./src/dbghelpapi.h
	./src/map.h
	./src/ntapi.h
	./src/set.h
	./src/tree.h
	./src/utility.h
	./src/vld.h
	./src/vldheap.h
	./src/vldint.h
)

set(SRCS_FILES
	./src/callstack.cpp
	./src/dbghelpapi.cpp
	./src/ntapi.cpp
	./src/utility.cpp
	./src/vld.cpp
	./src/vldapi.cpp
	./src/vldheap.cpp
)

source_group("Source Files" FILES ${SRCS_FILES})
source_group("Include Files" FILES ${INCS_FILES})

include_directories(./Inc)

set(ALL_SRCS ${SRCS_FILES} ${INCS_FILES})

ADD_LIBRARY(vld SHARED ${ALL_SRCS})

set_target_properties(vld PROPERTIES DEBUG_POSTFIX _d)