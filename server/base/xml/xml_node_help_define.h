#ifndef _XML_XML_NODE_HELP_DEFINE_
#define _XML_XML_NODE_HELP_DEFINE_

#define xml_parser_begin(Node) \
	template <int opcode> int operation(xml::detail::argument* arg) { \
		\
		if(!arg) return false; \
		\
		int node_flag = xml::detail::node_has_nothing; \
		\
		int parse_ret = true;	\
		\
		xml::detail::argument_dump* dump_arg = arg->get_dump_arg();	\
		\
		xml::detail::argument_parse* parse_arg = arg->get_parse_arg();

#define xml_parser_prop( nodename, prop)\
		switch (opcode) { \
		case xml::detail::opcode_init_value: \
		{ \
			prop.init_value();\
		} \
		break; \
		case xml::detail::opcode_init_name: \
		{ \
			prop.name = nodename;\
		} \
		break; \
		case xml::detail::opcode_dump:	\
		{	\
			if(dump_arg) { \
				\
				dump_arg->os<<" "<<prop; \
				node_flag |= xml::detail::node_has_props; \
				\
				}\
		}	\
		break; \
		case xml::detail::opcode_parse: \
		{ \
			if(parse_arg) { \
				if(!prop.parse(parse_arg->xml, parse_arg->node)) { \
					parse_ret = false; \
					}\
				}\
		} \
		break; \
		default: \
			return false; \
	}
	
#define xml_parser_prop_init( nodename, prop, initvalue)\
		switch (opcode) { \
		case xml::detail::opcode_init_value: \
		{ \
			prop.init_value(initvalue);\
		} \
		break; \
		case xml::detail::opcode_init_name: \
		{ \
			prop.name = nodename;\
		} \
		break; \
		case xml::detail::opcode_dump:	\
		{	\
			if(dump_arg) { \
				\
				dump_arg->os<<" "<<prop; \
				node_flag |= xml::detail::node_has_props; \
				\
				}\
		}	\
		break; \
		case xml::detail::opcode_parse: \
		{ \
			if(parse_arg) { \
				if(!prop.parse(parse_arg->xml, parse_arg->node)) { \
					parse_ret = false; \
					}\
				}\
		} \
		break; \
		default: \
			return false; \
	}
	
#define xml_parser_content( cont )\
		switch (opcode) { \
		case xml::detail::opcode_init_value: \
		{ \
			cont.init_value();\
		} \
		break; \
		case xml::detail::opcode_dump:	\
		{	\
			if(dump_arg) { \
				\
				dump_arg->os<<">"<<cont; \
				node_flag |= xml::detail::node_has_content; \
				\
				}\
		}	\
		break; \
		case xml::detail::opcode_parse: \
		{ \
			if(parse_arg) { \
				if(!cont.parse(parse_arg->xml, parse_arg->node)) { \
					parse_ret = false; \
					}\
				}\
		} \
		break; \
		default: \
			return false; \
	}
		
#define xml_parser_content_init( cont, initvalue )\
		switch (opcode) { \
		case xml::detail::opcode_init_value: \
		{ \
			cont.init_value(initvalue);\
		} \
		break; \
		case xml::detail::opcode_dump:	\
		{	\
			if(dump_arg) { \
				\
				dump_arg->os<<">"<<cont; \
				node_flag |= xml::detail::node_has_content; \
				\
				}\
		}	\
		break; \
		case xml::detail::opcode_parse: \
		{ \
			if(parse_arg) { \
				if(!cont.parse(parse_arg->xml, parse_arg->node)) { \
					parse_ret = false; \
					}\
				}\
		} \
		break; \
		default: \
			return false; \
	}


#define xml_parser_node(nodename, nodedata)\
		switch (opcode) { \
		case xml::detail::opcode_init_value: \
		{ \
			nodedata.init_value();\
		} \
		break; \
		case xml::detail::opcode_init_name: \
		{ \
			nodedata.name = nodename;\
		} \
		break; \
		case xml::detail::opcode_dump:	\
		{	\
			if(dump_arg) { \
				if(!(node_flag & xml::detail::node_has_subnodes)) {\
					\
					dump_arg->os<<">"<<std::endl; \
					node_flag |= xml::detail::node_has_subnodes; \
					\
					}\
					nodedata.dump(dump_arg->os, dump_arg->deep); \
				}\
		}	\
		break; \
		case xml::detail::opcode_parse: \
		{ \
			if(parse_arg) { \
				if(!nodedata.parse(parse_arg->xml, parse_arg->node)) { \
					parse_ret = false; \
					}\
				}\
		} \
		break; \
		default: \
			break; \
	}

#define xml_parser_map_node( nodename, keyname, nodedata)\
		switch (opcode) { \
		case xml::detail::opcode_init_value: \
		{ \
			nodedata.init_value();\
		} \
		break; \
		case xml::detail::opcode_init_name: \
		{ \
			nodedata.name = nodename;\
			nodedata.key = keyname; \
		} \
		break; \
		case xml::detail::opcode_dump:	\
		{	\
			if(dump_arg) { \
				if(!(node_flag & xml::detail::node_has_subnodes)) {\
					\
					dump_arg->os<<">"<<std::endl; \
					node_flag |= xml::detail::node_has_subnodes; \
					\
					}\
					nodedata.dump(dump_arg->os, dump_arg->deep); \
				}\
		}	\
		break; \
		case xml::detail::opcode_parse: \
		{ \
			if(parse_arg) { \
				if(!nodedata.parse(parse_arg->xml, parse_arg->node)) { \
					parse_ret = false; \
					}\
				}\
		} \
		break; \
		default: \
			break; \
	}	
	
#define xml_parser_end(Node)\
		switch (opcode) { \
		case xml::detail::opcode_init_value: \
		{ \
			return true; \
		} \
		break; \
		case xml::detail::opcode_init_name: \
		{ \
			return true;\
		} \
		break; \
		case xml::detail::opcode_dump:	\
		{	\
			if(dump_arg)  \
				return node_flag;\
			else \
			 return xml::detail::node_has_nothing; \
		}	\
		break; \
		case xml::detail::opcode_parse: \
		{ \
			if(!parse_arg) return false; \
			return parse_ret; \
		} \
		break; \
		default: \
			return false; \
	}	\
	return false; \
	}\
\
Node() \
{ \
	init_value(); \
	init_name(); \
} \
\
void init_value() \
{ \
	xml::detail::argument_init_value arg; \
	\
	operation<xml::detail::opcode_init_value>(&arg); \
} \
\
void init_name() \
{ \
	xml::detail::argument_init_name arg; \
	\
	operation<xml::detail::opcode_init_name>(&arg); \
} \
\
int dump(std::ostream& os, int deep = 0) const \
{ \
	xml::detail::argument_dump arg(os, deep); \
	\
	return (const_cast<Node*>(this))->operation<xml::detail::opcode_dump>(&arg); \
} \
\
bool parse(zXMLParser& xml, xmlNodePtr node) \
{ \
	xml::detail::argument_parse arg(xml, node); \
	\
	return operation<xml::detail::opcode_parse>(&arg); \
}

#endif

