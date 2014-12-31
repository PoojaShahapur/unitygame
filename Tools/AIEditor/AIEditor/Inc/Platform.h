#ifndef __PLATFORM_H_
#define __PLATFORM_H_

#include "GlobalDefine.h"

BEGIN_NAMESPACE_AIEDITOR

#if defined(WIN32)
	#if !defined(BTEDITOR_DLL)
	#    define BTEDITOR_EXPORT
	#else
	#    if defined( BTEDITOR_DLL_EXPORT )
	#        define BTEDITOR_EXPORT __declspec( dllexport )
	#    else
	#        define BTEDITOR_EXPORT __declspec( dllimport )
	#    endif
	#endif
#endif

typedef unsigned int uint32;
typedef unsigned short uint16;
typedef unsigned char uint8;
typedef int int32;
typedef short int16;
typedef signed char int8;
// define uint64 type
#if defined(WIN32)
    typedef unsigned __int64 uint64;
    typedef __int64 int64;
#else
    typedef unsigned long long uint64;
    typedef long long int64;
#endif

END_NAMESPACE_AIEDITOR

#endif