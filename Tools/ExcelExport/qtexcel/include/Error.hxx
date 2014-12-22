#ifndef _ERROR_H
#define _ERROR_H

#include <assert.h>

#ifdef _DEBUG
#  define TOOL_ASSERT assert;
#else
#  define TOOL_ASSERT assert
#endif

#endif