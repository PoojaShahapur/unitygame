/**
 * \brief ÂÆö‰πâÁªü‰∏ÄÁî®Êà∑Âπ≥Âè∞ÁôªÈôÜÊúçÂä°Âô®Êåá‰ª§
 */
#ifndef _RoleChangeCommand_h_
#define _RoleChangeCommand_h_
#include "zType.h"
#include "zNullCmd.h"
#pragma pack(1)

namespace Cmd
{
  namespace Rolechange
  {
    const BYTE CMD_LOGIN = 1;
    const BYTE CMD_COMMON = 2;
    const BYTE CMD_CHAT_OVER_ZONE = 3;
    const BYTE CMD_BATTLE = 129;


    //////////////////////////////////////////////////////////////
    /// ÁôªÈôÜRolechangeÊúçÂä°Âô®Êåá‰ª§
    //////////////////////////////////////////////////////////////
    const BYTE PARA_LOGIN = 1;
    struct t_LoginCmd : t_NullCmd
    {
      char strIP[MAX_IP_LENGTH];
      WORD port;

      t_LoginCmd()
        : t_NullCmd(CMD_LOGIN,PARA_LOGIN) {};
    };

    const BYTE PARA_LOGIN_OK = 2;
    struct t_LoginCmd_OK : t_NullCmd
    {
      GameZone_t gameZone;
      char name[MAX_NAMESIZE];
      BYTE netType;

      t_LoginCmd_OK()
        : t_NullCmd(CMD_LOGIN,PARA_LOGIN_OK) {};
    };
    //////////////////////////////////////////////////////////////
    /// ÁôªÈôÜRolechangeÊúçÂä°Âô®Êåá‰ª§
    //////////////////////////////////////////////////////////////

    struct t_CommonCmd : t_NullCmd
    {
	t_CommonCmd(BYTE para) : t_NullCmd(CMD_COMMON, para)
	{}
    };

    const BYTE PARA_FORWARD_MSG = 1;
    struct t_ForwardMsg_CommonCmd : t_CommonCmd
    {
	GameZone_t fromGameZone;
	GameZone_t toGameZone;
	DWORD size;
	BYTE msg[0];
	t_ForwardMsg_CommonCmd():t_CommonCmd(PARA_FORWARD_MSG)
	{}
    };

    struct t_ChatCmd : t_NullCmd
    {
	t_ChatCmd(BYTE para) : t_NullCmd(CMD_CHAT_OVER_ZONE, para)
	{}
    };

    const BYTE PARA_OVER_ZONE_CHAT = 1;
    struct t_OverZone_ChatCmd : t_ChatCmd
    {
	DWORD type;
	DWORD toID;
	DWORD size;
	BYTE msg[0];
	t_OverZone_ChatCmd() : t_ChatCmd(PARA_OVER_ZONE_CHAT)
	{
	    type = 0;
	    toID = 0;
	    size = 0;
	}
    };

    const BYTE PARA_OVER_ZONE_RET_CHAT = 2;
    struct t_OverZoneRet_ChatCmd : t_ChatCmd
    {
	DWORD type;
	DWORD toID;
	BYTE ret;   //0,≤ª‘⁄œﬂ
	char toName[MAX_NAMESIZE];
	t_OverZoneRet_ChatCmd() : t_ChatCmd(PARA_OVER_ZONE_RET_CHAT)
	{
	    type = 0;
	    toID = 0;
	    ret = 0;
	    bzero(toName, sizeof(toName));
	}

    };

    enum
    {
	TYPE_TOZONE,	    //»•’Ω«¯
	TYPE_BACKZONE,	    //ªÿ‘≠«¯
	TYPE_CHANGE_TRAVEL, //¬√”Œ◊™«¯
	TYPE_BACK_CHANGE_TRAVEL,    //ªÿ¬√”Œ«∞µƒ«¯
	TYPE_CHANGE_ZONE,   //”¿æ√◊™«¯

    };

    const BYTE PARA_CHECK_VALID = 1;
    struct t_Check_Valid : t_NullCmd
    {
	GameZone_t fromGameZone;    
	GameZone_t toGameZone;
	DWORD accid;
	DWORD userid;
	DWORD userlevel;
	DWORD checkWriteSize;
	DWORD charbaseSize;
	DWORD checkvalidSize;
	DWORD verifyVersion;
	DWORD objVersion;
	BYTE type;
	t_Check_Valid() : t_NullCmd(CMD_BATTLE, PARA_CHECK_VALID)
	{
	    accid = 0;
	    userid = 0;
	    userlevel = 0;
	    checkWriteSize = 0;
	    charbaseSize = 0;
	    checkvalidSize = 0;
	    verifyVersion = 0;
	    objVersion = 0;
	    type = TYPE_TOZONE;
	}

    };

    enum
    {
	BATTLE_SUCCESS,	    //’Ω«¯—È÷§Õ®π˝
	BATTLE_CLOSE,	    //’Ω«¯Œ¥ø™∑≈
	BATTLE_INVALID,	    //≤ª «’Ω«¯
	BATTLE_USER_REPEAT, //’À∫≈÷ÿ∏¥,’Ω«¯“—”–∏√’À∫≈µƒΩ«…´
	BATTLE_PLAYER_REPEAT,	//Ω«…´“—æ≠‘⁄∏√’Ω«¯,≤ªƒ‹÷ÿ◊™
	BATTLE_LEVEL_INVALID,	//µ»º∂≤ª‘⁄∑∂Œßƒ⁄
	BATTLE_VERSION_INVALID,	//∞Ê±æ≤ª∆•≈‰
	ZONE_CLOSE,	    //∏√«¯πÿ±’,ªÚ’ﬂŒ¥ø™∑≈◊™«¯π¶ƒ‹
	ZONE_LOGIN,	    //∏√’À∫≈“—µ«¬º∏√«¯,≤ª‘ –ÌΩ¯––¥´ÀÕ
	ZONE_SORT_INVALID,  //≈≈√˚π˝∏ﬂ≤ª‘ –Ì¬√”Œ
    };
    const BYTE PARA_RET_CHECK_VALID = 2;
    struct t_Ret_Check_Valid : t_NullCmd
    {
	GameZone_t fromGameZone;    
	GameZone_t toGameZone;
	DWORD accid;
	DWORD userid;
	BYTE state;
	BYTE type;
	t_Ret_Check_Valid() : t_NullCmd(CMD_BATTLE, PARA_RET_CHECK_VALID)
	{
	    accid = 0;
	    userid = 0;
	    state = 0;
	    type = 0;
	}

    };
    const BYTE PARA_SEND_USER_TOZONE = 3;
    struct t_sendUserToZone : t_NullCmd
    {
	GameZone_t fromGameZone;    
	GameZone_t toGameZone;
	DWORD accid;
	DWORD userid;
	BYTE type;
	DWORD secretkey;    //—È÷§√ÿ‘ø
	char data[0];	    //∞¸∫¨“ª∏ˆt_WriteUser_SceneRecordΩ·ππ
	t_sendUserToZone() : t_NullCmd(CMD_BATTLE, PARA_SEND_USER_TOZONE)
	{
	    accid = 0;
	    userid = 0;
	    secretkey = 0;
	}

    };
    enum
    {
	TOZONE_SUCCESS,	    //◊™«¯≥…π¶
	TOZONE_FAIL,	    //◊™«¯ ß∞‹
    };

    const BYTE PARA_RET_SEND_USER_TOZONE = 4;
    struct t_retSendUserToZone : t_NullCmd
    {
	GameZone_t fromGameZone;    
	GameZone_t toGameZone;
	DWORD accid;
	DWORD userid;
	DWORD state;
	BYTE type;
	char name[MAX_NAMESIZE+1];  
	t_retSendUserToZone() : t_NullCmd(CMD_BATTLE, PARA_SEND_USER_TOZONE)
	{
	    accid = 0;
	    userid = 0;
	    state = 0;
	    type = 0;
	    bzero(name, sizeof(name));
	}

    };

    enum
    {
	DIS_RECEIVE,	//æ‹æ¯Ω” ’ ˝æ›
	ENA_RECEIVE,	//ø…“‘Ω” ‹ ˝æ›
    };

    //œÚ∆ΩÃ®»∑»œø…“‘Ω” ‹’Ω«¯ ˝æ›¡À
    const BYTE PARA_CONFIRM_RECEIVE = 5;
    struct t_confirmReceive : t_NullCmd
    {
	GameZone_t gameZone;
	int state;	//0,æ‹æ¯Ω” ‹ 1,ø…“‘Ω” ‹
	t_confirmReceive() :t_NullCmd(CMD_BATTLE, PARA_CONFIRM_RECEIVE)
	{
	    state = 0;
	}
    };

    //Ω«…´◊¥Ã¨ºÏ≤È÷∏¡Ó
    const BYTE PARA_CHECK_ZONE_STATE = 6;
    struct t_checkZoneState : t_NullCmd
    {
	DWORD charid;
	DWORD accid;
	DWORD fromGameID;
	BYTE isForce;	    //1,«ø÷∆Ω‚À¯ 2-ÕÊº“ «∑ÒøÁ∑˛‘⁄œﬂ
	t_checkZoneState() : t_NullCmd(CMD_BATTLE, PARA_CHECK_ZONE_STATE)
	{
	    charid = 0;
	    accid = 0;
	    fromGameID = 0;
	    isForce = 0;
	}
    };

    //Ω«…´◊¥Ã¨ºÏ≤È∑µªÿ÷∏¡Ó
    const BYTE PARA_RET_CHECK_ZONE_STATE = 7;
    struct t_retCheckZoneState : t_NullCmd
    {
	DWORD charid;
	DWORD accid;
	DWORD type;
	DWORD dwTime;
	t_retCheckZoneState() : t_NullCmd(CMD_BATTLE, PARA_RET_CHECK_ZONE_STATE)
	{
	    charid = 0;
	    accid = 0;
	    type = 0;
	    dwTime = 0;
	}
    };

    const BYTE PARA_REQ_ZONE_LIST = 8;
    struct t_Req_ZoneList : t_NullCmd
    {
	GameZone_t GameZone;
	DWORD userid;
	DWORD type;	//0,µÁ–≈«¯ 1,Õ¯Õ®«¯
	t_Req_ZoneList() : t_NullCmd(CMD_BATTLE, PARA_REQ_ZONE_LIST)
	{
	    userid = 0;
	    type = 0;
	}
    };

    struct t_zone_info
    {
	char name[MAX_NAMESIZE];
	DWORD zone_id;
	DWORD level;
	t_zone_info()
	{
	    bzero(name, MAX_NAMESIZE);
	    zone_id = 0;
	    level = 0;
	}
    };
    const BYTE PARA_RTN_ZONE_LIST = 9;
    struct t_Rtn_ZoneList : t_NullCmd
    {
	DWORD userid;
	DWORD type;	//0,µÁ–≈«¯ 1,Õ¯Õ®«¯
	DWORD size;
	t_zone_info data[0];
	t_Rtn_ZoneList() : t_NullCmd(CMD_BATTLE, PARA_RTN_ZONE_LIST)
	{
	    userid = 0;
	    type = 0;
	    size = 0;
	}
    };

    const BYTE PARA_SEND_CHANGE_SORT = 10;
    struct t_sendChangeSort : t_NullCmd
    {
	GameZone_t gameZone;
	int level;	    //∏√«¯µ⁄20√˚µƒµ»º∂
	t_sendChangeSort() : t_NullCmd(CMD_BATTLE, PARA_SEND_CHANGE_SORT)
	{
	    level = 0;
	}
    };

#if 0
    const BYTE PARA_REQ_ZONE_LIST_NEW = 12;
    struct t_Req_ZoneListNew : t_NullCmd
    {
	GameZone_t GameZone;
	DWORD userid;
	DWORD type;	//0,µÁ–≈«¯ 1,Õ¯Õ®«¯
	t_Req_ZoneListNew() : t_NullCmd(CMD_BATTLE, PARA_REQ_ZONE_LIST_NEW)
	{
	    userid = 0;
	    type = 0;
	}
    };

    struct t_zone_info_new
    {
	char name[MAX_NAMESIZE];
	DWORD zone_id;
	DWORD level;
	bool travel_limit;	// «∑Ò»°œ˚øÁ«¯¬√”Œµ»º∂œﬁ÷∆
	t_zone_info_new()
	{
	    bzero(name, MAX_NAMESIZE);
	    zone_id = 0;
	    level = 0;
	    travel_limit = 0;
	}
    };
    const BYTE PARA_RTN_ZONE_LIST_NEW = 13;
    struct t_Rtn_ZoneListNew : t_NullCmd
    {
	DWORD userid;
	DWORD type;	//0,µÁ–≈«¯ 1,Õ¯Õ®«¯
	DWORD size;
	t_zone_info_new data[0];
	t_Rtn_ZoneListNew() : t_NullCmd(CMD_BATTLE, PARA_RTN_ZONE_LIST_NEW)
	{
	    userid = 0;
	    type = 0;
	    size = 0;
	}
    };

    const BYTE PARA_SEND_CHANGE_SORT_NEW = 14;
    struct t_sendChangeSortNew : t_NullCmd
    {
	GameZone_t gameZone;
	int level;	    //∏√«¯µ⁄20√˚µƒµ»º∂
	bool travel_limit;  // «∑Ò»°œ˚øÁ«¯¬√”Œµ»º∂œﬁ÷∆
	t_sendChangeSortNew() : t_NullCmd(CMD_BATTLE, PARA_SEND_CHANGE_SORT_NEW)
	{
	    level = 0;
	    travel_limit = false;
	}
    };
#endif
  };
};

#pragma pack()
#endif

