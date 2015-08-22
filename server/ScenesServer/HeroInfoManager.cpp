/*************************************************************************
 Author: wang
 Created Time: 2014年12月16日 星期二 16时46分40秒
 File Name: ScenesServer/HeroInfoManager.cpp
 Description: 
 ************************************************************************/
#include "HeroInfoManager.h"
#include "SceneUser.h"
#include "QuestEvent.h"
#include "QuestTable.h"
#include "zXML.h"
using namespace xml;

HeroInfoManager::HeroInfoManager()
{
}

HeroInfoManager::~HeroInfoManager()
{
}

void HeroInfoManager::initData(SceneUser& user)
{
    HeroData info;
    info.level = 1;
    info.exp = 0;
    info.isActive = 0;
    info.isGold = 0;
    user.ahData.heroData[2] = info;
    user.ahData.heroData[3] = info;
    user.ahData.heroData[4] = info;
    user.ahData.heroData[5] = info;
    user.ahData.heroData[6] = info;
    user.ahData.heroData[7] = info;
    user.ahData.heroData[8] = info;
    user.ahData.heroData[9] = info;
    info.isActive = 1;
    user.ahData.heroData[1] = info;
}

void HeroInfoManager::addOneHeroExp(SceneUser &user, const WORD occupation, const DWORD value)
{
    if(value == 0)
	return;
    std::map<DWORD, HeroData>::iterator it = user.ahData.heroData.find(occupation);
    if(it != user.ahData.heroData.end())
    {
	WORD oldLevel = it->second.getLevel();
	QWORD max_exp = getCurrMaxExpByLevel(oldLevel);
	QWORD oldExp = it->second.exp;
	if(oldExp + value >= max_exp)
	{
	    if(oldLevel+1 <= herobase.exp.maxlevel)
	    {
		it->second.exp = (oldExp + value) - max_exp;
		heroUpgradeOneLevel(user, occupation);
		Zebra::logger->debug("[英雄信息]角色名:%s 职业:%u 获得经验前(%u,%llu) 获得经验后(%u,%llu) 升级",user.name, occupation, oldLevel,oldExp,it->second.getLevel(),it->second.exp);
	    }
	    else
	    {
		it->second.exp = max_exp;
		Zebra::logger->debug("[英雄信息]角色名:%s 职业:%u 获得经验前(%u,%llu) 获得经验后(%u,%llu) 升满级了",user.name, occupation, oldLevel,oldExp,it->second.getLevel(),it->second.exp);
	    }
	}
	else
	{
	    it->second.addHeroExp(value);
	    Zebra::logger->debug("[英雄信息]角色名:%s 职业:%u 获得经验前(%u,%llu) 获得经验后(%u,%llu) 加经验",user.name, occupation, oldLevel,oldExp,it->second.getLevel(),it->second.exp);
	}
	
	refreshOneHero(user, occupation);
    }
}

void HeroInfoManager::heroUpgradeOneLevel(SceneUser &user, const WORD occupation)
{
    std::map<DWORD, HeroData>::iterator it = user.ahData.heroData.find(occupation);
    if(it != user.ahData.heroData.end())
    {
	(it->second.level)++;
	
	DWORD eventID = 0;
	switch(occupation)
	{
	    case HERO_OCCUPATION_1:
		eventID = 1;
		break;
	    case HERO_OCCUPATION_2:
		eventID = 2;
		break;
	    case HERO_OCCUPATION_3:
		eventID = 3;
		break;
	    default:
		break;
	}
	OnOther event(eventID);   //处理英雄升级脚本
	EventTable::instance().execute(user, event);
    }
}

WORD HeroInfoManager::getHeroTotalLevel(SceneUser& user)
{
    WORD total = 0;
    for(std::map<DWORD, HeroData>::iterator it = user.ahData.heroData.begin();
	    it != user.ahData.heroData.end(); it++)
    {
	total += it->second.getLevel();
    }
    return total;
}

QWORD HeroInfoManager::getCurrMaxExpByLevel(const WORD level)
{
    HeroBaseCFG::Exp::ItemMapIter it = herobase.exp.item.find(level);
    if(it != herobase.exp.item.end())
    {
	return (it->second.exp);
    }
    return 0;
}

WORD HeroInfoManager::getOneHeroLevel(SceneUser *user, const WORD occupation)
{
    if(!user)
	return 0;
    std::map<DWORD, HeroData>::iterator it = user->ahData.heroData.find(occupation);
    if(it != user->ahData.heroData.end())
    {
	return (it->second.getLevel());
    }
    return 0;
}

void HeroInfoManager::activeOneHero(SceneUser *user, const WORD occupation)
{
    if(!user)
	return;
    std::map<DWORD, HeroData>::iterator it = user->ahData.heroData.find(occupation);
    if(it != user->ahData.heroData.end())
    {
	it->second.setActive();
	it->second.level = 1;
	refreshOneHero(*user, occupation);
    }
}

void HeroInfoManager::notifyAllHeroInfoToMe(SceneUser& user)
{
    BUFFER_CMD(Cmd::stRetAllHeroInfoUserCmd, send, zSocket::MAX_USERDATASIZE);
    std::map<DWORD, HeroData>::iterator it = user.ahData.heroData.begin();
    for(; it != user.ahData.heroData.end(); it++)
    {
	send->info[send->count].occupation = it->first;
	send->info[send->count].level = it->second.level;
	send->info[send->count].exp = it->second.exp;
	send->info[send->count].isActive = it->second.isActive;
	send->info[send->count].isGold = it->second.isGold;

	send->count++; 
    }
    user.sendCmdToMe(send, sizeof(Cmd::stRetAllHeroInfoUserCmd)+send->count*sizeof(send->info[0]));
}

void HeroInfoManager::refreshOneHero(SceneUser& user, const WORD occupation)
{
    std::map<DWORD, HeroData>::iterator it = user.ahData.heroData.find(occupation);
    if(it != user.ahData.heroData.end())
    {
	Cmd::stRetOneHeroInfoUserCmd send;
	send.info.occupation = it->first;
	send.info.level = it->second.level;
	send.info.exp = it->second.exp;
	send.info.isActive = it->second.isActive;
	send.info.isGold = it->second.isGold;
	user.sendCmdToMe(&send, sizeof(send));
    }
}

void HeroInfoManager::setHeroInUse(SceneUser& user, const WORD occupation)
{
    user.ahData.inUseOccupation = occupation;
}

void HeroInfoManager::clearHeroInUse(SceneUser& user)
{
    if(user.ahData.inUseOccupation > 0)
    {
	user.ahData.inUseOccupation = 0;
    }
}

DWORD HeroInfoManager::getHCardByOccupation(const WORD occupation)
{
    HeroBaseCFG::Init::ItemMapIter it = herobase.init.item.find(occupation);
    if(it != herobase.init.item.end())
    {
	return it->second.hcard;
    }
    return 0;
}

DWORD HeroInfoManager::getSCardByOccupation(const WORD occupation)
{
    HeroBaseCFG::Init::ItemMapIter it = herobase.init.item.find(occupation);
    if(it != herobase.init.item.end())
    {
	return it->second.scard;
    }
    return 0;
}
//////////////////////////////////////我就是分隔线/////////////////////////////////////////////////////
unsigned int AllHeroData::loadHeroData(unsigned char* dest)
{
    if(!dest)
	return 0;
    inUseOccupation = *((WORD*)dest);
    int len = sizeof(inUseOccupation);
    int size = *((int*)(dest+len));
    len += sizeof(int);
    DWORD occu = 0;
    while(size-->0)
    {
	occu = *((DWORD*)(dest+len));
	len += sizeof(DWORD);
	memcpy(&this->heroData[occu], dest+len, sizeof(HeroData));
	len += sizeof(HeroData);
    }
#ifdef _WC_DEBUG
    Zebra::logger->debug("[英雄信息]二进制加载字节数:%u", len);
#endif

    return len;
}

unsigned int AllHeroData::saveHeroData(unsigned char* dest)
{
    if(!dest)
	return 0;
    memcpy(dest, &inUseOccupation, sizeof(WORD));
    int len = sizeof(inUseOccupation);
    int size = this->heroData.size();
    memcpy(dest+len, &size, sizeof(size));
    len += sizeof(int);
    std::map<DWORD, HeroData>::iterator iter = this->heroData.begin();
    while(iter != this->heroData.end())
    {
	memcpy(dest+len, &(iter->first), sizeof(DWORD));
	len += sizeof(DWORD);
	memcpy(dest+len, &(iter->second), sizeof(HeroData));
	len += sizeof(HeroData);
	++iter;
    }
#ifdef _WC_DEBUG
    Zebra::logger->debug("[英雄信息]二进制保存字节数:%u", len);
#endif
    return len;
}
