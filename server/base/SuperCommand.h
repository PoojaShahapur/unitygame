
/**
 * \brief 定义管理服务器的指令
 */
#ifndef _SuperCommand_h_
#define _SuperCommand_h_
#include <string.h> //for strncpy()
#include "zType.h"
#include "zNullCmd.h"

#pragma pack(1)

namespace Cmd
{

  namespace Super
  {
    const BYTE CMD_STARTUP  = 1;
    const BYTE CMD_BILL    = 3;
    const BYTE CMD_GATEWAY  = 4;
    //const BYTE CMD_GMTOOL  = 5;
    const BYTE CMD_SESSION  = 5;
    const BYTE CMD_SCENE  = 7;
    const BYTE CMD_COUNTRYONLINE  = 166;


    //////////////////////////////////////////////////////////////
    // 定义启动相关指令
    //////////////////////////////////////////////////////////////
    const BYTE PARA_STARTUP_REQUEST = 1;
    struct t_Startup_Request : t_NullCmd
    {
      WORD wdServerType;
      char pstrIP[MAX_IP_LENGTH];
      t_Startup_Request()
        : t_NullCmd(CMD_STARTUP,PARA_STARTUP_REQUEST) {};
    };

    const BYTE PARA_STARTUP_RESPONSE = 2;
    struct t_Startup_Response : t_NullCmd
    {
      WORD wdServerID;
      char pstrIP[MAX_IP_LENGTH];
      WORD wdPort;
      t_Startup_Response()
        : t_NullCmd(CMD_STARTUP,PARA_STARTUP_RESPONSE) {};
    };

    struct ServerEntry
    {


      WORD wdServerID;




      WORD wdServerType;
      char pstrIP[MAX_IP_LENGTH];
      WORD wdPort;
      WORD state;
      ServerEntry()
      {
        wdServerID = 0;
        wdServerType = 0;
        bzero(pstrIP,sizeof(pstrIP));
        wdPort = 0;
        state = 0;
      }
      ServerEntry(const ServerEntry& se)
      {
        wdServerID = se.wdServerID;
        wdServerType = se.wdServerType;
        bcopy(se.pstrIP, pstrIP, sizeof(pstrIP));
        wdPort = se.wdPort;
        state = se.state;
      }
      ServerEntry & operator= (const ServerEntry &se)
      {
        wdServerID = se.wdServerID;
        wdServerType = se.wdServerType;
        bcopy(se.pstrIP, pstrIP, sizeof(pstrIP));
        wdPort = se.wdPort;
        state = se.state;
        return *this;
      }

	  
	   operator int() const 
	  {
		return (int)wdServerID;
      }


    };
    const BYTE PARA_STARTUP_SERVERENTRY_NOTIFYME = 3;
    struct t_Startup_ServerEntry_NotifyMe : t_NullCmd
    {
      WORD size;
      ServerEntry entry[1];
      t_Startup_ServerEntry_NotifyMe()
        : t_NullCmd(CMD_STARTUP,PARA_STARTUP_SERVERENTRY_NOTIFYME),size(0) {};
    };
    const BYTE PARA_STARTUP_SERVERENTRY_NOTIFYOTHER = 4;
    struct t_Startup_ServerEntry_NotifyOther : t_NullCmd
    {
      WORD srcID;
      ServerEntry entry;
      t_Startup_ServerEntry_NotifyOther()
        : t_NullCmd(CMD_STARTUP,PARA_STARTUP_SERVERENTRY_NOTIFYOTHER) {};
    };

    const BYTE PARA_STARTUP_OK = 5;
    struct t_Startup_OK : t_NullCmd
    {
      WORD wdServerID;
      t_Startup_OK()
        : t_NullCmd(CMD_STARTUP,PARA_STARTUP_OK) {};
    };

    const BYTE PARA_GAMETIME = 6;
    struct t_GameTime : t_NullCmd
    {
      QWORD qwGameTime;
      t_GameTime()
        : t_NullCmd(CMD_STARTUP,PARA_GAMETIME) {};
    };

    const BYTE PARA_RESTART_SERVERENTRY_NOTIFYOTHER = 9;
    struct t_restart_ServerEntry_NotifyOther : t_NullCmd
    {
      WORD srcID;
      WORD dstID;
      t_restart_ServerEntry_NotifyOther()
        : t_NullCmd(CMD_STARTUP,PARA_RESTART_SERVERENTRY_NOTIFYOTHER) {};
    };
    //////////////////////////////////////////////////////////////
    // 定义启动相关指令
    //////////////////////////////////////////////////////////////


    //////////////////////////////////////////////////////////////
    // 定义管理服务器与计费服务器交互的指令
    //////////////////////////////////////////////////////////////
    const BYTE PARA_BILL_NEWSESSION = 1;
    struct t_NewSession_Bill : t_NullCmd
    {
      t_NewLoginSession session;

      t_NewSession_Bill()
        : t_NullCmd(CMD_BILL,PARA_BILL_NEWSESSION) {};
    };

    const BYTE PARA_BILL_IDINUSE = 2;
    struct t_idinuse_Bill : t_NullCmd
    {
      DWORD accid;
      DWORD loginTempID;
      WORD wdLoginID;
      char name[48];

      t_idinuse_Bill()
        : t_NullCmd(CMD_BILL,PARA_BILL_IDINUSE) { bzero(name,sizeof(name)); };
    };
    //////////////////////////////////////////////////////////////
    // 定义管理服务器与计费服务器交互的指令
    //////////////////////////////////////////////////////////////


    //////////////////////////////////////////////////////////////
    // 定义管理服务器与网关服务器交互的指令
    //////////////////////////////////////////////////////////////
    const BYTE PARA_GATEWAY_GYLIST = 1;
    struct t_GYList_Gateway : t_NullCmd
    {
      WORD wdServerID;      /**< 服务器编号 */
      char pstrIP[MAX_IP_LENGTH];  /**< 服务器地址 */
      WORD wdPort;        /**< 服务器端口 */
      WORD wdNumOnline;      /**< 网关在线人数 */
      int  state;          /**< 服务器状态 */
      DWORD zoneGameVersion;
      t_GYList_Gateway()
        : t_NullCmd(CMD_GATEWAY,PARA_GATEWAY_GYLIST) {};
    };

    const BYTE PARA_GATEWAY_RQGYLIST = 2;
    struct t_RQGYList_Gateway : t_NullCmd
    {
      t_RQGYList_Gateway()
        : t_NullCmd(CMD_GATEWAY,PARA_GATEWAY_RQGYLIST) {};
    };

    const BYTE PARA_GATEWAY_NEWSESSION = 3;
    struct t_NewSession_Gateway : t_NullCmd
    {
      t_NewLoginSession session;

      t_NewSession_Gateway()
        : t_NullCmd(CMD_GATEWAY,PARA_GATEWAY_NEWSESSION) {};
    };

    const WORD ROLEREG_STATE_TEST    = 1;  //测试
    const WORD ROLEREG_STATE_WRITE    = 2;  //回写
    const WORD ROLEREG_STATE_CLEAN    = 4;  //清除
    const WORD ROLEREG_STATE_HAS    = 8;  //测试有
    const WORD ROLEREG_STATE_OK      = 16;  //清除或回写成功

    const BYTE PARA_CHARNAME_GATEWAY = 4;
    struct t_Charname_Gateway : t_NullCmd
    {
	WORD regType;	    //ע������(���� ע���ɫ�����塢���)
      WORD wdServerID;      /**< 服务器编号 */
      DWORD accid;        /**< 账号编号 */
      DWORD charid;
      char name[MAX_NAMESIZE];  /**< 角色名称 */
      WORD state;          /**< 上面各种状态的位组合 */

      t_Charname_Gateway()
        :t_NullCmd(CMD_GATEWAY,PARA_CHARNAME_GATEWAY) 
      {
	  regType = 1;
	  wdServerID = 0;
	  accid = 0;
	  charid = 0;
	  state = 0;
	  bzero(name, sizeof(name));
      }
    };

	const BYTE PARA_NOTIFYGATE_FINISH = 5;
	struct t_notifyFinish_Gateway : t_NullCmd
	{
      t_notifyFinish_Gateway()
        :t_NullCmd(CMD_GATEWAY,PARA_NOTIFYGATE_FINISH) { }
	};

	const BYTE PARA_CHANGE_ZONE_DEL = 6;
	struct t_ChangeZoneDel_Gateway : t_NullCmd
      {
	  DWORD accid;
	  DWORD userid;
	  char name[MAX_NAMESIZE];
	  t_ChangeZoneDel_Gateway()
	      :t_NullCmd(CMD_GATEWAY, PARA_CHANGE_ZONE_DEL)
	  {
	      accid = 0;
	      userid = 0;
	      bzero(name, sizeof(name));
	  }
      };
    //////////////////////////////////////////////////////////////
    // 定义管理服务器与网关服务器交互的指令
    //////////////////////////////////////////////////////////////

    //////////////////////////////////////
    ///国家在线人数相关指令
    //////////////////////////////////////
    //请求国家在线人数信息
    const BYTE PARA_REQUEST_COUNTRYONLINE = 0;
    struct t_Request_CountryOnline : t_NullCmd
    {
      QWORD     rTimestamp;              //请求时间戳
      DWORD    infoTempID;
      t_Request_CountryOnline()
        : t_NullCmd(CMD_COUNTRYONLINE,PARA_REQUEST_COUNTRYONLINE) {};
    };
    //国家在线人数信息
    const BYTE PARA_COUNTRYONLINE = 1;
    struct t_CountryOnline : t_NullCmd
    {
      QWORD     rTimestamp;              //请求时间戳
      DWORD    infoTempID;
      DWORD    OnlineNum;
      struct Online
      {
        DWORD country;
        DWORD num;
      }
      CountryOnline[0];

      t_CountryOnline() : t_NullCmd(CMD_COUNTRYONLINE,PARA_COUNTRYONLINE)
      {
        OnlineNum = 0;
      }
    };
    //////////////////////////////////////
    ///国家在线人数相关指令
    //////////////////////////////////////
    //请求国家在线人数信息
    
    const BYTE PARA_SHUTDOWN =1;
    struct t_shutdown_Super : t_NullCmd
    {
      t_shutdown_Super(): t_NullCmd(CMD_SESSION,PARA_SHUTDOWN)
      {
      }
    };

    const BYTE PARA_USER_ONLINE_BROADCAST = 8;
    struct stUserOnlineBroadCast : t_NullCmd
    {
	char name[MAX_NAMESIZE+1];
	DWORD dwZoneID;
	BYTE dwNum;
	stUserOnlineBroadCast() : t_NullCmd(CMD_SESSION, PARA_USER_ONLINE_BROADCAST)
	{
	    bzero(name, sizeof(name));
	    dwZoneID = 0;
	    dwNum = 0;
	}
    };


    const BYTE PARA_STOPCHANGE_SCENE = 1;
    struct t_StopChange_Scene : t_NullCmd
    {
	t_StopChange_Scene() : t_NullCmd(CMD_SCENE, PARA_STOPCHANGE_SCENE)
	{}
    };

    const BYTE PARA_FORWARD_MSG_SCENE = 2;
    struct t_ForwardMsg_Scene : t_NullCmd
    {
	GameZone_t toGameZone;
	DWORD size;
	BYTE msg[0];
	t_ForwardMsg_Scene() : t_NullCmd(CMD_SCENE, PARA_FORWARD_MSG_SCENE)
	{
	    size = 0;
	}
    };
  };

};

#pragma pack()
#endif
