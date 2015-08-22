/*************************************************************************
 Author: wang
 Created Time: 2014��12��16�� ���ڶ� 16ʱ07��01��
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
    HERO_OCCUPATION_1 = 1,	//ְҵ1	
    HERO_OCCUPATION_2 = 2,	//ְҵ2
    HERO_OCCUPATION_3 = 3,	//ְҵ3

    HERO_OCCUPATION_MAX,	//ְҵMAX
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

    DWORD getHCardByOccupation(const WORD occupation);	    //��ȡӢ��
    DWORD getSCardByOccupation(const WORD occupation);	    //��ȡ����
    private:
    QWORD getCurrMaxExpByLevel(const WORD level);
    void refreshOneHero(SceneUser& user, const WORD occupation);
};

struct HeroData
{
    WORD level;		//Ӣ�۵ȼ�
    QWORD exp;		//Ӣ�۾���
    BYTE isActive;	//Ӣ���Ƿ��Ѽ���
    BYTE isGold;	//Ӣ���Ƿ��ǽ�ɫ
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
	WORD inUseOccupation;			    //��ս��ǰʹ�õ�ְҵ
	std::map<DWORD, HeroData> heroData;	    //Ӣ��ְҵ----Ӣ������
	unsigned int saveHeroData(unsigned char* dest);
	unsigned int loadHeroData(unsigned char* dest);

};
#endif

