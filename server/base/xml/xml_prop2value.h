#ifndef _XML_PROP_2_VALUE_H_
#define _XML_PROP_2_VALUE_H_

#include "./common.h"
#include "zMisc.h"

XML_NAMESPACE_BEGIN

namespace detail
{
	template<typename T>
	struct xml_prop2value
	{
		T operator() (const std::string& prop)
		{
			return zMisc::lexical_cast<T>(prop);
		}
	};

	template<typename T>
	inline T prop2value(const std::string& prop)
	{
		return xml_prop2value<T>()(prop);
	}
}
XML_NAMESPACE_END
#endif

