#ifndef _XML_XML_NODE_CONTAINER_H_
#define _XML_XML_NODE_CONTAINER_H_

#include "./common.h"
#include "./xml_prop2value.h"

XML_NAMESPACE_BEGIN

namespace detail
{
    template<typename T, typename Container>
	class xml_node_seq_container : public Container
    {
	public:
	    typedef typename Container::value_type  value_type;
	    typedef typename Container::iterator    iterator;
	    typedef typename Container::const_iterator const_iterator;

	    bool parse(zXMLParser& xml, xmlNodePtr node)
	    {
		bool ret = true;
		if(!node) return false;
		xmlNodePtr child = xml.getChildNode(node, name.c_str());
		while(child)
		{
		    T data;
		    if(!data.parse(xml, child))
		    {
			data.init_value();
			ret = false;
		    }
		    this->insert(this->end(), data);
		    child = xml.getNextNode(child, name.c_str());
		}
		return ret;
	    }
	    
	    int dump(std::ostream& os, int deep =0) const
	    {
		for(const_iterator it = this->begin(); it != this->end(); ++it)
		{
		    os << detail::indent(deep) << "<" << name;
		    deep++;
		    int node_flag = it->dump(os, deep);
		    deep--;
		    detail::dump_end_parenthese(name, os, node_flag, deep);
		}
		return 0;
	    }

	    void init_value()
	    {
		for(iterator  it = this->begin(); it != this->end(); ++it)
		{
		    (const_cast<T&>(*it)).init_value();
		}
		this->clear();
	    }
	public:
	    std::string name;
    };

    template<typename Key, typename T, typename Container>
	class xml_node_map_container : public Container
    {
	public:
	    typedef typename Container::value_type	value_type;
	    typedef typename Container::iterator	iterator;
	    typedef typename Container::const_iterator	const_iterator;

	    bool parse(zXMLParser& xml, xmlNodePtr node)
	    {
		bool ret = true;
		if(!node)   return false;
		xmlNodePtr child = xml.getChildNode(node, name.c_str());
		while(child)
		{
		    std::string keystr;
		    if(xml.getNodePropStr(child, key.c_str(), keystr))
		    {
			Key keyvalue = detail::prop2value<Key>(keystr);
			T data;
			if(!data.parse(xml, child))
			{
			    data.init_value();
			    ret = false;
			}
			this->insert(std::make_pair(keyvalue, data));
		    }
		    else
		    {
			ret = false;
		    }
		    child = xml.getNextNode(child, name.c_str());
		}
		return ret;
	    }
	
	    int dump(std::ostream& os, int deep =0) const
	    {
               for(const_iterator it = this->begin(); it != this->end(); ++it)
                {
                   os << detail::indent(deep) << "<" << name;
                       deep++;
                   int node_flag = it->second.dump(os, deep);
                     deep--;
                   detail::dump_end_parenthese(name, os, node_flag, deep);
                }
              return 0;
           }
	    
           void init_value()
           {
                 for(iterator it = this->begin(); it != this->end(); ++it)
                {
                        (const_cast<T&>(it->second)).init_value();
                }
                this->clear();
            }
	public:
	    std::string name;
	    std::string key;
    };
}
XML_NAMESPACE_END

namespace std
{
    template<typename T, typename Container>
	inline std::ostream& operator << (std::ostream& os, const xml::detail::xml_node_seq_container<T, Container>& seq)
	{
	    seq.dump(os);
	    return os;
	}
    template <typename Key, typename T, typename Container>
	inline std::ostream& operator << (std::ostream& os, const xml::detail::xml_node_map_container<Key, T, Container>& cont)
	{
	    cont.dump(os);
	    return os;
	}
}
#endif

