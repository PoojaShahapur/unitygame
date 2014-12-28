project(BTEditor)

cmake_minimum_required(VERSION 2.8.10)
#set(CMAKE_ALLOW_LOOSE_LOOP_CONSTRUCTS TRUE)
#cmake_policy(SET CMP0003 NEW)

set (CMAKE_PREFIX_PATH "C:/Program Files (x86)/Windows Kits/8.1/Lib/winv6.3/um/x64" ${CMAKE_PREFIX_PATH})

# CMake policy introduced in relation to Qt 5.1
if("${CMAKE_VERSION}" VERSION_GREATER 2.8.11)   
	cmake_policy(SET CMP0020 OLD)
endif()

set(INCS_FILES
	./Inc/Platform.h
	./Inc/Prerequisites.h
	./Inc/BTEditor.h
	./Inc/GlobalDefine.h
	./Inc/Aaa.h
)

set(SRCS_FILES
	./src/Aaa.cpp
)

source_group("Source Files" FILES ${SRCS_FILES})
source_group("Include Files" FILES ${INCS_FILES})

include_directories(./Inc)

ADD_DEFINITIONS(-DBTEDITOR_DLL -DBTEDITOR_DLL_EXPORT)

#set(ALL_SRCS ${SRCS_FILES} ${MOC_SRCS} ${UI_SRCS} ${INCS_FILES} ${QRC_SRCS} ${RC_FILES})
set(ALL_SRCS ${SRCS_FILES} ${INCS_FILES})

ADD_LIBRARY(BTEditor SHARED ${ALL_SRCS})

set_target_properties(BTEditor PROPERTIES DEBUG_POSTFIX _d)

#set(INST_FILES ${CMAKE_CURRENT_BINARY_DIR}/${PROJECT_NAME}/${CMAKE_BUILD_TYPE}/${PROJECT_NAME}${DEBUG_POSTFIX})
#set(INST_FILES ${LIBRARY_OUTPUT_DIR}/${PROJECT_NAME}${DEBUG_POSTFIX}.dll)
#set(INST_FILES ${CMAKE_CURRENT_BINARY_DIR}/${PROJECT_NAME}${DEBUG_POSTFIX}.dll)

#install(FILES 
#	${INST_FILES} DESTINATION "${CMAKE_BINARY_DIR}/${CMAKE_BUILD_TYPE}"
#)