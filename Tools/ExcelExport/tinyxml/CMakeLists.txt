project(tinyxml)

# 只处理 win32 平台  
if(WIN32)
	message(STATUS "TinyXml Project")
	set(SRCS
		./tinyxmlparser.cpp
		./tinystr.cpp
		./tinyxml.cpp
		./tinyxmlerror.cpp
	)

	set(INCS
		./tinystr.h
		./tinyxml.h
	)

	source_group("Source Files" FILES ${SRCS})
	source_group("Include Files" FILES ${INCS})

    # if we don't include this CMake will not include ui headers properly:
    include_directories(.)
    ADD_LIBRARY(tinyxml STATIC ${SRCS} ${INCS})
	set_target_properties(tinyxml PROPERTIES DEBUG_POSTFIX _d)
endif(WIN32)

# vim: set sw=2 ts=2 noet:
