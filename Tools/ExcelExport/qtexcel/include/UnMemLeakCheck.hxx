#ifndef __UNMEMLEAKCHECK_H
#define __UNMEMLEAKCHECK_H

#if defined ENABLE_CRT_MEM_LEAK
	#if defined _DEBUG
		#undef _CRTDBG_MAP_ALLOC
		#if defined new
			#undef new
			#pragma pop_macro("new")
		#endif
	#endif
#endif

#endif // __UNMEMLEAKCHECK_H 