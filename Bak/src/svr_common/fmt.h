// fmt.h
// Boost format helper.
// See: http://blog.csdn.net/jq0123/article/details/8308381
// Author: Jin Qing (http://blog.csdn.net/jq0123)

#ifndef SVR_COMMON_FMT_H__
#define SVR_COMMON_FMT_H__

#pragma warning( push )
#pragma warning( disable: 4819 )
// Disable boost/format/alt_sstream_impl.hpp : warning C4819: ...
#include <boost/format.hpp>
#pragma warning( pop )

namespace {

boost::format Fmt(const std::string & sFmt)
{
    boost::format fmter(sFmt);
#ifdef NDEBUG
    fmter.exceptions(boost::io::no_error_bits);
#endif    
    return fmter;
}

}  // namespace

#endif  // SVR_COMMON_FMT_H__
