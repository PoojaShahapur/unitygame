#ifndef _XML_XML_NODE_H_
#define _XML_XML_NODE_H_

#include "./common.h"
#include "./xml_utility.h"

XML_NAMESPACE_BEGIN

class xml_node_base
{
	public:
		virtual ~xml_node_base() {}
		virtual bool parse(zXMLParser& xml, xmlNodePtr node) {return false; }
		virtual int dump(std::ostream& os, int deep = 0) const {return false;}
};

template<typename T>
class xml_node : public T
{
	public:
		xml_node(const std::string nodename=""):name(nodename) {}
		
		bool parse(zXMLParser& xml, xmlNodePtr node, bool root =false)
		{
			if(!node) return false;
			if(root) return T::parse(xml, node);
			xmlNodePtr child = xml.getChildNode(node, name.c_str());
			if(!child) return false;
			return T::parse(xml,child);
		}
		
		int dump(std::ostream& os, int deep = 0) const
		{
			os << detail::indent(deep) <<"<" <<name;
			deep++;
			int node_flag = T::dump(os, deep);
			deep--;
			detail::dump_end_parenthese(name, os, node_flag, deep);
			return 0;
		}
		
		void init_value() {T::init_value(); }
		public:
			std::string name;
};

XML_NAMESPACE_END

namespace std
{
	template<typename T>
	inline std::ostream& operator << (std::ostream& os, const xml::xml_node<T>& node)
	{
		node.dump(os);
		return os;
	}
}
#endif

