#ifndef _PLATFORM_H
#define _PLATFORM_H

#if defined( __WIN32__ ) || defined( _WIN32 )
	#define stricmp _stricmp
	#define stricmp _stricmp
	#define stricmp _stricmp
#endif

typedef unsigned char uint8;
typedef char int8;

typedef unsigned short uint16;
typedef short int16;

typedef unsigned int uint32;
typedef int int32;

typedef unsigned _int64 uint64;
typedef _int64 int64;

//#define BEGINNAMESPACE(name) namespace name {
//#define ENDNAMESPACE(name) }
#define BEGINNAMESPACE(name)
#define ENDNAMESPACE(name)

#endif		// PLATFORM_H    