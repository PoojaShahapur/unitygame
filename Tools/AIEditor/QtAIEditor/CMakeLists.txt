project(QtAIEditor)

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

find_package(Qt5Widgets REQUIRED)

if(NOT Qt5Widgets_FOUND)
    message(SEND_ERROR "Failed to find Qt 5.0.0 or greater.")
else(NOT Qt5Widgets_FOUND)
    set(CMAKE_PACKAGE_QTGUI TRUE)

    file(GLOB UI_FILES *.ui)

    set(SRCS_FILES
	    ./src/Main.cpp
        ./src/MainWindow.cpp
		./src/LogWidget.cpp
		./src/ProjectWidget.cpp
		./src/CenterTabWidget.cpp
		./src/DragWidget.cpp
		./src/DraggablePiece.cpp
		./src/GraphicsScene.cpp
		./src/BoardHexagon.cpp
		./src/DraggableItemBase.cpp
		./src/DraggableItemWidget.cpp
		./src/GraphicsView.cpp
		./src/BezierCurveItem.cpp
		./src/DragDropSys.cpp
    )
	
	set(INCS_FILES
        ./Inc/MainWindow.h
		./Inc/LogWidget.h
		./Inc/ProjectWidget.h
		./Inc/CenterTabWidget.h
		./Inc/DragWidget.h
		./Inc/DraggablePiece.h
		./Inc/GraphicsScene.h
		./Inc/BoardHexagon.h
		./Inc/DraggableItemBase.h
		./Inc/DraggableItemWidget.h
		./Inc/GraphicsView.h
		./Inc/BezierCurveItem.h
		./Inc/DragDropSys.h
    )

	#these files need build moc files
    SET(MOC_FILES
        ./Inc/MainWindow.h
		./Inc/LogWidget.h
		./Inc/ProjectWidget.h
		./Inc/CenterTabWidget.h
		./Inc/DragWidget.h
		./Inc/GraphicsScene.h
		./Inc/DraggableItemWidget.h
		./Inc/GraphicsView.h
    )
	
	SET(QRC_FILES ./Res/Qrc.qrc)
	
	qt5_wrap_cpp(MOC_SRCS ${MOC_FILES})
    qt5_wrap_ui(UI_SRCS ${UI_FILES})
	qt5_add_resources(QRC_SRCS ${QRC_FILES})

    source_group("Moc Files" FILES ${MOC_SRCS})
    source_group("Ui Files" FILES ${UI_SRCS} ${UI_FILES})
	source_group("Qrc Files" FILES ${QRC_SRCS} ${QRC_FILES})
	
	file(GLOB RC_FILES ./Res/*.rc)
	source_group("Rc Files" FILES ${RC_FILES})
	
    source_group("Source Files" FILES ${SRCS_FILES})
    source_group("Include Files" FILES ${INCS_FILES})

    # if we don't include this CMake will not include ui headers properly:
    include_directories(${CMAKE_CURRENT_BINARY_DIR})
    include_directories(./Inc)
	include_directories(../AIEditor/Inc)
	include_directories(${QT_BINARY_DIR}/../include)
	
	link_directories(${QT_BINARY_DIR}/../lib)
	
	ADD_DEFINITIONS(-DAIEDITOR_DLL)
	
    set(ALLSRCS ${SRCS_FILES} ${MOC_SRCS} ${UI_SRCS} ${INCS_FILES} ${QRC_SRCS} ${RC_FILES})

    ADD_EXECUTABLE(QtAIEditor WIN32 ${ALLSRCS})

    set_target_properties(QtAIEditor PROPERTIES DEBUG_POSTFIX _d LINK_FLAGS "/ENTRY:\"mainCRTStartup\"")
	
	TARGET_LINK_LIBRARIES(QtAIEditor ${Qt5Widgets_LIBRARIES} AIEditor)
	
	add_dependencies(QtAIEditor AIEditor)
	
endif(NOT Qt5Widgets_FOUND)