#ifndef _XML_BASE_XMLPARSER_CARDTUJIAN_H
#define _XML_BASE_XMLPARSER_CARDTUJIAN_H
#include "zType.h"
///////////////////////////////////////////////
//
//config[Config/cardtujian.xml] defination by codemokey
//
// notice:this file was created by a tool,please do not modify
//
///////////////////////////////////////////////

struct CardTujianConfig
{
    struct Open
    {
        xml_prop<WORD>	flag;

        xml_parser_begin(Open)
        xml_parser_prop("flag",	flag);
        xml_parser_end(Open)
    };

    struct Limit
    {
        xml_prop<DWORD>	groupMaxNum;
        xml_prop<DWORD>	totalGroup;
        xml_prop<DWORD>	sameNameNum;
        xml_prop<DWORD>	legendNum;

        xml_parser_begin(Limit)
        xml_parser_prop("groupMaxNum",	groupMaxNum);
        xml_parser_prop("totalGroup",	totalGroup);
        xml_parser_prop("sameNameNum",	sameNameNum);
        xml_parser_prop("legendNum",	legendNum);
        xml_parser_end(Limit)
    };

    struct Init
    {
        struct Item
        {
            xml_prop<DWORD>	id;
            xml_prop<DWORD>	num;

            xml_parser_begin(Item)
            xml_parser_prop("id",	id);
            xml_parser_prop("num",	num);
            xml_parser_end(Item)
        };
        typedef xml_node_map<DWORD,Item>	ItemMap;
        typedef ItemMap::iterator	ItemMapIter;
        typedef ItemMap::const_iterator	ItemMapConstIter;

        xml_node_map<DWORD,Item>	item;

        xml_parser_begin(Init)
        xml_parser_map_node("item", "id", item);
        xml_parser_end(Init)
    };

    xml_node<Open>	open;
    xml_node<Limit>	limit;
    xml_node<Init>	init;

    xml_parser_begin(CardTujianConfig)
    xml_parser_node("open",	open);
    xml_parser_node("limit",	limit);
    xml_parser_node("init",	init);
    xml_parser_end(CardTujianConfig)
};

#endif //_XML_BASE_XMLPARSER_CARDTUJIAN_H

