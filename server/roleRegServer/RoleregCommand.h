/**
 * \brief 定义统一用户平台登陆服务器指令
 */
#ifndef _RoleRegCommand_h_
#define _RoleRegCommand_h_
#include "zType.h"
#include "zNullCmd.h"
#pragma pack(1)

namespace Cmd
{
  namespace RoleReg
  {
    const BYTE CMD_LOGIN = 1;
    const BYTE CMD_ROLEREG = 2;
    const BYTE CMD_REG_WITHID = 3;


    //////////////////////////////////////////////////////////////
    /// 登陆RoleReg服务器指令
    //////////////////////////////////////////////////////////////
    const BYTE PARA_LOGIN = 1;
    struct t_LoginRoleReg : t_NullCmd
    {
      char strIP[MAX_IP_LENGTH];
      WORD port;

      t_LoginRoleReg()
        : t_NullCmd(CMD_LOGIN,PARA_LOGIN) {};
    };

    const BYTE PARA_LOGIN_OK = 2;
    struct t_LoginRoleReg_OK : t_NullCmd
    {
      GameZone_t gameZone;
      char name[MAX_NAMESIZE];
      BYTE netType;

      t_LoginRoleReg_OK()
        : t_NullCmd(CMD_LOGIN,PARA_LOGIN_OK) {};
    };
    //////////////////////////////////////////////////////////////
    /// 登陆RoleReg服务器指令
    //////////////////////////////////////////////////////////////

    
    const WORD ROLEREG_STATE_TEST    = 1;  //测试
    const WORD ROLEREG_STATE_WRITE    = 2;  //回写
    const WORD ROLEREG_STATE_CLEAN    = 4;  //清除
    const WORD ROLEREG_STATE_HAS    = 8;  //测试有
    const WORD ROLEREG_STATE_OK      = 16;  //清除或回写成功

    const BYTE PARA_CHARNAME_REG_WITHID = 1;
    struct t_Charname_reg_withID : t_NullCmd
    {
	WORD regType;	    //ע������(���� ע���ɫ�����塢���)
      WORD wdServerID;      /**< 服务器编号 */
      DWORD accid;        /**< 账号编号 */
      GameZone_t gameZone;
      char name[MAX_NAMESIZE];  /**< 角色名称 */
      union
      {
	  DWORD charid;
	  DWORD familyid;
	  DWORD clubid;
      };
      WORD state;          /**< 上面各种状态的位组合 */

      t_Charname_reg_withID()
        :t_NullCmd(CMD_REG_WITHID, PARA_CHARNAME_REG_WITHID) 
      {
	  regType = 0;
	  charid = 0;
	  bzero(name, sizeof(name));
      }
    };

    enum
    {
	ROLE_REG_WITHID = 1,
	FAMILY_REG_WITHID = 2,
	CLUB_REG_WITHID = 3,
    };
  };
};

#pragma pack()
#endif

