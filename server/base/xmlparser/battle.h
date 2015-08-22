#ifndef _XML_BASE_XMLPARSER_BATTLE_H
#define _XML_BASE_XMLPARSER_BATTLE_H
#include "zType.h"
///////////////////////////////////////////////
//
//config[Config/battleconfig.xml] defination by codemokey
//
// notice:this file was created by a tool,please do not modify
//
///////////////////////////////////////////////

struct BattleConfig
{
    struct Limit
    {
        xml_prop<DWORD>	preparetime;
        xml_prop<DWORD>	roundtime;
        xml_prop<DWORD>	peaceNum;
        xml_prop<DWORD>	luckyCoin;
        xml_prop<DWORD>	tiredCard;

        xml_parser_begin(Limit)
        xml_parser_prop("preparetime",	preparetime);
        xml_parser_prop("roundtime",	roundtime);
        xml_parser_prop("peaceNum",	peaceNum);
        xml_parser_prop("luckyCoin",	luckyCoin);
        xml_parser_prop("tiredCard",	tiredCard);
        xml_parser_end(Limit)
    };

    struct ScenesPVP
    {
        struct Item
        {
            xml_prop<DWORD>	id;

            xml_parser_begin(Item)
            xml_parser_prop("id",	id);
            xml_parser_end(Item)
        };
        typedef xml_node_vector<Item>	ItemCont;
        typedef ItemCont::iterator	ItemContIter;
        typedef ItemCont::const_iterator	ItemContConstIter;

        xml_node_vector<Item>	item;

        xml_parser_begin(ScenesPVP)
        xml_parser_node("item",	item);
        xml_parser_end(ScenesPVP)
    };

    struct BattleServer
    {
        struct Item
        {
            xml_prop<DWORD>	id;

            xml_parser_begin(Item)
            xml_parser_prop("id",	id);
            xml_parser_end(Item)
        };
        typedef xml_node_map<DWORD,Item>	ItemMap;
        typedef ItemMap::iterator	ItemMapIter;
        typedef ItemMap::const_iterator	ItemMapConstIter;

        xml_node_map<DWORD,Item>	item;

        xml_parser_begin(BattleServer)
        xml_parser_map_node("item", "id", item);
        xml_parser_end(BattleServer)
    };

    xml_node<Limit>	limit;
    xml_node<ScenesPVP>	scenesPVP;
    xml_node<BattleServer>	battleServer;

    xml_parser_begin(BattleConfig)
    xml_parser_node("limit",	limit);
    xml_parser_node("scenesPVP",	scenesPVP);
    xml_parser_node("battleServer",	battleServer);
    xml_parser_end(BattleConfig)
};

#endif //_XML_BASE_XMLPARSER_BATTLE_H

