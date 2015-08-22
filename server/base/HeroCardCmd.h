/*************************************************************************
 Author: wang
 Created Time: 2014��10��22�� ������ 13ʱ21��38��
 File Name: base/HeroCardCmd.h
 Description: 
 ************************************************************************/
#ifndef _HeroCardCmd_h_
#define _HeroCardCmd_h_
#include "zType.h"
#include "Command.h"
#include "CmdType.h"
#include "Card.h"

#pragma pack(1)
namespace Cmd
{
    struct stHeroCardCmd : public stNullUserCmd
    {
	stHeroCardCmd()
	{
	    byCmd = HERO_CARD_USERCMD;
	}
    };
    
    enum
    {
	ACTION_TYPE_NONE = 0,
	ACTION_TYPE_HURT = 1,	//����
	ACTION_TYPE_CURE = 2,	//����
	ACTION_TYPE_DEAD = 3,	//����
	ACTION_TYPE_USEATTEND = 4,	//ʹ�������
	ACTION_TYPE_ATTEND_IN = 5,	//����ϳ�

    };

    struct t_Tujian
    {
	DWORD id;   //baseID
	BYTE num;   //����
	t_Tujian()
	{
	    id = 0;
	    num = 0;
	}
    };

    const BYTE NOFITY_ALL_CARD_TUJIAN_INFO_CMD = 1;
    struct stNotifyAllCardTujianInfoCmd : public stHeroCardCmd
    {
	stNotifyAllCardTujianInfoCmd()
	{
	    byParam = NOFITY_ALL_CARD_TUJIAN_INFO_CMD;
	    count = 0;
	}
	WORD count;
	t_Tujian info[0];
    };

    const BYTE NOFITY_ONE_CARD_TUJIAN_INFO_CMD = 2;
    struct stNotifyOneCardTujianInfoCmd : public stHeroCardCmd
    {
	stNotifyOneCardTujianInfoCmd()
	{
	    byParam = NOFITY_ONE_CARD_TUJIAN_INFO_CMD;
	    id = 0;
	    num = 0;
	}
	DWORD id;
	BYTE num;
    };

    const BYTE RET_GIFTBAG_CARDS_DATA_CMD = 3;
    struct stRetGiftBagCardsDataUserCmd : public stHeroCardCmd
    {
	stRetGiftBagCardsDataUserCmd()
	{
	    byParam = RET_GIFTBAG_CARDS_DATA_CMD;
	}
	DWORD id[5];		    //һ������е�5�ſ�
    };
    
    const BYTE REQ_ALL_CARD_TUJIAN_DATA_CMD = 4;
    struct stReqAllCardTujianDataUserCmd : public stHeroCardCmd
    {
	stReqAllCardTujianDataUserCmd()
	{
	    byParam = REQ_ALL_CARD_TUJIAN_DATA_CMD;
	}
    };


    const BYTE REQ_CARD_GROUP_LIST_INFO_CMD = 5;
    struct stReqCardGroupListInfoUserCmd : public stHeroCardCmd
    {
	stReqCardGroupListInfoUserCmd()
	{
	    byParam = REQ_CARD_GROUP_LIST_INFO_CMD;
	}
    };

    struct t_group_list
    {
	DWORD index;
	DWORD occupation;
	DWORD cardNum;
	char name[MAX_NAMESIZE+1];
	t_group_list()
	{
	    bzero(name, sizeof(name));
	    index = 0;
	    occupation = 0;
	    cardNum = 0;
	}
    };
    const BYTE RET_CARD_GROUP_LIST_INFO_CMD = 6;
    struct stRetCardGroupListInfoUserCmd : public stHeroCardCmd
    {
	stRetCardGroupListInfoUserCmd()
	{
	    byParam = RET_CARD_GROUP_LIST_INFO_CMD;
	    count = 0;
	}
	WORD count;
	t_group_list info[0];
    };

    const BYTE REQ_ONE_CARD_GROUP_INFO_CMD = 7;
    struct stReqOneCardGroupInfoUserCmd : public stHeroCardCmd
    {
	stReqOneCardGroupInfoUserCmd()
	{
	    byParam = REQ_ONE_CARD_GROUP_INFO_CMD;
	    index = 0;
	}
	DWORD index;
    };

    const BYTE RET_ONE_CARD_GROUP_INFO_CMD = 8;
    struct stRetOneCardGroupInfoUserCmd : public stHeroCardCmd
    {
	stRetOneCardGroupInfoUserCmd()
	{
	    byParam = RET_ONE_CARD_GROUP_INFO_CMD;
	    index = 0;
	    count = 0;
	}
	DWORD index;
	WORD count;
	DWORD id[0];
    };

    const BYTE REQ_CREATE_ONE_CARD_GROUP_CMD = 9;
    struct stReqCreateOneCardGroupUserCmd : public stHeroCardCmd
    {
	stReqCreateOneCardGroupUserCmd()
	{
	    byParam = REQ_CREATE_ONE_CARD_GROUP_CMD;
	    occupation = 0;
	}
	DWORD occupation;	//ְҵ
    };

    const BYTE REQ_SAVE_ONE_CARD_GROUP_CMD = 10;
    struct stReqSaveOneCardGroupUserCmd : public stHeroCardCmd
    {
	stReqSaveOneCardGroupUserCmd()
	{
	    byParam = REQ_SAVE_ONE_CARD_GROUP_CMD;
	    index = 0;
	    count = 0;
	}
	DWORD index;	
	WORD count;
	DWORD id[0];
    };
   
    const BYTE RET_CREATE_ONE_CARD_GROUP_CMD = 11;
    struct stRetCreateOneCardGroupUserCmd : public stHeroCardCmd
    {
	stRetCreateOneCardGroupUserCmd()
	{
	    byParam = RET_CREATE_ONE_CARD_GROUP_CMD;
	    occupation = 0;
	    index = 0;
	    bzero(name, sizeof(name));
	}
	DWORD occupation;	//ְҵ
	DWORD index;
	char name[MAX_NAMESIZE+1];
    };
    
    const BYTE REQ_DELETE_ONE_CARD_GROUP_CMD = 12;
    struct stReqDeleteOneCardGroupUserCmd : public stHeroCardCmd
    {
	stReqDeleteOneCardGroupUserCmd()
	{
	    byParam = REQ_DELETE_ONE_CARD_GROUP_CMD;
	    index = 0;
	}
	DWORD index;	
    };

    const BYTE RET_DELETE_ONE_CARD_GROUP_CMD = 13;
    struct stRetDeleteOneCardGroupUserCmd : public stHeroCardCmd
    {
	stRetDeleteOneCardGroupUserCmd()
	{
	    byParam = RET_DELETE_ONE_CARD_GROUP_CMD;
	    index = 0;
	    success = 0;
	}
	DWORD index;	
	BYTE success;	    //1�ɹ� 0ʧ��
    };

    const BYTE RET_SAVE_ONE_CARD_GROUP_CMD = 14;
    struct stRetSaveOneCardGroupUserCmd : public stHeroCardCmd
    {
	stRetSaveOneCardGroupUserCmd()
	{
	    byParam = RET_SAVE_ONE_CARD_GROUP_CMD;
	    index = 0;
	    success = 0;
	}
	DWORD index;	
	BYTE success;	    //1�ɹ� 0ʧ��
    };

    const BYTE REQ_ALL_HERO_INFO_CMD = 15;
    struct stReqAllHeroInfoUserCmd : public stHeroCardCmd
    {
	stReqAllHeroInfoUserCmd()
	{
	    byParam = REQ_ALL_HERO_INFO_CMD;
	}
    };

    struct t_hero
    {
	WORD occupation;
	WORD level;		//Ӣ�۵ȼ�
	QWORD exp;		//Ӣ�۾���
	BYTE isActive;		//Ӣ���Ƿ��Ѽ���
	BYTE isGold;		//Ӣ���Ƿ��ǽ�ɫ
	t_hero()
	{
	    bzero(this, sizeof(*this));
	}
    };
    const BYTE RET_ALL_HERO_INFO_CMD = 16;
    struct stRetAllHeroInfoUserCmd : public stHeroCardCmd
    {
	stRetAllHeroInfoUserCmd()
	{
	    byParam = RET_ALL_HERO_INFO_CMD;
	    count = 0;
	}
	WORD count;
	t_hero info[0];
    };
    
    const BYTE RET_ONE_HERO_INFO_CMD = 17;
    struct stRetOneHeroInfoUserCmd : public stHeroCardCmd
    {
	stRetOneHeroInfoUserCmd()
	{
	    byParam = RET_ONE_HERO_INFO_CMD;
	}
	t_hero info;
    };

    const BYTE REQ_HERO_FIGHT_MATCH_CMD = 18;
    struct stReqHeroFightMatchUserCmd : public stHeroCardCmd
    {
	stReqHeroFightMatchUserCmd()
	{
	    byParam = REQ_HERO_FIGHT_MATCH_CMD;
	    index = 0;
	    fightType = 0;
	    cancel = 0;
	}
	DWORD index;	    //��������
	BYTE fightType;	    //��ս����
	BYTE cancel;	    //ȡ��
    };

    const BYTE RET_HERO_FIGHT_MATCH_CMD = 19;
    struct stRetHeroFightMatchUserCmd : public stHeroCardCmd
    {
	stRetHeroFightMatchUserCmd()
	{
	    byParam = RET_HERO_FIGHT_MATCH_CMD;
	    fightType = 0;
	    success = 0;
	}
	BYTE fightType;	    //��ս����
	BYTE success;	   
    };

    const BYTE RET_LEFT_CARDLIB_NUM_CMD = 20;
    struct stRetLeftCardLibNumUserCmd : public stHeroCardCmd 
    {
	stRetLeftCardLibNumUserCmd()
	{
	    byParam = RET_LEFT_CARDLIB_NUM_CMD;
	    selfNum = 0;
	    otherNum = 0;
	}
	DWORD selfNum;	    //�Լ�ʣ��	
	DWORD otherNum;	    //�Է�ʣ��
    };
    
    struct t_MagicPoint
    {
	DWORD mp;
	DWORD maxmp;
	DWORD forbid;
	t_MagicPoint()
	{
	    mp = 0;
	    maxmp = 0;
	    forbid = 0;
	}
    };
    const BYTE RET_MAGIC_POINT_INFO_CMD = 21;
    struct stRetMagicPointInfoUserCmd : public stHeroCardCmd
    {
	stRetMagicPointInfoUserCmd()
	{
	    byParam = RET_MAGIC_POINT_INFO_CMD;
	}
	t_MagicPoint self;
	t_MagicPoint other;
    };

    const BYTE REQ_END_MY_ROUND_CMD = 22;
    struct stReqEndMyRoundUserCmd : public stHeroCardCmd
    {
	stReqEndMyRoundUserCmd()
	{
	    byParam = REQ_END_MY_ROUND_CMD;
	}
    };

    const BYTE RET_REFRESH_BATTLE_STATE_CMD = 23;
    struct stRetRefreshBattleStateUserCmd : public stHeroCardCmd
    {
	stRetRefreshBattleStateUserCmd()
	{
	    byParam = RET_REFRESH_BATTLE_STATE_CMD;
	    state = 0;
	}
	BYTE state;
    };

    const BYTE RET_REFRESH_BATTLE_PRIVILEGE_CMD = 24;
    struct stRetRefreshBattlePrivilegeUserCmd : public stHeroCardCmd
    {
	stRetRefreshBattlePrivilegeUserCmd()
	{
	    byParam = RET_REFRESH_BATTLE_PRIVILEGE_CMD;
	    priv = 0;
	}
	BYTE priv;	//1,�Լ� 2,�Է�
    };

    const BYTE REQ_GIVEUP_ONE_BATTLE_CMD = 25;
    struct stReqGiveUpOneBattleUserCmd : public stHeroCardCmd
    {
	stReqGiveUpOneBattleUserCmd()
	{
	    byParam = REQ_GIVEUP_ONE_BATTLE_CMD;
	}
    };

    const BYTE ADD_BATTLE_CARD_PROPERTY_CMD = 26;
    struct stAddBattleCardPropertyUserCmd : public stHeroCardCmd
    {
	stAddBattleCardPropertyUserCmd()
	{
	    byParam = ADD_BATTLE_CARD_PROPERTY_CMD;
	    slot = 0;
	    who = 0;
	    byActionType = 0;
	}
	BYTE slot;	    //�ĸ���
	BYTE who;	    //1,�Լ� 2,�Է�
	BYTE byActionType;  //1,��� 2,ˢ��
	t_Card object;	  
    };

    const BYTE NOTIFY_FIGHT_ENEMY_INFO_CMD = 27;
    struct stNotifyFightEnemyInfoUserCmd : public stHeroCardCmd
    {
	stNotifyFightEnemyInfoUserCmd()
	{
	    byParam = NOTIFY_FIGHT_ENEMY_INFO_CMD;
	    bzero(name, sizeof(name));
	    occupation = 0;
	}
	DWORD occupation;	    //ְҵ
	char name[MAX_NAMESIZE+1];  //����
    };

    const BYTE REQ_FIGHT_PREPARE_OVER_CMD = 28;
    struct stReqFightPrepareOverUserCmd : public stHeroCardCmd
    {
	stReqFightPrepareOverUserCmd()
	{
	    byParam = REQ_FIGHT_PREPARE_OVER_CMD;
	    change = 0;
	}
	BYTE change;	//�ӵ�λ����λ,ĳλ��1��ʾ�滻��λ�õ���
    };
	
    const BYTE RENAME_CARD_GROUP_USERCMD_PARAMETER = 29;
    struct stRenameCardGroupUserCmd : public stHeroCardCmd
    {
	stRenameCardGroupUserCmd()
	{
	    byParam = RENAME_CARD_GROUP_USERCMD_PARAMETER;
	    index = 0;
	    memset(name, 0, (MAX_NAMESIZE+1)*sizeof(char));
	}
	DWORD index;
	char name[MAX_NAMESIZE+1];
    };


    const BYTE RET_FIRST_HAND_CARD_CMD = 30;
    struct stRetFirstHandCardUserCmd : public stHeroCardCmd
    {
	stRetFirstHandCardUserCmd()
	{
	    byParam = RET_FIRST_HAND_CARD_CMD;
	    upperHand = 0;
	    bzero(id, sizeof(id));
	}
	BYTE upperHand;	    //1,���� 0,����
	DWORD id[4];
    };

    const BYTE RET_MOVE_CARD_USERCMD_PARAMETER = 31;
    struct stRetMoveGameCardUserCmd : public stHeroCardCmd
    {
	stRetMoveGameCardUserCmd()
	{
	    byParam = RET_MOVE_CARD_USERCMD_PARAMETER;
	}
	DWORD qwThisID;		    //����thisID
	stObjectLocation  dst;	    //Ŀ��λ����Ϣ
	BYTE success;		    //1,�ɹ� 0,ʧ��
    };

    const BYTE RET_NOTIFY_HAND_IS_FULL_CMD = 32;
    struct stRetNotifyHandIsFullUserCmd : public stHeroCardCmd
    {
	stRetNotifyHandIsFullUserCmd()
	{
	    byParam = RET_NOTIFY_HAND_IS_FULL_CMD;
	    id = 0;
	    who = 0;
	}
	DWORD id;   //���Ʊ�id
	BYTE who;   //1,�Լ� 2,�Է�
    };

    const BYTE ADD_ENEMY_HAND_CARD_PROPERTY_CMD = 33;
    struct stAddEnemyHandCardPropertyUserCmd : public stHeroCardCmd
    {
	stAddEnemyHandCardPropertyUserCmd()
	{
	    byParam = ADD_ENEMY_HAND_CARD_PROPERTY_CMD;
	}
    };

    const BYTE RET_NOTIFY_UNFINISHED_GAME_CMD = 34;
    struct stRetNotifyUnfinishedGameUserCmd : public stHeroCardCmd
    {
	stRetNotifyUnfinishedGameUserCmd()
	{
	    byParam = RET_NOTIFY_UNFINISHED_GAME_CMD;
	}
    };

    const BYTE REQ_ENTER_UNFINISHED_GAME_CMD = 35;
    struct stReqEnterUnfinishedGameUserCmd : public stHeroCardCmd
    {
	stReqEnterUnfinishedGameUserCmd()
	{
	    byParam = REQ_ENTER_UNFINISHED_GAME_CMD;
	}
    };

    const BYTE ADD_BATTLE_CARD_LIST_PROPERTY_CMD = 36;
    struct stAddBattleCardListPropertyUserCmd : public stHeroCardCmd
    {
	stAddBattleCardListPropertyUserCmd()
	{
	    byParam = ADD_BATTLE_CARD_LIST_PROPERTY_CMD;
	    count = 0;
	}
	WORD count;
	struct 
	{
	    BYTE who;
	    t_Card object;
	}list[0];
    };

    const BYTE RET_ENEMY_HAND_CARD_NUM_CMD = 37;
    struct stRetEnemyHandCardNumUserCmd : public stHeroCardCmd
    {
	stRetEnemyHandCardNumUserCmd()
	{
	    byParam = RET_ENEMY_HAND_CARD_NUM_CMD;
	    count = 0;
	}
	WORD count;
    };

    const BYTE REQ_CARD_MAGIC_USERCMD_PARA = 38;
    struct stCardAttackMagicUserCmd : public stHeroCardCmd
    {
	stCardAttackMagicUserCmd()
	{
	    byParam = REQ_CARD_MAGIC_USERCMD_PARA;
	    dwAttThisID = 0;
	    dwDefThisID = 0;
	    dwMagicType = 0;
	    flag = 0;
	}

	DWORD dwAttThisID;      
	DWORD dwDefThisID;   
	DWORD dwMagicType;	//����ID
	BYTE flag;	//1,�ֶ��ͷ�;0,������Ч��
    };

    const BYTE RET_REMOVE_BATTLE_CARD_USERCMD = 39;
    struct stRetRemoveBattleCardUserCmd : public stHeroCardCmd
    {
	stRetRemoveBattleCardUserCmd()
	{
	    byParam = RET_REMOVE_BATTLE_CARD_USERCMD;
	    dwThisID = 0;
	    opType = 0;
	}
	DWORD dwThisID;
	BYTE opType;
    };

    const BYTE DEL_ENEMY_HAND_CARD_PROPERTY_CMD = 40;
    struct stDelEnemyHandCardPropertyUserCmd : public stHeroCardCmd
    {
	stDelEnemyHandCardPropertyUserCmd()
	{
	    byParam = DEL_ENEMY_HAND_CARD_PROPERTY_CMD;
	    index = 0;
	}
	BYTE index;	//0----9[���Ƶ�λ��]
    };

    const BYTE RET_REFRESH_CARD_ALL_STATE_CMD = 41;
    struct stRetRefreshCardAllStateUserCmd: public stHeroCardCmd
    {
	stRetRefreshCardAllStateUserCmd()
	{
	    byParam = RET_REFRESH_CARD_ALL_STATE_CMD;
	    dwThisID = 0;
	    who = 0;
	    bzero(state, sizeof(state));
	}
	DWORD dwThisID;
	BYTE who;	//1,�Լ� 2,�Է�
	BYTE state[(CARD_STATE_MAX + 7) / 8];
    };

    const BYTE RET_CLEAR_CARD_ONE_STATE_CMD = 42;
    struct stRetClearCardOneStateUserCmd : public stHeroCardCmd
    {
	stRetClearCardOneStateUserCmd()
	{
	    byParam = RET_CLEAR_CARD_ONE_STATE_CMD;
	    dwThisID  = 0;
	    who = 0;
	    stateNum = 0;
	}
	DWORD dwThisID;
	BYTE who;   //1,�Լ� 2,�Է�
	BYTE stateNum;	    //״̬��ö��
    };

    const BYTE RET_SET_CARD_ONE_STATE_CMD = 43;
    struct stRetSetCardOneStateUserCmd : public stHeroCardCmd
    {
	stRetSetCardOneStateUserCmd()
	{
	    byParam = RET_SET_CARD_ONE_STATE_CMD;
	    dwThisID  = 0;
	    who = 0;
	    stateNum = 0;
	}
	DWORD dwThisID;
	BYTE who;   //1,�Լ� 2,�Է�
	BYTE stateNum;	    //״̬��ö��
    };

    const BYTE RET_BATTLE_GAME_RESULT_CMD = 44;
    struct stRetBattleGameResultUserCmd : public stHeroCardCmd
    {
	stRetBattleGameResultUserCmd()
	{
	    byParam = RET_BATTLE_GAME_RESULT_CMD;
	    win = 0;
	}
	BYTE win;	    //1Ӯ, 0��
    };

    const BYTE RET_HERO_INTO_BATTLE_SCENE_CMD = 45;
    struct stRetHeroIntoBattleSceneUserCmd : public stHeroCardCmd
    {
	stRetHeroIntoBattleSceneUserCmd()
	{
	    byParam = RET_HERO_INTO_BATTLE_SCENE_CMD;
	    sceneNumber = 0;
	}
	DWORD sceneNumber;  //�������
    };

    const BYTE RET_CARD_ATTACK_FAIL_USERCMD_PARA = 46;
    struct stRetCardAttackFailUserCmd : public stHeroCardCmd
    {
	stRetCardAttackFailUserCmd()
	{
	    byParam = RET_CARD_ATTACK_FAIL_USERCMD_PARA;
	    dwAttThisID = 0;
	}
	DWORD dwAttThisID;      
    };
    
    //C-->Sս��(���ƶ���Ϣ�ϲ�)
    const BYTE REQ_CARD_MOVE_AND_MAGIC_USERCMD_PARA = 47;
    struct stCardMoveAndAttackMagicUserCmd : public stHeroCardCmd
    {
	stCardMoveAndAttackMagicUserCmd()
	{
	    byParam = REQ_CARD_MOVE_AND_MAGIC_USERCMD_PARA;
	    dwAttThisID = 0;
	    dwDefThisID = 0;
	    dwMagicType = 0;
	}

	DWORD dwAttThisID;      
	DWORD dwDefThisID;   
	DWORD dwMagicType;  //����ID
	stObjectLocation  dst;      //�ƶ�Ŀ��λ����Ϣ
    };

    const BYTE RET_BATTLE_HISTORY_INFO_CMD = 48;
    struct stRetBattleHistoryInfoUserCmd : public stHeroCardCmd
    {
	stRetBattleHistoryInfoUserCmd()
	{
	    byParam = RET_BATTLE_HISTORY_INFO_CMD;
	    opType = 0;
	    count = 0;
	}
	t_Card maincard;
	BYTE opType;	
	WORD count;
	t_Card othercard[0];
    };

    const BYTE NOTIFY_BATTLE_CARD_PROPERTY_CMD = 49;
    struct stNotifyBattleCardPropertyUserCmd : public stHeroCardCmd
    {
	stNotifyBattleCardPropertyUserCmd()
	{
	    byParam = NOTIFY_BATTLE_CARD_PROPERTY_CMD;
	    count = 0;
	    dwMagicType = 0;
	    type = 0;
	}
	DWORD dwMagicType;
	BYTE type;	//1,�ٻ�;2,����
	WORD count;
	t_Card A_object;	  
	t_Card defList[0];
    };

    const BYTE NOTIFY_BATTLE_FLOW_START_CMD = 50;
    struct stNotifyBattleFlowStartUserCmd : public stHeroCardCmd
    {
	stNotifyBattleFlowStartUserCmd()
	{
	    byParam = NOTIFY_BATTLE_FLOW_START_CMD;
	}
    };

    const BYTE NOTIFY_BATTLE_FLOW_END_CMD = 51;
    struct stNotifyBattleFlowEndUserCmd : public stHeroCardCmd
    {
	stNotifyBattleFlowEndUserCmd()
	{
	    byParam = NOTIFY_BATTLE_FLOW_END_CMD;
	}
    };

    const BYTE NOTIFY_RESET_ATTACKTIMES_CMD = 52;
    struct stNotifyResetAttackTimesUserCmd : public stHeroCardCmd
    {
	stNotifyResetAttackTimesUserCmd()
	{
	    byParam = NOTIFY_RESET_ATTACKTIMES_CMD;
	}
    };

    const BYTE NOTIFY_OUT_CARD_INFO_CMD = 53;
    struct stNotifyOutCardInfoUserCmd : public stHeroCardCmd
    {
	stNotifyOutCardInfoUserCmd()
	{
	    byParam = NOTIFY_OUT_CARD_INFO_CMD;
	    cardID = 0;
	}
	DWORD cardID;	//����ID
    };
}
#pragma pack()
#endif
