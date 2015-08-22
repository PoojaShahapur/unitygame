#ifndef _XML_PARSER_GENERATOR_H_
#define _XML_PARSER_GENERATOR_H_

#include "./common.h"
#include "./xml.h"
#include <iostream>

XML_NAMESPACE_BEGIN

class xml_parser_generator_base;
class xml_prop_parser_generator;
class xml_content_parser_generator;
class xml_seq_node_parser_generator;
class xml_ass_node_parser_generator;

class xml_parser_generator_base
{
    public:
	virtual ~xml_parser_generator_base() { }
	virtual bool parse(zXMLParser& xml, xmlNodePtr node) = 0;
	virtual void generate(std::ostream& os, int deep = 0) = 0;
	virtual void dump(std::ostream& os, int deep = 0) { }
};

class xml_prop_parser_generator
{
    public:
	xml_prop_parser_generator(const std::string& propname)
	{
	    prop.name = propname;
	    value_is_str = false;
	}
	virtual ~xml_prop_parser_generator()
	{
	}
	virtual bool parse(zXMLParser& xml, xmlNodePtr node)
	{
	    if(!node) return false;
	    bool ret = prop.parse(xml, node);
	    if(!ret) return false;
	    return parse_prop_value();
	}
	virtual void generate(std::ostream& os, int deep = 0)
	{
	    os << detail::indent(deep) <<"xml_prop<"<<type<<">\t"<<member<<";";
	}
	virtual void generate_parser(std::ostream& os, int deep = 0)
	{
	    if(value.empty())
		os <<detail::indent(deep)<<"xml_parser_prop(\"" << prop.name<<"\",\t"<<member<<");";
	    else
	    {
		os <<detail::indent(deep)<<"xml_parser_prop_init(\""<<prop.name <<"\",\t"<<member<<",\t";
		if(value_is_str)
		{
		    os<< "\""<<value <<"\""<<");";
		    if(type != "string" && type != "std::string")
			std::cerr <<"node prop"<<prop<<"type is not string,but init value is string"<<std::endl;
		}
		else
		{
		    os<<value<<");";
		    if(type == "string" || type == "std::string")
			std::cerr<<"node prop"<<prop<<"type is string,but init value is not string"<<std::endl;
		}
	    }
	}
	virtual void dump(std::ostream& os, int deep = 0)
	{
	    if(!value.empty())
	    {
		prop.value = type +" "+member +" = ";
		if(value_is_str)
		    prop.value += "\'"+value+"\'";
		else
		    prop.value += value;
	    }
	    else if(prop.name != member)
		prop.value = type +" "+member;
	    else
		prop.value = type;

	    os <<prop;
	}
	bool parse_prop_value();
    public:
	xml_prop<std::string> prop;
	std::string type;
	std::string member;
	std::string value;
	bool value_is_str;
};

class xml_node_parser_generator
{
    public:
	typedef std::vector<xml_prop_parser_generator*> prop_parser_generator_cont;
	typedef std::vector<xml_node_parser_generator*> node_parser_generator_cont;
	static xml_node_parser_generator* make(zXMLParser& xml, xmlNodePtr node);
    public:

	virtual ~xml_node_parser_generator() 
	{
	    for(prop_parser_generator_cont::iterator it = _pp_generators.begin();
		    it != _pp_generators.end(); ++it)
		delete *it;
	    for(node_parser_generator_cont::iterator it = _np_generators.begin();
		    it != _np_generators.end(); ++it)
		delete *it;
	}

	virtual bool parse(zXMLParser& xml, xmlNodePtr node)
	{
	    if(!node) return false;

	    parse_the_node(xml, node);
	    xmlAttrPtr prop = xml.getNodeAttr(node, NULL);
	    while(prop)
	    {
		if(!is_keyword((char*)prop->name))
		{
		    xml_prop_parser_generator* generator = new xml_prop_parser_generator((char*)prop->name);
		    if(!generator) return false;
		    if(!generator->parse(xml, node))
			return false;
		    _pp_generators.push_back(generator);
		}
		prop = xml.getNodeNextAttr(node, prop);
	    }

	    xmlNodePtr child = xml.getChildNode(node, NULL);
	    while(child)
	    {
		if("comment" != std::string((char*)child->name))
		{
		    xml_node_parser_generator* generator = xml_node_parser_generator::make(xml, child);
		    if(generator)
		    {
			generator->parse(xml, child);
			_np_generators.push_back(generator);
		    }
		}
		child = xml.getNextNode(child, NULL);
	    }
	    return true;
	}
	virtual void generate(std::ostream& os, int deep = 0, int layout = 0)
	{
	    os<<detail::indent(deep) <<"struct "<<structname<<std::endl;
	    os<<detail::indent(deep) <<"{"<<std::endl;
	    deep++;
	    generate_subnodes_define(os, deep, layout);
	    generate_properties(os, deep);
	    generate_subnodes_dec(os, deep);
	    if(!layout)
		generate_parser(os, deep);
	    else
		generate_parser_2(os, deep);
	    deep--;
	    os<<detail::indent(deep)<<"};"<<std::endl;
	    generate_node_typedef(os, deep);
	}

	virtual void generate_subnodes_define(std::ostream& os, int deep = 0, int layout =0)
	{
	    for(node_parser_generator_cont::iterator it = _np_generators.begin();
		    it != _np_generators.end(); ++it)
	    {
		(*it)->generate(os, deep, layout);
		os<<std::endl;
	    }
	}
	void generate_properties(std::ostream& os, int deep = 0)
	{
	    for(prop_parser_generator_cont::iterator it = _pp_generators.begin();
		    it != _pp_generators.end(); ++it)
	    {
		(*it)->generate(os, deep);
		os<<std::endl;
	    }
	    if(!_pp_generators.empty())
		os <<std::endl;
	}
	void generate_subnodes_dec(std::ostream& os, int deep = 0)
	{
	    for(node_parser_generator_cont::iterator it = _np_generators.begin();
		    it != _np_generators.end(); ++it)
	    {
		(*it)->generate_node_dec(os, deep);
		os<<std::endl;
	    }
	    if(!_np_generators.empty())
		os<<std::endl;
	}
	void generate_parser(std::ostream& os, int deep = 0)
	{
	    os<<detail::indent(deep)<<"xml_parser_begin("<<structname<<")"<<std::endl;
	    generate_prop_parser(os, deep);
	    generate_subnodes_parser(os, deep);
	    os<<detail::indent(deep)<<"xml_parser_end("<<structname<<")"<<std::endl;
	}
	virtual void generate_prop_parser(std::ostream& os, int deep = 0)
	{
	    for(prop_parser_generator_cont::iterator it = _pp_generators.begin();
		    it != _pp_generators.end(); ++it)
	    {
		(*it)->generate_parser(os, deep);
		os<<std::endl;
	    }
	}
	virtual void generate_subnodes_parser(std::ostream& os, int deep = 0)
	{
	    for(node_parser_generator_cont::iterator it = _np_generators.begin();
		    it != _np_generators.end(); ++it)
	    {
		(*it)->generate_node_parser(os, deep);
		os<<std::endl;
	    }
	}
	virtual void generate_node_dec(std::ostream& os, int deep = 0)
	{
	    os<<detail::indent(deep)<<"xml_node<"<<structname <<">\t"<<membername<<";";
	}
	virtual void generate_node_parser(std::ostream& os, int deep = 0)
	{
	    os<<detail::indent(deep)<<"xml_parser_node(\""<<nodename <<"\",\t"<<membername<<");";
	}
	virtual void generate_node_typedef(std::ostream& os, int deep = 0) {	}
	void generate_parser_2(std::ostream& os, int deep = 0)
	{
	    os<<detail::indent(deep)<<structname<<"("<<")"
		<<"{ init_value(); init_name(); }"<<std::endl<<std::endl;

	    os<<detail::indent(deep)<<"void init_name( )"<<std::endl;
	    os<<detail::indent(deep)<<"{"<<std::endl;
	    deep++;
	    generate_parser_2_init_name_properties(os, deep);
	    generate_parser_2_init_name_subnodes(os, deep);
	    deep--;

	    os<<detail::indent(deep)<<"}"<<std::endl<<std::endl;
	    os<<detail::indent(deep)<<"void init_value( )"<<std::endl;
	    os<<detail::indent(deep)<<"{"<<std::endl;

	    deep++;
	    generate_parser_2_init_value_properties(os, deep);
	    deep--;

	    os<<detail::indent(deep) <<"}"<<std::endl<<std::endl;

	    os<<detail::indent(deep)<<"bool parse(zXMLParser& xml, xmlNodePtr node)"<<std::endl;
	    os<<detail::indent(deep)<<"{"<<std::endl;
	    deep++;
	    generate_parser_2_parser_properties(os, deep);
	    generate_parser_2_parser_subnodes(os, deep);
	    os<<detail::indent(deep)<<"return true;"<<std::endl;
	    deep--;
	    os<<detail::indent(deep)<<"}"<<std::endl<<std::endl;

	    os<<detail::indent(deep)<<"int dump(std::ostream& os, int deep=0) const"<<std::endl;
	    os<<detail::indent(deep)<<"{"<<std::endl;
	    deep++;
	    os<<detail::indent(deep)<<"int dump_flag = xml::detail::node_has_nothing;"<<std::endl<<std::endl;
	    generate_parser_2_dump_properties(os, deep);
	    if(!_pp_generators.empty())
	    {
		os<<detail::indent(deep)<<"dump_flag |= xml::deep::node_has_props;"<<std::endl;
	    }
	    if(!_np_generators.empty())
	    {
		os<<detail::indent(deep)<<"os << \">\" <<std::endl;"<<std::endl;
	    }
	    os<<std::endl;
	    generate_parser_2_dump_subnodes(os, deep);
	    if(!_np_generators.empty())
	    {
		os<<detail::indent(deep)<<"dump_flag |= xml::deep::node_has_subnodes;"<<std::endl;
	    }
	    os<<std::endl;
	    os<<detail::indent(deep)<<"return dump_flag;"<<std::endl;
	    deep--;
	    os<<detail::indent(deep)<<"}"<<std::endl<<std::endl;
	}
	virtual void generate_parser_2_init_name_properties(std::ostream& os, int deep = 0)
	{
	    for(prop_parser_generator_cont::iterator it = _pp_generators.begin();
		    it != _pp_generators.end(); ++it)
	    {
		os<<detail::indent(deep) <<(*it)->member<<".name = \""<<(*it)->prop.name<<"\";"<<std::endl;
	    }
	}
	virtual void generate_parser_2_init_name_subnodes(std::ostream& os, int deep = 0)
	{
	    for(node_parser_generator_cont::iterator it = _np_generators.begin();
		    it != _np_generators.end(); ++it)
	    {
		(*it)->generate_parser_2_init_name(os, deep);
	    }

	}
	virtual void generate_parser_2_init_name(std::ostream& os, int deep = 0)
	{
	    os << detail::indent(deep)<<membername<<".name=\""<<nodename<<"\";"<<std::endl;
	}
	virtual void generate_parser_2_init_value_properties(std::ostream& os, int deep     = 0)
	{
	    for(prop_parser_generator_cont::iterator it = _pp_generators.begin();
		    it != _pp_generators.end(); ++it)
	    {
		if((*it)->value_is_str)
		    os<<detail::indent(deep)<<(*it)->member<<".value = \""<<(*it)->value<<"\";" <<std::endl;
		else
		    os <<detail::indent(deep)<<(*it)->member<<".value = "<<(*it)->value<<";"<<std::endl;

	    }

	}
	virtual void generate_parser_2_parser_properties(std::ostream& os, int deep     = 0)
	{
	    for(prop_parser_generator_cont::iterator it = _pp_generators.begin();
		    it != _pp_generators.end(); ++it)
	    {
		os<<detail::indent(deep)<<(*it)->member<<".parse(xml, node);" <<std::endl;
	    }

	}
	virtual void generate_parser_2_parser_subnodes(std::ostream& os, int deep         = 0)
	{
	    for(node_parser_generator_cont::iterator it = _np_generators.begin();
		    it != _np_generators.end(); ++it)
	    {
		os<<detail::indent(deep) <<(*it)->membername<<".parse(xml, node);"<<std::endl;
	    }
	}
	virtual void generate_parser_2_dump_properties(std::ostream& os, int deep         = 0)
	{
	    for(prop_parser_generator_cont::iterator it = _pp_generators.begin();
		    it != _pp_generators.end(); ++it)
	    {
		os<<detail::indent(deep)<<"os<<\" \" << "<<(*it)->member<<";" <<std::endl;
	    }

	}
	virtual void generate_parser_2_dump_subnodes(std::ostream& os, int deep             = 0)
	{
	    for(node_parser_generator_cont::iterator it = _np_generators.begin();
		    it != _np_generators.end(); ++it)
	    {
		os<<detail::indent(deep)<<(*it)->membername<<".dump(os, deep);"<<std::endl;
	    }

	}
	virtual void dump(std::ostream& os, int deep = 0)
	{
	    os<<detail::indent(deep)<<"<"<<nodename;
	    deep++;
	    dump_properties(os, deep);
	    if(_np_generators.size())
	    {
		os<<">"<<std::endl;
		dump_subnodes(os, deep);
		deep--;
		os<<detail::indent(deep)<<"</"<<nodename<<">"<<std::endl;
	    }
	    else
	    {
		os<<"/>"<<std::endl;
	    }
	}

	virtual void dump_properties(std::ostream& os, int deep = 0)
	{
	    for(prop_parser_generator_cont::iterator it = _pp_generators.begin();
		    it != _pp_generators.end(); ++it)
	    {
		os<<" ";
		(*it)->dump(os, deep);
	    }
	    if(!nodevalue.empty())
	    {
		os<<"node_=\""<<nodevalue<<"\"";
	    }
	}

	virtual void dump_subnodes(std::ostream& os, int deep = 0)
	{
	    for(node_parser_generator_cont::iterator it = _np_generators.begin();
		    it != _np_generators.end(); ++it)
	    {
		(*it)->dump(os, deep);
	    }
	}

	bool is_keyword(const std::string& prop)
	{
	    if("content_" == prop) return true;
	    if("node_" == prop) return true;
	    if("container_" == prop) return true;
	    if("key_" == prop) return true;
	    return false;
	}

	void parse_the_node(zXMLParser& xml, xmlNodePtr node)
	{
	    if(!node) return;
	    nodename = (char*)node->name;
	    if(xml.getNodePropStr(node, "node_", nodevalue))
	    {
		parse_node_value();
	    }
	    if(structname.empty())
	    {
		structname = nodename;
		if(structname.size() > 0)
		    structname[0] = std::toupper(structname[0]);
	    }
	    if(membername.empty())
	    {
		membername = nodename;
		if(membername.size() > 0)
		    membername[0] = std::tolower(membername[0]);
	    }
	}

	void parse_node_value();
    private:
	prop_parser_generator_cont _pp_generators;
	node_parser_generator_cont _np_generators;
    public:
	std::string nodename;
	std::string nodevalue;
	std::string structname;
	std::string membername;
};

class xml_node_seq_parser_generator : public xml_node_parser_generator
{
    public:
	xml_node_seq_parser_generator(const std::string conttype)
	    :containertype(conttype)
	{
	}
	virtual void generate_node_typedef(std::ostream& os, int deep = 0)
	{
	    os<<detail::indent(deep)<<"typedef "<<
		getContType()<<"\t"<<getContTypeDef()<<";"<<std::endl;
	    os<<detail::indent(deep)<<"typedef "<<
		getContTypeDef()<<"::iterator\t"<<getContTypeDef()<<"Iter;"<<std::endl;
	    os<<detail::indent(deep)<<"typedef "<<
		getContTypeDef()<<"::const_iterator\t"<<getContTypeDef()<<"ConstIter;"<<std::endl;
	}
	virtual void generate_node_dec(std::ostream& os, int deep = 0)
	{
	    os<<detail::indent(deep)<<getContType()<<"\t"<<membername<<";";
	}
	virtual void dump_properties(std::ostream& os, int deep = 0)
	{
	    xml_node_parser_generator::dump_properties(os, deep);
	    os<<" container_=\""<<containertype<<"\"";
	}
	const std::string getContType() const
	{
	    return "xml_node_"+containertype +"<"+structname+">";
	}
	const std::string getContTypeDef() const
	{
	    return structname + "Cont";
	}
    public:
	std::string containertype;
};

class xml_node_set_parser_generator : public xml_node_parser_generator
{
    public:
	xml_node_set_parser_generator(const std::string settype, xml_prop_parser_generator* keyppg = NULL)
	    :containertype(settype), keyprop(keyppg)
	{
	}
	~xml_node_set_parser_generator()
	{
	    delete keyprop;
	}
	virtual bool parse(zXMLParser& xml, xmlNodePtr node)
	{
	    if(keyprop)
	    {
		if(!keyprop->parse(xml, node))
		    return false;
	    }
	    return xml_node_parser_generator::parse(xml, node);
	}
	virtual void generate_node_typedef(std::ostream& os, int deep =0)
	{
	    os<<detail::indent(deep)<<"typedef "<<
		getContType()<<"\t"<<getContTypeDef()<<";"<<std::endl;
	    os<<detail::indent(deep)<<"typedef "<<
		getContTypeDef()<<"::iterator\t"<<getContTypeDef()<<"Iter;"<<std::endl;
	    os<<detail::indent(deep)<<"typedef "<<
		getContTypeDef()<<"::const_iterator\t"<<getContTypeDef()<<"ConstIter;"<<std::endl;

	}
	virtual void generate_node_dec(std::ostream& os, int deep =0)
	{
	    os<<detail::indent(deep)<<getContType()<<"\t"<<membername<<";";

	}
	virtual void generate_subnodes_define(std::ostream& os, int deep =0, int layout=0)
	{
	    if(keyprop)
	    {
		os <<detail::indent(deep)<<"bool operator < (const "<<structname<<"& data) const"<<std::endl;
		os <<detail::indent(deep)<<"{"<<std::endl;
		deep++;

		os <<detail::indent(deep)<<"return this->"<<keyprop->member<<"() <"
		    <<"data."<<keyprop->member<<"();"<<std::endl;
		deep--;
		os <<detail::indent(deep)<<"}"<<std::endl<<std::endl;
	    }
	    xml_node_parser_generator::generate_subnodes_define(os, deep, layout);
	}
	virtual void dump_properties(std::ostream& os, int deep =0)
	{
	    xml_node_parser_generator::dump_properties(os, deep);
	    os<<" container_=\""<<containertype<<"\"";
	    if(keyprop)
	    {
		os<<" key_=\""<<keyprop->prop.name<<"\"";
	    }
	}
	const std::string getContType() const
	{
	    return "xml_node_"+containertype+"<"+structname+">";
	}
	const std::string getContTypeDef() const
	{
	    return structname + "Set";
	}
    public:
	std::string containertype;
	xml_prop_parser_generator* keyprop;
};

class xml_node_map_parser_generator : public xml_node_parser_generator
{
    public:
	xml_node_map_parser_generator(const std::string& maptype, xml_prop_parser_generator* keyppg)
	    :containertype(maptype),keyprop(keyppg)
	{
	}
	virtual ~xml_node_map_parser_generator()
	{
	    delete keyprop;
	}
	virtual bool parse(zXMLParser& xml, xmlNodePtr node)
	{
	    if(keyprop)
	    {
		if(!keyprop->parse(xml, node))
		    return false;
	    }
	    return xml_node_parser_generator::parse(xml, node);

	}
	virtual void generate_node_typedef(std::ostream& os, int deep =0)
	{
	    os<<detail::indent(deep)<<"typedef "<<
		getContType()<<"\t"<<getContTypeDef()<<";"<<std::endl;
	    os<<detail::indent(deep)<<"typedef "<<
		getContTypeDef()<<"::iterator\t"<<getContTypeDef()<<"Iter;"<<std::endl;
	    os<<detail::indent(deep)<<"typedef "<<
		getContTypeDef()<<"::const_iterator\t"<<getContTypeDef()<<"ConstIter;"<<std::endl;

	}
	virtual void generate_node_dec(std::ostream& os, int deep =0)
	{
	    os<<detail::indent(deep)<<getContType()<<"\t"<<membername<<";";

	}
	virtual void generate_node_parser(std::ostream& os, int deep =0)
	{
	    os << detail::indent(deep)<<"xml_parser_map_node(\""<<nodename<<"\", \""
		<<(keyprop?keyprop->prop.name:"keyprop")<<"\", "<<membername<<");";
	}
	virtual void generate_parser_2_init_name(std::ostream& os, int deep =0)
	{
	    xml_node_parser_generator::generate_parser_2_init_name(os, deep);
	    if(keyprop)
	    {
		os<<detail::indent(deep)<<membername<<"."<<keyprop->member<<" = \""
		    <<keyprop->prop.name<<"\";"<<std::endl;
	    }
	}
	virtual void dump_properties(std::ostream& os, int deep =0)
	{
	    xml_node_parser_generator::dump_properties(os, deep);
	    os<<" container_=\""<<containertype<<"\"";
	    if(keyprop)
	    {
		os<<" key_=\""<<keyprop->prop.name<<"\"";
	    }

	}
	const std::string getContType() const
	{
	    if(keyprop)
		return "xml_node_"+containertype+"<"+keyprop->type+","+structname+">";
	    else
		return "xml_node_"+containertype+"<keytype, "+structname+">";
	}
	const std::string getContTypeDef() const
	{
	    return structname + "Map";
	}
    public:
	std::string containertype;
	xml_prop_parser_generator* keyprop;
};

inline xml_node_parser_generator* xml_node_parser_generator::make(zXMLParser& xml, xmlNodePtr node)
{
    std::string container;
    std::string key;

    xml.getNodePropStr(node, "container_", container);
    xml.getNodePropStr(node, "key_", key);

    if(!container.empty())
    {
	if("vector" == container || "list" == container || "deque" == container)
	{
	    return new xml_node_seq_parser_generator(container);
	}
	else if("set" == container || "multiset" == container)
	{
	    if(key.empty())
	    {
		return new xml_node_set_parser_generator(container);
	    }
	    else
	    {
		return new xml_node_set_parser_generator(container, new xml_prop_parser_generator(key));
	    }
	}
	else if("map" == container || "multimap" == container)
	{
	    if(key.empty())
	    {
		return NULL;
	    }
	    else
		return new xml_node_map_parser_generator(container, new xml_prop_parser_generator(key));
	}
	else
	{
	    return NULL;
	}
    }
    return new xml_node_parser_generator();
}

inline bool xml_prop_parser_generator::parse_prop_value()
{
    int state = 0;
    size_t token_begin = 0;

    for(size_t i=0; i<prop.value.size(); ++i)
    {
	char ch = prop.value[i];

	switch(state)
	{
	    case 0:
		{
		    if(!std::isspace(ch))
		    {
			state = 1;
			token_begin = i;
		    }
		}
		break;
	    case 1:
		{
		    if(std::isspace(ch))
		    {
			state = 2;
			type.assign(prop.value, token_begin, i-token_begin);
		    }
		}
		break;
	    case 2:
		{
		    if(!std::isspace(ch))
		    {
			state = 3;
			token_begin = i;
		    }
		}
		break;
	    case 3:
		{
		    if(std::isspace(ch))
		    {
			state = 4;
			member.assign(prop.value, token_begin, i-token_begin);
		    }
		    else if('=' == ch)
		    {
			state = 5;
			member.assign(prop.value, token_begin, i-token_begin);
		    }
		}
		break;
	    case 4:
		{
		    if('=' == ch)
		    {
			state = 5;
		    }
		}
		break;
	    case 5:
		{
		    if(!std::isspace(ch))
		    {
			if('\'' == ch)
			{
			    value_is_str = true;
			    state = 7;
			}
			else
			{
			    state = 6;
			    token_begin = i;
			}
		    }
		}
		break;
	    case 6:
		{
		    if(std::isspace(ch))
		    {
			value.assign(prop.value, token_begin, i-token_begin);
		    }
		}
		break;
	    case 7:
		{
		    token_begin = i;
		    state = 8;
		}
		break;
	    case 8:
		{
		    if('\'' == ch)
		    {
			value.assign(prop.value, token_begin, i-token_begin);
			state = 9;
		    }
		}
		break;
	    default:
		break;
	}
    }

    switch(state)
    {
	case 0:
	    return false;
	    break;
	case 1:
	    type.assign(prop.value, token_begin, std::string::npos);
	    break;
	case 2:
	    break;
	case 3:
	    member.assign(prop.value, token_begin, std::string::npos);
	    break;
	case 4:
	    break;
	case 5:
	    return false;
	    break;
	case 6:
	    value.assign(prop.value, token_begin, std::string::npos);
	    break;
	case 7:
	case 8:
	    return false;
	    break;
	default:
	    break;
    }

    if(member.empty())
	member = prop.name;

    if(member.size() > 0)
    {
	if(member[0] != '_' && !std::isalpha(member[0]))
	{
	    return false;
	}
    }
    if("string" == type)
	type = "std::string";
    return true;
}

void xml_node_parser_generator::parse_node_value()
{
    int state = 0;
    size_t token_begin = 0;
    for(size_t i =0; i<nodevalue.size(); ++i)
    {
	char ch = nodevalue[i];
	switch(state)
	{
	    case 0:
		{
		    if(!std::isspace(ch))
		    {
			state = 1;
			token_begin = i;
		    }
		}
		break;
	    case 1:
		{
		    if(std::isspace(ch))
		    {
			state = 2;
			structname.assign(nodevalue, token_begin, i-token_begin);
		    }
		}
		break;
	    case 2:
		{
		    if(!std::isspace(ch))
		    {
			state = 3;
			token_begin = i;
		    }
		}
		break;
	    case 3:
		{
		    if(std::isspace(ch))
		    {
			state = 1;
			membername.assign(nodevalue, token_begin, i-token_begin);
		    }
		}
		break;
	}
    }
    switch(state)
    {
	case 1:
	    structname.assign(nodevalue, token_begin, std::string::npos);
	    break;
	case 3:
	    membername.assign(nodevalue, token_begin, std::string::npos);
	    break;
    }
}

XML_NAMESPACE_END

#endif

