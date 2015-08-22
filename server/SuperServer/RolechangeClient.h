#ifndef __RolechangeClient_H_
#define __RolechangeClient_H_

#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "NetType.h"



/**
 * \brief 统一用户平台登陆服务器的客户端连接类
 */
class RolechangeClient : public zTCPClientTask
{

  public:

    RolechangeClient(const std::string &ip, const WORD port);
    ~RolechangeClient();

    int checkRebound();
    void addToContainer();
    void removeFromContainer();
    bool connect();
    bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
    bool msgParse_Forward(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen, DWORD fromGameID);
    const NetType getNetType() const
    {
      return netType;
    }
    
    const DWORD getTempID() const
    {
	return tempid;
    }

  private:

    const DWORD tempid;
    static DWORD tempidAllocator;
    NetType netType;

};

#endif

