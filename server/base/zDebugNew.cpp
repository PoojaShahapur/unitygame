/*
 * debug_new.cpp  1.11 2003/07/03
 *
 * Implementation of debug versions of new and delete to check leakage
 *
 * By Wu Yongwei
 *
 */

#include <new>
#include <stdio.h>
#include <stdlib.h>
#include <map>
//#include <ext/hash_map>
#include "Zebra.h"
#include "zMutex.h"

#if 0
#ifdef _MSC_VER
#pragma warning(disable: 4073)
#pragma init_seg(lib)
#endif
#endif

#ifndef DEBUG_NEW_HASHTABLESIZE
#define DEBUG_NEW_HASHTABLESIZE (16384*100)
#endif

#ifndef DEBUG_NEW_HASH
#define DEBUG_NEW_HASH(p) (((unsigned)(p) >> 8) % DEBUG_NEW_HASHTABLESIZE)
#endif

// The default behaviour now is to copy the file name, because we found
// that the exit leakage check cannot access the address of the file
// name sometimes (in our case, a core dump will occur when trying to
// access the file name in a shared library after a SIGINT).
#ifndef DEBUG_NEW_FILENAME_LEN
#define DEBUG_NEW_FILENAME_LEN	32
#endif

#if 0
#if DEBUG_NEW_FILENAME_LEN == 0 && !defined(DEBUG_NEW_NO_FILENAME_COPY)
#define DEBUG_NEW_NO_FILENAME_COPY
#endif
#ifndef DEBUG_NEW_NO_FILENAME_COPY
#include <string.h>
#endif
#endif

struct NewAddr
{
    const char* file;
    int line;
    size_t size;
    NewAddr(const char* _file, int _line, size_t _size)
	:file(_file),line(_line),size(_size){}
};

//__gnu_cxx::hash_map<QWORD, NewAddr> newAddrMap;
std::map<QWORD, NewAddr> newAddrMap;

static zMutex mem_mutex;
static zMutex mutex;

void addNewAddr(void* p, const char* file, int line, size_t size)
{
    mutex.lock();
    newAddrMap.insert(std::make_pair((QWORD)p, NewAddr(file, line, size)));
    mutex.unlock();
}

void removeNewAddr(void* p)
{
    mutex.lock();
    newAddrMap.erase((QWORD)p);
    mutex.unlock();
}
struct new_ptr_list_t
{
	new_ptr_list_t*		next;
#if 0
#ifdef DEBUG_NEW_NO_FILENAME_COPY
	const char*			file;
#else
	char				file[DEBUG_NEW_FILENAME_LEN];
#endif
#endif
	char                            file[DEBUG_NEW_FILENAME_LEN];
	int					line;
	size_t				size;
	new_ptr_list_t()
	{
	    next = NULL;
	    bzero(file, DEBUG_NEW_FILENAME_LEN);
	    line = 0;
	    size = 0;
	}
};

static new_ptr_list_t* new_ptr_list[DEBUG_NEW_HASHTABLESIZE];
size_t list_size = 0;

#if 0
bool new_verbose_flag = false;
bool new_autocheck_flag = true;
#endif

bool check_leaks()
{
    struct ListEntry
    {
	char name[DEBUG_NEW_FILENAME_LEN+8];
	int size;
    };
	bool fLeaked = false;
	size_t index = 0;
	ListEntry* tempList = NULL;
	mem_mutex.lock();
	tempList = (ListEntry*)malloc(sizeof(ListEntry)*list_size);
	if(!tempList)
	{
	    mem_mutex.unlock();
	    return false;
	}
	for (int i = 0; i < DEBUG_NEW_HASHTABLESIZE; ++i)
	{
		new_ptr_list_t* ptr = new_ptr_list[i];
		if (ptr == NULL)
			continue;
		fLeaked = true;
		while (ptr)
		{
		    if(index <list_size)
		    {
			snprintf(tempList[index].name, DEBUG_NEW_HASHTABLESIZE+8, "%s:%d",ptr->file, ptr->line);
			tempList[index].size = ptr->size;
			++index;
		    }
			ptr = ptr->next;
		}
	}
	mem_mutex.unlock();
	std::map<std::string, int> leak_count;
	for(size_t i=0; i<index; ++i)
	{
	    leak_count[tempList[i].name] += tempList[i].size;
	}
	free(tempList);
	for(std::map<std::string, int>::iterator it=leak_count.begin();
		it!=leak_count.end(); it++)
	{
	    Zebra::logger->warn("[ÄÚ´æÐ¹Â¶Í³¼Æ]:%s  size:%d",it->first.c_str(),it->second);
	}
	if (fLeaked)
		return true;
	else
		return false;
}

void* operator new(size_t size, const char* file, int line)
{
    size_t s = size + sizeof(new_ptr_list_t);
    new_ptr_list_t* ptr = (new_ptr_list_t*)malloc(s);
    if (ptr == NULL)
    {
	abort();
    }
    void* pointer = (char*)ptr + sizeof(new_ptr_list_t);
    size_t hash_index = DEBUG_NEW_HASH(pointer);
#ifdef DEBUG_NEW_NO_FILENAME_COPY
    ptr->file = file;
#else
    strncpy(ptr->file, file, DEBUG_NEW_FILENAME_LEN - 1);
    //ptr->file[DEBUG_NEW_FILENAME_LEN - 1] = '\0';
#endif

    ptr->line = line;
    ptr->size = size;
    mem_mutex.lock();
    ptr->next = new_ptr_list[hash_index];
    new_ptr_list[hash_index] = ptr;
    ++list_size;
    mem_mutex.unlock();
#if 0
    if (new_verbose_flag)
	printf("new:  allocated  %p (size %u, %s:%d)\n",
		pointer, size, file, line);
#endif
    return pointer;
}

void* operator new[](size_t size, const char* file, int line)
{
	return operator new(size, file, line);
}

void* operator new(size_t size)
{
    return ::malloc(size);
//	return operator new(size, "<Unknown>", 0);
}

void* operator new[](size_t size)
{
    return ::malloc(size);
//	return operator new(size);
}

void* operator new(size_t size, const std::nothrow_t&) throw()
{
	return operator new(size);
}

void* operator new[](size_t size, const std::nothrow_t&) throw()
{
	return operator new[](size);
}

void operator delete(void* pointer)
{
	if (pointer == NULL)
		return;
	size_t hash_index = DEBUG_NEW_HASH(pointer);
	new_ptr_list_t* ptr_pre = NULL;

	mem_mutex.lock();
	new_ptr_list_t* ptr = new_ptr_list[hash_index];
	while (ptr)
	{
		if ((char*)ptr + sizeof(new_ptr_list_t) == pointer)
		{
#if 0
			if (new_verbose_flag)
				printf("delete: freeing  %p (size %u)\n", pointer, ptr->size);
#endif
			if (ptr_pre == NULL)
				new_ptr_list[hash_index] = ptr->next;
			else
				ptr_pre->next = ptr->next;

			--list_size;
			mem_mutex.unlock();
			free(ptr);
			return;
		}
		ptr_pre = ptr;
		ptr = ptr->next;
	}
	mem_mutex.unlock();
	free(pointer);
}

void operator delete[](void* pointer)
{
	operator delete(pointer);
}

// Some older compilers like Borland C++ Compiler 5.5.1 and Digital Mars
// Compiler 8.29 do not support placement delete operators.
// NO_PLACEMENT_DELETE needs to be defined when using such compilers.
// Also note that in that case memory leakage will occur if an exception
// is thrown in the initialization (constructor) of a dynamically
// created object.
#ifndef NO_PLACEMENT_DELETE
void operator delete(void* pointer, const char* file, int line)
{
	//if (new_verbose_flag)
	//	printf("info: exception thrown on initializing object at %p (%s:%d)\n",
	//			pointer, file, line);
	operator delete(pointer);
}

void operator delete[](void* pointer, const char* file, int line)
{
	operator delete(pointer);
}

void operator delete(void* pointer, const std::nothrow_t&)
{
	operator delete(pointer, "<Unknown>", 0);
}

void operator delete[](void* pointer, const std::nothrow_t&)
{
	operator delete(pointer, "<Unknown>", 0);
}
#endif // NO_PLACEMENT_DELETE
#if 0
// Proxy class to automatically call check_leaks if new_autocheck_flag is set
class new_check_t
{
public:
	new_check_t() {}
	~new_check_t()
	{
		if (new_autocheck_flag)
		{
			// Check for leakage.
			// If any leaks are found, set new_verbose_flag so that any
			// delete operations in the destruction of global/static
			// objects will display information to compensate for
			// possible false leakage reports.
			if (check_leaks())
				new_verbose_flag = true;
		}
	}
};
static new_check_t new_check_object;
#endif

