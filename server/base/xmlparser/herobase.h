#ifndef _XML_BASE_XMLPARSER_HEROBASE_H
#define _XML_BASE_XMLPARSER_HEROBASE_H
#include "zType.h"
///////////////////////////////////////////////
//
//config[Config/herobasecfg.xml] defination by codemokey
//
// notice:this file was created by a tool,please do not modify
//
///////////////////////////////////////////////

struct HeroBaseCFG
{
    struct Exp
    {
        struct Item
        {
            xml_prop<WORD>	level;
            xml_prop<QWORD>	exp;

            xml_parser_begin(Item)
            xml_parser_prop("level",	level);
            xml_parser_prop("exp",	exp);
            xml_parser_end(Item)
        };
        typedef xml_node_map<WORD,Item>	ItemMap;
        typedef ItemMap::iterator	ItemMapIter;
        typedef ItemMap::const_iterator	ItemMapConstIter;

        xml_prop<WORD>	maxlevel;

        xml_node_map<WORD,Item>	item;

        xml_parser_begin(Exp)
        xml_parser_prop("maxlevel",	maxlevel);
        xml_parser_map_node("item", "level", item);
        xml_parser_end(Exp)
    };

    struct Init
    {
        struct Item
        {
            xml_prop<DWORD>	occupation;
            xml_prop<DWORD>	hcard;
            xml_prop<DWORD>	scard;

            xml_parser_begin(Item)
            xml_parser_prop("occupation",	occupation);
            xml_parser_prop("hcard",	hcard);
            xml_parser_prop("scard",	scard);
            xml_parser_end(Item)
        };
        typedef xml_node_map<DWORD,Item>	ItemMap;
        typedef ItemMap::iterator	ItemMapIter;
        typedef ItemMap::const_iterator	ItemMapConstIter;

        xml_node_map<DWORD,Item>	item;

        xml_parser_begin(Init)
        xml_parser_map_node("item", "occupation", item);
        xml_parser_end(Init)
    };

    xml_node<Exp>	exp;
    xml_node<Init>	init;

    xml_parser_begin(HeroBaseCFG)
    xml_parser_node("exp",	exp);
    xml_parser_node("init",	init);
    xml_parser_end(HeroBaseCFG)
};

#endif //_XML_BASE_XMLPARSER_HEROBASE_H

