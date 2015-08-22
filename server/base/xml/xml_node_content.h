#ifndef _XML_XML_CONTENT_H_
#define _XML_XML_CONTENT_H_

#include "./common.h"
#include "./xml_prop2value.h"

XML_NAMESPACE_BEGIN

template <typename T>
class xml_content
{
    public:
	typedef T   value_type;
	xml_content() {
	}
	xml_content(const T& v): value(v){
	}
	template <typename T2>
	    inline xml_content(const xml_content<T2>& prop):value(prop.value){

	    }
	const value_type& operator ()() const {
	    return value;
	}
	value_type& operator() (){
	    return value;
	}
	operator T(){
	    return value;
	}
    public:
	value_type value;
    public:
	bool parse(zXMLParser& xml, xmlNodePtr node)
	{
	    if(!node) return false;
	    std::string content;
	    if(!xml.getNodeContentStr(node, content))
		return false;
	    value = detail::prop2value<value_type>(content);
	    return true;
	}

	void init_value() {
	    value = T();
	}

	void init_value(const T& init) {
	    value = init;
	}
};
XML_NAMESPACE_END

namespace std
{
    template <typename T>
	std::ostream& operator << (std::ostream& os, const xml::xml_content<T>& content)
	{
	    return os<<content();
	}
}
#endif

