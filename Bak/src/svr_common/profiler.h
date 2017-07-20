#ifndef __PROFILER_H__
#define __PROFILER_H__

#ifdef ENABLE_GVOE_GPROFTOOLS

#include <cstdint>

class Profiler
{
public:
    // mem
    double get_memory_release_rate();
    void set_memory_release_rate(double rate);  // Reasonable rates are in the range [0,10]
    static void release_free_memory();

    // heap
    static void heap_profiler_start(const char *prefix);
    static void heap_profiler_dump(const char *reason);
    static void heap_profiler_stop();
    static bool heap_profiler_running();
    static void heap_profiler_set_time_interval(int64_t interval); // seconds
    static int64_t heap_profiler_get_time_interval();
    static void heap_profiler_set_allocation_interval(int64_t interval); // bytes
    static int64_t heap_profiler_get_allocation_interval();

    // cpu
    static int profiler_start(const char *prefix);
    static void profiler_stop();
    static void profiler_flush();
};

#endif

#endif