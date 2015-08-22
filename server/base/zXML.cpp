#include "zXML.h"
#include "Zebra.h"
#include <iostream>
#include <fstream>
#include <sstream>
#include "xmlparser/test.h"
#include "xmlparser/herobase.h"
#include "xmlparser/market.h"
#include "xmlparser/giftbag.h"
#include "xmlparser/battle.h"
#include "xmlparser/cardtujian.h"
#include "xmlparser/summon.h"

XML_CONFIG_NAMESPACE_BEGIN

xml_config<TestConfig> test;
xml_config<HeroBaseCFG> herobase;
xml_config<MarketConfig> market;
xml_config<GiftBagConfig> giftbag;
xml_config<BattleConfig> battle;
xml_config<CardTujianConfig> cardtujian;
xml_config<SummonConfig> summon;

void Configs::load_base()
{
    load_config(battle, "battleconfig.xml");
}

void Configs::load_scene()
{
    load_base();
    //load_config(test, "test.xml");
    load_config(herobase, "herobasecfg.xml");
    load_config(market, "marketconfig.xml");
    load_config(giftbag, "giftbagconfig.xml");
    load_config(cardtujian, "cardtujian.xml");
    load_config(summon, "summonconfig.xml");
}

void Configs::load_session()
{
	load_base();
}

ConfigMap Configs::_configs;

void Configs::load_config(xml::xml_config_base& xml, const std::string& filename)
{
	if(!xml.dynamic_load(Zebra::global["configdir"] + filename))
	{
		std::ostringstream os;
		xml.dynamic_dump(os);
		Zebra::logger->warn("�Զ��������ļ�����ʧ��%s",filename.c_str());
	}
	_configs[filename] = &xml;
}

bool Configs::reload_config(const std::string& filename)
{
	xml::xml_config_base* config = get_config(filename);
	
	if(!config)
	{
		Zebra::logger->warn("�Զ��������ļ�����ʧ��,û���ҵ��ļ�");
		return false;
	}
	xml::xml_config_base* clone = config->clone();
	if(!clone)
	{
		Zebra::logger->warn("�Զ��������ļ�����ʧ�ܣ�cloneʧ��");
		return false;
	}
	
	if(!clone->dynamic_load())
	{
		std::ostringstream os;
		clone->dynamic_dump(os);
		delete clone;
		Zebra::logger->warn("�Զ��������ļ�����ʧ��,dynamic_loadʧ��");
		return false;
	}
	config->dynamic_copy(clone);
	delete clone;
	return true;
}

bool Configs::dump_config(const std::string& filename)
{
    xml::xml_config_base* config = get_config(filename);

    if(!config)
    {
	Zebra::logger->warn("�Զ��������ļ�����ʧ��");
	return false;
    }
    std::ostringstream os;
    config->dynamic_dump(os);
    return true;
}

bool Configs::dump_config_xmlfile(const std::string& filename, const std::string& xmlfilename)
{
	xml::xml_config_base* config = get_config(filename);
	
	if(!config)
	{
		Zebra::logger->warn("�Զ��������ļ�����ʧ��");
		return false;
	}
	std::ofstream of(xmlfilename.c_str());
	config->dynamic_dump(of);
	return true;
}

xml::xml_config_base* Configs::get_config(const std::string& filename)
{
	ConfigMap::iterator it = _configs.find(filename);
	if(it == _configs.end())
		return NULL;
	return it->second;
}

XML_CONFIG_NAMESPACE_END

