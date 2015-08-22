#ifndef _XML_XML_NODE_MAP_H_
#define _XML_XML_NODE_MAP_H_

#include "./xml_node_container.h"

XML_NAMESPACE_BEGIN

template <typename Key, typename T> struct xml_node_map :
    public detail::xml_node_map_container<Key, T, std::map<Key, T> > {};

template <typename Key, typename T> struct xml_node_multimap :
    public detail::xml_node_map_container<Key, T, std::multimap<Key, T> > {};

XML_NAMESPACE_END
#endif

