cmake_minimum_required(VERSION 2.6)

cmake_policy(SET CMP0003 NEW)
cmake_policy(SET CMP0005 NEW)

project(BTEditor)

message(STATUS "builder main start")

set(CMAKE_DEBUG_POSTFIX "_d")

set(CMAKE_INSTALL_PREFIX ${CMAKE_CURRENT_BINARY_DIR})

# if(MSVC)
if(MSVC11 OR MSVC12)
	message(STATUS "Setting Visual Studio 2005+ option /MP to ${OGITOR_MSVC_ENABLE_MP}")
	add_definitions(-D_SCL_SECURE_NO_WARNINGS)
	add_definitions(-D_CRT_SECURE_NO_WARNINGS)
	add_definitions(-DNOMINMAX)
endif(MSVC11 OR MSVC12)

add_subdirectory(BTEditor)
add_subdirectory(QtBTEditor)

install(FILES 
	${CMAKE_SOURCE_DIR}/config.xml DESTINATION "${CMAKE_BINARY_DIR}/qtexcel"
)