#ifndef _XML_BASE_XMLPARSER_TEST_H
#define _XML_BASE_XMLPARSER_TEST_H
#include "zType.h"
///////////////////////////////////////////////
//
//config[Config/test.xml] defination by codemokey
//
// notice:this file was created by a tool,please do not modify
//
///////////////////////////////////////////////

struct TestConfig
{
    struct Reward
    {
        struct Item
        {
            xml_prop<int>	id;
            xml_prop<int>	num;

            xml_parser_begin(Item)
            xml_parser_prop("id",	id);
            xml_parser_prop("num",	num);
            xml_parser_end(Item)
        };
        typedef xml_node_map<int,Item>	ItemMap;
        typedef ItemMap::iterator	ItemMapIter;
        typedef ItemMap::const_iterator	ItemMapConstIter;

        xml_node_map<int,Item>	item;

        xml_parser_begin(Reward)
        xml_parser_map_node("item", "id", item);
        xml_parser_end(Reward)
    };

    xml_node<Reward>	reward;

    xml_parser_begin(TestConfig)
    xml_parser_node("reward",	reward);
    xml_parser_end(TestConfig)
};

#endif //_XML_BASE_XMLPARSER_TEST_H

