#include "MarketSystemManager.h"
#include "zXML.h"
#include "SceneUser.h"
#include "Chat.h"
#include "zDatabaseManager.h"

///////////////////////////////////////////////
//
//code[ScenesServer/MarketSystemManager.cpp] defination by codemokey
//
//
///////////////////////////////////////////////

using namespace xml;


MarketSystemManager::MarketSystemManager()
{}

MarketSystemManager::~MarketSystemManager()
{}

bool MarketSystemManager::isMarketOpen()
{
    if(market.open.flag == 1)
	return true;
    return false;
}

bool MarketSystemManager::isIndexValid(const DWORD index)
{
    MarketConfig::Obj::ItemMapIter it = market.obj.item.find(index);
    if(it != market.obj.item.end())
	return true;
    return false;
}

DWORD MarketSystemManager::getPriceByMarketIndex(const DWORD index)
{
    DWORD price = (DWORD)-1;
    MarketConfig::Obj::ItemMapIter it = market.obj.item.find(index);
    if(it != market.obj.item.end())
    {
	price = it->second.price;
    }
    return price;
}

bool MarketSystemManager::handleBuyMarketObject(SceneUser& user, const DWORD index)
{
    if(!isMarketOpen())
    {
	return false;
    }
    if(!isIndexValid(index)) 
	return false;
    if(user.packs.main.space() < 1)
    {
	return false;
    }

    DWORD price = getPriceByMarketIndex(index);
    if(user.packs.checkGold(price))
    {
	user.packs.removeGold(price, "[商城]购买道具扣除");
    }
    else
    {
	return false;
    }

    DWORD objid = 0;
    DWORD objnum = 0;
    MarketConfig::Obj::ItemMapIter it = market.obj.item.find(index);
    if(it != market.obj.item.end())
    {
	objid = it->second.objid;
	objnum = it->second.num;
    }

    zObjectB *base = objectbm.get(objid);
    if(!base)
	return false;
    zObject *o = zObject::create(base, objnum, 0);
    if(!o)
	return false;

    zObject::logger(o->createid,o->data.qwThisID,o->data.strName,o->data.dwNum,o->data.dwNum,1,0,NULL,user.id,user.name,"商城购买获得",o->base,o->data.kind,o->data.upgrade);
    Combination callback(&user, o); 
    user.packs.main.execEvery(callback);	    //首先向主包裹中合并

    if(o->data.dwNum)				    //没有合并完
    {
	if(user.packs.addObject(o, true, Packages::MAIN_PACK))
	{
	    Cmd::stAddMobileObjectPropertyUserCmd status;
	    status.byActionType = Cmd::EQUIPACTION_OBTAIN;
	    o->fullMobileObject(status.object);
	    user.sendCmdToMe(&status, sizeof(status));

	    Cmd::stReqBuyMobileObjectPropertyUserCmd ok;
	    ok.index = index;
	    user.sendCmdToMe(&ok, sizeof(ok));
	}
	else
	{
	    zObject::destroy(o);
	    return false;
	}
    }
    return true;
}

void MarketSystemManager::notifyMarketInfo(SceneUser& user)
{
    BUFFER_CMD(Cmd::stNotifyMarketAllObjectPropertyUserCmd, send, zSocket::MAX_USERDATASIZE);
    MarketConfig::Obj::ItemMapIter it = market.obj.item.begin();
    for(; it!=market.obj.item.end(); it++)
    {
	send->id[send->count] = it->first;
	send->count++;
    }
    user.sendCmdToMe(send, sizeof(Cmd::stNotifyMarketAllObjectPropertyUserCmd)+send->count*sizeof(send->id[0]));
}
