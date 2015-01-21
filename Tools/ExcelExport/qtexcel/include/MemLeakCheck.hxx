#ifndef __MEMLEAKCHECK_H
#define __MEMLEAKCHECK_H

#if defined ENABLE_CRT_MEM_LEAK
	#if defined _DEBUG
		#pragma push_macro("new")
		#undef new
		#define DEBUG_NORMALBLOCK new(_NORMAL_BLOCK, __FILE__, __LINE__)

		#define _CRTDBG_MAP_ALLOC
		#include <stdlib.h>
		#include <crtdbg.h>
		#define new DEBUG_NORMALBLOCK
	#else
		#define DEBUG_NORMALBLOCK
	#endif
#endif

#endif // __MEMLEAKCHECK_H 