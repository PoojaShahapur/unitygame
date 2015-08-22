/*************************************************************************
 Author: wang
 Created Time: 2014年12月16日 星期二 16时07分01秒
 File Name: ScenesServer/HeroInfoManager.h
 Description: 
 ************************************************************************/
#ifndef _HeroInfoManager_h_
#define _HeroInfoManager_h_

#include "zSingleton.h"
#include "zType.h"
#include <map>

class SceneUser;

enum
{
    HERO_OCCUPATION_NONE = 0,	//NONE	
    HERO_OCCUPATION_1 = 1,	//职业1	
    HERO_OCCUPATION_2 = 2,	//职业2
    HERO_OCCUPATION_3 = 3,	//职业3

    HERO_OCCUPATION_MAX,	//职业MAX
};

class HeroInfoManager : public Singleton<HeroInfoManager>
{
    friend class SingletonFactory<HeroInfoManager>;
    public:
    HeroInfoManager();
    ~HeroInfoManager();
    void initData(SceneUser &user);
    void addOneHeroExp(SceneUser &user, const WORD occupation, const DWORD value);
    void heroUpgradeOneLevel(SceneUser &user, const WORD occupation);
    WORD getHeroTotalLevel(SceneUser &user);
    WORD getOneHeroLevel(SceneUser *user, const WORD occupation);
    void activeOneHero(SceneUser *user, const WORD occupation);
    void notifyAllHeroInfoToMe(SceneUser& user);
    void setHeroInUse(SceneUser& user, const WORD occupation);
    void clearHeroInUse(SceneUser& user);

    DWORD getHCardByOccupation(const WORD occupation);	    //获取英雄
    DWORD getSCardByOccupation(const WORD occupation);	    //获取技能
    private:
    QWORD getCurrMaxExpByLevel(const WORD level);
    void refreshOneHero(SceneUser& user, const WORD occupation);
};

struct HeroData
{
    WORD level;		//英雄等级
    QWORD exp;		//英雄经验
    BYTE isActive;	//英雄是否已激活
    BYTE isGold;	//英雄是否是金色
    HeroData()
    {
	bzero(this, sizeof(*this));
    }
    WORD getLevel()
    {
	return level;
    }
    void addHeroExp(DWORD value)
    {
	exp += value;
    }
    bool isGoldHero()
    {
	return isGold;
    }
    void setGold()
    {
	isGold = 1;
    }
    bool isActived()
    {
	return isActive;
    }
    void setActive()
    {
	isActive = 1;
    }
}__attribute__((packed));

class AllHeroData
{
    public:
	AllHeroData()
	{
	    inUseOccupation = 0;
	    heroData.clear();
	}
	WORD inUseOccupation;			    //对战当前使用的职业
	std::map<DWORD, HeroData> heroData;	    //英雄职业----英雄数据
	unsigned int saveHeroData(unsigned char* dest);
	unsigned int loadHeroData(unsigned char* dest);

};
#endif

