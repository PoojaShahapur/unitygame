#ifndef __MEMLEAKCHECK_H
#define __MEMLEAKCHECK_H

#ifdef _DEBUG
	#define DEBUG_NORMALBLOCK new(_NORMAL_BLOCK, __FILE__, __LINE__)

	#define _CRTDBG_MAP_ALLOC
	#include <crtdbg.h>
	#define new DEBUG_NORMALBLOCK
#else
	#define DEBUG_NORMALBLOCK
#endif

#endif // __MEMLEAKCHECK_H 