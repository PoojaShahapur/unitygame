cmake_minimum_required(VERSION 2.6)

cmake_policy(SET CMP0003 NEW)
cmake_policy(SET CMP0005 NEW)

project(ExcelMake)

message(STATUS "builder main start")

set(CMAKE_DEBUG_POSTFIX "_d")

if(MSVC80 OR MSVC90 OR MSVC10)
        message(STATUS "Setting Visual Studio 2005+ option /MP to ${OGITOR_MSVC_ENABLE_MP}")
        add_definitions(-D_SCL_SECURE_NO_WARNINGS)
        add_definitions(-D_CRT_SECURE_NO_WARNINGS)
        add_definitions(-DNOMINMAX)
endif(MSVC80 OR MSVC90 OR MSVC10)

add_subdirectory(tinyxml)
add_subdirectory(qtexcel)
