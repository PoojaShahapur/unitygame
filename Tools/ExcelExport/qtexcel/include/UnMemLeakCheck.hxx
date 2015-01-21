#ifndef __UNMEMLEAKCHECK_H
#define __UNMEMLEAKCHECK_H

#if defined _DEBUG
	#if defined new
		#undef new
		#pragma pop_macro("new")
	#endif
#endif

#endif // __UNMEMLEAKCHECK_H 