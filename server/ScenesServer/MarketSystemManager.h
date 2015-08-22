#ifndef _SCENESSERVER_MARKETSYSTEMMANAGER_H_
#define _SCENESSERVER_MARKETSYSTEMMANAGER_H_
#include "zType.h"
#include "zSingleton.h"
///////////////////////////////////////////////
//
//code[ScenesServer/MarketSystemManager.h] defination by codemokey
//
//
///////////////////////////////////////////////

class SceneUser;

class MarketSystemManager : public Singleton<MarketSystemManager>
{
    public:
        friend class SingletonFactory<MarketSystemManager>;
        MarketSystemManager();
        ~MarketSystemManager();
	bool isMarketOpen();
	bool isIndexValid(const DWORD index);
	bool handleBuyMarketObject(SceneUser& user, const DWORD index);	    //处理购买
	void notifyMarketInfo(SceneUser& user);
    private:
	DWORD getPriceByMarketIndex(const DWORD index);			    //根据索引获取售卖价格
};

#endif //_SCENESSERVER_MARKETSYSTEMMANAGER_H_

