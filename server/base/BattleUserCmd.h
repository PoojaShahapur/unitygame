/*************************************************************************
 Author: wang
 Created Time: 2015年02月13日 星期五 14时04分02秒
 File Name: base/BattleUserCmd.h
 Description: 
 ************************************************************************/
#ifndef _BattleUserCmd_h_
#define _BattleUserCmd_h_
#include "zType.h"
#include "Command.h"
#include "CmdType.h"

#pragma pack(1)
namespace Cmd
{
    struct stBattleUserCmd : public stNullUserCmd
    {
	stBattleUserCmd()
	{
	    byCmd = BATTLE_USERCMD;
	}
    };

    const BYTE BATTLE_STATE_PARA = 1;	//客户端收到该消息 表示进入的是战区
    struct stBattleStateUserCmd : public stBattleUserCmd
    {
	stBattleStateUserCmd()
	{
	    byParam = BATTLE_STATE_PARA;
	}
    };

    const BYTE REQ_ENTER_BATTLE_PARA = 2;
    struct stReqEnterBattleUserCmd : public stBattleUserCmd
    {
	stReqEnterBattleUserCmd()
	{
	    byParam = REQ_ENTER_BATTLE_PARA;
	    zone = 0;
	}
	DWORD zone;
    };

    const BYTE RTN_ENTER_BATTLE_PARA = 3;
    struct stRtnEnterBattleUserCmd : public stBattleUserCmd
    {
	stRtnEnterBattleUserCmd()
	{
	    byParam = RTN_ENTER_BATTLE_PARA;
	}
    };

    const BYTE REQ_RETURN_SOURCE_PARA = 4;
    struct stReqReturnSourceUserCmd : public stBattleUserCmd
    {
	stReqReturnSourceUserCmd()
	{
	    byParam = REQ_RETURN_SOURCE_PARA;
	}
    };


    enum
    {
	TO_BATTLE_ZONE,	    //去战区
	RETURN_SOURCE_ZONE,	    //回原区
	CHANGE_TRAVEL, //旅游转区
	BACK_CHANGE_TRAVEL,    //回旅游前的区
	FOREVER_CHANGE_ZONE,   //永久转区

    };

    const BYTE CHANGE_ZONE_VERIFY_PARA = 5;
    struct stChangeZoneVerify : public stBattleUserCmd
    {
	stChangeZoneVerify()
	{
	    byParam = CHANGE_ZONE_VERIFY_PARA;
	    zone_id = 0;
	    secretkey = 0;
	    accid = 0;
	    type = TO_BATTLE_ZONE;
	}
	DWORD zone_id;
	DWORD secretkey;
	DWORD accid;
	BYTE type;
    };

    const BYTE REQ_ZONE_LIST = 6;
    struct stReqZoneList : public stBattleUserCmd
    {
	stReqZoneList()
	{
	    byParam = REQ_ZONE_LIST;
	    type = 0;
	}
	DWORD type; //0,电信 1,网通
    };

    struct zone_info
    {
	char name[MAX_NAMESIZE];
	DWORD zone_id;
	DWORD zone_level;
	zone_info()
	{
	    bzero(name, sizeof(name));
	    zone_id = 0;
	    zone_level = 0;
	}
    };

    const BYTE RTN_ZONE_LIST = 7;
    struct stRtnZoneList : public stBattleUserCmd
    {
	stRtnZoneList()
	{
	    byParam = RTN_ZONE_LIST;
	    dwSize = 0;
	    type = 0;
	}
	DWORD dwSize;	//可跳转的数量
	DWORD type;
	zone_info data[0];
    };

    enum
    {
	CHANGE_ZONE,	//请求永久转区
	CHANGE_ZONE_TRAVEL, //请求转区旅游
    };

    const BYTE REQ_CHANGE_ZONE = 8;
    struct stReqChangeZoneUserCmd : public stBattleUserCmd
    {
	stReqChangeZoneUserCmd()
	{
	    byParam = REQ_CHANGE_ZONE;
	    zone = 0;
	    type = 0;
	}
	DWORD zone;
	WORD type;  //转区类型
    };
}
#pragma pack()
#endif

