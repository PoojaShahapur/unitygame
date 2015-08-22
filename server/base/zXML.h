#ifndef _Z_XML_H_
#define _Z_XML_H_

#include "xml/xml.h"
#include "Zebra.h"
#define XML_CONFIG_NAMESPACE_BEGIN namespace xml {
#define XML_CONFIG_NAMESPACE_END }


XML_CONFIG_NAMESPACE_BEGIN

typedef std::map<std::string, xml::xml_config_base*> ConfigMap;

class Configs
{
	public:
	static void load_base();
	static void load_scene();
	static void load_session();
	static void load_config(xml::xml_config_base& xml, const std::string& filename);
	static bool reload_config(const std::string& filename);
	static bool dump_config(const std::string& filename);
	static bool dump_config_xmlfile(const std::string& filename, const std::string& xmlfilename);
	static xml::xml_config_base* get_config(const std::string& filename);
	private:
	static ConfigMap _configs;
};

#include "xmlparser/test.h"
#include "xmlparser/herobase.h"
#include "xmlparser/market.h"
#include "xmlparser/giftbag.h"
#include "xmlparser/battle.h"
#include "xmlparser/cardtujian.h"
#include "xmlparser/summon.h"

extern xml_config<TestConfig> test;
extern xml_config<HeroBaseCFG> herobase;
extern xml_config<MarketConfig> market;
extern xml_config<GiftBagConfig> giftbag;
extern xml_config<BattleConfig> battle;
extern xml_config<CardTujianConfig> cardtujian;
extern xml_config<SummonConfig> summon;


XML_CONFIG_NAMESPACE_END

#include "xml/xml_undef.h"

#endif


