/**
 * \brief 定义统一用户平台登陆服务器指令
 */
#ifndef _BroadCastCommand_
#define _BroadCastCommand_
#include "zType.h"
#include "zNullCmd.h"
#pragma pack(1)

namespace Cmd
{
  namespace BroadCast
  {
    const BYTE CMD_LOGIN = 1;
    const BYTE CMD_BROAD = 2;
    const BYTE CMD_BROADDATA = 3;


    //////////////////////////////////////////////////////////////
    /// 登陆Cmd服务器指令
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
    /// 登陆Cmd服务器指令
    //////////////////////////////////////////////////////////////


    //////////////////////////////////////////////////////////////
    /// 定义网关信息相关指令
    //////////////////////////////////////////////////////////////
    const BYTE PARA_BROADMESSAGE = 1;
    struct t_BroadcastMessage : t_NullCmd
    {
      GameZone_t srcZone;
      GameZone_t destZone;
      DWORD rTimestamp;
      DWORD dataLen;
      char data[0];
      t_BroadcastMessage()
        : t_NullCmd(CMD_BROAD, PARA_BROADMESSAGE) {}
    };

    //////////////////////////////////////////////////////////////
    /// 定义网关信息相关指令
    //////////////////////////////////////////////////////////////
    
    const BYTE PARA_BROAD_CONSUME = 1;
    struct t_BroadcastConsume : t_NullCmd
    {
	char gamename[33];
	char zonename[33];
	DWORD accid;
	char rolename[33];
	DWORD point;
	QWORD money;
	t_BroadcastConsume() : t_NullCmd(CMD_BROADDATA, PARA_BROAD_CONSUME)
	{
	    bzero(gamename, sizeof(gamename));
	    bzero(zonename, sizeof(zonename));
	    accid = 0;
	    bzero(zonename, sizeof(zonename));
	    point = 0;
	    money = 0;
	}
    };

  };
};

#pragma pack()
#endif

