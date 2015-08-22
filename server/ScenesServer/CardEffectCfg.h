#ifndef _SCENESSERVER_CARDEFFECTCFG_H_
#define _SCENESSERVER_CARDEFFECTCFG_H_
#include "zType.h"
#include "zSingleton.h"
#include "zDatabase.h"
#include <map>
///////////////////////////////////////////////
//
//code[ScenesServer/CardEffectCfg.h] defination by codemokey
//
//
///////////////////////////////////////////////
struct t_CardPK;
struct t_EffectUnit;

class CardEffectCfg : public Singleton<CardEffectCfg>
{
    public:
	friend class SingletonFactory<CardEffectCfg>;
	CardEffectCfg();
	~CardEffectCfg();
	bool loadAllEffect();
	bool loadOneEffect(const DWORD cardID);
	bool fullOneCardPKData(const DWORD cardID, t_CardPK &pk);
    private:
	struct EffectInfo
	{
	    std::vector<DWORD> deadIDVec;   //亡语ID(skill)
	    DWORD select1ID;	    //抉择1(卡牌ID)
	    DWORD select2ID;	    //抉择2(卡牌ID)
	    std::vector<t_EffectUnit> roundSIDVec;	//自身回合开始(skill)
	    std::vector<t_EffectUnit> roundEIDVec;	//自身回合结束(skill)
	    std::vector<t_EffectUnit> enemyroundSIDVec;	//敌方回合开始(skill)
	    std::vector<t_EffectUnit> enemyroundEIDVec;	//敌方回合结束(skill)
	    DWORD sAttackID;	    //自身开始攻击时(skill)
	    DWORD beAttackID;	    //自身被攻击时(skill)
	    DWORD drawID;	    //自身抽牌时(skill)
	    DWORD beHurtID;	    //自身受伤时(skill)
	    DWORD beCureID;	    //自身受到治疗时(skill)
	    DWORD drawedID;	    //自身抽到该牌时(skill)
	    ConditionStatus hurtStatus;
	    DWORD otherBeHurtID;    //角色受伤
	    ConditionStatus deadStatus;
	    DWORD otherDeadID;	    //角色死亡
	    ConditionStatus cureStatus;
	    DWORD otherBeCureID;    //角色被治疗
	    DWORD selfUseMagic;	    //己方使用法术牌(skill)
	    DWORD enemyUseMagic;    //敌方使用法术牌(skill)
	    ConditionStatus useAttendStatus;
	    DWORD otherUseAttendID; //使用一张随从牌(skill)
	    ConditionStatus attendInStatus;
	    DWORD otherAttendInID;  //随从进场(skill)

	    DWORD halo_Ctype;	//光环开启条件类型
	    DWORD halo_Cid;	//光环开启条件ID
	    DWORD haloID;	//光环(skill)

	    DWORD attackEndCondition;	//攻击结束条件
	    DWORD attackEndID;		//攻击结束(skill)
	    EffectInfo()
	    {
		memset(this, 0, sizeof(*this));
	    }
	};

	bool set_ConditionState(const char *data, ConditionStatus &cs)
	{
	    std::vector<std::string> v_sec;
	    Zebra::stringtok(v_sec, data, "-");
	    if (v_sec.size() != 3)
	    {
		return false;
	    }
	    std::vector<std::string>::iterator it = v_sec.begin();
	    for(int i=0; i<3; i++)
	    {
		cs.status[i] = (WORD)atoi(it->c_str());
		it++;
	    }
	    return true;
	}
	typedef std::map<DWORD, EffectInfo> EffectMap;
	typedef std::map<DWORD, EffectInfo>::iterator EffectMap_IT;
	EffectMap effectMap;

};

#endif //_SCENESSERVER_CARDEFFECTCFG_H_

