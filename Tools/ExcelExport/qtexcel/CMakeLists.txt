project(QtExcel)
#set(QT_MIN_VERSION "4.8.0")

cmake_minimum_required(VERSION 2.8.10)
#set(CMAKE_ALLOW_LOOSE_LOOP_CONSTRUCTS TRUE)
#cmake_policy(SET CMP0003 NEW)

if (DEFINED ENV{QTDIR})
    set(CMAKE_PREFIX_PATH $ENV{QTDIR} ${CMAKE_PREFIX_PATH})
else ()
    message (FATAL_ERROR "ERROR: Environment variable QTDIR is not set. Please locate your Qt folder MY_QT5_DIR.")
endif ()

#set (CMAKE_MODULE_PATH "C:/Program Files (x86)/Windows Kits/8.1/Lib/winv6.3/um/x64")
set (CMAKE_PREFIX_PATH "C:/Program Files (x86)/Windows Kits/8.1/Lib/winv6.3/um/x64" ${CMAKE_PREFIX_PATH})

# CMake policy introduced in relation to Qt 5.1
if("${CMAKE_VERSION}" VERSION_GREATER 2.8.11)   
	cmake_policy(SET CMP0020 OLD)
endif()

#set(Qt4_FIND_REQUIRED FALSE)
#find_package(Qt4 COMPONENTS QtCore QtGui REQUIRED)

find_package(Qt5Widgets REQUIRED)

#if(NOT QT4_FOUND)
#    message(SEND_ERROR "Failed to find Qt 4.5 or greater.")
if(NOT Qt5Widgets_FOUND)
    message(SEND_ERROR "Failed to find Qt 5.0.0 or greater.")
#else(NOT QT4_FOUND)
else(NOT Qt5Widgets_FOUND)
    set(CMAKE_PACKAGE_QTGUI TRUE)

    file(GLOB UI_FILES *.ui)

    set(SRCS_FILES
        ./src/maindialog.cpp
        ./src/main.cpp
		./src/ExcelExport.cpp
		./src/Tools.cpp
		./src/WorkThread.cpp
		./src/DataItem.cpp
		./src/TabelAttr.cpp
		./src/CAppData.cpp
		./src/CTask.cpp
		./src/ByteBuffer.cpp
		./src/System.cpp
		./src/PropertyStr.cpp
		./src/TableListItem.cpp
		./src/ADOWrap.cpp
    )
	
	set(INCS_FILES
        ./include/maindialog.hxx
		./include/ExcelExport.hxx
		./include/Singleton.hxx
		./include/Tools.hxx
		./include/WorkThread.hxx
		./include/DataItem.hxx
		./include/Platform.hxx
		./include/TabelAttr.hxx
		./include/CAppData.hxx
		./include/CTask.hxx
		./include/ByteBuffer.hxx
		./include/ByteConverter.hxx
		./include/System.hxx
		./include/SystemEndian.hxx
		./include/Common.hxx
		./include/Error.hxx
		./include/PropertyBase.hxx
		./include/PropertyIns.hxx
		./include/PropertyStr.hxx
		./include/TableListItem.hxx
		./include/EnValue.hxx
		./include/ADOWrap.hxx
		./include/MemLeakCheck.hxx
    )

	#these files need build moc files
    SET(MOC_FILES
        ./include/maindialog.hxx
		./include/TableListItem.hxx
    )
	
	SET(QRC_FILES ./res/qtexcel.qrc)
	
    #qt4_wrap_cpp(MOC_SRCS ${MOC_FILES})
    #qt4_wrap_ui(UI_SRCS ${UI_FILES})
	
	qt5_wrap_cpp(MOC_SRCS ${MOC_FILES})
    qt5_wrap_ui(UI_SRCS ${UI_FILES})
	qt5_add_resources(QRC_SRCS ${QRC_FILES})

    source_group("Moc Files" FILES ${MOC_SRCS})
    source_group("Ui Files" FILES ${UI_SRCS})
	source_group("Qrc Files" FILES ${QRC_SRCS})
	
	file(GLOB RC_FILES ./res/*.rc)
	source_group("Rc Files" FILES ${RC_FILES})
	
    #source_group("Source Files" FILES ${SRCS})
    #source_group("Include Files" FILES ${INCS})

    # if we don't include this CMake will not include ui headers properly:
    include_directories(${CMAKE_CURRENT_BINARY_DIR})
    include_directories(./include)
	include_directories(../tinyxml)
	include_directories(${QT_BINARY_DIR}/../include)
	
	link_directories(${QT_BINARY_DIR}/../lib)
	
	#ADD_DEFINITIONS(-D_AFXDLL)
	
    set(SRCS ${SRCS_FILES} ${MOC_SRCS} ${UI_SRCS} ${INCS_FILES} ${QRC_SRCS} ${RC_FILES})

    ADD_EXECUTABLE(qtExcel WIN32 ${SRCS})

    set_target_properties(qtExcel PROPERTIES DEBUG_POSTFIX _d LINK_FLAGS "/ENTRY:\"mainCRTStartup\"")
	#TARGET_LINK_LIBRARIES(qtExcel QtCore4 QtGui4 tinyxml)
	TARGET_LINK_LIBRARIES(qtExcel ${Qt5Widgets_LIBRARIES} tinyxml)
	add_dependencies(qtExcel tinyxml)
endif(NOT Qt5Widgets_FOUND)