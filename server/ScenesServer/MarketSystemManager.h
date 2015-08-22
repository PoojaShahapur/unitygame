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
	bool handleBuyMarketObject(SceneUser& user, const DWORD index);	    //������
	void notifyMarketInfo(SceneUser& user);
    private:
	DWORD getPriceByMarketIndex(const DWORD index);			    //����������ȡ�����۸�
};

#endif //_SCENESSERVER_MARKETSYSTEMMANAGER_H_

