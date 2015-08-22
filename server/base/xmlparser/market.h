#ifndef _XML_BASE_XMLPARSER_MARKET_H
#define _XML_BASE_XMLPARSER_MARKET_H
#include "zType.h"
///////////////////////////////////////////////
//
//config[Config/marketconfig.xml] defination by codemokey
//
// notice:this file was created by a tool,please do not modify
//
///////////////////////////////////////////////

struct MarketConfig
{
    struct Open
    {
        xml_prop<WORD>	flag;

        xml_parser_begin(Open)
        xml_parser_prop("flag",	flag);
        xml_parser_end(Open)
    };

    struct Obj
    {
        struct Item
        {
            xml_prop<DWORD>	index;
            xml_prop<DWORD>	objid;
            xml_prop<DWORD>	num;
            xml_prop<DWORD>	price;

            xml_parser_begin(Item)
            xml_parser_prop("index",	index);
            xml_parser_prop("objid",	objid);
            xml_parser_prop("num",	num);
            xml_parser_prop("price",	price);
            xml_parser_end(Item)
        };
        typedef xml_node_map<DWORD,Item>	ItemMap;
        typedef ItemMap::iterator	ItemMapIter;
        typedef ItemMap::const_iterator	ItemMapConstIter;

        xml_node_map<DWORD,Item>	item;

        xml_parser_begin(Obj)
        xml_parser_map_node("item", "index", item);
        xml_parser_end(Obj)
    };

    xml_node<Open>	open;
    xml_node<Obj>	obj;

    xml_parser_begin(MarketConfig)
    xml_parser_node("open",	open);
    xml_parser_node("obj",	obj);
    xml_parser_end(MarketConfig)
};

#endif //_XML_BASE_XMLPARSER_MARKET_H

