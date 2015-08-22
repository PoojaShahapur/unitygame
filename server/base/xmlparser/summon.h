#ifndef _XML_BASE_XMLPARSER_SUMMON_H
#define _XML_BASE_XMLPARSER_SUMMON_H
#include "zType.h"
///////////////////////////////////////////////
//
//config[Config/summonconfig.xml] defination by codemokey
//
// notice:this file was created by a tool,please do not modify
//
///////////////////////////////////////////////

struct SummonConfig
{
    struct Card
    {
        struct Item
        {
            xml_prop<DWORD>	id;
            xml_prop<DWORD>	odds;

            xml_parser_begin(Item)
            xml_parser_prop("id",	id);
            xml_parser_prop("odds",	odds);
            xml_parser_end(Item)
        };
        typedef xml_node_map<DWORD,Item>	ItemMap;
        typedef ItemMap::iterator	ItemMapIter;
        typedef ItemMap::const_iterator	ItemMapConstIter;

        xml_prop<DWORD>	index;

        xml_node_map<DWORD,Item>	item;

        xml_parser_begin(Card)
        xml_parser_prop("index",	index);
        xml_parser_map_node("item", "id", item);
        xml_parser_end(Card)
    };
    typedef xml_node_map<DWORD,Card>	CardMap;
    typedef CardMap::iterator	CardMapIter;
    typedef CardMap::const_iterator	CardMapConstIter;

    xml_node_map<DWORD,Card>	card;

    xml_parser_begin(SummonConfig)
    xml_parser_map_node("card", "index", card);
    xml_parser_end(SummonConfig)
};

#endif //_XML_BASE_XMLPARSER_SUMMON_H

