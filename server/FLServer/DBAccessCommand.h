#ifndef _DBAccessCommand_h_
#define _DBAccessCommand_h_
#include "zType.h"
#include "zNullCmd.h"

#pragma pack(1)

namespace Cmd
{
  namespace DBAccess
  {
    const BYTE CMD_LOGON = 1;
    const BYTE CMD_LOGINSERVER = 2;


    //////////////////////////////////////////////////////////////
    // 定义登陆数据库访问中间件指令
    //////////////////////////////////////////////////////////////
    const BYTE PARA_LOGON = 1;
    struct t_LogonDBAccess : t_NullCmd
    {
      t_LogonDBAccess()
        : t_NullCmd(CMD_LOGON,PARA_LOGON) {};
    };
    //////////////////////////////////////////////////////////////
    // 定义登陆数据库访问中间件指令
    //////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////
    // 定义数据库访问中间件与登陆服务器之间的指令
    //////////////////////////////////////////////////////////////
    enum
    {
      SESSIONCHECK_SUCCESS = 0,
      SESSIONCHECK_DB_FAILURE = 1,
      SESSIONCHECK_PWD_FAILURE = 2
    };
    const BYTE PARA_LOGINSERVER_SESSIONCHECK = 1;
    struct t_LoginServer_SessionCheck : t_NullCmd
    {
      t_LoginServer_SessionCheck()
        : t_NullCmd(CMD_LOGINSERVER,PARA_LOGINSERVER_SESSIONCHECK)
        {
          retcode = SESSIONCHECK_DB_FAILURE;
        }

      BYTE retcode;
      t_NewLoginSession session;
    };

    const BYTE PARA_CHANGESERVER_SESSIONCHECK = 2;
    struct t_ChangeServer_SessionCheck : t_NullCmd
    {
      t_ChangeServer_SessionCheck()
        : t_NullCmd(CMD_LOGINSERVER,PARA_CHANGESERVER_SESSIONCHECK)
        {
          retcode = SESSIONCHECK_DB_FAILURE;
        }

      BYTE retcode;
      t_NewLoginSession session;
    };
    //////////////////////////////////////////////////////////////
    // 定义数据库访问中间件与登陆服务器之间的指令
    //////////////////////////////////////////////////////////////
  };

};

#pragma pack()
#endif
