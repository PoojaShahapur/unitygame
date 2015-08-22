/*************************************************************************
 Author: wang
 Created Time: 2014��12��09�� ���ڶ� 14ʱ15��48��
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
    CARD_STATE_NONE    = 0,    //��״̬
    CARD_STATE_TAUNT    = 1,    //����
    CARD_STATE_CHARGE   = 2,    //���
    CARD_STATE_WINDFURY = 3,    //��ŭ
    CARD_STATE_SNEAK    = 4,    //����
    CARD_STATE_SHIED    = 5,    //ʥ��
    CARD_STATE_SLEEP    = 6,    //˯
    CARD_STATE_FREEZE   = 7,    //����
    CARD_STATE_BECURE	= 8,	//��Ч��
    CARD_STATE_BEHURT	= 9,	//��Ч��
    CARD_STATE_DEADLAN	= 10,	//��Ч��
    CARD_STATE_SRSTART	= 11,	//��Ч��
    CARD_STATE_SREND	= 12,	//��Ч��
    CARD_STATE_ERSTART	= 13,	//��Ч��
    CARD_STATE_EREND	= 14,	//��Ч��
    CARD_STATE_SUSEMA	= 15,	//��Ч��
    CARD_STATE_EUSEMA	= 16,	//��Ч��
    CARD_STATE_ASTART	= 17,	//��Ч��
    CARD_STATE_BASTART	= 18,	//��Ч��
    CARD_STATE_DRAW	= 19,	//��Ч��
    CARD_STATE_DRAWED	= 20,	//��Ч��
    CARD_STATE_OHURT	= 21,	//��Ч��
    CARD_STATE_OCURE	= 22,	//��Ч��
    CARD_STATE_ODEAD	= 23,	//��Ч��
    CARD_STATE_SUSEA	= 24,	//��Ч��
    CARD_STATE_ATTENDIN	= 25,	//��Ч��
    CARD_STATE_AEND	= 26,	//��Ч��
    CARD_STATE_HALO	= 27,	//��Ч��

    CARD_STATE_MAX,

};

struct t_Card
{
	DWORD qwThisID;		    //��ƷΨһid
	DWORD dwObjectID;		    //��Ʒ���еı��
	stObjectLocation pos;	// λ��
	DWORD mpcost;		    //����
	DWORD damage;		    //������
	DWORD hp;		    //Ѫ��
	DWORD maxhp;		    //Ѫ������
	DWORD dur;		    //�;ö�
	
	BYTE magicDamAdd;	//�����˺�����(X)
	BYTE overload;		//����(num)
	
	DWORD armor;		//����ֵ
	BYTE attackTimes;	    //�غϹ�������
	BYTE equipOpen;		//����״̬(1����,0�ر�)
	BYTE side;		//1:player1; 2:player2
	DWORD popHpValue;		//ð��������(��Ѫ)
	DWORD popDamValue;		//ð��������(����)

	BYTE state[(CARD_STATE_MAX + 7) / 8];
	t_Card()
	{
	    memset(this, 0, sizeof(*this));
	}
};
#pragma pack()

/**
 * \brief һ���⻷��Ԫ
*/
struct t_haloInfo
{
    DWORD damage;	    //���ӹ�
    DWORD maxhp;	    //����Ѫ
    BYTE taunt;		    //����
    BYTE charge;	    //���
    BYTE windfury;	    //��ŭ
    DWORD incrmpcost;	    //��������
    DWORD decrmpcost;	    //��������
    t_haloInfo()
    {
	memset(this, 0, sizeof(*this));
    }
};

/**
 * \brief һ��Ч����Ԫ
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
    BYTE taunt;		    //����
    BYTE charge;	    //���
    BYTE windfury;	    //��ŭ
    BYTE sneak;		    //Ǳ��
    BYTE shield;	    //ʥ��
    BYTE antimagic;	    //ħ��
    BYTE freeze;	    //����
    DWORD freeze_round;	    //���ᷢ���Ļغ���
    BYTE awake;		    //��
    DWORD magicID;	    //����Я���ķ���ID
    DWORD shoutID;	    //ս��ID(skill)
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
    DWORD otherBeHurtID;    //��ɫ����(skill)
    ConditionStatus deadStatus;
    DWORD otherDeadID;	    //��ɫ����(skill)
    ConditionStatus cureStatus;
    DWORD otherBeCureID;    //��ɫ������(skill)
    DWORD selfUseMagic;	    //����ʹ�÷�����ʱ(skill)
    DWORD enemyUseMagic;    //�з�ʹ�÷�����ʱ(skill)
    ConditionStatus useAttendStatus;
    DWORD otherUseAttendID; //ʹ��һ�������(skill)
    ConditionStatus attendInStatus;
    DWORD otherAttendInID;  //��������ƽ���(skill)

    DWORD halo_Ctype;	//�⻷������������
    DWORD halo_Cid;	//�⻷��������ID
    DWORD haloID;	    //�⻷ID(skill)

    DWORD attackEndCondition;	//������������
    DWORD attackEndID;		//��������(skill)
    t_CardPK()
    {
	memset(this, 0, sizeof(*this));
    }
};
#endif

