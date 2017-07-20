#ifndef SVR_COMMON_LOG_H
#define SVR_COMMON_LOG_H

// log.h
// log4cxx helper.
// Author: Jin Qing (http://blog.csdn.net/jq0123)

#include <log4cxx/logger.h>

#include "fmt.h"  // for Fmt()

#ifdef _WIN32
#include <Windows.h>
#endif

// Must define LOG_NAME first.
// Usage:
// const char LOG_NAME[] = "MyClass";
// ...
// LOG_INFO("Hello world!");

#ifdef _DEBUG
#ifdef _WIN32
	#define LOG_DEBUG_TO(name, message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE); LOG4CXX_DEBUG(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_INFO_TO(name, message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_INTENSITY | FOREGROUND_GREEN ); LOG4CXX_INFO(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_WARN_TO(name, message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_INTENSITY | FOREGROUND_GREEN | FOREGROUND_BLUE); LOG4CXX_WARN(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_ERROR_TO(name, message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_GREEN); LOG4CXX_ERROR(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_FATAL_TO(name, message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_INTENSITY | FOREGROUND_RED);LOG4CXX_FATAL(::log4cxx::Logger::getLogger(name), message); } while(0)

	#define LOG_DEBUG(message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE); LOG4CXX_DEBUG(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)
	#define LOG_INFO(message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_INTENSITY | FOREGROUND_GREEN ); LOG4CXX_INFO(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)	
	#define LOG_WARN(message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_INTENSITY | FOREGROUND_GREEN | FOREGROUND_BLUE); LOG4CXX_WARN(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)
	#define LOG_ERROR(message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_GREEN); LOG4CXX_ERROR(::log4cxx::Logger::getLogger(LOG_NAME), message); } while (0)
	#define LOG_FATAL(message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_INTENSITY | FOREGROUND_RED); LOG4CXX_FATAL(::log4cxx::Logger::getLogger(LOG_NAME), message);} while (0)
#else
	#include <iostream>

	#define LOG_DEBUG_TO(name, message) do {std::cout << "\033[0m"; LOG4CXX_DEBUG(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_INFO_TO(name, message) do {std::cout << "\033[1;32m"; LOG4CXX_INFO(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_WARN_TO(name, message) do {std::cout << "\033[1;36m"; LOG4CXX_WARN(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_ERROR_TO(name, message) do {std::cout << "\033[1;33m"; LOG4CXX_ERROR(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_FATAL_TO(name, message) do {std::cout << "\033[1;31m"; LOG4CXX_FATAL(::log4cxx::Logger::getLogger(name), message); } while(0)

	#define LOG_DEBUG(message) do {std::cout << "\033[0m";LOG4CXX_DEBUG(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)
	#define LOG_INFO(message) do {std::cout << "\033[1;32m"; LOG4CXX_INFO(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)
	#define LOG_WARN(message) do {std::cout << "\033[1;36m"; LOG4CXX_WARN(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)
	#define LOG_ERROR(message) do {std::cout << "\033[1;33m"; LOG4CXX_ERROR(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)
	#define LOG_FATAL(message) do {std::cout << "\033[1;31m"; LOG4CXX_FATAL(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)

#endif
#else
#ifdef _WIN32
	#define LOG_DEBUG_TO(name, message) do { LOG4CXX_DEBUG(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_INFO_TO(name, message) do { LOG4CXX_INFO(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_WARN_TO(name, message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_INTENSITY | FOREGROUND_GREEN | FOREGROUND_BLUE); LOG4CXX_WARN(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_ERROR_TO(name, message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_GREEN); LOG4CXX_ERROR(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_FATAL_TO(name, message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_INTENSITY | FOREGROUND_RED);LOG4CXX_FATAL(::log4cxx::Logger::getLogger(name), message); } while(0)

	#define LOG_DEBUG(message) do { LOG4CXX_DEBUG(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)
	#define LOG_INFO(message) do { LOG4CXX_INFO(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)	
	#define LOG_WARN(message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_INTENSITY | FOREGROUND_GREEN | FOREGROUND_BLUE); LOG4CXX_WARN(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)
	#define LOG_ERROR(message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_GREEN); LOG4CXX_ERROR(::log4cxx::Logger::getLogger(LOG_NAME), message); } while (0)
	#define LOG_FATAL(message) do {SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE),FOREGROUND_INTENSITY | FOREGROUND_RED); LOG4CXX_FATAL(::log4cxx::Logger::getLogger(LOG_NAME), message);} while (0)
#else
	#include <iostream>

	#define LOG_DEBUG_TO(name, message) do {LOG4CXX_DEBUG(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_INFO_TO(name, message) do {LOG4CXX_INFO(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_WARN_TO(name, message) do {std::cout << "\033[1;36m"; LOG4CXX_WARN(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_ERROR_TO(name, message) do {std::cout << "\033[1;33m"; LOG4CXX_ERROR(::log4cxx::Logger::getLogger(name), message); } while(0)
	#define LOG_FATAL_TO(name, message) do {std::cout << "\033[1;31m"; LOG4CXX_FATAL(::log4cxx::Logger::getLogger(name), message); } while(0)

	#define LOG_DEBUG(message) do {LOG4CXX_DEBUG(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)
	#define LOG_INFO(message) do {LOG4CXX_INFO(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)
	#define LOG_WARN(message) do {std::cout << "\033[1;36m"; LOG4CXX_WARN(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)
	#define LOG_ERROR(message) do {std::cout << "\033[1;33m"; LOG4CXX_ERROR(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)
	#define LOG_FATAL(message) do {std::cout << "\033[1;31m"; LOG4CXX_FATAL(::log4cxx::Logger::getLogger(LOG_NAME), message); } while(0)
#endif

#endif
#endif  // SVR_COMMON_LOG_H
