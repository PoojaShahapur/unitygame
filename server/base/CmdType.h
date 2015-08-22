#ifndef _CMD_TYPE_H_
#define _CMD_TYPE_H_

#include "zType.h"

namespace Cmd{

//BEGIN_ONE_CMD

/// ��ָ��
const BYTE NULL_USERCMD      = 0;
/// ��½ָ��
const BYTE LOGON_USERCMD    = 104;
/// ʱ��ָ��
const BYTE TIME_USERCMD      = 2;
/// ����ָ��
const BYTE DATA_USERCMD      = 3;
/// ����ָ��
const BYTE PROPERTY_USERCMD    = 4;
/// ��ͼָ��
const BYTE MAPSCREEN_USERCMD    = 5;
/// �ƶ�ָ��
const BYTE MOVE_USERCMD      = 6;
/// ����ָ��
const BYTE BUILD_USERCMD    = 8;
/// ����ָ��
const BYTE MAKEOBJECT_USERCMD    = 10;
/// ����ָ��
const BYTE RELIVE_USERCMD    = 12;
/// ����ָ��
const BYTE CHAT_USERCMD      = 14;
/// �뿪ָ��
const BYTE LEAVEONLINE_USERCMD    = 15;
/// ����ָ��
const BYTE TRADE_USERCMD    = 17;
/// ħ��ָ��
const BYTE MAGIC_USERCMD    = 18;
/// ���ָ��
const BYTE UNION_USERCMD    = 21;
/// ����ָ��
const BYTE COUNTRY_USERCMD    = 22;
/// ����ָ��
const BYTE TASK_USERCMD      = 23;
/// ѡ��ָ��
const BYTE SELECT_USERCMD    = 24;
//  ����ϵָ��
const BYTE RELATION_USERCMD        = 25;
//  ���ɹ�ϵָ��
const BYTE SCHOOL_USERCMD    = 26;
//  �����ϵָ��
const BYTE SEPT_USERCMD      = 27;
// ս��ָ��
const BYTE DARE_USERCMD                 = 28;
// ����ָ��
const BYTE PET_USERCMD                  = 29;
// ��ȡ�������б�
const BYTE PING_USERCMD      = 30;
// ���ָ��
const BYTE GOLD_USERCMD      = 31;
// ����ָ��
const BYTE QUIZ_USERCMD      = 32;
// NPC����սָ��
const BYTE NPCDARE_USERCMD    = 33;
// ��GM���߽�����ָ��
const BYTE GMTOOL_USERCMD    = 34;
// �ʼ�ָ��
const BYTE MAIL_USERCMD      = 35;
// ����ָ��
const BYTE AUCTION_USERCMD    = 36;
// ��ͨ����ָ��
const BYTE CARTOON_USERCMD    = 37;
// ��Ʊָ��
const BYTE STOCK_SCENE_USERCMD    = 38;
const BYTE STOCK_BILL_USERCMD    = 39;
// ͶƱָ��
const BYTE VOTE_USERCMD      = 40;
// ����ָ��
const BYTE ARMY_USERCMD      = 41;
// ��������ָ��
const BYTE GEM_USERCMD      = 42;
// ����ϵͳָ��
const BYTE PRISON_USERCMD    = 43;
// ���ָ��
const BYTE GIFT_USERCMD      = 44;
// ����ͬ��ָ��
const BYTE ALLY_USERCMD      = 45;
// С��Ϸָ��
const BYTE MINIGAME_USERCMD    = 46;
// �Ƽ���ϵͳָ��
const BYTE RECOMMEND_USERCMD  = 47;
// �Ʋ�����ϵͳָ��
const BYTE SAFETY_USERCMD    = 48;
//����ָ��
const BYTE SAFETY_COWBOX = 49;

//ת��ָ��

const BYTE TURN_USERCMD = 50;

const BYTE HOTSPRING_USERCMD = 51;

const BYTE REMAKEOBJECT_USERCMD	= 52;

//ѵ��ָ�� [sky]
const BYTE HORSETRAINING_USERCMD	= 53;

//����ս��ָ��
const BYTE BATTLE_USERCMD	= 54;

// sky ս��-����-������ָ��
const BYTE ARENA_USERCMD	= 55;

//Ӣ������ָ��
const BYTE HEROLIST_USERCMD		= 56;

//¯ʯ��˵�����淨
const BYTE HERO_CARD_USERCMD	= 162;
}
#endif


