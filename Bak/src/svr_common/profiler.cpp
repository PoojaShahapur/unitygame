
#ifdef ENABLE_GVOE_GPROFTOOLS

#include "profiler.h"
#include <string>
#include <cassert>

#ifdef _MSC_VER
#define PERFTOOLS_DLL_DECL
#include <gperftools/tcmalloc.h>
#include <gperftools/malloc_extension.h>
#else
#include <gperftools/tcmalloc.h>
#include <gperftools/malloc_extension.h>
#include <gperftools/heap-profiler.h>
#include <gperftools/profiler.h>
#endif

#ifdef _MSC_VER
#ifdef _DEBUG
#pragma comment(lib,"libtcmalloc_minimal.lib")
#else
#pragma comment(lib,"libtcmalloc_minimal.lib")
#endif
#endif

double Profiler::get_memory_release_rate()
{
    return MallocExtension::instance()->GetMemoryReleaseRate();
}

void Profiler::set_memory_release_rate(double rate)
{
    MallocExtension::instance()->SetMemoryReleaseRate(rate);
}

void Profiler::release_free_memory()
{
    MallocExtension::instance()->ReleaseFreeMemory();
}

void Profiler::heap_profiler_start(const char *prefix)
{
#ifndef _MSC_VER
    HeapProfilerStart(prefix);
#endif
}

void Profiler::heap_profiler_dump(const char *reason)
{
#ifndef _MSC_VER
    HeapProfilerDump(reason);
#endif
}

void Profiler::heap_profiler_stop()
{
#ifndef _MSC_VER
    HeapProfilerStop();
#endif
}

bool Profiler::heap_profiler_running()
{
#ifndef _MSC_VER
    return IsHeapProfilerRunning() != 0;
#else
    return false;
#endif
}

#ifndef _MSC_VER
namespace FLAG__namespace_do_not_use_directly_use_DECLARE_int64_instead
{
    extern int64_t FLAGS_heap_profile_time_interval;
}
#endif

void Profiler::heap_profiler_set_time_interval(int64_t interval)
{
#ifndef _MSC_VER
    FLAG__namespace_do_not_use_directly_use_DECLARE_int64_instead::FLAGS_heap_profile_time_interval = interval;
#endif
}

int64_t Profiler::heap_profiler_get_time_interval()
{
#ifndef _MSC_VER
    return FLAG__namespace_do_not_use_directly_use_DECLARE_int64_instead::FLAGS_heap_profile_time_interval;
#else
    return -1;
#endif
}



#ifndef _MSC_VER
namespace FLAG__namespace_do_not_use_directly_use_DECLARE_int64_instead
{
    extern int64_t FLAGS_heap_profile_allocation_interval;
}
#endif

void Profiler::heap_profiler_set_allocation_interval(int64_t interval)
{
#ifndef _MSC_VER
    FLAG__namespace_do_not_use_directly_use_DECLARE_int64_instead::FLAGS_heap_profile_allocation_interval = interval;
#endif
}

int64_t Profiler::heap_profiler_get_allocation_interval()
{
#ifndef _MSC_VER
    return FLAG__namespace_do_not_use_directly_use_DECLARE_int64_instead::FLAGS_heap_profile_allocation_interval;
#else
    return -1;
#endif
}

int Profiler::profiler_start(const char *prefix)
{
#ifndef _MSC_VER
    return ProfilerStart(prefix);
#else
    return -1;
#endif
}

void Profiler::profiler_stop()
{
#ifndef _MSC_VER
    ProfilerStop();
#endif
}

void Profiler::profiler_flush()
{
#ifndef _MSC_VER
    ProfilerFlush();
#endif
}

#endif
