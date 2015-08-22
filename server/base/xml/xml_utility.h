#ifndef _XML_XML_UTILITY_H_
#define _XML_XML_UTILITY_H_

#include "./common.h"

XML_NAMESPACE_BEGIN

namespace detail
{
    struct indent
    {
	indent(int _deep, int _space = 4, char c=' ')
	    :deep(_deep), space(_space), character(c)
	{
	}
	int deep;
	int space;
	char character;
    };
}

XML_NAMESPACE_END

namespace std
{
    inline std::ostream& operator << (std::ostream& os, const xml::detail::indent& indent)
    {
	return os<<std::string(indent.deep*indent.space, indent.character);
    }
};

XML_NAMESPACE_BEGIN

namespace detail
{
    const int opcode_init_value	=0;
    const int opcode_init_name	=1;
    const int opcode_dump	=2;
    const int opcode_parse	=3;

    struct argument;
    struct argument_init_value;
    struct argument_init_name;
    struct argument_dump;
    struct argument_parse;

    struct argument
    {
	virtual argument_init_value* get_init_value_arg() 
	{
	    return NULL;
	}
	virtual argument_init_name* get_init_name_arg()
	{
	    return NULL;
	}
	virtual argument_dump* get_dump_arg()
	{
	    return NULL;
	}
	virtual argument_parse* get_parse_arg()
	{
	    return NULL;
	}
#if 0
	virtual ~argument()
	{
	}
#endif
    };

    struct argument_init_value : public argument
    {
	virtual argument_init_value* get_init_value_arg()
	{
	    return this;
	}
    };
    struct argument_init_name : public argument
    {
        virtual argument_init_name* get_init_name_arg()
        {
            return this;
        }
    };
    struct argument_dump : public argument
    {
	argument_dump(std::ostream& _os, int _deep = 0)
	    :os(_os), deep(_deep)
	{

	}
	virtual argument_dump* get_dump_arg()
	{
	    return this;
	}
	std::ostream& os;
	int deep;
    };
    struct argument_parse : public argument
    {
	argument_parse(zXMLParser& _xml, xmlNodePtr _node)
	    :xml(_xml), node(_node)
	{
	}
	virtual argument_parse* get_parse_arg()
	{
	    return this;
	}
	zXMLParser& xml;
	xmlNodePtr node;
    };
    
    const int node_has_nothing	=0x00;
    const int node_has_props	=0x01;
    const int node_has_subnodes =0x02;
    const int node_has_content  =0x04;

    inline void dump_end_parenthese(const std::string& name, std::ostream& os, int node_flag, int deep)
    {
	if(node_flag & detail::node_has_content)
	{
	    os <<"</"<<name<<">"<<std::endl;
	}
	else if(node_flag & detail::node_has_subnodes)
	{
	    os <<detail::indent(deep) << "</" <<name<<">" <<std::endl;
	}
	else
	{
	    os <<"/>"<<std::endl;
	}
    }
}
XML_NAMESPACE_END
#endif


