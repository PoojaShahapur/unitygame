cmake_minimum_required(VERSION 3.1.0)

cmake_policy(SET CMP0003 NEW)
cmake_policy(SET CMP0005 NEW)

project(AnimEditor)

message(STATUS "builder main start")

set(CMAKE_DEBUG_POSTFIX "_d")

if(MSVC11 OR MSVC12)
	message(STATUS "Setting Visual Studio 2005+ option /MP to ${OGITOR_MSVC_ENABLE_MP}")
	add_definitions(-D_SCL_SECURE_NO_WARNINGS)
	add_definitions(-D_CRT_SECURE_NO_WARNINGS)
	add_definitions(-DNOMINMAX)
endif(MSVC11 OR MSVC12)

message(STATUS "CMAKE_CURRENT_BINARY_DIR: " ${CMAKE_CURRENT_BINARY_DIR})
set(CMAKE_INSTALL_PREFIX ${CMAKE_CURRENT_BINARY_DIR})

add_subdirectory(AnimEditor)