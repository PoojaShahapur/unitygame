#include "GiftBagManager.h"
#include "zXML.h"
#include "SceneUser.h"
#include "HeroCardCmd.h"
#include "zDatabaseManager.h"

///////////////////////////////////////////////
//
//code[ScenesServer/GiftBagManager.cpp] defination by codemokey
//
//
///////////////////////////////////////////////
using namespace xml;



GiftBagManager::GiftBagManager()
{}

GiftBagManager::~GiftBagManager()
{}

void GiftBagManager::getGiftBagQualityVec(const DWORD id, std::vector<DWORD> &qualitys)
{
    GiftBagConfig::GiftMapIter it = giftbag.gift.find(id);
    if(it != giftbag.gift.end())
    {
	GiftBagConfig::Gift::CardContIter cit = it->second.card.begin();
	for(; cit!=it->second.card.end(); cit++)
	{
	    DWORD quality = 1;
	    std::vector<std::string> vs_str;
	    std::vector<DWORD> vs;
	    vs_str.clear();
	    vs.clear();
	    Zebra::stringtok(vs_str, cit->qualityOdds, ";");
	    for(std::vector<std::string>::iterator sit = vs_str.begin(); sit != vs_str.end(); sit++)
	    {
		vs.push_back(atoi((*sit).c_str()));
	    }
	    DWORD num = zMisc::randBetween(1, 100);
	    DWORD tempnum = 0;
	    for(std::vector<DWORD>::iterator i=vs.begin(); i!=vs.end(); i++)
	    {
		if(num <= (*i+tempnum))
		{
		    break; 
		}
		else
		{
		    tempnum += (*i);
		    quality++;
		}
	    }
	    if(quality > 0)
	    {
		qualitys.push_back(quality);
	    }
	}
    }
}

DWORD GiftBagManager::randomOneCard(const DWORD index, const DWORD level)
{
    DWORD cardID = 0;
    GiftBagConfig::BagMapIter it = giftbag.bag.find(index);
    if(it != giftbag.bag.end())
    {
	GiftBagConfig::Bag::QualityMapIter qit = it->second.quality.find(level);
	if(qit != it->second.quality.end())
	{
	    GiftBagConfig::Bag::Quality::ItemMapIter iit = qit->second.item.begin();
	    DWORD sum = 0;
	    for(; iit != qit->second.item.end(); iit++)
	    {
		sum += iit->second.odds;
	    }
	    DWORD num = zMisc::randBetween(1, sum);
	    DWORD tempnum = 0;
	    iit = qit->second.item.begin();
	    for(; iit != qit->second.item.end(); iit++)
	    {
		if(num <= (iit->second.odds+tempnum))
		{
		    cardID = iit->first;
		    break;
		}
		else
		{
		    tempnum += iit->second.odds;
		}
	    }
	    
	}
    }
    return cardID;
}

void GiftBagManager::openOneGiftBag(const DWORD objid, std::vector<DWORD> &cards)
{
    GiftBagConfig::GiftMapIter it = giftbag.gift.find(objid);
    if(it != giftbag.gift.end())
    {
	DWORD index = 0;
	index = it->second.index;
	std::vector<DWORD> qualitys;
	qualitys.clear();
	getGiftBagQualityVec(objid, qualitys);
	for(std::vector<DWORD>::iterator cit = qualitys.begin();  cit != qualitys.end(); cit++)
	{
	    DWORD cardID = randomOneCard(index, *cit);
	    if(cardID > 0)
	    {
		cards.push_back(cardID);
	    }
	}
    }
}

bool GiftBagManager::useGiftBag(SceneUser& user, zObject *obj)
{
    if(!obj)
	return false;
    std::vector<DWORD> cards;
    cards.clear();
    DWORD objid = obj->base->id;
    openOneGiftBag(objid, cards);
    if(cards.empty())
	return false;


    if(obj->data.dwNum > 0)
    {
	obj->data.dwNum--;
    }
    else
    {
	return false;
    }
    zObject::logger(obj->createid,obj->data.qwThisID,obj->data.strName,obj->data.dwNum,obj->data.dwNum,1,0,NULL,user.id,user.name,"开卡包扣除",obj->base,obj->data.kind,obj->data.upgrade);
    if(obj->data.dwNum == 0)
    {
	if(false == user.packs.removeObject(obj))
	    return false;
    }
    else
    {
	Cmd::stRefCountObjectPropertyUserCmd cmd;
	cmd.qwThisID = obj->data.qwThisID;
	cmd.dwNum = obj->data.dwNum;
	user.sendCmdToMe(&cmd, sizeof(cmd));
    }

    std::random_shuffle(cards.begin(), cards.end());		//打乱一下顺序
    Cmd::stRetGiftBagCardsDataUserCmd send;
    DWORD index = 0;
    for(std::vector<DWORD>::iterator it = cards.begin(); it != cards.end(); it++)
    {
	DWORD id = *it;
	zCardB *cb = cardbm.get(id);
	if(cb)
	    Zebra::logger->trace("[卡包系统] %s 打开:%u 获得:%s(%u) 卡",user.name, objid, cb->name, id);
	CardTujianManager::getMe().addTuJian(user, id);
	if(index < 5)
	{
	    send.id[index] = id;
	    index++;
	}
    }
    user.sendCmdToMe(&send, sizeof(send));
    return true;
}
