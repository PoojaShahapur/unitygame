/*************************************************************************
 Author: wang
 Created Time: 2014年12月09日 星期二 14时15分48秒
 File Name: base/Card.h
 Description: 
 ************************************************************************/
#ifndef _CARD_H_
#define _CARD_H_
#include "Object.h"
#include "zDatabase.h"
#pragma pack(1)
enum
{
    CARD_STATE_NONE    = 0,    //空状态
    CARD_STATE_TAUNT    = 1,    //嘲讽
    CARD_STATE_CHARGE   = 2,    //冲锋
    CARD_STATE_WINDFURY = 3,    //风怒
    CARD_STATE_SNEAK    = 4,    //隐形
    CARD_STATE_SHIED    = 5,    //圣盾
    CARD_STATE_SLEEP    = 6,    //睡
    CARD_STATE_FREEZE   = 7,    //冻结
    CARD_STATE_BECURE	= 8,	//有效果
    CARD_STATE_BEHURT	= 9,	//有效果
    CARD_STATE_DEADLAN	= 10,	//有效果
    CARD_STATE_SRSTART	= 11,	//有效果
    CARD_STATE_SREND	= 12,	//有效果
    CARD_STATE_ERSTART	= 13,	//有效果
    CARD_STATE_EREND	= 14,	//有效果
    CARD_STATE_SUSEMA	= 15,	//有效果
    CARD_STATE_EUSEMA	= 16,	//有效果
    CARD_STATE_ASTART	= 17,	//有效果
    CARD_STATE_BASTART	= 18,	//有效果
    CARD_STATE_DRAW	= 19,	//有效果
    CARD_STATE_DRAWED	= 20,	//有效果
    CARD_STATE_OHURT	= 21,	//有效果
    CARD_STATE_OCURE	= 22,	//有效果
    CARD_STATE_ODEAD	= 23,	//有效果
    CARD_STATE_SUSEA	= 24,	//有效果
    CARD_STATE_ATTENDIN	= 25,	//有效果
    CARD_STATE_AEND	= 26,	//有效果
    CARD_STATE_HALO	= 27,	//有效果

    CARD_STATE_MAX,

};

struct t_Card
{
	DWORD qwThisID;		    //物品唯一id
	DWORD dwObjectID;		    //物品表中的编号
	stObjectLocation pos;	// 位置
	DWORD mpcost;		    //蓝耗
	DWORD damage;		    //攻击力
	DWORD hp;		    //血量
	DWORD maxhp;		    //血量上限
	DWORD dur;		    //耐久度
	
	BYTE magicDamAdd;	//法术伤害增加(X)
	BYTE overload;		//过载(num)
	
	DWORD armor;		//护甲值
	BYTE attackTimes;	    //回合攻击次数
	BYTE equipOpen;		//武器状态(1开启,0关闭)
	BYTE side;		//1:player1; 2:player2
	DWORD popHpValue;		//冒出的数字(回血)
	DWORD popDamValue;		//冒出的数字(受伤)

	BYTE state[(CARD_STATE_MAX + 7) / 8];
	t_Card()
	{
	    memset(this, 0, sizeof(*this));
	}
};
#pragma pack()

/**
 * \brief 一个光环单元
*/
struct t_haloInfo
{
    DWORD damage;	    //增加攻
    DWORD maxhp;	    //增肌血
    BYTE taunt;		    //嘲讽
    BYTE charge;	    //冲锋
    BYTE windfury;	    //风怒
    DWORD incrmpcost;	    //耗蓝增加
    DWORD decrmpcost;	    //耗蓝减少
    t_haloInfo()
    {
	memset(this, 0, sizeof(*this));
    }
};

/**
 * \brief 一个效果单元
*/
struct t_EffectUnit
{
    DWORD id;
    BYTE temp;
    t_EffectUnit()
    {
	memset(this, 0, sizeof(*this));
    }
};

struct t_CardPK
{
    BYTE taunt;		    //嘲讽
    BYTE charge;	    //冲锋
    BYTE windfury;	    //风怒
    BYTE sneak;		    //潜行
    BYTE shield;	    //圣盾
    BYTE antimagic;	    //魔免
    BYTE freeze;	    //冻结
    DWORD freeze_round;	    //冻结发生的回合数
    BYTE awake;		    //醒
    DWORD magicID;	    //卡牌携带的法术ID
    DWORD shoutID;	    //战吼ID(skill)
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
    DWORD otherBeHurtID;    //角色受伤(skill)
    ConditionStatus deadStatus;
    DWORD otherDeadID;	    //角色死亡(skill)
    ConditionStatus cureStatus;
    DWORD otherBeCureID;    //角色被治疗(skill)
    DWORD selfUseMagic;	    //己方使用法术牌时(skill)
    DWORD enemyUseMagic;    //敌方使用法术牌时(skill)
    ConditionStatus useAttendStatus;
    DWORD otherUseAttendID; //使用一张随从牌(skill)
    ConditionStatus attendInStatus;
    DWORD otherAttendInID;  //其他随从牌进场(skill)

    DWORD halo_Ctype;	//光环开启条件类型
    DWORD halo_Cid;	//光环开启条件ID
    DWORD haloID;	    //光环ID(skill)

    DWORD attackEndCondition;	//攻击结束条件
    DWORD attackEndID;		//攻击结束(skill)
    t_CardPK()
    {
	memset(this, 0, sizeof(*this));
    }
};
#endif

