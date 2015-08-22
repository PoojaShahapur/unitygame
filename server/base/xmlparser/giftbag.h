#ifndef _XML_BASE_XMLPARSER_GIFTBAG_H
#define _XML_BASE_XMLPARSER_GIFTBAG_H
#include "zType.h"
///////////////////////////////////////////////
//
//config[Config/giftbagconfig.xml] defination by codemokey
//
// notice:this file was created by a tool,please do not modify
//
///////////////////////////////////////////////

struct GiftBagConfig
{
    struct Open
    {
        xml_prop<WORD>	flag;

        xml_parser_begin(Open)
        xml_parser_prop("flag",	flag);
        xml_parser_end(Open)
    };

    struct Bag
    {
        struct Quality
        {
            struct Item
            {
                xml_prop<DWORD>	cardid;
                xml_prop<DWORD>	odds;

                xml_parser_begin(Item)
                xml_parser_prop("cardid",	cardid);
                xml_parser_prop("odds",	odds);
                xml_parser_end(Item)
            };
            typedef xml_node_map<DWORD,Item>	ItemMap;
            typedef ItemMap::iterator	ItemMapIter;
            typedef ItemMap::const_iterator	ItemMapConstIter;

            xml_prop<DWORD>	level;

            xml_node_map<DWORD,Item>	item;

            xml_parser_begin(Quality)
            xml_parser_prop("level",	level);
            xml_parser_map_node("item", "cardid", item);
            xml_parser_end(Quality)
        };
        typedef xml_node_map<DWORD,Quality>	QualityMap;
        typedef QualityMap::iterator	QualityMapIter;
        typedef QualityMap::const_iterator	QualityMapConstIter;

        xml_prop<DWORD>	index;

        xml_node_map<DWORD,Quality>	quality;

        xml_parser_begin(Bag)
        xml_parser_prop("index",	index);
        xml_parser_map_node("quality", "level", quality);
        xml_parser_end(Bag)
    };
    typedef xml_node_map<DWORD,Bag>	BagMap;
    typedef BagMap::iterator	BagMapIter;
    typedef BagMap::const_iterator	BagMapConstIter;

    struct Gift
    {
        struct Card
        {
            xml_prop<std::string>	qualityOdds;

            xml_parser_begin(Card)
            xml_parser_prop("qualityOdds",	qualityOdds);
            xml_parser_end(Card)
        };
        typedef xml_node_vector<Card>	CardCont;
        typedef CardCont::iterator	CardContIter;
        typedef CardCont::const_iterator	CardContConstIter;

        xml_prop<DWORD>	objid;
        xml_prop<DWORD>	index;

        xml_node_vector<Card>	card;

        xml_parser_begin(Gift)
        xml_parser_prop("objid",	objid);
        xml_parser_prop("index",	index);
        xml_parser_node("card",	card);
        xml_parser_end(Gift)
    };
    typedef xml_node_map<DWORD,Gift>	GiftMap;
    typedef GiftMap::iterator	GiftMapIter;
    typedef GiftMap::const_iterator	GiftMapConstIter;

    xml_node<Open>	open;
    xml_node_map<DWORD,Bag>	bag;
    xml_node_map<DWORD,Gift>	gift;

    xml_parser_begin(GiftBagConfig)
    xml_parser_node("open",	open);
    xml_parser_map_node("bag", "index", bag);
    xml_parser_map_node("gift", "objid", gift);
    xml_parser_end(GiftBagConfig)
};

#endif //_XML_BASE_XMLPARSER_GIFTBAG_H

