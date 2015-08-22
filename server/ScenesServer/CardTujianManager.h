/*************************************************************************
 Author: wang
 Created Time: 2014年10月22日 星期三 11时07分00秒
 File Name: SessionServer/CardTujianManager.h
 Description: 
 ************************************************************************/
#ifndef _CardTujianManager_h
#define _CardTujianManager_h

#include "zSingleton.h"
#include "zType.h"
#include "Zebra.h"
#include <map>

class SceneUser;

class CardTujianManager : public Singleton<CardTujianManager>
{
    friend class SingletonFactory<CardTujianManager>;
    public:
    CardTujianManager();
    ~CardTujianManager();
    
    void initTujian(SceneUser& user);
    bool addTuJian(SceneUser& user, const DWORD id);
    void notifyAllTujianDataToMe(SceneUser& user);
    WORD getOneTujianNum(SceneUser& user, const DWORD id);
};

class CardTujianData
{
    public:
	CardTujianData()
	{
	    privateCardMap.clear();
	}
	std::map<DWORD, BYTE> privateCardMap;		     //baseid<---->num
	unsigned int saveCardTujianData(unsigned char* dest);
	unsigned int loadCardTujianData(unsigned char* src);
};
#endif

