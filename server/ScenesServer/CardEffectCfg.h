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
	    std::vector<DWORD> deadIDVec;   //����ID(skill)
	    DWORD select1ID;	    //����1(����ID)
	    DWORD select2ID;	    //����2(����ID)
	    std::vector<t_EffectUnit> roundSIDVec;	//����غϿ�ʼ(skill)
	    std::vector<t_EffectUnit> roundEIDVec;	//����غϽ���(skill)
	    std::vector<t_EffectUnit> enemyroundSIDVec;	//�з��غϿ�ʼ(skill)
	    std::vector<t_EffectUnit> enemyroundEIDVec;	//�з��غϽ���(skill)
	    DWORD sAttackID;	    //����ʼ����ʱ(skill)
	    DWORD beAttackID;	    //��������ʱ(skill)
	    DWORD drawID;	    //�������ʱ(skill)
	    DWORD beHurtID;	    //��������ʱ(skill)
	    DWORD beCureID;	    //�����ܵ�����ʱ(skill)
	    DWORD drawedID;	    //����鵽����ʱ(skill)
	    ConditionStatus hurtStatus;
	    DWORD otherBeHurtID;    //��ɫ����
	    ConditionStatus deadStatus;
	    DWORD otherDeadID;	    //��ɫ����
	    ConditionStatus cureStatus;
	    DWORD otherBeCureID;    //��ɫ������
	    DWORD selfUseMagic;	    //����ʹ�÷�����(skill)
	    DWORD enemyUseMagic;    //�з�ʹ�÷�����(skill)
	    ConditionStatus useAttendStatus;
	    DWORD otherUseAttendID; //ʹ��һ�������(skill)
	    ConditionStatus attendInStatus;
	    DWORD otherAttendInID;  //��ӽ���(skill)

	    DWORD halo_Ctype;	//�⻷������������
	    DWORD halo_Cid;	//�⻷��������ID
	    DWORD haloID;	//�⻷(skill)

	    DWORD attackEndCondition;	//������������
	    DWORD attackEndID;		//��������(skill)
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

