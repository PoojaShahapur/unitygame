#ifndef _XML_XML_NODE_SEQ_H_
#define _XML_XML_NODE_SEQ_H_

#include "./xml_node_container.h"

XML_NAMESPACE_BEGIN

template <typename T> struct xml_node_vector:
public detail::xml_node_seq_container<T, std::vector<T> > {};

template <typename T> struct xml_node_list:
public detail::xml_node_seq_container<T, std::list<T> > {};

template <typename T> struct xml_node_deque:
public detail::xml_node_seq_container<T, std::deque<T> > {};

XML_NAMESPACE_END
#endif

