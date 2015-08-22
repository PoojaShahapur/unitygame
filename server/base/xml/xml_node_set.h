#ifndef _XML_XML_NODE_SET_H_
#define _XML_XML_NODE_SET_H_

#include "./xml_node_container.h"

XML_NAMESPACE_BEGIN

template <typename T> struct xml_node_set:
public detail::xml_node_seq_container<T, std::set<T> > {};

template <typename T> struct xml_node_multiset:
public detail::xml_node_seq_container<T, std::multiset<T> > {};
XML_NAMESPACE_END
#endif

