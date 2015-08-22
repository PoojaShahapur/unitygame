#ifndef _XML_XML_PROP_H_
#define _XML_XML_PROP_H_

#include "./common.h"
#include "./xml_prop2value.h"

XML_NAMESPACE_BEGIN

template<typename T, typename P=std::string>
class xml_prop
{
    public:
	typedef P   name_type;
	typedef T   value_type;

	xml_prop()
	{
	    name = P();
	    value = T();
	}

	xml_prop(const T& v):value(v)
    {
	name = P();
    }
	xml_prop(const P&n, const T& v)
	    :name(n),value(v)
	{

	}

	template<typename T2, typename P2>
	    xml_prop(const xml_prop<T2, P2>& prop)
	    :name(prop.name),value(prop.value)
	    {

	    }
	const value_type& operator()() const
	{
	    return value;
	}

	value_type& operator()()
	{
	    return value;
	}

	operator T()
	{
	    return value;
	}
    public:
	name_type   name;
	value_type  value;
    public:
	bool parse(zXMLParser& xml, xmlNodePtr node)
	{
	    if(!node) return false;
	    std::string prop;
	    if(!xml.getNodePropStr(node, name.c_str(), prop))
		return false;
	    value = detail::prop2value<value_type>(prop);
	    return true;
	}

	void init_value()
	{
	    value = T();
	}

	void init_value(const T& init)
	{
	    value = init;
	}
};

XML_NAMESPACE_END

namespace std
{
    template<typename T, typename P>
	std::ostream& operator << (std::ostream& os, const xml::xml_prop<T, P>& prop)
	{
	    os << prop.name <<"=" <<"\""<<prop.value<<"\"";
	    return os;
	}
    template<typename T>
	std::ostream& operator << (std::ostream& os, const xml::xml_prop<T, std::string>& prop)
	{
	    if(prop.name.empty())
		os <<"unkown";
	    else
		os << prop.name;
	    os<<"=" <<"\""<<prop.value<<"\"";
	    return os;
	}
}
#endif

