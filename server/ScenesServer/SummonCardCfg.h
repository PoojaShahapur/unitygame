#ifndef _SCENESSERVER_SUMMONCARDCFG_H_
#define _SCENESSERVER_SUMMONCARDCFG_H_
#include "zType.h"
#include "zSingleton.h"
///////////////////////////////////////////////
//
//code[ScenesServer/SummonCardCfg.h] defination by codemokey
//
//
///////////////////////////////////////////////

class SummonCardCfg : public Singleton<SummonCardCfg>
{
    public:
        friend class SingletonFactory<SummonCardCfg>;
        SummonCardCfg();
        ~SummonCardCfg();
	DWORD randomOneIDByIndex(DWORD index);
};

#endif //_SCENESSERVER_SUMMONCARDCFG_H_

