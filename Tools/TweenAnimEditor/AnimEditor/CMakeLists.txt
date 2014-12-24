project(AnimEditor)

cmake_minimum_required(VERSION 3.1.0)

if (DEFINED ENV{QTDIR})
    set(CMAKE_PREFIX_PATH $ENV{QTDIR} ${CMAKE_PREFIX_PATH})
else ()
    message (FATAL_ERROR "ERROR: Environment variable QTDIR is not set. Please locate your Qt folder MY_QT5_DIR.")
endif ()

set (CMAKE_PREFIX_PATH "C:/Program Files (x86)/Windows Kits/8.1/Lib/winv6.3/um/x64" ${CMAKE_PREFIX_PATH})

# CMake policy introduced in relation to Qt 5.1
if("${CMAKE_VERSION}" VERSION_GREATER 2.8.11)   
	cmake_policy(SET CMP0020 OLD)
endif()

find_package(Qt5Widgets REQUIRED)

if(NOT Qt5Widgets_FOUND)
    message(SEND_ERROR "Failed to find Qt 5.0.0 or greater.")
else(NOT Qt5Widgets_FOUND)
    set(CMAKE_PACKAGE_QTGUI TRUE)

    file(GLOB UI_FILES *.ui)

    set(SRCS_FILES
        ./src/boardhexagon.cpp
        ./src/draggablepiece.cpp
		./src/graphicsscene.cpp
		./src/main.cpp
		./src/widget.cpp
    )
	
	set(INCS_FILES
        ./inc/boardhexagon.h
		./inc/draggablepiece.h
		./inc/graphicsscene.h
		./inc/widget.h
    )

	#these files need build moc files
    SET(MOC_FILES
        ./inc/widget.h
		./inc/graphicsscene.h
    )
	
	SET(QRC_FILES ./res/AnimEditor.qrc)
	
	qt5_wrap_cpp(MOC_SRCS ${MOC_FILES})
    qt5_wrap_ui(UI_SRCS ${UI_FILES})
	qt5_add_resources(QRC_SRCS ${QRC_FILES})

    source_group("Moc Files" FILES ${MOC_SRCS})
    source_group("Ui Files" FILES ${UI_SRCS})
	source_group("Qrc Files" FILES ${QRC_SRCS})
	
	file(GLOB RC_FILES ./res/*.rc)
	source_group("Rc Files" FILES ${RC_FILES})

    # if we don't include this CMake will not include ui headers properly:
    include_directories(${CMAKE_CURRENT_BINARY_DIR})
    include_directories(./inc)
	include_directories(${QT_BINARY_DIR}/../include)
	
	link_directories(${QT_BINARY_DIR}/../lib)
	
	#ADD_DEFINITIONS(-D_AFXDLL)
	
    set(SRCS ${SRCS_FILES} ${MOC_SRCS} ${UI_SRCS} ${INCS_FILES} ${QRC_SRCS} ${RC_FILES})

    ADD_EXECUTABLE(AnimEditor WIN32 ${SRCS})

    set_target_properties(AnimEditor PROPERTIES DEBUG_POSTFIX _d LINK_FLAGS "/ENTRY:\"mainCRTStartup\"")
	TARGET_LINK_LIBRARIES(AnimEditor ${Qt5Widgets_LIBRARIES})
endif(NOT Qt5Widgets_FOUND)