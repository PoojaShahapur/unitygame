#ifndef __SVR_COMMON_SINGLETON_HEAD__
#define __SVR_COMMON_SINGLETON_HEAD__

#include <boost/serialization/singleton.hpp>

template <class T>
using Singleton = boost::serialization::singleton<T>;

#endif  // __SVR_COMMON_SINGLETON_HEAD__
