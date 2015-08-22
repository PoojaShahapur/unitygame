/*
 * debug_new.h  1.7 2003/07/03
 *
 * Header file for checking leakage by operator new
 *
 * By Wu Yongwei
 *
 */

#ifndef _DEBUG_NEW_H
#define _DEBUG_NEW_H

#include <new>

/* Prototypes */
bool check_leaks();
void* operator new(size_t size, const char* file, int line);
void* operator new[](size_t size, const char* file, int line);

//#ifndef NO_PLACEMENT_DELETE
void operator delete(void* pointer, const char* file, int line);
void operator delete[](void* pointer, const char* file, int line);
//#endif // NO_PLACEMENT_DELETE


void addNewAddr(void* p, const char* file, int line, size_t size);
void removeNewAddr(void* p);

/* Macros */
#if 0
#ifndef DEBUG_NEW_NO_NEW_REDEFINITION
#define new DEBUG_NEW
#define DEBUG_NEW new(__FILE__, __LINE__)
#define debug_new new
#else
#define debug_new new(__FILE__, __LINE__)
#endif // DEBUG_NEW_NO_NEW_REDEFINITION
#endif

#define DEBUG_NEW new(__FILE__, __LINE__)
#if 0
#ifdef DEBUG_NEW_EMULATE_MALLOC
#include <stdlib.h>
#define malloc(s) ((void*)(debug_new char[s]))
#define free(p) delete[] (char*)(p)
#endif // DEBUG_NEW_EMULATE_MALLOC

/* Control flags */
extern bool new_verbose_flag;	// default to false: no verbose information
extern bool new_autocheck_flag;	// default to true: call check_leaks() on exit
#endif

#endif // _DEBUG_NEW_H
